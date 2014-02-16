using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Download_MThread.Core.Log
{
	public static class LogMaker
	{
		public static void MakeListLog(List<Log> linesList, string path )
		{
			var text = linesList.Select(log => log.GiveLog()).ToList();
			File.WriteAllLines(path, text);
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
