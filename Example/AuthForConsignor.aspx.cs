//
//文件名：    AuthForConsignor.aspx.cs
//功能描述：  货主认证
//创建时间：  2015/09/09
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

namespace M_Lhgl.Example
{
    public partial class AuthForConsignor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //用户编码
            var codeUser = Request.Params["CodeUser"];
            //真实姓名
            var userName = Request.Params["UserName"];
            //身份证号
            var identityCard = Request.Params["IdentityCard"];

            try
            {
                if (codeUser == null || userName == null || identityCard == null)
                {
                    string warning = string.Format("参数CodeUser，UserName，IdentityCard不能为nul！举例：http://218.92.115.55/M_Sph/Auth/AuthForConsignor.aspx?CodeUser=1DA60DDD8025725AE053A864016A725A&UserName=张三&IdentityCard=111111111111111111");
                    Json = JsonConvert.SerializeObject(new DicPackage(warning).FalseDic());
                    return;
                }

                //身份证验证
                if (!TokenTool.CheckIDCard(identityCard))
                {
                    Json = JsonConvert.SerializeObject(new DicPackage("身份证号码错误！").FalseDic());
                    return;
                }

                string strSql =
                    string.Format("select authstate from TB_SPH_USER_AUTH where code_user='{0}' and roletype='{1}'", codeUser, 2);
                //验证此会员是否已认证
                var dt = new Leo.Oracle.DataAccess(Leo.RegistryKey.KeyPathWlxgx).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    //添加认证数据
                    strSql =
                        string.Format("insert into TB_SPH_USER_AUTH (code_user,username,identity_card,roletype,authstate) values('{0}','{1}','{2}','{3}','{4}')", codeUser, userName, identityCard, "2", "0");
                }
                else
                {
                    //更新认证数据
                    strSql =
                        string.Format("update TB_SPH_USER_AUTH set username='{0}',identity_card='{1}',authstate='{2}',operatetime=to_date('{3}','YYYY-MM-DD HH24:MI:SS') where  code_user='{4}' and roletype={5}", userName, identityCard, "1", DateTime.Now, codeUser, 2);
                }

                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWlxgx).ExecuteTable(strSql);
                //检查数据是否插入/更新成功  
                strSql =
                    string.Format("select * from TB_SPH_USER_AUTH where  code_user='{0}' and roletype='{1}'", codeUser, "2");
                dt = new Leo.Oracle.DataAccess(Leo.RegistryKey.KeyPathWlxgx).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage("认证失败，请稍后重试！").FalseDic());
                }
                else
                {
                    Json = JsonConvert.SerializeObject(new DicPackage("审核中，请耐心等待！").TrueDic());
                }
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(string.Format("{0}：认证数据发生异常。{1}", ex.Source, ex.Message)).FalseDic());
            }

        }
        protected string Json;
    }
}