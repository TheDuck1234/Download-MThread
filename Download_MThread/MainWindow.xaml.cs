using System.Diagnostics;
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
        private DispatcherTimer _timer;
        private ImageFrame imageFrame;

        public MainWindow()
        {
            InitializeComponent();
            SetupListBox();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton(false);
            var testlist = XmlReader.GetXmlFiles(AppSettings.GetXmlFileName());
            var lists = DownloadLoader.Partition(testlist, 5);
            SetupTimer();
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

                _timer.Stop();

                LogMaker.MakeListLog(results, path);

                
            });
            SetupListBox();
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
        }


        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            var path = Directory.GetCurrentDirectory() + AppSettings.GetImagePath();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Process.Start(path);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var path = Directory.GetCurrentDirectory() + AppSettings.GetImagePath();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var delete = DownloadLoader.DeleteAllCache(path);

            MessageBox.Show(delete ? "Caches deleted" : "No caches to deleted");
        }

        private void Log_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void Image_Click(object sender, RoutedEventArgs e)
        {

            var path = Directory.GetCurrentDirectory() + AppSettings.GetImagePath();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Process.Start(path);
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SetupTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Tick += timer_Tick;
            _timer.Interval = new TimeSpan(0, 0, 3);
            _timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            SetupListBox();
        }

        private void SetupListBox()
        {
            var path = Directory.GetCurrentDirectory() + AppSettings.GetImagePath();
            var imageFiles = ImageHandler.LoadImageFiles(path);

            if (imageFiles == null) return;

            var list = imageFiles.Select(imageFile => imageFile.Name).ToList();
            ListBox.ItemsSource = list;
        }

        private void ListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (imageFrame == null)
            {
                imageFrame = new ImageFrame(ListBox.SelectedItem.ToString());
                imageFrame.Show();
            }
            else
            {
                imageFrame.LoadImage(ListBox.SelectedItem.ToString());
                imageFrame.Show();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (imageFrame == null) return;
            imageFrame.CloseImage();
        }
    }
}
