//
//文件名：    GetCodeCarries.aspx.cs
//功能描述：  获取载货工具编码（源、目的）
//创建时间：  2015/09/18
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
    public partial class GetCodeCarries : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //子过程标志编码
            var codeOperationFact = Request.Params["CodeOperationFact"];

            try
            {
                if (codeOperationFact == null)
                {
                    string warning = string.Format("参数CodeOperationFact不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Slip/GetCodeCarries.aspx?CodeOperationFact=24");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;

                }

                string strSql =
                        string.Format(@"select CODE_CARRIER_S,CODE_CARRIER_E  
                                        from nharbor.vw_br_operationfact  
                                        where code_operation_fact='{0}'",
                                        codeOperationFact);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "派工编码不存在！").DicInfo());
                    return;
                }             

                Dictionary<string, object> info = new Dictionary<string, object>();
                info.Add("CodeCarriesS", Convert.ToString(dt.Rows[0]["CODE_CARRIER_S"]));
                info.Add("CodeCarriesE", Convert.ToString(dt.Rows[0]["CODE_CARRIER_E"]));

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