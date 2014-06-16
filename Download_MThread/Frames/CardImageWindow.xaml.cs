using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Download_MThread.Core;
using Download_MThread.Core.Download;

namespace Download_MThread.Frames
{
    /// <summary>
    /// Interaction logic for CardImageWindow.xaml
    /// </summary>
    public partial class CardImageWindow
    {
        private List<ImageFile> _imageFiles;

        public CardImageWindow()
        {

            InitializeComponent();
            SetupListBox();
            SetupTimer();
        }

        private void SetupTimer()
        {
            var timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 3);
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            SetupListBox();
        }

        private void SetupListBox()
        {
            var path = Directory.GetCurrentDirectory() + AppSettings.GetImagePath();
            _imageFiles = WizardsImageHandler.LoadImageFiles(path);
            var list = _imageFiles.Select(imageFile => imageFile.Name).ToList();
            ListBox.ItemsSource = list;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void STextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            SetupListBox();
        }
    }
}
