using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Collections;
using System.Xml;
using System.Xml.Linq;

namespace FormatarHistóricoCotações
{
    class Estatística
    {
        public double Soma(List<double> ListaValores, int período) 
        {
            double soma;
            soma = 0;
            for (int i = 0; i < período; i++)
            {
                soma += ListaValores[i];
            }
            return soma;
        }

        public double Média(List<double> ListaValores, int período) 
        {
            return (Soma(ListaValores,período)/((double)período));
        }

        public double Variância(List<double> ListaValores, int período) 
        {
            List<double> Delta = new List<double>(); //Declara uma variável que será dada por Delta[i] = (ListaValores[i] - média)^2
            Delta.Clear();

            for (int i = 0; i < período; i++)
            {
                Delta[i] = Math.Pow(ListaValores[i] - Média(ListaValores, período), 2);
            }
            return (Soma(Delta,período)/(double)período);
        }

        public double DesvPad(List<double> ListaValores, int período) 
        {
            return Math.Sqrt(Variância(ListaValores, período));
        }
    }
}
