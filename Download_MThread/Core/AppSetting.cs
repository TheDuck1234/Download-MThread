using System.Configuration;

namespace Download_MThread.Core
{
    public static class AppSettings
    {
        public static string GetXmlFileName()
        {
            
            // ReSharper disable once CSharpWarnings::CS0618
            return ConfigurationSettings.AppSettings["XmlFileName"];
        }
    }
}
