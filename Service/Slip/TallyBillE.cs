//
//文件名：    TallyBillE.aspx.cs
//功能描述：  理货作业票数据集
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
    /// <summary>
    /// 机械
    /// </summary>
    public struct Machine
    {
        //选择标志
        public string select { get; set; }
        //机械名称
        public string machine { get; set; }
        //司机
        public string name { get; set; }
        //开始时间
        public string begintime { get; set; }
        //结束时间
        public string endtime { get; set; }
        //件
        public string amount { get; set; }
        //吨
        public string weight { get; set; }
        //数量
        public string count { get; set; }
        //机械编号
        public string code_machine { get; set; }
        //司机编号
        public string workno { get; set; }
        //司机部门编码
        public string code_department { get; set; }
    }

    /// <summary>
    /// 班组
    /// </summary>
    public struct TeamWork
    {
        //选择标志
        public string select { get; set; }
        //机械名称
        public string workteam { get; set; }
        //司机
        public string name { get; set; }
        //开始时间
        public string begintime { get; set; }
        //结束时间
        public string endtime { get; set; }
        //件
        public string amount { get; set; }
        //吨
        public string weight { get; set; }
        //数量
        public string count { get; set; }
        //班别编号
        public string code_workteam { get; set; }
        //工号
        public string workno { get; set; }
    }


    public class TallyBillE
    {
        #region 公共属性

        /// <summary>
        /// 理货作业票编码
        /// </summary>
        private string tbno;
        /// <summary>
        /// 
        /// </summary>
        private string obno;
        /// <summary>
        /// 调度执行编码
        /// </summary>
        private string pmno;
        /// <summary>
        /// 源票货编码
        /// </summary>
        private string gbno;
        /// <summary>
        /// 生成转卡作业票的源票货编码
        /// </summary>
        private string gbnoCard;
        /// <summary>
        /// 记账日期
        /// </summary>
        private DateTime signdate;
        /// <summary>
        /// 部门代码
        /// </summary>
        private string code_Department;
        /// <summary>
        /// 外部流通作业编码(纸质/生成)
        /// </summary>
        private string taskno;
        /// <summary>
        /// 源场地代码
        /// </summary>
        private string code_Storage;
        /// <summary>
        /// 源货位代码
        /// </summary>
        private string code_Booth;
        /// <summary>
        /// 源航次编码
        /// </summary>
        private string vgno;
        /// <summary>
        /// 源舱别
        /// </summary>
        private string cabin;
        /// <summary>
        /// 源车别代码
        /// </summary>
        private string code_Carrier;
        /// <summary>
        /// 源车号
        /// </summary>
        private string carriernum;
        /// <summary>
        /// 源驳船船舶规范编码
        /// </summary>
        private string code_nvessel;
        /// <summary>
        /// 源驳船属性
        /// </summary>
        private string bargepro;
        /// <summary>
        /// 件重
        /// </summary>
        private string pieceweight;
        /// <summary>
        /// 件数
        /// </summary>
        private Nullable<int> amount;
        /// <summary>
        /// 重量
        /// </summary>
        private Nullable<decimal> weight;
        /// <summary>
        /// 数量
        /// </summary>
        private Nullable<decimal> worknum;
        /// <summary>
        /// 计量单位代码
        /// </summary>
        private string code_Measure;
        /// <summary>
        /// 理货开始时间
        /// </summary>
        private string begintime;
        /// <summary>
        /// 理货结束时间
        /// </summary>
        private string endtime;
        /// <summary>
        /// 用户代码(理货作业票单证录入员)
        /// </summary>
        private string code_User;
        /// <summary>
        /// 用户姓名(理货作业票单证录入员)
        /// </summary>
        private string username;
        /// <summary>
        /// 委托操作类型
        /// </summary>
        private string code_Operation;
        /// <summary>
        /// 实际作业方式
        /// </summary>
        private string code_Operation_Fact;
        /// <summary>
        /// 子过程特殊
        /// </summary>
        private string code_SpecialMark;
        /// <summary>
        /// 
        /// </summary>
        private string code_workingarea1;
        /// <summary>
        /// 
        /// </summary>
        private string code_workingarealast1;
        /// <summary>
        /// 
        /// </summary>
        private string code_allocation1;
        /// <summary>
        /// 
        /// </summary>
        private string code_allocationlast1;
        /// <summary>
        /// 质量代码
        /// </summary>
        private string code_Quality;
        /// <summary>
        /// 作业班组代码
        /// </summary>
        private string code_Workteam;
        /// <summary>
        /// 调度班别
        /// </summary>
        private string code_Teamtype;
        /// <summary>
        /// 用户代码(理货员编码)
        /// </summary>
        private string code_Tallyman;
        /// <summary>
        /// 用户姓名(理货员编码)
        /// </summary>
        private string tallyman;
        /// <summary>
        /// 目的场地代码
        /// </summary>
        private string code_Storagelast;
        /// <summary>
        /// 目的货位代码
        /// </summary>
        private string code_Boothlast;
        /// <summary>
        /// 目的航次编码
        /// </summary>
        private string vgnolast;
        /// <summary>
        /// 目的航次舱别
        /// </summary>
        private string cabinlast;
        /// <summary>
        /// 目的车别代码
        /// </summary>
        private string code_Carrierlast;
        /// <summary>
        /// 目的车号
        /// </summary>
        private string carriernumlast;
        /// <summary>
        /// 目的驳船船舶规范编码
        /// </summary>
        private string code_nvessellast;
        /// <summary>
        /// 目的驳船属性
        /// </summary>
        private string bargeprolast;
        /// <summary>
        /// 源载体名称
        /// </summary>
        private string carrier1;
        /// <summary>
        /// 源载体属性
        /// </summary>
        private string carrier1num;
        /// <summary>
        /// 目的载体名称
        /// </summary>
        private string carrier2;
        /// <summary>
        /// 目的载体属性
        /// </summary>
        private string carrier2num;
        /// <summary>
        /// 进出库类别代码
        /// </summary>
        private string code_Opstype;
        /// <summary>
        /// 备注
        /// </summary>
        private string remark;
        /// <summary>
        /// 源票货描述
        /// </summary>
        private string gbdisplay;
        /// <summary>
        /// 理货作业票理货批次
        /// </summary>
        private string tallynum;
        /// <summary>
        /// 目的票货编码
        /// </summary>
        private string gbnolast;
        /// <summary>
        /// 目的票货描述
        /// </summary>
        private string gbdisplaylast;
        /// <summary>
        /// 原始票货编码
        /// </summary>
        private string gbnofirst;
        /// <summary>
        /// 体积
        /// </summary>
        private string volume;
        /// <summary>
        /// 作业工班代码
        /// </summary>
        private string code_Worktime;
        /// <summary>
        /// 集装箱箱号
        /// </summary>
        private string containernum;
        /// <summary>
        /// 清场标记
        /// </summary>
        private string mark_Clearbooth;
        /// <summary>
        /// 内部委托编码
        /// </summary>
        private string cgno;
        /// <summary>
        /// 父理货作业票编码
        /// </summary>
        private string parenttbno;
        /// <summary>
        ///  规格（特殊分票）
        /// </summary>
        private string spec;
        /// <summary>
        ///  出场时减堆存帐的票货
        /// </summary>
        private string gbnofromstock;
        /// <summary>
        ///  公司代码
        /// </summary>
        private string code_Company;
        /// <summary>
        ///  库区代码
        /// </summary>
        private string code_Section;
        /// <summary>
        ///  车数
        /// </summary>
        private Nullable<decimal> trainnum;
        /// <summary>
        ///  装车点
        /// </summary>
        private string code_Loadplace;
        /// <summary>
        ///  理货日期
        /// </summary>
        private DateTime tallydate;
        /// <summary>
        ///  操作工艺
        /// </summary>
        private string code_workflow;
        /// <summary>
        ///  火车道别编码
        /// </summary>
        private string code_trainroad;
        /// <summary>
        ///  车队编码
        /// </summary>
        private string code_motorcade;
        /// <summary>
        ///  司机人数
        /// </summary>
        private string drivernum;
        /// <summary>
        ///  工人人数
        /// </summary>
        private string workernum;
        /// <summary>
        ///  机械描述
        /// </summary>
        private string machine;
        /// <summary>
        ///  创建人编码
        /// </summary>
        private string creator;
        /// <summary>
        ///  创建人名称
        /// </summary>
        private string creatorname;
        /// <summary>
        ///  创建时间
        /// </summary>
        private Nullable<DateTime> createTime;
        /// <summary>
        ///  0:生成；1：录入；2：系统产生
        /// </summary>
        private string code_billtype;
        /// <summary>
        ///  商务计划结束标志
        /// </summary>
        private string mark_comp_plan;
        /// <summary>
        ///  票货结束标志
        /// </summary>
        private string mark_comp_goodsbill;
        /// <summary>
        ///  船舶结束标志
        /// </summary>
        private string mark_comp_ship;
        /// <summary>
        ///  审核标志
        /// </summary>
        private string mark_Audit;
        /// <summary>
        ///  审核时间
        /// </summary>
        private Nullable<DateTime> auditTime;
        /// <summary>
        ///  审核人
        /// </summary>
        private string auditor;
        /// <summary>
        ///  审核人名称
        /// </summary>
        private string auditorname;
        /// <summary>
        ///  实发场地代码
        /// </summary>
        private string code_Storage_Stock;
        /// <summary>
        ///  实发货位代码
        /// </summary>
        private string code_Booth_Stock;
        /// <summary>
        ///  
        /// </summary>
        private string code_allocationstock1;
        /// <summary>
        ///  作业班组描述
        /// </summary>
        private string workteam;
        /// <summary>
        ///  分解过程的进出库类别代码
        /// </summary>
        private string code_opstype_fact;
        /// <summary>
        ///  清舱数
        /// </summary>
        private int clear_num;
        /// <summary>
        ///  清舱舱别
        /// </summary>
        private string clear_remark;
        /// <summary>
        ///  交接数
        /// </summary>
        private Nullable<int> join_num;
        /// <summary>
        ///  换包装后的件数
        /// </summary>
        private Nullable<int> amount2;
        /// <summary>
        ///  唛头1（驳船名）
        /// </summary>
        private string mark1;
        /// <summary>
        ///  唛头2（驳船名）
        /// </summary>
        private string mark2;
        /// <summary>
        ///  唛头3（驳船名）
        /// </summary>
        private string mark3;
        /// <summary>
        ///  票货本机内码
        /// </summary>
        private string gBNO_PHASE;
        /// <summary>
        ///  颜色
        /// </summary>
        private string color;
        /// <summary>
        ///  新批次号
        /// </summary>
        private string lot2;
        /// <summary>
        ///  作业票编号
        /// </summary>
        private string zYP_ID;
        /// <summary>
        ///  修改人编码
        /// </summary>
        private string modifier;
        /// <summary>
        ///  修改人名称
        /// </summary>
        private string modifiername;
        /// <summary>
        ///  修改时间
        /// </summary>
        private Nullable<DateTime> modifyTime;
        /// <summary>
        ///  
        /// </summary>
        private string cODE_MACHINE;
        /// <summary>
        ///  
        /// </summary>
        private string mACHINE;
        /// <summary>
        ///  
        /// </summary>
        private string cODE_MACHINETYPE;
        /// <summary>
        ///  
        /// </summary>
        private string mACHINETYPE;
        /// <summary>
        ///  
        /// </summary>
        private string cODE_MAN;
        /// <summary>
        ///  
        /// </summary>
        private string mAN;
        /// <summary>
        ///  
        /// </summary>
        private string nSNO_BARGE;
        /// <summary>
        ///  
        /// </summary>
        private string nSNO_BARGELAST;
        /// <summary>
        ///  理货票完成标志
        /// </summary>
        private Nullable<char> mark_finish;
        /// <summary>
        /// 机械
        /// </summary>
        private string machineData;
        /// <summary>
        /// 班组
        /// </summary>
        private string teamWorkData;
        /// <summary>
        /// 起堆
        /// </summary>
        private string carrier1A;
        /// <summary>
        /// 止堆
        /// </summary>
        private string carrier2A;


        public string Tbno
        {
            get { return tbno; }
            set { tbno = value; }
        }
        public string Obno
        {
            get { return obno; }
            set { obno = value; }
        }
        public string Pmno
        {
            get { return pmno; }
            set { pmno = value; }
        }
        public string Gbno
        {
            get { return gbno; }
            set { gbno = value; }
        }
        public string GbnoCard
        {
            get { return gbnoCard; }
            set { gbnoCard = value; }
        }
        public DateTime Signdate
        {
            get { return signdate; }
            set { signdate = value; }
        }
        public string Code_Department
        {
            get { return code_Department; }
            set { code_Department = value; }
        }
        public string Taskno
        {
            get { return taskno; }
            set { taskno = value; }
        }
        public string Code_Storage
        {
            get { return code_Storage; }
            set { code_Storage = value; }
        }
        public string Code_Booth
        {
            get { return code_Booth; }
            set { code_Booth = value; }
        }
        public string Vgno
        {
            get { return vgno; }
            set { vgno = value; }
        }
        public string Cabin
        {
            get { return cabin; }
            set { cabin = value; }
        }
        public string Code_Carrier
        {
            get { return code_Carrier; }
            set { code_Carrier = value; }
        }
        public string Carriernum
        {
            get { return carriernum; }
            set { carriernum = value; }
        }
        public string Code_nvessel
        {
            get { return code_nvessel; }
            set { code_nvessel = value; }
        }
        public string Bargepro
        {
            get { return bargepro; }
            set { bargepro = value; }
        }
        public string Pieceweight
        {
            get { return pieceweight; }
            set { pieceweight = value; }
        }
        public Nullable<int> Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        public Nullable<decimal> Weight
        {
            get { return weight; }
            set { weight = value; }
        }
        public Nullable<decimal> Worknum
        {
            get { return worknum; }
            set { worknum = value; }
        }
        public string Code_Measure
        {
            get { return code_Measure; }
            set { code_Measure = value; }
        }
        public string Begintime
        {
            get { return begintime; }
            set { begintime = value; }
        }
        public string Endtime
        {
            get { return endtime; }
            set { endtime = value; }
        }
        public string Code_User
        {
            get { return code_User; }
            set { code_User = value; }
        }
        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        public string Code_Operation
        {
            get { return code_Operation; }
            set { code_Operation = value; }
        }
        public string Code_Operation_Fact
        {
            get { return code_Operation_Fact; }
            set { code_Operation_Fact = value; }
        }
        public string Code_SpecialMark
        {
            get { return code_SpecialMark; }
            set { code_SpecialMark = value; }
        }
        public string code_workingarea
        {
            get { return code_workingarea1; }
            set { code_workingarea1 = value; }
        }
        public string code_workingarealast
        {
            get { return code_workingarealast1; }
            set { code_workingarealast1 = value; }
        }
        public string code_allocation
        {
            get { return code_allocation1; }
            set { code_allocation1 = value; }
        }
        public string code_allocationlast
        {
            get { return code_allocationlast1; }
            set { code_allocationlast1 = value; }
        }
        public string Code_Quality
        {
            get { return code_Quality; }
            set { code_Quality = value; }
        }
        public string Code_Workteam
        {
            get { return code_Workteam; }
            set { code_Workteam = value; }
        }
        public string Code_Teamtype
        {
            get { return code_Teamtype; }
            set { code_Teamtype = value; }
        }
        public string Code_Tallyman
        {
            get { return code_Tallyman; }
            set { code_Tallyman = value; }
        }
        public string Tallyman
        {
            get { return tallyman; }
            set { tallyman = value; }
        }
        public string Code_Storagelast
        {
            get { return code_Storagelast; }
            set { code_Storagelast = value; }
        }
        public string Code_Boothlast
        {
            get { return code_Boothlast; }
            set { code_Boothlast = value; }
        }
        public string Vgnolast
        {
            get { return vgnolast; }
            set { vgnolast = value; }
        }
        public string Cabinlast
        {
            get { return cabinlast; }
            set { cabinlast = value; }
        }
        public string Code_Carrierlast
        {
            get { return code_Carrierlast; }
            set { code_Carrierlast = value; }
        }
        public string Carriernumlast
        {
            get { return carriernumlast; }
            set { carriernumlast = value; }
        }
        public string Code_nvessellast
        {
            get { return code_nvessellast; }
            set { code_nvessellast = value; }
        }
        public string Bargeprolast
        {
            get { return bargeprolast; }
            set { bargeprolast = value; }
        }
        public string Carrier1
        {
            get { return carrier1; }
            set { carrier1 = value; }
        }
        public string Carrier1num
        {
            get { return carrier1num; }
            set { carrier1num = value; }
        }
        public string Carrier2
        {
            get { return carrier2; }
            set { carrier2 = value; }
        }
        public string Carrier2num
        {
            get { return carrier2num; }
            set { carrier2num = value; }
        }
        public string Code_Opstype
        {
            get { return code_Opstype; }
            set { code_Opstype = value; }
        }
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        public string Gbdisplay
        {
            get { return gbdisplay; }
            set { gbdisplay = value; }
        }
        public string Tallynum
        {
            get { return tallynum; }
            set { tallynum = value; }
        }
        public string Gbnolast
        {
            get { return gbnolast; }
            set { gbnolast = value; }
        }
        public string Gbdisplaylast
        {
            get { return gbdisplaylast; }
            set { gbdisplaylast = value; }
        }
        public string Gbnofirst
        {
            get { return gbnofirst; }
            set { gbnofirst = value; }
        }
        public string Volume
        {
            get { return volume; }
            set { volume = value; }
        }
        public string Code_Worktime
        {
            get { return code_Worktime; }
            set { code_Worktime = value; }
        }
        public string Containernum
        {
            get { return containernum; }
            set { containernum = value; }
        }
        public string Mark_Clearbooth
        {
            get { return mark_Clearbooth; }
            set { mark_Clearbooth = value; }
        }
        public string Cgno
        {
            get { return cgno; }
            set { cgno = value; }
        }
        public string Parenttbno
        {
            get { return parenttbno; }
            set { parenttbno = value; }
        }
        public string Spec
        {
            get { return spec; }
            set { spec = value; }
        }
        public string Gbnofromstock
        {
            get { return gbnofromstock; }
            set { gbnofromstock = value; }
        }
        public string Code_Company
        {
            get { return code_Company; }
            set { code_Company = value; }
        }
        public string Code_Section
        {
            get { return code_Section; }
            set { code_Section = value; }
        }
        public Nullable<decimal> Trainnum
        {
            get { return trainnum; }
            set { trainnum = value; }
        }
        public string Code_Loadplace
        {
            get { return code_Loadplace; }
            set { code_Loadplace = value; }
        }
        public DateTime Tallydate
        {
            get { return tallydate; }
            set { tallydate = value; }
        }
        public string Code_workflow
        {
            get { return code_workflow; }
            set { code_workflow = value; }
        }
        public string Code_trainroad
        {
            get { return code_trainroad; }
            set { code_trainroad = value; }
        }
        public string Code_motorcade
        {
            get { return code_motorcade; }
            set { code_motorcade = value; }
        }
        public string Drivernum
        {
            get { return drivernum; }
            set { drivernum = value; }
        }
        public string Workernum
        {
            get { return workernum; }
            set { workernum = value; }
        }
        public string Machine
        {
            get { return machine; }
            set { machine = value; }
        }
        public string Creator
        {
            get { return creator; }
            set { creator = value; }
        }
        public string Creatorname
        {
            get { return creatorname; }
            set { creatorname = value; }
        }
        public Nullable<DateTime> CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }
        public string Code_billtype
        {
            get { return code_billtype; }
            set { code_billtype = value; }
        }
        public string Mark_comp_plan
        {
            get { return mark_comp_plan; }
            set { mark_comp_plan = value; }
        }
        public string Mark_comp_goodsbill
        {
            get { return mark_comp_goodsbill; }
            set { mark_comp_goodsbill = value; }
        }
        public string Mark_comp_ship
        {
            get { return mark_comp_ship; }
            set { mark_comp_ship = value; }
        }
        public string Mark_Audit
        {
            get { return mark_Audit; }
            set { mark_Audit = value; }
        }
        public Nullable<DateTime> AuditTime
        {
            get { return auditTime; }
            set { auditTime = value; }
        }
        public string Auditor
        {
            get { return auditor; }
            set { auditor = value; }
        }
        public string Auditorname
        {
            get { return auditorname; }
            set { auditorname = value; }
        }
        public string Code_Storage_Stock
        {
            get { return code_Storage_Stock; }
            set { code_Storage_Stock = value; }
        }
        public string Code_Booth_Stock
        {
            get { return code_Booth_Stock; }
            set { code_Booth_Stock = value; }
        }
        public string code_allocationstock
        {
            get { return code_allocationstock1; }
            set { code_allocationstock1 = value; }
        }
        public string Workteam
        {
            get { return workteam; }
            set { workteam = value; }
        }
        public string Code_opstype_fact
        {
            get { return code_opstype_fact; }
            set { code_opstype_fact = value; }
        }
        public int Clear_num
        {
            get { return clear_num; }
            set { clear_num = value; }
        }
        public string Clear_remark
        {
            get { return clear_remark; }
            set { clear_remark = value; }
        }
        public Nullable<int> Join_num
        {
            get { return join_num; }
            set { join_num = value; }
        }
        public Nullable<int> Amount2
        {
            get { return amount2; }
            set { amount2 = value; }
        }
        public string Mark1
        {
            get { return mark1; }
            set { mark1 = value; }
        }
        public string Mark2
        {
            get { return mark2; }
            set { mark2 = value; }
        }
        public string Mark3
        {
            get { return mark3; }
            set { mark3 = value; }
        }
        public string GBNO_PHASE
        {
            get { return gBNO_PHASE; }
            set { gBNO_PHASE = value; }
        }
        public string Color
        {
            get { return color; }
            set { color = value; }
        }
        public string Lot2
        {
            get { return lot2; }
            set { lot2 = value; }
        }
        public string ZYP_ID
        {
            get { return zYP_ID; }
            set { zYP_ID = value; }
        }
        public string Modifier
        {
            get { return modifier; }
            set { modifier = value; }
        }
        public string Modifiername
        {
            get { return modifiername; }
            set { modifiername = value; }
        }
        public Nullable<DateTime> ModifyTime
        {
            get { return modifyTime; }
            set { modifyTime = value; }
        }
        public string CODE_MACHINE
        {
            get { return cODE_MACHINE; }
            set { cODE_MACHINE = value; }
        }
        public string MACHINE
        {
            get { return mACHINE; }
            set { mACHINE = value; }
        }
        public string CODE_MACHINETYPE
        {
            get { return cODE_MACHINETYPE; }
            set { cODE_MACHINETYPE = value; }
        }
        public string MACHINETYPE
        {
            get { return mACHINETYPE; }
            set { mACHINETYPE = value; }
        }
        public string CODE_MAN
        {
            get { return cODE_MAN; }
            set { cODE_MAN = value; }
        }
        public string MAN
        {
            get { return mAN; }
            set { mAN = value; }
        }
        public string NSNO_BARGE
        {
            get { return nSNO_BARGE; }
            set { nSNO_BARGE = value; }
        }
        public string NSNO_BARGELAST
        {
            get { return nSNO_BARGELAST; }
            set { nSNO_BARGELAST = value; }
        }
        public Nullable<char> Mark_finish
        {
            get { return mark_finish; }
            set { mark_finish = value; }
        }
        public string MachineData
        {
            get { return machineData; }
            set { machineData = value; }
        }
        public string TeamWorkData
        {
            get { return teamWorkData; }
            set { teamWorkData = value; }
        }
        public string Carrier1A
        {
            get { return carrier1A; }
            set { carrier1A = value; }
        }
        public string Carrier2A
        {
            get { return carrier2A; }
            set { carrier2A = value; }
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 初始化TallyBillE
        /// </summary>
        public TallyBillE()
        {
            CODE_MACHINE = null; MACHINE = null; CODE_MACHINETYPE = null; MACHINETYPE = null; CODE_MAN = null; MAN = null;
            Obno = null;
            Tbno = null;
            Pmno = null;
            Gbno = null;
            GbnoCard = null;
            Signdate = DateTime.MinValue;
            Code_Department = null;
            Taskno = null;
            Code_Storage = null;
            Code_Booth = null;
            Vgno = null;
            Cabin = null;
            Code_Carrier = null;
            Carriernum = null;
            Code_nvessel = null;
            Bargepro = null;
            Pieceweight = null;
            Amount = 0;
            Weight = null;
            Worknum = null;
            Volume = null;
            Code_Measure = null;
            Begintime = null;
            Endtime = null;
            Code_User = null;
            Username = null;
            Code_Operation = null;
            Code_Operation_Fact = null;
            Code_SpecialMark = null;
            Code_Quality = null;
            Code_Workteam = null;
            Code_Teamtype = null;
            Code_Tallyman = null;
            Tallyman = null;
            Code_Storagelast = null;
            Code_Boothlast = null;
            Vgnolast = null;
            Cabinlast = null;
            Code_Carrierlast = null;
            Carriernumlast = null;
            Code_nvessellast = null;
            Bargeprolast = null;
            Carrier1 = null;
            Carrier1num = null;
            Carrier2 = null;
            Carrier2num = null;
            Code_Opstype = null;
            Remark = null;
            Gbdisplay = null;
            Tallynum = null;
            Gbnolast = null;
            Gbdisplaylast = null;
            Gbnofirst = null;
            Code_Worktime = null;
            Containernum = null;
            //_Code_Zone = null;
            //_Code_Station = null;
            Mark_Clearbooth = null;
            Cgno = null;
            Parenttbno = null;
            Spec = null;
            Gbnofromstock = null;
            Code_Company = null;
            Code_Section = null;
            Trainnum = null;
            Code_Loadplace = null;
            Tallydate = DateTime.MinValue;
            Code_trainroad = null;
            Code_motorcade = null;
            Code_workflow = null;
            Drivernum = null;
            Workernum = null;
            Machine = null;
            Code_billtype = null;
            Creator = null;
            Creatorname = null;
            CreateTime = null;
            ModifyTime = null;

            Mark_comp_plan = "0";
            Mark_comp_goodsbill = "0";
            Mark_comp_ship = "0";
            Mark_Audit = "0";
            AuditTime = null;
            Auditorname = null;
            Auditor = null;
            Code_Storage_Stock = null;
            Code_Booth_Stock = null;
            Workteam = null;
            Code_opstype_fact = null;
            Clear_num = 0;
            Clear_remark = null;
            Join_num = 0;
            Amount2 = 0;
            Mark1 = null;
            Mark2 = null;
            Mark3 = null;
            GBNO_PHASE = null;
            Color = null;
            Lot2 = null;
            ZYP_ID = null;
            code_workingarea = null;
            code_workingarealast = null;
            code_allocation = null;
            code_allocationlast = null;
            code_allocationstock = null;
            NSNO_BARGE = null;
            NSNO_BARGELAST = null;
            mark_finish = '0';
            MachineData = null;
            TeamWorkData = null;
            Carrier1A = null;
            Carrier2A = null;
        }
        #endregion
    }


    ///// <summary>
    ///// 作业票数据信息
    ///// </summary>
    //public struct TallyBillInfo
    //{
    //    //公司编码
    //    public string CodeCompany { get; set; }
    //    //部门编码
    //    public string CodeDepartment { get; set; }
    //    //委托编码
    //    public string Cgno { get; set; }
    //    //派工编码
    //    public string Pmno { get; set; }
    //    //用户编码
    //    public string CodeTallyman { get; set; }
    //    //用户姓名
    //    public string Tallyman { get; set; }

    //    //源航次编码
    //    public string Vgno { get; set; }
    //    //源仓别
    //    public string Cabin { get; set; }
    //    //源车别代码
    //    public string CodeCarrier { get; set; }
    //    //源车号
    //    public string CarrierNum { get; set; }
    //    //源驳船船舶规范编码
    //    public string CodeNvessel { get; set; }
    //    //源驳船属性
    //    public string Bargepro { get; set; }
    //    //源场地编码
    //    public string CodeStorage { get; set; }
    //    //源货位编码
    //    public string CodeBooth { get; set; }
    //    //源桩角编码
    //    public string CodeAllocation { get; set; }
    //    //源载体描述
    //    public string Carrier1 { get; set; }
    //    //源载体属性
    //    public string Carrier1Num { get; set; }

    //    //目的航次编码
    //    public string VgnoLast { get; set; }
    //    //目的仓别
    //    public string CabinLast { get; set; }
    //    //目的车别代码
    //    public string CodeCarrierLast { get; set; }
    //    //目的车号
    //    public string CarrierNumLast { get; set; }
    //    //目的驳船船舶规范编码
    //    public string CodeNvesselLast { get; set; }
    //    //目的驳船属性
    //    public string BargeproLast { get; set; }
    //    //目的场地编码
    //    public string CodeStorageLast { get; set; }
    //    //目的货位编码
    //    public string CodeBoothLast { get; set; }
    //    //目的桩角编码
    //    public string CodeAllocationLast { get; set; }
    //    //目的载体描述
    //    public string Carrier2 { get; set; }
    //    //目的载体属性
    //    public string Carrier2num { get; set; }


    //    //票货编码
    //    public string CodeGoodsBill { get; set; }
    //    //票货描述
    //    public string GoodsBillDisplay { get; set; }
    //    //商务票货编码
    //    public string CodeGbBusiness { get; set; }
    //    //商务票货描述
    //    public string GbBusinessDisplay { get; set; }

    //    //子过程特殊标志编码
    //    public string CodeSpecialMark { get; set; }   
    //    //源区域编码
    //    public string CodeWorkingArea { get; set; }   
    //    //目的区域编码
    //    public string CodeWorkingAreaLast { get; set; }
    //    //质量
    //    public string Quality { get; set; }
    //    //件数1
    //    public string Amount { get; set; }
    //    //重量1
    //    public string Weight { get; set; }
    //    //数量1
    //    public string Count { get; set; }
    //    //件数2
    //    public string Amount2 { get; set; }
    //    //重量2
    //    public string Weight2 { get; set; }
    //    //数量2
    //    public string Count2 { get; set; }

    //    //机械
    //    public string Machine { get; set; }
    //    //班组
    //    public string WorkTeam { get; set; } 
    //}







}