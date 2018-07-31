//
//文件名：    GetAllCarries.aspx.cs
//功能描述：  获取所有载体数据
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
    public partial class GetAllCarries : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //派工编码
            var pmno = Request.Params["Pmno"];
            //载体类型
            var carriesType = Request.Params["CarriesType"];

            try
            {
                if (pmno == null || carriesType == null)
                {
                    string warning = string.Format("参数Pmno，CarriesType不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Slip/GetAllCarries.aspx?Pmno=20151212000565&CarriesType=01");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;
                }

                string strSql =
                        string.Format(@"select code_storage,code_storagelast,mark_tallybill,code_operation,code_operation_fact,CODE_CARRIER_S,CODE_CARRIER_E,vgdisplay,vgno,cabin,vgdisplaylast,vgnolast,cabinlast,code_carrier,code_carrierlast,nvessel,
                                        code_nvessel,nvessellast,code_nvessellast,carrier1,carrier2,nsno,tallydate,code_opstype,code_workflow,code_worktime,code_teamtype,code_trainroad,code_motorcade,begintime,endtime,nsno_barge,nsno_bargelast          
                                        from vw_ps_mission where pmno='{0}'",
                                        pmno);
                var dt0 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt0.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "派工编码不存在！").DicInfo());
                    return;
                }

                //获取场地、其他数据
                DataTable dtStorage = null;
                //载体编码类型
                string strCodeCarrierType = string.Empty;
                //场地编码
                string strCodeStorage = string.Empty;
                //场地
                string strStorage = string.Empty;
                //航次
                string strVgDisplay = string.Empty;
                //航次编码
                string strVgno = string.Empty;
                //仓别
                string strCabin = string.Empty;
                //车型编码
                string strCodeCarrier = string.Empty;
                //车型
                string strCarrier = string.Empty;
                //驳船
                string strNvessel = string.Empty;
                //驳船编码
                string strCodeNvessel = string.Empty;
                //作业过程分解类型编码
                string strCodeOpstype = string.Empty;

                if (!string.IsNullOrWhiteSpace(Convert.ToString(dt0.Rows[0]["code_operation"])) || !string.IsNullOrWhiteSpace(Convert.ToString(dt0.Rows[0]["code_operation"])))
                {
                    strSql =
                        string.Format("select code_storage code,storage name from BASERESOURCE.tb_code_storage");
                    var dt1 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCBase).ExecuteTable(strSql);
                    dtStorage = dt1;
                }
                // '01'表示内部库场
                if ((Convert.ToString(dt0.Rows[0]["CODE_CARRIER_S"]) == "01") || (Convert.ToString(dt0.Rows[0]["CODE_CARRIER_E"]) == "01"))
                {
                    dtStorage = new DataTable("STORAGE");
                    dtStorage.Columns.Add("CODE");
                    dtStorage.Columns.Add("NAME");
                    dtStorage.AcceptChanges();
                }

                strCodeCarrierType = carriesType == "0" ? Convert.ToString(dt0.Rows[0]["CODE_CARRIER_S"]) : Convert.ToString(dt0.Rows[0]["CODE_CARRIER_E"]);
                Dictionary<string, object> info = new Dictionary<string, object>();
                if (strCodeCarrierType == "02")//船
                {
                    strCodeStorage = string.Empty;
                    strStorage = string.Empty;
                    strVgDisplay = carriesType == "0" ? Convert.ToString(dt0.Rows[0]["vgdisplay"]) : Convert.ToString(dt0.Rows[0]["vgdisplaylast"]);
                    strVgno = carriesType == "0" ? Convert.ToString(dt0.Rows[0]["vgno"]) : Convert.ToString(dt0.Rows[0]["vgnolast"]);
                    strCabin = carriesType == "0" ? Convert.ToString(dt0.Rows[0]["cabin"]) : Convert.ToString(dt0.Rows[0]["cabinlast"]);
                }
                else if (strCodeCarrierType == "03" || strCodeCarrierType == "04" || strCodeCarrierType == "06")//车、汽、集装箱
                {
                    strCodeStorage = string.Empty;
                    strStorage = string.Empty;
                    strCodeCarrier = carriesType == "0" ? Convert.ToString(dt0.Rows[0]["code_carrier"]) : Convert.ToString(dt0.Rows[0]["code_carrierlast"]);
                    strSql = string.Format("select carrier from tb_code_carrier where code_carrier='{0}'", strCodeCarrier);
                    var dt2 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCBase).ExecuteTable(strSql);
                    if (dt2.Rows.Count > 0)
                    {
                        strCarrier = Convert.ToString(dt2.Rows[0]["carrier"]);
                    }
                }
                else if (strCodeCarrierType == "05")//驳船
                {
                    strCodeStorage = string.Empty;
                    strStorage = string.Empty;

                    strNvessel = carriesType == "0" ? Convert.ToString(dt0.Rows[0]["nvessel"]) : Convert.ToString(dt0.Rows[0]["nvessellast"]);
                    strCodeNvessel = carriesType == "0" ? Convert.ToString(dt0.Rows[0]["code_nvessel"]) : Convert.ToString(dt0.Rows[0]["code_nvessellast"]);
                }
                else //场地、其他
                {
                    if (dtStorage.Select("code='" + Convert.ToString(dt0.Rows[0]["code_storage"]) + "'").Length == 0)
                    {
                        if (carriesType == "0")
                        {
                            //商务计划中不存在派工中的场地
                            strCodeStorage = Convert.ToString(dt0.Rows[0]["code_storage"]);
                            strStorage = Convert.ToString(dt0.Rows[0]["carrier1"]);
                        }
                        else
                        {
                            //商务计划中不存在派工中的场地
                            strCodeStorage = Convert.ToString(dt0.Rows[0]["code_storagelast"]);
                            strStorage = Convert.ToString(dt0.Rows[0]["carrier2"]);
                        }
                    }
                    else
                    {
                        if (carriesType == "0")
                        {
                            strCodeStorage = Convert.ToString(dt0.Rows[0]["code_storage"]);
                            strStorage = Convert.ToString(dtStorage.Select("code='" + Convert.ToString(dt0.Rows[0]["code_storage"]) + "'")[0]["name"]);
                        }
                        else
                        {
                            strCodeStorage = Convert.ToString(dt0.Rows[0]["code_storagelast"]);
                            strStorage = Convert.ToString(dtStorage.Select("code='" + Convert.ToString(dt0.Rows[0]["code_storagelast"]) + "'")[0]["name"]);
                        }
                    }
                    strCodeOpstype = Convert.ToString(dt0.Rows[0]["code_opstype"]);
                }

                info.Add("VgDisplay", strVgDisplay);
                info.Add("Vgno", strVgno);
                info.Add("Cabin", strCabin);
                info.Add("CodeCarrier", strCodeCarrier);
                info.Add("Nvessel", strNvessel);
                info.Add("CodeNvessel", strCodeNvessel);
                info.Add("Storage", strStorage);
                info.Add("CodeStorage", strCodeStorage);
                info.Add("CodeOpstype", strCodeOpstype);
                info.Add("Carrier", strCarrier);

                Json = JsonConvert.SerializeObject(new DicPackage(true, info, null).DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;

//        #region 获取调度执行表数据
//        /// <summary>
//        /// 获取调度执行表数据
//        /// </summary>
//        /// <param name="strCgno">委托编码</param>
//        /// <returns>DataTable</returns>
//        private DataTable GetDTForVwPsMission(string pmno)
//        {
//            string strSql =
//                    string.Format(@"select code_storage,code_storagelast,mark_tallybill,code_operation,code_operation_fact,CODE_CARRIER_S,CODE_CARRIER_E,vgdisplay,vgno,cabin,vgdisplaylast,vgnolast,cabinlast,code_carrier,code_carrierlast,nvessel,
//                                    code_nvessel,nvessellast,code_nvessellast,carrier1,carrier2,nsno,tallydate,code_opstype,code_workflow,code_worktime,code_teamtype,code_trainroad,code_motorcade,begintime,endtime,nsno_barge,nsno_bargelast          
//                                    from vw_ps_mission where pmno='{0}'",
//                                    pmno);
//            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql); ;
//            return dt;
//        }
//        #endregion
    }
}