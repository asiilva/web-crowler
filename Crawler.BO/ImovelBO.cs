using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebCrawlerNetImoveis.Crawler.VO;
using WebCrawlerNetImoveis.Crawler.DAO;
using System.IO;

namespace WebCrawlerNetImoveis.Crawler.BO
{
    class ImovelBO
    {
        public void InsertImovel(ImoveisVO vo)
        {
            try
            {
                ImovelDAO dataAcess = new ImovelDAO();
                dataAcess.InsertImovel(vo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool ExistImovel(long codImovel)
        {
            ImovelDAO dao = new ImovelDAO();
            return dao.ExistImovel(codImovel);
        }

        public void LoggerDuplicateData(string logInformations)
        {
            StreamWriter stream = new StreamWriter(@"C:\Users\Alan\Dropbox\WebCrawler TIG Documentation\Net Imoveis\LogsNetImoveis\log.txt", true, Encoding.ASCII);
            stream.WriteLine(logInformations);
            stream.Close();
        }

        public void GenerateLog(long codigoImovel, string urlPage)
        {
            StringBuilder str = new StringBuilder();
            str.Append("Foi encontrado um registro duplicado.");
            str.Append(Environment.NewLine);
            str.Append("Url: " + urlPage);
            str.Append(Environment.NewLine);
            str.Append("Codigo do Registro: " + codigoImovel);
            str.Append(Environment.NewLine);
            this.LoggerDuplicateData(str.ToString());
        }

        public void InitLog(long codImovel)
        {
            StringBuilder str = new StringBuilder();
            str.Append("Foi iniciado o processo de importação do imovel de código: " + codImovel + ".");
            this.LoggerDuplicateData(str.ToString());
        }
    }
}
