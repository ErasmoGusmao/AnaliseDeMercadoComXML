using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;

namespace FormatarHistóricoCotações
{
    public partial class Form1 : Form
    {

        string CaminhoDoDiretorio;
        List<string> codigoPapeis = new List<string>();

        bool tipoDeSaida;
        HistóricoCotação histórico; 
        
        public Form1()
        {
            InitializeComponent();
            histórico = new HistóricoCotação();
            label1.Text = "Lista contem: " + codigoPapeis.Count() + " papel.";
        }

        private void ConcatenaArquivos_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = @"C:\Users\Erasmo\Documents\GitHub\AnaliseMercado";
            openFileDialog1.Filter = "Arquivos de Texto (*.txt)|*.txt";
            openFileDialog1.FileName = "AAAA.txt";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = false;
            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                FileInfo infoArquivo = new FileInfo(Path.GetFullPath(openFileDialog1.FileName));
                CaminhoDoDiretorio = infoArquivo.DirectoryName;
            }

            if (radioButtonTXT.Checked)
            {
                tipoDeSaida = true;
            }else if (radioButtonXML.Checked)
            {
                tipoDeSaida = false;
            }

            histórico.ConcatenaArquivos(CaminhoDoDiretorio,tipoDeSaida);

            //
        }

        private void testarConsultaXML_Click(object sender, EventArgs e)
        {
            //XDocument docTeste = XDocument.Load(@"C:\Users\Erasmo\Desktop\Teste de Aplicativo Mercado Financeiro Erasmo\Histórico Anual\Para testes Rapidos\Histórico Concatenado\HistóricoConcatenado.xml");

            XDocument docTeste = XDocument.Load(@"C:\Users\Erasmo\Desktop\Teste de Aplicativo Mercado Financeiro Erasmo\Histórico Anual\Histórico Concatenado\HistóricoConcatenado.xml");

            var data = from item in docTeste.Descendants("PAPEL")
                       where item.Element("DATA").Value == "25/11/2015"     //where item.Element("DATA").Value == "12/02/2009"
                       select item.Element("CODIGO").Value; 
            foreach (var p in data)
            {
                codigoPapeis.Add(p.ToString());
                Console.WriteLine(p.ToString());
            }

            foreach (string codigo in codigoPapeis)
            {
                listBoxPapeis.Items.Add(codigo);
            }

            label1.Text = "Lista contem: " + codigoPapeis.Count() + " papeis.";
            
        }
    }
}
