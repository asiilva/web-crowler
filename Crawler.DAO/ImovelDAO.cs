using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebCrawlerNetImoveis.Crawler.VO;
using WebCrawlerNetImoveis.UtilsBD;

namespace WebCrawlerNetImoveis.Crawler.DAO
{
    class ImovelDAO
    {
        public void InsertImovel(ImoveisVO vo)
        {
            try
            {
                string sql = @"INSERT INTO [BDNetMoveis].[dbo].[Imoveis]
                                ([TipoImovel]
                                ,[Bairro]
                                ,[CodigoImovel]
                                ,[Preco]
                                ,[QtdBanheiros]
                                ,[Interfone]
                                ,[PortaoEletronico]
                                ,[Area]
                                ,[ElevadorSocial]
                                ,[QtdSuites]
                                ,[QtdPavimentos]
                                ,[SalaFesta]
                                ,[Piscina]
                                ,[QtdSala]
                                ,[QtdQuarto]
                                ,[QtdVaranda]
                                ,[PortariaPermanente]
                                ,[Posicao]
                                ,[QtdVaga]
                                ,[QtdUnidPorAndar]
                                ,[PrecoCondominio]
                                ,[PrecoIptu]
                                ,[PlayGround]
                                ,[ElevadorServico]
                                ,[Cidade]
                                ,[Estado]
                                ,[DataHora]
                                ,[Origem])
                              VALUES
                                 (@TipoImovel
                                 ,@Bairro
                                 ,@CodigoImovel
                                 ,@Preco
                                 ,@QtdBanheiros
                                 ,@Interfone
                                 ,@PortaoEletronico
                                 ,@Area
                                 ,@ElevadorSocial
                                 ,@QtdSuites
                                 ,@QtdPavimentos
                                 ,@SalaFesta
                                 ,@Piscina
                                 ,@QtdSala
                                 ,@QtdQuarto
                                 ,@QtdVaranda
                                 ,@PortariaPermanente
                                 ,@Posicao
                                 ,@QtdVaga
                                 ,@QtdUnidPorAndar
                                 ,@PrecoCondominio
                                 ,@PrecoIptu
                                 ,@PlayGround
                                 ,@ElevadorServico
                                 ,@Cidade
                                 ,@Estado
                                 ,@DataHora
                                 ,@Origem)";

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@TipoImovel", vo.Tipo);
                parameters.Add("@Bairro", vo.Bairro);
                parameters.Add("@CodigoImovel", vo.Codigo);
                parameters.Add("@Preco", vo.Preco);
                parameters.Add("@QtdBanheiros", vo.QtdBanheiros);
                parameters.Add("@Interfone", vo.Interfone);
                parameters.Add("@PortaoEletronico", vo.PortaoEletronico);
                parameters.Add("@Area", vo.Area);
                parameters.Add("@ElevadorSocial", vo.ElevadorSocial);
                parameters.Add("@QtdSuites", vo.Qtdsuites);
                parameters.Add("@QtdPavimentos", vo.QtdPavimentos);
                parameters.Add("@SalaFesta", vo.Salaofestas);
                parameters.Add("@Piscina", vo.Piscina);
                parameters.Add("@QtdSala", vo.Qtdsalas);
                parameters.Add("@QtdQuarto", vo.Qtdquartos);
                parameters.Add("@QtdVaranda", vo.Qtdvarandas);
                parameters.Add("@PortariaPermanente", vo.PortariaPermanente);
                parameters.Add("@Posicao", vo.Posicao);
                parameters.Add("@QtdVaga", vo.Qtdvagas);
                parameters.Add("@QtdUnidPorAndar", vo.Qtduniporandar);
                parameters.Add("@PrecoCondominio", vo.PrecoCondominio);
                parameters.Add("@PrecoIptu", vo.PrecoIptu);
                parameters.Add("@PlayGround", vo.Playground);
                parameters.Add("@ElevadorServico", vo.ElevadorServico);
                parameters.Add("@Cidade", vo.Cidade);
                parameters.Add("@Estado", vo.Estado);
                parameters.Add("@DataHora", vo.DataHora);
                parameters.Add("@Origem", vo.Origem);


                ADONetmoveis ado = new ADONetmoveis();
                ado.Execute(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool ExistImovel(long codImovel)
        {
            string sql = @"SELECT CodigoImovel FROM IMOVEIS WHERE CodigoImovel = @CodigoImovel";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("CodigoImovel", codImovel);

            ADONetmoveis ado = new ADONetmoveis();
            ado.Execute(sql, parameters);

            return ado.ExistImovel(sql, parameters);
        }
    }
}
