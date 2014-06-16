using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Download_MThread.Core.Download
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
                //throw new Exception("connection error :" + fileName );
            }
        }

       public static List<ImageFile> LoadImageFiles(string path)
       {
           try
           {
               var fileArray = Directory.GetFiles(path, "*.jpg", SearchOption.AllDirectories);
               var imageFiles = new List<ImageFile>();

               foreach (var s in fileArray)
               {
                   var file = new ImageFile();
                   var fullname = Path.GetFileName(s);
                   if (fullname != null)
                   {
                       var sp = fullname.Split('.');
                       file.Name = sp[0];
                   }
                   file.Url = s;
                   imageFiles.Add(file);
               }

               return imageFiles;
           }
           catch (DirectoryNotFoundException)
           {
               return null;
           }
       } 
    }
}
