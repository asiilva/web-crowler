using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebCrawlerNetImoveis.UtilsBD
{
    class ConfigureLayout
    {
        public void LayoutInitial(string url)
        {
            Console.WriteLine("                                                                               ");
            Console.WriteLine("                                                                               ");
            Console.WriteLine("                                                                               ");
            Console.WriteLine("                                                                               ");
            Console.WriteLine("                                                                               ");
            Console.WriteLine("                                                                               ");
            Console.WriteLine("  ---------------------------------------------------------------------------- ");
            Console.WriteLine("  -------------------------- .:: WEB CRAWLER  ::. ---------------------------- ");
            Console.WriteLine("  ---------------------------------------------------------------------------- ");
            Console.WriteLine("  ---------------------------- FONTE DE DADOS -------------------------------- ");
            Console.WriteLine("  ----------------------  " + url + " ---------------------------------------- ");
            Console.WriteLine("  ---------------------------------------------------------------------------- ");
        }

        public void LayoutFinalizationImportation(string url, int count)
        {
            Console.WriteLine("                                                                                 ");
            Console.WriteLine("                                                                                 ");
            Console.WriteLine("                                                                                 ");
            Console.WriteLine("                                                                                 ");
            Console.WriteLine("                                                                                 ");
            Console.WriteLine("                                                                                 ");
            Console.WriteLine("                                                                                 ");
            Console.WriteLine("                                                                                 ");
            Console.WriteLine("                                                                                 ");
            Console.WriteLine("                                                                                 ");
            Console.WriteLine("                                                                                 ");
            Console.WriteLine("   PROCESSO DE IMPORTAÇÃO DE DADOS DA PAGINA " + count + " REALIZADO COM SUCESSO ");
            Console.WriteLine("  ----------------------------------------------------------------------------   ");
            Console.WriteLine("   URL: " + url + "                                                              ");
            Console.WriteLine("                            Weeb Solutions                                       ");
            Console.WriteLine("                                                                 Version : 1.0   ");
            Console.WriteLine("  ----------------------------------------------------------------------------   ");
            Console.WriteLine("                                                                                 ");
            Console.WriteLine("                                                                                 ");
            Console.WriteLine("                                                                                 ");
            Console.WriteLine("                                                                                 ");
            Console.WriteLine("                                                                                 ");
            Console.WriteLine("                                                                                 ");
            Console.WriteLine("                                                                                 ");
            Console.WriteLine("                                                                                 ");
            Console.WriteLine("                                                                                 ");
        }

        public void LayoutException(Exception ex)
        {
            Console.WriteLine("  ---------------------------------------------------------------------------- ");
            Console.WriteLine("                            Weeb Solutions                                     ");
            Console.WriteLine("                                                                 Version : 1.0 ");
            Console.WriteLine("  ---------------------------------------------------------------------------- ");
            Console.WriteLine(Messages.OcurredError + ex.Message);
            Console.ReadLine();
        }

        public void LayoutExistRegister(string urlPage, long registerCurrent)
        {
            Console.WriteLine("                                                                               ");
            Console.WriteLine("                     " + Messages.ExistRegister + "                            ");
            Console.WriteLine("  ---------------------------------------------------------------------------- ");
            Console.WriteLine("   URL: " + urlPage + "                                                        ");
            Console.WriteLine("   Registro : " + registerCurrent + "                                                             ");
            Console.WriteLine("                            Weeb Solutions                                     ");
            Console.WriteLine("                                                                 Version : 1.0 ");
            Console.WriteLine("                                                                               ");
            Console.WriteLine("                                                                               ");
            Console.WriteLine("                                                                               ");
            Console.WriteLine("                                                                               ");
            Console.WriteLine("                                                                               ");
        }

        public void LayoutRegisterCadastred(string urlPage)
        {
            Console.WriteLine("                                                                               ");
            Console.WriteLine("                                                                               ");
            Console.WriteLine("                      " + Messages.RegisterCadastred + "                       ");
            Console.WriteLine("  ---------------------------------------------------------------------------- ");
            Console.WriteLine("   URL: " + urlPage + "                                                        ");
            Console.WriteLine("                            Weeb Solutions                                     ");
            Console.WriteLine("                                                                 Version : 1.0 ");
            Console.WriteLine("                                                                               ");
            Console.WriteLine("                                                                               ");
            Console.WriteLine("                                                                               ");
        }

        public void LayoutImportationFinalized(string urlPage)
        {
            Console.WriteLine("                                                                               ");
            Console.WriteLine("                                                                               ");
            Console.WriteLine("                      Processo de importação finalizado                        ");
            Console.WriteLine("  ---------------------------------------------------------------------------- ");
            Console.WriteLine("   URL: " + urlPage + "                                                        ");
            Console.WriteLine("                            Weeb Solutions                                     ");
            Console.WriteLine("                                                                 Version : 1.0 ");
            Console.WriteLine("                                                                               ");
            Console.WriteLine("                                                                               ");
            Console.WriteLine("                                                                               ");
        }
    }
}
