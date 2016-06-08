using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace WebCrawlerNetImoveis.UtilsBD
{
    class RequestResponse
    {
        public string RequestResponseHTML(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader stream = new StreamReader(response.GetResponseStream());
            string HTML = stream.ReadToEnd();

            return HTML;
        }
    }
}
