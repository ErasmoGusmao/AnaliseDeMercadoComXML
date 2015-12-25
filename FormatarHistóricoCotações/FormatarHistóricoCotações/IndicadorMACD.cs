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
        public List<double> HistóricoLinhaMACD { get; private set;}
        public List<double> HistórcioSinalMACD { get; private set;}
        public List<double> HistográmaMACD {get; private set;}


        public IndicadorMACD(List<double> preçoFechamento,int períodoCurto, int períodoLongo, int períodoSinal) //Construtor
        {
            GerarHistóricoMACD(preçoFechamento,períodoCurto,períodoLongo,períodoSinal);
            GerarHistográmaMACD();
        }

        private void GerarHistóricoMACD(List<double> preçoFechamento,int períodoCurto, int períodoLongo, int períodoSinal) // Gera a linha do indicador MACD
        {

            HistóricoLinhaMACD = new List<double>();
            HistórcioSinalMACD = new List<double>();

            HistóricoLinhaMACD.Clear();
            HistórcioSinalMACD.Clear();


            MédiaMóvelExponencial MME_Lenta = new MédiaMóvelExponencial(preçoFechamento, períodoLongo);
            MédiaMóvelExponencial MME_Rápida = new MédiaMóvelExponencial(preçoFechamento, períodoCurto);
            MédiaMóvelExponencial MME_Sinal = new MédiaMóvelExponencial(preçoFechamento, períodoSinal);

            for (int i = 0; i < MME_Lenta.ListaDaMME.Count; i++)
            {
                HistóricoLinhaMACD.Add(MME_Rápida.ListaDaMME[i] - MME_Lenta.ListaDaMME[i]);
            }

            for (int i = 1; i < MME_Sinal.ListaDaMME.Count; i++) //Varre HistóricoLinhaMACD do segundo elemento válido
            {
                HistórcioSinalMACD.Add(MME_Sinal.ListaDaMME[i]);
            }
        }

        private void GerarHistográmaMACD() 
        {
            HistográmaMACD = new List<double>();
            HistográmaMACD.Clear();

            for (int i = 0; i < HistóricoLinhaMACD.Count; i++)
            {
                HistográmaMACD.Add(HistóricoLinhaMACD[i] - HistórcioSinalMACD[i]);
            }
        }
    }
}
