using System;

namespace Download_MThread.Core.Download
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
}