//
//文件名：    GetEndWork.aspx.cs
//功能描述：  获取完工数据
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
using M_Lhgl.Service.Vehicle;

namespace M_Lhgl.Service.Vehicle
{
    public partial class GetEndWork : System.Web.UI.Page
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
                    string warning = string.Format("参数No,CodeCompany，RecognizeMethod不能为null！举例：http://218.92.115.55/M_Lhgl/Service/Vehicle/GetEndWork.aspx?No=690000&CodeCompany=77&RecognizeMethod=CARD");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;
                }

                string[] strNameArray = { "ID", "车号", "船名", "货代", "货物", "场地", "货位", "集疏港", "装卸车", "任务号", "通行证号", "申报时间", "衡重", "开工时间", "货重" };
                Dictionary<string, object> info = new Dictionary<string, object>();

                //号码字段名称
                switch (recognizeMethod)
                {
                    case "CARD":
                        workE.StrNoFieldName = "EXTER_NO";
                        break;
                    case "NFC":
                        workE.StrNoFieldName = "PARK_CARD_NO";
                        break;
                    default:
                        throw new Exception("错误的对象索引！");
                }

                //校验状态：黑名单无效，卡状态不是在用无效，卡被禁用无效，车状态不在港内无效
                Json = VerifyState(strNameArray, no);
                if (Json != string.Empty)
                {
                    return;
                }
                //校验：未车辆运输申报无效
                Json = VerifyVehicleTransportDeclare(strNameArray, codeCompany);
                if (Json != string.Empty)
                {
                    return;
                }
                //获取申报信息
                GetDeclareInfo(workE);
                //校验：未放行无效
                Json = VerifyVehiclePass(strNameArray);
                if (Json != string.Empty)
                {
                    return;
                }
                //对于东联的申报，还要检查是否通过车辆检查
                Json = VerifyDLVehicleCheck(strNameArray, codeCompany);
                if (Json != string.Empty)
                {
                    return;
                }
                //校验：过磅记录
                Json = VerifyWeightRecord(strNameArray);
                if (Json != string.Empty)
                {
                    return;
                }
                //校验:是否已开工
                Json = VerifyStartWork(strNameArray);
                if (Json != string.Empty)
                {
                    return;
                }
                //校验:是否已完工
                Json = VerifyEndWork(strNameArray);
                if (Json != string.Empty)
                {
                    return;
                }

                info = GetAllInfo(strNameArray);
                Json = JsonConvert.SerializeObject(new DicPackage(true, info, "等待完工").DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
        //开工数据对象
        WorkE workE = new WorkE();

        /// <summary>
        /// 校验状态（卡状态不是在用无效，卡被禁用无效，车状态不在港内无效）
        /// </summary>
        /// <param name="strNameArray">字段名称集合</param>
        /// <param name="no">号码（通行证号/NFC卡号）</param>
        /// <param name="workE">开、完工数据集</param>
        /// <returns></returns>
        private string VerifyState(string[] strNameArray, string no)
        {
            string strJson = string.Empty;
            Dictionary<string, object> info = new Dictionary<string, object>();
            string strSql =
                    string.Format("select blacklist,state,forbid_mark,veh_state,vehicle,exter_no,card_no from TRANSIT.V_VEH_CARD_HARBOR where {0}='{1}'", workE.StrNoFieldName, no.ToUpper());
            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathHarbor).ExecuteTable(strSql);
            if (dt.Rows.Count <= 0)
            {
                strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "此卡未办理！").DicInfo());
                return strJson;
            }
            //workE.StrBlackList = Convert.ToString(dt.Rows[0]["blacklist"]);
            workE.StrState = Convert.ToString(dt.Rows[0]["state"]);
            workE.StrForbidMark = Convert.ToString(dt.Rows[0]["forbid_mark"]);
            workE.StrVehState = Convert.ToString(dt.Rows[0]["veh_state"]);
            workE.StrVehicle = Convert.ToString(dt.Rows[0]["vehicle"]);
            workE.StrExterNo = Convert.ToString(dt.Rows[0]["exter_no"]);
            workE.StrCardNo = Convert.ToString(dt.Rows[0]["card_no"]);

            if (workE.StrState != "1")
            {
                info = GetAllInfo(strNameArray);
                strJson = JsonConvert.SerializeObject(new DicPackage(false, info, "此卡未办理进港业务！").DicInfo());
                return strJson;
            }
            if (workE.StrForbidMark == "1")
            {
                info = GetAllInfo(strNameArray);
                strJson = JsonConvert.SerializeObject(new DicPackage(false, info, "此卡禁用！").DicInfo());
                return strJson;
            }
            if (workE.StrVehState != "1")
            {
                info = GetAllInfo(strNameArray);
                strJson = JsonConvert.SerializeObject(new DicPackage(false, info, "此车不在港内！").DicInfo());
                return strJson;
            }

            return strJson;
        }

        /// <summary>
        /// 校验车辆运输申报
        /// </summary>
        /// <param name="strNameArray">字段名称集合</param>
        /// <param name="codeCompany">公司编码</param>
        /// <returns></returns>
        private string VerifyVehicleTransportDeclare(string[] strNameArray, string codeCompany)
        {
            string strJson = string.Empty;
            Dictionary<string, object> info = new Dictionary<string, object>();
            string strSql =
                string.Format(@"select id,audit_mark,storage,booth,cgno,to_char(submittime, 'yyyy-MM-dd HH24:mi:ss') as submittime,audittime  
                                from Harbor.V_CONSIGN_VEHICLE_ONLY_QUICK 
                                where submittime>sysdate-1 and code_department='{0}' and exter_no='{1}' and vehicle='{2}' 
                                order by submittime desc",
                                codeCompany, workE.StrExterNo, workE.StrVehicle);
            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathHarbor).ExecuteTable(strSql);
            if (dt.Rows.Count <= 0)
            {
                info = GetAllInfo(strNameArray);
                strJson = JsonConvert.SerializeObject(new DicPackage(false, info, "此车没有本公司运输申报！").DicInfo());
                return strJson;
            }
            workE.StrStorage = Convert.ToString(dt.Rows[0]["storage"]);
            workE.StrBooth = Convert.ToString(dt.Rows[0]["booth"]);
            workE.StrId = Convert.ToString(dt.Rows[0]["id"]);
            workE.StrCgno = Convert.ToString(dt.Rows[0]["cgno"]);
            workE.StrSubmittime = Convert.ToString(dt.Rows[0]["SUBMITTIME"]);
            workE.StrAuditMark = Convert.ToString(dt.Rows[0]["audit_mark"]);
            workE.StrAuditTime = Convert.ToString(dt.Rows[0]["audittime"]);

            return strJson;
        }

        /// <summary>
        /// 校验车辆放行
        /// </summary>
        /// <param name="strNameArray">字段名称集合</param>
        /// <param name="codeCompany">公司编码</param>
        /// <returns></returns>
        private string VerifyVehiclePass(string[] strNameArray)
        {
            string strJson = string.Empty;
            Dictionary<string, object> info = new Dictionary<string, object>();
            if (workE.StrAuditMark != "1")
            {
                info = GetAllInfo(strNameArray);
                strJson = JsonConvert.SerializeObject(new DicPackage(false, info, "此车未放行！").DicInfo());
                return strJson;
            }

            return strJson;
        }

        /// <summary>
        /// 校验东联车辆检查
        /// </summary>
        /// <param name="strNameArray">字段名称集合</param>
        /// <param name="codeCompany">公司编码</param>
        /// <param name="workE">开、完工数据集</param>
        /// <returns></returns>
        private string VerifyDLVehicleCheck(string[] strNameArray, string codeCompany)
        {
            string strJson = string.Empty;
            Dictionary<string, object> info = new Dictionary<string, object>();
            if (codeCompany == "77")
            {
                string strSql =
                    string.Format(@"select check_mark  
                                    from CGATE.v_cgate_record_cache t 
                                    where pass_time>sysdate-1 and code_company='{0}' and exter_no='{1}' and vehicle='{2}' and inout_mark='0' and  pass_time>to_date('{3}', 'yyyy-MM-dd HH24:mi:ss')
                                    order by pass_time desc",
                                    codeCompany, workE.StrExterNo, workE.StrVehicle, workE.StrAuditTime);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0 || string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["check_mark"])))
                {
                    info = GetAllInfo(strNameArray);
                    strJson = JsonConvert.SerializeObject(new DicPackage(false, info, "此车未进门！").DicInfo());
                    return strJson;
                }
                if (Convert.ToString(dt.Rows[0]["check_mark"]) != "1")
                {
                    info = GetAllInfo(strNameArray);
                    strJson = JsonConvert.SerializeObject(new DicPackage(false, info, "此车未通过检查！").DicInfo());
                    return strJson;
                }
            }

            return strJson;
        }

        /// <summary>
        /// 校验过磅记录
        /// </summary>
        /// <param name="strNameArray">字段名称集合</param>
        /// <param name="workE">开、完工数据集</param>
        /// <returns></returns>
        private string VerifyWeightRecord(string[] strNameArray)
        {
            string strJson = string.Empty;
            Dictionary<string, object> info = new Dictionary<string, object>();
            string strSql =
                string.Format(@"select weight1,weight2,weightcargo,convert(varchar(100), recordtime, 120) as recordtime
                                    from BALANCECENTER..V_MetageForComm 
                                    where RecordTime>getdate()-1 and CardNo='{0}' and Truck='{1}' and  RecordTime>'{2}'
                                    order by RecordTime desc ",
                                    workE.StrCardNo, workE.StrVehicle, workE.StrAuditTime);
            var dt = new Leo.SqlServer.DataAccess(RegistryKey.KeyPathBc).ExecuteTable(strSql);
            if (dt.Rows.Count <= 0)
            {
                info = GetAllInfo(strNameArray);
                strJson = JsonConvert.SerializeObject(new DicPackage(false, info, "此车无过磅记录！").DicInfo());
                return strJson;
            }
            workE.StrRecordtime = Convert.ToString(dt.Rows[0]["recordtime"]);

            if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["weightcargo"])))
            {
                workE.StrWeightCargo = Convert.ToString(dt.Rows[0]["weightcargo"]);
                info = GetAllInfo(strNameArray);
                strJson = JsonConvert.SerializeObject(new DicPackage(false, info, "此车已过完磅！").DicInfo());
                return strJson;
            }
            //千克转换成吨
            if (string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["weight1"])))
            {
                workE.StrWeight = string.Format("{0:N2}", Convert.ToInt32(dt.Rows[0]["weight2"]) / 1000);
            }
            else
            {
                workE.StrWeight = string.Format("{0:N2}", Convert.ToInt32(dt.Rows[0]["weight1"]) / 1000);
            }

            return strJson;
        }

        /// <summary>
        /// 校验是否已开工
        /// </summary>
        /// <param name="strNameArray">字段名称集合</param>
        /// <returns></returns>
        private string VerifyStartWork(string[] strNameArray)
        {
            string strJson = string.Empty;
            Dictionary<string, object> info = new Dictionary<string, object>();
            string strSql =
                string.Format("select to_char(start_time, 'yyyy-MM-dd HH24:mi:ss') as start_time from tb_pro_consignvehicle where id='{0}'", workE.StrId);
            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathHarbor).ExecuteTable(strSql);
            if (dt.Rows.Count <= 0)
            {
                info = GetAllInfo(strNameArray);
                strJson = JsonConvert.SerializeObject(new DicPackage(false, info, "不存在！").DicInfo());
                return strJson;
            }

            if (string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["start_time"])))
            {
                info = GetAllInfo(strNameArray);
                strJson = JsonConvert.SerializeObject(new DicPackage(false, info, "未开工").DicInfo());
                return strJson;
            }

            workE.StrStartTime = Convert.ToString(dt.Rows[0]["start_time"]);

            return strJson;
        }

        /// <summary>
        /// 校验是否已完工
        /// </summary>
        /// <param name="strNameArray">字段名称集合</param>
        /// <returns></returns>
        private string VerifyEndWork(string[] strNameArray)
        {
            string strJson = string.Empty;
            Dictionary<string, object> info = new Dictionary<string, object>();
            string strSql =
                string.Format("select to_char(COMPLETE_TIME, 'yyyy-MM-dd HH24:mi:ss') as COMPLETE_TIME from tb_pro_consignvehicle where id='{0}'", workE.StrId);
            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathHarbor).ExecuteTable(strSql);
            if (dt.Rows.Count <= 0)
            {
                info = GetAllInfo(strNameArray);
                strJson = JsonConvert.SerializeObject(new DicPackage(false, info, "不存在！").DicInfo());
                return strJson;
            }

            if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["COMPLETE_TIME"])))
            {
                info = GetAllInfo(strNameArray);
                strJson = JsonConvert.SerializeObject(new DicPackage(false, info, "已完工，不能重复作业！").DicInfo());
                return strJson;
            }      

            return strJson;
        }

        /// <summary>
        /// 获取申报信息
        /// </summary>
        /// <param name="workE">开、完工数据集</param>
        /// <returns></returns>
        private void GetDeclareInfo(WorkE workE)
        {
            //获取申报信息
            string strSql =
                string.Format("select taskno,cargo,vessel,client,fullorempty from HARBOR.V_CONSIGN_QUICK where cgno='{0}'", workE.StrCgno);
            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathHarbor).ExecuteTable(strSql);
            if (dt.Rows.Count > 0)
            {
                workE.StrTaskno = Convert.ToString(dt.Rows[0]["taskno"]);
                workE.StrCargo = Convert.ToString(dt.Rows[0]["cargo"]);
                workE.StrVessel = Convert.ToString(dt.Rows[0]["vessel"]);
                workE.StrClient = Convert.ToString(dt.Rows[0]["client"]);
                workE.StrFullOrEmpty = Convert.ToString(dt.Rows[0]["fullorempty"]);
                workE.StrWorkStyle = workE.StrFullOrEmpty == "集港" ? "卸车" : "装车";
            }
        }

        /// <summary>
        /// 获取全部要返回的字典数据
        /// </summary>
        /// <param name="strNameArray">字段名称集合</param>
        /// <param name="workE">开、完工数据集</param>
        /// <returns></returns>
        private Dictionary<string, object> GetAllInfo(string[] strNameArray)
        {
            Dictionary<string, object> info = new Dictionary<string, object>();
            info.Add(strNameArray[0], workE.StrId);
            info.Add(strNameArray[1], workE.StrVehicle);
            info.Add(strNameArray[2], workE.StrVessel);
            info.Add(strNameArray[3], workE.StrClient);
            info.Add(strNameArray[4], workE.StrCargo);
            info.Add(strNameArray[5], workE.StrStorage);
            info.Add(strNameArray[6], workE.StrBooth);
            info.Add(strNameArray[7], workE.StrFullOrEmpty);
            info.Add(strNameArray[8], workE.StrWorkStyle);
            info.Add(strNameArray[9], workE.StrTaskno);
            info.Add(strNameArray[10], workE.StrExterNo);
            info.Add(strNameArray[11], workE.StrSubmittime);
            info.Add(strNameArray[12], workE.StrWeight);
            info.Add(strNameArray[13], workE.StrStartTime);
            info.Add(strNameArray[14], workE.StrWeightCargo);
            return info;
        }
    }
}