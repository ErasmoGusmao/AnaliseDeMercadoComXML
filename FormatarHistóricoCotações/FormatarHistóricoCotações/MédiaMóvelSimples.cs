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

        public MédiaMóvelSimples(List<double> fechamento, int período) 
        {
            ListaDaMMS = new List<double>();
            ListaDaMMS.Clear();

            GerarMMS(fechamento, período);
        }
        public MédiaMóvelSimples(List<Papeis> HistóricoPapel, int período)
        {
            ListaDaMMS = new List<double>();
            ListaDaMMS.Clear();

            GerarMMS(HistóricoPapel, período);
        }

        private void GerarMMS(List<double> fechamento, int período) 
        {
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
        private void GerarMMS(List<Papeis> HistóricoPapel, int período)
        {
            ListaDaMMS = new List<double>();
            ListaDaMMS.Clear();

            for (int i = 0; i < período - 1; i++)
            {
                ListaDaMMS.Add(0); // Os primeiros elementos da lista serão zero
            }

            Estatística estatística = new Estatística();
            foreach (double valor in estatística.MédiaMóvel(HistóricoPapel, período))
            {
                ListaDaMMS.Add(valor);
            }
        }
    }
}
