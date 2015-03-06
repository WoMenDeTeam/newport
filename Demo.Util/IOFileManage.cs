using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Demo.Util
{
    public class IOFileManage
    {
        public static string[] GetFolderList(string path)
        {
            return Directory.GetDirectories(path);
        }

        public static string[] GetFileList(string path)
        {
            return Directory.GetFiles(path);
        }

        public static bool BackUpFile(string filepath, string aimpath)
        {
            try
            {
                if (!Directory.Exists(aimpath))
                {
                    CreateFolder(aimpath);
                }
                string aimfilepath = aimpath + GetFileName(filepath);
                File.Copy(filepath, aimpath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void RemoveFile(string filepath, string aimpath)
        {
            if (BackUpFile(filepath, aimpath))
            {
                File.Delete(filepath);
            }
        }

        public static string GetFileName(string filepath)
        {
            int lastindex = filepath.LastIndexOf('\\');
            return filepath.Substring(lastindex + 1);
        }

        public static bool CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void DeleteFile(string path)
        {
            File.Delete(path);
        }

        public static void DeleteFolder(string path)
        {
            string[] ChildFolder = GetFolderList(path);
            if (ChildFolder.Length > 0)
            {
                foreach (string folder in ChildFolder)
                {
                    DeleteFolder(folder);
                }
            }
            string[] ChildFile = GetFileList(path);
            if (ChildFile.Length > 0)
            {
                foreach (string file in ChildFile)
                {
                    File.Delete(file);
                }
            }
            Directory.Delete(path);
        }

        private static string GetFolderPath(string filepath)
        {
            int lastindex = filepath.LastIndexOf('\\');
            return filepath.Substring(0, lastindex);
        }
        /// <summary>
        /// httppostfile控件上传
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <param name="fileName"></param>
        /// <param name="savePath"></param>
        public static void UploadFile(System.Web.HttpPostedFile uploadFile, string filepath)
        {
            try
            {
                string path = GetFolderPath(filepath);
                if (Directory.Exists(path) == false)
                {
                    Directory.CreateDirectory(path);

                }
                if (uploadFile.ContentLength > 0)
                {

                    if (File.Exists(filepath) == false)///检查是否存在同名文件
                    {
                        uploadFile.SaveAs(filepath);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Generic Exception Handler: {0}", e);
            }
        }
    }
}
