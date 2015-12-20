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
        private List<double> ListaDaMME;

        public List<double> GerarMME(List<double> fechamento, int período) //Retornará uma lista com a média móvel exponencila para elaboração do gráfico
        {
            ListaDaMME = new List<double>();
            ListaDaMME.Clear(); //Limpo a lista antes de começar

            ListaDaMME.Add(0); // O primeiro elemento da lista é zero -> para usar na formula de recorrência

            for (int i = 0; i < fechamento.Count; i++) //Varre todo o histórico
            {
                ListaDaMME.Add((ListaDaMME[i] + (((double)2 / (double)(período + 1))) * (fechamento[i] - ListaDaMME[i])));
            }
            return ListaDaMME;
        }
    }
}
