using System;
using System.Windows;
using Download_MThread.Core.Download;

namespace Download_MThread.Frames
{
    /// <summary>
    /// Interaction logic for ImageFrame.xaml
    /// </summary>
    public partial class ImageFrame : Window
    {
        private Boolean _close = false;

        public ImageFrame(String imageName)
        {
            InitializeComponent();
            LoadImage(imageName);
        }
        public void LoadImage(string imageName)
        {
            Title = imageName;
            var bitmap = ImageHandler.LoadImage(imageName);
            FileImage.Source = bitmap;
        }

        public void CloseImage()
        {
            _close = true;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_close) return;
            e.Cancel = true;
            Hide();
        }
    }
}
