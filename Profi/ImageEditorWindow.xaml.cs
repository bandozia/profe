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
using System.Text.RegularExpressions;
using An.Helpers;
using ImageMagick;
using System.IO;
using An.IO;
using Profi.Controls;
using Profi.Controls.Visuals;

namespace Profi
{
    public partial class ImageEditorWindow : Window
    {
        public ImageEditorWindow()
        {
            InitializeComponent();
            Drop += ImageEditorWindow_Drop;
        }

        private void ImageEditorWindow_Drop(object sender, DragEventArgs e)
        {
            //Caso sejam arquivos de imagem
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null)
            {
                AddLayer(DropHandler.HandleFilesDrop(files), e.GetPosition(layersGrid));
                return;
            }

            //Caso seja uma url arrastada do browser
            object dropedFromBrowser = e.Data.GetData(DataFormats.Html);
            if (dropedFromBrowser != null)
            {
                LoadingSpiner loading = new LoadingSpiner { 
                    Margin = new Thickness(e.GetPosition(layersGrid).X, e.GetPosition(layersGrid).Y, 0, 0) 
                };
                mainGrid.Children.Add(loading);
                Task.Run(() =>
                {
                    byte[] imageBytes = DropHandler.GetImageBytes(dropedFromBrowser);
                    Dispatcher.Invoke(() =>
                    {
                        AddLayer(DropHandler.HandleHtmlDrop(imageBytes), e.GetPosition(layersGrid));
                        mainGrid.Children.Remove(loading);
                    });
                });    
            }                       
        }
        
        private void AddLayer(Layer layer, Point position)
        {
            layer.Margin = new Thickness(position.X, position.Y, 0, 0);
            layersGrid.Children.Add(layer);
        }

        private void AddLayer(List<Layer> layers, Point position)
        {
            foreach (Layer layer in layers)
            {
                AddLayer(layer, position);
            }
        }
        
                

    }
}
