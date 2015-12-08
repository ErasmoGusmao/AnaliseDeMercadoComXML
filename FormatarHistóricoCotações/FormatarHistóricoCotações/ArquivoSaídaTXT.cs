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
    class ArquivoSaídaTXT:TratarDadosBrutos
    {
        private void CabeçalhoArquivo(string salvarComo)
        {
            using (StreamWriter writer = new StreamWriter(salvarComo, false)) //Escreve o Cabeçalho do arquivo de saída formatado em *.txt
            {
                writer.WriteLine("DATA\tCODNEG\tNOME\tESPECI\tMOEDAF\tP.Abe\tP.Max\tP.Min\tP.Med\tP.Fech\tP.Ult\tP.OFC\tP.OFV\tTotal NEG.\tQt.Total\tVolume");
            }
        }

        private void FormatarArquivoEmTXT(string caminhoDoArquivo, string caminhoParaSalvarArqivo)
        {
          using (StreamReader reader = new StreamReader(caminhoDoArquivo))
          {
              using (StreamWriter writer = new StreamWriter(caminhoParaSalvarArqivo, true)) //true não sobrescreve no arquivo; false sobrescreve
              {
                  while (!reader.EndOfStream)
                  {
                   string LinhaDoArquivo = reader.ReadLine();

                   DateTime DataPregão = DateTime.ParseExact(LinhaDoArquivo.Substring(2, 8), "yyyymmdd", DateTimeFormatInfo.InvariantInfo);    //Data do pregão
                   DateTime DataVencimentoOpções = DateTime.ParseExact(LinhaDoArquivo.Substring(202, 8), "yyyymmdd", DateTimeFormatInfo.InvariantInfo);    //Data do vencimento de Opções

                   //Tratamento dos valores de cotações lidos
                   double PreçoAbertura = double.Parse(LinhaDoArquivo.Substring(56, 13)) / 100;
                   double PreçoMáximo = double.Parse(LinhaDoArquivo.Substring(69, 13)) / 100;
                   double PreçoMínimo = double.Parse(LinhaDoArquivo.Substring(82, 13)) / 100;
                   double PreçoMédio = double.Parse(LinhaDoArquivo.Substring(95, 13)) / 100;
                   double PreçoFechamento = double.Parse(LinhaDoArquivo.Substring(108, 13)) / 100;
                   double PreçoMelhorCompra = double.Parse(LinhaDoArquivo.Substring(121, 13)) / 100;
                   double PreçoMelhorVenda = double.Parse(LinhaDoArquivo.Substring(134, 13)) / 100;
                   double VolumeTotalNegociado = double.Parse(LinhaDoArquivo.Substring(170, 18)) / 100;
                   
                   writer.WriteLine(DataPregão.ToString("dd/mm/yyyy")  + "\t" 
                                    + LinhaDoArquivo.Substring(12, 12) + "\t"
                                    + LinhaDoArquivo.Substring(27, 12) + "\t" 
                                    + LinhaDoArquivo.Substring(39, 10) + "\t" 
                                    + LinhaDoArquivo.Substring(52, 4)  + "\t" 
                                    + PreçoAbertura.ToString() + "\t" 
                                    + PreçoMáximo.ToString() + "\t" 
                                    + PreçoMínimo.ToString() + "\t" 
                                    + PreçoMédio.ToString() + "\t" 
                                    + PreçoFechamento.ToString() + "\t" 
                                    + PreçoMelhorCompra.ToString() + "\t" 
                                    + PreçoMelhorVenda.ToString() + "\t" 
                                    + LinhaDoArquivo.Substring(147, 5) + "\t" 
                                    + LinhaDoArquivo.Substring(152, 18) + "\t" 
                                    + VolumeTotalNegociado.ToString() + "\t");
                }
             }
          }
       }

        public override void ConcatenaArquivos(string caminhoDoDiretorio) // Concatena arquivos com dados brutos de entrada e formata-os em uma única saída *.txt
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

                    #region "Formata o arquivo de saída HistóricoConcatenado.txt em *.txt"

                    {
                        string salvarComoDadosFormatadosTXT = caminhoSalvar + @"\HistóricoConcatenado.txt";

                        if (File.Exists(salvarComoDadosFormatadosTXT))
                        {
                            File.Delete(salvarComoDadosFormatadosTXT);
                        }

                        CabeçalhoArquivo(salvarComoDadosFormatadosTXT);                            //Cabeçalho do arquivo concatenado

                        FormatarArquivoEmTXT(salvarComoDadosBrutos, salvarComoDadosFormatadosTXT); //Formata o arquivo bruto concatenado do diretório "salvarComoDadosBrutos" (nome completo "C:\User\...\9999.txt") em arquivo.txt

                        if (invaliadação) //Verifica a validade do formato dos arquivos lidos => se inválidos, então deleta tudo
                        {
                            DeleteArquivo(salvarComoDadosFormatadosTXT);                            //Deletar pasta onde o arquivo concatenado seria criado
                            DeleteArquivo(salvarComoDadosBrutos);                                   //Deletar pasta onde o arquivo com dados brutos concatenado seria criado
                            Console.WriteLine("Formatação incompleta do arquivo TXT!");
                            MessageBox.Show("Formatação incompleta do arquivo TXT!", "Operação abortada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            invaliadação = false;
                        }
                        else
                        {
                            DeleteArquivo(salvarComoDadosBrutos);                                   //Deletar pasta onde o arquivo com dados brutos concatenado seria criado (Arquivo temporário)
                            MessageBox.Show("Formatação completa do arquivo TXT!!");
                            Console.WriteLine("Formatação completa do arquivo TXT!!");
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
