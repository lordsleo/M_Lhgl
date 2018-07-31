//
//文件名：    GetMissionPlan.aspx.cs
//功能描述：  获取派工计划数据
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
    public partial class GetMissionPlan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //数据起始行
            var startRow = Request.Params["StartRow"];
            //行数
            var count = Request.Params["Count"];
            //公司编码
            var codeCompany = Request.Params["CodeCompany"];

            //startRow = "1";
            //count = "15";
            //codeCompany = "14";

            try
            {
                if (startRow == null || count == null || codeCompany == null)
                {
                    string warning = string.Format("参数StartRow，Count，CodeCompany不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Slip/GetMissionPlan.aspx?StartRow=1&Count=14&CodeCompany=14");
                    Json = JsonConvert.SerializeObject(new DicPackage(warning).FalseDic());
                    return;

                }
                string strSql =
                    string.Format(@"select *
                                    from vw_ps_mission_yardplan1 where code_company='{0}' 
                                    order by tallydate desc", 
                                    codeCompany);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZHarbor).ExecuteTable(strSql, Convert.ToInt32(startRow), Convert.ToInt32(startRow) + Convert.ToInt32(count) - 1);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage("暂无数据！").FalseDic());
                    return;
                }

                string[,] strArray = new string[dt.Rows.Count, 7];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    strArray[iRow, 0] = Convert.ToString(dt.Rows[iRow]["cgno"]);
                    strArray[iRow, 1] = Convert.ToString(dt.Rows[iRow]["TASKNO"]);
                    strArray[iRow, 2] = Convert.ToString(dt.Rows[iRow]["nberthlast"]);
                    strArray[iRow, 3] = Convert.ToString(dt.Rows[iRow]["operation_fact"]);
                    strArray[iRow, 4] = Convert.ToString(dt.Rows[iRow]["begintime"]);
                    strArray[iRow, 5] = Convert.ToString(dt.Rows[iRow]["endtime"]);
                    strArray[iRow, 6] = Convert.ToString(dt.Rows[iRow]["cargo"]);
                }

                Json = JsonConvert.SerializeObject(new DicPackage().TrueDic(strArray));
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(string.Format("{0}：获取数据发生异常。{1}", ex.Source, ex.Message)).FalseDic());
            }
        }
        protected string Json;
    }
}