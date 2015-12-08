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
using System.Xml;
using System.Xml.Linq;

namespace FormatarHistóricoCotações
{
    public partial class Form1 : Form
    {

        string CaminhoDoDiretorio;
        //string arquivoXMLSalvo = @"C:\Users\Erasmo\Desktop\Teste de Aplicativo Mercado Financeiro Erasmo\Histórico Anual\Para testes Rapidos\Histórico Concatenado\HistóricoConcatenado.xml";
        string arquivoXMLSalvo = @"C:\Users\Erasmo\Desktop\Teste de Aplicativo Mercado Financeiro Erasmo\Histórico Anual\Histórico Concatenado\HistóricoConcatenado.xml";

        List<string> codigoPapeis = new List<string>();

        //bool tipoDeSaida;

        //Substituição das classes HistóricoCotação por TratarDadosBrutos
        //HistóricoCotação histórico;
        ArquivoSaídaTXT históricoSaídaTXT;
        ArquivoSaídaXML históricoSaídaXML;

        List<Papeis> hitoricoPapel;
        
        public Form1()
        {
            InitializeComponent();
            //Substituição das classes HistóricoCotação por TratarDadosBrutos
            //histórico = new HistóricoCotação();
            históricoSaídaTXT = new ArquivoSaídaTXT();
            históricoSaídaXML = new ArquivoSaídaXML();

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

            if (radioButtonTXT.Checked) // Arquivo de saída *.txt
            {
                //Substituição das classes HistóricoCotação por TratarDadosBrutos
                //tipoDeSaida = true;
                históricoSaídaTXT.ConcatenaArquivos(CaminhoDoDiretorio);
            }
            else if (radioButtonXML.Checked) // Arquivo de saída *.xml
            {
                //Substituição das classes HistóricoCotação por TratarDadosBrutos
                //tipoDeSaida = false;
                históricoSaídaXML.ConcatenaArquivos(CaminhoDoDiretorio);
            }
            
            //Substituição das classes HistóricoCotação por TratarDadosBrutos
            //histórico.ConcatenaArquivos(CaminhoDoDiretorio,tipoDeSaida);
        }

        private void testarConsultaXML_Click(object sender, EventArgs e)
        {
            XDocument docTeste = XDocument.Load(arquivoXMLSalvo);

            var data = from item in docTeste.Descendants("PAPEL")
                       where (item.Element("DATA").Value == "25/11/2015" && double.Parse(item.Element("TOTAL_NEG.").Value) > 2000)     //Pega todos os papeis da da ultima data que tenham Total de negócios maior que 2000
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
            
            //Para alimentar o GridView
            DataSet dataSete = new DataSet();
            dataSete.ReadXml(arquivoXMLSalvo);
            dvgXML.DataSource = dataSete.Tables[0].DefaultView;

            //Testando uso da Classe Papeis e testando calculo
            //Pegando somento o papel EMBR3 e alimentando a classe papeis do docTeste já definido

            //1o passo - fasso o filtro no que quero
            var queryData = from açao in docTeste.Descendants("PAPEL")
                            where açao.Element("CODIGO").Value == "EMBR3"
                            select açao;
            //2o passo - alimento minha classe Papeis instanciada como papel depos da consulta anterior

            hitoricoPapel = new List<Papeis>();
            foreach (var item in queryData) //Alimenta o histórico do papel filtrado para calculo.
            {
                hitoricoPapel.Add(new Papeis(){ Data=DateTime.Parse(item.Element("DATA").Value),
                                                CODIGO = item.Element("CODIGO").Value,
                                                Nome = item.Element("NOME").Value,
                                                ESPECI = item.Element("ESPECI").Value,
                                                Moeda = item.Element("MOEDA").Value,
                                                PreçoAbertura = double.Parse(item.Element("P.Abr").Value),
                                                PreçoMáximo = double.Parse(item.Element("P.Max").Value),
                                                PreçoMínimo = double.Parse(item.Element("P.Min").Value),
                                                PreçoMédio = double.Parse(item.Element("P.Med").Value),
                                                PreçoAnterior = double.Parse(item.Element("P.Anterior").Value),
                                                PreçoMelhorCompra = double.Parse(item.Element("M_Compra").Value),
                                                PreçoMelhorVenda = double.Parse(item.Element("M_Venda").Value),
                                                TotalDeNegocios = double.Parse(item.Element("TOTAL_NEG.").Value),
                                                QuantidadePapeis = double.Parse(item.Element("Qnt.Papeis").Value),
                                                Volume = double.Parse(item.Element("VOLUME").Value)});
            }
            
        }

        private void bTesteCalculo_Click(object sender, EventArgs e)
        {

        }
    }
}
