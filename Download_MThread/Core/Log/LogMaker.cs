using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Download_MThread.Core.Log
{
	public static class LogMaker
	{
		public static void MakeListLog(List<Download.Log> linesList, string path )
		{
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
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
