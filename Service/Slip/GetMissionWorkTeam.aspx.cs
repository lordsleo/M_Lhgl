//
//文件名：    GetMissionWorkTeam.aspx.cs
//功能描述：  获取配工班组班别数据
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
    public partial class GetMissionWorkTeam : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //派工编码
            var pmno = Request.Params["Pmno"];

            try
            {
                if (pmno == null)
                {
                    string warning = string.Format("参数Pmno不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Slip/GetMissionWorkTeam.aspx?Pmno=20151010000161");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;

                }

                string[,] strArray = null;
                string strSql =
                    string.Format("select a.code_workteam,b.workteam from TB_PS_MISSION_FACTWORKER a, baseresource.tb_code_workteam b where a.code_workteam = b.code_workteam and pmno='{0}' order by code_workteam", pmno);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count > 0)
                {
                    strArray = new string[dt.Rows.Count, 2];
                    for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                    {
                        strArray[iRow, 0] = Convert.ToString(dt.Rows[iRow]["code_workteam"]);
                        strArray[iRow, 1] = Convert.ToString(dt.Rows[iRow]["workteam"]);
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