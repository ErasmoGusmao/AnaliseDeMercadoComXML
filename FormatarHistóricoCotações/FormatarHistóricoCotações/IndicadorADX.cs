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
    class IndicadorADX
    {
        private List<double> deltaMáximas = new List<double>();             //Calcula a variação entre as máximas Máxima[i] - Máxima[i-1]
        private List<double> deltaMínimas = new List<double>();             //Calcula a variação entre as mínimas Mínima[i] - Mínima[i-1]
        private List<double> TR_1 = new List<double>();                     // Average True range
        private List<double> plusDM_1 = new List<double>();                 // +DM(1)
        private List<double> lessDM_1 = new List<double>();                 // -DM(1)

        private List<double> TR = new List<double>();                       // Average True range (Período)
        private List<double> PlusDM = new List<double>();                   // +DM(período)
        private List<double> LessDM = new List<double>();                   // -DM(período)

        public List<double> PlusDI { get; private set; }                    // +DI(período)
        public List<double> LessDI { get; private set; }                    // -DI(período)
        public List<double> ADX { get; private set; }                       // ADX

        private List<double> SomaDI = new List<double>();                   // SomaDI = PlusDI + LessDI
        private List<double> DifDI = new List<double>();                    // DifDI = Abs(PlusDI - LessDI)

        public List<double> DX { get; private set; }                        // DX = DifDI/SomaDI*100 será sempre maior ou igual a zero
        private List<double> tempDX = new List<double>();                   // Gera DX sem os elementos iniciais zerados para calculo do ADX

        public IndicadorADX(List<Papeis> HistóricoPapel,int período) 
        {
            CalcularVariaçãoDelta(HistóricoPapel);
            CalcularTR_1(HistóricoPapel);
            CalcularDM_1();

            GerarTR(período);
            GerarDM(período);
            GerarDI(período);

            CalculaSomaDif(período);
            GerarDX(período);

            GerarADX(período);
        }
        public IndicadorADX(List<double> fechamento, List<double> máxima, List<double> mínima, int período) 
        {
            CalcularVariaçãoDelta(máxima, mínima);
            CalcularTR_1(fechamento, máxima, mínima);
            CalcularDM_1();
            
            GerarTR(período);
            GerarDM(período);
            GerarDI(período);

            CalculaSomaDif(período);
            GerarDX(período);

            GerarADX(período);
        }


        private void CalcularVariaçãoDelta(List<double> máxima, List<double> mínima)
        {
            deltaMáximas.Clear();
            deltaMínimas.Clear();
            //deltaMáximas.Add(0);
            //deltaMínimas.Add(0);

            for (int i = 1; i < máxima.Count; i++)
            {
                deltaMáximas.Add(máxima[i] - máxima[i-1]);
                deltaMínimas.Add(mínima[i-1] - mínima[i]);
            }
        }
        private void CalcularVariaçãoDelta(List<Papeis> HistóricoPapel)
        {
            deltaMáximas.Clear();
            deltaMínimas.Clear();
            //deltaMáximas.Add(0);
            //deltaMínimas.Add(0);

            for (int i = 1; i < HistóricoPapel.Count; i++)
            {
                deltaMáximas.Add(HistóricoPapel[i].PreçoMáximo-HistóricoPapel[i-1].PreçoMáximo);
                deltaMínimas.Add(HistóricoPapel[i-1].PreçoMínimo - HistóricoPapel[i].PreçoMínimo);
            }
        }

        private void CalcularTR_1(List<double> fechamento, List<double> máxima, List<double> mínima)
        {
            TR_1.Clear();
            //TR_1.Add(0);
            for (int i = 1; i < fechamento.Count; i++)
            {
                double A;
                double B;
                double C;

                A = máxima[i] - mínima[i];
                B = Math.Abs(máxima[i] - fechamento[i - 1]);
                C = Math.Abs(mínima[i] - fechamento[i - 1]);

                TR_1.Add(Math.Max(A, Math.Max(B, C)));
            }
        }
        private void CalcularTR_1(List<Papeis> HistóricoPapel)
        {
            TR_1.Clear();
            //TR_1.Add(0);
            for (int i = 1; i < HistóricoPapel.Count; i++)
            {
                double A;
                double B;
                double C;

                A = HistóricoPapel[i].PreçoMáximo-HistóricoPapel[i].PreçoMínimo;
                B = Math.Abs(HistóricoPapel[i].PreçoMáximo-HistóricoPapel[i-1].PreçoFechamento);
                C = Math.Abs(HistóricoPapel[i].PreçoMínimo-HistóricoPapel[i-1].PreçoFechamento);

                TR_1.Add(Math.Max(A,Math.Max(B,C)));
            }
        }

        private void CalcularDM_1()
        {
            plusDM_1.Clear();
            lessDM_1.Clear();

            for (int i = 0; i < deltaMáximas.Count; i++)
            {
                if (deltaMáximas[i]<=deltaMínimas[i] || deltaMáximas[i]<=0)
                {
                    plusDM_1.Add(0);
                }
                else
                {
                    plusDM_1.Add(deltaMáximas[i]);
                }

                if (deltaMínimas[i]<=deltaMáximas[i] || deltaMínimas[i]<=0)
                {
                    lessDM_1.Add(0);
                }
                else
                {
                    lessDM_1.Add(deltaMínimas[i]);
                }
            }
        }

        private void GerarTR(int período)
        {
            TR.Clear();

            Estatística estatísticaTR = new Estatística();

            for (int i = 0; i < período; i++)
            {
                TR.Add(0);
            }
            foreach (double valor in estatísticaTR.SomaMóvel(TR_1,período))
            {
                TR.Add(valor);
            }
        }

        private void GerarDM(int período)
        {
            PlusDM.Clear();
            LessDM.Clear();

            Estatística estatísticaDM = new Estatística();

            for (int i = 0; i < período; i++)
            {
                PlusDM.Add(0);
                LessDM.Add(0);
            }
            foreach (double valor in estatísticaDM.SomaMóvel(plusDM_1,período))
            {
                PlusDM.Add(valor);
            }
            foreach (double valor in estatísticaDM.SomaMóvel(lessDM_1,período))
            {
                LessDM.Add(valor);
            }
        }

        private void GerarDI(int período)
        {
            PlusDI = new List<double>();
            LessDI = new List<double>();

            PlusDI.Clear();
            LessDI.Clear();

            for (int i = 0; i < período; i++)
            {
                PlusDI.Add(0);
                LessDI.Add(0);
            }
            for (int i = período; i < TR.Count; i++)
            {
                PlusDI.Add((PlusDM[i] / TR[i]) * 100);
                LessDI.Add((LessDM[i] / TR[i]) * 100);
            }
        }

        private void CalculaSomaDif(int período)
        {
            SomaDI.Clear();
            DifDI.Clear();

            for (int i = 0; i < período; i++)
            {
                SomaDI.Add(0);
                DifDI.Add(0);
            }
            for (int i = período; i < PlusDI.Count; i++)
            {
                SomaDI.Add(PlusDI[i] + LessDI[i]);
            }
            for (int i = período; i < PlusDI.Count; i++)
            {
                DifDI.Add(Math.Abs(PlusDI[i] - LessDI[i]));
            }
        }

        private void GerarDX(int período)
        {
            DX = new List<double>();

            DX.Clear();
            tempDX.Clear();

            for (int i = 0; i < período; i++)
            {
                DX.Add(0);
            }
            for (int i = período; i < SomaDI.Count; i++)
            {
                DX.Add((DifDI[i] / SomaDI[i]) * 100);
                tempDX.Add((DifDI[i] / SomaDI[i]) * 100);
            }
        }

        private void GerarADX(int período)
        {
            ADX = new List<double>();

            ADX.Clear();

            Estatística estatísticaDX = new Estatística();

            for (int i = 0; i < 2*período-1; i++)
            {
                ADX.Add(0);
            }
            foreach (double valor in estatísticaDX.MédiaMóvel(tempDX,período))
            {
                ADX.Add(valor);
            }
        }
    }
}
