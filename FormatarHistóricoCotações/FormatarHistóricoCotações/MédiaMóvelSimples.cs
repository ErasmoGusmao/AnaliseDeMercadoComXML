using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormatarHistóricoCotações
{
    class MédiaMóvelSimples
    {
        public List<double> ListaDaMMS { get; private set; }

        public void GerarMMS(List<double> fechamento, int período) 
        {
            ListaDaMMS = new List<double>();
            ListaDaMMS.Clear();

            for (int i = 0; i < período - 1; i++)
            {
                ListaDaMMS.Add(0); // Os primeiros elementos da lista serão zero
            }

            for (int i = 0; i < fechamento.Count; i++)
            {
                double soma = 0;
                for (int j = i; j < período; j++)
                {
                    soma += fechamento[j];
                }
                ListaDaMMS.Add(soma / ((double)período));
            }
        }
    }
}
