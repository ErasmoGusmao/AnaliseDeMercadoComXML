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
        private List<double> listaTempMACD;                         //Para calculo da ListaDoSinalMACD
        public List<double> ListaDoSinalMACD { get; private set;}
        public List<double> HistográmaMACD {get; private set;}


        public IndicadorMACD(List<double> preçoFechamento,int períodoCurto, int períodoLongo, int períodoSinal) 
        {//Construtor
            GerarHistóricoMACD(preçoFechamento,períodoCurto,períodoLongo,períodoSinal);
            GerarHistográmaMACD();
        }
        public IndicadorMACD(List<Papeis> HistóricoPapel, int períodoCurto, int períodoLongo, int períodoSinal)
        {//Construtor
            GerarHistóricoMACD(HistóricoPapel, períodoCurto, períodoLongo, períodoSinal);
            GerarHistográmaMACD();
        }

        private void GerarHistóricoMACD(List<double> preçoFechamento,int períodoCurto, int períodoLongo, int períodoSinal) // Gera a linha do indicador MACD
        {

            ListaDaMACD = new List<double>();
            listaTempMACD = new List<double>();
            ListaDoSinalMACD = new List<double>();

            ListaDaMACD.Clear();
            ListaDoSinalMACD.Clear();
            listaTempMACD.Clear();


            MédiaMóvelExponencial MME_Lenta = new MédiaMóvelExponencial(preçoFechamento, períodoLongo);
            MédiaMóvelExponencial MME_Rápida = new MédiaMóvelExponencial(preçoFechamento, períodoCurto);

            for (int i = 0; i < períodoLongo-1; i++)
            {// Zero so primeiros elementos
                ListaDaMACD.Add(0);  
            }

            for (int i = períodoLongo-1; i < MME_Lenta.ListaDaMME.Count; i++)
            {//Calcula ListaDaMACD
                ListaDaMACD.Add(MME_Rápida.ListaDaMME[i] - MME_Lenta.ListaDaMME[i]);
            }

            for (int i = períodoLongo-1; i < ListaDaMACD.Count; i++)
            {//Pega os elementos da ListaDaMACD para calculo do sinal
                listaTempMACD.Add(ListaDaMACD[i]);
            }
           
            MédiaMóvelExponencial MME_Sinal = new MédiaMóvelExponencial(listaTempMACD, períodoSinal);
            for (int i = 0; i < períodoLongo-1; i++)
            {
                ListaDoSinalMACD.Add(0);
            }

            for (int i = 0; i < MME_Sinal.ListaDaMME.Count; i++)
            {//Calcula ListaDoSinalMACD
                ListaDoSinalMACD.Add(MME_Sinal.ListaDaMME[i]);
            }
        }
        private void GerarHistóricoMACD(List<Papeis> HistóricoPapel, int períodoCurto, int períodoLongo, int períodoSinal) // Gera a linha do indicador MACD
        {

            ListaDaMACD = new List<double>();
            listaTempMACD = new List<double>();
            ListaDoSinalMACD = new List<double>();

            ListaDaMACD.Clear();
            ListaDoSinalMACD.Clear();
            listaTempMACD.Clear();


            MédiaMóvelExponencial MME_Lenta = new MédiaMóvelExponencial(HistóricoPapel, períodoLongo);
            MédiaMóvelExponencial MME_Rápida = new MédiaMóvelExponencial(HistóricoPapel, períodoCurto);

            for (int i = 0; i < períodoLongo - 1; i++)
            {// Zero so primeiros elementos
                ListaDaMACD.Add(0);
            }

            for (int i = períodoLongo - 1; i < MME_Lenta.ListaDaMME.Count; i++)
            {//Calcula ListaDaMACD
                ListaDaMACD.Add(MME_Rápida.ListaDaMME[i] - MME_Lenta.ListaDaMME[i]);
            }

            for (int i = períodoLongo - 1; i < ListaDaMACD.Count; i++)
            {//Pega os elementos da ListaDaMACD para calculo do sinal
                listaTempMACD.Add(ListaDaMACD[i]);
            }

            MédiaMóvelExponencial MME_Sinal = new MédiaMóvelExponencial(listaTempMACD, períodoSinal);
            for (int i = 0; i < períodoLongo - 1; i++)
            {
                ListaDoSinalMACD.Add(0);
            }

            for (int i = 0; i < MME_Sinal.ListaDaMME.Count; i++)
            {//Calcula ListaDoSinalMACD
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
