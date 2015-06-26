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

        private readonly List<string> _comboList = new List<string> { "All Cards" };

        public CardImageWindow(Window window)
        {
            this.Owner = window;
            InitializeComponent();

            //start setup :
            SetupListBox();
            //SetupTimer();
            SetupCombobox();
        }

        private void SetupCombobox()
        {
            TypeComboBox.ItemsSource = _comboList;
            TypeComboBox.SelectedItem = TypeComboBox.Items[0];
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
            _imageFiles = ImageHandler.LoadImageFiles(path);

            if (_imageFiles == null) return;

            var list = _imageFiles.Select(imageFile => imageFile.Name).ToList();
            ListBox.ItemsSource = list;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Owner.Show();
            Owner = null;
            this.Close();
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var frame = new ImageFrame(ListBox.SelectedItem.ToString());
            frame.Show();
        }

        private void STextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(STextBox.Text))
            {
                var newlist = ImageHandler.ImageSearch(STextBox.Text, TypeComboBox.SelectedItem.ToString(),
                    _imageFiles);
                var list = newlist.Select(imageFile => imageFile.Name).ToList();
                ListBox.ItemsSource = list;
            }
            else
            {
                var list = _imageFiles.Select(imageFile => imageFile.Name).ToList();
                ListBox.ItemsSource = list;
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            SetupListBox();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (Owner != null) { Owner.Close(); }
        }

    }
}
