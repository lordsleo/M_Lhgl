//
//文件名：    GoodBillE.aspx.cs
//功能描述：  票货数据集
//创建时间：  2015/10/24
//作者：      
//修改时间：  暂无
//修改描述：  暂无
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M_Lhgl.Service.Slip
{
    public class GoodsBillE
    {
        #region 公共属性
        //票货1名称
        private string strGoodsBill1Name;
        //票货2名称
        private string strGoodsBill2Name;
        //源场地编码
        private string strCodeStorage;
        //目的场地编码
        private string strCodeStorageLast;
        //理货单生成标志
        private string strMarkTallyBill;
        //调发货标志
        private string strMarkExchange;
        //目标票货标志
        private string strMarkLast;
        //借贷委托标志
        private string strMarkBorrow;
        //委托操作过程
        private string strCodeOperation;
        //调度操作过程
        private string strCodeOperationFact;


        public string StrGoodsBill1Name
        {
            get { return strGoodsBill1Name; }
            set { strGoodsBill1Name = value; }
        }
        public string StrGoodsBill2Name
        {
            get { return strGoodsBill2Name; }
            set { strGoodsBill2Name = value; }
        }
        public string StrCodeStorage
        {
            get { return strCodeStorage; }
            set { strCodeStorage = value; }
        }
        public string StrCodeStorageLast
        {
            get { return strCodeStorageLast; }
            set { strCodeStorageLast = value; }
        }
        public string StrMarkTallyBill
        {
            get { return strMarkTallyBill; }
            set { strMarkTallyBill = value; }
        }
        public string StrMarkExchange
        {
            get { return strMarkExchange; }
            set { strMarkExchange = value; }
        }
        public string StrMarkLast
        {
            get { return strMarkLast; }
            set { strMarkLast = value; }
        }
        public string StrMarkBorrow
        {
            get { return strMarkBorrow; }
            set { strMarkBorrow = value; }
        }
        public string StrCodeOperation
        {
            get { return strCodeOperation; }
            set { strCodeOperation = value; }
        }
        public string StrCodeOperationFact
        {
            get { return strCodeOperationFact; }
            set { strCodeOperationFact = value; }
        }


        #endregion

        #region 公共方法
        /// <summary>
        /// 初始化理货作业票数据集
        /// </summary>
        public GoodsBillE()
        {
            strGoodsBill1Name = string.Empty;
            strGoodsBill2Name = string.Empty;
            strCodeStorage = string.Empty;
            strCodeStorageLast = string.Empty;
            strMarkTallyBill = string.Empty;
            strMarkExchange = string.Empty;
            strMarkLast = string.Empty;
            StrMarkBorrow = string.Empty;
            strCodeOperation = string.Empty;
            strCodeOperationFact = string.Empty;
        }

        #endregion

    }
}