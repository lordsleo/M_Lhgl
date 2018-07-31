using Leo.Oracle;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml;

namespace ServiceInterface.Common 
{
    public class DataAccessC : DataAccess
    {
        /// <summary>
        /// 数据库连接串
        /// </summary>
        private static string _strConnOle = null;
        /// <summary>
        /// 数据库连接获取数据方法
        /// </summary>
        /// <param name="keyPath">注册表路径</param>
        public DataAccessC(string keyPath)
            : base(keyPath)
        {
            //注册表内容
            string strRegistryData = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(keyPath).GetValue("DataBase").ToString();
            //xml格式数据读取
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(strRegistryData);
            //数据库连接串初始化
            _strConnOle = string.Format("Provider=OraOLEDB.Oracle.1;PLSQLRSet=True;User ID={0};Data Source={1};Extended Properties=;Persist Security Info=True;Password={2};", xmlDoc.FirstChild.Attributes["login"].Value, xmlDoc.FirstChild.Attributes["service"].Value, xmlDoc.FirstChild.Attributes["password"].Value);
        }
        /// <summary>
        /// （以数据表形式）获取数据
        /// </summary>
        /// <param name="strSql">获取数据的存储过程</param>
        /// <returns>返回的数据集</returns>
        public DataSet GetDataSetByStoredProcedure(string strSql)
        {
            DataSet ds = new DataSet();
            using (System.Data.OleDb.OleDbConnection dbc = new System.Data.OleDb.OleDbConnection(_strConnOle))
            {
                if (dbc.State != ConnectionState.Open)
                {
                    dbc.Open();
                }
                System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand(strSql, dbc);
                System.Data.OleDb.OleDbDataAdapter ad = new System.Data.OleDb.OleDbDataAdapter(cmd);
                ds.Reset();
                try
                {
                    ad.Fill(ds);
                    dbc.Close();
                    return ds;
                }
                catch (Exception ex)
                {
                    dbc.Close();
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}