//
//文件名：    GetMissionCommand.aspx.cs
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
    public partial class GetMissionCommand : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //数据起始行
            var startRow = Request.Params["StartRow"];
            //行数
            var count = Request.Params["Count"];
            //用户编码
            var codeUser = Request.Params["CodeUser"];
            //理货日期
            var tallyDate = Request.Params["TallyDate"];
            //白夜班
            var dayNight = Request.Params["DayNight"];

            tallyDate = tallyDate == null ? string.Empty : tallyDate;
            dayNight = dayNight == null ? string.Empty : dayNight;

            //dayNight = "夜班";
            codeUser = "71B4CD89E6654D84B8AED9116DDB1C4C";
            //startRow = "1";
            //count = "15";

            //cargo = "煤炭";
            //taskNo = "北京弘帆";

            try
            {
                if (startRow == null || count == null || codeUser == null)
                {
                    string warning = string.Format("参数StartRow，Count，CodeUser不能为null！举例：http://218.92.115.55/M_Lhgl/Service/Slip/GetMissionCommand.aspx?StartRow=1&Count=14&CodeUser=71B4CD89E6654D84B8AED9116DDB1C4C");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;
                }

                string strFilter = string.Empty;
                string strTable = "vw_ps_mission_ma a";
                if (!string.IsNullOrWhiteSpace(codeUser))
                {
                    strFilter += string.Format("a.code_tallyman2='{0}' and ", codeUser);
                }
                if (!string.IsNullOrWhiteSpace(tallyDate))
                {
                    strFilter += string.Format("a.tallydate=to_date('{0}', 'yyyy/mm/dd') and ", Convert.ToDateTime(tallyDate).ToShortDateString());
                }
                if (!string.IsNullOrWhiteSpace(dayNight))
                {
                    dayNight = dayNight == "白班" ? "1" : "2";
                    strFilter += string.Format("a.code_worktime='{0}' and ", dayNight);
                }
                if (!string.IsNullOrWhiteSpace(strFilter))
                {
                    strFilter = strFilter.Remove(strFilter.Length - 5, 5);
                    strFilter = " where " + strFilter;
                }         

                string strSql =
                    string.Format(@"select a.pmno,a.cgno,a.client,a.taskno,a.cargo,a.operation,a.planweight,a.begintime,a.endtime,a.carrier1,a.carrier2
                                    from {0}{1} order by a.tallydate desc",
                                    strTable, strFilter);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql, Convert.ToInt32(startRow), Convert.ToInt32(startRow) + Convert.ToInt32(count) - 1);
                if (dt.Rows.Count <= 0)
                {
                    string strWarning = startRow == "1" ? "暂无数据！" : "暂无更多数据！";
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, strWarning).DicInfo());
                    return;
                }

                string[,] strArray = new string[dt.Rows.Count, 14];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    strArray[iRow, 0] = Convert.ToString(dt.Rows[iRow]["pmno"]);
                    strArray[iRow, 1] = Convert.ToString(dt.Rows[iRow]["cgno"]);
                    strArray[iRow, 2] = string.Empty;
                    strArray[iRow, 3] = Convert.ToString(dt.Rows[iRow]["client"]);
                    strArray[iRow, 4] = Convert.ToString(dt.Rows[iRow]["taskno"]);
                    strArray[iRow, 5] = Convert.ToString(dt.Rows[iRow]["cargo"]);
                    strArray[iRow, 6] = Convert.ToString(dt.Rows[iRow]["operation"]);
                    strArray[iRow, 7] = Convert.ToString(dt.Rows[iRow]["planweight"]);
                    strArray[iRow, 8] = string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[iRow]["begintime"])) == true ? string.Empty : StringTool.ToDayNightForSql(Convert.ToString(dt.Rows[iRow]["begintime"]));
                    strArray[iRow, 9] = string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[iRow]["endtime"])) == true ? string.Empty : StringTool.ToDayNightForSql(Convert.ToString(dt.Rows[iRow]["endtime"]));
                    strArray[iRow, 10] = Convert.ToString(dt.Rows[iRow]["carrier1"]);
                    strArray[iRow, 11] = Convert.ToString(dt.Rows[iRow]["carrier2"]);
                    strArray[iRow, 12] = string.Empty;
                    strArray[iRow, 13] = string.Empty;
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