//
//文件名：    UploadFile.aspx.cs
//功能描述：  上传文件(图片、音频等)
//创建时间：  2015/10/9
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
using System.IO;

namespace M_Lhgl.Service.Handover
{
    public partial class UploadFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //交接班令牌编码
            var codeToken = Request.Params["CodeToken"];
            //var fileName = Request.Params["xyz"];
            Stream stream = null;

            //codeToken = "2015420810424973958";
            try
            {
                if (codeToken == null)
                {
                    string warning = string.Format("参数CodeToken不能为null！举例：http://218.92.115.55/M_Lhgl/Service/Handover/UploadFile.aspx?CodeToken=201554080154189826");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;
                }
                if (Context.Request.Files.Count == 0)
                {
                    string warning = string.Format("服务器端接收文件流失败！");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;
                }

                for (int iFile = 0; iFile < Context.Request.Files.Count; iFile++)
                {
                    //文件
                    var file = Context.Request.Files[0];
                    //文件类型
                    var type = file.ContentType;
                    //文件名
                    var name = file.FileName;
                    //文件流
                    stream = file.InputStream;
                    //文件上传路径
                    string strFilePath = string.Empty;
                    //文件下载Url
                    string strFileUrl = string.Empty;
                    //文件类型
                    string strFileType = string.Empty;
                    //文件后缀
                    string strFileExt = string.Empty;
                    //type = "image/png";

                    Json = GetFileData(type, codeToken, ref strFileType, ref strFileExt, ref strFilePath, ref strFileUrl);
                    if (Json != string.Empty)
                    {
                        Json = JsonConvert.SerializeObject(new DicPackage(false, null, "获取文件数据失败！").DicInfo());
                        return;
                    };

                    //var strFileName = Guid.NewGuid().ToString("N") + strFileExt;
                    var strFileName = name + strFileExt;
                    //上传文件
                    if (!FileTool.UploadFile(strFilePath, strFileName, stream))
                    {
                        Json = JsonConvert.SerializeObject(new DicPackage(false, null, "上传失败！").DicInfo());
                        return;
                    }
                    stream.Dispose();

                    //保存文件路径
                    Json = SaveFilePath(codeToken, strFileType, strFileUrl, strFileName);
                    if (Json != string.Empty)
                    {
                        Json = JsonConvert.SerializeObject(new DicPackage(false, null, "文件路径保存失败！").DicInfo());
                        return;
                    }
                }
                Json = JsonConvert.SerializeObject(new DicPackage(true, null, "上传成功！").DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：提交数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }
        protected string Json;

        /// <summary>
        /// 获取文件数据
        /// </summary>
        /// <param name="strCodeToken">文件具体类型</param>
        /// <param name="strCodeToken">令牌编码</param>
        /// <param name="strFileType">文件类型</param>
        /// <param name="strFilePath">文件后缀名</param>
        /// <param name="strFileName">文件路径</param>
        /// <returns></returns>
        private string GetFileData(string strType, string strCodeToken, ref string strFileType, ref string strFileExt, ref string strFilePath, ref string strFileUrl)
        {
            string strJson = string.Empty;

            switch (strType)
            {
                case "image/jpeg":
                    strFileExt = ".jpg";
                    strFileType = "Pic";
                    break;
                case "image/gif":
                    strFileExt = ".gif";
                    strFileType = "Pic";
                    break;
                case "image/png":
                    strFileExt = ".png";
                    strFileType = "Pic";
                    break;

                case "audio/wav":
                    strFileExt = ".wav";
                    strFileType = "Voice";
                    break;
                case "audio/x-ms-wma":
                    strFileExt = ".wma";
                    strFileType = "Voice";
                    break;
                case "udio/scpls":
                    strFileExt = ".wma";
                    strFileType = "Voice";
                    break;
                case "audio/x-mei-aac":
                    strFileExt = ".acp";
                    strFileType = "Voice";
                    break;
                case "audio/aiff":
                    strFileExt = ".aiff";
                    strFileType = "Voice";
                    break;
                case "audio/basic":
                    strFileExt = ".au";
                    strFileType = "Voice";
                    break;
                case "audio/amr":
                    strFileExt = ".amr";
                    strFileType = "Voice";
                    break;
                default:
                    throw new Exception("错误对象索引！");
            }

            if (strFileType == "Pic")
            {
                strFilePath = FileTool.GetWebConfigKey("PicUploadPath") + strCodeToken + "\\";
                strFileUrl = FileTool.GetWebConfigKey("PicLoadUrl") + strCodeToken + "/";
                strFileType = "0";
            }
            else if (strFileType == "Voice")
            {
                strFilePath = FileTool.GetWebConfigKey("VoiceUploadPath") + strCodeToken + "\\";
                strFileUrl = FileTool.GetWebConfigKey("VoiceLoadUrl") + strCodeToken + "/";
                strFileType = "1";
            }

            return strJson;
        }

        /// <summary>
        /// 保存文件路径
        /// </summary>
        /// <param name="strCodeToken">令牌编码</param>
        /// <param name="strFileType">文件类型</param>
        /// <param name="strFilePath">文件下载Url</param>
        /// <param name="strFileUrl">文件名</param>
        /// <returns></returns>
        private string SaveFilePath(string strCodeToken, string strFileType, string strFileUrl, string strFileName)
        {
            string strJson = string.Empty;
            string strUrl = string.Empty;

            //获取已保存路径
            string strSql =
                string.Format("select * from TB_APP_GWTXC_HANDOVER_URL where CODE_TOKEN='{0}' and CODE_FILE='{1}'", strCodeToken, strFileName);
            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
            if (dt.Rows.Count < 0)
            {
                return strJson;
            }

            strUrl += strFileUrl + strFileName;
            strSql =
                    string.Format(@"insert into TB_APP_GWTXC_HANDOVER_URL (FILETYPE, CODE_TOKEN, CODE_FILE, FILEURL) 
                                    values('{0}','{1}','{2}','{3}')",
                                    strFileType, strCodeToken, strFileName, strUrl);
            new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteNonQuery(strSql);

            return strJson;
        }
    }
}