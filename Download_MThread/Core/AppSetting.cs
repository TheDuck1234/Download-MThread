using System.Configuration;

namespace Download_MThread.Core
{
    public static class AppSettings
    {
        public static string GetXmlFileName()
        {
            
            return ConfigurationSettings.AppSettings["XmlFileName"];
        }
        public static string GetImagePath()
        {

            return ConfigurationSettings.AppSettings["ImagePath"];
        }
        public static string GetLogPath()
        {

            return ConfigurationSettings.AppSettings["LogPath"];
        }
    }
}
