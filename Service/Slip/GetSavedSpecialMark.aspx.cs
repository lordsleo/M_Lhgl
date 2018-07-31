//
//文件名：    GetSavedSpecialMark.aspx.cs
//功能描述：  获取暂存子过程标志
//创建时间：  2015/11/16
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
    public partial class GetSavedSpecialMark : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //理货单编码
            var tbno = Request.Params["Tbno"];

            try
            {
                if (tbno == null)
                {
                    string warning = string.Format("参数Tbno不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Slip/GetSavedSpecialMark.aspx?Tbno=20151028063933");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;

                }

                string[,] strArray = new string[1, 3];
                string strSql =
                    string.Format(@"select b.CODE_SPECIALMARK,b.SPECIALMARK,b.LOGOGRAM
                                    from VW_REG_TALLYBILL a,baseresource.TB_CODE_SPECIALMARK b  
                                    where a.tbno='{0}' and a.code_specialmark=b.code_specialmark", 
                                    tbno);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count > 0)
                {
                    strArray[0, 0] = Convert.ToString(dt.Rows[0]["CODE_SPECIALMARK"]);
                    strArray[0, 1] = Convert.ToString(dt.Rows[0]["SPECIALMARK"]);
                    strArray[0, 2] = Convert.ToString(dt.Rows[0]["LOGOGRAM"]);
                }
                else
                {
                    strArray[0, 0] = string.Empty;
                    strArray[0, 1] = string.Empty;
                    strArray[0, 2] = string.Empty;
                }

                Json = JsonConvert.SerializeObject(new DicPackage(true, strArray, null).DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}