//
//文件名：    GetMassInfo.aspx.cs
//功能描述：  获取摊信息数据
//创建时间：  2015/09/30
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


namespace M_Lhgl.Service.Map
{
    public partial class GetMassInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //摊位编号
            var massNum = Request.Params["MassNum"];

            //codeCompany = "14";

            try
            {
                if (massNum == null)
                {
                    string warning = string.Format("参数MassNum不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Map/GetMassInfo.aspx?MassNum=1130-320");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;

                }
                string strSql =
                    string.Format("select * from V_MASS_INFO_TO_GPS where mass_num='{0}' order by blno asc", massNum);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathXsggps).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "摊位编号不存在！").DicInfo());
                    return;
                }

                string[,] strArray = new string[dt.Rows.Count, 6];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    strArray[iRow, 0] = Convert.ToString(dt.Rows[iRow]["cargo"]);
                    strArray[iRow, 1] = Convert.ToString(dt.Rows[iRow]["vgdisplay"]);
                    strArray[iRow, 2] = Convert.ToString(dt.Rows[iRow]["client"]);
                    strArray[iRow, 3] = Convert.ToString(dt.Rows[iRow]["cargo_name"]);
                    strArray[iRow, 4] = Convert.ToString(dt.Rows[iRow]["blno"]);
                    strArray[iRow, 5] = Convert.ToString(dt.Rows[iRow]["weight"]);
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