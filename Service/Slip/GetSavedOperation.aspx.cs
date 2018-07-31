//
//文件名：    GetSavedOperation.aspx.cs
//功能描述：  获取暂存子过程
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
    public partial class GetSavedOperation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //理货单编码
            var tbno = Request.Params["Tbno"];

            try
            {
                if (tbno == null)
                {
                    string warning = string.Format("参数Tbno不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Slip/GetSavedOperation.aspx?Tbno=20151212065573");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;

                }

                string[,] strArray = null;
                string strCodeOperationFact = string.Empty;
                string strSql =
                    string.Format(@"select code_operation_fact
                                    from VW_REG_TALLYBILL 
                                    where tbno='{0}'",
                                    tbno);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count > 0)
                {
                    strCodeOperationFact = Convert.ToString(dt.Rows[0]["code_operation_fact"]);
                }

                strSql = string.Format(@"select code_operation_fact code,operationfact displayname,logogram  
                                         from nharbor.vw_br_operationfact where code_operation_fact='{0}' order by code", strCodeOperationFact);
                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count > 0)
                {
                    strArray = new string[dt.Rows.Count, 3];
                    for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                    {
                        strArray[iRow, 0] = Convert.ToString(dt.Rows[iRow]["code"]);
                        strArray[iRow, 1] = Convert.ToString(dt.Rows[iRow]["displayname"]);
                        strArray[iRow, 2] = Convert.ToString(dt.Rows[iRow]["logogram"]);
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