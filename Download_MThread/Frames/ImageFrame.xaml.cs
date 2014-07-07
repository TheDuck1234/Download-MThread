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
        public ImageFrame(String imageName)
        {
            InitializeComponent();
            LoadImage(imageName);
        }
        private void LoadImage(string imageName)
        {
            Title = imageName;
            var bitmap = ImageHandler.LoadImage(imageName);
            FileImage.Source = bitmap;
        }
    }
}
