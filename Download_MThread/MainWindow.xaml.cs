using System.IO;
using Download_MThread.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Download_MThread.Core.Download;
using Download_MThread.Core.Log;
using Download_MThread.Frames;

namespace Download_MThread
{
    public partial class MainWindow
    {
        private int _count;
        private DateTime _starttime;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton(false);
            var testlist = XmlReader.GetXmlFiles(AppSettings.GetXmlFileName());
            var lists = DownloadLoader.Partition(testlist, 5);

            // Create and collect tasks in list

            _starttime = DateTime.Now;

            var tasks = lists.Select(list => Task.Factory.StartNew(() =>
            {
                var worker = new DownloadWorker();
                worker.Progressed += (o, args) =>
                {
                    lock (this)
                    {
                        _count++;
                    }
                    Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {    

                        var procent = ((100 * _count) / testlist.Count);
                        ProgressBar.Value = procent;
                        ProgressLabel.Content = procent + "%";

                        //CountLabel.Content = "Total items cleanse: " + _count;

                        if (_count > 0)
                        {
                            EstimateTimeLabel.Content = "Time: " + EstimateTime(testlist.Count).ToString();
                        }
                    });
                };

                var path = Directory.GetCurrentDirectory() + AppSettings.GetImagePath();

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var result = worker.DownloadeImage(list.ToList(), path);
                return result;


            })).ToList();
            
            Task.Factory.StartNew(() =>
            {
                var results = new List<Log>();

                // Wait till all tasks completed

                foreach (var result in tasks.Select(task => task.Result))
                {
                    results.AddRange(result.ToList());
                }
                var path = Directory.GetCurrentDirectory() + AppSettings.GetLogPath();

                LogMaker.MakeListLog(results, path);

                
            });
            ToggleButton(true);
        }
        private TimeSpan EstimateTime(int max)
        {
            var timespent = DateTime.Now - _starttime;
            var secondsremaining = (int)(timespent.TotalSeconds / _count * (max - _count));
            var timespan = TimeSpan.FromSeconds(secondsremaining);
            return timespan;
        }

        private void ToggleButton(bool status)
        {
            TestButton.IsEnabled = status;
            DcButton.IsEnabled = status;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var path = Directory.GetCurrentDirectory() + AppSettings.GetImagePath();

            var delete = DownloadLoader.DeleteAllCache(path);

            MessageBox.Show(delete ? "Caches deleted" : "No caches to deleted");
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            var imageWindow = new CardImageWindow(this);
            this.Hide();
            imageWindow.Show();
        }
    }
}
