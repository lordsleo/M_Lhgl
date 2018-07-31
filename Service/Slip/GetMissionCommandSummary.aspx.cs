//
//文件名：    GetMissionCommandSummary.aspx.cs
//功能描述：  获取派工指令概要数据
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
    public partial class GetMissionCommandSummary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //派工编码
            var pmno = Request.Params["Pmno"];
            //委托编码
            var cgno = Request.Params["Cgno"];
            //票货编码
            var gbno = Request.Params["Gbno"];

            try
            {
                if (pmno == null || cgno == null)
                {
                    string warning = string.Format("参数Pmno，Cgno，Gbno不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Slip/GetMissionCommandSummary.aspx?Pmno=20151010000161&Cgno=d1bff20fa2d54a0b87e4385a5cb46914");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;

                }
                string strSql =
                    string.Format(@"select taskno,cargo,planweight,carrier1,carrier2,operation 
                                    from VW_PS_MISSION_MA where pmno='{0}' and cgno='{1}'",
                                    pmno, cgno);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "派工编码或委托编码不存在！").DicInfo());
                    return;
                }

                string strCommandInfo = string.Empty;
                strCommandInfo += Convert.ToString(dt.Rows[0]["taskno"]) + "/";
                strCommandInfo += Convert.ToString(dt.Rows[0]["cargo"]) + "/";
                strCommandInfo += Convert.ToString(dt.Rows[0]["planweight"]) + "/";
                strCommandInfo += Convert.ToString(dt.Rows[0]["operation"]) + "/";
                strCommandInfo += Convert.ToString(dt.Rows[0]["carrier1"]) + "/";
                strCommandInfo += Convert.ToString(dt.Rows[0]["carrier2"]);

                string[] strNameArray = { "指令概要" };
                Dictionary<string, object> info = new Dictionary<string, object>();
                info.Add(strNameArray[0], strCommandInfo);

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