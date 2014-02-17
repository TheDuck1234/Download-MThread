using System.Collections.Generic;

namespace Download_MThread.Core.Log
{
    public class LogManager
	{
		public List<Download.Log> LogList { get; private set; }

		public LogManager()
		{
			LogList = new List<Download.Log>();
		}

		public void Log(string text)
		{
			LogList.Add(new Download.Log{Data = text});
		}

		public void FinishLog()
		{
		}
	}
}
