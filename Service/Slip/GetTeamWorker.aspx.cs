//
//文件名：    GetTeamWorker.aspx.cs
//功能描述：  获取班组数据
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
    public partial class GetTeamWorker : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //派工编码
            var pmno = Request.Params["Pmno"];
            //pmno = "20151010000161";
           
            try
            {
                if (pmno == null)
                {
                    string warning = string.Format("参数Pmno不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Slip/GetTeamWorker.aspx?Pmno=20151212000565");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;

                }
                string strSql =
                    string.Format(@"select code_workteam,workteam,begintime,endtime,amount,pmno,weight
                                           from VW_PS_MISSION_FACTWORKER  
                                           where  pmno='{0}'", pmno);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "暂无数据！").DicInfo());
                    return;
                }

                string[,] strArray = new string[dt.Rows.Count, 10];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    strArray[iRow, 0] = Convert.ToString(dt.Rows[iRow]["code_workteam"]);
                    strArray[iRow, 1] = string.Empty;
                    strArray[iRow, 2] = Convert.ToString(dt.Rows[iRow]["workteam"]);
                    strArray[iRow, 3] = string.Empty;
                    strArray[iRow, 4] = Convert.ToString(dt.Rows[iRow]["begintime"]);
                    strArray[iRow, 5] = Convert.ToString(dt.Rows[iRow]["endtime"]);
                    strArray[iRow, 6] = Convert.ToString(dt.Rows[iRow]["amount"]);
                    strArray[iRow, 7] = Convert.ToString(dt.Rows[iRow]["pmno"]);
                    strArray[iRow, 8] = Convert.ToString(dt.Rows[iRow]["weight"]);
                    strArray[iRow, 9] = string.Empty;
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