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
        public DateTime Data = new DateTime();
        public string CODIGO;
        public string Nome;                                
        public string ESPECI;
        public string Moeda;
        public double PreçoAbertura = new double();
        public double PreçoMáximo = new double();
        public double PreçoMínimo = new double();
        public double PreçoMédio = new double();
        public double PreçoFechamento = new double();
        public double PreçoMelhorCompra = new double();
        public double PreçoMelhorVenda = new double();
        public double TotalDeNegocios = new double();
        public double QuantidadePapeis = new double();
        public double Volume = new double();      
    }
}
