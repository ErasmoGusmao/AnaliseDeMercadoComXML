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

            Estatística estatística = new Estatística();
            foreach (double valor in estatística.MédiaMóvel(fechamento,período))
            {
                ListaDaMMS.Add(valor);
            }
        }
    }
}
