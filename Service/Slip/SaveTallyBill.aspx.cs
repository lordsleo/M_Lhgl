//
//文件名：    SaveTallyBill.aspx.cs
//功能描述：  保存理货作业票
//创建时间：  2015/11/06
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
using Leo.Oracle;

namespace M_Lhgl.Service.Slip
{
    public partial class SaveTallyBill : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ////http://218.92.115.55/M_Lhgl/Service/Slip/SaveTallyBill.aspx
            var codeCompany = Request.Params["CodeCompany"];
            var codeDepartment = Request.Params["CodeDepartment"];
            var pmno = Request.Params["Pmno"];
            var cgno = Request.Params["Cgno"];
            var codeTallyman = Request.Params["CodeTallyman"];
            var tallyman = Request.Params["Tallyman"];
            var vgno = Request.Params["Vgno"];
            var cabin = Request.Params["Cabin"];
            var codeCarrier = Request.Params["CodeCarrier"];
            var carrierNum = Request.Params["CarrierNum"];
            var codeNvessel = Request.Params["CodeNvessel"];
            var bargepro = Request.Params["Bargepro"];
            var codeStorage = Request.Params["CodeStorage"];
            var codeBooth = Request.Params["CodeBooth"];
            var codeAllocation = Request.Params["CodeAllocation"];
            var allocation = Request.Params["Allocation"].Trim();
            //var allocation = Request.Params["CodeAllocation"];
            var carrier1 = Request.Params["Carrier1"];
            var carrier1Num = Request.Params["Carrier1Num"];
            var vgnoLast = Request.Params["VgnoLast"];
            var cabinLast = Request.Params["CabinLast"];
            var codeCarrierLast = Request.Params["CodeCarrierLast"];
            var carrierNumLast = Request.Params["CarrierNumLast"];
            var codeNvesselLast = Request.Params["CodeNvesselLast"];
            var bargeproLast = Request.Params["BargeproLast"];
            var codeStorageLast = Request.Params["CodeStorageLast"];
            var codeBoothLast = Request.Params["CodeBoothLast"];
            var codeAllocationLast = Request.Params["CodeAllocationLast"].Trim();
            var allocationLast = Request.Params["AllocationLast"].Trim();
            //var allocationLast = Request.Params["CodeAllocationLast"].Trim();
            var carrier2 = Request.Params["Carrier2"];
            var carrier2Num = Request.Params["Carrier2num"];
            var codeGoodsBill = Request.Params["CodeGoodsBill"];
            var goodsBillDisplay = Request.Params["GoodsBillDisplay"];
            var codeGbBusiness = Request.Params["CodeGbBusiness"];
            var gbBusinessDisplay = Request.Params["GbBusinessDisplay"];
            var codeSpecialMark = Request.Params["CodeSpecialMark"];
            var codeWorkingArea = Request.Params["CodeWorkingArea"];
            var codeWorkingAreaLast = Request.Params["CodeWorkingAreaLast"];
            var quality = Request.Params["Quality"];
            var amount = Request.Params["Amount"].Trim();
            var weight = Request.Params["Weight"].Trim();
            var count = Request.Params["Count"].Trim();
            var amount2 = Request.Params["Amount2"].Trim();
            var weight2 = Request.Params["Weight2"].Trim();
            var count2 = Request.Params["Count2"].Trim();
            var trainNum = Request.Params["TrainNum"].Trim();
            var machine = Request.Params["Machine"];
            var workTeam = Request.Params["WorkTeam"];
            var tbno = Request.Params["Tbno"].Trim();
            var markFinish = Request.Params["MarkFinish"].Trim();
            var codeOperationFact = Request.Params["CodeOperationFact"];

            tallyman = "PDA测试员";

//            string message = string.Format(@"vgno:'{0}', cabin:'{1}', codeCarrier:'{2}', carrierNum:'{3}',codeNvessel:'{4}',
//                                             bargepro:'{5}',codeStorage:'{6}',codeBooth:'{7}',codeAllocation:'{8}', + 
//                                             vgnoLast:'{9}', cabinLast:'{10}', codeCarrierLast:'{11}', carrierNumLast:'{12}',codeNvesselLast:'{13}',
//                                             bargeproLast:'{14}',codeStorageLast:'{15}',codeBoothLast:'{16}',codeAllocationLast:'{17}' , + 
//                                             carrier1:'{18}', carrier1Num:'{19}', carrier2:'{20}', carrier2Num:'{21}'",
//                                             vgno, cabin, codeCarrier, carrierNum, codeNvessel, bargepro, codeStorage, codeBooth, codeAllocation, vgnoLast, cabinLast, codeCarrierLast, carrierNumLast, codeNvesselLast, bargeproLast, codeStorageLast, codeBoothLast, codeAllocationLast, carrier1, carrier1Num, carrier2, carrier2Num);

//            Json = JsonConvert.SerializeObject(new DicPackage(false, null, message).DicInfo());
//            return;

            ////测试
            //codeCompany = "14";
            //codeDepartment = "24";
            //pmno = "20151010000161";
            //cgno = "d1bff20fa2d54a0b87e4385a5cb46914";
            //codeTallyman = "227";
            //tallyman = "申";
            //vgno = "20151003000002";
            //cabin = "";
            //codeCarrier = "";
            //carrierNum = "";
            //CodeNvessel = "";
            //bargepro = "";
            //codeStorage = "1";
            //codeBooth = "185";
            //codeAllocation = "";
            //carrier1 = "中韩之星_2099999";
            //carrier1Num = "";
            //vgnoLast = "";
            //cabinLast = "";
            //codeCarrierLast = "";
            //carrierNumLast = "";
            //codeNvesselLast = "";
            //bargeproLast = "";
            //codeStorageLast = "111";
            //codeBoothLast = "183";
            //codeAllocationLast = "";
            //carrier2 = "009场";
            //carrier2Num = "无";
            //codeGoodsBill = "1facb087e4654ddead033fb25552e358";
            //goodsBillDisplay = "淮钢/铁矿石/散/0吨/进口/内贸/中韩之星_20151003_1/b/0/12232";
            //codeGbBusiness = "1facb087e4654ddead033fb25552e358";
            //gbBusinessDisplay = "淮钢/铁矿石/散/0吨/进口/内贸/中韩之星_20151003_1/b/0/12232";
            //codeSpecialMark = "";
            //codeWorkingArea = "";
            //codeWorkingAreaLast = "";
            //quality = "合格";
            //amount = "123";
            //weight = "123.4";
            //count = Request.Params["Count"];
            //amount2 = Request.Params["Amount2"];
            //weight2 = Request.Params["Weight2"];
            //count2 = Request.Params["Count2"];
            //machine = Request.Params["Machine"];
            //workTeam = Request.Params["WorkTeam"];
            //trainNum = "23";
            //tbno = "20151112064082";
            ////tbno = string.Empty;
            //markFinish = "1";

            //var codeCompany = "14";
            //var codeDepartment = "24";
            //var pmno = "20151010000161";
            //var cgno = "d1bff20fa2d54a0b87e4385a5cb46914";
            //var codeTallyman = "227";
            //var tallyman = "薛辉";
            //var vgno = "20151003000002";
            //var cabin = "";
            //var codeCarrier = "";
            //var carrierNum = "";
            //var codeNvessel = "";
            //var bargepro = "";
            //var codeStorage = "";
            //var codeBooth = "";
            //var codeAllocation = "";
            //var carrier1 = "中韩之星_2099999";
            //var carrier1Num = "";
            //var vgnoLast = "";
            //var cabinLast = "";
            //var codeCarrierLast = "";
            //var carrierNumLast = "";
            //var codeNvesselLast = "";
            //var bargeproLast = "";
            //var codeStorageLast = "";
            //var codeBoothLast = "";
            //var codeAllocationLast = "";
            //var carrier2 = "";
            //var carrier2Num = "";
            //var codeGoodsBill = "1facb087e4654ddead033fb25552e358";
            //var goodsBillDisplay = "淮钢/铁矿石/散/0吨/进口/内贸/中韩之星_20151003_1/b/0/12232";
            //var codeGbBusiness = "";
            //var gbBusinessDisplay = "";
            //var codeSpecialMark = "101";
            //var codeWorkingArea = "66D0FE5D86DB45AFB4B572C9AF09D939";
            //var codeWorkingAreaLast = "66D0FE5D86DB45AFB4B572C9AF09D939";
            //var quality = "合格";
            //var amount = "10";
            //var weight = "123";
            //var count = "11";
            //var amount2 = "";
            //var weight2 = "";
            //var count2 = "";
            ////var machine = "[{\"endtime\"1412\"\",\"amount\":\"\",\"pmno\":\"20151021000241\",\"weight\":\"\",\"workno\":\"\",\"count\":\"\",\"begintime\":\"1512\",\"select\":\"1\",\"name\":\"111\",\"code_machine\":\"2C-6-11\",\"machine\":\"6t叉车\"},{\"endtime\":\"\",\"amount\":\"\",\"pmno\":\"20151021000241\",\"weight\":\"\",\"workno\":\"\",\"count\":\"\",\"begintime\":\"\",\"select\":\"1\",\"name\":\"\",\"code_machine\":\"2Q-25-01\",\"machine\":\"牵引车\"}]";
            //var machine = "[{\"endtime\":\"15:33\",\"amount\":\"11\",\"pmno\":\"20151010000161\",\"weight\":\"12.3\",\"workno\":\"3\",\"count\":\"\",\"begintime\":\"14:12\",\"select\":\"1\",\"name\":\"\",\"code_machine\":\"2ZJK-2D-12\",\"machine\":\"集卡\"}]";
            //var workTeam = "";
            //var trainNum = "";
            ////var tbno = "20151112064117";
            //var tbno = string.Empty;
            //var markFinish = "0";


            //调发货标志
            string strMarkExchange = string.Empty;
            string strTallydate = string.Empty;
            string strTaskNo = string.Empty;
            string strGbno = string.Empty;
            string strGbdisplay = string.Empty;
            string strGbnolast = string.Empty;
            string strGbdisplaylast = string.Empty;
            string strCodeMeasure = string.Empty;
            string strCodeOpstype = string.Empty;
            string strCodeBillType = string.Empty;
            string strGbnoFromStock = string.Empty;
            string strCodeStorageStock = string.Empty;
            string strCodeBoothStock = string.Empty;
            string strCodeAllocationStock = string.Empty;
            string strGbnoFirst = string.Empty;
            string strCodeOperation = string.Empty;
            string strCodeOperationFact = string.Empty;
            string strCodeOpstypeFact = string.Empty;
            string strCodeWorkflow = string.Empty;
            string strCodeWorkTime = string.Empty;
            string strCodeTeamType = string.Empty;
            string strCodeTrainRode = string.Empty;
            string strCodeMotorcade = string.Empty;
            string strBeginTime = string.Empty;
            string strEndTime = string.Empty;
            string strNsnoBarge = string.Empty;
            string strNsnoBargeLast = string.Empty;
            DateTime strCurTime = DateTime.Now;
            TallyBillE tallybille = new TallyBillE();

            try
            {
                //获取调发货标志
                string strSql =
                       string.Format("select mark_exchange,mark_last from vw_hc_consign where cgno='{0}'", cgno);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count > 0)
                {
                    strMarkExchange = Convert.ToString(dt.Rows[0]["mark_exchange"]); ;
                }

                //获取理货日期、进出库类别代码、分解过程的进出库类别代码
                strSql =
                        string.Format(@"select code_storage,code_storagelast,mark_tallybill,code_operation,code_operation_fact,CODE_CARRIER_S,CODE_CARRIER_E,vgdisplay,vgno,cabin,code_carrier,nvessel,
                                        code_nvessel,carrier1,nsno,tallydate,code_opstype,code_workflow,code_worktime,code_teamtype,code_trainroad,code_motorcade,begintime,endtime,nsno_barge,nsno_bargelast          
                                        from vw_ps_mission where pmno='{0}'",
                                        pmno);
                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count > 0)
                {
                    strTallydate = Convert.ToString(dt.Rows[0]["tallydate"]);
                    strCodeOpstype = Convert.ToString(dt.Rows[0]["code_opstype"]);
                    strCodeOpstypeFact = Convert.ToString(dt.Rows[0]["code_opstype"]);
                    strCodeWorkflow = Convert.ToString(dt.Rows[0]["code_workflow"]);
                    strCodeWorkTime = Convert.ToString(dt.Rows[0]["code_worktime"]);
                    strCodeTeamType = Convert.ToString(dt.Rows[0]["code_teamtype"]);
                    strCodeTrainRode = Convert.ToString(dt.Rows[0]["code_trainroad"]);
                    strCodeMotorcade = Convert.ToString(dt.Rows[0]["code_motorcade"]);
                    strBeginTime = Convert.ToString(dt.Rows[0]["begintime"]);
                    strEndTime = Convert.ToString(dt.Rows[0]["endtime"]);
                    strNsnoBarge = Convert.ToString(dt.Rows[0]["nsno_barge"]);
                    strNsnoBargeLast = Convert.ToString(dt.Rows[0]["nsno_bargelast"]);
                    strCodeOperation = Convert.ToString(dt.Rows[0]["code_operation"]);
                    strCodeOperationFact = Convert.ToString(dt.Rows[0]["code_operation_fact"]);
                }

                //获取任务号、目的票货编码、目的票货描述
                strSql =
                        string.Format(@"select GBDISPLAY,GBNO,MARK_EXCHANGE,HZGOODSBILL,MARK_LAST,CODE_CARRIER_S,CODE_CARRIER_E,TASKNO from VW_HC_CONSIGN_GBNO where code_company='{0}' and cgno='{1}'",
                                        codeCompany, cgno);
                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count > 0)
                {
                    strTaskNo = Convert.ToString(dt.Rows[0]["taskno"]);
                }

                strGbnolast = codeGbBusiness;
                strGbdisplaylast = gbBusinessDisplay;

                //获取计量单位编码、票货类型、源票货编码、源票货描述....
                strSql =
                        string.Format(@"select code_measure from tb_hc_goodsbill where gbno='{0}'",
                                        codeGoodsBill);
                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count > 0)
                {
                    strCodeMeasure = Convert.ToString(dt.Rows[0]["code_measure"]);
                }

                strCodeBillType = "0";
                strGbno = codeGoodsBill;
                strGbdisplay = goodsBillDisplay;

                //校验和赋值货位（堆）数据
                Json = VerifyAndSetAllocationData(codeCompany, codeStorage, allocation, codeBooth, strCodeOpstype, ref codeAllocation);
                if (!string.IsNullOrWhiteSpace(Json))
                {
                    return;
                }
                Json = VerifyAndSetAllocationData(codeCompany, codeStorageLast, allocationLast, codeBoothLast, strCodeOpstype, ref codeAllocationLast);
                if (!string.IsNullOrWhiteSpace(Json))
                {
                    return;
                }

                if (strMarkExchange == "1")//调发货
                {
                    strGbno = codeGbBusiness;
                    strGbdisplay = gbBusinessDisplay;

                    //strSql =
                    //       string.Format("select gbno,exdisplay,gbdisplay,code_storage,code_booth,storage,booth from vw_hc_consign_adjust2 where cgno='{0}'", cgno);
                    //dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                    //if (dt.Rows.Count > 0)
                    //{
                    //    codeStorage = Convert.ToString(dt.Rows[0]["code_storage"]);
                    //    codeBooth = Convert.ToString(dt.Rows[0]["code_booth"]);
                    //    carrier1 = Convert.ToString(dt.Rows[0]["storage"]);
                    //    carrier1Num = Convert.ToString(dt.Rows[0]["booth"]);
                    //}

                    //codeStorage = Convert.ToString(dt.Rows[0]["code_storage"]);
                    //codeBooth = Convert.ToString(dt.Rows[0]["code_booth"]);
                    //carrier1 = Convert.ToString(dt.Rows[0]["storage"]);
                    //carrier1Num = Convert.ToString(dt.Rows[0]["booth"]);

                    strGbnoFromStock = codeGoodsBill;
                    strCodeStorageStock = codeStorage;
                    strCodeBoothStock = codeBooth;               

                    if (strCodeOpstype == "1")//作业方式属于进港
                    {
                        //判断是否已存在，存在返回编码，不存在则插入，并取回生成的编码
                        strCodeAllocationStock = InsertAllocation(codeCompany, strCodeStorageStock, allocation, strCodeBoothStock);
                    }
                    else
                    {
                        strCodeAllocationStock = codeAllocation;
                    }
                }


                //获取原始票货编码
                strGbnoFirst = GetGbnofirst(strGbno);
                //获取理货单编码
                if (markFinish == "0" && string.IsNullOrWhiteSpace(tbno))
                {
                    tbno = GetTbno();
                }
                if (markFinish == "1" && string.IsNullOrWhiteSpace(tbno))
                {
                    tbno = GetTbno();
                }

                //赋值
                tallybille.Code_Company = codeCompany;
                tallybille.Code_Department = codeDepartment;
                tallybille.Cgno = cgno;
                tallybille.Pmno = pmno;
                tallybille.Code_Tallyman = codeTallyman;
                tallybille.Tallyman = tallyman;
                tallybille.Vgno = vgno;
                tallybille.Cabin = cabin;
                tallybille.Code_Carrier = codeCarrier;
                tallybille.Carriernum = carrierNum;
                tallybille.Code_nvessel = codeNvessel;
                tallybille.Bargepro = bargepro;
                tallybille.Code_Storage = codeStorage;
                tallybille.Code_Booth = codeBooth;
                tallybille.code_allocation = codeAllocation;
                tallybille.Carrier1 = carrier1;
                tallybille.Carrier1num = carrier1Num;
                tallybille.Vgnolast = vgnoLast;
                tallybille.Cabinlast = cabinLast;
                tallybille.Code_Carrierlast = codeCarrierLast;
                tallybille.Carriernumlast = carrierNumLast;
                tallybille.Code_nvessellast = codeNvesselLast;
                tallybille.Bargeprolast = bargeproLast;
                tallybille.Code_Storagelast = codeStorageLast;
                tallybille.Code_Boothlast = codeBoothLast;
                tallybille.code_allocationlast = codeAllocationLast;
                tallybille.Carrier2 = carrier2;
                tallybille.Carrier2num = carrier2Num;
                tallybille.Code_SpecialMark = codeSpecialMark;
                tallybille.code_workingarea = codeWorkingArea;
                tallybille.code_workingarealast = codeWorkingAreaLast;
                tallybille.Code_Quality = quality == "合格" ? "01" : "02";
                tallybille.Creator = codeTallyman;
                tallybille.Creatorname = tallyman;
                tallybille.Carrier1A = allocation;
                tallybille.Carrier2A = allocationLast;

                tallybille.Signdate = Convert.ToDateTime(strTallydate);
                tallybille.Tallydate = Convert.ToDateTime(strTallydate);
                tallybille.Code_Opstype = strCodeOpstype;
                tallybille.Code_opstype_fact = strCodeOpstypeFact;
                tallybille.Code_workflow = strCodeWorkflow;
                tallybille.Code_Worktime = strCodeWorkTime;
                tallybille.Code_Teamtype = strCodeTeamType;
                tallybille.Code_trainroad = strCodeTrainRode;
                tallybille.Code_motorcade = strCodeMotorcade;
                tallybille.Begintime = strBeginTime;
                tallybille.Endtime = strEndTime;
                tallybille.NSNO_BARGE = strNsnoBarge;
                tallybille.NSNO_BARGELAST = strNsnoBargeLast;
                tallybille.Taskno = strTaskNo;
                tallybille.Gbnolast = strGbnolast;
                tallybille.Gbdisplaylast = strGbdisplaylast;
                tallybille.Gbno = strGbno;
                tallybille.Gbdisplay = strGbdisplay;
                tallybille.Code_billtype = strCodeBillType;
                tallybille.Gbnofromstock = strGbnoFromStock;
                tallybille.Code_Storage_Stock = strCodeStorageStock;
                tallybille.Code_Booth_Stock = strCodeBoothStock;
                tallybille.code_allocationstock = strCodeAllocationStock;
                tallybille.Gbnofirst = strGbnoFirst;
                tallybille.Code_Operation = strCodeOperation;
                tallybille.Code_Operation_Fact = strCodeOperationFact;
                tallybille.Tbno = tbno;
                tallybille.Mark_finish = Convert.ToChar(markFinish);
                tallybille.MachineData = machine;
                tallybille.TeamWorkData = workTeam;
                tallybille.Mark_Audit = "1";
                tallybille.AuditTime = strCurTime;
                tallybille.Auditor = codeTallyman;
                tallybille.Auditorname = tallyman;

                //Json = JsonConvert.SerializeObject(new DicPackage(false, null, machine + '+' + workTeam).DicInfo());
                //return;
                //Json = JsonConvert.SerializeObject(new DicPackage(false, null, amount + weight).DicInfo());

                //校验和赋值数量数据
                Json = VerifyAndSetQuantityData(amount, amount2, weight, count, trainNum, ref tallybille);
                if (!string.IsNullOrWhiteSpace(Json))
                {
                    return;
                }

                da.BeginTransaction();
                //暂存或提交数据
                if (tallybille.Mark_finish == '1')//提交
                {
                    if (!SaveTB(tallybille))
                    {
                        Json = JsonConvert.SerializeObject(new DicPackage(false, null, "提交失败！").DicInfo());
                    }
                    Json = SaveMachine(tallybille);
                    if (!string.IsNullOrWhiteSpace(Json))
                    {
                        return;
                    }
                    Json = SaveWorkTeam(tallybille);
                    if (!string.IsNullOrWhiteSpace(Json))
                    {
                        return;
                    }
                    //if (!AuditTB(tallybille))
                    //{
                    //    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "提交失败！").DicInfo());
                    //}

                    Json = JsonConvert.SerializeObject(new DicPackage(true, null, "提交成功！").DicInfo());
                }
                else//暂存
                {
                    if (!SaveTB(tallybille))
                    {
                        Json = JsonConvert.SerializeObject(new DicPackage(false, null, "暂存失败！").DicInfo());
                    }
                    Json = SaveMachine(tallybille);
                    if (!string.IsNullOrWhiteSpace(Json))
                    {
                        return;
                    }
                    Json = SaveWorkTeam(tallybille);
                    if (!string.IsNullOrWhiteSpace(Json))
                    {
                        return;
                    }

                    Json = JsonConvert.SerializeObject(new DicPackage(true, null, "暂存成功！").DicInfo());
                }
                da.CommitTransaction();

                //string strKeyPath = "Software\\HarborSoft\\NHarborService";
                //new DataAccessC(strKeyPath).GetDataSetByStoredProcedure("{ call  PK_TALLYBILL_ACCOUNT.P_TALLYBILL_PIECERATE_MATCH('" + tallybille.Tbno + "')}");
            }
            catch (Exception ex)
            {
                da.RollbackTransaction();
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：提交数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
        DataAccess da = (DataAccess)new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor);  

        #region 获取原始票货编码
        /// <summary>
        /// 获取原始票货编码
        /// </summary>
        /// <param name="strGbno">源票货编码</param>
        /// <returns></returns>
        public string GetGbnofirst(string strGbno)
        {
            string strGbno2 =string.Empty;
            string lGbno = string.Empty;
            int c = 1;
            string strSql = "select gbno from tb_hc_consign where gbno is not null and gbno<>'" + strGbno + "' and GBNO_FORCONSIGN='" + strGbno + "'";
            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
            if (dt.Rows.Count == 0)
            {
                return strGbno;
                
            }
            strGbno2 = dt.Rows[0]["gbno"].ToString();
            lGbno = strGbno + "," + strGbno2;
            while (c > 0)
            {
                strSql = "select gbno from tb_hc_consign where gbno is not null and gbno not in (" + lGbno + ") and GBNO_FORCONSIGN='" + strGbno2 + "'";
                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                c = dt.Rows.Count;
                if (c == 0)
                {
                    return strGbno2;
                }
                strGbno2 = dt.Rows[0]["gbno"].ToString();
                lGbno = lGbno + "," + strGbno2;
            }
            return string.Empty;
        }
        #endregion

        #region 保存作业票
        /// <summary>
        /// 保存作业票
        /// </summary>
        /// <param name="strGbno">TallyBillE对象</param>
        /// <returns></returns>
        public bool SaveTB(TallyBillE tb)
        {
            //校验作业票是否是重复生成
            if (tb.Pmno != string.Empty)
            {
                string strFilter = string.Format(" where tbno='{0}'", tb.Tbno);
                string strSql = string.Format("select * from tb_hs_tallybill{0}", strFilter);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count > 0)//已存在，更新
                {
                    string strUpdate_Hs_Tallybill = @"update vw_reg_tallybill set code_department='{0}',amount='{1}',weight='{2}',code_tallyman='{3}',tallyman='{4}',code_company='{5}', modifier='{6}',modifiername='{7}',modifytime=to_date('{10}', 'yyyy/mm/dd hh24:mi:ss'),amount2='{8}',mark_finish='{9}'" + strFilter;
                    strSql = string.Format(strUpdate_Hs_Tallybill, tb.Code_Department, tb.Amount, tb.Weight, tb.Code_Tallyman, tb.Tallyman, tb.Code_Company,
                                           tb.Creator, tb.Creatorname, tb.Amount2, tb.Mark_finish, tb.AuditTime);
                    da.ExecuteNonQuery(strSql);                                                
                }
                else//不存在，插入
                {
                    string strInsert_Hs_Tallybill = "insert into vw_reg_tallybill(tbno,pmno,gbno,signdate,code_department,taskno,cgno,code_storage,code_booth,CODE_ALLOCATION,vgno,cabin,code_carrier,carriernum,NSNO_BARGE,bargepro,pieceweight,amount,weight,volume,code_measure,begintime,endtime,code_operation,code_operation_fact,code_quality,code_workflow,code_trainroad,workteam,code_worktime,code_teamtype,code_tallyman,tallyman,code_storagelast,code_boothlast,CODE_ALLOCATIONLAST,vgnolast,cabinlast,code_carrierlast,carriernumlast,NSNO_BARGELAST,bargeprolast,carrier1,carrier1num,carrier2,carrier2num,code_opstype,remark,gbdisplay,tallynum,gbnolast,gbdisplaylast,gbnofirst,containernum,parenttbno,gbnofromstock,code_company,code_section,trainnum,tallydate,mark_comp_plan,mark_comp_goodsbill,mark_comp_ship,code_billtype,creator,creatorname,createtime,code_motorcade,code_storage_stock,code_booth_stock,CODE_ALLOCATION_STOCK,code_opstype_fact,drivernum,workernum,amount2,code_workteam,machine,CODE_WORKINGAREA,CODE_WORKINGAREALAST,mark_finish,CARRIER1A,CARRIER2A) " +
                                                    " values('{0}','{1}','{2}',to_date('{3}','yyyy-mm-dd hh24:mi:ss'),'{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}', '{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}','{33}','{34}','{35}','{36}','{37}','{38}','{39}','{40}','{41}','{42}','{43}','{44}','{45}','{46}','{47}','{48}','{49}','{50}','{51}','{52}','{53}','{54}','{55}','{56}','{57}','{58}',to_date('{59}','yyyy-mm-dd hh24:mi:ss'),'{60}','{61}','{62}','{63}','{64}','{65}',to_date('{79}', 'yyyy/mm/dd hh24:mi:ss'),'{66}','{67}','{68}','{69}','{70}','{71}','{72}','{73}','{74}','{75}','{76}','{77}','{78}','{79}','{80}')";
                    strSql = string.Format(strInsert_Hs_Tallybill, tb.Tbno, tb.Pmno, tb.Gbno, tb.Signdate.ToString(),
                                           tb.Code_Department, tb.Taskno, tb.Cgno, tb.Code_Storage, tb.Code_Booth, tb.code_allocation, tb.Vgno,
                                           tb.Cabin, tb.Code_Carrier, tb.Carriernum, tb.NSNO_BARGE, tb.Bargepro, tb.Pieceweight,
                                           tb.Amount, tb.Weight, tb.Volume, tb.Code_Measure, tb.Begintime, tb.Endtime,
                                           tb.Code_Operation, tb.Code_Operation_Fact, tb.Code_Quality, tb.Code_workflow, tb.Code_trainroad, tb.Workteam,
                                           tb.Code_Worktime, tb.Code_Teamtype, tb.Code_Tallyman, tb.Tallyman, tb.Code_Storagelast,
                                           tb.Code_Boothlast, tb.code_allocationlast, tb.Vgnolast, tb.Cabinlast, tb.Code_Carrierlast,
                                           tb.Carriernumlast, tb.NSNO_BARGELAST, tb.Bargeprolast, tb.Carrier1, tb.Carrier1num, tb.Carrier2, tb.Carrier2num,
                                           tb.Code_Opstype, tb.Remark, tb.Gbdisplay, tb.Tallynum, tb.Gbnolast,
                                           tb.Gbdisplaylast, tb.Gbnofirst, tb.Containernum,
                                           tb.Parenttbno, tb.Gbnofromstock, tb.Code_Company, tb.Code_Section, tb.Trainnum, tb.Tallydate,
                                           tb.Mark_comp_plan, tb.Mark_comp_goodsbill, tb.Mark_comp_ship, tb.Code_billtype,
                                           tb.Creator, tb.Creatorname, tb.Code_motorcade, tb.Code_Storage_Stock, tb.Code_Booth_Stock, tb.code_allocationstock,
                                           tb.Code_opstype_fact, tb.Drivernum,
                                           tb.Workernum, tb.Amount2, tb.Code_Workteam, tb.Machine, tb.code_workingarea, tb.code_workingarealast, tb.Mark_finish, tb.Carrier1A, tb.Carrier2A);
                    da.ExecuteNonQuery(strSql);         
                }
            }

            return true;
        }
        #endregion

        #region 审核作业票
        /// <summary>
        /// 审核作业票
        /// </summary>
        /// <param name="strGbno">TallyBillE对象</param>
        /// <returns></returns>
        public bool AuditTB(TallyBillE tb)
        {
            //校验作业票是否是重复生成
            if (tb.Pmno != null)
            {
                string strSql = string.Format(@"update vw_reg_tallybill set mark_audit='{0}',audittime=to_date('{1}', 'yyyy/mm/dd hh24:mi:ss'),auditor='{2}' ,auditorname='{3}' 
                                                where tbno='{4}'", 
                                                tb.Mark_Audit, tb.AuditTime, tb.Auditor, tb.Auditorname, tb.Tbno);
                da.ExecuteNonQuery(strSql);  
            }
            
            return true;
        }
        #endregion

        #region 保存机械
        /// <summary>
        /// 保存机械
        /// </summary>
        /// <param name="TallyBillE">TallyBillE对象</param>
        /// <returns></returns>
        private string SaveMachine(TallyBillE tb)
        {
            string strJson = string.Empty;
            if (tb.MachineData != string.Empty)
            {
                List<Machine> listMachine = JsonConvert.DeserializeObject<List<Machine>>(tb.MachineData);
                for (int iList = 0; iList < listMachine.Count; iList++)
                {
                    Nullable<int> amount = null;
                    Nullable<decimal> weight = null;
                    Nullable<decimal> count = null;                    
                    Machine m = listMachine[iList];

                    /*
                     *判断机械是否被选中，
                     *1选中：判断是否已经存在；存在，更新；不存在，插入；
                     *2未选中：判断是否已经存在；存在，删除；不存在，不用管；
                    */
                    string strFilter = string.Format(" where tbno='{0}' and pmno='{1}' and code_machine='{2}' and workno='{3}'", tb.Tbno, tb.Pmno, m.code_machine, m.workno);
                    string strSql = string.Format("select * from TB_TALLY_MACHINE_DRIVER{0}", strFilter);
                    var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                    if (m.select == "1")
                    {
                        if (string.IsNullOrWhiteSpace(m.begintime) || string.IsNullOrWhiteSpace(m.begintime))
                        {
                            strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "机械起、至时间不能为空！").DicInfo());
                            return strJson;
                        }
                        if (!TokenTool.VerifyHoursAndMinute(m.begintime) || !TokenTool.VerifyHoursAndMinute(m.endtime))
                        {
                            strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "机械起、至时间格式不正确！").DicInfo());
                            return strJson;
                        }

                        strJson = VerifyAndSetQuantityData(m.amount, m.weight, m.count, ref amount, ref weight, ref count);
                        if (!string.IsNullOrWhiteSpace(strJson))
                        {
                            return strJson;
                        }
                        Nullable<decimal> strWeightConfirm = tb.Mark_finish == '1' ? count : null;

                        if (dt.Rows.Count > 0)//已存在，更新
                        {
                            strSql =
                                string.Format(@"update TB_TALLY_MACHINE_DRIVER set start_time='{0}',end_time='{1}',amount='{2}',weight='{3}',worknum='{4}',weight_confirm='{5}',modifytime=to_date('{6}','yyyy-mm-dd hh24:mi:ss'),modifier='{7}',modifiername='{8}'",
                                                m.begintime.Remove(2, 1), m.endtime.Remove(2, 1), amount, weight, count, strWeightConfirm, tb.AuditTime, tb.Creator, tb.Creatorname);
                        }
                        else//不存在，插入
                        {
                            strSql =
                                string.Format(@"insert into TB_TALLY_MACHINE_DRIVER(pmno,tbno,code_machine,workno,start_time,end_time,amount,weight,worknum,code_department,weight_confirm,createtime,creator,creatorname)
                                                values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}',to_date('{11}','yyyy-mm-dd hh24:mi:ss'),'{12}','{13}')",
                                                tb.Pmno, tb.Tbno, m.code_machine, m.workno, m.begintime.Remove(2, 1), m.endtime.Remove(2, 1), amount, weight, count, tb.Code_Department, strWeightConfirm, tb.AuditTime, tb.Creator, tb.Creatorname);
                        }
                    }
                    else
                    {
                        if (dt.Rows.Count > 0)//已存在，删除
                        {
                            strSql =
                                string.Format("delete from TB_TALLY_MACHINE_DRIVER{0}", strFilter);
                        }
                    }
                    da.ExecuteNonQuery(strSql);  
                }
            }

            return strJson;
        }
        #endregion

        #region 保存班组
        /// <summary>
        /// 保存班组
        /// </summary>
        /// <param name="strGbno">TallyBillE对象</param>
        /// <returns></returns>
        private string SaveWorkTeam(TallyBillE tb)
        {
            string strJson = string.Empty;
            if (tb.TeamWorkData != string.Empty)
            {
                List<TeamWork> listTeamWork = JsonConvert.DeserializeObject<List<TeamWork>>(tb.TeamWorkData);
                for (int iList = 0; iList < listTeamWork.Count; iList++)
                {
                    Nullable<int> amount = null;
                    Nullable<decimal> weight = null;
                    Nullable<decimal> count = null;
                    TeamWork tw = listTeamWork[iList];

                    /*
                     *判断班组是否被选中，
                     *1选中：判断是否已经存在；存在，更新；不存在，插入；
                     *2未选中：判断是否已经存在；存在，删除；不存在，不用管；
                    */
                    string strFilter = string.Format(" where tbno='{0}' and pmno='{1}' and code_workteam='{2}' and workno='{3}'", tb.Tbno, tb.Pmno, tw.code_workteam, tw.workno);
                    string strSql = string.Format("select * from TB_TALLY_WORKER{0}", strFilter);
                    var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                    if (tw.select == "1")
                    {
                        if (string.IsNullOrWhiteSpace(tw.begintime) || string.IsNullOrWhiteSpace(tw.endtime))
                        {
                            strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "班组起、至时间不能为空！").DicInfo());
                            return strJson;
                        }
                        if (!TokenTool.VerifyHoursAndMinute(tw.begintime) || !TokenTool.VerifyHoursAndMinute(tw.endtime))
                        {
                            strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "班组起、至时间格式不正确！").DicInfo());
                            return strJson;
                        }

                        strJson = VerifyAndSetQuantityData(tw.amount, tw.weight, tw.count, ref amount, ref weight, ref count);
                        if (!string.IsNullOrWhiteSpace(strJson))
                        {
                            return strJson;
                        }
                        Nullable<decimal> strWeightConfirm = tb.Mark_finish == '1' ? count : null;

                        if (dt.Rows.Count > 0)//已存在，更新
                        {
                            strSql =
                                string.Format(@"update TB_TALLY_WORKER set start_time='{0}',end_time='{1}',amount='{2}',weight='{3}',worknum='{4}',weight_confirm='{5}',modifytime=to_date('{6}','yyyy-mm-dd hh24:mi:ss'),modifier='{7}',modifiername='{8}'",
                                                tw.begintime.Remove(2, 1), tw.endtime.Remove(2, 1), amount, weight, count, strWeightConfirm, tb.AuditTime, tb.Creator, tb.Creatorname);
                        }
                        else//不存在，插入
                        {
                            strSql =
                                string.Format(@"insert into TB_TALLY_WORKER(pmno,tbno,code_workteam,workno,start_time,end_time,amount,weight,worknum,code_department,weight_confirm,createtime,creator,creatorname)
                                                values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}',to_date('{11}','yyyy-mm-dd hh24:mi:ss'),'{12}','{13}')",
                                                tb.Pmno, tb.Tbno, tw.code_workteam, tw.workno, tw.begintime.Remove(2, 1), tw.endtime.Remove(2, 1), amount, weight, count, tb.Code_Department, strWeightConfirm, tb.AuditTime, tb.Creator, tb.Creatorname);
                        }
                    }
                    else
                    {
                        if (dt.Rows.Count > 0)//已存在，删除
                        {
                            strSql =
                                string.Format("delete from TB_TALLY_WORKER{0}", strFilter);
                        }
                    }
                    da.ExecuteNonQuery(strSql);  
                }
            }

            return strJson;
        }
        #endregion

        #region 获取理货单编码
        /// <summary>
        /// 获取理货单编码
        /// </summary>
        /// <param name="strCodeCompany">公司编码</param>
        /// <returns></returns>
        private string GetTbno()
        {
            string strJson = string.Empty;
            string strSql = "select to_char(sysdate,'yyyymmdd') || lpad(to_char(TBNO_ID.Nextval),6,'0') as tbno from dual";
            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
            if (dt.Rows.Count <= 0)
            {
                strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "网络连接异常！").DicInfo());
                return strJson;
            }

            return Convert.ToString(dt.Rows[0]["tbno"]); ;
        }
        #endregion

        #region 校验和赋值数量数据
        /// <summary>
        /// 校验和赋值数量数据
        /// </summary>
        /// <param name="amount">件数</param>
        /// <param name="weight">件数2</param>
        /// <param name="count">重量</param>
        /// <param name="count">数量</param>
        /// <param name="count">车数</param>
        /// <param name="count">TallyBillE对象</param>
        /// <returns></returns>
        private string VerifyAndSetQuantityData(string amount, string amount2, string weight, string count, string trainNum, ref TallyBillE tb)
        {             
            string strJson = string.Empty;
            try
            {
                tb.Amount = Format.ConToInt32(amount);
            }
            catch
            {
                strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "件数格式输入不正确！").DicInfo());
                return strJson;
            }
            try
            {
                tb.Amount2 = Format.ConToInt32(amount2);
            }
            catch
            {
                strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "件数2格式输入不正确！").DicInfo());
                return strJson;
            }        
            try
            {
                if (string.IsNullOrWhiteSpace(weight))
                {
                    strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "请输入重量！").DicInfo());
                    return strJson;
                }
                tb.Weight = Format.ConToDecimal(weight);
            }
            catch
            {
                strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "重量格式输入不正确！").DicInfo());
                return strJson;
            }
            try
            {
                tb.Worknum = Format.ConToDecimal(count);
            }
            catch
            {
                strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "数量格式输入不正确！").DicInfo());
                return strJson;
            }
            try
            {
                tb.Trainnum = Format.ConvertToDecimal(trainNum);
            }
            catch
            {
                strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "车数格式输入不正确！").DicInfo());
                return strJson;
            }
            
            return strJson;
        }
        #endregion

        #region 校验和赋值数量数据
        /// <summary>
        /// 校验和赋值数量数据
        /// </summary>
        /// <param name="amount">件数</param>
        /// <param name="weight">重量</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        private string VerifyAndSetQuantityData(string amount1, string weight1, string count1, ref Nullable<int> amount2, ref Nullable<decimal> weight2, ref Nullable<decimal> count2)
        {       
            string strJson = string.Empty;
            try
            {
                amount2 = Format.ConToInt32(amount1);
            }
            catch
            {
                strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "件数格式输入不正确！").DicInfo());
                return strJson;
            }
            try
            {
                if (string.IsNullOrWhiteSpace(weight1))
                {
                    strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "请输入重量！").DicInfo());
                    return strJson;
                }
                weight2 = Format.ConToDecimal(weight1);
            }
            catch
            {
                strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "重量格式输入不正确！").DicInfo());
                return strJson;
            }         
            try
            {
                count2 = Format.ConToDecimal(count1);
            }
            catch
            {
                strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "数量格式输入不正确！").DicInfo());
                return strJson;
            }     

            return strJson;
        }
        #endregion

        #region 校验和赋值货位（堆）数据
        /// <summary>
        /// 校验和赋值货位（堆）数据
        /// </summary>
        /// <param name="strCodeCompany">公司编码</param>
        /// <param name="strCodeStorage">场地编码</param>
        /// <param name="strCodeAllocation">货位（堆）</param>
        /// <param name="strCodeBooth">桩角编码</param>
        /// <param name="strCodeOpstype">作业过程库场分类编码</param>
        /// <param name="tb"></param>
        /// <returns></returns>
        private string VerifyAndSetAllocationData(string strCodeCompany, string strCodeStorage, string strAllocation, string strCodeBooth, string strCodeOpstype, ref string strCodeAllocation)
        {             
            string strJson = string.Empty;

            if (string.IsNullOrWhiteSpace(strCodeStorage))
            {
                return strJson;
            }
            if (!string.IsNullOrWhiteSpace(strCodeStorage) && string.IsNullOrWhiteSpace(strAllocation))
            {
                strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "堆号不能为空！").DicInfo());
                return strJson;
            }
            if (!string.IsNullOrWhiteSpace(strCodeStorage) && string.IsNullOrWhiteSpace(strCodeBooth))
            {
                strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "桩角不能为空！").DicInfo());
                return strJson;
            }
            if (strCodeOpstype == "1")//作业方式属于进港
            {
                //判断是否已存在，存在返回编码，不存在则插入，并取回生成的编码
                strCodeAllocation = InsertAllocation(strCodeCompany, strCodeStorage, strAllocation, strCodeBooth);
            }
            //else 
            //{
            //    strCodeAllocation = strCodeAllocation;
            //}
                     
            return strJson;
        }
        #endregion

        #region 判断堆里是否已存在堆号，不存在，则插入
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strCodeCompany"></param>
        /// <param name="strCodeStorage"></param>
        /// <param name="strCodeAllocation"></param>
        /// <param name="strCodeBooth"></param>
        /// <returns></returns>
        private string InsertAllocation(string strCodeCompany, string strCodeStorage, string strAllocation, string strCodeBooth)
        {
            string strSql = string.Empty;
            string strCode = string.Empty;
            strSql = string.Format(@"select count(*) as total  from  TB_CODE_ALLOCATION 
                where code_company='" + strCodeCompany + "' and code_storage='" + strCodeStorage + "' and  code_booth='" + strCodeBooth + "' and allocation='" + strAllocation + "' ");
            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCBase).ExecuteTable(strSql);
            if (Convert.ToString(dt.Rows[0]["total"]) == "0")
            {
                //执行语句
                strSql = string.Format(@"insert into TB_CODE_ALLOCATION(code_company,code_storage,code_booth,allocation)
                                       values('{0}','{1}','{2}','{3}')", strCodeCompany, strCodeStorage, strCodeBooth, strAllocation);
                new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCBase).ExecuteNonQuery(strSql);
                strSql = string.Format(@"select CODE_ALLOCATION  from TB_CODE_ALLOCATION 
                    where code_company='" + strCodeCompany + "' and code_storage='" + strCodeStorage + "' and  code_booth='" + strCodeBooth + "' and allocation='" + strAllocation + "' ");
                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCBase).ExecuteTable(strSql);
                if (dt.Rows.Count >= 0)
                {
                    strCode = Convert.ToString(dt.Rows[0]["CODE_ALLOCATION"]);
                }
            }
            else
            {
                strSql = "select distinct CODE_ALLOCATION from baseresource.TB_CODE_ALLOCATION where  code_company='" + strCodeCompany + "' and ALLOCATION='" + strAllocation + "'";
                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCBase).ExecuteTable(strSql);
                if (dt.Rows.Count >= 0)
                {
                    strCode = Convert.ToString(dt.Rows[0]["CODE_ALLOCATION"]);
                }
            }
            return strCode;
        }
        #endregion

    }
}










