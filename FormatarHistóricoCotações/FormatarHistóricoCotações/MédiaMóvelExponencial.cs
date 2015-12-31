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
    class MédiaMóvelExponencial
    {
        public List<double> ListaDaMME { get; private set; }

        public MédiaMóvelExponencial(List<double> fechamento, int período) 
        {
            ListaDaMME = new List<double>();
            ListaDaMME.Clear(); //Limpo a lista antes de começar

            GerarMME(fechamento, período);
        }
        public MédiaMóvelExponencial(List<Papeis> HistóricoPapel, int período)
        {
            ListaDaMME = new List<double>();
            ListaDaMME.Clear(); //Limpo a lista antes de começar

            GerarMME(HistóricoPapel, período);
        }

        private void GerarMME(List<double> fechamento, int período) 
        {//Retornará uma lista com a média móvel exponencila para elaboração do gráfico
            for (int i = 0; i < período-1; i++)
            {
                ListaDaMME.Add(0); // Os primeiros elementos da lista serão zero
            }

            Estatística estatística = new Estatística();
            ListaDaMME.Add(estatística.Média(fechamento, período)); //O elemento de número [perído-1] é a média dos anteriroes

            for (int i = período-1; i < fechamento.Count; i++) //Varre todos os elementos restantes do histórico que será tirado a MME
            {
                ListaDaMME.Add((ListaDaMME[i] + (((double)2 / (double)(período + 1))) * (fechamento[i] - ListaDaMME[i])));//Formula da MME
            }
        }
        private void GerarMME(List<Papeis> HistóricoPapel, int período)
        {//Retornará uma lista com a média móvel exponencila para elaboração do gráfico
            for (int i = 0; i < período - 1; i++)
            {
                ListaDaMME.Add(0); // Os primeiros elementos da lista serão zero
            }

            Estatística estatística = new Estatística();
            ListaDaMME.Add(estatística.Média(HistóricoPapel, período)); //O elemento de número [perído-1] é a média dos anteriroes

            for (int i = período - 1; i < HistóricoPapel.Count; i++) //Varre todos os elementos restantes do histórico que será tirado a MME
            {
                ListaDaMME.Add((ListaDaMME[i] + (((double)2 / (double)(período + 1))) * (HistóricoPapel[i].PreçoFechamento - ListaDaMME[i])));//Formula da MME
            }
        }
    }
}
