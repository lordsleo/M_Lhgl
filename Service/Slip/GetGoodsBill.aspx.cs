//
//文件名：    GetGoodsBill.aspx.cs
//功能描述：  获取票货数据
//创建时间：  2015/10/02
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
using M_Lhgl.Service.Slip;
using System.Data;

namespace M_Lhgl.Service.Slip
{
    public partial class GetGoodsBill : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //派工编码
            var pmno = Request.Params["Pmno"];
            //委托编码
            var cgno = Request.Params["Cgno"];
            //作业子过程编码
            var codeOperationFact = Request.Params["CodeOperationFact"];

            try
            {
                //
                // 非调发 ?Pmno=20151010000161&Cgno=d1bff20fa2d54a0b87e4385a5cb46914  
                //        ?Pmno=20151021000241&Cgno=c14ba9abd9694d9b8e40250c5891b360 
                //   调发 ?Pmno=20151009000121&Cgno=b02fb038209f4008bedfba6f27966efe 
                //

                if (pmno == null || cgno == null || codeOperationFact == null)
                {
                    string warning = string.Format("参数Pmno，Cgno，CodeOperation，CodeOperationFact不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Slip/GetGoodsBill.aspx?Pmno=20151212000565&Cgno=8f18014480034dbdb3f5c234ccd35b1f&CodeOperationFact=24");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;
                }

                //获取基础数据
                Json = GetBaseData(pmno, cgno);
                if (Json != string.Empty)
                {
                    return;
                }

                goodBillE.StrGoodsBill1Name = "销账票货";
                goodBillE.StrGoodsBill2Name = "商务销账";

                //针对子作业过程改变，特殊标志改变 
                if ((!string.IsNullOrWhiteSpace(codeOperationFact)) && (!string.IsNullOrWhiteSpace(cgno)))//变动
                {
                    if (goodBillE.StrMarkExchange == "0")//非调发
                    {
                        Json = GetGoodsBillForNoAdjust1(goodBillE.StrCodeOperation, codeOperationFact, cgno);
                    }
                    else//调发
                    {
                        Json = GetGoodsBillForAdjust1(goodBillE.StrCodeOperation, codeOperationFact, cgno);
                    } 
                }
                else//初始化时
                {
                    if (goodBillE.StrMarkExchange == "0")//非调发
                    {
                        Json = GetGoodsBillForNoAdjust(cgno);
                    }
                    else//调发
                    {
                        Json = GetGoodsBillForAdjust(cgno);
                    }  
                }                     
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
        GoodsBillE goodBillE = new GoodsBillE();

        /// <summary>
        /// 获取基础数据(源场地编码、目的场地编码、理货单生成标志、调发货标志)
        /// </summary>
        /// <param name="strPmno">票货编码</param>
        /// <param name="strCgno">委托编码</param>
        private string GetBaseData(string strPmno, string strCgno)
        {
            string strJson = string.Empty;
            Dictionary<string, object> info = new Dictionary<string, object>();
            string strSql =
                    string.Format(@"select code_storage,code_storagelast,mark_tallybill,code_operation,code_operation_fact,CODE_CARRIER_S,CODE_CARRIER_E,vgdisplay,vgno,cabin,code_carrier,nvessel,
                                    code_nvessel,carrier1,nsno,tallydate,code_opstype,code_workflow,code_worktime,code_teamtype,code_trainroad,code_motorcade,begintime,endtime,nsno_barge,nsno_bargelast          
                                    from vw_ps_mission where pmno='{0}'",
                                    strPmno);
            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
            if (dt.Rows.Count <= 0)
            {
                strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "派工编码不存在！").DicInfo());
                return strJson;
            }

            goodBillE.StrCodeStorage = Convert.ToString(dt.Rows[0]["code_storage"]);
            goodBillE.StrCodeStorageLast = Convert.ToString(dt.Rows[0]["code_storagelast"]);
            goodBillE.StrMarkTallyBill = Convert.ToString(dt.Rows[0]["mark_tallybill"]);
            goodBillE.StrCodeOperation = Convert.ToString(dt.Rows[0]["code_operation"]);
            goodBillE.StrCodeOperationFact = Convert.ToString(dt.Rows[0]["code_operation_fact"]);

            strSql =
                string.Format("select mark_exchange,mark_last,mark_borrow from vw_hc_consign where cgno='{0}'", strCgno);
            dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
            if (dt.Rows.Count <= 0)
            {
                strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "票货编码不存在！").DicInfo());
                return strJson;
            }

            goodBillE.StrMarkExchange = Convert.ToString(dt.Rows[0]["mark_exchange"]);
            goodBillE.StrMarkLast = Convert.ToString(dt.Rows[0]["mark_last"]);
            goodBillE.StrMarkBorrow = Convert.ToString(dt.Rows[0]["mark_borrow"]);

            return strJson;
        }

        /// <summary>
        /// 获取票货数据（非调发）
        /// </summary>
        /// <param name="strCgno">委托编码</param>
        /// <returns>Json</returns>
        private string GetGoodsBillForNoAdjust(string strCgno)
        {
            string strJson = string.Empty;
            Dictionary<string, object> info = new Dictionary<string, object>();         
            //票货1数据
            string[,] strGoodsBill1Array = null;
            //票货2数据
            string[,] strGoodsBill2Array = null;
            //件数2显示状态
            string strAmount2Visible = "0";         

            //获取票货1数据
            if (goodBillE.StrMarkBorrow == "1")
            {
                var dt = GetDTForVwConsignAssort(strCgno);
                if (dt.Rows.Count > 0)
                {
                    strGoodsBill1Array = new string[dt.Rows.Count, 2];
                    for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                    {
                        strGoodsBill1Array[iRow, 0] = Convert.ToString(dt.Rows[iRow]["gbno"]);
                        strGoodsBill1Array[iRow, 1] = Convert.ToString(dt.Rows[iRow]["gbdisplay"]);
                    }
                }
            }
            else
            {
                var dt = GetDTForVwConsignGbno(strCgno);
                if (dt.Rows.Count > 0)
                {
                    strGoodsBill1Array = new string[dt.Rows.Count, 2];
                    for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                    {
                        strGoodsBill1Array[iRow, 0] = Convert.ToString(dt.Rows[iRow]["gbno"]);
                        strGoodsBill1Array[iRow, 1] = Convert.ToString(dt.Rows[iRow]["gbdisplay"]);
                    }
                }
            }

            //获取票货2数据
            if (goodBillE.StrMarkLast == "1")//目标票货
            {
                goodBillE.StrGoodsBill2Name = "目标票货";
                var dt = GetDTForvVwConsignGbLast(strCgno);
                if (dt.Rows.Count > 0)
                {
                    strGoodsBill2Array = new string[dt.Rows.Count, 2];
                    for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                    {
                        strGoodsBill2Array[iRow, 0] = Convert.ToString(dt.Rows[iRow]["gbno"]);
                        strGoodsBill2Array[iRow, 1] = Convert.ToString(dt.Rows[iRow]["exdisplay"]);
                    }
                }

                strAmount2Visible = "1";
            }

            info.Add("Amount2Visible", strAmount2Visible);
            info.Add("GoodsBill1", strGoodsBill1Array);
            info.Add("GoodsBill2", strGoodsBill2Array);
            info.Add("GoodsBill1Name", goodBillE.StrGoodsBill1Name);
            info.Add("GoodsBill2Name", goodBillE.StrGoodsBill2Name);
            strJson = JsonConvert.SerializeObject(new DicPackage(true, info, null).DicInfo());
            return strJson;
        }

        /// <summary>
        /// 获取票货数据（调发）
        /// </summary>
        /// <param name="strCgno">委托编码</param>
        /// <returns>Json</returns>
        private string GetGoodsBillForAdjust(string strCgno)
        {
            string strJson = string.Empty;
            Dictionary<string, object> info = new Dictionary<string, object>();
            //票货1数据
            string[,] strGoodsBill1Array = null;
            //票货2数据
            string[,] strGoodsBill2Array = null;
            //件数2显示状态
            string strAmount2Visible = "0";

            if (goodBillE.StrCodeOperation != goodBillE.StrCodeOperationFact)
            {
                //是否做业务进出存(1：做；0：不做)
                string strDo = string.Empty;
                var dt = GetDTForTbBrOperationFact();
                if (dt.Rows.Count > 0)
                {
                    strDo = Convert.ToString(dt.Rows[0]["dobusinessaccount"]);
                }
                if (strDo == "1")
                {
                    //获取票货1数据
                    dt = GetDTForVwConsignAdjust(strCgno);
                    if (dt.Rows.Count > 0)
                    {
                        strGoodsBill1Array = new string[dt.Rows.Count, 2];
                        for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                        {
                            strGoodsBill1Array[iRow, 0] = Convert.ToString(dt.Rows[iRow]["gbno"]);
                            strGoodsBill1Array[iRow, 1] = Convert.ToString(dt.Rows[iRow]["gbdisplay"]);
                        }
                    }
                }
                else
                {
                    goodBillE.StrGoodsBill1Name = "库场发货";
                    //获取票货1数据
                    dt = GetDTForVwConsignGbno(strCgno);
                    if (dt.Rows.Count > 0)
                    {
                        strGoodsBill1Array = new string[dt.Rows.Count, 2];
                        for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                        {
                            strGoodsBill1Array[iRow, 0] = Convert.ToString(dt.Rows[iRow]["gbno"]);
                            strGoodsBill1Array[iRow, 1] = Convert.ToString(dt.Rows[iRow]["gbdisplay"]);
                        }
                    }

                    //获取票货2数据
                    dt = GetDTForVwConsignAdjust(strCgno);
                    if (dt.Rows.Count > 0)
                    {
                        strGoodsBill2Array = new string[dt.Rows.Count, 2];
                        for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                        {
                            strGoodsBill2Array[iRow, 0] = Convert.ToString(dt.Rows[iRow]["gbno"]);
                            strGoodsBill2Array[iRow, 1] = Convert.ToString(dt.Rows[iRow]["exdisplay"]);
                        }
                    }
                }
            }
            else//源过程与分解一致
            {
                goodBillE.StrGoodsBill1Name = "库场发货";
                //获取票货1数据
                var dt = GetDTForVwConsignGbno(strCgno);
                if (dt.Rows.Count > 0)
                {
                    strGoodsBill1Array = new string[dt.Rows.Count, 2];
                    for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                    {
                        strGoodsBill1Array[iRow, 0] = Convert.ToString(dt.Rows[iRow]["gbno"]);
                        strGoodsBill1Array[iRow, 1] = Convert.ToString(dt.Rows[iRow]["gbdisplay"]);
                    }
                }

                //获取票货2数据
                dt = GetDTForVwConsignAdjust(strCgno);
                if (dt.Rows.Count > 0)
                {
                    strGoodsBill2Array = new string[dt.Rows.Count, 2];
                    for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                    {
                        strGoodsBill2Array[iRow, 0] = Convert.ToString(dt.Rows[iRow]["gbno"]);
                        strGoodsBill2Array[iRow, 1] = Convert.ToString(dt.Rows[iRow]["exdisplay"]);
                    }
                }

            }

            info.Add("Amount2Visible", strAmount2Visible);
            info.Add("GoodsBill1", strGoodsBill1Array);
            info.Add("GoodsBill2", strGoodsBill2Array);
            info.Add("GoodsBill1Name", goodBillE.StrGoodsBill1Name);
            info.Add("GoodsBill2Name", goodBillE.StrGoodsBill2Name);
            strJson = JsonConvert.SerializeObject(new DicPackage(true, info, null).DicInfo());
            return strJson;
        }

        /// <summary>
        /// 获取票货数据（非调发）
        /// </summary>
        /// <param name="strCodeOperation">作业过程编码</param>
        /// <param name="strCodeOperationFact">作业过程子编码</param>
        /// <param name="strCgno">委托编码</param>
        /// <returns>Json</returns>
        private string GetGoodsBillForNoAdjust1(string strCodeOperation, string strCodeOperationFact, string strCgno)
        {
            string strJson = string.Empty;
            Dictionary<string, object> info = new Dictionary<string, object>();
            //票货1数据
            string[,] strGoodsBill1Array = null;
            //票货2数据
            string[,] strGoodsBill2Array = null;
            //件数2显示状态
            string strAmount2Visible = "0";

            //获取票货1数据
            if (goodBillE.StrMarkBorrow == "1")
            {
                var dt = GetDTForVwConsignAssort(strCgno);
                if (dt.Rows.Count > 0)
                {
                    strGoodsBill1Array = new string[dt.Rows.Count, 2];
                    for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                    {
                        strGoodsBill1Array[iRow, 0] = Convert.ToString(dt.Rows[iRow]["gbno"]);
                        strGoodsBill1Array[iRow, 1] = Convert.ToString(dt.Rows[iRow]["gbdisplay"]);
                    }
                }
            }
            else
            {
                var dt = new DataTable();
                if (GetMarkSource(strCodeOperation, strCodeOperationFact) != "1")
                {
                    dt = GetDTForVwConsignGbno(strCgno);
                }
                else
                {
                    dt = GetDTForvVwConsignGbLast(strCgno);
                }
             
                if (dt.Rows.Count > 0)
                {
                    strGoodsBill1Array = new string[dt.Rows.Count, 2];
                    for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                    {
                        strGoodsBill1Array[iRow, 0] = Convert.ToString(dt.Rows[iRow]["gbno"]);
                        strGoodsBill1Array[iRow, 1] = Convert.ToString(dt.Rows[iRow]["gbdisplay"]);
                    }
                }
            }

            //获取票货2数据
            if (goodBillE.StrMarkLast == "1")//目标票货
            {
                goodBillE.StrGoodsBill2Name = "目标票货";
                var dt = GetDTForvVwConsignGbLast(strCgno);
                if (dt.Rows.Count > 0)
                {
                    strGoodsBill2Array = new string[dt.Rows.Count, 2];
                    for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                    {
                        strGoodsBill2Array[iRow, 0] = Convert.ToString(dt.Rows[iRow]["gbno"]);
                        strGoodsBill2Array[iRow, 1] = Convert.ToString(dt.Rows[iRow]["exdisplay"]);
                    }
                }

                strAmount2Visible = "1";
            }

            info.Add("Amount2Visible", strAmount2Visible);
            info.Add("GoodsBill1", strGoodsBill1Array);
            info.Add("GoodsBill2", strGoodsBill2Array);
            info.Add("GoodsBill1Name", goodBillE.StrGoodsBill1Name);
            info.Add("GoodsBill2Name", goodBillE.StrGoodsBill2Name);
            strJson = JsonConvert.SerializeObject(new DicPackage(true, info, null).DicInfo());
            return strJson;
        }

        /// <summary>
        /// 获取票货数据（调发）
        /// </summary>
        /// <param name="strCgno">委托编码</param>
        /// <param name="strCodeOperation">作业过程编码</param>
        /// <param name="strCodeOperationFact">作业过程子编码</param>
        /// <returns>Json</returns>
        private string GetGoodsBillForAdjust1(string strCodeOperation, string strCodeOperationFact, string strCgno)
        {
            string strJson = string.Empty;
            Dictionary<string, object> info = new Dictionary<string, object>();
            //票货1数据
            string[,] strGoodsBill1Array = null;
            //票货2数据
            string[,] strGoodsBill2Array = null;
            //件数2显示状态
            string strAmount2Visible = "0";

            if (goodBillE.StrCodeOperation != goodBillE.StrCodeOperationFact)
            {
                //是否做业务进出存(1：做；0：不做)
                string strDo = string.Empty;
                var dt = GetDTForTbBrOperationFact();
                if (dt.Rows.Count > 0)
                {
                    strDo = Convert.ToString(dt.Rows[0]["dobusinessaccount"]);
                }
                if (strDo == "1")
                {
                    //获取票货1数据
                    dt = GetDTForVwConsignAdjust(strCgno);
                    if (dt.Rows.Count > 0)
                    {
                        strGoodsBill1Array = new string[dt.Rows.Count, 2];
                        for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                        {
                            strGoodsBill1Array[iRow, 0] = Convert.ToString(dt.Rows[iRow]["gbno"]);
                            strGoodsBill1Array[iRow, 1] = Convert.ToString(dt.Rows[iRow]["gbdisplay"]);
                        }
                    }
                }
                else
                {
                    goodBillE.StrGoodsBill1Name = "库场发货";
                    //获取票货1数据
                    dt = GetDTForVwConsignGbno(strCgno);
                    if (dt.Rows.Count > 0)
                    {
                        strGoodsBill1Array = new string[dt.Rows.Count, 2];
                        for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                        {
                            strGoodsBill1Array[iRow, 0] = Convert.ToString(dt.Rows[iRow]["gbno"]);
                            strGoodsBill1Array[iRow, 1] = Convert.ToString(dt.Rows[iRow]["gbdisplay"]);
                        }
                    }

                    //获取票货2数据
                    dt = GetDTForVwConsignAdjust(strCgno);
                    if (dt.Rows.Count > 0)
                    {
                        strGoodsBill2Array = new string[dt.Rows.Count, 2];
                        for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                        {
                            strGoodsBill2Array[iRow, 0] = Convert.ToString(dt.Rows[iRow]["gbno"]);
                            strGoodsBill2Array[iRow, 1] = Convert.ToString(dt.Rows[iRow]["exdisplay"]);
                        }
                    }
                }
            }
            else//源过程与分解一致
            {
                goodBillE.StrGoodsBill1Name = "库场发货";
                //获取票货1数据
                var dt = GetDTForVwConsignGbno(strCgno);
                if (dt.Rows.Count > 0)
                {
                    strGoodsBill1Array = new string[dt.Rows.Count, 2];
                    for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                    {
                        strGoodsBill1Array[iRow, 0] = Convert.ToString(dt.Rows[iRow]["gbno"]);
                        strGoodsBill1Array[iRow, 1] = Convert.ToString(dt.Rows[iRow]["gbdisplay"]);
                    }
                }

                //获取票货2数据
                dt = GetDTForVwConsignAdjust(strCgno);
                if (dt.Rows.Count > 0)
                {
                    strGoodsBill2Array = new string[dt.Rows.Count, 2];
                    for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                    {
                        strGoodsBill2Array[iRow, 0] = Convert.ToString(dt.Rows[iRow]["gbno"]);
                        strGoodsBill2Array[iRow, 1] = Convert.ToString(dt.Rows[iRow]["exdisplay"]);
                    }
                }

            }

            info.Add("Amount2Visible", strAmount2Visible);
            info.Add("GoodsBill1", strGoodsBill1Array);
            info.Add("GoodsBill2", strGoodsBill2Array);
            info.Add("GoodsBill1Name", goodBillE.StrGoodsBill1Name);
            info.Add("GoodsBill2Name", goodBillE.StrGoodsBill2Name);
            strJson = JsonConvert.SerializeObject(new DicPackage(true, info, null).DicInfo());
            return strJson;
        }    

        /// <summary>
        /// 获取委托票货子表数据
        /// </summary>
        /// <param name="strCgno">委托编码</param>
        /// <returns>DataTable</returns>
        private DataTable GetDTForVwConsignGbno(string strCgno)
        {
            string strSql =
                string.Format("select gbno,gbdisplay || '/' || stockamount || '/' || stockweight as gbdisplay from vw_hc_consign_gbno where cgno='{0}'", strCgno);
            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
            return dt;
        }

        /// <summary>
        /// 获取委托最终票货子表数据
        /// </summary>
        /// <param name="strCgno">委托编码</param>
        /// <returns>DataTable</returns>
        private DataTable GetDTForvVwConsignGbLast(string strCgno)
        {
            string strSql =
                string.Format("select gbno,gbdisplay from vw_hc_consign_gblast where cgno='{0}'", strCgno);
            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
            return dt;
        }

        /// <summary>
        /// 获取调发委托销账票货视图数据
        /// </summary>
        /// <param name="strCgno">委托编码</param>
        /// <returns>DataTable</returns>
        private DataTable GetDTForVwConsignAdjust(string strCgno)
        {
            string strSql =
                string.Format("select gbno,exdisplay,gbdisplay  from vw_hc_consign_adjust2 where cgno='{0}'", strCgno);
            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
            return dt;
        }

        /// <summary>
        /// 获取借贷的委托票货子表数据
        /// </summary>
        /// <param name="strCgno">委托编码</param>
        /// <returns>DataTable</returns>
        private DataTable GetDTForVwConsignAssort(string strCgno)
        {
            string strSql =
                string.Format("select cgno,gbno,code_client,code_pack,gbdisplay || '/' || stockamount || '/' || stockweight as gbdisplay,stockamount,stockweight,mark,blno,vgdisplay,code_inout,factweight from vw_hc_consign_assort where cgno='{0}'", strCgno);
            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
            return dt;
        }

        /// <summary>
        /// 获取分解作业过程表数据
        /// </summary>
        /// <param name="strCgno">委托编码</param>
        /// <returns>DataTable</returns>
        private DataTable GetDTForTbBrOperationFact()
        {
            string strSql =
                    string.Format(@"select dobusinessaccount from TB_BR_OPERATION_FACT 
                                    where code_operation='{0}' and code_operation_fact='{1}'",
                                    goodBillE.StrCodeOperation, goodBillE.StrCodeOperationFact);
            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);;
            return dt;
        }

        /// <summary>
        /// 获取目标票货作为源票货使用标志
        /// </summary>
        /// <param name="strCgno">委托编码</param>
        /// <returns>DataTable</returns>
        private string GetMarkSource(string strCodeOperation, string strCodeOperationFact)
        {
            string strMarkSource = string.Empty;
            string strSql =
                    string.Format(@"select MARK_SOURCE from  nharbor.TB_BR_OPERATION_FACT 
                                    where code_operation='{0}' and code_operation_fact='{1}'",
                                    strCodeOperation, strCodeOperationFact);
            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
            if (dt.Rows.Count > 0)
            {
                strMarkSource = Convert.ToString(dt.Rows[0]["MARK_SOURCE"]);
            }
            return strMarkSource;
        }


    }
}