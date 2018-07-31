//
//文件名：    GetConsignDetail.aspx.cs
//功能描述：  获取委托查询明细数据
//创建时间：  2015/09/18
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

namespace M_Lhgl.Service.Consign
{
    public partial class GetConsignDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //委托编码
            var cgno = Request.Params["Cgno"];
            //cgno = "48d31ca70ade41c1a823be7f025ca141";

            try
            {
                if (cgno == null)
                {
                    string warning = string.Format("参数Cgno不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Consign/GetConsignDetail.aspx?Cgno=cbdde8de4bca4e628856c1a5167b4e32");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;

                }

                string strFilter = string.Format("where cgno='{0}'", cgno);
                string strSql =
                    string.Format("select * from VW_HC_CONSIGN {0}", strFilter);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "委托编码不存在！").DicInfo());
                    return;
                }

                string[] strNameArray = { "货主", "货物", "航次", "作业过程", "委托号", 
                                          "计划件数", "计划重量","实际件数","实际重量",
                                          "委托日期", "卸船航次","装船航次", "进出", "贸别", 
                                          "包装", "唛头" , "公司"};
                //数组排序连接
                string strOrderLink = StringTool.ArrayToString(strNameArray);
        
                Dictionary<string, object> info = new Dictionary<string, object>();
                info.Add(strNameArray[0], Convert.ToString(dt.Rows[0]["cargoowner"]));
                info.Add(strNameArray[1], Convert.ToString(dt.Rows[0]["cargo"]));
                info.Add(strNameArray[2], Convert.ToString(dt.Rows[0]["voyage"]));
                info.Add(strNameArray[3], Convert.ToString(dt.Rows[0]["operation"]));
                info.Add(strNameArray[4], Convert.ToString(dt.Rows[0]["taskno"]));
                info.Add(strNameArray[5], Convert.ToString(dt.Rows[0]["planamount"]));
                info.Add(strNameArray[6], Convert.ToString(dt.Rows[0]["planweight"]));
                info.Add(strNameArray[7], Convert.ToString(dt.Rows[0]["factamount"]));
                info.Add(strNameArray[8], Convert.ToString(dt.Rows[0]["factweight"]));
                info.Add(strNameArray[9], Convert.ToString(dt.Rows[0]["signdate"]));
                info.Add(strNameArray[10], Convert.ToString(dt.Rows[0]["vgno"]));
                info.Add(strNameArray[11], Convert.ToString(dt.Rows[0]["vgno_last"]));
                info.Add(strNameArray[12], Convert.ToString(dt.Rows[0]["inout"]));
                info.Add(strNameArray[13], Convert.ToString(dt.Rows[0]["trade"]));
                info.Add(strNameArray[14], Convert.ToString(dt.Rows[0]["pack"]));
                info.Add(strNameArray[15], Convert.ToString(dt.Rows[0]["mark"]));
                info.Add(strNameArray[16], Convert.ToString(dt.Rows[0]["company"]));
                info.Add("Order", strOrderLink);

                Json = JsonConvert.SerializeObject(new DicPackage(true, info, null).DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}