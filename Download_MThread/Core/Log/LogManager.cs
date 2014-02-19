using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Download_MThread.Core.Log
{
    public class LogManager
	{
		public List<Log> LogList { get; private set; }

		public LogManager()
		{
			LogList = new List<Log>();
		}

		public void Log(string text)
		{
			LogList.Add(new Log{Data = text});
		}

		public void FinishLog()
		{
            //make a log
		}
	}

    public class Log
    {
        public string Data { get; set; }
        public string Time = DateTime.Now.ToLongTimeString();

        public string GiveLog()
        {
            return Time + " | " + Data;
        }
    }
    public static class LogMaker
    {
        public static void MakeListLog(List<Log> linesList, string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var text = linesList.Select(log => log.GiveLog()).ToList();
            File.WriteAllLines(path + @"\logs.txt", text);
        }

        public static string[] ReadLogs(string folderPath)
        {
            var pdfFiles = Directory.GetFiles(folderPath, "*.txt")
                                     .Select(Path.GetFileName)
                                     .ToArray();

            return pdfFiles;
        }
    }
}
