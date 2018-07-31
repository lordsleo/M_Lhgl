//
//文件名：    GetUncommitedMachineCommand.aspx.cs
//功能描述：  获取未提交过的派工计划数据
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
    public partial class GetUncommitedMachineCommand : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //用户编码
            var codeUser = Request.Params["CodeUser"];

            codeUser = "71B4CD89E6654D84B8AED9116DDB1C4C";
            //startRow = "1";
            //count = "15";

            try
            {
                if (codeUser == null)
                {
                    string warning = string.Format("参数CodeUser不能为null！举例：http://218.92.115.55/M_Lhgl/Service/Slip/GetUncommitedMachineCommand.aspx?CodeUser=71B4CD89E6654D84B8AED9116DDB1C4C");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;
                }

                string strSql =
                    string.Format(@"select pmno,cgno,client,taskno,cargo,operation,planweight,begintime,endtime,carrier1,carrier2,tallydate,code_tallyman2 from (select a.pmno,a.cgno,a.client,a.taskno,a.cargo,a.operation,a.planweight,a.begintime,a.endtime,a.carrier1,a.carrier2,a.tallydate,a.code_tallyman2
                                    from  vw_ps_mission_ma a
                                    where not exists(select * from vw_reg_tallybill b where a.pmno = b.pmno and a.cgno = b.cgno and b.mark_finish=1)) 
                                    where code_tallyman2='{0}' 
                                    order by tallydate desc",
                                    codeUser);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "暂无数据！").DicInfo());
                    return;
                }

                string[,] strArray = new string[dt.Rows.Count, 12];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    strArray[iRow, 0] = Convert.ToString(dt.Rows[iRow]["pmno"]);
                    strArray[iRow, 1] = Convert.ToString(dt.Rows[iRow]["cgno"]);
                    strArray[iRow, 2] = string.Empty;
                    strArray[iRow, 3] = Convert.ToString(dt.Rows[iRow]["client"]);
                    strArray[iRow, 4] = Convert.ToString(dt.Rows[iRow]["taskno"]);
                    strArray[iRow, 5] = Convert.ToString(dt.Rows[iRow]["cargo"]);
                    strArray[iRow, 6] = Convert.ToString(dt.Rows[iRow]["operation"]);
                    strArray[iRow, 7] = Convert.ToString(dt.Rows[iRow]["planweight"]);
                    strArray[iRow, 8] = string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[iRow]["begintime"])) == true ? string.Empty : StringTool.ToDayNightForSql(Convert.ToString(dt.Rows[iRow]["begintime"]));
                    strArray[iRow, 9] = string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[iRow]["endtime"])) == true ? string.Empty : StringTool.ToDayNightForSql(Convert.ToString(dt.Rows[iRow]["endtime"]));
                    strArray[iRow, 10] = Convert.ToString(dt.Rows[iRow]["carrier1"]);
                    strArray[iRow, 11] = Convert.ToString(dt.Rows[iRow]["carrier2"]);
                    //strArray[iRow, 12] = string.Empty;
                    //strArray[iRow, 13] = string.Empty;
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