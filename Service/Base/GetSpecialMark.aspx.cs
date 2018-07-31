//
//文件名：    GetSpecialMark.cs
//功能描述：  获取子过程标志列表数据（基础数据）
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

namespace M_Lhgl.Service.Base
{
    public partial class GetSpecialMark : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //派工编码
            var pmno = Request.Params["Pmno"];
            //公司编码
            var codeCompany = Request.Params["CodeCompany"];

            try
            {
                if (pmno == null)
                {
                    string warning = string.Format("参数Pmno，CodeCompany不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Base/GetSpecialMark.aspx?Pmno=20151119000281&CodeCompany=14");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;
                }

                string strCodeOperation = string.Empty;
                string strSql =
                        string.Format(@"select code_storage,code_storagelast,mark_tallybill,code_operation,code_operation_fact,CODE_CARRIER_S,CODE_CARRIER_E,vgdisplay,vgno,cabin,vgdisplaylast,vgnolast,cabinlast,code_carrier,code_carrierlast,nvessel,
                                        code_nvessel,nvessellast,code_nvessellast,carrier1,carrier2,nsno,tallydate,code_opstype,code_workflow,code_worktime,code_teamtype,code_trainroad,code_motorcade,begintime,endtime,nsno_barge,nsno_bargelast          
                                        from vw_ps_mission where pmno='{0}'",
                                        pmno);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count > 0)
                {
                    strCodeOperation = Convert.ToString(dt.Rows[0]["code_operation_fact"]);
                }
                
                strSql =
                    string.Format(@"select distinct CODE_SPECIALMARK,SPECIALMARK,LOGOGRAM 
                                    from TB_CODE_SPECIALMARK 
                                    where code_operation='{0}' and code_company='{1}'
                                    order by logogram",
                                    strCodeOperation, codeCompany);
                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCBase).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "暂无数据！").DicInfo());
                    return;
                }

                string[,] strArray = new string[dt.Rows.Count, 3];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    strArray[iRow, 0] = Convert.ToString(dt.Rows[iRow]["CODE_SPECIALMARK"]);
                    strArray[iRow, 1] = Convert.ToString(dt.Rows[iRow]["SPECIALMARK"]);
                    strArray[iRow, 2] = Convert.ToString(dt.Rows[iRow]["LOGOGRAM"]);
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