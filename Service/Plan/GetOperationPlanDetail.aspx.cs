//
//文件名：    GetOperationPlanDetail.aspx.cs
//功能描述：  获取作业计划明细数据
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

namespace M_Lhgl.Service.Plan
{
    public partial class GetOperationPlanDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //委托编码
            var cgno = Request.Params["Cgno"];
            cgno = "14";

            try
            {
                if (cgno == null)
                {
                    string warning = string.Format("参数Cgno不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Plan/GetOperationPlanDetail.aspx?Cgno=14");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;

                }
                string strSql =
                    string.Format(@"select * 
                                    from vw_ps_mission_yardplan1 where cgno='{0}'",
                                    cgno);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "委托编码不存在！").DicInfo());
                    return;
                }

                string[] strNameArray = { "货主", "货物", "航次", "货代", "货场", 
                                          "重量", "进场日期","进出口","内外贸",
                                          "唛头", "货位", "包装", "件数","件重"};
                //数组排序连接
                string strOrderLink = StringTool.ArrayToString(strNameArray);

                Dictionary<string, object> info = new Dictionary<string, object>();
                info.Add(strNameArray[0], Convert.ToString(dt.Rows[0]["cargoowner"]));
                info.Add(strNameArray[1], Convert.ToString(dt.Rows[0]["cargo"]));
                info.Add(strNameArray[2], Convert.ToString(dt.Rows[0]["vgdisplay"]));
                info.Add(strNameArray[3], Convert.ToString(dt.Rows[0]["client"]));
                info.Add(strNameArray[4], Convert.ToString(dt.Rows[0]["storage"]));
                info.Add(strNameArray[5], Convert.ToString(dt.Rows[0]["weight"]));
                info.Add(strNameArray[6], Convert.ToString(dt.Rows[0]["first_indate"]));
                info.Add(strNameArray[7], Convert.ToString(dt.Rows[0]["inout"]));
                info.Add(strNameArray[8], Convert.ToString(dt.Rows[0]["trade"]));
                info.Add(strNameArray[9], Convert.ToString(dt.Rows[0]["mark"]));
                info.Add(strNameArray[10], Convert.ToString(dt.Rows[0]["booth"]));
                info.Add(strNameArray[11], Convert.ToString(dt.Rows[0]["pack"]));
                info.Add(strNameArray[12], Convert.ToString(dt.Rows[0]["amount"]));
                info.Add(strNameArray[13], Convert.ToString(dt.Rows[0]["pieceweight"]));
                info.Add("Order", strOrderLink);

                Json = JsonConvert.SerializeObject(new DicPackage(true, null, null).DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}