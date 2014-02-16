using Download_MThread.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Download_MThread
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _count = 0;
        private DateTime _starttime;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var testlist = XmlLoaderTest.GetUrl();
            var lists = DownloadLoader.Partition(testlist, 5);
            // Create and collect tasks in list
            _starttime = DateTime.Now;
            var tasks = lists.Select(list => Task.Factory.StartNew(() =>
            {
                var _worker = new DownloadWorker();
                _worker.Progressed += (o, args) =>
                {
                    lock (this)
                    {
                        _count++;
                    }
                    Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        // ReSharper disable once RedundantCast
                        var procent = ((100 * _count) / testlist.Count);
                        ProgressBar.Value = procent;
                        ProgressLabel.Content = "Progress: " + procent + "%";
                        //CountLabel.Content = "Total items cleanse: " + _count;
                        if (_count > 0)
                        {
                            EstimateTimeLabel.Content = "Estimate Time: " + EstimateTime(testlist.Count).ToString();
                        }
                    });
                };

                var result = _worker.DownloadeImage(list.ToList(),@"C:\HS Card Cache");
                return result;


            })).ToList();
            // ReSharper disable once ImplicitlyCapturedClosure
            Task.Factory.StartNew(() =>
            {
                var results = new List<bool>();
                // Wait till all tasks completed

                foreach (var result in tasks.Select(task => task.Result))
                {
                    results.AddRange(result.ToList());
                }

            });
        }
        private TimeSpan EstimateTime(int max)
        {
            var timespent = DateTime.Now - _starttime;
            var secondsremaining = (int)(timespent.TotalSeconds / _count * (max - _count));
            var timespan = TimeSpan.FromSeconds(secondsremaining);
            return timespan;
        }
    }
}
