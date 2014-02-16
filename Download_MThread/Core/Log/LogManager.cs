using System;
using System.Collections.Generic;

namespace Download_MThread.Core.Log
{
	public class Log
	{
		public string Data { get; set; }
		public string Time = DateTime.Now.ToLongTimeString();

		public string GiveLog()
		{
			return Time + " | " + Data;
		}
	}
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
		    var path = "2.txt"; //AppSettings.GetLogPath()+DateTime.Now.ToShortDateString() +".txt";
			LogMaker.MakeListLog(LogList,path);
            LogList.Clear();
		}
	}
}
