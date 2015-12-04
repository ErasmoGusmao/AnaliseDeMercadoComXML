using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FormatarHistóricoCotações
{
    class AçãoSelect //Seleciona a ação que vai para o listBox
    {
        private List<string> listaAçao;

        public List<string> ListaDeAção(string caminhoDoArquivo) 
        {
            List<string> listaTemp = new List<string>(); //Pega todos os papeis na coluna reservada para as ações (com repetição)
            using (StreamReader reader = new StreamReader(caminhoDoArquivo))
            {
                while (!reader.EndOfStream) //Ler até o final do arquivo
                {
                    string LinhaDoArquivo = reader.ReadLine(); //Ler cada linha do arquivo
                    if (LinhaDoArquivo.Substring(0,2)=="01") //Verifica se não é o cabeçalho
                    {
                        listaTemp.Add(LinhaDoArquivo.Substring(17,6)); //Adiciona todos os papeis na lista temporária
                    }
                }

                listaAçao = new List<string>();        //Seleção de todas as ações da ListaTemp (sem repetição)
                //Muito lento uso de foreach
                foreach (string item in listaTemp)                 //Verifica e adiciona a nova ação a listaAção<string>
                {
                    if (listaAçao.Contains(item))                   //Se ela já contem o item adicionado
                    {
                        listaAçao.Remove(item);                     //Então remova
                    }
                    else
                    {
                        listaAçao.Add(item);                            // Adiciono o item a minha lista ação
                    }
                }
                listaAçao.Sort();                                   //Ordena minha listaAção
                return listaAçao;                                   //Retorna a lista de ação
            }

        }
        public IEnumerable<string> PegaNomeAçao(string caminhoDoArquivo)
        {
            string[] NomeAçao = new string[ListaDeAção(caminhoDoArquivo).Count];  //Dimenção da string ListaDeAção(caminhoDoArquivo).Count
            for (int i = 0; i < ListaDeAção(caminhoDoArquivo).Count; i++)
            {
                NomeAçao[i] = ListaDeAção(caminhoDoArquivo)[i];
            }
            return NomeAçao;
        }
    }
}
