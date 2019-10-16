using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ImageMagick;
using System.Windows;

namespace Profi.Controls
{
    public class Layer : Image
    {
        private MagickImage mImage;

        public Layer(MagickImage mImage)
        {            
            this.mImage = mImage;
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            Width = mImage.Width;
            Height = mImage.Height;
            Source = mImage.ToBitmapSource();
            Unloaded += Layer_Unloaded;                        
        }

        private void Layer_Unloaded(object sender, RoutedEventArgs e)
        {
            mImage.Dispose();
        }
    }
}
