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
            GerarMME(fechamento, período);
        }

        public void GerarMME(List<double> fechamento, int período) //Retornará uma lista com a média móvel exponencila para elaboração do gráfico
        {
            ListaDaMME = new List<double>();
            ListaDaMME.Clear(); //Limpo a lista antes de começar

            for (int i = 0; i < período-1; i++)
            {
                ListaDaMME.Add(0); // Os primeiros elementos da lista serão zero
            }

            double soma = 0;
            for (int i = 0; i < período; i++)
            {
                soma += fechamento[i];
            }

            ListaDaMME.Add(soma / ((double)período)); //O elemento de número [perído-1] é a média dos anteriroes

            for (int i = período-1; i < fechamento.Count; i++) //Varre todos os elementos restantes do histórico que será tirado a MME
            {
                ListaDaMME.Add((ListaDaMME[i] + (((double)2 / (double)(período + 1))) * (fechamento[i] - ListaDaMME[i])));//Formula da MME
            }
        }
    }
}
