//
//文件名：    GetStock.aspx.cs
//功能描述：  获取堆存查询列表数据
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

namespace M_Lhgl.Service.Stock
{
    public partial class GetStock : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //数据起始行
            var startRow = Request.Params["StartRow"];
            //行数
            var count = Request.Params["Count"];
            //公司编码
            var codeCompany = Request.Params["CodeCompany"];
            //货物
            var cargo = Request.Params["Cargo"];
            //货主
            var cargoOwner = Request.Params["CargoOwner"];
            //货代
            var client = Request.Params["Client"];
            //货场
            var storage = Request.Params["Storage"];


            cargo = cargo == null ? string.Empty : cargo;
            cargoOwner = cargoOwner == null ? string.Empty : cargoOwner;
            client = client == null ? string.Empty : client;
            storage = storage == null ? string.Empty : storage;

            //startRow = "1";
            //count = "15";
            //cargo = "煤炭";
            //cargoOwner = "北京弘帆";

            codeCompany = "693";

            try
            {
                if (startRow == null || count == null || codeCompany == null)
                {
                    string warning = string.Format("参数StartRow，Count，CodeCompany不能为null！举例：http://218.92.115.55/M_Lhgl/Service/Stock/GetStock.aspx?StartRow=1&Count=14&CodeCompany=14");
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, warning).DicInfo());
                    return;
                }

                string strFilter = string.Empty;
                string strTable = "VW_HS_STOCKDORMANT a";
                if (!string.IsNullOrWhiteSpace(codeCompany))
                {
                    strFilter += string.Format("a.code_company='{0}' and ", codeCompany);
                }
                if (!string.IsNullOrWhiteSpace(cargo))
                {
                    strFilter += string.Format("a.cargo='{0}' and ", cargo);
                }
                if (!string.IsNullOrWhiteSpace(cargoOwner))
                {
                    strFilter += string.Format("a.cargoowner='{0}' and ", cargoOwner);
                }
                if (!string.IsNullOrWhiteSpace(client))
                {
                    strFilter += string.Format("a.client='{0}' and ", client);
                }
                if (!string.IsNullOrWhiteSpace(storage))
                {
                    strFilter += string.Format("a.storage='{0}' and ", storage);
                }
                if (!string.IsNullOrWhiteSpace(strFilter))
                {
                    strFilter = strFilter.Remove(strFilter.Length - 5, 5);
                    strFilter = " where " + strFilter;
                }

                string strSql =
                    string.Format(@"select a.gbno,a.cargoowner,a.cargo,a.vgdisplay,a.client,a.storage,a.weight,a.code_storage,code_booth
                                    from {0}{1} order by  a.storagetype,a.storage",
                                    strTable, strFilter);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathJHHarbor).ExecuteTable(strSql, Convert.ToInt32(startRow), Convert.ToInt32(startRow) + Convert.ToInt32(count) - 1);
                if (dt.Rows.Count <= 0)
                {
                    string strWarning = startRow == "1" ? "暂无数据！" : "暂无更多数据！";
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, strWarning).DicInfo());
                    return;
                }

                string[,] strArray = new string[dt.Rows.Count, 9];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    strArray[iRow, 0] = Convert.ToString(dt.Rows[iRow]["gbno"]);
                    strArray[iRow, 1] = Convert.ToString(dt.Rows[iRow]["cargoowner"]);
                    strArray[iRow, 2] = Convert.ToString(dt.Rows[iRow]["cargo"]);
                    strArray[iRow, 3] = Convert.ToString(dt.Rows[iRow]["vgdisplay"]);
                    strArray[iRow, 4] = Convert.ToString(dt.Rows[iRow]["client"]);
                    strArray[iRow, 5] = Convert.ToString(dt.Rows[iRow]["storage"]);
                    strArray[iRow, 6] = Convert.ToString(dt.Rows[iRow]["weight"]);
                    strArray[iRow, 7] = Convert.ToString(dt.Rows[iRow]["code_storage"]);
                    strArray[iRow, 8] = Convert.ToString(dt.Rows[iRow]["code_booth"]);
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