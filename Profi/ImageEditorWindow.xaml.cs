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
                HandleFiles(files);
                return;
            }

            //Caso seja uma url arrastada do browser
            object dropedFromBrowser = e.Data.GetData(DataFormats.Html);
            if (dropedFromBrowser != null)
            {                
                HandleHTML(dropedFromBrowser, e.GetPosition(mainGrid));
            }                       
        }

        private void HandleFiles(string[] files)
        {
            //TODO: carregaar imagens de arquivo
        }

        private void HandleHTML(object htmlSource, Point pos)
        {
            string source = null;
            var match = Regex.Match(htmlSource.ToString(), "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase);
            if(match.Groups.Count > 1)
            {
                source = match.Groups[1].Value;
            }
            
            if (source.ToLower().Contains(";base64,"))
            {
                string[] b64Split = source.Split(',');
                if (b64Split.Length == 2)
                {
                    byte[] imageBytes = Convert.FromBase64String(b64Split[1]);
                    using (var imageFile = new FileStream("teste.png", FileMode.Create))
                    {
                        imageFile.Write(imageBytes, 0, imageBytes.Length);
                        imageFile.Flush();
                    }
                }
                else
                {
                    //TODO: tratar erro. deu ruim
                }                                
            }
            else
            {
                Uri imageUri = source.ToImageUri();
                if (imageUri != null)
                {
                    //TODO: socar na tela
                    Console.WriteLine("sou imagem");
                }
                else
                {
                    //TODO: tratar erro. e uma url mas nao e imagem
                }
            }
        }

    }
}
