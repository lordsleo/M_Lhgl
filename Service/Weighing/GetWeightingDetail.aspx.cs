//
//文件名：    GetWeightingDetail.aspx.cs
//功能描述：  获取衡重查询明细数据
//创建时间：  2015/09/22
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
    public partial class GetWeightingDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //委托编码
            var cgno = Request.Params["Cgno"];
            //公司编码
            var codeDepartment = Request.Params["CodeDepartment"];
            //白夜班
            var dayNight = Request.Params["DayNight"];
            //班组日期
            var teamDate = Request.Params["TeamDate"];

            //cgno = "0114111611";
            codeDepartment = "11";
            //dayNight = "白班";
            //teamDate = "2015-09-22";       

            try
            {
                if (cgno == null || codeDepartment == null || dayNight == null || teamDate == null)
                {
                    string warning = string.Format("参数Cgno，CodeDepartment，TeamDate不能为null！举例：http://218.92.115.55/M_Lhgl/Service/Weighing/GetWeighting.aspx?Cgno=0114111611&CodeDepartment=11&DayNight=白班&TeamDate=2015-08-19");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;
                }

                // 每日过磅统计。
                string VwDailyCount = "select consign,department,departmentname,client,ship,billnumber1,cargo,weight,sum(weight2-weight1) netweight,count(truck) trucknumber from BALANCECENTER..vw_metages ";
                // 每日过磅统计(累计)。
                string VwDailyCount2 = " select consign,sum(weight2-weight1) netweight,count(truck) trucknumber from BALANCECENTER..vw_metages ";
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
                            "department='{0}' and FinishTime is not null and finishtime<'{1}' and finishtime>='{2}' and  consign='{3}'",
                            codeDepartment,
                            string.Concat(Convert.ToDateTime(teamDate).ToShortDateString(), night),
                            string.Concat(Convert.ToDateTime(teamDate).ToShortDateString(), day),
                            cgno);
                              
                }
                else
                {
                    strFilter =
                        string.Format(
                            "department='{0}' and FinishTime is not null and ((finishtime>='{1}' and finishtime<='{2}') or (finishtime>='{3}' and finishtime<'{4}')) and  consign='{5}'",
                            codeDepartment,
                            string.Concat(Convert.ToDateTime(teamDate).ToShortDateString(), night),
                            string.Concat(Convert.ToDateTime(teamDate).AddDays(1).ToShortDateString(), " 00:00"),
                            string.Concat(Convert.ToDateTime(teamDate).AddDays(1).ToShortDateString(), " 00:00"),
                            string.Concat(Convert.ToDateTime(teamDate).AddDays(1).ToShortDateString(), day),
                            cgno);
                }
                strSql =
                    string.Format(
                        "{0} where {1} group by consign,department,departmentname,client,ship,billnumber1,cargo,weight",
                        VwDailyCount, strFilter);
                var dt0 = new Leo.SqlServer.DataAccess(RegistryKey.KeyPathBc).ExecuteTable(strSql);

                strSql = string.Format(" {0} where consign='{1}' group by consign ", VwDailyCount2, cgno);
                var dt1 = new Leo.SqlServer.DataAccess(RegistryKey.KeyPathBc).ExecuteTable(strSql);
                if (dt1.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "委托号不存在！").DicInfo());
                    return;
                }

                string[] strNameArray = { "委托号", "船名", "委托人", "货种", "计划重量", 
                                          "当班量", "累计量","当班车次","累计车次",
                                          "提单号", "公司", "班组日期", "白夜班"};
                //数组排序连接
                string strOrderLink = StringTool.ArrayToString(strNameArray);

                Dictionary<string, object> info = new Dictionary<string, object>();
                info.Add(strNameArray[0], Convert.ToString(dt0.Rows[0]["consign"]));
                info.Add(strNameArray[1], Convert.ToString(dt0.Rows[0]["ship"]));
                info.Add(strNameArray[2], Convert.ToString(dt0.Rows[0]["client"]));
                info.Add(strNameArray[3], Convert.ToString(dt0.Rows[0]["cargo"]));
                info.Add(strNameArray[4], Convert.ToString(dt0.Rows[0]["weight"]));
                info.Add(strNameArray[5], Convert.ToString(dt0.Rows[0]["netweight"]));
                info.Add(strNameArray[6], Convert.ToString(dt1.Rows[0]["NETWEIGHT"]));
                info.Add(strNameArray[7], Convert.ToString(dt0.Rows[0]["trucknumber"]));
                info.Add(strNameArray[8], Convert.ToString(dt1.Rows[0]["TRUCKNUMBER"]));
                info.Add(strNameArray[9], Convert.ToString(dt0.Rows[0]["billnumber1"]));
                info.Add(strNameArray[10], Convert.ToString(dt0.Rows[0]["departmentname"]));
                info.Add(strNameArray[11], teamDate);
                info.Add(strNameArray[12], dayNight);
                info.Add("Order", strOrderLink);

                Json = JsonConvert.SerializeObject(new DicPackage(true, info, null).DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}