using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using WebCrawlerNetImoveis.Crawler.VO;
using WebCrawlerNetImoveis.Crawler.BO;
using WebCrawlerNetImoveis.UtilsBD;
using System.Threading;

namespace WebCrawlerNetImoveis.ConsoleCrawler
{
    class SpiderNetImoveis : RequestResponse
    {
        public void CrawlingNetImoveis()
        {
            try
            {
                ConfigureLayout cl = new ConfigureLayout();
                cl.LayoutInitial(Urls.urlNetImoveis);

                int? posicao;
                int? posicaoSubString;
                int? posicaoFim;
                int[] vetTipoImovel = { 2, 3, 4, 5, 6, 7 };
                decimal qtdImovelResult = 1000;

                for (int j = 0; j < vetTipoImovel.Count(); j++)
                {
                    for (int i = 1; i <= qtdImovelResult; i++)
                    {
                        string urlPage = string.Format(Urls.urlNetImoveisCrawling, Cidades.BeloHorizonte, vetTipoImovel[j], i);
                        int registerCurrent = 0;

                        string HTML = this.RequestResponseHTML(urlPage);

                        int count = 0;
                        int count2 = 0;

                        posicao = HTML.Trim().IndexOf("ContentPlaceHolder1_lb_total", count);
                        posicaoSubString = HTML.Trim().IndexOf(">", posicao.Value) + 1;
                        posicaoFim = HTML.Trim().IndexOf("<", posicaoSubString.Value);
                        qtdImovelResult = Math.Ceiling(Convert.ToDecimal(HTML.Trim().Substring(posicaoSubString.Value, posicaoFim.Value - posicaoSubString.Value)) / 20);

                        while (count2 != -1)
                        {
                            #region Index de dados da paginas
                            ImovelVO vo = new ImovelVO();
                            registerCurrent++;
                            vo.Cidade = Cidades.BeloHorizonte;

                            count = HTML.Trim().IndexOf("<!-- Imovel-->", count);
                            posicao = HTML.Trim().IndexOf("ListaCampoTipo", count);
                            posicaoSubString = HTML.Trim().IndexOf(">", posicao.Value);
                            posicaoFim = HTML.Trim().IndexOf("<", posicaoSubString.Value) - 1;

                            vo.TipoImovel = HTML.Trim().Substring(posicaoSubString.Value + 1, posicaoFim.Value - posicaoSubString.Value);

                            posicao = HTML.Trim().IndexOf("ListaCampoBairro", count);
                            posicaoSubString = HTML.Trim().IndexOf(">", posicao.Value);
                            posicaoFim = HTML.Trim().IndexOf("<", posicao.Value) - 1;

                            vo.Bairro = HTML.Trim().Substring(posicaoSubString.Value + 1, posicaoFim.Value - posicaoSubString.Value);

                            posicao = HTML.Trim().IndexOf("ListaCampoArea", count);
                            posicaoSubString = HTML.Trim().IndexOf(">", posicao.Value);
                            int posinicio = HTML.Trim().IndexOf(">", posicaoSubString.Value) + 1;
                            int ultimaPos = HTML.Trim().IndexOf("<", posicaoSubString.Value);

                            string area = HTML.Trim().Substring(posicaoSubString.Value + 1, ultimaPos - posinicio);
                            vo.Area = string.IsNullOrEmpty(area) ? 0 : double.Parse(area);

                            posicao = HTML.Trim().IndexOf("ListaCampoVagas", count);
                            posinicio = HTML.Trim().IndexOf(">", posicao.Value);
                            posicaoFim = HTML.Trim().IndexOf("<", posinicio) - 1;

                            string vagas = HTML.Trim().Substring(posinicio + 1, posicaoFim.Value - posinicio);

                            if (!string.IsNullOrEmpty(vagas))
                                vo.VagasGaragem = int.Parse(vagas);
                            else
                                vo.VagasGaragem = 0;

                            posicao = HTML.Trim().IndexOf("ListaCampoPreco", count);
                            posicaoSubString = HTML.Trim().IndexOf("font-size:20px;", count);

                            if (posicaoSubString.Value == -1)
                            {
                                posicaoSubString = HTML.Trim().IndexOf("PrecoImovel", count);
                            }

                            posinicio = HTML.Trim().IndexOf(">", posicaoSubString.Value) + 1;
                            ultimaPos = HTML.Trim().IndexOf("<", posicaoSubString.Value) - 1;

                            string preco = (HTML.Trim().Substring(posinicio, ultimaPos - posinicio)).Trim();
                            vo.Preco = double.Parse(preco.Replace(",", "."));
                            vo.DataRegistro = DateTime.Now;

                            posicao = HTML.Trim().IndexOf("ListaCodigo", count);
                            posinicio = HTML.Trim().IndexOf(">COD. ", posicao.Value) + 1;
                            ultimaPos = HTML.Trim().IndexOf("<", posinicio);
                            vo.CodigoImovel = long.Parse((HTML.Trim().Substring(posinicio, ultimaPos - posinicio)).Trim().Replace("COD. ", ""));
                            vo.Origem = Origem.NetImoveis;

                            count = HTML.Trim().IndexOf("<!-- Fim do Imovel-->", count);
                            count2 = HTML.Trim().IndexOf("<!-- Imovel-->", count);

                            CrawlerSerialize master = new CrawlerSerialize();
                            //master.SaveData(vo, urlPage, registerCurrent);

                            if (count2 == -1)
                            {
                                cl.LayoutFinalizationImportation(urlPage, i);
                            }

                            #endregion
                        }

                        Thread.Sleep(3000);
                    }
                }
            }
            catch (Exception ex)
            {
                ConfigureLayout cl = new ConfigureLayout();
                cl.LayoutException(ex);
            }
        }
    }
}
