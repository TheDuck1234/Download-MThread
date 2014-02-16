using System;
using System.IO;
using System.Net;
using System.Windows;

namespace Download_MThread.Core
{
   public class WizardsImageHandler
    {
        public static void DownloadRemoteImageFile(string uri, string fileName)
        {
            try

            {
                var request = (HttpWebRequest)WebRequest.Create(uri);
                var response = (HttpWebResponse)request.GetResponse();
                if ((response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Moved &&
                     response.StatusCode != HttpStatusCode.Redirect) ||
                    !response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase)) return;
                using (var inputStream = response.GetResponseStream())
                using (var outputStream = File.OpenWrite(fileName))
                {
                    var buffer = new byte[4096];
                    var bytesRead = 0;
                    do
                    {
                        if (inputStream != null) bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                        outputStream.Write(buffer, 0, bytesRead);
                    } while (bytesRead != 0);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("No Internetconnection");
            }
        }
    }
}
