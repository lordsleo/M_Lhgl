using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using ServiceInterface.Common;
using Leo;

namespace M_Lhgl.Service.Vehicle
{
    public partial class GetVehicleTransport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //车号
            var vehicleNum = Request.Params["VehicleNum"];

            try
            {
                if (vehicleNum == null)
                {
                    string warning = string.Format("参数vehicleNum不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Vehicle/GetVehicleTransport.aspx?vehicleNum=14");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;

                }

                string strSql = string.Format("select CODE_CLIENT,CARGO,VEHICLE,CODE_WORKTEAM,AMOUNT,GATE_RECORD_ID,audittime from(select CODE_CLIENT,CARGO,VEHICLE,CODE_WORKTEAM,AMOUNT,GATE_RECORD_ID,audittime from v_pda_pro_consignvehicle where  VEHICLE = '{0}' order by audittime desc ) where ROWNUM <=1", vehicleNum);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "车号或用户名不存在！").DicInfo());
                    return;
                }
                string[] strNameArray = { "货代", "货物", "车号", "班组", "件数", "记录"};
                //数组排序连接





                string strOrderLink = StringTool.ArrayToString(strNameArray);
                Dictionary<string, object> info = new Dictionary<string, object>();
                info.Add(strNameArray[0], Convert.ToString(dt.Rows[0]["CODE_CLIENT"]));
                info.Add(strNameArray[1], Convert.ToString(dt.Rows[0]["CARGO"]));
                info.Add(strNameArray[2], Convert.ToString(dt.Rows[0]["VEHICLE"]));
                info.Add(strNameArray[3], Convert.ToString(dt.Rows[0]["CODE_WORKTEAM"]));
                info.Add(strNameArray[4], Convert.ToString(dt.Rows[0]["AMOUNT"]));
                info.Add(strNameArray[5], Convert.ToString(dt.Rows[0]["GATE_RECORD_ID"]));
                info.Add("Order", strOrderLink);
                Json = JsonConvert.SerializeObject(new DicPackage(false, info, null).DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
             
        }
        protected string Json;
    }
}