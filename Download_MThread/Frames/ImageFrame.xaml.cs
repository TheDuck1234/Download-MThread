using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
            Image.Source = bitmap;
        }
    }
}
