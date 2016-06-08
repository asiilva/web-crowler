using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebCrawlerNetImoveis.ConsoleCrawler;

namespace WebCrawlerNetImoveis.ConsoleCrawler
{
    class ManageCrawler
    {
        static void Main(string[] args)
        {

            SpiderNetImoveisV2 netImoveisV2 = new SpiderNetImoveisV2();
            netImoveisV2.CrawlingNetImoveis();

            //SpiderRedeImvista spRedeImvista = new SpiderRedeImvista();
            //spRedeImvista.CrawlingRedeImvista();

            //SpiderNetImoveis spNetImoveis = new SpiderNetImoveis();
            //spNetImoveis.CrawlingNetImoveis();

            Console.Clear();


            Console.WriteLine("PROCESSO DE INDEXAÇÃO DE DADOS DA REDE NETIMOVEIS E REDE IMVISTA CONCLUIDO COM SUCESSO.");
            Console.ReadLine();
        }
    }
}
