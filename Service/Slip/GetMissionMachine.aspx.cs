//
//文件名：    GetMissionMachine.aspx.cs
//功能描述：  获取配工机械数据
//创建时间：  2015/10/01
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
    public partial class GetMissionMachine : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //派工编码
            var pmno = Request.Params["Pmno"];

            try
            {
                if (pmno == null)
                {
                    string warning = string.Format("参数Pmno不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Slip/GetMissionMachine.aspx?Pmno=20151010000161");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;

                }

                string[,] strArray = null;
                string strSql =
                    string.Format("select a.code_machine,b.machine from TB_PS_MISSION_FACTMACHINE a, tb_br_machine b where a.code_machine = b.code_machine and a.pmno='{0}' order by code_machine", pmno);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count > 0)
                {
                    strArray = new string[dt.Rows.Count, 2];
                    for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                    {
                        strArray[iRow, 0] = Convert.ToString(dt.Rows[iRow]["code_machine"]);
                        strArray[iRow, 1] = Convert.ToString(dt.Rows[iRow]["machine"]);
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