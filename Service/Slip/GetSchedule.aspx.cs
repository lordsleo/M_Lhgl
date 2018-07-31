//
//文件名：    GetSchedule.aspx.cs
//功能描述：  获取班次
//创建时间：  2015/10/014
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
    public partial class GetSchedule : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //公式编码
            var codeCompany = Request.Params["CodeCompany"];

            try
            {
                if (codeCompany == null)
                {
                    string warning = string.Format("参数CodeCompany不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Slip/GetSchedule.aspx?CodeCompany=14");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;
                }

                string strTakeDate = string.Empty;
                string strWorkTime = string.Empty;
                string strSql =
                        string.Format(@"select to_char(c_takedate, 'yyyy-mm-dd') as c_takedate ,c_worktime
                                        from TB_PS_RUNCONFIG  
                                        where code_company='{0}'",
                                        codeCompany);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "公司编码不存在！").DicInfo());
                    return;
                }

                strTakeDate = Convert.ToString(dt.Rows[0]["c_takedate"]);
                strWorkTime = Convert.ToString(dt.Rows[0]["c_worktime"]) == "1" ? "白班" : "夜班";
                Dictionary<string, object> info = new Dictionary<string, object>();
                info.Add("TakeDate", strTakeDate);
                info.Add("WorkTime", strWorkTime); 

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