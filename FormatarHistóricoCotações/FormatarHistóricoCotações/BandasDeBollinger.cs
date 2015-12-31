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
    class BandasDeBollinger
    {
        public List<double> MédiaDaBanda { get; private set; }
        public List<double> BandaSuperior { get; private set; }
        public List<double> BandaInferior { get; private set; }

        public BandasDeBollinger(List<double> fechamento, int período, double desvioMédia) 
        {//Construtor
            MédiaDaBanda = new List<double>();
            MédiaDaBanda.Clear();
            BandaSuperior = new List<double>();
            BandaSuperior.Clear();
            BandaInferior = new List<double>();
            BandaInferior.Clear();

            GerarBandasDeBollinger(fechamento,período, desvioMédia);
        }
        public BandasDeBollinger(List<Papeis> HistóricoPapel, int período, double desvioMédia)
        {//Construtor sobrecarregado
            MédiaDaBanda = new List<double>();
            MédiaDaBanda.Clear();
            BandaSuperior = new List<double>();
            BandaSuperior.Clear();
            BandaInferior = new List<double>();
            BandaInferior.Clear();

            GerarBandasDeBollinger(HistóricoPapel,período, desvioMédia);
        }

        private void GerarBandasDeBollinger(List<double> fechamento, int período, double desvioMédia)
        {
            MédiaMóvelSimples médiaMS = new MédiaMóvelSimples(fechamento, período);
            Estatística estatística = new Estatística();

            List<double> semibanda = new List<double>();
            for (int i = 0; i < período - 1; i++)
            {
                semibanda.Add(0);
            }
            foreach (double valor in estatística.DesvPadAMóvel(fechamento, período))
            {
                semibanda.Add(valor * desvioMédia); //Para ser somado e subitraido da MédiaDaBanda
            }

            foreach (double valor in médiaMS.ListaDaMMS)
            {
                MédiaDaBanda.Add(valor);
            }

            for (int i = 0; i < MédiaDaBanda.Count; i++)
            {
                BandaSuperior.Add(MédiaDaBanda[i] + semibanda[i]);
                BandaInferior.Add(MédiaDaBanda[i] - semibanda[i]);
            }
        }
        private void GerarBandasDeBollinger(List<Papeis> HistóricoPapel, int período, double desvioMédia)
        {
            MédiaMóvelSimples médiaMS = new MédiaMóvelSimples(HistóricoPapel, período);
            Estatística estatística = new Estatística();

            List<double> semibanda = new List<double>();
            for (int i = 0; i < período-1; i++)
            {
                semibanda.Add(0);
            }

            foreach (double valor in estatística.DesvPadAMóvel(HistóricoPapel, período))
            {
                semibanda.Add(valor * desvioMédia); //Para ser somado e subitraido da MédiaDaBanda
            }

            foreach (double valor in médiaMS.ListaDaMMS)
            {
                MédiaDaBanda.Add(valor);
            }

            for (int i = 0; i < MédiaDaBanda.Count; i++)
            {
                BandaSuperior.Add(MédiaDaBanda[i] + semibanda[i]);
                BandaInferior.Add(MédiaDaBanda[i] - semibanda[i]);
            }
        }
    }
}
