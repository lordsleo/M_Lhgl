//
//文件名：    GetWeighting.aspx.cs
//功能描述：  获取衡重查询列表数据
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

namespace M_Lhgl.Service.Weighing
{
    public partial class GetWeighting : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //数据起始行
            var startRow = Request.Params["StartRow"];
            //行数
            var count = Request.Params["Count"];
            //公司编码
            var codeDepartment = Request.Params["CodeDepartment"];
            //白夜班
            var dayNight = Request.Params["DayNight"];
            //班组日期
            var teamDate = Request.Params["TeamDate"];

            codeDepartment = "11";
            //startRow = "1";
            //count = "30";
            //dayNight = "白班";
            //teamDate = "2015-09-22";

            try
            {
                if (startRow == null || count == null || codeDepartment == null || dayNight == null || teamDate == null)
                {
                    string warning = string.Format("参数StartRow，Count，CodeDepartment，TeamDate不能为null！举例：http://218.92.115.55/M_Lhgl/Service/Weighing/GetWeighting.aspx?StartRow=1&Count=14&CodeDepartment=11&DayNight=白班&TeamDate=2015-08-19");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;
                }

                // 每日过磅统计。
                string VwDailyCount = "select consign,department,departmentname,client,ship,billnumber1,cargo,weight,sum(weight2-weight1) netweight,count(truck) trucknumber from BALANCECENTER..vw_metages ";
                //获取班次统计时间
                string strSql = string.Format("select DayTime,NightTime from BALANCECENTER..RunConfig where Department = {0}", codeDepartment);
                var dt = new Leo.SqlServer.DataAccess(RegistryKey.KeyPathBc).ExecuteTable(strSql);
                string day = " 06:00";
                string night = " 17:00";
                if (dt.Rows.Count > 0)
                {
                    day = StringTool.ToDayNightForSql(dt.Rows[0]["DayTime"].ToString());
                    night = StringTool.ToDayNightForSql(dt.Rows[0]["NightTime"].ToString());
                }
                //拼接查询条件          
                string strFilter;
                if (dayNight == "白班")//
                {
                    strFilter =
                        string.Format(
                            "department='{0}' and FinishTime is not null and finishtime<'{1}' and finishtime>='{2}' ",
                            codeDepartment,
                            string.Concat(Convert.ToDateTime(teamDate).ToShortDateString(), night),
                            string.Concat(Convert.ToDateTime(teamDate).ToShortDateString(), day));
                }
                else
                {
                    strFilter =
                        string.Format(
                            "department='{0}' and FinishTime is not null and ((finishtime>='{1}' and finishtime<='{2}') or (finishtime>='{3}' and finishtime<'{4}')) ",
                            codeDepartment,
                            string.Concat(Convert.ToDateTime(teamDate).ToShortDateString(), night),
                            string.Concat(Convert.ToDateTime(teamDate).AddDays(1).ToShortDateString(), " 00:00"),
                            string.Concat(Convert.ToDateTime(teamDate).AddDays(1).ToShortDateString(), " 00:00"),
                            string.Concat(Convert.ToDateTime(teamDate).AddDays(1).ToShortDateString(), day));
                }

                //获取当班数据、累计数据  拼入一个DataTab
                strSql =
                    string.Format(
                        "{0} where {1} group by consign,department,departmentname,client,ship,billnumber1,cargo,weight ",
                        VwDailyCount, strFilter);
                dt = new Leo.SqlServer.DataAccess(RegistryKey.KeyPathBc).ExecuteTable(strSql, Convert.ToInt32(startRow), Convert.ToInt32(startRow) + Convert.ToInt32(count) - 1);

                string[,] strArray = new string[dt.Rows.Count, 6];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    strArray[iRow, 0] = Convert.ToString(dt.Rows[iRow]["consign"]);
                    strArray[iRow, 1] = Convert.ToString(dt.Rows[iRow]["ship"]);
                    strArray[iRow, 2] = Convert.ToString(dt.Rows[iRow]["client"]);
                    strArray[iRow, 3] = Convert.ToString(dt.Rows[iRow]["cargo"]);
                    strArray[iRow, 4] = Convert.ToString(dt.Rows[iRow]["weight"]);
                    strArray[iRow, 5] = Convert.ToString(dt.Rows[iRow]["netweight"]);
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