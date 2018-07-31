//
//文件名：    GetOperation.aspx.cs
//功能描述：  获取子过程列表数据
//创建时间：  2015/12/18
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
    public partial class GetOperation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //派工编码
            var pmno = Request.Params["Pmno"];
            //pmno = "20151021000241";

            try
            {
                if (pmno == null)
                {
                    string warning = string.Format("参数Pmno不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Slip/GetOperation.aspx?Pmno=20151212000565");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;

                }

                string[,] strArray = null;
                //作业过程编码 
                string strCodeOperation = string.Empty; 
                string strSql =
                        string.Format(@"select code_storage,code_storagelast,mark_tallybill,code_operation,code_operation_fact,CODE_CARRIER_S,CODE_CARRIER_E,vgdisplay,vgno,cabin,code_carrier,nvessel,
                                        code_nvessel,carrier1,nsno,tallydate,code_opstype,code_workflow,code_worktime,code_teamtype,code_trainroad,code_motorcade,begintime,endtime,nsno_barge,nsno_bargelast          
                                        from vw_ps_mission where pmno='{0}'",
                                        pmno);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(true, strArray, null).DicInfo());
                    return;   
                }

                strCodeOperation = Convert.ToString(dt.Rows[0]["code_operation"]);
                strSql = string.Format(@"select code_operation_fact code,operationfact displayname,logogram,CODE_CARRIER_S,CODE_CARRIER_E  
                                         from nharbor.vw_br_operationfact where CODE_OPERATION='{0}' order by code", strCodeOperation);
                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count > 0)
                {
                    strArray = new string[dt.Rows.Count, 3];
                    for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                    {
                        strArray[iRow, 0] = Convert.ToString(dt.Rows[iRow]["code"]);
                        strArray[iRow, 1] = Convert.ToString(dt.Rows[iRow]["displayname"]);
                        strArray[iRow, 2] = Convert.ToString(dt.Rows[iRow]["logogram"]);
                    }
                }

                Json = JsonConvert.SerializeObject(new DicPackage(true, strArray, null).DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}