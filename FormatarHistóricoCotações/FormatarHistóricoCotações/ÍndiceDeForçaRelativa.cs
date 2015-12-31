using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormatarHistóricoCotações
{
    class ÍndiceDeForçaRelativa
    {
        private List<double> variação = new List<double>();     // Lista com a variação de todos os preços
        private List<double> varPositiva = new List<double>();  // Lista com a variação positiva de todos os preços
        private List<double> varNegativa = new List<double>();  // Lista com a variação negativa de todos os preços
        private List<double> listaGan = new List<double>();     // Lista com a média de todas as variações positivas de preço do período em estudo
        private List<double> listaLoss = new List<double>();    // Lista com a média de todas as variações negativas de preço do período em estudo
        public List<double> RS {get; private set;}              // Lista com a divisão (listaGan / listaLoss )
        public List<double> IFR {get; private set;}             // Lista com IFR (Índice de Força Relativa)

        public ÍndiceDeForçaRelativa(List<double> fechamento, int período)
        {
            RS = new List<double>();
            IFR = new List<double>();

            RS.Clear();
            IFR.Clear();
            variação.Clear();
            varPositiva.Clear();
            varNegativa.Clear();
            listaGan.Clear();
            listaLoss.Clear();

            CalcularVariação(fechamento);
            CalcularRS(varPositiva, varNegativa, período);
            CalcularIFR(período);
        }
        public ÍndiceDeForçaRelativa(List<Papeis> HistóricoPapel, int período) 
        {
            RS = new List<double>();
            IFR = new List<double>();

            RS.Clear();
            IFR.Clear();
            variação.Clear();
            varPositiva.Clear();
            varNegativa.Clear();
            listaGan.Clear();
            listaLoss.Clear();

            CalcularVariação(HistóricoPapel);
            CalcularRS(varPositiva, varNegativa, período);
            CalcularIFR(período);
        }


        private void CalcularVariação(List<double> fechamento)
        {
            variação.Add(0);

            for (int i = 1; i < fechamento.Count; i++)
            {
                variação.Add(fechamento[i]-fechamento[i-1]);
            }

            //foreach (double valor in variação)
            //{
            //    if (valor > 0)
            //    {
            //        varPositiva.Add(valor);
            //        varNegativa.Add(0);
            //    } 
            //    if (valor<0)
            //    {
            //        varPositiva.Add(0);
            //        varNegativa.Add(-valor);
            //    }
            //}
            for (int i = 0; i < variação.Count; i++)
			{
                if (variação[i]>=0)
                {
                    varPositiva.Add(variação[i]);
                    varNegativa.Add(0);
                }
                else
                {
                    varPositiva.Add(0);
                    varNegativa.Add(-variação[i]);
                }
			}
        }
        private void CalcularVariação(List<Papeis> HistóricoPapel)
        {
            variação.Add(0);

            for (int i = 1; i < HistóricoPapel.Count; i++)
            {
                variação.Add(HistóricoPapel[i].PreçoFechamento - HistóricoPapel[i - 1].PreçoFechamento);
            }

            //foreach (double valor in variação)
            //{
            //    if (valor > 0)
            //    {
            //        varPositiva.Add(valor);
            //        varNegativa.Add(0);
            //    }
            //    if (valor < 0)
            //    {
            //        varPositiva.Add(0);
            //        varNegativa.Add(-valor);
            //    }
            //}
            for (int i = 0; i < variação.Count; i++)
            {
                if (variação[i] >= 0)
                {
                    varPositiva.Add(variação[i]);
                    varNegativa.Add(0);
                }
                else
                {
                    varPositiva.Add(0);
                    varNegativa.Add(-variação[i]);
                }
            }
        }

        private void CalcularRS(List<double> varPositiva, List<double> varNegativa, int período) 
        {
            MédiaMóvelSimples MMS_P = new MédiaMóvelSimples(varPositiva, período);
            foreach (double valor in MMS_P.ListaDaMMS)
            {
                listaGan.Add(valor);
            }
            MédiaMóvelSimples MMS_N = new MédiaMóvelSimples(varNegativa, período);
            foreach (double valor in MMS_N.ListaDaMMS)
            {
                listaLoss.Add(valor);
            }
            for (int i = 0; i < período-1; i++)
            {
                RS.Add(0);
            }
            for (int i = período-1; i < listaGan.Count; i++)
            {
                RS.Add(listaGan[i]/listaLoss[i]);
            }
        }
        
        private void CalcularIFR(int período)
        {
            foreach (double valor in RS)
            {
                IFR.Add(100-(100/(1+valor)));
            }
        }
    }
}
