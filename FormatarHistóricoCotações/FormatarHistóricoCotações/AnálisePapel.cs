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
    class AnálisePapel
    {

        public PontuaçãoPorCategorias PontuaçãoCategoria { get; private set; }  // São categorias para análise e cada categoria tem um peso correspondente
        public Categorias StatusCategoria { get; private set; }                 // Cada categoria tem um status. Exemplo: A categoria "HISTOGRAMA MACD (PICOS)" pode ter 3 status (INCONSSITÊNCIA POSITIVA,INDEFINIÇÃO,INCONSSITÊNCIA NEGATIVA)
        public double MontanteParaInvestir {get; private set;}                  //Montante disponível para investir

        public enum Tendência
        { 
            Alta,
            Indefinição,
            Baixa
        }


        public AnálisePapel(List<Papeis> HistóricoPapel, string operação, double ParaInvestir, string tendênciaÍndice, int períodoMMELenta, int períodoMMEIntermediária, int períodoMMERápida, int períodoBandaBollinger, int períodoIFR, int períodoMACD, int períodoADX)
        {
            PontuaçãoCategoria = new PontuaçãoPorCategorias();
            StatusCategoria = new Categorias();
            MontanteParaInvestir = ParaInvestir;

            TipoDeOperação(operação);                                               //1o método: Tipo de operação (comprado, vendido)
            PreçoAtivo(HistóricoPapel);                                             //2o método: Preço do ativo
            VerificarPreçosDeCompra(HistóricoPapel);                                //3o método: Verificar preço (pode comprar?)
            TendênciaDoÍNDICE(tendênciaÍndice);                                     //4o método: TENDÊNCIA DO ÍNDICE ANALISADO (IBOV -> alta, indefinição, baixa)
                      

            //*5o método: CANDEL TENDÊNCIA (alta, indefinição, baixa) (padrão do candel -> martel, enforcado, estrela cadente, etc.)

            //*6o método: TENDÊNDIA INTERMEDIÁRIA ou do Papel (alta, indefinição, baixa)

            FechamentoAtualAberturaAnterior(HistóricoPapel);                        //7o método: Compar o FECHAMENTO ATUAL e a AERTURA ANTERIOR (Fec > Abe, Fec = Abe, Fec < Abe)

            FechamentoDoCandel(HistóricoPapel);                                     //8o método: FECHAMENTO DO CANDEL (Fechou em alta, próximo da abertura, fechou em baixa)

            AberturaComMMELenta(HistóricoPapel, períodoMMELenta);                   //9o método: ABERTURA EM RELAÇÃO A  MÉDIA LENTA (Abe > MMELenta, Abe = MMELenta, Abe < MMELenta)

            AberturaComMMEIntermediária(HistóricoPapel, períodoMMEIntermediária);   //10o método: ABERTURA EM RELAÇÃO A  MÉDIA INTERMEDIÁRIA (Abe > MMEInter, Abe = MMEInter, Abe < MMEInter)

            AberturaComMMERápida(HistóricoPapel, períodoMMERápida);                 //11o método: ABERTURA EM RELAÇÃO A  MÉDIA RÁPIDA (Abe > MMERápida, Abe = MMERápida, Abe < MMERápida)

            //12o método: FECHAMENTO EM RELAÇÃO A  MÉDIA LENTA (Fec > MMELenta, Fec = MMELenta, Fec < MMELenta)

            //13o método: FECHAMENTO EM RELAÇÃO A  MÉDIA INTERMEDIÁRIA (Fec > MMEInter, Fec = MMEInter, Fec < MMEInter)

            //14o método: FECHAMENTO EM RELAÇÃO A  MÉDIA RÁPIDA (Fec > MMERÁPIDA, Fec = MMERÁPIDA, Fec < MMERÁPIDA)

            //15o método: PROXIMIDADE DO CANDEL & MÉDIAS MÓVEIS (Muito acima, pouco acima, próximo das médias, pouco abaixo, Muito abaixo)

            //16o método: MÉDIA RÁPIDA vs. MÉDIA LENTA (MMERápida > MMELenta, MMERápida = MMELenta, MMERápida < MMELenta)

            //17o método: BANDAS DE BOLLINGER (Fechou acima da superior, fechou próxima da superior, entre a superior e a média, próximo da média, entre a média e a inferior, próximo da inferior, fechou abaixo da inferior)

            //18o método: HISTOGRAMA MACD (PICOS) (Inconsistência positiva, Indefinição, Inconsistência negativa)

            //19o método: HISTOGRAMA MACD (Muito sobre comprado, sobre comprado, indefinido, sobre vendido, muito sobre vendido)

            //20o método: IFR (IFR>65% , 45< IFR<65%, IFR<45% )

            //21o método: Volume financeiro (Acima da média, próximo da média, abaixo da média)

            //22o método: Nº DE OPERAÇÃO (>2000, 1000< No < 2000, <1000)

            //23o método: DMI ( FORÇA DA TENDÊNCIA ) (Muito aberto e positivo, pouco aberto e positivo, próximas, pouco aberto e negativo, muito aberto e negativo)

            //24o método: DMI ( DIREÇÃO DA TENDÊNCIA ) (ADX < 30 e subindo, ADX > 30 e subindo, ADX caindo)

            //25o método: pontuação final (soma dos pontos)
        }
        // Método que define igualdade: Se |Fec - Abe| > 0 => Igualde = (+ ou - 5%|Fec - Abe|), Caso Contrário => Igualdede = (+ ou - 2,5%|Max - Min|)

        private void TipoDeOperação(string operação)                                                        //1o método: Tipo de operação (comprado, vendido)
        {
            StatusCategoria.Operação = operação;
            PontuaçãoCategoria.Operação = 0;
        }

        private void PreçoAtivo(List<Papeis> HistóricoPapel)                                                //2o método: Preço do ativo
        {
            StatusCategoria.PreçoDoAtivo=HistóricoPapel[HistóricoPapel.Count].PreçoFechamento;   //Pega a ultima cotação do papel
            PontuaçãoCategoria.PreçoDoAtivo = 0;
        }

        private void VerificarPreçosDeCompra(List<Papeis> HistóricoPapel)                                   //3o método: Verificar preço (pode comprar?)
        {
            if (MontanteParaInvestir/1000>HistóricoPapel[HistóricoPapel.Count].PreçoFechamento)     //Pode fazer compra de pelo menos 1000 ações
            {
                StatusCategoria.VerificaPreço = true;
                PontuaçãoCategoria.VerificaPreço = 1;
            }
            else
            {
                StatusCategoria.VerificaPreço = false;
                PontuaçãoCategoria.VerificaPreço = 0;
            }
        }

        private void TendênciaDoÍNDICE(string tendênciaÍndice)                                              //4o método: TENDÊNCIA DO ÍNDICE ANALISADO (IBOV -> alta, indefinição, baixa)
        {
            if (StatusCategoria.Operação == "comprado")
            {
                switch (tendênciaÍndice)
                {
                    case "alta":
                        StatusCategoria.TendênciaÍndice = Tendência.Alta.ToString();
                        PontuaçãoCategoria.TendênciaÍndice = 2;
                        break;
                    case "indefinição":
                        StatusCategoria.TendênciaÍndice = Tendência.Indefinição.ToString();
                        PontuaçãoCategoria.TendênciaÍndice = 1;
                        break;
                    case "baixa":
                        StatusCategoria.TendênciaÍndice = Tendência.Baixa.ToString();
                        PontuaçãoCategoria.TendênciaÍndice = 0;
                        break;
                }
            }
            else if (StatusCategoria.Operação == "vendido")
            {
                switch (tendênciaÍndice)
                {
                    case "alta":
                        StatusCategoria.TendênciaÍndice = Tendência.Alta.ToString();
                        PontuaçãoCategoria.TendênciaÍndice = 0;
                        break;
                    case "indefinição":
                        StatusCategoria.TendênciaÍndice = Tendência.Indefinição.ToString();
                        PontuaçãoCategoria.TendênciaÍndice = 1;
                        break;
                    case "baixa":
                        StatusCategoria.TendênciaÍndice = Tendência.Baixa.ToString();
                        PontuaçãoCategoria.TendênciaÍndice = 2;
                        break;
                }
            }
            else
            {
                MessageBox.Show("Tipo de operação não informada", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //*5o método: CANDEL TENDÊNCIA (alta, indefinição, baixa) (padrão do candel -> martel, enforcado, estrela cadente, etc.)
        //*6o método: TENDÊNDIA INTERMEDIÁRIA ou do Papel (alta, indefinição, baixa)

        private void FechamentoAtualAberturaAnterior(List<Papeis> HistóricoPapel)                           //7o método: Compar o FECHAMENTO ATUAL e a AERTURA ANTERIOR (Fec > Abe, Fec = Abe, Fec < Abe)
        {
            if (StatusCategoria.Operação=="comprado")
            {
                if (HistóricoPapel[HistóricoPapel.Count].PreçoFechamento > (1.05)*HistóricoPapel[HistóricoPapel.Count-1].PreçoAbertura)                  //Se FechamentoAtual > 1,05*AberturaAnterior (fechamento atual 5% maior que a abertura anterior)
                {
                    StatusCategoria.FechamentoEAberturarAnteriorPapel = Tendência.Alta.ToString();
                    PontuaçãoCategoria.FechamentoEAberturarAnteriorPapel = 2;
                }
                else if (HistóricoPapel[HistóricoPapel.Count].PreçoFechamento < (0.95)*HistóricoPapel[HistóricoPapel.Count - 1].PreçoAbertura)           //Se FechamentoAtual < 0,95*AberturaAnterior (fechamento atual 5% menor que a abertura anterior)
                {
                    StatusCategoria.FechamentoEAberturarAnteriorPapel = Tendência.Baixa.ToString();
                    PontuaçãoCategoria.FechamentoEAberturarAnteriorPapel = 0;
                }
                else//Igualdade                                                                                                                          //Se 0,95*AberturaAnterior <= FechamentoAtual <= 1,05*AberturaAnterior (fechamento atual entre + ou - 5% da abertura anterior)
                {
                    StatusCategoria.FechamentoEAberturarAnteriorPapel = Tendência.Indefinição.ToString();
                    PontuaçãoCategoria.FechamentoEAberturarAnteriorPapel = 1;
                }
            }
            else if (StatusCategoria.Operação=="vendido")
            {
                if (HistóricoPapel[HistóricoPapel.Count].PreçoFechamento > (1.05) * HistóricoPapel[HistóricoPapel.Count - 1].PreçoAbertura)                  //Se FechamentoAtual > 1,05*AberturaAnterior (fechamento atual 5% maior que a abertura anterior)
                {
                    StatusCategoria.FechamentoEAberturarAnteriorPapel = Tendência.Alta.ToString();
                    PontuaçãoCategoria.FechamentoEAberturarAnteriorPapel = 0;
                }
                else if (HistóricoPapel[HistóricoPapel.Count].PreçoFechamento < (0.95) * HistóricoPapel[HistóricoPapel.Count - 1].PreçoAbertura)           //Se FechamentoAtual < 0,95*AberturaAnterior (fechamento atual 5% menor que a abertura anterior)
                {
                    StatusCategoria.FechamentoEAberturarAnteriorPapel = Tendência.Baixa.ToString();
                    PontuaçãoCategoria.FechamentoEAberturarAnteriorPapel = 2;
                }
                else//Igualdade                                                                                                                          //Se 0,95*AberturaAnterior <= FechamentoAtual <= 1,05*AberturaAnterior (fechamento atual entre + ou - 5% da abertura anterior)
                {
                    StatusCategoria.FechamentoEAberturarAnteriorPapel = Tendência.Indefinição.ToString();
                    PontuaçãoCategoria.FechamentoEAberturarAnteriorPapel = 1;
                }
            }
            else
            {
                MessageBox.Show("Tipo de operação não informada", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FechamentoDoCandel(List<Papeis> HistóricoPapel)                                        //8o método: FECHAMENTO DO CANDEL (Fechou em alta, próximo da abertura, fechou em baixa)
        {

            if (StatusCategoria.Operação == "comprado")
            {
                if (HistóricoPapel[HistóricoPapel.Count].PreçoFechamento > (1.05) * HistóricoPapel[HistóricoPapel.Count].PreçoAbertura)                  //Se FechamentoAtual > 1,05*AberturaAtual (fechamento atual 5% maior que a abertura atual)
                {
                    StatusCategoria.FechamentoCandel = Tendência.Alta.ToString();
                    PontuaçãoCategoria.FechamentoCandel = 2;
                }
                else if (HistóricoPapel[HistóricoPapel.Count].PreçoFechamento < (0.95) * HistóricoPapel[HistóricoPapel.Count].PreçoAbertura)           //Se FechamentoAtual < 0,95*AberturaAtual (fechamento atual 5% menor que a abertura atual)
                {
                    StatusCategoria.FechamentoCandel = Tendência.Baixa.ToString();
                    PontuaçãoCategoria.FechamentoCandel = 0;
                }
                else//Igualdade                                                                                                                          //Se 0,95*AberturaAtual <= FechamentoAtual <= 1,05*AberturaAtual (fechamento atual entre + ou - 5% da abertura atual)
                {
                    StatusCategoria.FechamentoCandel = Tendência.Indefinição.ToString();
                    PontuaçãoCategoria.FechamentoCandel = 1;
                }
            }
            else if (StatusCategoria.Operação == "vendido")
            {
                if (HistóricoPapel[HistóricoPapel.Count].PreçoFechamento > (1.05) * HistóricoPapel[HistóricoPapel.Count].PreçoAbertura)                  //Se FechamentoAtual > 1,05*AberturaAtual (fechamento atual 5% maior que a abertura atual)
                {
                    StatusCategoria.FechamentoCandel = Tendência.Alta.ToString();
                    PontuaçãoCategoria.FechamentoCandel = 0;
                }
                else if (HistóricoPapel[HistóricoPapel.Count].PreçoFechamento < (0.95) * HistóricoPapel[HistóricoPapel.Count].PreçoAbertura)           //Se FechamentoAtual < 0,95*AberturaAnterior (fechamento atual 5% menor que a abertura anterior)
                {
                    StatusCategoria.FechamentoCandel = Tendência.Baixa.ToString();
                    PontuaçãoCategoria.FechamentoCandel = 2;
                }
                else//Igualdade                                                                                                                          //Se 0,95*AberturaAnterior <= FechamentoAtual <= 1,05*AberturaAnterior (fechamento atual entre + ou - 5% da abertura anterior)
                {
                    StatusCategoria.FechamentoCandel = Tendência.Indefinição.ToString();
                    PontuaçãoCategoria.FechamentoCandel = 1;
                }
            }
            else
            {
                MessageBox.Show("Tipo de operação não informada", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AberturaComMMELenta(List<Papeis> HistóricoPapel, int períodoMMELenta)                  //9o método: ABERTURA EM RELAÇÃO A  MÉDIA LENTA (Abe > MMELenta, Abe = MMELenta, Abe < MMELenta)
        {
            MédiaMóvelExponencial MMELenta = new MédiaMóvelExponencial(HistóricoPapel, períodoMMELenta);

            if (StatusCategoria.Operação == "comprado")
            {
                if (HistóricoPapel[HistóricoPapel.Count].PreçoAbertura > (1.05) * MMELenta.ListaDaMME[MMELenta.ListaDaMME.Count])                        //Se Abertura > 1,05*MMELenta (abertura atual 5% maior que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    StatusCategoria.AberturaEMédiaLenta = Tendência.Alta.ToString();
                    PontuaçãoCategoria.AberturaEMédiaLenta = 2;
                }
                else if (HistóricoPapel[HistóricoPapel.Count].PreçoAbertura < (0.95) * MMELenta.ListaDaMME[MMELenta.ListaDaMME.Count])                 //Se Abertura < 0,95*MMELenta (abertura atual 5% menor que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    StatusCategoria.AberturaEMédiaLenta = Tendência.Baixa.ToString();
                    PontuaçãoCategoria.AberturaEMédiaLenta = 0;
                }
                else//Igualdade                                                                                                                          //Se 0,95*MMELenta <= Abertura <= 1,05*MMELenta (fechamento atual entre + ou - 5% da abertura atual)
                {
                    StatusCategoria.AberturaEMédiaLenta = Tendência.Indefinição.ToString();
                    PontuaçãoCategoria.AberturaEMédiaLenta = 1;
                }
            }
            else if (StatusCategoria.Operação == "vendido")
            {
                if (HistóricoPapel[HistóricoPapel.Count].PreçoAbertura > (1.05) * MMELenta.ListaDaMME[MMELenta.ListaDaMME.Count])                  //Se Abertura > 1,05*MMELenta (abertura atual 5% maior que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    StatusCategoria.AberturaEMédiaLenta = Tendência.Alta.ToString();
                    PontuaçãoCategoria.AberturaEMédiaLenta = 0;
                }
                else if (HistóricoPapel[HistóricoPapel.Count].PreçoAbertura < (0.95) * MMELenta.ListaDaMME[MMELenta.ListaDaMME.Count])           //Se Abertura < 0,95*MMELenta (abertura atual 5% menor que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    StatusCategoria.AberturaEMédiaLenta = Tendência.Baixa.ToString();
                    PontuaçãoCategoria.AberturaEMédiaLenta = 2;
                }
                else//Igualdade                                                                                                                           //Se 0,95*MMELenta <= Abertura <= 1,05*MMELenta (fechamento atual entre + ou - 5% da abertura atual)
                {
                    StatusCategoria.AberturaEMédiaLenta = Tendência.Indefinição.ToString();
                    PontuaçãoCategoria.AberturaEMédiaLenta = 1;
                }
            }
            else
            {
                MessageBox.Show("Tipo de operação não informada", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AberturaComMMEIntermediária(List<Papeis> HistóricoPapel, int períodoMMEIntermediária)  //10o método: ABERTURA EM RELAÇÃO A  MÉDIA INTERMEDIÁRIA (Abe > MMEInter, Abe = MMEInter, Abe < MMEInter)
        {
            MédiaMóvelExponencial MMEIntermediária = new MédiaMóvelExponencial(HistóricoPapel, períodoMMEIntermediária);

            if (StatusCategoria.Operação == "comprado")
            {
                if (HistóricoPapel[HistóricoPapel.Count].PreçoAbertura > (1.05) * MMEIntermediária.ListaDaMME[MMEIntermediária.ListaDaMME.Count])                        //Se Abertura > 1,05*MMELenta (abertura atual 5% maior que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    StatusCategoria.AberturaEMédiaIntermediária = Tendência.Alta.ToString();
                    PontuaçãoCategoria.AberturaEMédiaIntermediária = 2;
                }
                else if (HistóricoPapel[HistóricoPapel.Count].PreçoAbertura < (0.95) * MMEIntermediária.ListaDaMME[MMEIntermediária.ListaDaMME.Count])                 //Se Abertura < 0,95*MMELenta (abertura atual 5% menor que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    StatusCategoria.AberturaEMédiaIntermediária = Tendência.Baixa.ToString();
                    PontuaçãoCategoria.AberturaEMédiaIntermediária = 0;
                }
                else//Igualdade                                                                                                                          //Se 0,95*MMELenta <= Abertura <= 1,05*MMELenta (fechamento atual entre + ou - 5% da abertura atual)
                {
                    StatusCategoria.AberturaEMédiaIntermediária = Tendência.Indefinição.ToString();
                    PontuaçãoCategoria.AberturaEMédiaIntermediária = 1;
                }
            }
            else if (StatusCategoria.Operação == "vendido")
            {
                if (HistóricoPapel[HistóricoPapel.Count].PreçoAbertura > (1.05) * MMEIntermediária.ListaDaMME[MMEIntermediária.ListaDaMME.Count])                  //Se Abertura > 1,05*MMELenta (abertura atual 5% maior que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    StatusCategoria.AberturaEMédiaIntermediária = Tendência.Alta.ToString();
                    PontuaçãoCategoria.AberturaEMédiaIntermediária = 0;
                }
                else if (HistóricoPapel[HistóricoPapel.Count].PreçoAbertura < (0.95) * MMEIntermediária.ListaDaMME[MMEIntermediária.ListaDaMME.Count])           //Se Abertura < 0,95*MMELenta (abertura atual 5% menor que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    StatusCategoria.AberturaEMédiaIntermediária = Tendência.Baixa.ToString();
                    PontuaçãoCategoria.AberturaEMédiaIntermediária = 2;
                }
                else//Igualdade                                                                                                                           //Se 0,95*MMELenta <= Abertura <= 1,05*MMELenta (fechamento atual entre + ou - 5% da abertura atual)
                {
                    StatusCategoria.AberturaEMédiaIntermediária = Tendência.Indefinição.ToString();
                    PontuaçãoCategoria.AberturaEMédiaIntermediária = 1;
                }
            }
            else
            {
                MessageBox.Show("Tipo de operação não informada", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AberturaComMMERápida(List<Papeis> HistóricoPapel, int períodoMMERápida)                //11o método: ABERTURA EM RELAÇÃO A  MÉDIA RÁPIDA (Abe > MMERápida, Abe = MMERápida, Abe < MMERápida)
        {
            MédiaMóvelExponencial MMERápida = new MédiaMóvelExponencial(HistóricoPapel, períodoMMERápida);

            if (StatusCategoria.Operação == "comprado")
            {
                if (HistóricoPapel[HistóricoPapel.Count].PreçoAbertura > (1.05) * MMERápida.ListaDaMME[MMERápida.ListaDaMME.Count])                        //Se Abertura > 1,05*MMELenta (abertura atual 5% maior que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    StatusCategoria.AberturaEMédiaRápida = Tendência.Alta.ToString();
                    PontuaçãoCategoria.AberturaEMédiaRápida = 2;
                }
                else if (HistóricoPapel[HistóricoPapel.Count].PreçoAbertura < (0.95) * MMERápida.ListaDaMME[MMERápida.ListaDaMME.Count])                 //Se Abertura < 0,95*MMELenta (abertura atual 5% menor que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    StatusCategoria.AberturaEMédiaRápida = Tendência.Baixa.ToString();
                    PontuaçãoCategoria.AberturaEMédiaRápida = 0;
                }
                else//Igualdade                                                                                                                          //Se 0,95*MMELenta <= Abertura <= 1,05*MMELenta (fechamento atual entre + ou - 5% da abertura atual)
                {
                    StatusCategoria.AberturaEMédiaRápida = Tendência.Indefinição.ToString();
                    PontuaçãoCategoria.AberturaEMédiaRápida = 1;
                }
            }
            else if (StatusCategoria.Operação == "vendido")
            {
                if (HistóricoPapel[HistóricoPapel.Count].PreçoAbertura > (1.05) * MMERápida.ListaDaMME[MMERápida.ListaDaMME.Count])                  //Se Abertura > 1,05*MMELenta (abertura atual 5% maior que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    StatusCategoria.AberturaEMédiaRápida = Tendência.Alta.ToString();
                    PontuaçãoCategoria.AberturaEMédiaRápida = 0;
                }
                else if (HistóricoPapel[HistóricoPapel.Count].PreçoAbertura < (0.95) * MMERápida.ListaDaMME[MMERápida.ListaDaMME.Count])           //Se Abertura < 0,95*MMELenta (abertura atual 5% menor que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    StatusCategoria.AberturaEMédiaRápida = Tendência.Baixa.ToString();
                    PontuaçãoCategoria.AberturaEMédiaRápida = 2;
                }
                else//Igualdade                                                                                                                           //Se 0,95*MMELenta <= Abertura <= 1,05*MMELenta (fechamento atual entre + ou - 5% da abertura atual)
                {
                    StatusCategoria.AberturaEMédiaRápida = Tendência.Indefinição.ToString();
                    PontuaçãoCategoria.AberturaEMédiaRápida = 1;
                }
            }
            else
            {
                MessageBox.Show("Tipo de operação não informada", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
