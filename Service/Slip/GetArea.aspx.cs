//
//文件名：    GetArea.aspx.cs
//功能描述：  获取区域数据
//创建时间：  2015/11/07
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
    public partial class GetArea : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //派工编码
            var pmno = Request.Params["Pmno"];
            //载体编码
            var codeCarries = Request.Params["CodeCarries"];

            //pmno = "20151021000241";
            //codeCarries = "02";

            try
            {
                if (pmno == null || codeCarries == null)
                {
                    string warning = string.Format("参数Pmno，CodeCarries不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Slip/GetArea.aspx?Pmno=20151021000241&CodeCarries=02");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;
                }

                if (codeCarries == "02" || codeCarries == "05")
                {
                    Json = GetShipArea(pmno);
                }
                else if (codeCarries == "01")
                {
                    Json = GetStorageArea(pmno);
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
        /// <param name="pmno">派工编码</param>
        /// <returns>场地区域数据</returns>
        private string GetStorageArea(string pmno)
        {
            string strJson = string.Empty;
            //场地编码
            string strCodeStorage = string.Empty;
            //获取场地编码
            var dt = GetDTForVwPsMission(pmno);
            if (dt.Rows.Count > 0)
            {
                strCodeStorage = Convert.ToString(dt.Rows[0]["code_storage"]);
            }

            string[,] strArray = null;
            string strSql =
                string.Format("select a.code_workingarea,b.workingarea from tb_code_storage a, tb_code_workingarea b where a.code_workingarea=b.code_workingarea and a.code_storage='{0}'", strCodeStorage);
            dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCBase).ExecuteTable(strSql);
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
        /// <returns>船舶区域数据</returns>
        private string GetShipArea(string pmno)
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
                string.Format("select code_workingarea,workingarea from VW_HS_NSNO_WORKINGAREA where nsno='{0}'", strNsno);
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
                                    code_nvessel,carrier1,carrier2,nsno,tallydate,code_opstype,code_workflow,code_worktime,code_teamtype,code_trainroad,code_motorcade,begintime,endtime,nsno_barge,nsno_bargelast          
                                    from vw_ps_mission where pmno='{0}'",
                                    pmno);
            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);;
            return dt;
        }
        #endregion
    }
}