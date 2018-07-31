//
//文件名：    WorkHandover.aspx.cs
//功能描述：  工作交接提交
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
    public partial class WorkHandover : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //源用户编码（发送者）
            var codeUserFirst = Request.Params["CodeUserFirst"];
            //源公司编码（发送者）
            var codeCompanyFirst = Request.Params["CodeCompanyFirst"];
            //目的用户编码（接收者）
            var codeUserSecond = Request.Params["CodeUserSecond"];
            //目的公司编码（接收者）
            var codeCompanySecond = Request.Params["CodeCompanySecond"];
            ////应用名称
            //var appName = Request.Params["AppName"];
            //文本备注
            var text = Request.Params["Text"];
            //照片数量
            var picNum = Request.Params["PicNum"];
            //语音数量
            var voiceNum = Request.Params["VoiceNum"];


            text = text == null ? string.Empty : text;
            picNum = picNum == null ? "0" : picNum;
            voiceNum = voiceNum == null ? "0" : voiceNum;
           
            try
            {
                if (codeUserFirst == null || codeCompanyFirst == null || codeUserSecond == null || codeCompanySecond == null)
                {
                    string warning = string.Format("参数CodeUserFirst，CodeCompanyFirst，CodeUserSecond，CodeCompanySecond不能为null！举例：http://218.92.115.55/M_Lhgl/Service/Handover/WorkHandover.aspx?CodeUserFirst=227&CodeCompanyFirst=14&CodeUserSecond=336&CodeCompanySecond=14&Text=dddddd");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;
                }
            
                if (string.IsNullOrWhiteSpace(text) && picNum == "0" && voiceNum == "0")
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "提交数据为空！").DicInfo());
                    return;
                }

                Random rd = new Random();
                string strCodeToken = DateTime.Now.ToString("yyyyMMddHHmmssfff") +rd.Next(100000); 
                string strSql =
                    string.Format(@"insert into TB_APP_GWTXC_HANDOVER (CODE_TOKEN,CODE_USER_FIRST,CODE_COMPANY_FIRST,CODE_USER_SECOND,CODE_COMPANY_SECOND,TEXT,PICNUM,VOICENUM) 
                                    values('{0}','{1}','{2}','{3}','{4}','{5}',{6},{7})",
                                    strCodeToken, codeUserFirst, codeCompanyFirst, codeUserSecond, codeCompanySecond, text, picNum, voiceNum);
                new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteNonQuery(strSql);

                Json = JsonConvert.SerializeObject(new DicPackage(true, strCodeToken, null).DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：修改数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}

