//
//文件名：    GetEmployee.aspx.cs
//功能描述：  获取理货员列表
//创建时间：  2015/10/1
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
namespace M_Lhgl.Service.Handover
{
    public partial class GetEmployee : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //公司编码
            var codeCompany = Request.Params["CodeCompany"];

            //codeCompany = "14";

            try
            {
                if (codeCompany == null)
                {
                    string warning = string.Format("参数CodeCompany不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Handover/GetEmployee.aspx?CodeCompany=14");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;

                }
//                string strSql =
//                    string.Format(@"select distinct code,name,logogram 
//                                    from vw_employee_tally  
//                                    where code_company='{0}' and mark_audit='1'
//                                    order by logogram asc", 
//                                    codeCompany);
//                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCIport).ExecuteTable(strSql);
//                if (dt.Rows.Count <= 0)
//                {
//                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "暂无数据！").DicInfo());
//                    return;
//                }

//                string[,] strArray = new string[dt.Rows.Count, 3];
//                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
//                {
//                    strArray[iRow, 0] = Convert.ToString(dt.Rows[iRow]["code"]);
//                    strArray[iRow, 1] = Convert.ToString(dt.Rows[iRow]["name"]);
//                    strArray[iRow, 2] = Convert.ToString(dt.Rows[iRow]["logogram"]);
//                }

                string[,] strArray = new string[1, 3];
                for (int iRow = 0; iRow < 1; iRow++)
                {
                    strArray[0, 0] = "106945570C1A4D82971545D519AF2051";
                    strArray[0, 1] = "靳敏杰";
                    strArray[0, 2] = "JMJ";
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