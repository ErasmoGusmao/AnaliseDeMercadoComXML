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
    class Estatística
    {
        public double Soma(List<double> ListaValores, int período)
        {//Calcula soma simples de todos os valores de um determinado período
            double soma;
            soma = 0;
            for (int i = 0; i < período; i++)
            {
                soma += ListaValores[i];
            }
            return soma;
        }
        public double Soma(List<Papeis> HistórioPapel, int período)
        {//Calcula soma simples de todos os valores de um determinado período (sobrecarga)
            double soma;
            soma = 0;
            for (int i = 0; i < período; i++)
            {
                soma += HistórioPapel[i].PreçoFechamento;
            }
            return soma;
        }

        public List<double> SomaMóvel(List<double> ListaValores, int período) 
        { //Calcula a soma de todos os elementos de uma lista de valores deslocando a janela de soma
            List<double> somaMóvel = new List<double>();
            somaMóvel.Clear();

            for (int i = 0; i < ListaValores.Count-período+1; i++)
            {
                double soma = 0;
                for (int j = i; j < período+i; j++)
                {
                    soma += ListaValores[j];   
                }
                somaMóvel.Add(soma);
            }
            return somaMóvel;
        }
        public List<double> SomaMóvel(List<Papeis> HistórioPapel, int período)
        { //Calcula a soma de todos os elementos de uma lista de valores deslocando a janela de soma (Sobrecarga)
            List<double> somaMóvel = new List<double>();
            somaMóvel.Clear();

            for (int i = 0; i < HistórioPapel.Count - período + 1; i++)
            {
                double soma = 0;
                for (int j = i; j < período + i; j++)
                {
                    soma += HistórioPapel[j].PreçoFechamento;
                }
                somaMóvel.Add(soma);
            }
            return somaMóvel;
        }

        public double Média(List<double> ListaValores, int período) 
        {//Calcula a média simples de todos os valores de um determinado período
            return (Soma(ListaValores,período)/((double)período));
        }
        public double Média(List<Papeis> HistórioPapel, int período)
        {//Calcula a média simples de todos os valores de um determinado período (Sobrecarga)
            return (Soma(HistórioPapel, período) / ((double)período));
        }

        public List<double> MédiaMóvel(List<double> ListaValores, int período)
        {//Calcula a média móvel de todos os elementos de uma lista de valores deslocando a janela de soma
            List<double> médiaMóvel = new List<double>();
            médiaMóvel.Clear();

            foreach (double valor in SomaMóvel(ListaValores,período))
            {
                médiaMóvel.Add(valor/((double)período));
            }
            return médiaMóvel;
        }
        public List<double> MédiaMóvel(List<Papeis> HistórioPapel, int período)
        {//Calcula a média móvel de todos os elementos de uma lista de valores deslocando a janela de soma (Sobrecarga)
            List<double> médiaMóvel = new List<double>();
            médiaMóvel.Clear();

            foreach (double valor in SomaMóvel(HistórioPapel, período))
            {
                médiaMóvel.Add(valor / ((double)período));
            }
            return médiaMóvel;
        }

        public double VariânciaP(List<double> ListaValores, int período) 
        {//Calcula a variância da população dos valores de um determinado período
            List<double> Delta = new List<double>(); //Declara uma variável que será dada por Delta[i] = (ListaValores[i] - média)^2
            Delta.Clear();

            for (int i = 0; i < período; i++)
            {
                Delta.Add(Math.Pow(ListaValores[i] - Média(ListaValores, período), 2));
            }
            return (Soma(Delta, período) / (double)período);                                                //(ListaValores[i] - média)^2/n
        }
        public double VariânciaP(List<Papeis> HistórioPapel, int período)
        {//Calcula a variância da população dos valores de um determinado período (Sobrecarga)
            List<double> Delta = new List<double>(); //Declara uma variável que será dada por Delta[i] = (ListaValores[i] - média)^2
            Delta.Clear();

            for (int i = 0; i < período; i++)
            {
                Delta.Add(Math.Pow(HistórioPapel[i].PreçoFechamento - Média(HistórioPapel, período), 2));
            }
            return (Soma(Delta, período) / (double)período);                                                //(ListaValores[i] - média)^2/n
        }

        public List<double> VariânciaPMóvel(List<double> ListaValores, int período) 
        {
            List<double> varPMóvel = new List<double>();
            varPMóvel.Clear();

            List<double> Delta = new List<double>(); //Declara uma variável que será dada por Delta[i] = (ListaValores[i] - média)^2

            for (int i = 0; i < ListaValores.Count-período+1; i++)
            {
                
                Delta.Clear();
                for (int j = i; j < período+i; j++)
                {
                    Delta.Add(Math.Pow(ListaValores[j] - MédiaMóvel(ListaValores, período)[i], 2));
                }
                varPMóvel.Add(Soma(Delta, período) / (double)período);                                                //(ListaValores[i] - média)^2/n
            }
            return varPMóvel;
        }
        public List<double> VariânciaPMóvel(List<Papeis> HistórioPapel, int período)
        {//Sobrecarga
            List<double> varPMóvel = new List<double>();
            varPMóvel.Clear();

            List<double> Delta = new List<double>(); //Declara uma variável que será dada por Delta[i] = (ListaValores[i] - média)^2

            for (int i = 0; i < HistórioPapel.Count - período + 1; i++)
            {

                Delta.Clear();
                for (int j = i; j < período + i; j++)
                {
                    Delta.Add(Math.Pow(HistórioPapel[j].PreçoFechamento - MédiaMóvel(HistórioPapel, período)[i], 2));
                }
                varPMóvel.Add(Soma(Delta, período) / (double)período);                                                //(ListaValores[i] - média)^2/n
            }
            return varPMóvel;
        }

        public double VariânciaA(List<double> ListaValores, int período)
        {//Calcula a variância da amostra dos valores de um determinado período
            List<double> Delta = new List<double>(); //Declara uma variável que será dada por Delta[i] = (ListaValores[i] - média)^2
            Delta.Clear();

            for (int i = 0; i < período; i++)
            {
                Delta.Add(Math.Pow(ListaValores[i] - Média(ListaValores, período), 2));
            }
            return (Soma(Delta, período) / ((double)período - 1));                                          //(ListaValores[i] - média)^2/(n-1)
        }
        public double VariânciaA(List<Papeis> HistórioPapel, int período)
        {//Calcula a variância da amostra dos valores de um determinado período (Sobrecarga)
            List<double> Delta = new List<double>(); //Declara uma variável que será dada por Delta[i] = (ListaValores[i] - média)^2
            Delta.Clear();

            for (int i = 0; i < período; i++)
            {
                Delta.Add(Math.Pow(HistórioPapel[i].PreçoFechamento - Média(HistórioPapel, período), 2));
            }
            return (Soma(Delta, período) / ((double)período - 1));                                          //(ListaValores[i] - média)^2/(n-1)
        }

        public List<double> VariânciaAMóvel(List<double> ListaValores, int período)
        {
            List<double> varAMóvel = new List<double>();
            varAMóvel.Clear();

            List<double> Delta = new List<double>(); //Declara uma variável que será dada por Delta[i] = (ListaValores[i] - média)^2

            for (int i = 0; i < ListaValores.Count - período + 1; i++)
            {

                Delta.Clear();
                for (int j = i; j < período + i; j++)
                {
                    Delta.Add(Math.Pow(ListaValores[j] - MédiaMóvel(ListaValores, período)[i], 2));
                }
                varAMóvel.Add(Soma(Delta, período) / ((double)período-1));                                                //(ListaValores[i] - média)^2/(n-1)
            }
            return varAMóvel;
        }
        public List<double> VariânciaAMóvel(List<Papeis> HistórioPapel, int período)
        {
            List<double> varAMóvel = new List<double>();
            varAMóvel.Clear();

            List<double> Delta = new List<double>(); //Declara uma variável que será dada por Delta[i] = (ListaValores[i] - média)^2

            for (int i = 0; i < HistórioPapel.Count - período + 1; i++)
            {

                Delta.Clear();
                for (int j = i; j < período + i; j++)
                {
                    Delta.Add(Math.Pow(HistórioPapel[j].PreçoFechamento - MédiaMóvel(HistórioPapel, período)[i], 2));
                }
                varAMóvel.Add(Soma(Delta, período) / ((double)período - 1));                                                //(ListaValores[i] - média)^2/(n-1)
            }
            return varAMóvel;
        }

        public double DesvPadP(List<double> ListaValores, int período) 
        {//Calcula o desvio padrão da poçulação de um conjunto de valores de um determinado período
            return Math.Sqrt(VariânciaP(ListaValores, período));
        }
        public double DesvPadP(List<Papeis> HistórioPapel, int período)
        {//Calcula o desvio padrão da poçulação de um conjunto de valores de um determinado período (Sobrecarga)
            return Math.Sqrt(VariânciaP(HistórioPapel, período));
        }

        public List<double> DesvPadPMóvel(List<double> ListaValores, int período) 
        {
            List<double> desvPadPMóvel = new List<double>();
            desvPadPMóvel.Clear();

            foreach (double valor in VariânciaPMóvel(ListaValores,período))
            {
                desvPadPMóvel.Add(Math.Sqrt(valor));
            }
            return desvPadPMóvel;
        }
        public List<double> DesvPadPMóvel(List<Papeis> HistórioPapel, int período)
        {
            List<double> desvPadPMóvel = new List<double>();
            desvPadPMóvel.Clear();

            foreach (double valor in VariânciaPMóvel(HistórioPapel, período))
            {
                desvPadPMóvel.Add(Math.Sqrt(valor));
            }
            return desvPadPMóvel;
        }

        public double DesvPadA(List<double> ListaValores, int período)
        {//Calcula o desvio padrão da amostra de um conjunto de valores de um determinado período
            return Math.Sqrt(VariânciaA(ListaValores, período));
        }
        public double DesvPadA(List<Papeis> HistórioPapel, int período)
        {//Calcula o desvio padrão da amostra de um conjunto de valores de um determinado período
            return Math.Sqrt(VariânciaA(HistórioPapel, período));
        }

        public List<double> DesvPadAMóvel(List<double> ListaValores, int período)
        {
            List<double> desvPadAMóvel = new List<double>();
            desvPadAMóvel.Clear();

            foreach (double valor in VariânciaAMóvel(ListaValores, período))
            {
                desvPadAMóvel.Add(Math.Sqrt(valor));
            }
            return desvPadAMóvel;
        }
        public List<double> DesvPadAMóvel(List<Papeis> HistórioPapel, int período)
        {
            List<double> desvPadAMóvel = new List<double>();
            desvPadAMóvel.Clear();

            foreach (double valor in VariânciaAMóvel(HistórioPapel, período))
            {
                desvPadAMóvel.Add(Math.Sqrt(valor));
            }
            return desvPadAMóvel;
        }
    }
}
