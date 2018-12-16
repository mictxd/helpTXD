using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TXD.CF
{
    public class Sche_Help
    {
        public static string pt_datetime2str(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd hh:mm:ss");
        }

        public static DateTime pt_str2datetime(string timeStr)
        {
            return new DateTime(Convert.ToInt32(timeStr.Substring(0, 4))
                , Convert.ToInt32(timeStr.Substring(5, 2))
                , Convert.ToInt32(timeStr.Substring(8, 2))
                , Convert.ToInt32(timeStr.Substring(11, 2))
                , Convert.ToInt32(timeStr.Substring(14, 2))
                , Convert.ToInt32(timeStr.Substring(17, 2))
                , 0);
        }

        /// <summary>
        /// 尝试读取文件，解析首行文本为时间。
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns>返回解析时间，如果没文件则会返回最小时间常数。</returns>
        public static DateTime pt_GetTimeFromFile(string keyName)
        {
            Assembly asembly = Assembly.GetExecutingAssembly();

            DateTime lvdt = DateTime.MinValue;
            string lv_DateFileName = asembly.Location + keyName;
            if (!File.Exists(lv_DateFileName))
            {
                return lvdt;
            }
            using (StreamReader sr = new StreamReader(lv_DateFileName))
            {
                string line = sr.ReadLine();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    lvdt = pt_str2datetime(line);
                }
                sr.Close();
                sr.Dispose();
            }
            return lvdt;
        }

        public static void pt_SetTimetoFile(string keyName, DateTime dtLast)
        {
            Assembly asembly = Assembly.GetExecutingAssembly();

            string lv_DateFileName = asembly.Location + keyName;
            using (StreamWriter sw = new StreamWriter(lv_DateFileName))
            {
                sw.WriteLine(pt_datetime2str(dtLast));
                sw.Flush();
                sw.Close();
            }
        }
    }
}