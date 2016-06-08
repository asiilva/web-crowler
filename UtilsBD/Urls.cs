using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebCrawlerNetImoveis.UtilsBD
{
    class Urls
    {
        public const string urlNetImoveis = "http://www.netimoveis.com.br";
        public const string urlNetImoveisCrawling = "http://www.netimoveis.com.br/resultado/Comprar/Casa/{0}/MG/?TipoId={1}&p={2}";
        public const string urlNetImoveisChildCrawling = "http://www.netimoveis.com.br/imovel/{0}";
        public const string urlRedeImvista = "http://www.imvista.com.br/";
        public const string urlRedeImvistaCrawling = "http://www.imvista.com.br/resultado-da-pesquisa.php?pesquisaOrigem=ok&situacao=Venda&estado=MG&multiselect_Estado=MG&cidade=Belo%20Horizonte&multiselect_Cidade=Belo%20Horizonte&tipo[]=Casa&multiselect_Tipo=Casa&bairros_relacionados=0&pagina_atual=1";
        public const string urlSearchImovelRedeImvista = "http://www.imvista.com.br/imovel/{0}";
    }
}
