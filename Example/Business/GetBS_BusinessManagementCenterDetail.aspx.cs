//
//文件名：    GetBS_BusinessManagementCenterDetail.aspx.cs
//功能描述：  获取保税业务管理中心明细数据
//创建时间：  2015/09/09
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

namespace M_Lhgl.Example.Business
{
    public partial class GetBS_BusinessManagementCenterDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = Request.Params["ID"];
            id = "LQ150914004";
          
            try
            {
                if (id == null)
                {
                    string warning = string.Format("参数ID不能为nul！举例：http://218.92.115.55/M_Lhgl/Example/Business/GetBS_BusinessManagementCenterDetail.aspx?ID=LQ150914004");
                    Json = JsonConvert.SerializeObject(new DicPackage(warning).FalseDic());
                    return;
                }

                string sql =
                        string.Format(@"SELECT '' AS FLAG, id,shipname,voyage,blno,cargo_code,bonded_type,blno_count,cargo_owner,arrival_port_date, order_date,
                                               ctnno_count,progress,input_man,TO_DATE(INPUT_DATE,'YYYYMMDDHH24miss') INPUTDATE,remark,doc_complete,
                                               decode(doc_complete,'1','是','否')as doc_complete_name ,contract_no,TO_DATE( decode(over_date,'-',null,over_date),'YYYYMMDDHH24miss') over_date,
                                               habitat,net,ctn_type,location,handno,cargo_name,expenses_name,clientshort,username,progress_name, storage_pro,fanghuo,iport_dept,department,
                                               inspection_flag,customs_flag,port_transport_flag,contract_flag,fanghuo1,fanghuo2,storage_state from view_all_delegation where id='{0}'", id);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathLbts).ExecuteTable(sql);
                if (dt.Rows.Count == 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage("ID不存在！").FalseDic());
                    return;
                }

                string[] nameArray = { "保税部", "船名", "货物名称", "货主名称", "提单数", "箱数", "保税类型","录入人","合同关联",  
                                       "报检结束", "报关结束", "港口作业结束", "开始放货", "货放完", "仓储结束"};
                //排序字符串
                string order = string.Empty;
                order = nameArray[0] + "+" + nameArray[1] + "+" + nameArray[2] + "+" + nameArray[3] + "+" + nameArray[4] + "+" + nameArray[5] + "+" + nameArray[6] + "+" +
                        nameArray[7] + "+" + nameArray[8] + "+" + nameArray[9] + "+" + nameArray[10] + "+" + nameArray[11] + "+" + nameArray[12] + "+" +
                        nameArray[13] + "+" + nameArray[14];

                Dictionary<string, object> info = new Dictionary<string, object>();
                info.Add(nameArray[0], Convert.ToString(dt.Rows[0]["DEPARTMENT"]));
                info.Add(nameArray[1], Convert.ToString(dt.Rows[0]["SHIPNAME"]));
                info.Add(nameArray[2], Convert.ToString(dt.Rows[0]["CARGO_NAME"]));
                info.Add(nameArray[3], Convert.ToString(dt.Rows[0]["CLIENTSHORT"]));
                info.Add(nameArray[4], Convert.ToString(dt.Rows[0]["BLNO_COUNT"]));
                info.Add(nameArray[5], Convert.ToString(dt.Rows[0]["CTNNO_COUNT"]));
                info.Add(nameArray[6], Convert.ToString(dt.Rows[0]["EXPENSES_NAME"]));
                info.Add(nameArray[7], Convert.ToString(dt.Rows[0]["USERNAME"]));
                info.Add(nameArray[8], Convert.ToString(dt.Rows[0]["CONTRACT_FLAG"]) != "0" ? "" : "√");
                info.Add(nameArray[9], Convert.ToString(dt.Rows[0]["INSPECTION_FLAG"]) != "1" ? "" : "√");
                info.Add(nameArray[10], Convert.ToString(dt.Rows[0]["CUSTOMS_FLAG"]) != "1" ? "" : "√");
                info.Add(nameArray[11], Convert.ToString(dt.Rows[0]["PORT_TRANSPORT_FLAG"]) == "2" ? "" : "√");
                info.Add(nameArray[12], Convert.ToString(dt.Rows[0]["FANGHUO1"]) != "已放货" ? "" : "√");
                info.Add(nameArray[13], Convert.ToString(dt.Rows[0]["FANGHUO2"]) != "已放完" ? "" : "√");
                info.Add(nameArray[14], Convert.ToString(dt.Rows[0]["STORAGE_STATE"]) != "完工" ? "" : "√");
                info.Add("Order", order);

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