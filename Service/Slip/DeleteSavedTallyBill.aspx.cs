//
//文件名：    DeleteSavedTallyBill.aspx.cs
//功能描述：  删除已暂存理货作业票
//创建时间：  2015/11/06
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
    public partial class DeleteSavedTallyBill : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //理货单编码
            var tbno = Request.Params["Tbno"];

            try
            {
                if (tbno == null)
                {
                    string warning = string.Format("参数Tbno不能为nul！举例：http://218.92.115.55/M_Lhgl/Service/Slip/DeleteSavedTallyBill.aspx?Tbno=20151112064117");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;
                }
                if (!DeleteTB(tbno) || !DeleteMachine(tbno) || !DeleteWorkTeam(tbno))
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "删除失败！").DicInfo());
                }

                Json = JsonConvert.SerializeObject(new DicPackage(true, null, "删除成功").DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;

        #region 删除作业票
        /// <summary>
        /// 删除作业票
        /// </summary>
        /// <param name="strCgno">理货单编码</param>
        /// <returns>DataTable</returns>
        private bool DeleteTB(string tbno)
        {
            string strSql =
                 string.Format("delete from TB_HS_TALLYBILL where tbno='{0}'", tbno);
            new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteNonQuery(strSql);
            return true;
        }
        #endregion

        #region 删除机械
        /// <summary>
        /// 删除机械
        /// </summary>
        /// <param name="strCgno">理货单编码</param>
        /// <returns>DataTable</returns>
        private bool DeleteMachine(string tbno)
        {
            string strSql =
                 string.Format("delete from TB_TALLY_MACHINE_DRIVER where tbno='{0}'", tbno);
            new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteNonQuery(strSql);
            return true;
        }
        #endregion

        #region 删除班组
        /// <summary>
        /// 删除班组
        /// </summary>
        /// <param name="strCgno">理货单编码</param>
        /// <returns>DataTable</returns>
        private bool DeleteWorkTeam(string tbno)
        {
            string strSql =
                 string.Format("delete from TB_TALLY_WORKER where tbno='{0}'", tbno);
            new Leo.Oracle.DataAccess(RegistryKey.KeyPathZCHarbor).ExecuteNonQuery(strSql);
            return true;
        }
        #endregion
    }
}