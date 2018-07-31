//
//文件名：    GetGoogsBill.aspx.cs
//功能描述：  获取票货数据
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
    public partial class GetGoogsBill : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string strSql =
                    string.Format(@"select cgno,gbno,code_client,code_pack,gbdisplay || '/' || stockamount || '/' || stockweight as gbdisplay,stockamount,stockweight,mark,blno,vgdisplay,code_inout,factweight 
                                    from vw_hc_consign_gbno");
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "暂无数据！").DicInfo());
                    return;
                }

                string[,] strArray = new string[dt.Rows.Count, 3];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    strArray[iRow, 0] = Convert.ToString(dt.Rows[iRow]["cgno"]);
                    strArray[iRow, 1] = Convert.ToString(dt.Rows[iRow]["gbno"]);
                    strArray[iRow, 2] = Convert.ToString(dt.Rows[iRow]["gbdisplay"]);
                }

                //string strBillInfo = string.Empty;
                //strBillInfo += Convert.ToString(dt.Rows[0]["gbdisplay"]) + "/";
                //strBillInfo += Convert.ToString(dt.Rows[0]["stockamount"]) + "/";
                //strBillInfo += Convert.ToString(dt.Rows[0]["stockweight"]) + "/";
                //strBillInfo += Convert.ToString(dt.Rows[0]["mark"]) + "/";
                //strBillInfo += Convert.ToString(dt.Rows[0]["blno"]) + "/";
                //strBillInfo += Convert.ToString(dt.Rows[0]["vgdisplay"]) + "/";
                //strBillInfo += Convert.ToString(dt.Rows[0]["code_inout"]) + "/";
                //strBillInfo += Convert.ToString(dt.Rows[0]["factweight"]);

                //string[] strNameArray = { "委托编码", "票货" };
                //Dictionary<string, object> info = new Dictionary<string, object>();
                //info.Add(strNameArray[0], Convert.ToString(dt.Rows[0]["cgno"]));
                //info.Add(strNameArray[1], strBillInfo);

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