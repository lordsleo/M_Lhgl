//
//文件名：    WorkE.aspx.cs
//功能描述：  开工、完工数据集
//创建时间：  2015/09/24
//作者：      
//修改时间：  暂无
//修改描述：  暂无
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M_Lhgl.Service.Vehicle
{
    public class WorkE
    {
        #region 公共属性

        //号码字段名称
        private string strNoFieldName;
        //黑名单
        private string strBlackList;
        //卡状态
        private string strState;
        //卡禁用
        private string strForbidMark;
        //车状态
        private string strVehState;
        //卡号
        private string strCardNo;
        //内部委托号
        private string strCgno;
        //ID
        private string strId;
        //船名
        private string strVessel;
        //货代
        private string strClient;
        //货物
        private string strCargo;
        //场地
        private string strStorage;
        //货位
        private string strBooth;
        //集疏港
        private string strFullOrEmpty;
        //装卸货
        private string strWorkStyle;
        //任务号
        private string strTaskno;
        //通行证号
        private string strExterNo;
        //衡重
        private string strWeight;
        //申报时间
        private string strSubmittime;
        //过磅时间
        private string strRecordtime;
        //开工时间
        private string strStartTime;
        //放行标志
        private string strAuditMark;
        //放行时间
        private string strAuditTime;
        //货重
        private string strWeightCargo;

        public string StrNoFieldName
        {
            get { return strNoFieldName; }
            set { strNoFieldName = value; }
        }
        public string StrBlackList
        {
            get { return strBlackList; }
            set { strBlackList = value; }
        }
        public string StrState
        {
            get { return strState; }
            set { strState = value; }
        }
        public string StrForbidMark
        {
            get { return strForbidMark; }
            set { strForbidMark = value; }
        }
        public string StrVehState
        {
            get { return strVehState; }
            set { strVehState = value; }
        }
        public string StrCardNo
        {
            get { return strCardNo; }
            set { strCardNo = value; }
        }
        public string StrCgno
        {
            get { return strCgno; }
            set { strCgno = value; }
        }
        public string StrId
        {
            get { return strId; }
            set { strId = value; }
        }
        //车牌号
        private string strVehicle;

        public string StrVehicle
        {
            get { return strVehicle; }
            set { strVehicle = value; }
        }
        public string StrVessel
        {
            get { return strVessel; }
            set { strVessel = value; }
        }
        public string StrClient
        {
            get { return strClient; }
            set { strClient = value; }
        }
        public string StrCargo
        {
            get { return strCargo; }
            set { strCargo = value; }
        }
        public string StrStorage
        {
            get { return strStorage; }
            set { strStorage = value; }
        }
        public string StrBooth
        {
            get { return strBooth; }
            set { strBooth = value; }
        }
        public string StrFullOrEmpty
        {
            get { return strFullOrEmpty; }
            set { strFullOrEmpty = value; }
        }
        public string StrWorkStyle
        {
            get { return strWorkStyle; }
            set { strWorkStyle = value; }
        }
        public string StrTaskno
        {
            get { return strTaskno; }
            set { strTaskno = value; }
        }
        public string StrExterNo
        {
            get { return strExterNo; }
            set { strExterNo = value; }
        }
        public string StrWeight
        {
            get { return strWeight; }
            set { strWeight = value; }
        }
        public string StrSubmittime
        {
            get { return strSubmittime; }
            set { strSubmittime = value; }
        }
        public string StrRecordtime
        {
            get { return strRecordtime; }
            set { strRecordtime = value; }
        }
        public string StrStartTime
        {
            get { return strStartTime; }
            set { strStartTime = value; }
        }
        public string StrAuditMark
        {
            get { return strAuditMark; }
            set { strAuditMark = value; }
        }
        public string StrAuditTime
        {
            get { return strAuditTime; }
            set { strAuditTime = value; }
        }
        public string StrWeightCargo
        {
            get { return strWeightCargo; }
            set { strWeightCargo = value; }
        }

        #endregion

        #region 公共方法
        /// <summary>
        /// 初始化开工、完工数据集
        /// </summary>
        public WorkE()
        { 
            //ID
            strId = string.Empty;   
            //车牌号
            strVehicle = string.Empty;   
            //船名
            strVessel = string.Empty;
            //货代
            strClient = string.Empty;
            //货物
            strCargo = string.Empty;
            //场地
            strStorage = string.Empty;
            //货位
            strBooth = string.Empty;
            //集疏港
            strFullOrEmpty = string.Empty;
            //装卸货
            strWorkStyle = string.Empty;
            //任务号
            strTaskno = string.Empty;
            //通行证号
            strExterNo = string.Empty;
            //衡重
            strWeight = string.Empty;
            //申报时间
            strSubmittime = string.Empty;
            //过磅时间
            strRecordtime = string.Empty;
            //开工时间
            strStartTime = string.Empty;
            //放行标志
            strAuditMark = string.Empty;
            //放行时间
            strAuditTime = string.Empty;
            //货重
            strWeightCargo = string.Empty;
        }
        #endregion

    }
}