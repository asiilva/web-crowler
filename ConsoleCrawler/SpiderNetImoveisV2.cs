using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebCrawlerNetImoveis.UtilsBD;
using WebCrawlerNetImoveis.Crawler.VO;
using System.Threading;
using System.Xml;
using WebCrawlerNetImoveis.Crawler.BO;
using System.Xml.Linq;

namespace WebCrawlerNetImoveis.ConsoleCrawler
{
    class SpiderNetImoveisV2 : RequestResponse
    {
        public void CrawlingNetImoveis()
        {
            try
            {
                List<CidadesVO> cidades = this.LoadParametersXml();

                ConfigureLayout cl = new ConfigureLayout();
                cl.LayoutInitial(Urls.urlNetImoveis);

                int? posicaoTag;
                int? posicaoSubString;
                int? posicaoInicioTag;
                int? posicaoFimTag;
                int[] vetTipoImovel = { 2, 3, 4, 5, 6, 7, 8, 9, 119, 125, 126, 128, 133, 140, 143, 145 };
                decimal qtdImovelResult = 1000;
                List<long> listCodImovel = null;

                foreach (CidadesVO cidade in cidades)
                {
                    for (int j = 0; j < vetTipoImovel.Count(); j++)
                    {
                        for (int i = 1; i <= qtdImovelResult; i++)
                        {
                            string urlPage = string.Format(Urls.urlNetImoveisCrawling, cidade.Key, vetTipoImovel[j], i);
                            string HTML = this.RequestResponseHTML(urlPage);

                            listCodImovel = new List<long>();

                            int tagIniciaImovel = 0;
                            int tagFechaImovel = 0;

                            posicaoTag = HTML.Trim().IndexOf("ContentPlaceHolder1_lb_total", tagIniciaImovel);
                            posicaoSubString = HTML.Trim().IndexOf(">", posicaoTag.Value) + 1;
                            posicaoFimTag = HTML.Trim().IndexOf("<", posicaoSubString.Value);
                            qtdImovelResult = Math.Ceiling(Convert.ToDecimal(HTML.Trim().Substring(posicaoSubString.Value, posicaoFimTag.Value - posicaoSubString.Value)) / 20);

                            if (qtdImovelResult == 0)
                            {
                                qtdImovelResult++;
                                break;
                            }
                            else
                            {
                                while (tagIniciaImovel != -1)
                                {
                                    tagIniciaImovel = HTML.Trim().IndexOf("<!-- Imovel-->", tagFechaImovel);

                                    posicaoTag = HTML.Trim().IndexOf("ListaCodigo", tagIniciaImovel);
                                    posicaoInicioTag = HTML.Trim().IndexOf(">COD. ", posicaoTag.Value) + 1;
                                    posicaoFimTag = HTML.Trim().IndexOf("<", posicaoInicioTag.Value);
                                    long codImovel = long.Parse((HTML.Trim().Substring(posicaoInicioTag.Value, posicaoFimTag.Value - posicaoInicioTag.Value)).Trim().Replace("COD. ", ""));

                                    if (!listCodImovel.Contains(codImovel))
                                        listCodImovel.Add(codImovel);

                                    tagFechaImovel = HTML.Trim().IndexOf("<!-- Fim do Imovel-->", tagIniciaImovel);
                                    tagIniciaImovel = HTML.Trim().IndexOf("<!-- Imovel-->", tagFechaImovel);

                                    if (tagIniciaImovel == -1)
                                    {
                                        cl.LayoutFinalizationImportation(urlPage, i);
                                    }
                                }

                                foreach (long itemCod in listCodImovel)
                                {
                                    ImovelBO bo = new ImovelBO();

                                    if (!bo.ExistImovel(itemCod))
                                    {
                                        bo.InitLog(itemCod);

                                        string urlChild = string.Format(Urls.urlNetImoveisChildCrawling, itemCod);
                                        string filtratedHtml;

                                        Thread.Sleep(1500);

                                        string htmlChild = this.RequestResponseHTML(urlChild);

                                        if (htmlChild.Trim().IndexOf("DetalhePrecoCondIptu", 0) != -1)
                                        {
                                            ImoveisVO vo = new ImoveisVO();

                                            posicaoTag = htmlChild.Trim().IndexOf("DetalhePrecoCondIptu", 0);
                                            posicaoTag = htmlChild.Trim().IndexOf("Preco", posicaoTag.Value);
                                            posicaoTag = htmlChild.Trim().IndexOf("DetalhePreco", posicaoTag.Value);
                                            posicaoInicioTag = htmlChild.Trim().IndexOf(">", posicaoTag.Value) + 1;
                                            posicaoFimTag = htmlChild.Trim().IndexOf("<", posicaoInicioTag.Value);
                                            filtratedHtml = htmlChild.Trim().Substring(posicaoInicioTag.Value, posicaoFimTag.Value - posicaoInicioTag.Value);
                                            vo.Preco = string.IsNullOrEmpty(filtratedHtml) ? 0 : Convert.ToDecimal(filtratedHtml.Replace("R$", " "));

                                            int posicaoTagMarcacaoInicio = htmlChild.Trim().IndexOf("CaracPrincipal", 0);

                                            posicaoTag = htmlChild.Trim().IndexOf("dscTipo", posicaoTagMarcacaoInicio);
                                            posicaoInicioTag = htmlChild.Trim().IndexOf(">", posicaoTag.Value) + 1;
                                            posicaoFimTag = htmlChild.Trim().IndexOf("<", posicaoInicioTag.Value);
                                            vo.Tipo = htmlChild.Trim().Substring(posicaoInicioTag.Value, posicaoFimTag.Value - posicaoInicioTag.Value);

                                            posicaoTag = htmlChild.Trim().IndexOf("DetalheBairro", 0);
                                            posicaoTag = htmlChild.Trim().IndexOf("_Bairro", posicaoTag.Value);
                                            posicaoInicioTag = htmlChild.Trim().IndexOf(">", posicaoTag.Value) + 1;
                                            posicaoFimTag = htmlChild.Trim().IndexOf("<", posicaoInicioTag.Value);
                                            vo.Bairro = htmlChild.Trim().Substring(posicaoInicioTag.Value, posicaoFimTag.Value - posicaoInicioTag.Value);

                                            vo.Codigo = itemCod;

                                            posicaoTag = htmlChild.Trim().IndexOf("Banhos", 0);
                                            posicaoTag = htmlChild.Trim().IndexOf("_banho", posicaoTag.Value);
                                            posicaoInicioTag = htmlChild.Trim().IndexOf("b>", posicaoTag.Value) + 2;
                                            posicaoFimTag = htmlChild.Trim().IndexOf("</b", posicaoInicioTag.Value);
                                            filtratedHtml = htmlChild.Trim().Substring(posicaoInicioTag.Value, posicaoFimTag.Value - posicaoInicioTag.Value);
                                            vo.QtdBanheiros = string.IsNullOrEmpty(filtratedHtml) ? 0 : int.Parse(filtratedHtml);

                                            posicaoTag = htmlChild.Trim().IndexOf("interfone", posicaoTagMarcacaoInicio);
                                            posicaoInicioTag = htmlChild.Trim().IndexOf("b>", posicaoTag.Value) + 2;
                                            posicaoFimTag = htmlChild.Trim().IndexOf("</b", posicaoInicioTag.Value);
                                            vo.Interfone = htmlChild.Trim().Substring(posicaoInicioTag.Value, posicaoFimTag.Value - posicaoInicioTag.Value).ToLower().Equals("sim") ? true : false;

                                            posicaoTag = htmlChild.Trim().IndexOf("portale", posicaoTagMarcacaoInicio);
                                            posicaoInicioTag = htmlChild.Trim().IndexOf("b>", posicaoTag.Value) + 2;
                                            posicaoFimTag = htmlChild.Trim().IndexOf("</b", posicaoInicioTag.Value);
                                            vo.PortaoEletronico = htmlChild.Trim().Substring(posicaoInicioTag.Value, posicaoFimTag.Value - posicaoInicioTag.Value).ToLower().Equals("sim") ? true : false;

                                            posicaoTag = htmlChild.Trim().IndexOf("areaPri", posicaoTagMarcacaoInicio);
                                            posicaoInicioTag = htmlChild.Trim().IndexOf("b>", posicaoTag.Value) + 2;
                                            posicaoFimTag = htmlChild.Trim().IndexOf("</b", posicaoInicioTag.Value);
                                            filtratedHtml = htmlChild.Trim().Substring(posicaoInicioTag.Value, posicaoFimTag.Value - posicaoInicioTag.Value);
                                            vo.Area = string.IsNullOrEmpty(filtratedHtml) ? 0 : float.Parse(filtratedHtml.Replace("m²", " ").Replace("Hec", " "));

                                            posicaoTag = htmlChild.Trim().IndexOf("elevadorsocial", posicaoTagMarcacaoInicio);
                                            posicaoInicioTag = htmlChild.Trim().IndexOf("b>", posicaoTag.Value) + 2;
                                            posicaoFimTag = htmlChild.Trim().IndexOf("</b", posicaoInicioTag.Value);
                                            vo.ElevadorSocial = htmlChild.Trim().Substring(posicaoInicioTag.Value, posicaoFimTag.Value - posicaoInicioTag.Value).ToLower().Equals("sim") ? true : false;

                                            posicaoTag = htmlChild.Trim().IndexOf("suites", posicaoTagMarcacaoInicio);
                                            posicaoInicioTag = htmlChild.Trim().IndexOf("b>", posicaoTag.Value) + 2;
                                            posicaoFimTag = htmlChild.Trim().IndexOf("</b", posicaoInicioTag.Value);
                                            filtratedHtml = htmlChild.Trim().Substring(posicaoInicioTag.Value, posicaoFimTag.Value - posicaoInicioTag.Value);
                                            vo.Qtdsuites = string.IsNullOrEmpty(filtratedHtml) ? 0 : int.Parse(filtratedHtml);

                                            posicaoTag = htmlChild.Trim().IndexOf("npavimentos", posicaoTagMarcacaoInicio);
                                            posicaoInicioTag = htmlChild.Trim().IndexOf("b>", posicaoTag.Value) + 2;
                                            posicaoFimTag = htmlChild.Trim().IndexOf("</b", posicaoInicioTag.Value);
                                            filtratedHtml = htmlChild.Trim().Substring(posicaoInicioTag.Value, posicaoFimTag.Value - posicaoInicioTag.Value);
                                            vo.QtdPavimentos = string.IsNullOrEmpty(filtratedHtml) ? 0 : int.Parse(filtratedHtml);

                                            posicaoTag = htmlChild.Trim().IndexOf("salaofestas", posicaoTagMarcacaoInicio);
                                            posicaoInicioTag = htmlChild.Trim().IndexOf("b>", posicaoTag.Value) + 2;
                                            posicaoFimTag = htmlChild.Trim().IndexOf("</b", posicaoInicioTag.Value);
                                            vo.Salaofestas = htmlChild.Trim().Substring(posicaoInicioTag.Value, posicaoFimTag.Value - posicaoInicioTag.Value).ToLower().Equals("sim") ? true : false;

                                            posicaoTag = htmlChild.Trim().IndexOf("pscina", posicaoTagMarcacaoInicio);
                                            posicaoInicioTag = htmlChild.Trim().IndexOf("b>", posicaoTag.Value) + 2;
                                            posicaoFimTag = htmlChild.Trim().IndexOf("</b", posicaoInicioTag.Value);
                                            vo.Piscina = htmlChild.Trim().Substring(posicaoInicioTag.Value, posicaoFimTag.Value - posicaoInicioTag.Value).ToLower().Equals("sim") ? true : false;

                                            posicaoTag = htmlChild.Trim().IndexOf("nsalas", posicaoTagMarcacaoInicio);
                                            posicaoInicioTag = htmlChild.Trim().IndexOf("b>", posicaoTag.Value) + 2;
                                            posicaoFimTag = htmlChild.Trim().IndexOf("</b", posicaoInicioTag.Value);
                                            filtratedHtml = htmlChild.Trim().Substring(posicaoInicioTag.Value, posicaoFimTag.Value - posicaoInicioTag.Value);
                                            vo.Qtdsalas = string.IsNullOrEmpty(filtratedHtml) ? 0 : int.Parse(filtratedHtml);

                                            posicaoTag = htmlChild.Trim().IndexOf("quartos", posicaoTagMarcacaoInicio);
                                            posicaoInicioTag = htmlChild.Trim().IndexOf("b>", posicaoTag.Value) + 2;
                                            posicaoFimTag = htmlChild.Trim().IndexOf("</b", posicaoInicioTag.Value);
                                            filtratedHtml = htmlChild.Trim().Substring(posicaoInicioTag.Value, posicaoFimTag.Value - posicaoInicioTag.Value);
                                            vo.Qtdquartos = string.IsNullOrEmpty(filtratedHtml) ? 0 : int.Parse(filtratedHtml);

                                            posicaoTag = htmlChild.Trim().IndexOf("varandas", posicaoTagMarcacaoInicio);
                                            posicaoInicioTag = htmlChild.Trim().IndexOf("b>", posicaoTag.Value) + 2;
                                            posicaoFimTag = htmlChild.Trim().IndexOf("</b", posicaoInicioTag.Value);
                                            filtratedHtml = htmlChild.Trim().Substring(posicaoInicioTag.Value, posicaoFimTag.Value - posicaoInicioTag.Value);
                                            vo.Qtdvarandas = string.IsNullOrEmpty(filtratedHtml) ? 0 : int.Parse(filtratedHtml);

                                            posicaoTag = htmlChild.Trim().IndexOf("portariapermanente", posicaoTagMarcacaoInicio);
                                            posicaoInicioTag = htmlChild.Trim().IndexOf("b>", posicaoTag.Value) + 2;
                                            posicaoFimTag = htmlChild.Trim().IndexOf("</b", posicaoInicioTag.Value);
                                            vo.PortariaPermanente = htmlChild.Trim().Substring(posicaoInicioTag.Value, posicaoFimTag.Value - posicaoInicioTag.Value).ToLower().Equals("sim") ? true : false;

                                            posicaoTag = htmlChild.Trim().IndexOf("posicao", posicaoTagMarcacaoInicio);
                                            posicaoInicioTag = htmlChild.Trim().IndexOf("b>", posicaoTag.Value) + 2;
                                            posicaoFimTag = htmlChild.Trim().IndexOf("</b", posicaoInicioTag.Value);
                                            vo.Posicao = htmlChild.Trim().Substring(posicaoInicioTag.Value, posicaoFimTag.Value - posicaoInicioTag.Value);

                                            posicaoTag = htmlChild.Trim().IndexOf("vagas", posicaoTagMarcacaoInicio);
                                            posicaoInicioTag = htmlChild.Trim().IndexOf("b>", posicaoTag.Value) + 2;
                                            posicaoFimTag = htmlChild.Trim().IndexOf("</b", posicaoInicioTag.Value);
                                            filtratedHtml = htmlChild.Trim().Substring(posicaoInicioTag.Value, posicaoFimTag.Value - posicaoInicioTag.Value);
                                            vo.Qtdvagas = string.IsNullOrEmpty(filtratedHtml) ? 0 : int.Parse(filtratedHtml);

                                            posicaoTag = htmlChild.Trim().IndexOf("uniporandar", posicaoTagMarcacaoInicio);
                                            posicaoInicioTag = htmlChild.Trim().IndexOf("b>", posicaoTag.Value) + 2;
                                            posicaoFimTag = htmlChild.Trim().IndexOf("</b", posicaoInicioTag.Value);
                                            filtratedHtml = htmlChild.Trim().Substring(posicaoInicioTag.Value, posicaoFimTag.Value - posicaoInicioTag.Value);
                                            vo.Qtduniporandar = string.IsNullOrEmpty(filtratedHtml) ? 0 : int.Parse(filtratedHtml);

                                            posicaoTag = htmlChild.Trim().IndexOf("consominio", posicaoTagMarcacaoInicio);
                                            posicaoInicioTag = htmlChild.Trim().IndexOf("b>", posicaoTag.Value) + 2;
                                            posicaoFimTag = htmlChild.Trim().IndexOf("</b", posicaoInicioTag.Value);
                                            filtratedHtml = htmlChild.Trim().Substring(posicaoInicioTag.Value, posicaoFimTag.Value - posicaoInicioTag.Value);
                                            vo.PrecoCondominio = string.IsNullOrEmpty(filtratedHtml) ? 0 : decimal.Parse(filtratedHtml.Replace("R$", " "));

                                            posicaoTag = htmlChild.Trim().IndexOf("iptu", posicaoTagMarcacaoInicio);
                                            posicaoInicioTag = htmlChild.Trim().IndexOf("b>", posicaoTag.Value) + 2;
                                            posicaoFimTag = htmlChild.Trim().IndexOf("</b", posicaoInicioTag.Value);
                                            filtratedHtml = htmlChild.Trim().Substring(posicaoInicioTag.Value, posicaoFimTag.Value - posicaoInicioTag.Value);
                                            vo.PrecoIptu = string.IsNullOrEmpty(filtratedHtml) ? 0 : decimal.Parse(filtratedHtml.Replace("R$", " "));

                                            posicaoTag = htmlChild.Trim().IndexOf("playground", posicaoTagMarcacaoInicio);
                                            posicaoInicioTag = htmlChild.Trim().IndexOf("b>", posicaoTag.Value) + 2;
                                            posicaoFimTag = htmlChild.Trim().IndexOf("</b", posicaoInicioTag.Value);
                                            vo.Playground = htmlChild.Trim().Substring(posicaoInicioTag.Value, posicaoFimTag.Value - posicaoInicioTag.Value).ToLower().Equals("sim") ? true : false;

                                            posicaoTag = htmlChild.Trim().IndexOf("elevadorServico", posicaoTagMarcacaoInicio);
                                            posicaoInicioTag = htmlChild.Trim().IndexOf("b>", posicaoTag.Value) + 2;
                                            posicaoFimTag = htmlChild.Trim().IndexOf("</b", posicaoInicioTag.Value);
                                            vo.ElevadorServico = htmlChild.Trim().Substring(posicaoInicioTag.Value, posicaoFimTag.Value - posicaoInicioTag.Value).ToLower().Equals("sim") ? true : false;

                                            vo.Cidade = cidade.Key;
                                            vo.Origem = "Rede Net Imoveis";
                                            vo.Estado = "Minas Gerais";
                                            vo.DataHora = DateTime.Now;

                                            CrawlerSerialize serialize = new CrawlerSerialize();
                                            serialize.SaveData(vo, urlChild, itemCod);
                                        }
                                    }
                                    else
                                    {
                                        bo.GenerateLog(itemCod, urlPage);
                                        cl.LayoutExistRegister(urlPage, itemCod);
                                    }
                                }
                            }
                            Thread.Sleep(1500);
                        }
                    }
                }

                cl.LayoutImportationFinalized(Urls.urlNetImoveis);
            }
            catch (Exception ex)
            {
                ConfigureLayout cl = new ConfigureLayout();
                cl.LayoutException(ex);
            }
        }

        public List<CidadesVO> LoadParametersXml()
        {
            List<CidadesVO> cidades = new List<CidadesVO>();

            XDocument doc = XDocument.Load("C:/Users/Alan/Dropbox/Weeb Crawler NetMoveis Development/SubversionDropBoxNetmoveis/Trunk/WebCrawler TIG/Net Imoveis/WebCrawlerNetImoveis/WebCrawlerNetImoveis/Crawler.VO/cidades.xml");

            foreach (XElement elementCidade in doc.Root.Elements("Estado"))
            {
                XElement cidade = elementCidade.Element("Cidades");

                if (cidade != null)
                {
                    foreach (XElement element in cidade.Elements("parameters"))
                    {
                        foreach (XElement parameterElement in element.Nodes())
                        {
                            CidadesVO vo = new CidadesVO();

                            vo.Key = parameterElement.Attribute("key").Value;
                            vo.Value = parameterElement.Attribute("value").Value;

                            cidades.Add(vo);
                        }
                    }
                }
            }

            return cidades;
        }
    }
}
