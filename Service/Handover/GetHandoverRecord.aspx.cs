//
//文件名：    GetHandoverRecord.aspx.cs
//功能描述：  获取交接班记录
//创建时间：  2015/10/1
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

namespace M_Lhgl.Service.Handover
{
    public partial class GetHandoverRecord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //当前时间
            var time = Request.Params["Time"];
            //用户编码
            var codeUser = Request.Params["CodeUser"];

            try
            {
                if (time == null || codeUser == null)
                {
                    string warning = string.Format("参数CurTime，CodeUser不能为null！举例：http://218.92.115.55/M_Lhgl/Service/Handover/GetHandoverRecord.aspx?Time=2015/12/16 08:44:52&CodeUser=16");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;
                }

                //time = "2015/12/16 08:44:52";

                //查询上传记录
                string strSql =
                    string.Format(@"select USERNAME1,USERNAME2,TEXT,ISREAD,CREATETIME,CODE_TOKEN 
                                    from (select b.USERNAME as USERNAME1,c.USERNAME as USERNAME2,a.TEXT,a.ISREAD,a.CREATETIME,a.CODE_TOKEN
                                    from TB_APP_GWTXC_HANDOVER a 
                                    left join iport.tb_sys_user b on a.CODE_USER_FIRST=b.CODE_USER 
                                    left join iport.tb_sys_user c on a.CODE_USER_SECOND=c.CODE_USER
                                    where a.CODE_USER_FIRST='{0}' or a.CODE_USER_SECOND='{1}') 
                                    where CREATETIME > to_date('{2}', 'yyyy/MM/dd hh24:mi:ss')
                                    order by CREATETIME desc",
                                    codeUser, codeUser, time);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "暂无数据！").DicInfo());
                    return;
                }

                Dictionary<string, object>[] dicArray = new Dictionary<string, object>[dt.Rows.Count];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("UserNameFirst", Convert.ToString(dt.Rows[iRow]["USERNAME1"]));
                    dic.Add("UserNameSecond", Convert.ToString(dt.Rows[iRow]["USERNAME2"]));
                    dic.Add("PICURL", GetUrlaArray("0", Convert.ToString(dt.Rows[iRow]["CODE_TOKEN"])));
                    dic.Add("VOICEURL", GetUrlaArray("1", Convert.ToString(dt.Rows[iRow]["CODE_TOKEN"])));
                    dic.Add("IsRead", Convert.ToString(dt.Rows[iRow]["ISREAD"]));
                    dic.Add("CreateTime", Convert.ToString(dt.Rows[iRow]["CREATETIME"]));
                    dic.Add("CodeToken", Convert.ToString(dt.Rows[iRow]["CODE_TOKEN"]));
                    dic.Add("Text", Convert.ToString(dt.Rows[iRow]["TEXT"]));
                    dicArray[iRow] = dic; 
                }

                Json = JsonConvert.SerializeObject(new DicPackage(true, dicArray, null).DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;

        /// <summary>
        /// 获取Url数组
        /// </summary>
        /// <param name="strFileType">文件类型</param>
        /// <param name="strCodeToken">记录令牌</param>
        /// <returns></returns>
        private string[] GetUrlaArray(string strFileType, string strCodeToken)
        {
            string strSql = string.Empty;
            string[] strArray = null;
            if (strFileType == "0")
            {
                strSql =
                    string.Format("select FILEURL from TB_APP_GWTXC_HANDOVER_URL where CODE_TOKEN='{0}' and FILETYPE='{1}'", strCodeToken, "0");

            }
            else if (strFileType == "1")
            {
                strSql =
                    string.Format("select FILEURL from TB_APP_GWTXC_HANDOVER_URL where CODE_TOKEN='{0}' and FILETYPE='{1}'", strCodeToken, "1");
            }
            var dt1 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
            if (dt1.Rows.Count > 0)
            {
                strArray = new string[dt1.Rows.Count];
                for (int iRow = 0; iRow < dt1.Rows.Count; iRow++)
                {
                    strArray[iRow] = Convert.ToString(dt1.Rows[iRow]["FILEURL"]);
                }
            }

            return strArray;
        }
    }
}