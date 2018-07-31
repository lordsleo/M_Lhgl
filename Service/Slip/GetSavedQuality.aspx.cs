//
//文件名：    GetSavedQuality.aspx.cs
//功能描述：  获取暂存质量
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
    public partial class GetSavedQuality : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //理货单编码
            var tbno = Request.Params["Tbno"];

            try
            {
                if (tbno == null)
                {
                    string warning = string.Format("参数Tbno不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Slip/GetSavedQuality.aspx?Tbno=20151028063933");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;

                }

                string strSpecialMark = string.Empty;
                string strSql =
                    string.Format(@"select case when CODE_QUALITY='01' then '合格' else '不合格' end quality 
                                    from VW_REG_TALLYBILL 
                                    where tbno='{0}'",
                                    tbno);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count > 0)
                {
                    strSpecialMark = Convert.ToString(dt.Rows[0]["quality"]);
                }

                Dictionary<string, object> info = new Dictionary<string, object>();
                info.Add("Quality", strSpecialMark);

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