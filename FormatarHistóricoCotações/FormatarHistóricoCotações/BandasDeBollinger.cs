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
        public BandasDeBollinger(List<double> fechamento, double desvioMédia) 
        {
            GerarBandasDeBollinger(fechamento, desvioMédia);
        }
        public BandasDeBollinger(List<Papeis> HistóricoPapel, double desvioMédia)
        {
            GerarBandasDeBollinger(HistóricoPapel, desvioMédia);
        }

        private void GerarBandasDeBollinger(List<double> fechamento, double desvioMédia)
        {
            throw new NotImplementedException();
        }
    }
}
