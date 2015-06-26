using System;
using System.Collections.Generic;
using System.IO;

namespace Download_MThread.Core.Download
{
    public static class DownloadLoader
    {
        public static List<T>[] Partition<T>(List<T> list, int totalPartitions)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            if (totalPartitions < 1)
                throw new ArgumentOutOfRangeException("totalPartitions");

            var partitions = new List<T>[totalPartitions];

            var maxSize = (int)Math.Ceiling(list.Count / (double)totalPartitions);
            var k = 0;

            for (var i = 0; i < partitions.Length; i++)
            {
                partitions[i] = new List<T>();
                for (var j = k; j < k + maxSize; j++)
                {
                    if (j >= list.Count)
                        break;
                    partitions[i].Add(list[j]);
                }
                k += maxSize;
            }

            return partitions;
        }
        public static bool IsCache(string fileName)
        {
            return File.Exists(fileName);
        }

        public static bool DeleteAllCache(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    return false;
                }

                Directory.Delete(path, true);
                
                return true;
            }
            catch (Exception)
            {
                //throw new Exception("connection error :" + path);
            }
            return false;
        }
    }
    public class DownloadWorker
    {
        public event EventHandler<EventArgs> Progressed;

        protected virtual void OnProgressed()
        {
            var handler = Progressed;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public List<Log.Log> DownloadeImage(List<ImageFile> fileList, string path)
        {
            var errorList = new List<Log.Log>();

            foreach (var file in fileList)
            {
                var fileName = path + @"\"+ file.Name +".jpg";
                if (!DownloadLoader.IsCache(fileName))
                {
                    ImageHandler.DownloadImageFile(file.Url, fileName);
                    if (!DownloadLoader.IsCache(fileName))
                    {
                        errorList.Add(new Log.Log{Data = file.Name+ " didn't download fully !Error!"});
                    }
                }
                OnProgressed();
            }

            return errorList;
        }
    }
}
