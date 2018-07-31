using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using ServiceInterface.Common;
using Leo;
using System.Web.Services.Protocols;

namespace M_Lhgl
{
    /// <summary>
    /// ServiceSCYWZH 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class ServiceSCYWZH : System.Web.Services.WebService
    {
        //定义用户身份验证类变量authHeader
        public AuthHeader authHeader;

        #region 计划任务
        /// <summary>
        /// 提交计划任务
        /// </summary>
        /// <param name="cryptogram">校验码</param>
        /// <param name="receiverId">接收着用户编码</param>
        /// <param name="senderId">发送者用户编码（0表示系统用户）</param>
        /// <param name="appName">应用名称</param>
        /// <param name="title">标题</param>
        /// <param name="content">消息内容</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        [WebMethod]
        [SoapHeader("authHeader")]
        public Package<bool> SubmitPlanTask(string receiverId, string senderId, string appName, string title, string message, string param)
        {
            try
            {
                //验证是否有权访问
                if (authHeader.ValideUser(authHeader.UserName, authHeader.PassWord))
                {
                    return new Package<bool>(true, "没有访问权限！", false);
                }

                string strSql =
                    string.Format("insert into TB_SCYWZH_PLAN_TASK (receiverid,senderid,appname,title,message,params) values('{0}','{1}','{2}','{3}','{4}','{5}')", receiverId, senderId, appName, title, message, param);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);

                return new Package<bool>(true, null, true);
            }
            catch (Exception ex)
            {
                return new Package<bool>(false, string.Format("{0}：修改数据发生异常。{1}", ex.Source, ex.Message), false);
            }
        }

        #endregion
    }
}
