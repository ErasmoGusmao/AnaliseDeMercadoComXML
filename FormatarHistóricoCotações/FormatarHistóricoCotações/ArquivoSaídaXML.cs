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
    class ArquivoSaídaXML:TratarDadosBrutos
    {
        private void FormatarArquivoEmXML(string caminhoDoArquivo, string caminhoParaSalvarArquivo) //Formata o arquivo de dados brutos tratado 9999.txt em um arquivo *.xml
        {
            using (StreamReader reader = new StreamReader(caminhoDoArquivo))
            {
                using (XmlTextWriter writerXml = new XmlTextWriter(caminhoParaSalvarArquivo, null)) //Salva o arquivo Xml no caminho informado
                {

                    writerXml.WriteStartDocument();                                                 //Inicia o documento XML
                    writerXml.Formatting = Formatting.Indented;                                     //Usa a formatação
                    writerXml.WriteStartElement("PAPEIS");                                          //Escreve o elemento Raiz

                    while (!reader.EndOfStream)                                                     //Converte arquivo 9999.txt até o final
                    {
                        string LinhaDoArquivo = reader.ReadLine();                                  //Ler linha

                        //Tratamento dos das datas
                        DateTime DataPregão = DateTime.ParseExact(LinhaDoArquivo.Substring(2, 8), "yyyymmdd", DateTimeFormatInfo.InvariantInfo);

                        //Tratamento dos valores de cotações lidos
                        double PreçoAbertura = double.Parse(LinhaDoArquivo.Substring(56, 13)) / 100;
                        double PreçoMáximo = double.Parse(LinhaDoArquivo.Substring(69, 13)) / 100;
                        double PreçoMínimo = double.Parse(LinhaDoArquivo.Substring(82, 13)) / 100;
                        double PreçoMédio = double.Parse(LinhaDoArquivo.Substring(95, 13)) / 100;
                        double PreçoAnterior = double.Parse(LinhaDoArquivo.Substring(108, 13)) / 100;
                        double PreçoMelhorCompra = double.Parse(LinhaDoArquivo.Substring(121, 13)) / 100;
                        double PreçoMelhorVenda = double.Parse(LinhaDoArquivo.Substring(134, 13)) / 100;
                        double TotalDeNegocios = double.Parse(LinhaDoArquivo.Substring(147, 5));
                        double QuantidadePapeis = double.Parse(LinhaDoArquivo.Substring(152, 18));
                        double Volume = double.Parse(LinhaDoArquivo.Substring(170, 18)) / 100;

                        //Escreve o elemento Raiz
                        writerXml.WriteStartElement("PAPEL");

                        //Escreve os sub-elementos
                        writerXml.WriteElementString("DATA", DataPregão.ToString("dd/mm/yyyy"));
                        writerXml.WriteElementString("CODIGO", LinhaDoArquivo.Substring(12, 12));
                        writerXml.WriteElementString("NOME", LinhaDoArquivo.Substring(27, 12));
                        writerXml.WriteElementString("ESPECI", LinhaDoArquivo.Substring(39, 10));
                        writerXml.WriteElementString("MOEDA", LinhaDoArquivo.Substring(52, 4));
                        writerXml.WriteElementString("P.Abr", PreçoAbertura.ToString());
                        writerXml.WriteElementString("P.Max", PreçoMáximo.ToString());
                        writerXml.WriteElementString("P.Min", PreçoMínimo.ToString());
                        writerXml.WriteElementString("P.Med", PreçoMédio.ToString());
                        writerXml.WriteElementString("P.Anterior", PreçoAnterior.ToString());
                        writerXml.WriteElementString("M_Compra", PreçoMelhorCompra.ToString());
                        writerXml.WriteElementString("M_Venda", PreçoMelhorVenda.ToString());
                        writerXml.WriteElementString("TOTAL_NEG.", TotalDeNegocios.ToString());
                        writerXml.WriteElementString("Qnt.Papeis", QuantidadePapeis.ToString());
                        writerXml.WriteElementString("VOLUME", Volume.ToString());

                        //Encerra os elementos itens
                        writerXml.WriteEndElement();
                    }
                }
            }
        }

        public override void  ConcatenaArquivos(string caminhoDoDiretorio) // Concatena arquivos com dados brutos de entrada e formata-os em uma única saída *.xml
        {
            try
            {
                string[] arquivos = Directory.GetFiles(caminhoDoDiretorio, "*.txt", SearchOption.TopDirectoryOnly);         //Captura o nome completo "C:\User\...\AAAA.txt" de todos arquivos *.txt do diretório

                if (VerNomeArquivo(caminhoDoDiretorio))                                                                     //Método que verifiar se os arquivos dos diretórios informados são todos válidos retorna um bool true ou false
                {
                    string caminhoSalvar = caminhoDoDiretorio + @"\Histórico Concatenado";
                    string salvarComoDadosBrutos = caminhoSalvar + @"\9999.txt";                                            //Salva um arquivo de dados brutos temporário com um formato válido

                    #region "Bloco que verifica a existência ou não do diretório onde será salvo o arquivo concatenado 'caminhoSalva'."
                    if (!Directory.Exists(caminhoSalvar))
                    {
                        Directory.CreateDirectory(caminhoSalvar);
                    }

                    if (File.Exists(salvarComoDadosBrutos))
                    {
                        File.Delete(salvarComoDadosBrutos);
                    }
                    #endregion
                    
                    #region "Bloco que unifica os dados brutos em um unico arquivo temporário 9999.txt."
                    foreach (string arq in arquivos)
                    {
                        Console.WriteLine(arq);
                        UnificaDadosBrutosTXT(arq, salvarComoDadosBrutos);
                    }
                    #endregion

                    #region "Formata o arquivo de saída HistóricoConcatenado.xml em *.xml"

                    {
                        string salvarComoDadosFormatadosXML = caminhoSalvar + @"\HistóricoConcatenado.xml";

                        if (File.Exists(salvarComoDadosFormatadosXML))
                        {
                            File.Delete(salvarComoDadosFormatadosXML);
                        }

                        FormatarArquivoEmXML(salvarComoDadosBrutos, salvarComoDadosFormatadosXML); //Formata o arquivo bruto concatenado do diretório "salvarComoDadosBrutos" (nome completo "C:\User\...\9999.txt") em arquivo.xml

                        if (invaliadação) //Verifica a validade do formato dos arquivos lidos => se inválidos, então deleta tudo
                        {
                            DeleteArquivo(salvarComoDadosFormatadosXML);                            //Deletar pasta onde o arquivo concatenado seria criado
                            DeleteArquivo(salvarComoDadosBrutos);                                   //Deletar pasta onde o arquivo com dados brutos concatenado seria criado
                            Console.WriteLine("Formatação incompleta do arquivo XML!");
                            MessageBox.Show("Formatação incompleta do arquivo XML!", "Operação abortada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            invaliadação = false;
                        }
                        else
                        {
                            DeleteArquivo(salvarComoDadosBrutos);                                   //Deletar pasta onde o arquivo com dados brutos concatenado seria criado (Arquivo temporário)
                            MessageBox.Show("Formatação completa do arquivo XML!!");
                            Console.WriteLine("Formatação completa do arquivo XML!!");
                        }
                    }
                    #endregion
                }
                else
                {
                    MessageBox.Show("Concatenação não concluida!!", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine("ERRO NO FORMADO DO ARQUIVO!");
                }
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Diretório com arquivos não selecionado!", "AVISO!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
