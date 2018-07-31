//
//文件名：    GetSavedSourceArea.aspx.cs
//功能描述：  获取暂存源区域
//创建时间：  2015/09/18
//作者：      
//修改时间：  暂无
//修改描述：  暂无
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Leo;
using ServiceInterface.Common;
using System.Data;

namespace M_Lhgl.Service.Slip
{
    public partial class GetSavedArea : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //派工编码
            var pmno = Request.Params["Pmno"];
            //理货单编码
            var tbno = Request.Params["Tbno"];
            //区域类型
            var areaType = Request.Params["AreaType"];

            pmno = "20151021000241";

            try
            {
                if (pmno == null || tbno == null || areaType == null)
                {
                    string warning = string.Format("参数Pmno,Tbno，AreaType不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Slip/GetSavedArea.aspx?Pmno=20151009000041&Tbno=20151020063793&AreaType=0");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;

                }
               
                //获取源载体编码
                string strSql =
                        string.Format(@"select CODE_CARRIER_S,CODE_CARRIER_E          
                                        from vw_ps_mission where pmno='{0}'",
                                        pmno);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "派工编码不存在！").DicInfo());
                    return;
                }
                string strCodeCarrier = areaType == "0" ? Convert.ToString(dt.Rows[0]["CODE_CARRIER_S"]) : Convert.ToString(dt.Rows[0]["CODE_CARRIER_E"]);

                //获取源区域编码
                strSql =
                    string.Format(@"select code_workingarea,code_workingarealast
                                    from VW_REG_TALLYBILL 
                                    where tbno='{0}'",
                                    tbno);
                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "理货单编码不存在！").DicInfo());
                    return;
                }
                string strCodeWorkingArea = areaType == "0" ? Convert.ToString(dt.Rows[0]["code_workingarea"]) : Convert.ToString(dt.Rows[0]["code_workingarealast"]);

                if (strCodeCarrier == "02" || strCodeCarrier == "05")
                {
                    Json = GetShipArea(pmno, strCodeWorkingArea);
                }
                else if (strCodeCarrier == "01")
                {
                    Json = GetStorageArea(strCodeWorkingArea);
                }
                else
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(true, null, null).DicInfo());
                } 
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;

        #region 获取场地区域数据
        /// <summary>
        /// 获取场地区域数据
        /// </summary>
        /// <param name="strCodeWorkingArea">区域编码</param>
        /// <returns>场地区域数据</returns>
        private string GetStorageArea(string strCodeWorkingArea)
        {
            string strJson = string.Empty;

            string[,] strArray = null;
            string strSql =
                string.Format("select code_workingarea, workingarea from tb_code_workingarea where code_workingarea='{0}'", strCodeWorkingArea);
            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCBase).ExecuteTable(strSql);
            if (dt.Rows.Count > 0)
            {
                strArray = new string[dt.Rows.Count, 2];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    strArray[iRow, 0] = Convert.ToString(dt.Rows[iRow]["code_workingarea"]);
                    strArray[iRow, 1] = Convert.ToString(dt.Rows[iRow]["workingarea"]);
                }
            }

            strJson = JsonConvert.SerializeObject(new DicPackage(true, strArray, null).DicInfo());
            return strJson;
        }
        #endregion

        #region 获取船舶（海伦、驳船）区域数据
        /// <summary>
        /// 获取船舶（海伦、驳船）区域数据
        /// </summary>
        /// <param name="pmno">派工编码</param>
        /// <param name="strCodeWorkingArea">区域编码</param>
        /// <returns>船舶区域数据</returns>
        private string GetShipArea(string pmno, string strCodeWorkingArea)
        {
            string strJson = string.Empty;
            //船舶ID
            string strNsno = string.Empty;
            //获取船舶ID
            var dt = GetDTForVwPsMission(pmno);
            if (dt.Rows.Count > 0)
            {
                strNsno = Convert.ToString(dt.Rows[0]["nsno"]);
            }

            string[,] strArray = null;
            string strSql =
                string.Format("select code_workingarea,workingarea from VW_HS_NSNO_WORKINGAREA where nsno='{0}' and code_workingarea='{1}'", strNsno, strCodeWorkingArea);
            dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
            if (dt.Rows.Count > 0)
            {
                strArray = new string[dt.Rows.Count, 2];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    strArray[iRow, 0] = Convert.ToString(dt.Rows[iRow]["code_workingarea"]);
                    strArray[iRow, 1] = Convert.ToString(dt.Rows[iRow]["workingarea"]);
                }
            }

            strJson = JsonConvert.SerializeObject(new DicPackage(true, strArray, null).DicInfo());
            return strJson;
        }
        #endregion

        #region 获取调度执行表数据
        /// <summary>
        /// 获取调度执行表数据
        /// </summary>
        /// <param name="strCgno">委托编码</param>
        /// <returns>DataTable</returns>
        private DataTable GetDTForVwPsMission(string pmno)
        {
            string strSql =
                    string.Format(@"select code_storage,code_storagelast,mark_tallybill,code_operation,code_operation_fact,CODE_CARRIER_S,CODE_CARRIER_E,vgdisplay,vgno,cabin,code_carrier,nvessel,
                                    code_nvessel,carrier1,nsno,tallydate,code_opstype,code_workflow,code_worktime,code_teamtype,code_trainroad,code_motorcade,begintime,endtime,nsno_barge,nsno_bargelast          
                                    from vw_ps_mission where pmno='{0}'",
                                    pmno);
            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql); ;
            return dt;
        }
        #endregion
    }
}