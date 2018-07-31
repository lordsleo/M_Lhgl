//
//文件名：    FileTool.aspx.cs
//功能描述：  文件（文本、图片、语音等）工具类
//创建时间：  2015/07/09
//作者：      
//修改时间：  暂无
//修改描述：  暂无
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Configuration;

namespace ServiceInterface.Common
{
    public class FileTool
    {
        #region
        ///<summary>
        ///上传文件（图片、语音等）
        ///</summary>
        ///<param name="filePath">文件上传目的地址（文件存放的上级文件夹）</param>
        ///<param name="fileName">上传文件名称</param>
        ///<param name="fileCode">文件编码后的字符串（base64编码）</param>
        public static bool UploadFile(string filePath, string fileName, string fileCode)
        {
            bool success = false;
            FileStream FsWrite = null;
            try
            {
                byte[] bytes = Convert.FromBase64String(fileCode);   //对android传过来的图片字符串进行解码
                DirectoryInfo fileDir = new DirectoryInfo(filePath);         //判断当前存储路径是否存在，不存在创建
                if (!fileDir.Exists)
                {
                    fileDir.Create();
                }
                FsWrite = new FileStream(fileDir.ToString() + fileName, FileMode.Create, FileAccess.Write);
                FsWrite.Write(bytes, 0, bytes.Length);
                FsWrite.Flush();
                success = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (FsWrite != null)
                {
                    FsWrite.Close();
                }
            }
            return success;
        }
        #endregion

        #region
        ///<summary>
        ///上传文件（图片、语音等）
        ///</summary>
        ///<param name="filePath">文件上传目的地址（文件存放的上级文件夹）</param>
        ///<param name="fileName">上传文件名称</param>
        ///<param name="fileCode">文件字节数组</param>
        public static bool UploadFile(string filePath, string fileName, byte[] bytes)
        {
            bool success = false;
            FileStream FsWrite = null;
            try
            {
                DirectoryInfo fileDir = new DirectoryInfo(filePath);         //判断当前存储路径是否存在，不存在创建
                if (!fileDir.Exists)
                {
                    fileDir.Create();
                }
                FsWrite = new FileStream(fileDir.ToString() + fileName, FileMode.Create, FileAccess.Write);
                FsWrite.Write(bytes, 0, bytes.Length);
                FsWrite.Flush();
                success = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (FsWrite != null)
                {
                    FsWrite.Close();
                }
            }
            return success;
        }
        #endregion

        #region
        ///<summary>
        ///上传文件（图片、语音等）
        ///</summary>
        ///<param name="filePath">文件上传目的地址（文件存放的上级文件夹）</param>
        ///<param name="fileName">上传文件名称</param>
        ///<param name="fileCode">文件流</param>
        public static bool UploadFile(string filePath, string fileName, Stream stream)
        {
            bool success = false;
            FileStream FsWrite = null;
            try
            {
                DirectoryInfo fileDir = new DirectoryInfo(filePath);         //判断当前存储路径是否存在，不存在创建
                if (!fileDir.Exists)
                {
                    fileDir.Create();
                }
                FsWrite = new FileStream(fileDir.ToString() + fileName, FileMode.Create, FileAccess.Write);
                //每次读取的65535个字节
                byte[] bytes = new byte[65535];
                int numReadByte = 0;
                while ((numReadByte = stream.Read(bytes, 0, 65535)) != 0)
                {
                    FsWrite.Write(bytes, 0, numReadByte);
                }
                FsWrite.Flush();
                success = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (FsWrite != null)
                {
                    FsWrite.Close();
                }
            }
            return success;
        }
        #endregion
       
        /// <summary>
        /// 获取Web.config应用设置。
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>设置值</returns>
        public static string GetWebConfigKey(string key)
        {
            return WebConfigurationManager.AppSettings[key];
        }
    }
}