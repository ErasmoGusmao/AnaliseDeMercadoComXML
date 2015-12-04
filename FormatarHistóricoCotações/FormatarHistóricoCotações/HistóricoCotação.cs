using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Collections;

namespace FormatarHistóricoCotações
{
    class HistóricoCotação
    {
        private bool invaliadação = false;

        public void ConcatenaArquivos(string caminhoDoDiretorio) 
        {
            try
            {
                string[] arquivos = Directory.GetFiles(caminhoDoDiretorio, "*.txt", SearchOption.TopDirectoryOnly);         //Captura o nome completo "C:\User\...\AAAA.txt" de todos arquivos *.txt do diretório

                if (VerNomeArquivo(caminhoDoDiretorio))                                                                     //Método que verifiar se os arquivos dos diretórios informados são todos válidos retorna um bool true ou false
                {
                    string caminhoSalvar = caminhoDoDiretorio + @"\Histórico Concatenado";
                    string salvarComo = caminhoSalvar + @"\HistóricoConcatenado.txt";

                    //Bloco que verifica a existência ou não do diretório onde será salvo o arquivo concatenado "caminhoSalva"
                    if (!Directory.Exists(caminhoSalvar))
                    {
                        Directory.CreateDirectory(caminhoSalvar);
                    }
                    if (File.Exists(salvarComo))
                    {
                        File.Delete(salvarComo);
                    }
                    //Cabeçalho do arquivo concatenado

                    CabeçalhoArquivo(salvarComo);

                    //Bloco que concatena os arquivos do diretório "caminhoDoDiretorio" passado para esse método           
                    foreach (string arq in arquivos)
                    {
                        Console.WriteLine(arq);
                        FormatarArquivo(arq, salvarComo);
                    }
                    //Bloco que verifica a validade do formato dos arquivos lidos
                    if (invaliadação)
                    {
                        DeleteArquivo(salvarComo); //Deletar pasta onde o arquivo concatenado seria criado
                        Console.WriteLine("Concatenação incompleta!");
                        MessageBox.Show("Concatenação incompleta!", "Operação abortada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        invaliadação = false;
                    }
                    else
                    {
                        Console.WriteLine("Concatenação completa!!");
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

        private void FormatarArquivo(string caminhoDoArquivo, string caminhoParaSalvarArqivo)
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
                                    switch (LinhaDoArquivo.Substring(0, 2))
                                    {
                                        case "00":
                                            //Não escreve nada
                                            //DateTime DataGeraçãoArquivo = DateTime.ParseExact(LinhaDoArquivo.Substring(23, 8), "yyyymmdd", DateTimeFormatInfo.InvariantInfo);    //Converte um String em DateTime do cabeçalho
                                            //writer.WriteLine(LinhaDoArquivo.Substring(0, 2) + "\t" + LinhaDoArquivo.Substring(2, 13) + "\t" + LinhaDoArquivo.Substring(15, 8) + "\t" + DataGeraçãoArquivo.ToString("dd/mm/yyyy") + "\t" + LinhaDoArquivo.Substring(31, 214));  //Escreve o Cabeçalho
                                            break;

                                        case "01":
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

                                            writer.WriteLine(LinhaDoArquivo.Substring(0, 2) + "\t" + DataPregão.ToString("dd/mm/yyyy") + "\t" + LinhaDoArquivo.Substring(10, 2)
                                        + "\t" + LinhaDoArquivo.Substring(12, 12) + "\t" + LinhaDoArquivo.Substring(24, 3) + "\t" + LinhaDoArquivo.Substring(27, 12)
                                        + "\t" + LinhaDoArquivo.Substring(39, 10) + "\t" + LinhaDoArquivo.Substring(49, 3) + "\t" + LinhaDoArquivo.Substring(52, 4)
                                        + "\t" + PreçoAbertura.ToString() + "\t" + PreçoMáximo.ToString() + "\t" + PreçoMínimo.ToString()
                                        + "\t" + PreçoMédio.ToString() + "\t" + PreçoAnterior.ToString() + "\t" + PreçoMelhorCompra.ToString()
                                        + "\t" + PreçoMelhorVenda.ToString() + "\t" + LinhaDoArquivo.Substring(147, 5) + "\t" + LinhaDoArquivo.Substring(152, 18)
                                        + "\t" + VolumeTotalNegociado.ToString() + "\t" + PreçoExercício.ToString() + "\t" + LinhaDoArquivo.Substring(201, 1)
                                        + "\t" + DataVencimentoOpções.ToString("dd/mm/yyyy") + "\t" + LinhaDoArquivo.Substring(210, 7) + "\t" + LinhaDoArquivo.Substring(217, 7)
                                        + "\t" + LinhaDoArquivo.Substring(230, 12) + "\t" + LinhaDoArquivo.Substring(242, 3));
                                            break;

                                        case "99":
                                            //Não escreve nada
                                            //DateTime DataGeraçãoArquivoFim = DateTime.ParseExact(LinhaDoArquivo.Substring(23, 8), "yyyymmdd", DateTimeFormatInfo.InvariantInfo);    //Converte um String em DateTime do fim do arquivo
                                            //writer.WriteLine(LinhaDoArquivo.Substring(0, 2) + "\t" + LinhaDoArquivo.Substring(2, 13) + "\t" + LinhaDoArquivo.Substring(15, 8) + "\t" + DataGeraçãoArquivoFim.ToString("dd/mm/yyyy") + "\t" + LinhaDoArquivo.Substring(31, 11) + "\t" + LinhaDoArquivo.Substring(42, 203));  //Escreve o Cabeçalho
                                            break;
                                    }
                                }
                            }
                            catch (FormatException)
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

        private void CabeçalhoArquivo(string salvarComo)
        {
            using (StreamWriter writer = new StreamWriter(salvarComo, false)) {
                writer.WriteLine("TIPREG\tDATA\tCODBDI\tCODNEG\tTPMERC\tNOMRES\tESPECI\tPRAZOT\tMODREF\tP.Abe\tP.Max\tP.Min\tP.Ult\tP.OFC\tP.OFV\tTotal NEG.\tQt.Total\tVolume Total\tP.EXE\tINDOPC\tDATA VENC.\tFATCOT\tPTOEXE\tCODISI\tDISMES");  //Escreve o Cabeçalho
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
