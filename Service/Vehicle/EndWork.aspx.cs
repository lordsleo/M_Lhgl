//
//文件名：    EndWork.aspx.cs
//功能描述：  完工
//创建时间：  2015/09/28
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

namespace M_Lhgl.Service.Vehicle
{
    public partial class EndWork : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ID
            var id = Request.Params["Id"];
            //完工记录人
            var endUserName = Request.Params["EndUserName"];
            //完工时间
            var endTime = Request.Params["endTime"];
            //班组编码
            var codeWorkTeam = Request.Params["CodeWorkTeam"];
            //白夜班
            var dayNight = Request.Params["DayNight"];
            //件数
            var count = Request.Params["Count"];

            try
            {
                if (id == null || endUserName == null || endTime == null || codeWorkTeam == null || dayNight == null || count == null)
                {
                    string warning = string.Format("参数Id,EndUserName，endTime，CodeWorkTeam，DayNight，Count不能为null！举例：http://218.92.115.55/M_Lhgl/Service/Vehicle/EndWork.aspx?Id=14&EndUserName=张三&EndTime=8:27:55&CodeWorkTeam=7&DayNight=白班&Count=");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;
                }

                endTime = DateTime.Now.ToShortDateString() + " " + endTime;
                string strSql =
                    string.Format("select id,start_time from tb_pro_consignvehicle where id='{0}'", id);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "Id不存在！").DicInfo());
                    return;
                }
                if (string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["start_time"])))
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "请先做开工记录！").DicInfo());
                    return;
                }

                string codeDayNight = dayNight=="白班"? "0":"1";
                strSql =
                    string.Format(@"update tb_pro_consignvehicle 
                                   set COMPLETE_USERNAME='{0}' ,COMPLETE_TIME= to_date('{1}', 'yyyy-mm-dd hh24:mi:ss') ,code_workteam='{2}', code_daynight='{3}', amount='{4}' ,workstyle='1' 
                                   where id='{5}'", 
                                   endUserName, endTime, codeWorkTeam, codeDayNight, count, id);
                new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteNonQuery(strSql);
                Json = JsonConvert.SerializeObject(new DicPackage(true, null, "提交成功！").DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：修改数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}