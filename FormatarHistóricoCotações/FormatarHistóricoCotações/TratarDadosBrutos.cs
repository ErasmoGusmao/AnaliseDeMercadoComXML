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
    class TratarDadosBrutos
    {
        protected bool invaliadação = false; // verificador deve está somente no método de UnificaçãoDadosBrutos
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

        protected void UnificaDadosBrutosTXT(string caminhoDosDadosBrutos, string salvarComoDadosBrutos)// Método que valida e filtra cada linha dos arquivos *.txt de entrada que serão concatenados
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
                                IniciaListaESPECI(); //Filtar dados brutos para leitura de algumas linhas do arquivo de entrada

                                while (!reader.EndOfStream)
                                {
                                    string LinhaDoArquivo = reader.ReadLine();
                                    if (LinhaDoArquivo.Substring(0, 2) == "01")
                                    {
                                        if (LinhaDoArquivo.Substring(24, 3).ToString() == "010") //Se o tipo de mercado for Mercado Vista ele copia linha caso contrário pula
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

                                                writer.WriteLine(LinhaDoArquivo); //Copia a linha do arquivo de entrada
                                            }
                                        }
                                    }
                                }

                                especiPapel.Clear(); // Limpa a lista
                            }
                            catch (FormatException) //Verifica o formato dos dados do arquivo de entrada para validação
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

        protected void DeleteArquivo(string caminhoDoArquivoSalvo)
        {

            if (File.Exists(caminhoDoArquivoSalvo)) // Se o arquivo tiver sido criado, então delete!
            {
                File.Delete(caminhoDoArquivoSalvo);
            }
        }

        protected bool VerNomeArquivo(string caminhoDoDiretorio) // Método para validar nome do arquivo de entrada e verificar se não falta o arquivo AAAA.txt ausente
        { //(protected para poder ser acessado somente pelas subclasses)
            string[] arquivos = Directory.GetFiles(caminhoDoDiretorio, "*.txt", SearchOption.TopDirectoryOnly);         //Captura o nome completo "C:\User\...\AAAA.txt" de todos arquivos *.txt do diretório
            FileInfo infoArquivo;                                                                                       //Para obter informações do arquivo como nome, caminho do diretório, extenção, etc.

            List<String> listaArquivos = new List<string>();                                                            //Cria uma lista onde serão armazenados os nomes dos arquivos
            foreach (string arq in arquivos)
            {
                listaArquivos.Add(arq);                                                                                 //Armazeno os arquivos numa lista
            }

            #region "Checa a validação dos nomes dos arquivos históriocos de entrada"
         
            List<String> nomeDosArquivos = new List<string>();                                                          //Cria uma lista com o nome dos arquivos para serem verificados
            List<int> anoArquivos = new List<int>();                                                                    //Cria uma lista com os anos dos arquivos para serem verificados

            MessageBox.Show("Temos " + listaArquivos.Count.ToString() + " arquivos *.txt.");                            //Checagem
            Console.WriteLine("Arquivo \t ANO");                                                                        //Checagem
            int i = 0;
            foreach (string listFile in listaArquivos)
            {
                infoArquivo = new FileInfo(listFile);
                nomeDosArquivos.Add(infoArquivo.Name);                                                                  //Adiciona o nome dos arquivos a lista nomeDosArqivos
                if (nomeDosArquivos[i].Length == 8)                                                                     //Verifica se o arquivo tem nome de comprimento 8 AAAA.txt
                {
                    try
                    {
                        anoArquivos.Add(int.Parse(nomeDosArquivos[i].Substring(0, 4)));                                 //Converte o nome dos arquivos para ano e adicina a lista anoArqivos
                        Console.WriteLine("Nome do Arquivo: " + nomeDosArquivos[i] + "\t" + "ANO: " + anoArquivos[i].ToString()); //Checagem não preciso dele
                        i = ++i;
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("O arquivo " + nomeDosArquivos[i] + " não está no formato ano.txt (AAAA.txt)", "NOME DE ARQUIVO INVÁLIDO!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;                                                                                   //Caso o arquvo não esteja no formato esperado então
                    }
                }
                else
                {
                    MessageBox.Show("O arquivo " + nomeDosArquivos[i] + " não está no formato ano.txt (AAAA.txt)", "NOME DE ARQUIVO INVÁLIDO!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            #endregion

            #region "Verifica ordem dos nomes dos anos dos arquivos históricos"
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
                    MessageBox.Show("Não existe o ano de " + ano, "ARQUIVO NÃO ENCONTRADO!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            #endregion

            return true;
        }

        public  virtual void ConcatenaArquivos(string caminhoDoDiretorio)
        {
            //Será alterado pelos arquivos das classes que herdam de TratarDadosBrutos
        }
    }
}
