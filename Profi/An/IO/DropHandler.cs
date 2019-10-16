using An.Helpers;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Profi.Controls;
using System.Net;

namespace An.IO
{
    public class DropHandler
    {
        public static List<Layer> HandleFilesDrop(string[] files)
        {
            var layerList = new List<Layer>();
            foreach (string file in files)
            {
                MagickImage img = new MagickImage(file);
                layerList.Add(new Layer(img));
            }

            return layerList;
        }
        
        public static Layer HandleHtmlDrop(byte[] imageBytes)
        {
            if (imageBytes != null)
            {
                MagickImage img = new MagickImage(imageBytes);
                return new Layer(img);
            }
            else
            {
                return GetErrorImage();
            }          
        }

        public static byte[] GetImageBytes(object htmlSource)
        {
            string source = null;
            var match = Regex.Match(htmlSource.ToString(), "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase);
            if (match.Groups.Count > 1)
            {
                source = match.Groups[1].Value;
            }

            if (source.ToLower().Contains(";base64,"))
            {
                string[] b64Split = source.Split(',');
                if (b64Split.Length == 2)
                {
                    byte[] imageBytes = Convert.FromBase64String(b64Split[1]);
                    return imageBytes;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                Uri imageUri = source.ToImageUri();
                if (imageUri != null)
                {
                    using (var client = new WebClient())
                    {
                        byte[] imageBytes = client.DownloadData(imageUri);
                        return imageBytes;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public static Layer GetErrorImage()
        {
            MagickImage errorImage = new MagickImage(new MagickColor("#000000"), 300, 300);
            new Drawables()
                .TextAlignment(TextAlignment.Center)
                .FillColor(MagickColors.Red)
                .Text(0, 0, "ERRO NA IMAGEM")
                .Draw(errorImage);

            return new Layer(errorImage);
        }

    }
}
