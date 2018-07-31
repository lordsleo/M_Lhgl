//
//文件名：    GetMissionPlanDetail.aspx.cs
//功能描述：  获取派工明细数据
//创建时间：  2015/10/01
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
    public partial class GetMissionPlanDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //委托编码
            var cgno = Request.Params["Cgno"];
            cgno = "14";

            try
            {
                if (cgno == null)
                {
                    string warning = string.Format("参数Cgno不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Slip/GetMissionPlanDetail.aspx?Cgno=14");
                    Json = JsonConvert.SerializeObject(new DicPackage(warning).FalseDic());
                    return;

                }
                string strSql =
                    string.Format(@"select cargo,cargoowner,operation_fact,planweight 
                                    from vw_ps_mission_yardplan1 where cgno='{0}'",
                                    cgno);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage("委托编码不存在！").FalseDic());
                    return;
                }

                string strBillInfo = string.Empty;
                strBillInfo += Convert.ToString(dt.Rows[0]["cargo"]) + "/";
                strBillInfo += Convert.ToString(dt.Rows[0]["cargoowner"]) + "/";
                strBillInfo += Convert.ToString(dt.Rows[0]["operation_fact"]) + "/";
                strBillInfo += Convert.ToString(dt.Rows[0]["planweight"]);

                string[] strNameArray = { "销账票货" };
                Dictionary<string, object> info = new Dictionary<string, object>();
                info.Add(strNameArray[0], strBillInfo);

                Json = JsonConvert.SerializeObject(new DicPackage().TrueDic(info));
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(string.Format("{0}：获取数据发生异常。{1}", ex.Source, ex.Message)).FalseDic());
            }
        }
        protected string Json;
    }
}