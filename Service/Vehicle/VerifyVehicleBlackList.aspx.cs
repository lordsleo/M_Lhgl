//
//文件名：    VerifyVehicleBlackList.aspx.cs
//功能描述：  校验车辆是否黑名单
//创建时间：  2015/10/22
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

namespace M_Lhgl.Service.Vehicle
{
    public partial class VerifyVehicleBlackList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //号码（通行证号/NFC卡号）
            var no = Request.Params["No"];
            //识别方式
            var recognizeMethod = Request.Params["RecognizeMethod"];

            try
            {
                if (no == null || recognizeMethod == null)
                {
                    string warning = string.Format("参数No,RecognizeMethod不能为null！举例：http://218.92.115.55/M_Lhgl/Service/Vehicle/VerifyVehicleBlackList.aspx?No=690000&RecognizeMethod=CARD");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;
                }

                //号码字段名称
                string strNoFieldName = string.Empty;

                //号码字段名称
                switch (recognizeMethod)
                {
                    case "CARD":
                        strNoFieldName = "EXTER_NO";
                        break;
                    case "NFC":
                        strNoFieldName = "PARK_CARD_NO";
                        break;
                    default:
                        throw new Exception("错误的对象索引");
                }

                //校验状态：黑名单无效，卡状态不是在用无效，卡被禁用无效，车状态不在港内无效
                string strSql =
                    string.Format("select blacklist from TRANSIT.V_VEH_CARD_HARBOR where {0}='{1}'", strNoFieldName, no.ToUpper());
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "此卡未办理！").DicInfo());
                    return;
                }
                if (Convert.ToString(dt.Rows[0]["blacklist"]) == "1")
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(true, null, "此车黑名单！").DicInfo());
                    return;
                }

                Json = JsonConvert.SerializeObject(new DicPackage(true, null, null).DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}