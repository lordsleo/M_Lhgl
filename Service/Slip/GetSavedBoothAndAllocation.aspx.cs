//
//文件名：    GetSavedBoothAndAllocation.aspx.cs
//功能描述：  获取暂存桩脚和货位（堆）
//创建时间：  2015/11/16
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
    public partial class GetSavedBoothAndAllocation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //理货单编码
            var tbno = Request.Params["Tbno"];
            //载体类型
            var carriesType = Request.Params["CarriesType"];

            try
            {
                if (tbno == null || carriesType == null)
                {
                    string warning = string.Format("参数Tbno，CarriesType不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Slip/GetSavedBoothAndAllocation.aspx?Tbno=20151221066196&CarriesType=0");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;

                }

                string strBooth = string.Empty;
                string strAllocation = string.Empty;
                string strSql = string.Empty;

                if (carriesType == "0")
                {
                    strSql = 
                        string.Format(@"select b.booth,c.allocation
                                        from VW_REG_TALLYBILL a, baseresource.TB_CODE_BOOTH b,baseresource.tb_code_allocation c 
                                        where a.CODE_BOOTH=b.code_booth(+) and a.CODE_ALLOCATION=c.code_allocation(+) and  a.TBNO='{0}'",
                                        tbno);
                }
                else
                {
                    strSql =
                        string.Format(@"select b.booth,c.allocation
                                        from VW_REG_TALLYBILL a, baseresource.TB_CODE_BOOTH b,baseresource.tb_code_allocation c 
                                        where a.CODE_BOOTHLAST=b.code_booth(+) and a.CODE_ALLOCATIONLAST=c.code_allocation(+) and  a.TBNO='{0}'",
                                        tbno);
                }


                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count > 0)
                {
                    strBooth = Convert.ToString(dt.Rows[0]["booth"]);
                    strAllocation = Convert.ToString(dt.Rows[0]["allocation"]);
                }

                Dictionary<string, object> info = new Dictionary<string, object>();
                info.Add("Booth", strBooth);
                info.Add("Allocation", strAllocation);

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