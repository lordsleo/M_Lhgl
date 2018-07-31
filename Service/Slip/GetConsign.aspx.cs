//
//文件名：    GetConsign.aspx.cs
//功能描述：  获取委托数据
//创建时间：  2015/10/02
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
    public partial class GetConsign : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //委托编码
            var cgno = Request.Params["Cgno"];
            //公司编码
            var codeCompany = Request.Params["CodeCompany"];
 
            try
            {
                if (cgno == null || codeCompany == null)
                {
                    string warning = string.Format("参数Cgno不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Slip/GetConsign.aspx?Cgno=14&CodeCompany=14");
                    Json = JsonConvert.SerializeObject(new DicPackage(warning).FalseDic());
                    return;

                }
                string strSql =
                    string.Format(@"select cgno,gbdisplay GBDISPLAY,gbnos GBNO,MARK_EXCHANGE,gbno HZGOODSBILL,MARK_LAST,CODE_CARRIER_S,CODE_CARRIER_E,code_storage,code_storagelast   
                                    from   vw_ps_mission_yardplan1 
                                    where code_company='{0}' and cgno='{1}'",
                                    codeCompany, cgno);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage("委托编码不存在！").FalseDic());
                    return;
                }

                string strBillInfo = string.Empty;
                strBillInfo += Convert.ToString(dt.Rows[0]["gbdisplay"]) + "/";
                strBillInfo += Convert.ToString(dt.Rows[0]["GBDISPLAY"]) + "/";
                strBillInfo += Convert.ToString(dt.Rows[0]["gbnos"]) + "/";
                strBillInfo += Convert.ToString(dt.Rows[0]["GBNO"]) + "/";
                strBillInfo += Convert.ToString(dt.Rows[0]["MARK_EXCHANGE"]) + "/";
                strBillInfo += Convert.ToString(dt.Rows[0]["gbno"]) + "/";
                strBillInfo += Convert.ToString(dt.Rows[0]["HZGOODSBILL"]) + "/";
                strBillInfo += Convert.ToString(dt.Rows[0]["MARK_LAST"]);

                string[] strNameArray = { "委托编码", "委托"};
                Dictionary<string, object> info = new Dictionary<string, object>();
                info.Add(strNameArray[0], Convert.ToString(dt.Rows[0]["cgno"]));
                info.Add(strNameArray[1], strBillInfo);

                Json = JsonConvert.SerializeObject(new DicPackage().TrueDic(strBillInfo));
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(string.Format("{0}：获取数据发生异常。{1}", ex.Source, ex.Message)).FalseDic());
            }
        }
        protected string Json;
    }
}