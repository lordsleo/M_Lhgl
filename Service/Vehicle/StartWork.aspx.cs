//
//文件名：    StartWork.aspx.cs
//功能描述：  开工
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
    public partial class StartWork : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ID
            var id = Request.Params["Id"];
            //开工记录人
            var startUserName = Request.Params["StartUserName"];
            //开工时间
            var startTime = Request.Params["StartTime"];

            try
            {
                if (id == null || startUserName == null || startTime == null)
                {
                    string warning = string.Format("参数Id,StartUserName，StartTime不能为null！举例：http://218.92.115.55/M_Lhgl/Service/Vehicle/StartWork.aspx?Id=14&startUserName=张三&startTime=18:27:55");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;
                }

                startTime = DateTime.Now.ToShortDateString() + " " + startTime;
                string strSql = 
                    string.Format("select id from tb_pro_consignvehicle where id='{0}'", id);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "ID不存在！").DicInfo());
                    return;
                }
     
                strSql = 
                    string.Format(@"update tb_pro_consignvehicle 
                                    set START_USERNAME='{0}' ,START_TIME=to_date('{1}', 'yyyy-mm-dd hh24:mi:ss') 
                                    where id='{2}'", 
                                    startUserName, startTime, id);
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