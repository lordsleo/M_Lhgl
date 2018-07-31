//
//文件名：    GetStartWork.aspx.cs
//功能描述：  获取开工数据
//创建时间：  2015/09/24
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
    public partial class GetWork : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //号码（通行证号/NFC卡号）
            var no = Request.Params["No"];
            //公司编码
            var codeCompany = Request.Params["CodeCompany"];
            //识别方式
            var recognizeMethod = Request.Params["RecognizeMethod"];

            try
            {
                if (no == null || codeCompany == null || recognizeMethod == null)
                {
                    string warning = string.Format("参数No,CodeCompany，RecognizeMethod不能为null！举例：http://218.92.115.55/M_Lhgl/Service/Vehicle/GetStartWork.aspx?No=690000&CodeCompany=77&RecognizeMethod=CARD");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;
                }

                //号码字段名称
                string strNoFieldName = string.Empty;

                //黑名单
                string strBlackList = string.Empty;
                //卡状态
                string strState = string.Empty;
                //卡禁用
                string strForbidMark = string.Empty;
                //车状态
                string strVehState = string.Empty;
                //车牌号
                string strVehicle = string.Empty;
                //卡号
                string cardNo = string.Empty;

                //场地
                string strStorage = string.Empty;
                //货位
                string strBooth = string.Empty;
                //ID
                string strId = string.Empty;
                //内部委托号
                string strCgno = string.Empty;

                //任务号
                string strTaskno = string.Empty;
                //货物
                string strCargo = string.Empty;
                //船名
                string strVessel = string.Empty;
                //货代
                string strClient = string.Empty;
                //集疏港
                string strFullOrEmpty = string.Empty;
                //装卸货
                string strWorkStyle = string.Empty;

                //衡重
                string strWeight = string.Empty;
                //申报时间
                string strSubmittime = string.Empty;

                string[] strNameArray = { "ID", "车号", "船名", "货代", "货物", "场地", "货位", "集疏港", "装卸车", "任务号", "通行证号", "申报时间", "衡重" };
                Dictionary<string, object> info = new Dictionary<string, object>();

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
                    string.Format("select blacklist,state,forbid_mark,veh_state,vehicle,exter_no,card_no from TRANSIT.V_VEH_CARD_HARBOR where {0}='{1}'", strNoFieldName, no.ToUpper());
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "此卡无效！").DicInfo());
                    return;
                }
                strBlackList = Convert.ToString(dt.Rows[0]["blacklist"]);
                strState = Convert.ToString(dt.Rows[0]["state"]);
                strForbidMark = Convert.ToString(dt.Rows[0]["forbid_mark"]);
                strVehState = Convert.ToString(dt.Rows[0]["veh_state"]);
                strVehicle = Convert.ToString(dt.Rows[0]["vehicle"]);
                no = Convert.ToString(dt.Rows[0]["exter_no"]);
                cardNo = Convert.ToString(dt.Rows[0]["card_no"]);

                if (strState != "1")
                {
                    info = GetAllInfo(strNameArray, strId, strVehicle, strVessel, strClient, strCargo, strStorage, strBooth, strFullOrEmpty, strWorkStyle, strTaskno, no, strSubmittime, strWeight);
                    Json = JsonConvert.SerializeObject(new DicPackage(false, info, "此卡当前状态无用！").DicInfo());
                    return;
                }
                if (strForbidMark == "1")
                {
                    info = GetAllInfo(strNameArray, strId, strVehicle, strVessel, strClient, strCargo, strStorage, strBooth, strFullOrEmpty, strWorkStyle, strTaskno, no, strSubmittime, strWeight);
                    Json = JsonConvert.SerializeObject(new DicPackage(false, info, "此卡禁用！").DicInfo());
                    return;
                }
                if (strVehState != "1")
                {
                    info = GetAllInfo(strNameArray, strId, strVehicle, strVessel, strClient, strCargo, strStorage, strBooth, strFullOrEmpty, strWorkStyle, strTaskno, no, strSubmittime, strWeight);
                    Json = JsonConvert.SerializeObject(new DicPackage(false, info, "此车不在港内！").DicInfo());
                    return;
                }

                //校验状态：未放行无效
                strSql =
                    string.Format(@"select id,audit_mark,storage,booth,cgno,to_char(submittime, 'yyyy-MM-dd HH24:mi :ss') as submittime 
                                   from Harbor.V_CONSIGN_VEHICLE_ONLY_QUICK 
                                   where submittime>sysdate-1 and code_department='{0}' and exter_no='{1}' and vehicle='{2}' 
                                   order by submittime desc",
                                   codeCompany, no, strVehicle);
                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0 || Convert.ToString(dt.Rows[0]["audit_mark"]) != "1")
                {
                    info = GetAllInfo(strNameArray, strId, strVehicle, strVessel, strClient, strCargo, strStorage, strBooth, strFullOrEmpty, strWorkStyle, strTaskno, no, strSubmittime, strWeight);
                    Json = JsonConvert.SerializeObject(new DicPackage(false, info, "此车未放行！").DicInfo());
                    return;
                }
                strStorage = Convert.ToString(dt.Rows[0]["storage"]);
                strBooth = Convert.ToString(dt.Rows[0]["booth"]);
                strId = Convert.ToString(dt.Rows[0]["id"]);
                strCgno = Convert.ToString(dt.Rows[0]["cgno"]);
                strSubmittime = Convert.ToString(dt.Rows[0]["SUBMITTIME"]);

                //对于东联的申报，还要检查是否通过车辆检查
                if (codeCompany == "77")
                {
                    strSql =
                        string.Format(@"select check_mark  
                                    from CGATE.v_cgate_record_cache t 
                                    where pass_time>sysdate-1 and code_company='{0}' and exter_no='{1}' and vehicle='{2}' and inout_mark='0' 
                                    order by pass_time desc",
                                    codeCompany, no, strVehicle);
                    dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathHarbor).ExecuteTable(strSql);
                    if (dt.Rows.Count <= 0 || string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["check_mark"])))
                    {
                        info = GetAllInfo(strNameArray, strId, strVehicle, strVessel, strClient, strCargo, strStorage, strBooth, strFullOrEmpty, strWorkStyle, strTaskno, no, strSubmittime, strWeight);
                        Json = JsonConvert.SerializeObject(new DicPackage(false, info, "此车未进门！").DicInfo());
                        return;
                    }
                    if (Convert.ToString(dt.Rows[0]["check_mark"]) != "1")
                    {
                        info = GetAllInfo(strNameArray, strId, strVehicle, strVessel, strClient, strCargo, strStorage, strBooth, strFullOrEmpty, strWorkStyle, strTaskno, no, strSubmittime, strWeight);
                        Json = JsonConvert.SerializeObject(new DicPackage(false, info, "此车未通过检查！").DicInfo());
                        return;
                    }
                }

                //校验：过磅记录
                strSql =
                    string.Format(@"select weight1,weight2,weightcargo
                                    from BALANCECENTER..V_MetageForComm 
                                    where RecordTime>getdate()-1 and CardNo='{0}' and Truck='{1}' order by RecordTime desc ",
                                    cardNo, strVehicle);
                dt = new Leo.SqlServer.DataAccess(RegistryKey.KeyPathBc).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    info = GetAllInfo(strNameArray, strId, strVehicle, strVessel, strClient, strCargo, strStorage, strBooth, strFullOrEmpty, strWorkStyle, strTaskno, no, strSubmittime, strWeight);
                    Json = JsonConvert.SerializeObject(new DicPackage(false, info, "此车无过磅记录！").DicInfo());
                    return;
                }
                if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["weightcargo"])))
                {
                    info = GetAllInfo(strNameArray, strId, strVehicle, strVessel, strClient, strCargo, strStorage, strBooth, strFullOrEmpty, strWorkStyle, strTaskno, no, strSubmittime, strWeight);
                    Json = JsonConvert.SerializeObject(new DicPackage(false, info, "此车已过完磅！").DicInfo());
                    return;
                }
                if (string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["weight1"])))
                {
                    strWeight = string.Format("{0:N2}", Convert.ToInt32(dt.Rows[0]["weight2"]) / 1000);
                }
                else 
                {
                    strWeight = string.Format("{0:N2}", Convert.ToInt32(dt.Rows[0]["weight1"]) / 1000);
                }
                
                //获取申报信息
                strSql =
                    string.Format("select taskno,cargo,vessel,client,fullorempty from HARBOR.V_CONSIGN_QUICK where cgno='{0}'", strCgno);
                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count > 0)
                {
                    strTaskno = Convert.ToString(dt.Rows[0]["taskno"]);
                    strCargo = Convert.ToString(dt.Rows[0]["cargo"]);
                    strVessel = Convert.ToString(dt.Rows[0]["vessel"]);
                    strClient = Convert.ToString(dt.Rows[0]["client"]);
                    strFullOrEmpty = Convert.ToString(dt.Rows[0]["fullorempty"]);
                    strWorkStyle = strFullOrEmpty == "集港" ? "卸车" : "装车";
                }

                info = GetAllInfo(strNameArray, strId, strVehicle, strVessel, strClient, strCargo, strStorage, strBooth, strFullOrEmpty, strWorkStyle, strTaskno, no, strSubmittime, strWeight);
                if (strBlackList == "1")
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(true, info, "此车黑名单！").DicInfo());
                }
                else
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(true, info, null).DicInfo());
                }                         
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;

        /// <summary>
        /// 获取全部要返回的字典数据
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetAllInfo(string[] strNameArray, string strId, string strVehicle, string strVessel, string strClient, string strCargo, string strStorage, string strBooth, string strFullOrEmpty, string strWorkStyle, string strTaskno, string no, string strSubmittime, string strWeight)
        {
            Dictionary<string, object> info = new Dictionary<string, object>();
            info.Add(strNameArray[0], strId);
            info.Add(strNameArray[1], strVehicle);
            info.Add(strNameArray[2], strVessel);
            info.Add(strNameArray[3], strClient);
            info.Add(strNameArray[4], strCargo);
            info.Add(strNameArray[5], strStorage);
            info.Add(strNameArray[6], strBooth);
            info.Add(strNameArray[7], strFullOrEmpty);
            info.Add(strNameArray[8], strWorkStyle);
            info.Add(strNameArray[9], strTaskno);
            info.Add(strNameArray[10], no);
            info.Add(strNameArray[11], strSubmittime);
            info.Add(strNameArray[12], strWeight);
            return info;
        }
    }
}