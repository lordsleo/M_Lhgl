//
//文件名：    GetAllTallyBill.aspx.cs
//功能描述：  获取派工编码和委托编码下所有生成的理货作业票（保存、提交）
//创建时间：  2015/10/01
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

namespace M_Lhgl.Service.Slip
{
    public partial class GetAllTallyBill : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //派工编码
            var pmno = Request.Params["Pmno"];
            //委托编码
            var cgno = Request.Params["Cgno"];

            //pmno = "20151212000565";
            //cgno = "8f18014480034dbdb3f5c234ccd35b1f";

            try
            {
                if (pmno == null || cgno == null)
                {
                    string warning = string.Format("参数Pmno，Cgno不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Slip/GetAllTallyBill.aspx?Pmno=20151212000565&Cgno=8f18014480034dbdb3f5c234ccd35b1f");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;

                }
                
                string strSql =
                    string.Format(@"select a.tbno,a.gbno,a.gbdisplay,a.gbnolast,a.gbdisplaylast,a.taskno,a.weight,a.amount,a.amount2,b.code_carrier_s,b.code_carrier_e,a.carrier1,a.carrier1num,a.carrier2,a.carrier2num,a.mark_finish 
                                    from VW_REG_TALLYBILL a, vw_ps_mission b 
                                    where a.pmno=b.pmno and a.pmno='{0}' and a.cgno='{1}' order by a.mark_finish asc, a.modifytime desc", pmno, cgno);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "暂无数据！").DicInfo());
                    return;
                }

                string[,] strArray = new string[dt.Rows.Count, 8];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    string strCount = string.Empty;
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[iRow]["amount"])) && Convert.ToString(dt.Rows[iRow]["amount"]) != "0")
                    {
                        strCount += string.Format("{0}件", Convert.ToString(dt.Rows[iRow]["amount"])) + "/"; ;
                    }
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[iRow]["amount2"])) && Convert.ToString(dt.Rows[iRow]["amount2"]) != "0")
                    {
                        strCount += string.Format("{0}件2", Convert.ToString(dt.Rows[iRow]["amount2"])) + "/"; ;
                    }
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[iRow]["weight"])) && Convert.ToString(dt.Rows[iRow]["weight"]) != "0")
                    {
                        strCount += string.Format("{0}吨", Convert.ToString(dt.Rows[iRow]["weight"])) + "/"; ;
                    }
                    if (!string.IsNullOrWhiteSpace(strCount))
                    {
                        strCount = strCount.Remove(strCount.Length - 1, 1);
                    }

                    string strCarrier1 = string.Empty;
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[iRow]["carrier1"])))
                    {
                        strCarrier1 += string.Format("{0}", Convert.ToString(dt.Rows[iRow]["carrier1"])) + "/"; ;
                    }
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[iRow]["carrier1num"])))
                    {
                        strCarrier1 += string.Format("{0}", Convert.ToString(dt.Rows[iRow]["carrier1num"])) + "/"; ;
                    }
                    if (!string.IsNullOrWhiteSpace(strCarrier1))
                    {
                        strCarrier1 = strCarrier1.Remove(strCarrier1.Length - 1, 1);
                    }

                    string strCarrier2 = string.Empty;
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[iRow]["carrier2"])))
                    {
                        strCarrier2 += string.Format("{0}", Convert.ToString(dt.Rows[iRow]["carrier2"])) + "/"; ;
                    }
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[iRow]["carrier2num"])))
                    {
                        strCarrier2 += string.Format("{0}", Convert.ToString(dt.Rows[iRow]["carrier2num"])) + "/"; ;
                    }
                    if (!string.IsNullOrWhiteSpace(strCarrier2))
                    {
                        strCarrier2 = strCarrier2.Remove(strCarrier2.Length - 1, 1);
                    }

                    string strGbDisplayLast = string.Empty;
                    string strMarkLast = string.Empty;
                    strSql =
                        string.Format("select mark_last from TB_HC_CONSIGN_GBNO where cgno='{0}' and gbno='{1}'", cgno, Convert.ToString(dt.Rows[iRow]["gbnolast"]));
                    var dt1 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                    if (dt1.Rows.Count > 0)
                    {
                        strMarkLast = Convert.ToString(dt1.Rows[0]["mark_last"]);
                    }
                    if (strMarkLast == "1")
                    {
                        strGbDisplayLast = Convert.ToString(dt.Rows[iRow]["gbdisplaylast"]);
                    }
                    
                    strArray[iRow, 0] = Convert.ToString(dt.Rows[iRow]["tbno"]);
                    strArray[iRow, 1] = Convert.ToString(dt.Rows[iRow]["gbdisplay"]);
                    strArray[iRow, 2] = strGbDisplayLast;
                    strArray[iRow, 3] = strCarrier1;
                    strArray[iRow, 4] = strCarrier2;
                    strArray[iRow, 5] = Convert.ToString(dt.Rows[iRow]["taskno"]);
                    strArray[iRow, 6] = strCount;
                    strArray[iRow, 7] = Convert.ToString(dt.Rows[iRow]["mark_finish"]);
                }

                Json = JsonConvert.SerializeObject(new DicPackage(true, strArray, null).DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}