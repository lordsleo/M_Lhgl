//
//文件名：    GetSavedGoodsBill.aspx.cs
//功能描述：  获取暂存票货数据
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
    public partial class GetSavedGoodsBill : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //派工编码
            var pmno = Request.Params["Pmno"];
            //委托编码
            var cgno = Request.Params["Cgno"];
            //理货单编码
            var tbno = Request.Params["Tbno"];

            try
            {
                //
                // 非调发 ?Pmno=20151010000161&Cgno=d1bff20fa2d54a0b87e4385a5cb46914  
                //        ?Pmno=20151021000241&Cgno=c14ba9abd9694d9b8e40250c5891b360 
                //   调发 ?Pmno=20151009000121&Cgno=b02fb038209f4008bedfba6f27966efe 
                //

                if (pmno == null || cgno == null || tbno == null)
                {
                    string warning = string.Format("参数Pmno，Cgno，Tbno不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Slip/GetSavedGoodsBill.aspx?Pmno=20151212000565&Cgno=8f18014480034dbdb3f5c234ccd35b1f&Tbno=20151221066196");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;
                }

                string strGbdisplay = string.Empty;
                string strGbdisplaylast = string.Empty;
                string strSql =
                    string.Format(@"select gbdisplay,gbdisplaylast 
                                    from VW_REG_TALLYBILL 
                                    where tbno='{0}'",
                                    tbno);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count > 0)
                {
                    strGbdisplay = Convert.ToString(dt.Rows[0]["gbdisplay"]);
                    strGbdisplaylast = Convert.ToString(dt.Rows[0]["gbdisplaylast"]);
                }

                //获取基础数据
                Json = GetBaseData(pmno, cgno);
                if (Json != string.Empty)
                {
                    return;
                }

                goodBillE.StrGoodsBill1Name = "销账票货";
                goodBillE.StrGoodsBill2Name = "商务销账";

                if (goodBillE.StrMarkExchange == "0")//非调发
                {
                    Json = GetGoodsBillForNoAdjust(cgno, strGbdisplay, strGbdisplaylast);
                }
                else//调发
                {
                    Json = GetGoodsBillForAdjust(cgno, strGbdisplay, strGbdisplaylast);
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
                string.Format("select mark_exchange,mark_last from vw_hc_consign where cgno='{0}'", strCgno);
            dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
            if (dt.Rows.Count <= 0)
            {
                strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "票货编码不存在！").DicInfo());
                return strJson;
            }

            goodBillE.StrMarkExchange = Convert.ToString(dt.Rows[0]["mark_exchange"]);
            goodBillE.StrMarkLast = Convert.ToString(dt.Rows[0]["mark_last"]);

            return strJson;
        }

        /// <summary>
        /// 获取票货数据（非调发）
        /// </summary>
        /// <param name="strPmno">派工编码</param>
        /// <param name="strCgno">委托编码</param>
        /// <param name="strGbdisplay">源票货描述</param>
        /// <param name="strGbdisplaylast">目的票货描述</param>
        /// <returns></returns>
        private string GetGoodsBillForNoAdjust(string strCgno, string strGbdisplay, string strGbdisplaylast)
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
            strGoodsBill1Array = new string[1, 2];
            strGoodsBill1Array[0, 0] = string.Empty;
            strGoodsBill1Array[0, 1] = strGbdisplay;   

            //获取票货2数据
            if (goodBillE.StrMarkLast == "1")//目标票货
            {
                goodBillE.StrGoodsBill2Name = "目标票货";
                strGoodsBill2Array = new string[1, 2];
                strGoodsBill2Array[0, 0] = string.Empty;
                strGoodsBill2Array[0, 1] = strGbdisplaylast;             

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
        /// <param name="strGbdisplay">源票货描述</param>
        /// <param name="strGbdisplaylast">目的票货描述</param>
        /// <returns>Json</returns>
        private string GetGoodsBillForAdjust(string strCgno, string strGbdisplay, string strGbdisplaylast)
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
                    strGoodsBill1Array = new string[dt.Rows.Count, 2];
                    strGoodsBill1Array[0, 0] = string.Empty;
                    strGoodsBill1Array[0, 1] = strGbdisplay;
                  
                }
                else
                {
                    goodBillE.StrGoodsBill1Name = "库场发货";
                    //获取票货1数据      
                    strGoodsBill1Array = new string[1, 2];             
                    strGoodsBill1Array[0, 0] = string.Empty;
                    strGoodsBill1Array[0, 1] = strGbdisplaylast;                             

                    //获取票货2数据
                    strGoodsBill2Array = new string[1, 2];
                    strGoodsBill2Array[0, 0] = string.Empty;
                    strGoodsBill2Array[0, 1] = strGbdisplay;                              
                }
            }
            else//源过程与分解一致
            {
                goodBillE.StrGoodsBill1Name = "库场发货";
                //获取票货1数据
                strGoodsBill1Array = new string[1, 2];
                strGoodsBill1Array[0, 0] = string.Empty;
                strGoodsBill1Array[0, 1] = strGbdisplay;

                //获取票货2数据
                strGoodsBill2Array = new string[1, 2];
                strGoodsBill2Array[0, 0] = string.Empty;
                strGoodsBill2Array[0, 1] = strGbdisplay; 

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
                string.Format("select gbno,gbdisplay from vw_hc_consign_gbno where cgno='{0}'", strCgno);
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
                string.Format("select gbno,exdisplay,gbdisplay,code_storage,code_booth,storage,booth from vw_hc_consign_adjust2 where cgno='{0}'", strCgno);
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
            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql); ;
            return dt;
        }

    }
}