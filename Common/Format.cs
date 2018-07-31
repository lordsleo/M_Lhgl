using System;
using System.Text.RegularExpressions;

namespace ServiceInterface.Common
{
    /// <summary>
    /// 格式化工具。
    /// </summary>
    internal class Format
    {
        #region
        #region 除去文本前后空格
        /// <summary>
        /// 除去文本前后空格。
        /// </summary>/// <param name="text">文本。</param>/// <returns>除去前后空格的文本。</returns>
        public static string Trim(string text)
        {
            if (text == null || text.Trim().Length == 0)
                return null;
            else
                return text.Trim();
        }
        #endregion

        #region 可为空值的数据转化为字符串
        /// <summary>
        /// 可为空值的数据转化为字符串。
        /// </summary>/// <param name="value">布尔或空值。</param>/// <returns>字符串。</returns>
        public static string NullableToString(Nullable<bool> value)
        {
            if (value.HasValue == false)
                return null;
            else
                return value.Value.ToString();
        }
        /// <summary>
        /// 可为空值的数据转化为字符串。
        /// </summary>/// <param name="value">时间或空值。</param>/// <returns>字符串。</returns>
        public static string NullableToString(Nullable<DateTime> value)
        {
            if (value.HasValue == false)
                return null;
            else
                return value.Value.ToString();
        }
        /// <summary>
        /// 可为空值的数据转化为字符串。
        /// </summary>/// <param name="value">时间或空值。</param>/// <param name="format">格式化字串。</param>/// <returns>字符串。</returns>
        public static string NullableToString(Nullable<DateTime> value, string format)
        {
            if (value.HasValue == false)
                return null;
            else
                return value.Value.ToString(format);
        }
        /// <summary>
        /// 可为空值的数据转化为字符串。
        /// </summary>/// <param name="value">定点数或空值。</param>/// <returns>字符串。</returns>
        public static string NullableToString(Nullable<decimal> value)
        {
            if (value.HasValue == false)
                return null;
            else
                return value.Value.ToString();
        }
        /// <summary>
        /// 可为空值的数据转化为字符串。
        /// </summary>/// <param name="value">定点数或空值。</param>/// <param name="format">格式化字串。</param>/// <returns>字符串。</returns>
        public static string NullableToString(Nullable<decimal> value, string format)
        {
            if (value.HasValue == false)
                return null;
            else
                return value.Value.ToString(format);
        }
        /// <summary>
        /// 可为空值的数据转化为字符串。
        /// </summary>/// <param name="value">短整数或空值。</param>/// <returns>字符串。</returns>
        public static string NullableToString(Nullable<short> value)
        {
            if (value.HasValue == false)
                return null;
            else
                return value.Value.ToString();
        }
        /// <summary>
        /// 可为空值的数据转化为字符串。
        /// </summary>/// <param name="value">短整数或空值。</param>/// <param name="format">格式化字串。</param>/// <returns>字符串。</returns>
        public static string NullableToString(Nullable<short> value, string format)
        {
            if (value.HasValue == false)
                return null;
            else
                return value.Value.ToString(format);
        }
        /// <summary>
        /// 可为空值的数据转化为字符串。
        /// </summary>/// <param name="value">整数或空值。</param>/// <returns>字符串。</returns>
        public static string NullableToString(Nullable<int> value)
        {
            if (value.HasValue == false)
                return null;
            else
                return value.Value.ToString();
        }
        /// <summary>
        /// 可为空值的数据转化为字符串。
        /// </summary>/// <param name="value">整数或空值。</param>/// <param name="format">格式化字串。</param>/// <returns>字符串。</returns>
        public static string NullableToString(Nullable<int> value, string format)
        {
            if (value.HasValue == false)
                return null;
            else
                return value.Value.ToString(format);
        }
        /// <summary>
        /// 可为空值的数据转化为记录值。
        /// </summary>/// <param name="value">布尔或空值。</param>/// <returns>记录值。</returns>
        public static object NullableToRecord(Nullable<bool> value)
        {
            if (value.HasValue == false)
                return DBNull.Value;
            else
                return value.Value;
        }
        /// <summary>
        /// 可为空值的数据转化为记录值。
        /// </summary>/// <param name="value">时间或空值。</param>/// <returns>记录值。</returns>
        public static object NullableToRecord(Nullable<DateTime> value)
        {
            if (value.HasValue == false)
                return DBNull.Value;
            else
                return value.Value;
        }
        /// <summary>
        /// 可为空值的数据转化为记录值。
        /// </summary>/// <param name="value">定点数或空值。</param>/// <returns>记录值。</returns>
        public static object NullableToRecord(Nullable<decimal> value)
        {
            if (value.HasValue == false)
                return DBNull.Value;
            else
                return value.Value;
        }
        /// <summary>
        /// 可为空值的数据转化为记录值。
        /// </summary>/// <param name="value">短整数或空值。</param>/// <returns>记录值。</returns>
        public static object NullableToRecord(Nullable<short> value)
        {
            if (value.HasValue == false)
                return DBNull.Value;
            else
                return value.Value;
        }
        /// <summary>
        /// 可为空值的数据转化为记录值。
        /// </summary>/// <param name="value">整数或空值。</param>/// <returns>记录值。</returns>
        public static object NullableToRecord(Nullable<int> value)
        {
            if (value.HasValue == false)
                return DBNull.Value;
            else
                return value.Value;
        }
        #endregion

        #region 分析可为空值的字符串
        /// <summary>
        /// 分析可为空值的字符串。
        /// </summary>/// <param name="text">字符串。</param>/// <returns>布尔或空值。</returns>
        public static Nullable<bool> ParseBoolean(string text)
        {
            string value = Format.Trim(text);
            if (value == null)
                return null;
            else
                return bool.Parse(text);
        }
        /// <summary>
        /// 分析可为空值的字符串。
        /// </summary>/// <param name="text">字符串。</param>/// <returns>整数或空值。</returns>
        public static Nullable<DateTime> ParseDateTime(string text)
        {
            string value = Format.Trim(text);
            if (value == null)
                return null;
            else
                return DateTime.Parse(text);
        }
        /// <summary>
        /// 分析可为空值的字符串。
        /// </summary>/// <param name="text">字符串。</param>/// <returns>定点数或空值。</returns>
        public static Nullable<decimal> ParseDecimal(string text)
        {
            string value = Format.Trim(text);
            if (value == null)
                return null;
            else
                return decimal.Parse(text);
        }
        /// <summary>
        /// 分析可为空值的字符串。
        /// </summary>/// <param name="text">字符串。</param>/// <returns>短整数或空值。</returns>
        public static Nullable<short> ParseInt16(string text)
        {
            string value = Format.Trim(text);
            if (value == null)
                return null;
            else
                return short.Parse(text);
        }
        /// <summary>
        /// 分析可为空值的字符串。
        /// </summary>/// <param name="text">字符串。</param>/// <returns>整数或空值。</returns>
        public static Nullable<int> ParseInt32(string text)
        {
            string value = Format.Trim(text);
            if (value == null)
                return null;
            else
                return int.Parse(text);
        }

        #endregion

        #region 转化为可为空值的数据
        /// <summary>
        /// 转化为可为空值的数据。
        /// </summary>/// <param name="value">数据。</param>/// <returns>布尔或空值。</returns>
        public static string ConvertToString(object value)
        {
            if (value == null || value is DBNull || value.ToString().Trim() == string.Empty)
                return null;
            else
                return Convert.ToString(value);
        }
        /// <summary>
        /// 转化为可为空值的数据。
        /// </summary>/// <param name="value">数据。</param>/// <returns>布尔或空值。</returns>
        public static Nullable<bool> ConvertToBoolean(object value)
        {
            if (value == null || value is DBNull || value.ToString().Trim() == string.Empty)
                return null;
            else
                return Convert.ToBoolean(value);
        }
        /// <summary>
        /// 转化为可为空值的数据。
        /// </summary>/// <param name="value">数据。</param>/// <returns>时间或空值。</returns>
        public static Nullable<DateTime> ConvertToDateTime(object value)
        {
            if (value == null || value is DBNull || value.ToString().Trim() == string.Empty)
                return null;
            else
                return Convert.ToDateTime(value);
        }
        /// <summary>
        /// 转化为可为空值的数据。
        /// </summary>/// <param name="value">数据。</param>/// <returns>定点数或空值。</returns>
        public static Nullable<decimal> ConvertToDecimal(object value)
        {
            if (value == null || value is DBNull || value.ToString().Trim() == string.Empty)
                return null;
            else
                return Convert.ToDecimal(value);
        }
        /// <summary>
        /// 转化为可为空值的数据。
        /// </summary>/// <param name="value">数据。</param>/// <returns>短整数或空值。</returns>
        public static Nullable<short> ConvertToInt16(object value)
        {
            if (value == null || value is DBNull || value.ToString().Trim() == string.Empty)
                return null;
            else
                return Convert.ToInt16(value);
        }
        /// <summary>
        /// 转化为可为空值的数据。
        /// </summary>/// <param name="value">数据。</param>/// <returns>整数或空值。</returns>
        public static Nullable<int> ConvertToInt32(object value)
        {
            if (value == null || value is DBNull || value.ToString().Trim() == string.Empty)
                return null;
            else
                return Convert.ToInt32(value);
        }
        #endregion

        #region 判断数据是否改变
        /// <summary>
        /// 判断数据是否改变。
        /// </summary>/// <param name="text">文本。</param>/// <param name="value">数据。</param>/// <returns>是否改变。</returns>
        public static bool HasChange(string text, Nullable<bool> value)
        {
            try
            {
                if (text == null || value.HasValue == false)
                    return (text != null || value.HasValue != false);
                else
                    return bool.Parse(text) != value;
            }
            catch
            {
                return true;
            }
        }
        /// <summary>
        /// 判断数据是否改变。
        /// </summary>/// <param name="text">文本。</param>/// <param name="value">数据。</param>/// <returns>是否改变。</returns>
        public static bool HasChange(string text, Nullable<DateTime> value)
        {
            try
            {
                if (text == null || value.HasValue == false)
                    return (text != null || value.HasValue != false);
                else
                    return DateTime.Parse(text) != value;
            }
            catch
            {
                return true;
            }
        }
        /// <summary>
        /// 判断数据是否改变。
        /// </summary>/// <param name="text">文本。</param>/// <param name="value">数据。</param>/// <returns>是否改变。</returns>
        public static bool HasChange(string text, Nullable<decimal> value)
        {
            try
            {
                if (text == null || value.HasValue == false)
                    return (text != null || value.HasValue != false);
                else
                    return decimal.Parse(text) != value;
            }
            catch
            {
                return true;
            }
        }
        /// <summary>
        /// 判断数据是否改变。
        /// </summary>/// <param name="text">文本。</param>/// <param name="value">数据。</param>/// <returns>是否改变。</returns>
        public static bool HasChange(string text, Nullable<short> value)
        {
            try
            {
                if (text == null || value.HasValue == false)
                    return (text != null || value.HasValue != false);
                else
                    return short.Parse(text) != value;
            }
            catch
            {
                return true;
            }
        }
        /// <summary>
        /// 判断数据是否改变。
        /// </summary>/// <param name="text">文本。</param>/// <param name="value">数据。</param>/// <returns>是否改变。</returns>
        public static bool HasChange(string text, Nullable<int> value)
        {
            try
            {
                if (text == null || value.HasValue == false)
                    return (text != null || value.HasValue != false);
                else
                    return int.Parse(text) != value;
            }
            catch
            {
                return true;
            }
        }
        #endregion

        #region 返回值不可为空的转换
        /// <summary>
        /// 转化为可为0的数据。
        /// </summary>/// <param name="value">数据。</param>/// <returns>结果整数</returns>
        public static int ConToInt16(object value)
        {
            if (value == null || value is DBNull || value.ToString().Trim() == string.Empty)
                return 0;
            else
                return Convert.ToInt16(value);
        }

        /// <summary>
        /// 转化为可为0的数据。
        /// </summary>/// <param name="value">数据。</param>/// <returns>结果整数</returns>
        public static int ConToInt32(object value)
        {
            if (value == null || value is DBNull || value.ToString().Trim() == string.Empty)
                return 0;
            else
                return Convert.ToInt32(value);
        }

        /// <summary>
        /// 转化为可为0的小数
        /// </summary>/// <param name="value">数据。</param>/// <returns>结果小数</returns>
        public static decimal ConToDecimal(object value)
        {
            if (value == null || value is DBNull || value.ToString().Trim() == string.Empty)
                return 0;
            else
                return Convert.ToDecimal(value);
        }

        /// <summary>
        /// 转化为可为""的字符串
        /// </summary>/// <param name="value">数据</param>/// <returns>结果字符串</returns>
        public static string ConToString(object value)
        {
            if (value == null || value is DBNull || value.ToString().Trim() == string.Empty)
                return "";
            else
                return Convert.ToString(value);
        }
        #endregion


 
        #endregion
    }
}
