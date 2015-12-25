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
    class IndicadorMACD
    {
        public List<double> ListaDaMACD { get; private set;}
        public List<double> ListaDoSinalMACD { get; private set;}
        public List<double> HistográmaMACD {get; private set;}


        public IndicadorMACD(List<double> preçoFechamento,int períodoCurto, int períodoLongo, int períodoSinal) //Construtor
        {
            GerarHistóricoMACD(preçoFechamento,períodoCurto,períodoLongo,períodoSinal);
            GerarHistográmaMACD();
        }

        private void GerarHistóricoMACD(List<double> preçoFechamento,int períodoCurto, int períodoLongo, int períodoSinal) // Gera a linha do indicador MACD
        {

            ListaDaMACD = new List<double>();
            ListaDoSinalMACD = new List<double>();

            ListaDaMACD.Clear();
            ListaDoSinalMACD.Clear();


            MédiaMóvelExponencial MME_Lenta = new MédiaMóvelExponencial(preçoFechamento, períodoLongo);
            MédiaMóvelExponencial MME_Rápida = new MédiaMóvelExponencial(preçoFechamento, períodoCurto);

            for (int i = 0; i < MME_Lenta.ListaDaMME.Count; i++)
            {
                ListaDaMACD.Add(MME_Rápida.ListaDaMME[i] - MME_Lenta.ListaDaMME[i]);
            }

            MédiaMóvelExponencial MME_Sinal = new MédiaMóvelExponencial(ListaDaMACD, períodoSinal);
            for (int i = 1; i < MME_Sinal.ListaDaMME.Count; i++) //Varre HistóricoLinhaMACD do segundo elemento válido
            {
                ListaDoSinalMACD.Add(MME_Sinal.ListaDaMME[i]);
            }
        }

        private void GerarHistográmaMACD() 
        {
            HistográmaMACD = new List<double>();
            HistográmaMACD.Clear();

            for (int i = 0; i < ListaDaMACD.Count; i++)
            {
                HistográmaMACD.Add(ListaDaMACD[i] - ListaDoSinalMACD[i]);
            }
        }
    }
}
