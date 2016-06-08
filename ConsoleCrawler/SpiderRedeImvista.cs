using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using WebCrawlerNetImoveis.UtilsBD;
using System.IO;
using System.Xml;
using WebCrawlerNetImoveis.Crawler.VO;

namespace WebCrawlerNetImoveis.ConsoleCrawler
{
    class SpiderRedeImvista : RequestResponse
    {
        public void CrawlingRedeImvista()
        {
            RedeImvistaVO vo = new RedeImvistaVO();

            string HTML = this.RequestResponseHTML(Urls.urlRedeImvistaCrawling);

            int marcador = HTML.IndexOf("imovSel");
            int posinicio = HTML.IndexOf("<p>", marcador);
            int posfim = HTML.IndexOf("</p>", posinicio);

            vo.CodigoImovel = HTML.Substring(posinicio, posfim - posinicio).Replace("\n", "").Replace("\t", "").Replace("<p>C�d.", "").TrimEnd();

            string urlSearchImovel = String.Format(Urls.urlSearchImovelRedeImvista, vo.CodigoImovel);

            string HTMLChild = this.RequestResponseHTML(urlSearchImovel);
            
        }
    }
}
