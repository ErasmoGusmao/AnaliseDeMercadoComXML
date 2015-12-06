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
    class HistóricoCotação
    {
        private bool invaliadação = false; // verificador deve está somente no método de UnificaçãoDadosBrutos
        private List<string> especiPapel = new List<string>();

        private void IniciaListaESPECI() //Filtra os dados brutos tratados somente para determinadas especifiações de papel
        {
            especiPapel.Add("ON ");
            especiPapel.Add("PN ");
            especiPapel.Add("PNA");
            especiPapel.Add("PNB");
            especiPapel.Add("PNC");
            especiPapel.Add("PND");
            especiPapel.Add("PNE");
            especiPapel.Add("PNF");
            especiPapel.Add("PNG");
            especiPapel.Add("PNH");
            especiPapel.Add("PNV");
            especiPapel.Add("OR ");
            especiPapel.Add("PRA");
            especiPapel.Add("PRB");
            especiPapel.Add("PRC");
            especiPapel.Add("PRD");
            especiPapel.Add("PRE");
            especiPapel.Add("PRF");
            especiPapel.Add("PRG");
            especiPapel.Add("PRH");
            especiPapel.Add("PNR");
            especiPapel.Add("PRV");
            especiPapel.Add("PR ");
        }

        public void ConcatenaArquivos(string caminhoDoDiretorio, bool tipoDeSaida) 
        {
            try
            {
                string[] arquivos = Directory.GetFiles(caminhoDoDiretorio, "*.txt", SearchOption.TopDirectoryOnly);         //Captura o nome completo "C:\User\...\AAAA.txt" de todos arquivos *.txt do diretório

                if (VerNomeArquivo(caminhoDoDiretorio))                                                                     //Método que verifiar se os arquivos dos diretórios informados são todos válidos retorna um bool true ou false
                {
                    string caminhoSalvar = caminhoDoDiretorio + @"\Histórico Concatenado";
                    string salvarComoDadosBrutos = caminhoSalvar + @"\9999.txt"; //Salva um arquivo de dados brutos temporário com um formato válido

                    //Bloco que verifica a existência ou não do diretório onde será salvo o arquivo concatenado "caminhoSalva"
                    if (!Directory.Exists(caminhoSalvar))
                    {
                        Directory.CreateDirectory(caminhoSalvar);
                    } 
                    
                    if (File.Exists(salvarComoDadosBrutos))
                    {
                        File.Delete(salvarComoDadosBrutos);
                    }
                    
                    //Bloco que unifica os dados brutos em um unico arquivo temporário 9999.txt           
                    foreach (string arq in arquivos)
                    {
                        Console.WriteLine(arq);
                        UnificaDadosBrutosTXT(arq, salvarComoDadosBrutos);
                    }

                    if (tipoDeSaida) //Se verdade -> saída no formato *.txt
                    {
                    string salvarComoDadosFormatadosTXT = caminhoSalvar + @"\HistóricoConcatenado.txt";
                    
                        if (File.Exists(salvarComoDadosFormatadosTXT))
                        {
                            File.Delete(salvarComoDadosFormatadosTXT);
                        }                 
                        //Cabeçalho do arquivo concatenado
                        CabeçalhoArquivo(salvarComoDadosFormatadosTXT);

                        //Bloco que formata o arquivo bruto concatenado do diretório "salvarComoDadosBrutos" (nome completo "C:\User\...\9999.txt") em arquivo.txt
                        FormatarArquivoTXT(salvarComoDadosBrutos, salvarComoDadosFormatadosTXT);

                        //Bloco que verifica a validade do formato dos arquivos lidos
                        if (invaliadação)
                        {
                            DeleteArquivo(salvarComoDadosFormatadosTXT); //Deletar pasta onde o arquivo concatenado seria criado
                            DeleteArquivo(salvarComoDadosBrutos);        //Deletar pasta onde o arquivo com dados brutos concatenado seria criado
                            Console.WriteLine("Concatenação incompleta!");
                            MessageBox.Show("Concatenação incompleta!", "Operação abortada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            invaliadação = false;
                        }
                        else
                        {
                            DeleteArquivo(salvarComoDadosBrutos);        //Deletar pasta onde o arquivo com dados brutos concatenado seria criado (Arquivo temporário)
                            MessageBox.Show("Concatenação completa!!");
                            Console.WriteLine("Concatenação completa!!");
                        }
                    }
                    else //Se falso -> saída no formato *.xml
                    {
                        string salvarComoDadosFormatadosXML = caminhoSalvar + @"\HistóricoConcatenado.xml";

                    if (File.Exists(salvarComoDadosFormatadosXML))
                    {
                        File.Delete(salvarComoDadosFormatadosXML);
                    }

                        //Bloco que formata o arquivo bruto concatenado do diretório "salvarComoDadosBrutos" (nome completo "C:\User\...\9999.txt") em arquivo.xml
                        FormatarArquivoXML(salvarComoDadosBrutos,salvarComoDadosFormatadosXML);

                        //Bloco que verifica a validade do formato dos arquivos lidos
                        if (invaliadação)
                        {
                            DeleteArquivo(salvarComoDadosFormatadosXML); //Deletar pasta onde o arquivo concatenado seria criado
                            DeleteArquivo(salvarComoDadosBrutos);        //Deletar pasta onde o arquivo com dados brutos concatenado seria criado
                            Console.WriteLine("Concatenação incompleta!");
                            MessageBox.Show("Concatenação incompleta!", "Operação abortada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            invaliadação = false;
                        }
                        else
                        {
                            DeleteArquivo(salvarComoDadosBrutos);        //Deletar pasta onde o arquivo com dados brutos concatenado seria criado (Arquivo temporário)
                            MessageBox.Show("Concatenação completa!!");
                            Console.WriteLine("Concatenação completa!!");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Concatenação não concluida!!", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine("ERRO NO FORMADO DO ARQUIVO!");
                }
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Diretório com arquivos não selecionado!","AVISO!",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
        }

        private void UnificaDadosBrutosTXT(string caminhoDosDadosBrutos, string salvarComoDadosBrutos)
        {
            try
            {
                try
                {
                    using (StreamReader reader = new StreamReader(caminhoDosDadosBrutos))
                    {
                        using (StreamWriter writer = new StreamWriter(salvarComoDadosBrutos, true)) //true não sobrescreve no arquivo; false sobrescreve
                        {
                            try
                            {
                                IniciaListaESPECI();

                                while (!reader.EndOfStream)
                                {
                                    string LinhaDoArquivo = reader.ReadLine();
                                    if (LinhaDoArquivo.Substring(0, 2) =="01")
                                    {
                                            if (LinhaDoArquivo.Substring(24, 3).ToString()=="010") //Se o tipo de mercado for Mercado Vista ele copia linha caso contrário pula
                                            {
                                                if (especiPapel.Contains(LinhaDoArquivo.Substring(39, 3).ToString())) //Verifica ESPECIFICAÇÃO DO PAPEL está contida na lista
                                                {
                                                    
                                                //Faz a conversão dos valores de data e preço caso o formato do arquivo esteja errado para um desses valores
                                                //irá gerar uma exceção que será tratada

                                                //Tratamento dos valores de datas lidos
                                                DateTime DataPregão = DateTime.ParseExact(LinhaDoArquivo.Substring(2, 8), "yyyymmdd", DateTimeFormatInfo.InvariantInfo);    //Data do pregão
                                                DateTime DataVencimentoOpções = DateTime.ParseExact(LinhaDoArquivo.Substring(202, 8), "yyyymmdd", DateTimeFormatInfo.InvariantInfo);    //Data do vencimento de Opções

                                                //Tratamento dos valores de cotações lidos
                                                decimal PreçoAbertura = decimal.Parse(LinhaDoArquivo.Substring(56, 13)) / 100;
                                                decimal PreçoMáximo = decimal.Parse(LinhaDoArquivo.Substring(69, 13)) / 100;
                                                decimal PreçoMínimo = decimal.Parse(LinhaDoArquivo.Substring(82, 13)) / 100;
                                                decimal PreçoMédio = decimal.Parse(LinhaDoArquivo.Substring(95, 13)) / 100;
                                                decimal PreçoAnterior = decimal.Parse(LinhaDoArquivo.Substring(108, 13)) / 100;
                                                decimal PreçoMelhorCompra = decimal.Parse(LinhaDoArquivo.Substring(121, 13)) / 100;
                                                decimal PreçoMelhorVenda = decimal.Parse(LinhaDoArquivo.Substring(134, 13)) / 100;
                                                decimal VolumeTotalNegociado = decimal.Parse(LinhaDoArquivo.Substring(170, 18)) / 100;
                                                decimal PreçoExercício = decimal.Parse(LinhaDoArquivo.Substring(188, 13)) / 100;

                                                writer.WriteLine(LinhaDoArquivo); //Copia a linha do arquivo
                                                }
                                            }
                                    }
                                }

                                especiPapel.Clear(); // Limpa a lista
                            }
                            catch (FormatException)//Verifica a validação do arquivo
                            {
                                FileInfo infoFile = new FileInfo(Path.GetFullPath(caminhoDosDadosBrutos));
                                MessageBox.Show("Incapaz de ler arquivo " + infoFile.Name + ", pois está formatado errado!", "Sua execução foi invalidada!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                invaliadação = true; //Deleta pasta do arquivo concatenado
                            }
                        }
                    }
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show("ARQUIVO NÃO ENCONTRADO!", "Sua execução foi invalidada!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    invaliadação = true; //Deleta pasta do arquivo concatenado
                }
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Caminho do arquivo histórico não informado!", "Sua execução foi invalidada!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }           
        
        }

        private void FormatarArquivoTXT(string caminhoDoArquivo, string caminhoParaSalvarArqivo)
        {
            try
            {
                try
                {
                    using (StreamReader reader = new StreamReader(caminhoDoArquivo))
                    {
                        using (StreamWriter writer = new StreamWriter(caminhoParaSalvarArqivo, true)) //true não sobrescreve no arquivo; false sobrescreve
                        {

                            try
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
                                            double PreçoAnterior = double.Parse(LinhaDoArquivo.Substring(108, 13)) / 100;
                                            double PreçoMelhorCompra = double.Parse(LinhaDoArquivo.Substring(121, 13)) / 100;
                                            double PreçoMelhorVenda = double.Parse(LinhaDoArquivo.Substring(134, 13)) / 100;
                                            double VolumeTotalNegociado = double.Parse(LinhaDoArquivo.Substring(170, 18)) / 100;
                                            //double PreçoExercício = double.Parse(LinhaDoArquivo.Substring(188, 13)) / 100;

                                            writer.WriteLine(/*LinhaDoArquivo.Substring(0, 2) + "\t" + */DataPregão.ToString("dd/mm/yyyy") + "\t" +/*+ LinhaDoArquivo.Substring(10, 2)
                                        + "\t" + */LinhaDoArquivo.Substring(12, 12) + "\t" + /*LinhaDoArquivo.Substring(24, 3) + "\t" + */LinhaDoArquivo.Substring(27, 12)
                                        + "\t" + LinhaDoArquivo.Substring(39, 10) + "\t" + /*LinhaDoArquivo.Substring(49, 3) + "\t" + */LinhaDoArquivo.Substring(52, 4)
                                        + "\t" + PreçoAbertura.ToString() + "\t" + PreçoMáximo.ToString() + "\t" + PreçoMínimo.ToString()
                                        + "\t" + PreçoMédio.ToString() + "\t" + PreçoAnterior.ToString() + "\t" + PreçoMelhorCompra.ToString()
                                        + "\t" + PreçoMelhorVenda.ToString() + "\t" + LinhaDoArquivo.Substring(147, 5) + "\t" + LinhaDoArquivo.Substring(152, 18)
                                        + "\t" + VolumeTotalNegociado.ToString() + "\t" /*+ PreçoExercício.ToString() + "\t" + LinhaDoArquivo.Substring(201, 1)
                                        + "\t" + DataVencimentoOpções.ToString("dd/mm/yyyy") + "\t" + LinhaDoArquivo.Substring(210, 7) + "\t" + LinhaDoArquivo.Substring(217, 7)
                                        + "\t" + LinhaDoArquivo.Substring(230, 12) + "\t" + LinhaDoArquivo.Substring(242, 3)*/);
                                }
                            }
                            catch (FormatException)//Verifica a validação do arquivo
                            {
                                FileInfo infoFile = new FileInfo(Path.GetFullPath(caminhoDoArquivo));
                                MessageBox.Show("Incapaz de ler arquivo " + infoFile.Name + ", pois está formatado errado!", "Sua execução foi invalidada!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                invaliadação = true; //Deleta pasta do arquivo concatenado
                            }
                        }
                    }
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show("ARQUIVO NÃO ENCONTRADO!", "Sua execução foi invalidada!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    invaliadação = true; //Deleta pasta do arquivo concatenado
                }
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Caminho do arquivo histórico não informado!", "Sua execução foi invalidada!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }           
        }

        private void FormatarArquivoXML(string caminhoDoArquivo, string caminhoParaSalvarArquivo) 
        {
            try
            {
                try
                {
                    using (StreamReader reader = new StreamReader(caminhoDoArquivo))
                    {
                            try
                            {
                                using (XmlTextWriter writerXml = new XmlTextWriter(caminhoParaSalvarArquivo, null)) //Salva o arquivo Xml no caminho informado
                                {

                                    writerXml.WriteStartDocument();                                         //Inicia o documento XML
                                    writerXml.Formatting = Formatting.Indented;                                 //Usa a formatação
                                    writerXml.WriteStartElement("PAPEIS");                                   //Escreve o elemento Raiz

                                    while (!reader.EndOfStream)                                                 //Converte arquivo 9999.txt até o final
                                    {
                                        string LinhaDoArquivo = reader.ReadLine();                              //Ler linha

                                        //Tratamento dos das datas
                                        DateTime DataPregão = DateTime.ParseExact(LinhaDoArquivo.Substring(2, 8), "yyyymmdd", DateTimeFormatInfo.InvariantInfo);    //Data do pregão
                                        //                  DateTime DataVencimentoOpções = DateTime.ParseExact(LinhaDoArquivo.Substring(202, 8), "yyyymmdd", DateTimeFormatInfo.InvariantInfo);    //Data do vencimento de Opções
                                        //double
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
                                        //double PreçoExercício = double.Parse(LinhaDoArquivo.Substring(188, 13)) / 100;

                                        writerXml.WriteStartElement("PAPEL");                                   //Escreve o elemento Raiz

                                        //Escreve os sub-elementos
                                        writerXml.WriteElementString("DATA", DataPregão.ToString("dd/mm/yyyy"));
                                        //                    writerXml.WriteElementString("COD_BDI", LinhaDoArquivo.Substring(10, 2));
                                        writerXml.WriteElementString("CODIGO", LinhaDoArquivo.Substring(12, 12));
                                        //                    writerXml.WriteElementString("TIPO_MERC.", LinhaDoArquivo.Substring(24, 3)); //jÁ SEI QUE O TIPO DE MERCADO É VISTA (TIPO 010)
                                        writerXml.WriteElementString("NOME", LinhaDoArquivo.Substring(27, 12));
                                        writerXml.WriteElementString("ESPECI", LinhaDoArquivo.Substring(39, 10));
                                        //                    writerXml.WriteElementString("PRAZOT", LinhaDoArquivo.Substring(49, 3));
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
                                        //                    writerXml.WriteElementString("Pr_Exec.", PreçoExercício.ToString());
                                        //                    writerXml.WriteElementString("INDOPC", LinhaDoArquivo.Substring(201, 1));
                                        //                    writerXml.WriteElementString("DATA_VENCIMENTO", DataVencimentoOpções.ToString("dd/mm/yyyy"));
                                        //                    writerXml.WriteElementString("FATCOT", LinhaDoArquivo.Substring(210, 7));  //Fator de correção para opções
                                        //                    writerXml.WriteElementString("Pr_Exec_Ref_Dólar", LinhaDoArquivo.Substring(217, 7));
                                        //                    writerXml.WriteElementString("COD_ISI", LinhaDoArquivo.Substring(230, 12));
                                        //                    writerXml.WriteElementString("DISMES", LinhaDoArquivo.Substring(242, 3));

                                        //Encerra os elementos itens
                                        writerXml.WriteEndElement();
                                    }
                                    //writerXml.Close();                                                          //Escreve o XML para o arquivo e fecha o objeto escritor
                                }
                            }
                            catch (FormatException)//Verifica a validação do arquivo
                            {
                                FileInfo infoFile = new FileInfo(Path.GetFullPath(caminhoDoArquivo));
                                MessageBox.Show("Incapaz de ler arquivo " + infoFile.Name + ", pois está formatado errado!", "Sua execução foi invalidada!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                invaliadação = true; //Deleta pasta do arquivo concatenado
                            }
                    }
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show("ARQUIVO NÃO ENCONTRADO!", "Sua execução foi invalidada!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    invaliadação = true; //Deleta pasta do arquivo concatenado
                }
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Caminho do arquivo histórico não informado!", "Sua execução foi invalidada!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CabeçalhoArquivo(string salvarComo)
        {
            using (StreamWriter writer = new StreamWriter(salvarComo, false)) {
                writer.WriteLine("DATA\tCODNEG\tNOME\tESPECI\tMOEDAF\tP.Abe\tP.Max\tP.Min\tP.Med\tP.Ant\tP.Ult\tP.OFC\tP.OFV\tTotal NEG.\tQt.Total\tVolume");  //Escreve o Cabeçalho
            } 
        }

        private void DeleteArquivo(string caminhoDoArquivoSalvo)
        {

            if (File.Exists(caminhoDoArquivoSalvo)) // Se o arquivo tiver sido criado, então delete!
            {
                File.Delete(caminhoDoArquivoSalvo);
            }
        }

        private bool VerNomeArquivo(string caminhoDoDiretorio)
        {
            string[] arquivos = Directory.GetFiles(caminhoDoDiretorio, "*.txt", SearchOption.TopDirectoryOnly);         //Captura o nome completo "C:\User\...\AAAA.txt" de todos arquivos *.txt do diretório
            FileInfo infoArquivo;                                                                                       //Para obter informações do arquivo como nome, caminho do diretório, extenção, etc.
                      
            List<String> listaArquivos = new List<string>();                                                            //Cria uma lista onde serão armazenados os nomes dos arquivos
            foreach (string arq in arquivos)
            {
                listaArquivos.Add(arq);                                                                                 //Armazeno os arquivos numa lista
            }

            //Bloco para validação dos nomes dos arquivos histórioco =============================================            
            List<String> nomeDosArquivos = new List<string>();                                                          //Cria uma lista com o nome dos arquivos para serem verificados
            List<int> anoArquivos = new List<int>();                                                                    //Cria uma lista com os anos dos arquivos para serem verificados

            MessageBox.Show("Temos " + listaArquivos.Count.ToString() + " arquivos *.txt.");                            //Checagem
            Console.WriteLine("Arquivo \t ANO");                                                                        //Checagem
            int i = 0;
            foreach (string listFile in listaArquivos)
            {
                infoArquivo = new FileInfo(listFile);
                nomeDosArquivos.Add(infoArquivo.Name);                                                                  //Adiciona o nome dos arquivos a lista nomeDosArqivos
                if (nomeDosArquivos[i].Length==8)                                                                       //Verifica se o arquivo tem nome de comprimento 8 AAAA.txt
                {
                    try
                    {
                        anoArquivos.Add(int.Parse(nomeDosArquivos[i].Substring(0, 4)));                                 //Converte o nome dos arquivos para ano e adicina a lista anoArqivos
                        Console.WriteLine("Nome do Arquivo: " + nomeDosArquivos[i] + "\t" + "ANO: " + anoArquivos[i].ToString()); //Checagem
                        i = ++i;
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("O arquivo " + nomeDosArquivos[i] + " não está no formato ano.txt (AAAA.txt)", "NOME DE ARQUIVO INVÁLIDO!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;                                                                     //Caso o arquvo não esteja no formato esperado então
                    }
                }
                else
                {
                    MessageBox.Show("O arquivo " + nomeDosArquivos[i] + " não está no formato ano.txt (AAAA.txt)", "NOME DE ARQUIVO INVÁLIDO!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            //Bloco que verificar ordem dos nomes dos anos dos arquivos históricos======================
            anoArquivos.Sort();                                                                                         //Ordena o ano dos arquivos do menor para o maior
            int ano = anoArquivos[0];                                                                                   //Pego o primemiro ano
            bool existeAno;

            for (int cont = 0; cont < anoArquivos.Count; cont++)                                                        //Verifica se existe todos os anos esperados em ordem
            {
                existeAno = anoArquivos.Contains(ano);
                if (existeAno)
                {
                    Console.WriteLine("Existe o ano de " + ano);                                                        //Checagem
                    ano = ++ano;
                }
                else
                {
                    Console.WriteLine("Não existe o ano de " + ano);
                    MessageBox.Show("Não existe o ano de " + ano,"ARQUIVO NÃO ENCONTRADO!",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            return true;
        }        
    }
}
