using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ClassGenerator.Tools
{
    public class FileManager
    {
        /// <summary>
        /// Return Application path
        /// </summary>
        public static string GetAppPath
        {
            get
            {
                return Directory.GetCurrentDirectory();
            }
        }

        /// <summary>
        /// Check is Directory exist, else create
        /// </summary>
        /// <param name="folderName">full path</param>
        public static void CheckExistOrCreate(string folderName)
        {
            string tmpUrl = Path.Combine(GetAppPath, folderName);
            if (!Directory.Exists(tmpUrl))
                Directory.CreateDirectory(tmpUrl);
        }

        /// <summary>
        /// Create File from string
        /// </summary>
        /// <param name="fileName">Full Path</param>
        /// <param name="value">String</param>
        public static void WriteFile(string fileName, string value, bool overrideFile = true)
        {
            if (overrideFile)
            {
                if (File.Exists(fileName))
                    File.Delete(fileName);
            }


            TextWriter txtWriter = new StreamWriter(fileName);
            txtWriter.Write(value);
            txtWriter.Flush();
            txtWriter.Close();
            //txtWriter.Dispose();
            txtWriter = null;
        }

        /// <summary>
        /// Create file from Binary
        /// </summary>
        /// <param name="fileName">Full Path</param>
        /// <param name="binary">Binary File</param>
        public static void WriteBinaryFile(string fileName, byte[] binary)
        {
            if (binary == null) return;
            if (File.Exists(fileName)) File.Delete(fileName);

            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(binary);
                }
            }
        }

        /// <summary>
        /// Read file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadFile(string filePath)
        {
            string tmp = string.Empty;
            using (StreamReader sr = new StreamReader(filePath))
            {
               tmp = sr.ReadToEnd();
            }
            return tmp;
        }
    }
}
