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
    class Papeis
    {
        //public List<DateTime> Data = new List<DateTime>();
        //public List<double> PreçoAbertura = new List<double>();
        //public List<double> PreçoMáximo = new List<double>();
        //public List<double> PreçoMínimo = new List<double>();
        //public List<double> PreçoMédio = new List<double>();
        //public List<double> PreçoAnterior = new List<double>();
        //public List<double> PreçoMelhorCompra = new List<double>();
        //public List<double> PreçoMelhorVenda = new List<double>();
        //public List<double> TotalDeNegocios = new List<double>();
        //public List<double> QuantidadePapeis = new List<double>();
        //public List<double> Volume = new List<double>();

        public DateTime Data = new DateTime();
        public string CODIGO;
        public string Nome;                                
        public string ESPECI;
        public string Moeda;
        public double PreçoAbertura = new double();
        public double PreçoMáximo = new double();
        public double PreçoMínimo = new double();
        public double PreçoMédio = new double();
        public double PreçoAnterior = new double();
        public double PreçoMelhorCompra = new double();
        public double PreçoMelhorVenda = new double();
        public double TotalDeNegocios = new double();
        public double QuantidadePapeis = new double();
        public double Volume = new double();
       
    }
}
