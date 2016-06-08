using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebCrawlerNetImoveis.Crawler.VO;
using WebCrawlerNetImoveis.Crawler.BO;
using WebCrawlerNetImoveis.UtilsBD;

namespace WebCrawlerNetImoveis.ConsoleCrawler
{
    class CrawlerSerialize
    {
        public void SaveData(ImoveisVO vo, string urlPage, long codCurrent)
        {
            ImovelBO bo = new ImovelBO();
            ConfigureLayout cl = new ConfigureLayout();

            bo.InsertImovel(vo);
            cl.LayoutRegisterCadastred(urlPage);
        }
    }
}
