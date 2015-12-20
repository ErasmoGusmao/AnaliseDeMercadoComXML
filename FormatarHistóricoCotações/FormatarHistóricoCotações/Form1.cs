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
        string codigoPapelisteBox;

        ArquivoSaídaTXT históricoSaídaTXT;
        ArquivoSaídaXML históricoSaídaXML;

        List<Papeis> hitoricoPapel;
        List<double> fechamentoPapel;

        //Testa a classe média móvel exponencial
        MédiaMóvelExponencial MME_rapida = new MédiaMóvelExponencial();
        int períodoRápido = 10;
        MédiaMóvelExponencial MME_intermediária = new MédiaMóvelExponencial();
        int períodoIntermediário = 15;
        MédiaMóvelExponencial MME_lenta = new MédiaMóvelExponencial();
        int períodoLento = 20;

        
        public Form1()
        {
            InitializeComponent();

            históricoSaídaTXT = new ArquivoSaídaTXT();
            históricoSaídaXML = new ArquivoSaídaXML();

            label1.Text = "Lista contem: " + codigoPapeis.Count() + " papel.";

            grafico1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            grafico1.Series[0].Color = Color.Brown;
            grafico1.Series[0].Name = "Frechamento";

            //Novas séries
            grafico1.Series.Add("MME_Rápida");
            grafico1.Series["MME_Rápida"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            grafico1.Series["MME_Rápida"].Color = Color.Red;

            grafico1.Series.Add("MME_Intermediária");
            grafico1.Series["MME_Intermediária"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            grafico1.Series["MME_Intermediária"].Color = Color.Black;

            //Novas séries
            grafico1.Series.Add("MME_Lenta");
            grafico1.Series["MME_Lenta"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            grafico1.Series["MME_Lenta"].Color = Color.Blue;
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
                históricoSaídaTXT.ConcatenaArquivos(CaminhoDoDiretorio);
            }
            else if (radioButtonXML.Checked) // Arquivo de saída *.xml
            {
                históricoSaídaXML.ConcatenaArquivos(CaminhoDoDiretorio);
            }
            
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
        }



        private void listBoxPapeis_SelectedIndexChanged(object sender, EventArgs e)
        {
            codigoPapelisteBox = listBoxPapeis.Items[listBoxPapeis.SelectedIndex].ToString();

            hitoricoPapel = new List<Papeis>();
            hitoricoPapel.Clear();

            //Lista com o fechamento dos papeis
            fechamentoPapel = new List<double>();
            fechamentoPapel.Clear(); //Limpo lista

            //Limpa gráfico
            this.grafico1.Series[0].Points.Clear(); // Histórico de fechamento
            
            
            //Limpar novas séries
            this.grafico1.Series["MME_Rápida"].Points.Clear();
            this.grafico1.Series["MME_Intermediária"].Points.Clear();
            this.grafico1.Series["MME_Lenta"].Points.Clear();


            //Fazer gráfico com dados do arquivo xml
            XDocument docTeste = XDocument.Load(arquivoXMLSalvo);
            ////1o passo - fasso o filtro no que quero
            var queryData = from açao in docTeste.Descendants("PAPEL")
                            where açao.Element("CODIGO").Value == codigoPapelisteBox //"EMBR3  "Coolocar 7 caracteres incluindo espaço em brando para pesquisa
                            select açao;

            ////2o passo - alimento minha classe Papeis instanciada como papel depos da consulta anterior

            foreach (var item in queryData) //Alimenta o histórico do papel filtrado para calculo.
            {
                hitoricoPapel.Add(new Papeis()
                {
                    Data = DateTime.Parse(item.Element("DATA").Value),
                    CODIGO = item.Element("CODIGO").Value,
                    Nome = item.Element("NOME").Value,
                    ESPECI = item.Element("ESPECI").Value,
                    Moeda = item.Element("MOEDA").Value,
                    PreçoAbertura = double.Parse(item.Element("P.Abr").Value),
                    PreçoMáximo = double.Parse(item.Element("P.Max").Value),
                    PreçoMínimo = double.Parse(item.Element("P.Min").Value),
                    PreçoMédio = double.Parse(item.Element("P.Med").Value),
                    PreçoFechamento = double.Parse(item.Element("P.Fech").Value),
                    PreçoMelhorCompra = double.Parse(item.Element("M_Compra").Value),
                    PreçoMelhorVenda = double.Parse(item.Element("M_Venda").Value),
                    TotalDeNegocios = double.Parse(item.Element("TOTAL_NEG.").Value),
                    QuantidadePapeis = double.Parse(item.Element("Qnt.Papeis").Value),
                    Volume = double.Parse(item.Element("VOLUME").Value)
                });

                fechamentoPapel.Add(double.Parse(item.Element("P.Fech").Value)); //Carrego a lista com o histórico do fechamento do papel
            }

            this.grafico1.Titles[0].Text = hitoricoPapel[0].CODIGO + "- " + hitoricoPapel[0].Nome;
            this.grafico1.Series[0].IsVisibleInLegend = false; //desabilita legenda
            //this.grafico1.Series[0].LegendText = "Gráfico do fechamento do papel " + hitoricoPapel[0].CODIGO; //Modifico a legenda

                      
            for (int i = 0; i < hitoricoPapel.Count(); i++) //Percorre todo o histórico do papel para fazer gráfico
            {
                this.grafico1.Series[0].Points.AddXY(hitoricoPapel[i].Data,hitoricoPapel[i].PreçoFechamento);


            //Testar a classe Média móvel expnencial
                this.grafico1.Series["MME_Rápida"].Points.AddXY(hitoricoPapel[i].Data, MME_rapida.GerarMME(fechamentoPapel, períodoRápido)[i]);
                this.grafico1.Series["MME_Intermediária"].Points.AddXY(hitoricoPapel[i].Data, MME_rapida.GerarMME(fechamentoPapel, períodoIntermediário)[i]);
                this.grafico1.Series["MME_Lenta"].Points.AddXY(hitoricoPapel[i].Data, MME_rapida.GerarMME(fechamentoPapel, períodoLento)[i]);
            }
        }
    }
}
