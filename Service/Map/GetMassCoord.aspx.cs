//
//文件名：    GetMassCoord.aspx.cs
//功能描述：  获取摊位坐标列表数据
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
    public partial class GetMassCoord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string strSql =
                    string.Format("select * from vw_mass_info_to_external order by mass_num");
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathXsggps).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "暂无数据！").DicInfo());
                    return;
                }

                string[,] strArray = new string[dt.Rows.Count, 14];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    strArray[iRow, 0] = Convert.ToString(dt.Rows[iRow]["mass_num"]);
                    strArray[iRow, 1] = Convert.ToString(dt.Rows[iRow]["vertex_count"]);
                    strArray[iRow, 2] = Convert.ToString(dt.Rows[iRow]["vertex1_latitude"]);
                    strArray[iRow, 3] = Convert.ToString(dt.Rows[iRow]["vertex1_long"]);
                    strArray[iRow, 4] = Convert.ToString(dt.Rows[iRow]["vertex2_latitude"]);
                    strArray[iRow, 5] = Convert.ToString(dt.Rows[iRow]["vertex2_long"]);
                    strArray[iRow, 6] = Convert.ToString(dt.Rows[iRow]["vertex3_latitude"]);
                    strArray[iRow, 7] = Convert.ToString(dt.Rows[iRow]["vertex3_long"]);
                    strArray[iRow, 8] = Convert.ToString(dt.Rows[iRow]["vertex4_latitude"]);
                    strArray[iRow, 9] = Convert.ToString(dt.Rows[iRow]["vertex4_long"]);
                    strArray[iRow, 10] = Convert.ToString(dt.Rows[iRow]["vertex5_latitude"]);
                    strArray[iRow, 11] = Convert.ToString(dt.Rows[iRow]["vertex5_long"]);
                    strArray[iRow, 12] = Convert.ToString(dt.Rows[iRow]["vertex6_latitude"]);
                    strArray[iRow, 13] = Convert.ToString(dt.Rows[iRow]["vertex6_long"]);
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