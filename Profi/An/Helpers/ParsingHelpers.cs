using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace An.Helpers
{
    public static class ParsingHelpers
    {
        public static Uri ToImageUri(this string source)
        {
            try
            {
                Uri uri = new Uri(source);
                var req = (HttpWebRequest)WebRequest.Create(uri.AbsoluteUri);
                req.Method = "HEAD";
                using (var resp = req.GetResponse())
                {
                    if (resp.ContentType.ToLower().StartsWith("image/"))
                    {
                        return uri;
                    }
                    else
                    {
                        return null;                        
                    }
                }

            }
            catch
            {                
                return null;
            }
        }
    }
}
