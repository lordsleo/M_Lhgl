//
//文件名：    GetOperationPlan.aspx.cs
//功能描述：  获取作业计划数据
//创建时间：  2015/10/03
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

namespace M_Lhgl.Service.Plan
{
    public partial class GetOperationPlan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //数据起始行
            var startRow = Request.Params["StartRow"];
            //行数
            var count = Request.Params["Count"];
            //公司编码
            var codeCompany = Request.Params["CodeCompany"];
            //泊位
            var nberth = Request.Params["Nberth"];
            //过程
            var operationFact = Request.Params["OperationFact"];
            //开始时间
            var beginTime = Request.Params["BeginTime"];
            //结束时间
            var endTime = Request.Params["EndTime"];
            //货名
            var cargo = Request.Params["Cargo"];

            nberth = nberth == null ? string.Empty : nberth;
            operationFact = operationFact == null ? string.Empty : operationFact;
            beginTime = beginTime == null ? string.Empty : beginTime;
            endTime = endTime == null ? string.Empty : endTime;
            cargo = cargo == null ? string.Empty : cargo;

            //startRow = "1";
            //count = "15";
            //cargo = "煤炭";
            //cargoOwner = "北京弘帆";

            try
            {
                if (startRow == null || count == null || codeCompany == null)
                {
                    string warning = string.Format("参数StartRow，Count，CodeCompany不能为null！举例：http://218.92.115.55/M_Lhgl/Service/Plan/GetOperationPlan.aspx?StartRow=1&Count=14&CodeCompany=14");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;
                }

                string strFilter = string.Empty;
                string strTable = "vw_ps_mission_yardplan1 a";
                if (!string.IsNullOrWhiteSpace(codeCompany))
                {
                    strFilter += string.Format("a.code_company='{0}' and ", codeCompany);
                }
                if (!string.IsNullOrWhiteSpace(nberth))
                {
                    strFilter += string.Format("a.nberthlast='{0}' and ", nberth);
                }
                if (!string.IsNullOrWhiteSpace(operationFact))
                {
                    strFilter += string.Format("a.operation_fact='{0}' and ", operationFact);
                }
                if (!string.IsNullOrWhiteSpace(beginTime))
                {
                    strFilter += string.Format("a.begintime='{0}' and ", beginTime);
                }
                if (!string.IsNullOrWhiteSpace(endTime))
                {
                    strFilter += string.Format("a.endtime='{0}' and ", endTime);
                }
                if (!string.IsNullOrWhiteSpace(cargo))
                {
                    strFilter += string.Format("a.cargo='{0}' and ", cargo);
                }
                if (!string.IsNullOrWhiteSpace(strFilter))
                {
                    strFilter = strFilter.Remove(strFilter.Length - 5, 5);
                    strFilter = " where " + strFilter;
                }

                string strSql =
                    string.Format(@"select a.cgno,a.nberthlast,a.operation_fact,a.begintime,a.endtime,a.cargo 
                                    from {0}{1} order by a.begintime",
                                    strTable, strFilter);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql, Convert.ToInt32(startRow), Convert.ToInt32(startRow) + Convert.ToInt32(count) - 1);
                if (dt.Rows.Count <= 0)
                {
                    string strWarning = startRow == "1" ? "暂无数据！" : "暂无更多数据！";
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, strWarning).DicInfo());
                    return;
                }

                string[,] strArray = new string[dt.Rows.Count, 6];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    strArray[iRow, 0] = Convert.ToString(dt.Rows[iRow]["cgno"]);
                    strArray[iRow, 1] = Convert.ToString(dt.Rows[iRow]["nberthlast"]);
                    strArray[iRow, 2] = Convert.ToString(dt.Rows[iRow]["operation_fact"]);
                    strArray[iRow, 3] = Convert.ToString(dt.Rows[iRow]["begintime"]);
                    strArray[iRow, 4] = Convert.ToString(dt.Rows[iRow]["endtime"]);
                    strArray[iRow, 5] = Convert.ToString(dt.Rows[iRow]["cargo"]);;
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