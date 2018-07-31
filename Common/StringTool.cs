//
//文件名：    StringTool.aspx.cs
//功能描述：  字符串工具类
//创建时间：  2015/07/09
//作者：      
//修改时间：  暂无
//修改描述：  暂无
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceInterface.Common
{
    public class StringTool
    {
        /// <summary>
        /// 将数组连接成字符串
        /// </summary>
        /// <param name="strNameArray"></param>
        /// <returns></returns>
        public static string ArrayToString(string[] strNameArray)
        {
            string strLink = string.Empty;
            if (strNameArray.Length > 0)
            {
                for (int iName = 0; iName < strNameArray.Length - 1; iName++)
                {
                    strLink += strNameArray[iName];
                    strLink += "+";
                }
                strLink += strNameArray[strNameArray.Length - 1];
            }

            return strLink;
        }

        /// <summary>
        /// 时间格式化（衡重查询使用）
        /// </summary>
        /// <param name="str">时间字符串（0600）</param>
        /// <returns>格式化数据（06:00）</returns>
        public static string ToDayNightForSql(string str)
        {
            return string.Format(" {0}:{1}", str.Substring(0, 2), str.Substring(2, 2));
        }
    }
}