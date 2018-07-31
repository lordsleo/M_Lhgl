//
//文件名：    GetMachine.aspx.cs
//功能描述：  获取机械数据
//创建时间：  2015/10/20
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
    public partial class GetMachine : System.Web.UI.Page
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
                    string warning = string.Format("参数Pmno不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Slip/GetMachine.aspx?Pmno=20151212000565");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;

                }
                string strSql =
                    string.Format(@"select id,code_machine,workno,machine,name,begintime,endtime,amount,weight,pmno,code_department  
                                           from vw_PS_MISSION_FACTMACHINE  
                                           where  pmno='{0}'", pmno);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "暂无数据！").DicInfo());
                    return;
                }

                string[,] strArray = new string[dt.Rows.Count, 11];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    strArray[iRow, 0] = Convert.ToString(dt.Rows[iRow]["code_machine"]);
                    strArray[iRow, 1] = Convert.ToString(dt.Rows[iRow]["workno"]);
                    strArray[iRow, 2] = Convert.ToString(dt.Rows[iRow]["machine"]);
                    strArray[iRow, 3] = Convert.ToString(dt.Rows[iRow]["name"]);
                    strArray[iRow, 4] = Convert.ToString(dt.Rows[iRow]["begintime"]);
                    strArray[iRow, 5] = Convert.ToString(dt.Rows[iRow]["endtime"]);
                    strArray[iRow, 6] = Convert.ToString(dt.Rows[iRow]["amount"]);
                    strArray[iRow, 7] = Convert.ToString(dt.Rows[iRow]["pmno"]);
                    strArray[iRow, 8] = Convert.ToString(dt.Rows[iRow]["weight"]);
                    strArray[iRow, 9] = string.Empty;
                    strArray[iRow, 10] = Convert.ToString(dt.Rows[iRow]["code_department"]);
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