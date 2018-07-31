//
//文件名：    GetConsign.aspx.cs
//功能描述：  获取委托查询列表数据
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

namespace M_Lhgl.Service.Consign
{
    public partial class GetConsign : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //数据起始行
            var startRow = Request.Params["StartRow"];
            //行数
            var count = Request.Params["Count"];
            //公司编码
            var codeCompany = Request.Params["CodeCompany"];
            //起始时间
            var startTime = Request.Params["StartTime"];
            //终止时间
            var endTime = Request.Params["EndTime"];
            //货物
            var cargo = Request.Params["Cargo"];
            //货主
            var cargoOwner = Request.Params["CargoOwner"];
            //航次
            var voyage = Request.Params["Voyage"];
            //作业过程
            var operation = Request.Params["Operation"];
            //委托号
            var taskNo = Request.Params["TaskNo"];
            //唛头
            var mark = Request.Params["Mark"];
            //驳船名
            var nvessel = Request.Params["Nvessel"];

            cargo = cargo == null ? string.Empty : cargo;
            voyage = voyage == null ? string.Empty : voyage;
            taskNo = taskNo == null ? string.Empty : taskNo;
            mark = mark == null ? string.Empty : mark;
            nvessel = nvessel == null ? string.Empty : nvessel;

            //codeCompany = "14";
            startRow = "1";
            count = "15";
            //startTime = "2015-01-18";
            //endTime = "2015-10-18";
            //cargo = "煤炭";
            //cargoOwner = "北京弘帆";

            try
            {
                if (startRow == null || count == null || codeCompany == null || startTime == null || endTime == null)
                {
                    string warning = string.Format("参数StartRow，Count，CodeCompany，StartTime，EndTime不能为null！举例：http://218.92.115.55/M_Lhgl/Service/Consign/GetConsign.aspx?StartRow=1&Count=14&CodeCompany=14&StartTime=2015-08-19&EndTime=2015-09-19");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;
                }

                string strFilter = string.Empty;
                string strTable = "VW_HC_CONSIGN a";
                if (!string.IsNullOrWhiteSpace(codeCompany))
                {
                    strFilter += string.Format("a.code_company='{0}' and ", codeCompany);
                }
                if (!string.IsNullOrWhiteSpace(startTime))
                {
                    strFilter += string.Format("a.signdate>=to_date('{0}','yyyy/MM/dd') and ", Convert.ToDateTime(startTime).ToShortDateString());
                }
                if (!string.IsNullOrWhiteSpace(endTime))
                {
                    strFilter += string.Format("a.signdate<=to_date('{0}','yyyy/MM/dd') and ", Convert.ToDateTime(endTime).ToShortDateString());
                }
                if (!string.IsNullOrWhiteSpace(cargo))
                {
                    strFilter += string.Format("a.cargo='{0}' and ", cargo);
                }
                if (!string.IsNullOrWhiteSpace(cargoOwner))
                {
                    strFilter += string.Format("a.cargoowner='{0}' and ", cargoOwner);
                }
                if (!string.IsNullOrWhiteSpace(voyage))
                {
                    strFilter += string.Format("a.voyage='{0}' and ", voyage);
                }
                if (!string.IsNullOrWhiteSpace(operation))
                {
                    strFilter += string.Format("a.operation='{0}' and ", operation);
                }
                if (!string.IsNullOrWhiteSpace(taskNo))
                {
                    strFilter += string.Format("a.taskno like '{0}' and ", taskNo);
                }
                if (!string.IsNullOrWhiteSpace(mark))
                {
                    strFilter += string.Format("a.mark like '{0}' and ", mark);
                }
                if (!string.IsNullOrWhiteSpace(nvessel))
                {
                    strFilter += string.Format("b.nvessel like '{0}' and ", mark);
                    strFilter += "a.cgno=b.cgno and ";
                    strTable = "VW_HC_CONSIGN a, VW_HC_CONSIGNSHIP b";
                }
                if (!string.IsNullOrWhiteSpace(strFilter))
                {
                    strFilter = strFilter.Remove(strFilter.Length - 5, 5);
                    strFilter = " where " + strFilter;
                }

                string strSql =
                    string.Format(@"select a.cgno,a.cargoowner,a.cargo,a.voyage,a.operation,a.taskno,a.planamount,a.planweight
                                    from {0}{1} order by a.signdate desc",                                
                                    strTable, strFilter);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql, Convert.ToInt32(startRow), Convert.ToInt32(startRow) + Convert.ToInt32(count) - 1);
                if (dt.Rows.Count <= 0)
                {
                    string strWarning = startRow == "1" ? "暂无数据！" : "暂无更多数据！";
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, strWarning).DicInfo());
                    return;
                }

                string[,] strArray = new string[dt.Rows.Count, 8];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    strArray[iRow, 0] = Convert.ToString(dt.Rows[iRow]["cgno"]);
                    strArray[iRow, 1] = Convert.ToString(dt.Rows[iRow]["cargoowner"]);
                    strArray[iRow, 2] = Convert.ToString(dt.Rows[iRow]["cargo"]);
                    strArray[iRow, 3] = Convert.ToString(dt.Rows[iRow]["voyage"]);
                    strArray[iRow, 4] = Convert.ToString(dt.Rows[iRow]["operation"]);
                    strArray[iRow, 5] = Convert.ToString(dt.Rows[iRow]["taskno"]);
                    strArray[iRow, 6] = Convert.ToString(dt.Rows[iRow]["planamount"]);
                    strArray[iRow, 7] = Convert.ToString(dt.Rows[iRow]["planweight"]);
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