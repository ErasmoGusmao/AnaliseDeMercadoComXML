﻿using System;
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
using System.Windows.Forms.DataVisualization.Charting;  //Para usar Series

namespace FormatarHistóricoCotações
{
    public partial class Form1 : Form
    {

        string CaminhoDoDiretorio;
        //string arquivoXMLSalvo = @"C:\Users\Erasmo\Desktop\Teste de Aplicativo Mercado Financeiro Erasmo\Histórico Anual\Para testes Rapidos\Histórico Concatenado\HistóricoConcatenado.xml";
        //string arquivoXMLSalvo = @"C:\Users\Erasmo\Desktop\Teste de Aplicativo Mercado Financeiro Erasmo\Histórico Anual\Para testes Completos\Histórico Concatenado\HistóricoConcatenado.xml";
        string arquivoXMLSalvo = @"C:\Users\Erasmo\Desktop\Teste de Aplicativo Mercado Financeiro Erasmo\Histórico Anual\Histórico Concatenado\HistóricoConcatenado.xml";

        List<string> codigoPapeis = new List<string>();
        string codigoPapelisteBox;

        ArquivoSaídaTXT históricoSaídaTXT = new ArquivoSaídaTXT();
        ArquivoSaídaXML históricoSaídaXML = new ArquivoSaídaXML();

        List<Papeis> históricoPapel = new List<Papeis>();
        List<double> fechamentoPapel = new List<double>();
        List<double> maximaPapel = new List<double>();
        List<double> minimaPapel = new List<double>();
        List<DateTime> data = new List<DateTime>();


        public Form1()
        {
            InitializeComponent();
            label1.Text = "Lista contem: " + codigoPapeis.Count() + " papel.";

            #region "Gráfico com Candlestick"
            
            //grafico1.Series["Frechamento"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            //grafico1.Series["Frechamento"].Color = Color.Brown;

            Series Ação = new Series("Ação");
            grafico1.Series.Add(Ação);

            //Set o tipo de gráfico
            grafico1.Series["Ação"].ChartType = SeriesChartType.Candlestick;

            //Set the style of the open-close marks
            grafico1.Series["Ação"]["OpenCloseStyle"] = "Triangle";

            // Show both open and close marks
            grafico1.Series["Ação"]["ShowOpenClose"] = "Both";

            // Set point width
            grafico1.Series["Ação"]["PointWidth"] = "1.0";

            // Set colors bars
            grafico1.Series["Ação"]["PriceUpColor"] = "Green"; // <<== use text indexer for series
            grafico1.Series["Ação"]["PriceDownColor"] = "Red"; // <<== use text indexer for series

            #endregion

            #region "Inicialização do gráfico 1 Médias móveis Exponenciais e Banda de Bollinger"
            //Novas séries gráfico 1
            grafico1.Series.Add("MME_Rápida");
            grafico1.Series["MME_Rápida"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            grafico1.Series["MME_Rápida"].Color = Color.Red;
            this.grafico1.Series["MME_Rápida"].IsVisibleInLegend = false; //desabilita legenda

            grafico1.Series.Add("MME_Intermediária");
            grafico1.Series["MME_Intermediária"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            grafico1.Series["MME_Intermediária"].Color = Color.Purple;
            this.grafico1.Series["MME_Intermediária"].IsVisibleInLegend = false; //desabilita legenda

            grafico1.Series.Add("MME_Lenta");
            grafico1.Series["MME_Lenta"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            grafico1.Series["MME_Lenta"].Color = Color.Blue;
            this.grafico1.Series["MME_Lenta"].IsVisibleInLegend = false; //desabilita legenda

            //Bandas de Bollinger
            grafico1.Series.Add("MMS");
            grafico1.Series["MMS"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            grafico1.Series["MMS"].Color = Color.Black;
            this.grafico1.Series["MMS"].IsVisibleInLegend = false; //desabilita legenda

            grafico1.Series.Add("BB_Inf");
            grafico1.Series["BB_Inf"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            grafico1.Series["BB_Inf"].Color = Color.Pink;
            this.grafico1.Series["BB_Inf"].IsVisibleInLegend = false; //desabilita legenda

            grafico1.Series.Add("BB_Sup");
            grafico1.Series["BB_Sup"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            grafico1.Series["BB_Sup"].Color = Color.GreenYellow;
            this.grafico1.Series["BB_Sup"].IsVisibleInLegend = false; //desabilita legenda

            #endregion

            #region "Inicialização do gráfico2"
            //Novas séries gráfico 2
            gráfico2.Series.Add("LineMACD");
            gráfico2.Series["LineMACD"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            gráfico2.Series["LineMACD"].Color = Color.Black;
            this.gráfico2.Series["LineMACD"].IsVisibleInLegend = false; //desabilita legenda

            gráfico2.Series.Add("sinal");
            gráfico2.Series["sinal"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            gráfico2.Series["sinal"].Color = Color.Red;
            this.gráfico2.Series["sinal"].IsVisibleInLegend = false; //desabilita legenda
            #endregion

            #region "Inicialização do gráfico3"
            //Novas séries gráfico 3
            //gráfico3.Series.Add("VOLUME");
            gráfico3.Series["VOLUME"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            gráfico3.Series["VOLUME"].Color = Color.Red;
            this.gráfico3.Series["VOLUME"].IsVisibleInLegend = true; //desabilita legenda
            #endregion
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

        private void ConsultaXML_Click(object sender, EventArgs e)
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

            históricoPapel.Clear();
            fechamentoPapel.Clear();
            
            carregarDadosPapeisXML();
            
            gráficoHistóricoPapel();
            gráficoMACD();

            gráficoVolume();

            testarIndicadorADX();
            //testaEstatística();
            //testarMédiaMóvelSimples();
            //testarIFR();
        }

        private void testarIndicadorADX()
        {
            int período = 20;
            IndicadorADX indicador = new IndicadorADX(fechamentoPapel, maximaPapel, minimaPapel, período);
        }

        private void carregarDadosPapeisXML()
        {
            //Fazer gráfico com dados do arquivo xml
            XDocument docTeste = XDocument.Load(arquivoXMLSalvo);
            ////1o passo - fasso o filtro no que quero
            var queryData = from açao in docTeste.Descendants("PAPEL")
                            where açao.Element("CODIGO").Value == codigoPapelisteBox //"EMBR3  "Coolocar 7 caracteres incluindo espaço em brando para pesquisa
                            select açao;

            ////2o passo - alimento minha classe Papeis instanciada como papel depos da consulta anterior

            foreach (var item in queryData) //Alimenta o histórico do papel filtrado para calculo.
            {
                históricoPapel.Add(new Papeis()
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

                fechamentoPapel.Add(double.Parse(item.Element("P.Fech").Value));    //Carrego a lista com o histórico do fechamento do papel
                maximaPapel.Add(double.Parse(item.Element("P.Max").Value));         //Carrego a lista com o histórico dos preços de máxima do papel
                minimaPapel.Add(double.Parse(item.Element("P.Min").Value));         //Carrego a lista com o histórico dos preços de mínima do papel
                data.Add(DateTime.Parse(item.Element("DATA").Value));               //Carrego a lista com o histórico das datas do papel
            }
        }

        private List<double> testarMédiaMóvelSimples()
        {
            int período = 10;
            MédiaMóvelSimples MMS = new MédiaMóvelSimples(históricoPapel, período);
            return MMS.ListaDaMMS;
        }

        private void gráficoHistóricoPapel() 
        {
            //Limpa gráfico 1
            // Histórico de fechamento
            //this.grafico1.Series[0].Points.Clear();
            this.grafico1.Series[0].IsVisibleInLegend = false; //desabilita legenda
            
            this.grafico1.Series["Ação"].Points.Clear();
            this.grafico1.Series["Ação"].IsVisibleInLegend = false; //desabilita legenda

            // Médias móveis exponenciais
            this.grafico1.Series["MME_Rápida"].Points.Clear();
            this.grafico1.Series["MME_Intermediária"].Points.Clear();
            this.grafico1.Series["MME_Lenta"].Points.Clear();

            //Banda de Bollinger
            this.grafico1.Series["MMS"].Points.Clear();
            this.grafico1.Series["BB_Inf"].Points.Clear();
            this.grafico1.Series["BB_Sup"].Points.Clear();

            this.grafico1.Titles[0].Text = históricoPapel[0].CODIGO + "- " + históricoPapel[0].Nome;
            //this.grafico1.Series[0].LegendText = "Gráfico do fechamento do papel " + hitoricoPapel[0].CODIGO; //Modifico a legenda

            #region "Teste de Candlestick"
            grafico1.Series["Ação"].XValueType = ChartValueType.Auto;
            grafico1.Series["Ação"].YValueType = ChartValueType.Double;
            grafico1.Series["Ação"].IsXValueIndexed = true; //Datas com valores vazios são retirados
            //grafico1.ChartAreas["Ação"].Axes[1].IsStartedFromZero = false; //Eixo y = [1] não começa do zero ajustado nas propriedades

            for (int i = 0; i < históricoPapel.Count; i++)
            {
                //Adicionar data e preçoMáximo
                grafico1.Series["Ação"].Points.AddXY(históricoPapel[i].Data, históricoPapel[i].PreçoMáximo);
                //Adicionar preçoMinimo
                grafico1.Series["Ação"].Points[i].YValues[1] = históricoPapel[i].PreçoMínimo;
                //Adicionar preçoAbertura
                grafico1.Series["Ação"].Points[i].YValues[2] = históricoPapel[i].PreçoAbertura;
                //Adicionar preçoFechamento
                grafico1.Series["Ação"].Points[i].YValues[3] = históricoPapel[i].PreçoFechamento;
            }
            #endregion

            #region "Teste da classe Média Móvel Exponencial"

            //Testa a classe média móvel exponencial
            int períodoRápido = 10;
            int períodoIntermediário = 15;
            int períodoLento = 20;

            MédiaMóvelExponencial MME_rápida = new MédiaMóvelExponencial(históricoPapel, períodoRápido);
            MédiaMóvelExponencial MME_Intermediária = new MédiaMóvelExponencial(históricoPapel, períodoIntermediário);
            MédiaMóvelExponencial MME_Lenta = new MédiaMóvelExponencial(históricoPapel, períodoLento);

            grafico1.Series["MME_Rápida"].IsXValueIndexed = true;
            grafico1.Series["MME_Intermediária"].IsXValueIndexed = true;
            grafico1.Series["MME_Lenta"].IsXValueIndexed = true;

            for (int i = 0; i < períodoRápido - 1; i++)
            {
                this.grafico1.Series["MME_Rápida"].Points.AddXY(históricoPapel[i].Data, (históricoPapel[i].PreçoFechamento + históricoPapel[i].PreçoAbertura) / 2);
            }
            for (int i = períodoRápido - 1; i < históricoPapel.Count(); i++)
            {
                this.grafico1.Series["MME_Rápida"].Points.AddXY(históricoPapel[i].Data, MME_rápida.ListaDaMME[i]);
            }

            for (int i = 0; i < períodoIntermediário - 1; i++)
            {
                this.grafico1.Series["MME_Intermediária"].Points.AddXY(históricoPapel[i].Data, (históricoPapel[i].PreçoFechamento + históricoPapel[i].PreçoAbertura) / 2);
            }
            for (int i = períodoIntermediário - 1; i < históricoPapel.Count(); i++)
            {
                this.grafico1.Series["MME_Intermediária"].Points.AddXY(históricoPapel[i].Data, MME_Intermediária.ListaDaMME[i]);
            }

            for (int i = 0; i < períodoLento - 1; i++)
            {
                this.grafico1.Series["MME_Lenta"].Points.AddXY(históricoPapel[i].Data, (históricoPapel[i].PreçoFechamento + históricoPapel[i].PreçoAbertura) / 2);
            }
            for (int i = períodoLento - 1; i < históricoPapel.Count(); i++)
            {
                this.grafico1.Series["MME_Lenta"].Points.AddXY(históricoPapel[i].Data, MME_Lenta.ListaDaMME[i]);
            }
            #endregion

            #region "Teste das Bandas de Bollinger"
            int períodoBB = 20;
            double desvMédia = 2;

            grafico1.Series["MMS"].IsXValueIndexed = true;
            grafico1.Series["BB_Inf"].IsXValueIndexed = true;
            grafico1.Series["BB_Sup"].IsXValueIndexed = true;

            BandasDeBollinger bandaBollinger = new BandasDeBollinger(históricoPapel, períodoBB, desvMédia);
            for (int i = 0; i < períodoBB - 1; i++)
            {
                this.grafico1.Series["MMS"].Points.AddXY(históricoPapel[i].Data, (históricoPapel[i].PreçoFechamento + históricoPapel[i].PreçoAbertura) / 2);
                this.grafico1.Series["BB_Inf"].Points.AddXY(históricoPapel[i].Data, (históricoPapel[i].PreçoFechamento + históricoPapel[i].PreçoAbertura) / 2);
                this.grafico1.Series["BB_Sup"].Points.AddXY(históricoPapel[i].Data, (históricoPapel[i].PreçoFechamento + históricoPapel[i].PreçoAbertura) / 2);
            }
            for (int i = períodoBB-1; i < históricoPapel.Count(); i++)
            {
                    this.grafico1.Series["MMS"].Points.AddXY(históricoPapel[i].Data, bandaBollinger.MédiaDaBanda[i]);
                    this.grafico1.Series["BB_Inf"].Points.AddXY(históricoPapel[i].Data, bandaBollinger.BandaInferior[i]);
                    this.grafico1.Series["BB_Sup"].Points.AddXY(históricoPapel[i].Data, bandaBollinger.BandaSuperior[i]);
                
            }
            #endregion

        }

        private void gráficoMACD() 
        {
            //Limpa gráfico 2
            this.gráfico2.Series["MACD"].Points.Clear();
            this.gráfico2.Series["LineMACD"].Points.Clear();
            this.gráfico2.Series["sinal"].Points.Clear();

            int períodoCurtoMACD = 12;
            int períodoLongoMACD = 26;
            int períodoSinalMACD = 9;

            IndicadorMACD MACD = new IndicadorMACD(históricoPapel, períodoCurtoMACD, períodoLongoMACD, períodoSinalMACD);

            gráfico2.Series["MACD"].IsXValueIndexed = true;
            gráfico2.Series["LineMACD"].IsXValueIndexed = true;
            gráfico2.Series["sinal"].IsXValueIndexed = true;


            for (int i = 0; i < históricoPapel.Count(); i++)
            {
                this.gráfico2.Series["MACD"].Points.AddXY(históricoPapel[i].Data, MACD.HistográmaMACD[i]);
                this.gráfico2.Series["LineMACD"].Points.AddXY(históricoPapel[i].Data, MACD.ListaDaMACD[i]);
                this.gráfico2.Series["sinal"].Points.AddXY(históricoPapel[i].Data, MACD.ListaDoSinalMACD[i]);
            }
        }

        private void gráficoVolume()
        {
            //Limpa gráfico 3
            this.gráfico3.Series["VOLUME"].Points.Clear();
            this.gráfico3.Series["VOLUME"].LegendText = "R$ (Milhões)"; // +hitoricoPapel[0].CODIGO; //Modifico a legenda

            gráfico3.Series["VOLUME"].IsXValueIndexed = true;

            for (int i = 0; i < históricoPapel.Count(); i++)
            {
                this.gráfico3.Series["VOLUME"].Points.AddXY(históricoPapel[i].Data, históricoPapel[i].Volume/(1000000));
            }
        }

        private void testaEstatística() 
        {
            Estatística estatística = new Estatística();
            int períodoEstatística = 10;

            estatística.Soma(históricoPapel, períodoEstatística);
            estatística.SomaMóvel(históricoPapel, períodoEstatística);
            estatística.Média(históricoPapel, períodoEstatística);
            estatística.MédiaMóvel(históricoPapel, períodoEstatística);
            estatística.VariânciaP(históricoPapel, períodoEstatística);
            estatística.VariânciaPMóvel(históricoPapel, períodoEstatística);
            estatística.VariânciaA(históricoPapel, períodoEstatística);
            estatística.VariânciaAMóvel(históricoPapel, períodoEstatística);
            estatística.DesvPadP(históricoPapel, períodoEstatística);
            estatística.DesvPadPMóvel(históricoPapel, períodoEstatística);
            estatística.DesvPadA(históricoPapel, períodoEstatística);
            estatística.DesvPadAMóvel(históricoPapel, períodoEstatística);
        }

        private void testarIFR()
        {
            int período = 9;
            ÍndiceDeForçaRelativa ifr_test = new ÍndiceDeForçaRelativa(fechamentoPapel, período);
        }

    }
}
