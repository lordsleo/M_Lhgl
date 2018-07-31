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
    public partial class GetBS_BusinessManagementCenter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //用户编码
            var codeUser = Request.Params["CodeUser"];
            //起始时间
            var startTime = Request.Params["StartTime"];
            //终止时间
            var endTime = Request.Params["EndTime"];

            //codeUser = "121355";
            //startTime = "2015-08-11";
            //endTime = "2015-09-11";

            try
            {
                if (codeUser == null || startTime == null || endTime == null)
                {
                    string warning = string.Format("参数CodeUser，StartTime，EndTime不能为nul！举例：http://218.92.115.55/M_Lhgl/Example/Business/GetBS_BusinessManagementCenter.aspx?CodeUser=121355&StartTime=2015-08-11&EndTime=2015-09-11");
                    Json = JsonConvert.SerializeObject(new DicPackage(warning).FalseDic());
                    return;

                }

                startTime = Convert.ToDateTime(startTime).ToString("yyyyMMdd");
                endTime = Convert.ToDateTime(endTime).ToString("yyyyMMdd");

                string viewDelegationAll = @"SELECT '' AS FLAG, id,shipname,voyage,blno,cargo_code,bonded_type,blno_count,cargo_owner,arrival_port_date, order_date,
                                                    ctnno_count,progress,input_man,TO_DATE(INPUT_DATE,'YYYYMMDDHH24miss') INPUTDATE,remark,doc_complete,
                                                    decode(doc_complete,'1','是','否')as doc_complete_name ,contract_no,TO_DATE( decode(over_date,'-',null,over_date),'YYYYMMDDHH24miss') over_date,
                                                    habitat,net,ctn_type,location,handno,cargo_name,expenses_name,clientshort,username,progress_name, storage_pro,fanghuo,iport_dept,department,
                                                    inspection_flag,customs_flag,port_transport_flag,contract_flag,fanghuo1,fanghuo2,storage_state from view_all_delegation";
                string firm = "EXISTS(SELECT * FROM ACCESS_CONTROL WHERE CODE_USER='" + codeUser + "'AND VIEW_ALL_DELEGATION.IPORT_DEPT=ACCESS_CONTROL.CODE_DEPARTMENT)";
                string firmQuasi = "(SUBSTR(ORDER_DATE,1,8)>='" + startTime + "'AND SUBSTR(ORDER_DATE,1,8)<='" + endTime + "')";
                string taxis = "ORDER BY FANGHUO asc, INPUTDATE DESC";

                string sql =
                    string.Format("{0} where {1} and {2} {3}", viewDelegationAll, firm, firmQuasi, taxis);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathLbts).ExecuteTable(sql);
                if (dt.Rows.Count == 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage("暂无数据！").FalseDic());
                    return;
                }

                string[,] array = new string[dt.Rows.Count, 7];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    array[iRow, 0] = Convert.ToString(dt.Rows[iRow]["ID"]);
                    array[iRow, 1] = Convert.ToString(dt.Rows[iRow]["DEPARTMENT"]);
                    array[iRow, 2] = Convert.ToString(dt.Rows[iRow]["SHIPNAME"]);
                    array[iRow, 3] = Convert.ToString(dt.Rows[iRow]["CARGO_NAME"]);
                    array[iRow, 4] = Convert.ToString(dt.Rows[iRow]["CLIENTSHORT"]);
                    array[iRow, 5] = Convert.ToString(dt.Rows[iRow]["BLNO_COUNT"]);
                    array[iRow, 6] = Convert.ToString(dt.Rows[iRow]["CTNNO_COUNT"]);
                }

                Json = JsonConvert.SerializeObject(new DicPackage().TrueDic(array));
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(string.Format("{0}：获取数据发生异常。{1}", ex.Source, ex.Message)).FalseDic());
            }
        }
        protected string Json;
    }
}