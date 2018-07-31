//
//文件名：    GetOneHandoverRecord.aspx.cs
//功能描述：  获取指定交接班记录
//创建时间：  2015/12/23
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
    public partial class GetOneHandoverRecord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //交接班记录令牌
            var codeToken = Request.Params["CodeToken"];

            try
            {
                if (codeToken == null)
                {
                    string warning = string.Format("参数CodeToken不能为null！举例：http://218.92.115.55/M_Lhgl/Service/Handover/GetOneHandoverRecord.aspx?CodeToken=201550230250548004");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;
                }

                //查询上传记录
                string strSql =
                    string.Format(@"select b.USERNAME as USERNAME1,c.USERNAME as USERNAME2,a.TEXT,a.ISREAD,a.CREATETIME,a.CODE_TOKEN,a.TEXT
                                    from TB_APP_GWTXC_HANDOVER a 
                                    left join iport.tb_sys_user b on a.CODE_USER_FIRST=b.CODE_USER 
                                    left join iport.tb_sys_user c on a.CODE_USER_SECOND=c.CODE_USER
                                    where a.CODE_TOKEN='{0}'",
                                    codeToken);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "暂无数据！").DicInfo());
                    return;
                }

                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("UserNameFirst", Convert.ToString(dt.Rows[0]["USERNAME1"]));
                dic.Add("UserNameSecond", Convert.ToString(dt.Rows[0]["USERNAME2"]));
                dic.Add("PICURL", GetUrlaArray("0", codeToken));
                dic.Add("VOICEURL", GetUrlaArray("1", codeToken));
                dic.Add("IsRead", Convert.ToString(dt.Rows[0]["ISREAD"]));
                dic.Add("CreateTime", Convert.ToString(dt.Rows[0]["CREATETIME"]));
                dic.Add("Text", Convert.ToString(dt.Rows[0]["TEXT"]));

                Json = JsonConvert.SerializeObject(new DicPackage(true, dic, null).DicInfo());
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