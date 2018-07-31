//
//文件名：    GetTeamWorkerAndMark.aspx.cs
//功能描述：  获取班组数据（包含是否已选中标志）
//创建时间：  2015/11/14
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
    public partial class GetTeamWorkerAndMark : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //派工编码
            var pmno = Request.Params["Pmno"];
            //理货单编码
            var tbno = Request.Params["Tbno"];

            try
            {
                if (pmno == null || tbno == null)
                {
                    string warning = string.Format("参数Pmno，Tbno不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Slip/GetTeamWorkerAndMark.aspx?Pmno=20151212000565&Tbno=20151221066216");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;

                }

                string strSql =
                    string.Format(@"select code_workteam,workteam,begintime,endtime,amount,pmno,weight
                                           from VW_PS_MISSION_FACTWORKER  
                                           where  pmno='{0}'", pmno);
                var dt0 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt0.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "暂无数据！").DicInfo());
                    return;
                }

                string[,] strArray = new string[dt0.Rows.Count, 11];
                for (int iRow = 0; iRow < dt0.Rows.Count; iRow++)
                {
                    string strStartTime = string.Empty;
                    string strEndTime = string.Empty;
                    string strAmount = string.Empty;
                    string strWeight = string.Empty;
                    string strCount = string.Empty;
                    strSql =
                        string.Format(@"select * from TB_TALLY_WORKER 
                                        where pmno='{0}' and tbno='{1}' and code_workteam='{2}'",
                                        pmno, tbno, Convert.ToString(dt0.Rows[iRow]["code_workteam"]));
                    var dt1 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                    if (dt1.Rows.Count <= 0)
                    {
                        strArray[iRow, 0] = "0";
                    }
                    else
                    {
                        strArray[iRow, 0] = "1";
                        strStartTime = Convert.ToString(dt1.Rows[0]["start_time"]);
                        strEndTime = Convert.ToString(dt1.Rows[0]["end_time"]);
                        strAmount = Convert.ToString(dt1.Rows[0]["amount"]);
                        strWeight = Convert.ToString(dt1.Rows[0]["weight"]);
                        strCount = Convert.ToString(dt1.Rows[0]["WORKNUM"]);
                    }

                    strArray[iRow, 1] = Convert.ToString(dt0.Rows[iRow]["code_workteam"]);
                    strArray[iRow, 2] = string.Empty;
                    strArray[iRow, 3] = Convert.ToString(dt0.Rows[iRow]["workteam"]);
                    strArray[iRow, 4] = string.Empty;
                    strArray[iRow, 5] = strStartTime;
                    strArray[iRow, 6] = strEndTime;
                    strArray[iRow, 7] = strAmount;
                    strArray[iRow, 8] = Convert.ToString(dt0.Rows[iRow]["pmno"]);
                    strArray[iRow, 9] = strWeight;
                    strArray[iRow, 10] = strCount; 
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