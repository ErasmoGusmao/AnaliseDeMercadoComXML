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
        public Categorias CategoriaStatus { get; private set; }                 // Cada categoria tem um status. Exemplo: A categoria "HISTOGRAMA MACD (PICOS)" pode ter 3 status (INCONSSITÊNCIA POSITIVA,INDEFINIÇÃO,INCONSSITÊNCIA NEGATIVA)
        public double MontanteParaInvestir {get; private set;}                  //Montante disponível para investir
        
        private double tolerânciaIgualdade = 0.005;                             //Margem de + ou - 0,5% de tolerância para igualdade

        public enum Tendência
        { 
            Alta,
            Indefinição,
            Baixa
        }


        public AnálisePapel(List<Papeis> HistóricoPapel, string operação, double ParaInvestir, string tendênciaÍndice, int períodoMMELenta, int períodoMMEIntermediária, int períodoMMERápida, int períodoBandaBollinger, double desvioMédiaBandaBollinger, int períodoIFR, int períodoMACD, int períodoADX)
        {
            PontuaçãoCategoria = new PontuaçãoPorCategorias();
            CategoriaStatus = new Categorias();
            MontanteParaInvestir = ParaInvestir;

            TipoDeOperação(operação);                                                                   //1o método: Tipo de operação (comprado, vendido)
            PreçoAtivo(HistóricoPapel);                                                                 //2o método: Preço do ativo
            VerificarPreçosDeCompra(HistóricoPapel);                                                    //3o método: Verificar preço (pode comprar?)
            TendênciaDoÍNDICE(tendênciaÍndice);                                                         //4o método: TENDÊNCIA DO ÍNDICE ANALISADO (IBOV -> alta, indefinição, baixa)
                      

            //*5o método: CANDEL TENDÊNCIA (alta, indefinição, baixa) (padrão do candel -> martel, enforcado, estrela cadente, etc.)

            //*6o método: TENDÊNDIA INTERMEDIÁRIA ou do Papel (alta, indefinição, baixa)

            FechamentoAtualAberturaAnterior(HistóricoPapel);                                            //7o método: Compar o FECHAMENTO ATUAL e a AERTURA ANTERIOR (Fec > Abe, Fec = Abe, Fec < Abe)

            FechamentoDoCandel(HistóricoPapel);                                                         //8o método: FECHAMENTO DO CANDEL (Fechou em alta, próximo da abertura, fechou em baixa)

            AberturaComMMELenta(HistóricoPapel, períodoMMELenta);                                       //9o método: ABERTURA EM RELAÇÃO A  MÉDIA LENTA (Abe > MMELenta, Abe = MMELenta, Abe < MMELenta)

            AberturaComMMEIntermediária(HistóricoPapel, períodoMMEIntermediária);                       //10o método: ABERTURA EM RELAÇÃO A  MÉDIA INTERMEDIÁRIA (Abe > MMEInter, Abe = MMEInter, Abe < MMEInter)

            AberturaComMMERápida(HistóricoPapel, períodoMMERápida);                                     //11o método: ABERTURA EM RELAÇÃO A  MÉDIA RÁPIDA (Abe > MMERápida, Abe = MMERápida, Abe < MMERápida)

            FechamentoComMMELenta(HistóricoPapel, períodoMMELenta);                                     //12o método: FECHAMENTO EM RELAÇÃO A  MÉDIA LENTA (Fec > MMELenta, Fec = MMELenta, Fec < MMELenta)

            FechamentoComMMEIntermediária(HistóricoPapel, períodoMMEIntermediária);                     //13o método: FECHAMENTO EM RELAÇÃO A  MÉDIA INTERMEDIÁRIA (Fec > MMEInter, Fec = MMEInter, Fec < MMEInter)

            FechamentoComMMERápida(HistóricoPapel, períodoMMERápida);                                   //14o método: FECHAMENTO EM RELAÇÃO A  MÉDIA RÁPIDA (Fec > MMERÁPIDA, Fec = MMERÁPIDA, Fec < MMERÁPIDA)

            CandelEMédias(HistóricoPapel, períodoMMELenta, períodoMMEIntermediária, períodoMMERápida);  //15o método: PROXIMIDADE DO CANDEL & MÉDIAS MÓVEIS (Muito acima, pouco acima, próximo das médias, pouco abaixo, Muito abaixo)

            ComparaMMERápidaLenta(HistóricoPapel, períodoMMELenta, períodoMMERápida);                   //16o método: MÉDIA RÁPIDA vs. MÉDIA LENTA (MMERápida > MMELenta, MMERápida = MMELenta, MMERápida < MMELenta)

            FechamentoBandasDeBollinger(HistóricoPapel, períodoBandaBollinger, desvioMédiaBandaBollinger);                         //17o método: BANDAS DE BOLLINGER (Fechou acima da superior, fechou próxima da superior, entre a superior e a média, próximo da média, entre a média e a inferior, próximo da inferior, fechou abaixo da inferior)

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

        private void TipoDeOperação(string operação)                                                                                    //1o método: Tipo de operação (comprado, vendido)
        {
            CategoriaStatus.Operação = operação;
            PontuaçãoCategoria.Operação = 0;
        }

        private void PreçoAtivo(List<Papeis> HistóricoPapel)                                                                            //2o método: Preço do ativo
        {
            CategoriaStatus.PreçoDoAtivo=HistóricoPapel[HistóricoPapel.Count].PreçoFechamento;   //Pega a ultima cotação do papel
            PontuaçãoCategoria.PreçoDoAtivo = 0;
        }

        private void VerificarPreçosDeCompra(List<Papeis> HistóricoPapel)                                                               //3o método: Verificar preço (pode comprar?)
        {
            if (MontanteParaInvestir/1000>HistóricoPapel[HistóricoPapel.Count].PreçoFechamento)     //Pode fazer compra de pelo menos 1000 ações
            {
                CategoriaStatus.VerificaPreço = true;
                PontuaçãoCategoria.VerificaPreço = 1;
            }
            else
            {
                CategoriaStatus.VerificaPreço = false;
                PontuaçãoCategoria.VerificaPreço = 0;
            }
        }

        private void TendênciaDoÍNDICE(string tendênciaÍndice)                                                                          //4o método: TENDÊNCIA DO ÍNDICE ANALISADO (IBOV -> alta, indefinição, baixa)
        {
            if (CategoriaStatus.Operação == "comprado")
            {
                switch (tendênciaÍndice)
                {
                    case "alta":
                        CategoriaStatus.TendênciaÍndice = Tendência.Alta.ToString();
                        PontuaçãoCategoria.TendênciaÍndice = 2;
                        break;
                    case "indefinição":
                        CategoriaStatus.TendênciaÍndice = Tendência.Indefinição.ToString();
                        PontuaçãoCategoria.TendênciaÍndice = 1;
                        break;
                    case "baixa":
                        CategoriaStatus.TendênciaÍndice = Tendência.Baixa.ToString();
                        PontuaçãoCategoria.TendênciaÍndice = 0;
                        break;
                }
            }
            else if (CategoriaStatus.Operação == "vendido")
            {
                switch (tendênciaÍndice)
                {
                    case "alta":
                        CategoriaStatus.TendênciaÍndice = Tendência.Alta.ToString();
                        PontuaçãoCategoria.TendênciaÍndice = 0;
                        break;
                    case "indefinição":
                        CategoriaStatus.TendênciaÍndice = Tendência.Indefinição.ToString();
                        PontuaçãoCategoria.TendênciaÍndice = 1;
                        break;
                    case "baixa":
                        CategoriaStatus.TendênciaÍndice = Tendência.Baixa.ToString();
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

        private void FechamentoAtualAberturaAnterior(List<Papeis> HistóricoPapel)                                                       //7o método: Compar o FECHAMENTO ATUAL e a AERTURA ANTERIOR (Fec > Abe, Fec = Abe, Fec < Abe)
        {
            if (CategoriaStatus.Operação=="comprado")
            {
                if (HistóricoPapel[HistóricoPapel.Count].PreçoFechamento > (1+tolerânciaIgualdade)*HistóricoPapel[HistóricoPapel.Count-1].PreçoAbertura)                  //Se FechamentoAtual > 1,005*AberturaAnterior (fechamento atual 0,5% maior que a abertura anterior)
                {
                    CategoriaStatus.FechamentoEAberturarAnteriorPapel = Tendência.Alta.ToString();
                    PontuaçãoCategoria.FechamentoEAberturarAnteriorPapel = 2;
                }
                else if (HistóricoPapel[HistóricoPapel.Count].PreçoFechamento < (1-tolerânciaIgualdade)*HistóricoPapel[HistóricoPapel.Count - 1].PreçoAbertura)           //Se FechamentoAtual < 0,995*AberturaAnterior (fechamento atual 0,5% menor que a abertura anterior)
                {
                    CategoriaStatus.FechamentoEAberturarAnteriorPapel = Tendência.Baixa.ToString();
                    PontuaçãoCategoria.FechamentoEAberturarAnteriorPapel = 0;
                }
                else//Igualdade                                                                                                                          //Se 0,995*AberturaAnterior <= FechamentoAtual <= 1,005*AberturaAnterior (fechamento atual entre + ou - 0,5% da abertura anterior)
                {
                    CategoriaStatus.FechamentoEAberturarAnteriorPapel = Tendência.Indefinição.ToString();
                    PontuaçãoCategoria.FechamentoEAberturarAnteriorPapel = 1;
                }
            }
            else if (CategoriaStatus.Operação=="vendido")
            {
                if (HistóricoPapel[HistóricoPapel.Count].PreçoFechamento > (1+tolerânciaIgualdade) * HistóricoPapel[HistóricoPapel.Count - 1].PreçoAbertura)                  //Se FechamentoAtual > 1,005*AberturaAnterior (fechamento atual 0,5% maior que a abertura anterior)
                {
                    CategoriaStatus.FechamentoEAberturarAnteriorPapel = Tendência.Alta.ToString();
                    PontuaçãoCategoria.FechamentoEAberturarAnteriorPapel = 0;
                }
                else if (HistóricoPapel[HistóricoPapel.Count].PreçoFechamento < (1-tolerânciaIgualdade) * HistóricoPapel[HistóricoPapel.Count - 1].PreçoAbertura)           //Se FechamentoAtual < 0,995*AberturaAnterior (fechamento atual 0,5% menor que a abertura anterior)
                {
                    CategoriaStatus.FechamentoEAberturarAnteriorPapel = Tendência.Baixa.ToString();
                    PontuaçãoCategoria.FechamentoEAberturarAnteriorPapel = 2;
                }
                else//Igualdade                                                                                                                          //Se 0,995*AberturaAnterior <= FechamentoAtual <= 1,005*AberturaAnterior (fechamento atual entre + ou - 0,5% da abertura anterior)
                {
                    CategoriaStatus.FechamentoEAberturarAnteriorPapel = Tendência.Indefinição.ToString();
                    PontuaçãoCategoria.FechamentoEAberturarAnteriorPapel = 1;
                }
            }
            else
            {
                MessageBox.Show("Tipo de operação não informada", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FechamentoDoCandel(List<Papeis> HistóricoPapel)                                                                    //8o método: FECHAMENTO DO CANDEL (Fechou em alta, próximo da abertura, fechou em baixa)
        {

            if (CategoriaStatus.Operação == "comprado")
            {
                if (HistóricoPapel[HistóricoPapel.Count].PreçoFechamento > (1+tolerânciaIgualdade) * HistóricoPapel[HistóricoPapel.Count].PreçoAbertura)                  //Se FechamentoAtual > 1,005*AberturaAtual (fechamento atual 0,5% maior que a abertura atual)
                {
                    CategoriaStatus.FechamentoCandel = Tendência.Alta.ToString();
                    PontuaçãoCategoria.FechamentoCandel = 2;
                }
                else if (HistóricoPapel[HistóricoPapel.Count].PreçoFechamento < (1-tolerânciaIgualdade) * HistóricoPapel[HistóricoPapel.Count].PreçoAbertura)           //Se FechamentoAtual < 0,995*AberturaAtual (fechamento atual 0,5% menor que a abertura atual)
                {
                    CategoriaStatus.FechamentoCandel = Tendência.Baixa.ToString();
                    PontuaçãoCategoria.FechamentoCandel = 0;
                }
                else//Igualdade                                                                                                                          //Se 0,995*AberturaAtual <= FechamentoAtual <= 1,005*AberturaAtual (fechamento atual entre + ou - 5% da abertura atual)
                {
                    CategoriaStatus.FechamentoCandel = Tendência.Indefinição.ToString();
                    PontuaçãoCategoria.FechamentoCandel = 1;
                }
            }
            else if (CategoriaStatus.Operação == "vendido")
            {
                if (HistóricoPapel[HistóricoPapel.Count].PreçoFechamento > (1+tolerânciaIgualdade) * HistóricoPapel[HistóricoPapel.Count].PreçoAbertura)                  //Se FechamentoAtual > 1,005*AberturaAtual (fechamento atual 0,5% maior que a abertura atual)
                {
                    CategoriaStatus.FechamentoCandel = Tendência.Alta.ToString();
                    PontuaçãoCategoria.FechamentoCandel = 0;
                }
                else if (HistóricoPapel[HistóricoPapel.Count].PreçoFechamento < (1-tolerânciaIgualdade) * HistóricoPapel[HistóricoPapel.Count].PreçoAbertura)           //Se FechamentoAtual < 0,995*AberturaAnterior (fechamento atual 0,5% menor que a abertura anterior)
                {
                    CategoriaStatus.FechamentoCandel = Tendência.Baixa.ToString();
                    PontuaçãoCategoria.FechamentoCandel = 2;
                }
                else//Igualdade                                                                                                                          //Se 0,995*AberturaAnterior <= FechamentoAtual <= 1,005*AberturaAnterior (fechamento atual entre + ou - 0,5% da abertura anterior)
                {
                    CategoriaStatus.FechamentoCandel = Tendência.Indefinição.ToString();
                    PontuaçãoCategoria.FechamentoCandel = 1;
                }
            }
            else
            {
                MessageBox.Show("Tipo de operação não informada", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AberturaComMMELenta(List<Papeis> HistóricoPapel, int períodoMMELenta)                                              //9o método: ABERTURA EM RELAÇÃO A  MÉDIA LENTA (Abe > MMELenta, Abe = MMELenta, Abe < MMELenta)
        {
            MédiaMóvelExponencial MMELenta = new MédiaMóvelExponencial(HistóricoPapel, períodoMMELenta);

            if (CategoriaStatus.Operação == "comprado")
            {
                if (HistóricoPapel[HistóricoPapel.Count].PreçoAbertura > (1+tolerânciaIgualdade) * MMELenta.ListaDaMME[MMELenta.ListaDaMME.Count])                        //Se Abertura > 1,005*MMELenta (abertura atual 0,5% maior que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    CategoriaStatus.AberturaEMédiaLenta = Tendência.Alta.ToString();
                    PontuaçãoCategoria.AberturaEMédiaLenta = 2;
                }
                else if (HistóricoPapel[HistóricoPapel.Count].PreçoAbertura < (1-tolerânciaIgualdade) * MMELenta.ListaDaMME[MMELenta.ListaDaMME.Count])                 //Se Abertura < 0,995*MMELenta (abertura atual 0,5% menor que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    CategoriaStatus.AberturaEMédiaLenta = Tendência.Baixa.ToString();
                    PontuaçãoCategoria.AberturaEMédiaLenta = 0;
                }
                else//Igualdade                                                                                                                          //Se 0,995*MMELenta <= Abertura <= 1,005*MMELenta (fechamento atual entre + ou - 0,5% da abertura atual)
                {
                    CategoriaStatus.AberturaEMédiaLenta = Tendência.Indefinição.ToString();
                    PontuaçãoCategoria.AberturaEMédiaLenta = 1;
                }
            }
            else if (CategoriaStatus.Operação == "vendido")
            {
                if (HistóricoPapel[HistóricoPapel.Count].PreçoAbertura > (1+tolerânciaIgualdade) * MMELenta.ListaDaMME[MMELenta.ListaDaMME.Count])                  //Se Abertura > 1,005*MMELenta (abertura atual 0,5% maior que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    CategoriaStatus.AberturaEMédiaLenta = Tendência.Alta.ToString();
                    PontuaçãoCategoria.AberturaEMédiaLenta = 0;
                }
                else if (HistóricoPapel[HistóricoPapel.Count].PreçoAbertura < (1-tolerânciaIgualdade) * MMELenta.ListaDaMME[MMELenta.ListaDaMME.Count])           //Se Abertura < 0,995*MMELenta (abertura atual 0,5% menor que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    CategoriaStatus.AberturaEMédiaLenta = Tendência.Baixa.ToString();
                    PontuaçãoCategoria.AberturaEMédiaLenta = 2;
                }
                else//Igualdade                                                                                                                           //Se 0,995*MMELenta <= Abertura <= 1,005*MMELenta (fechamento atual entre + ou - 0,5% da abertura atual)
                {
                    CategoriaStatus.AberturaEMédiaLenta = Tendência.Indefinição.ToString();
                    PontuaçãoCategoria.AberturaEMédiaLenta = 1;
                }
            }
            else
            {
                MessageBox.Show("Tipo de operação não informada", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AberturaComMMEIntermediária(List<Papeis> HistóricoPapel, int períodoMMEIntermediária)                              //10o método: ABERTURA EM RELAÇÃO A  MÉDIA INTERMEDIÁRIA (Abe > MMEInter, Abe = MMEInter, Abe < MMEInter)
        {
            MédiaMóvelExponencial MMEIntermediária = new MédiaMóvelExponencial(HistóricoPapel, períodoMMEIntermediária);

            if (CategoriaStatus.Operação == "comprado")
            {
                if (HistóricoPapel[HistóricoPapel.Count].PreçoAbertura > (1+tolerânciaIgualdade) * MMEIntermediária.ListaDaMME[MMEIntermediária.ListaDaMME.Count])                        //Se Abertura > 1,005*MMELenta (abertura atual 5% maior que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    CategoriaStatus.AberturaEMédiaIntermediária = Tendência.Alta.ToString();
                    PontuaçãoCategoria.AberturaEMédiaIntermediária = 2;
                }
                else if (HistóricoPapel[HistóricoPapel.Count].PreçoAbertura < (1-tolerânciaIgualdade) * MMEIntermediária.ListaDaMME[MMEIntermediária.ListaDaMME.Count])                 //Se Abertura < 0,995*MMELenta (abertura atual 0,5% menor que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    CategoriaStatus.AberturaEMédiaIntermediária = Tendência.Baixa.ToString();
                    PontuaçãoCategoria.AberturaEMédiaIntermediária = 0;
                }
                else//Igualdade                                                                                                                          //Se 0,995*MMELenta <= Abertura <= 1,005*MMELenta (fechamento atual entre + ou - 0,5% da abertura atual)
                {
                    CategoriaStatus.AberturaEMédiaIntermediária = Tendência.Indefinição.ToString();
                    PontuaçãoCategoria.AberturaEMédiaIntermediária = 1;
                }
            }
            else if (CategoriaStatus.Operação == "vendido")
            {
                if (HistóricoPapel[HistóricoPapel.Count].PreçoAbertura > (1+tolerânciaIgualdade) * MMEIntermediária.ListaDaMME[MMEIntermediária.ListaDaMME.Count])                  //Se Abertura > 1,005*MMELenta (abertura atual 0,5% maior que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    CategoriaStatus.AberturaEMédiaIntermediária = Tendência.Alta.ToString();
                    PontuaçãoCategoria.AberturaEMédiaIntermediária = 0;
                }
                else if (HistóricoPapel[HistóricoPapel.Count].PreçoAbertura < (1-tolerânciaIgualdade) * MMEIntermediária.ListaDaMME[MMEIntermediária.ListaDaMME.Count])           //Se Abertura < 0,995*MMELenta (abertura atual 0,5% menor que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    CategoriaStatus.AberturaEMédiaIntermediária = Tendência.Baixa.ToString();
                    PontuaçãoCategoria.AberturaEMédiaIntermediária = 2;
                }
                else//Igualdade                                                                                                                           //Se 0,995*MMELenta <= Abertura <= 1,005*MMELenta (fechamento atual entre + ou - 0,5% da abertura atual)
                {
                    CategoriaStatus.AberturaEMédiaIntermediária = Tendência.Indefinição.ToString();
                    PontuaçãoCategoria.AberturaEMédiaIntermediária = 1;
                }
            }
            else
            {
                MessageBox.Show("Tipo de operação não informada", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AberturaComMMERápida(List<Papeis> HistóricoPapel, int períodoMMERápida)                                            //11o método: ABERTURA EM RELAÇÃO A  MÉDIA RÁPIDA (Abe > MMERápida, Abe = MMERápida, Abe < MMERápida)
        {
            MédiaMóvelExponencial MMERápida = new MédiaMóvelExponencial(HistóricoPapel, períodoMMERápida);

            if (CategoriaStatus.Operação == "comprado")
            {
                if (HistóricoPapel[HistóricoPapel.Count].PreçoAbertura > (1+tolerânciaIgualdade) * MMERápida.ListaDaMME[MMERápida.ListaDaMME.Count])                        //Se Abertura > 1,005*MMELenta (abertura atual 0,5% maior que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    CategoriaStatus.AberturaEMédiaRápida = Tendência.Alta.ToString();
                    PontuaçãoCategoria.AberturaEMédiaRápida = 2;
                }
                else if (HistóricoPapel[HistóricoPapel.Count].PreçoAbertura < (1-tolerânciaIgualdade) * MMERápida.ListaDaMME[MMERápida.ListaDaMME.Count])                 //Se Abertura < 0,995*MMELenta (abertura atual 0,5% menor que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    CategoriaStatus.AberturaEMédiaRápida = Tendência.Baixa.ToString();
                    PontuaçãoCategoria.AberturaEMédiaRápida = 0;
                }
                else//Igualdade                                                                                                                          //Se 0,995*MMELenta <= Abertura <= 1,005*MMELenta (fechamento atual entre + ou - 0,5% da abertura atual)
                {
                    CategoriaStatus.AberturaEMédiaRápida = Tendência.Indefinição.ToString();
                    PontuaçãoCategoria.AberturaEMédiaRápida = 1;
                }
            }
            else if (CategoriaStatus.Operação == "vendido")
            {
                if (HistóricoPapel[HistóricoPapel.Count].PreçoAbertura > (1+tolerânciaIgualdade) * MMERápida.ListaDaMME[MMERápida.ListaDaMME.Count])                  //Se Abertura > 1,005*MMELenta (abertura atual 0,5% maior que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    CategoriaStatus.AberturaEMédiaRápida = Tendência.Alta.ToString();
                    PontuaçãoCategoria.AberturaEMédiaRápida = 0;
                }
                else if (HistóricoPapel[HistóricoPapel.Count].PreçoAbertura < (1-tolerânciaIgualdade) * MMERápida.ListaDaMME[MMERápida.ListaDaMME.Count])           //Se Abertura < 0,995*MMELenta (abertura atual 0,5% menor que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    CategoriaStatus.AberturaEMédiaRápida = Tendência.Baixa.ToString();
                    PontuaçãoCategoria.AberturaEMédiaRápida = 2;
                }
                else//Igualdade                                                                                                                           //Se 0,995*MMELenta <= Abertura <= 1,005*MMELenta (fechamento atual entre + ou - 0,5% da abertura atual)
                {
                    CategoriaStatus.AberturaEMédiaRápida = Tendência.Indefinição.ToString();
                    PontuaçãoCategoria.AberturaEMédiaRápida = 1;
                }
            }
            else
            {
                MessageBox.Show("Tipo de operação não informada", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FechamentoComMMELenta(List<Papeis> HistóricoPapel, int períodoMMELenta)                                            //12o método: FECHAMENTO EM RELAÇÃO A  MÉDIA LENTA (Fec > MMELenta, Fec = MMELenta, Fec < MMELenta)
        {
            MédiaMóvelExponencial MMELenta = new MédiaMóvelExponencial(HistóricoPapel, períodoMMELenta);

            if (CategoriaStatus.Operação == "comprado")
            {
                if (HistóricoPapel[HistóricoPapel.Count].PreçoFechamento > (1+tolerânciaIgualdade) * MMELenta.ListaDaMME[MMELenta.ListaDaMME.Count])                        //Se Abertura > 1,005*MMELenta (abertura atual 0,5% maior que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    CategoriaStatus.FechamentoEMédiaLenta = Tendência.Alta.ToString();
                    PontuaçãoCategoria.FechamentoEMédiaLenta = 2;
                }
                else if (HistóricoPapel[HistóricoPapel.Count].PreçoFechamento < (1-tolerânciaIgualdade) * MMELenta.ListaDaMME[MMELenta.ListaDaMME.Count])                 //Se Abertura < 0,995*MMELenta (abertura atual 0,5% menor que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    CategoriaStatus.FechamentoEMédiaLenta = Tendência.Baixa.ToString();
                    PontuaçãoCategoria.FechamentoEMédiaLenta = 0;
                }
                else//Igualdade                                                                                                                          //Se 0,995*MMELenta <= Abertura <= 1,005*MMELenta (fechamento atual entre + ou - 0,5% da abertura atual)
                {
                    CategoriaStatus.FechamentoEMédiaLenta = Tendência.Indefinição.ToString();
                    PontuaçãoCategoria.FechamentoEMédiaLenta = 1;
                }
            }
            else if (CategoriaStatus.Operação == "vendido")
            {
                if (HistóricoPapel[HistóricoPapel.Count].PreçoFechamento > (1+tolerânciaIgualdade) * MMELenta.ListaDaMME[MMELenta.ListaDaMME.Count])                  //Se Abertura > 1,005*MMELenta (abertura atual 0,5% maior que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    CategoriaStatus.FechamentoEMédiaLenta = Tendência.Alta.ToString();
                    PontuaçãoCategoria.FechamentoEMédiaLenta = 0;
                }
                else if (HistóricoPapel[HistóricoPapel.Count].PreçoFechamento < (1-tolerânciaIgualdade) * MMELenta.ListaDaMME[MMELenta.ListaDaMME.Count])           //Se Abertura < 0,995*MMELenta (abertura atual  0,5% menor que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    CategoriaStatus.FechamentoEMédiaLenta = Tendência.Baixa.ToString();
                    PontuaçãoCategoria.FechamentoEMédiaLenta = 2;
                }
                else//Igualdade                                                                                                                           //Se 0,995*MMELenta <= Abertura <= 1,005*MMELenta (fechamento atual entre + ou - 0,5% da abertura atual)
                {
                    CategoriaStatus.FechamentoEMédiaLenta = Tendência.Indefinição.ToString();
                    PontuaçãoCategoria.FechamentoEMédiaLenta = 1;
                }
            }
            else
            {
                MessageBox.Show("Tipo de operação não informada", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FechamentoComMMEIntermediária(List<Papeis> HistóricoPapel, int períodoMMEIntermediária)                            //13o método: FECHAMENTO EM RELAÇÃO A  MÉDIA INTERMEDIÁRIA (Fec > MMEInter, Fec = MMEInter, Fec < MMEInter)
        {
            MédiaMóvelExponencial MMEIntermediária = new MédiaMóvelExponencial(HistóricoPapel, períodoMMEIntermediária);

            if (CategoriaStatus.Operação == "comprado")
            {
                if (HistóricoPapel[HistóricoPapel.Count].PreçoFechamento > (1+tolerânciaIgualdade) * MMEIntermediária.ListaDaMME[MMEIntermediária.ListaDaMME.Count])                        //Se Abertura > 1,005*MMELenta (abertura atual 0,5% maior que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    CategoriaStatus.FechamentoEMédiaIntermediária = Tendência.Alta.ToString();
                    PontuaçãoCategoria.FechamentoEMédiaIntermediária = 2;
                }
                else if (HistóricoPapel[HistóricoPapel.Count].PreçoFechamento < (1-tolerânciaIgualdade) * MMEIntermediária.ListaDaMME[MMEIntermediária.ListaDaMME.Count])                 //Se Abertura < 0,995*MMELenta (abertura atual 0,5% menor que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    CategoriaStatus.FechamentoEMédiaIntermediária = Tendência.Baixa.ToString();
                    PontuaçãoCategoria.FechamentoEMédiaIntermediária = 0;
                }
                else//Igualdade                                                                                                                          //Se 0,995*MMELenta <= Abertura <= 1,005*MMELenta (fechamento atual entre + ou - 0,5% da abertura atual)
                {
                    CategoriaStatus.FechamentoEMédiaIntermediária = Tendência.Indefinição.ToString();
                    PontuaçãoCategoria.FechamentoEMédiaIntermediária = 1;
                }
            }
            else if (CategoriaStatus.Operação == "vendido")
            {
                if (HistóricoPapel[HistóricoPapel.Count].PreçoFechamento > (1+tolerânciaIgualdade) * MMEIntermediária.ListaDaMME[MMEIntermediária.ListaDaMME.Count])                  //Se Abertura > 1,005*MMELenta (abertura atual 0,5% maior que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    CategoriaStatus.FechamentoEMédiaIntermediária = Tendência.Alta.ToString();
                    PontuaçãoCategoria.FechamentoEMédiaIntermediária = 0;
                }
                else if (HistóricoPapel[HistóricoPapel.Count].PreçoFechamento < (1-tolerânciaIgualdade) * MMEIntermediária.ListaDaMME[MMEIntermediária.ListaDaMME.Count])           //Se Abertura < 0,995*MMELenta (abertura atual  0,5% menor que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    CategoriaStatus.FechamentoEMédiaIntermediária = Tendência.Baixa.ToString();
                    PontuaçãoCategoria.FechamentoEMédiaIntermediária = 2;
                }
                else//Igualdade                                                                                                                           //Se 0,995*MMELenta <= Abertura <= 1,005*MMELenta (fechamento atual entre + ou - 0,5% da abertura atual)
                {
                    CategoriaStatus.FechamentoEMédiaIntermediária = Tendência.Indefinição.ToString();
                    PontuaçãoCategoria.FechamentoEMédiaIntermediária = 1;
                }
            }
            else
            {
                MessageBox.Show("Tipo de operação não informada", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FechamentoComMMERápida(List<Papeis> HistóricoPapel, int períodoMMERápida)                                          //14o método: FECHAMENTO EM RELAÇÃO A  MÉDIA RÁPIDA (Fec > MMERÁPIDA, Fec = MMERÁPIDA, Fec < MMERÁPIDA)
        {
            MédiaMóvelExponencial MMERápida = new MédiaMóvelExponencial(HistóricoPapel, períodoMMERápida);

            if (CategoriaStatus.Operação == "comprado")
            {
                if (HistóricoPapel[HistóricoPapel.Count].PreçoFechamento > (1+tolerânciaIgualdade) * MMERápida.ListaDaMME[MMERápida.ListaDaMME.Count])                        //Se Abertura > 1,005*MMELenta (abertura atual 0,5% maior que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    CategoriaStatus.FechamentoEMédiaRápida = Tendência.Alta.ToString();
                    PontuaçãoCategoria.FechamentoEMédiaRápida = 2;
                }
                else if (HistóricoPapel[HistóricoPapel.Count].PreçoFechamento < (1-tolerânciaIgualdade) * MMERápida.ListaDaMME[MMERápida.ListaDaMME.Count])                 //Se Abertura < 0,995*MMELenta (abertura atual 0,5% menor que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    CategoriaStatus.FechamentoEMédiaRápida = Tendência.Baixa.ToString();
                    PontuaçãoCategoria.FechamentoEMédiaRápida = 0;
                }
                else//Igualdade                                                                                                                          //Se 0,995*MMELenta <= Abertura <= 1,005*MMELenta (fechamento atual entre + ou - 0,5% da abertura atual)
                {
                    CategoriaStatus.FechamentoEMédiaRápida = Tendência.Indefinição.ToString();
                    PontuaçãoCategoria.FechamentoEMédiaRápida = 1;
                }
            }
            else if (CategoriaStatus.Operação == "vendido")
            {
                if (HistóricoPapel[HistóricoPapel.Count].PreçoFechamento > (1+tolerânciaIgualdade) * MMERápida.ListaDaMME[MMERápida.ListaDaMME.Count])                  //Se Abertura > 1,005*MMELenta (abertura atual 0,5% maior que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    CategoriaStatus.FechamentoEMédiaRápida = Tendência.Alta.ToString();
                    PontuaçãoCategoria.FechamentoEMédiaRápida = 0;
                }
                else if (HistóricoPapel[HistóricoPapel.Count].PreçoFechamento < (1-tolerânciaIgualdade) * MMERápida.ListaDaMME[MMERápida.ListaDaMME.Count])           //Se Abertura < 0,995*MMELenta (abertura atual 0,5% menor que a MMELenta) // Comparo com o ultimo elemento da MMELenta
                {
                    CategoriaStatus.FechamentoEMédiaRápida = Tendência.Baixa.ToString();
                    PontuaçãoCategoria.FechamentoEMédiaRápida = 2;
                }
                else//Igualdade                                                                                                                           //Se 0,995*MMELenta <= Abertura <= 1,005*MMELenta (fechamento atual entre + ou - 0,5% da abertura atual)
                {
                    CategoriaStatus.FechamentoEMédiaRápida = Tendência.Indefinição.ToString();
                    PontuaçãoCategoria.FechamentoEMédiaRápida = 1;
                }
            }
            else
            {
                MessageBox.Show("Tipo de operação não informada", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void CandelEMédias(List<Papeis> HistóricoPapel, int períodoMMELenta, int períodoMMEIntermediária, int períodoMMERápida) //15o método: PROXIMIDADE DO CANDEL & MÉDIAS MÓVEIS (Muito acima, pouco acima, próximo das médias, pouco abaixo, Muito abaixo)
        {
            MédiaMóvelExponencial MMELenta = new MédiaMóvelExponencial(HistóricoPapel, períodoMMELenta);
            MédiaMóvelExponencial MMEIntermediária = new MédiaMóvelExponencial(HistóricoPapel, períodoMMEIntermediária);
            MédiaMóvelExponencial MMERápida = new MédiaMóvelExponencial(HistóricoPapel, períodoMMERápida);

            double preçoMédio = HistóricoPapel[HistóricoPapel.Count].PreçoMédio;

            double Média_das_MME = (MMERápida.ListaDaMME[MMERápida.ListaDaMME.Count] + MMELenta.ListaDaMME[MMELenta.ListaDaMME.Count] + MMEIntermediária.ListaDaMME[MMEIntermediária.ListaDaMME.Count]) / 3;

            //Intervalo de tolerância para análise
            double muito = 0.01;                    //+ ou - 1% de tolerância
            double pouco = 0.005;                   //+ ou - 0,5% de tolerância
            if (CategoriaStatus.Operação =="comprado")
            {
            #region"Comparar PreçoMédio com a Média_das_MME"
            
            if (preçoMédio > (1 + muito) * Média_das_MME || preçoMédio < (1 - muito) * Média_das_MME)     //testa se o preço média é 1% maior ou 1% menor que a média das MME
            {
                if (preçoMédio > (1 + muito) * Média_das_MME)
                {
                    CategoriaStatus.CandelEMédias = "muito acima";
                    PontuaçãoCategoria.CandelEMédias = 0;
                }
                else
                {
                    CategoriaStatus.CandelEMédias = "muito abaixo";
                    PontuaçãoCategoria.CandelEMédias = 0.5;
                }
            }
            if ((preçoMédio > (1+pouco)*Média_das_MME && preçoMédio < (1+muito)*Média_das_MME)||(preçoMédio > (1-muito)*Média_das_MME && preçoMédio < (1-pouco)*Média_das_MME))
            {
                if (preçoMédio > (1+pouco)*Média_das_MME && preçoMédio < (1+muito)*Média_das_MME)
                {
                    CategoriaStatus.CandelEMédias = "pouco acima";
                    PontuaçãoCategoria.CandelEMédias = 1.5;
                }
                else
                {
                    CategoriaStatus.CandelEMédias = "pouco abaixo";
                    PontuaçãoCategoria.CandelEMédias = 1.5;
                }
            }
            if (preçoMédio > (1 - pouco)*Média_das_MME && preçoMédio < (1 + pouco)*Média_das_MME)
            {
                PontuaçãoCategoria.CandelEMédias = 3;
                CategoriaStatus.CandelEMédias = "próximo das médias";
            }
            #endregion   
            }
            else if (CategoriaStatus.Operação =="vendido")
            {
            #region"Comparar PreçoMédio com a Média_das_MME"

                if (preçoMédio > (1 + muito) * Média_das_MME || preçoMédio < (1 - muito) * Média_das_MME)     //testa se o preço média é 1% maior ou 1% menor que a média das MME
                {
                    if (preçoMédio > (1 + muito) * Média_das_MME)
                    {
                        CategoriaStatus.CandelEMédias = "muito acima";
                        PontuaçãoCategoria.CandelEMédias = 0.5;
                    }
                    else
                    {
                        CategoriaStatus.CandelEMédias = "muito abaixo";
                        PontuaçãoCategoria.CandelEMédias = 0;
                    }
                }
                if ((preçoMédio > (1 + pouco) * Média_das_MME && preçoMédio < (1 + muito) * Média_das_MME) || (preçoMédio > (1 - muito) * Média_das_MME && preçoMédio < (1 - pouco) * Média_das_MME))
                {
                    if (preçoMédio > (1 + pouco) * Média_das_MME && preçoMédio < (1 + muito) * Média_das_MME)
                    {
                        CategoriaStatus.CandelEMédias = "pouco acima";
                        PontuaçãoCategoria.CandelEMédias = 1.5;
                    }
                    else
                    {
                        CategoriaStatus.CandelEMédias = "pouco abaixo";
                        PontuaçãoCategoria.CandelEMédias = 1.5;
                    }
                }
                if (preçoMédio > (1 - pouco) * Média_das_MME && preçoMédio < (1 + pouco) * Média_das_MME)
                {
                    PontuaçãoCategoria.CandelEMédias = 3;
                    CategoriaStatus.CandelEMédias = "próximo das médias";
                }
                #endregion   
            }
            else
            {
                MessageBox.Show("Tipo de operação não informada", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void ComparaMMERápidaLenta(List<Papeis> HistóricoPapel, int períodoMMELenta, int períodoMMERápida)                      //16o método: MÉDIA RÁPIDA vs. MÉDIA LENTA (MMERápida > MMELenta, MMERápida = MMELenta, MMERápida < MMELenta)
        {
            MédiaMóvelExponencial MMERápida = new MédiaMóvelExponencial(HistóricoPapel, períodoMMERápida);
            MédiaMóvelExponencial MMELenta = new MédiaMóvelExponencial(HistóricoPapel, períodoMMELenta);

            if (CategoriaStatus.Operação == "comprado")
            {
                if (MMERápida.ListaDaMME[MMERápida.ListaDaMME.Count] > (1 + tolerânciaIgualdade) * MMELenta.ListaDaMME[MMELenta.ListaDaMME.Count])//MMERápida > MMELenta
                {
                    CategoriaStatus.MédiaRápidaeLenta = Tendência.Alta.ToString();
                    PontuaçãoCategoria.MédiaRápidaeLenta = 2;
                } 
                else if (MMERápida.ListaDaMME[MMERápida.ListaDaMME.Count] < (1 - tolerânciaIgualdade) * MMELenta.ListaDaMME[MMELenta.ListaDaMME.Count])//MMERápida > MMELenta
                {
                    CategoriaStatus.MédiaRápidaeLenta = Tendência.Baixa.ToString();
                    PontuaçãoCategoria.MédiaRápidaeLenta = 0;
                }
                else//Igualdade
                {
                    CategoriaStatus.MédiaRápidaeLenta = Tendência.Indefinição.ToString();
                    PontuaçãoCategoria.MédiaRápidaeLenta = 1;
                }
            } 
            if (CategoriaStatus.Operação == "vendido")
            {
                if (MMERápida.ListaDaMME[MMERápida.ListaDaMME.Count] > (1 + tolerânciaIgualdade) * MMELenta.ListaDaMME[MMELenta.ListaDaMME.Count])//MMERápida > MMELenta
                {
                    CategoriaStatus.MédiaRápidaeLenta = Tendência.Alta.ToString();
                    PontuaçãoCategoria.MédiaRápidaeLenta = 0;
                } 
                else if (MMERápida.ListaDaMME[MMERápida.ListaDaMME.Count] < (1 - tolerânciaIgualdade) * MMELenta.ListaDaMME[MMELenta.ListaDaMME.Count])//MMERápida > MMELenta
                {
                    CategoriaStatus.MédiaRápidaeLenta = Tendência.Baixa.ToString();
                    PontuaçãoCategoria.MédiaRápidaeLenta = 2;
                }
                else//Igualdade
                {
                    CategoriaStatus.MédiaRápidaeLenta = Tendência.Indefinição.ToString();
                    PontuaçãoCategoria.MédiaRápidaeLenta = 1;
                }
            }
            else
            {
                MessageBox.Show("Tipo de operação não informada", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FechamentoBandasDeBollinger(List<Papeis> HistóricoPapel, int períodoBandaBollinger,double desvioMédiaBandaBollinger)                                //17o método: BANDAS DE BOLLINGER (Fechou acima da superior, fechou próxima da superior, entre a superior e a média, próximo da média, entre a média e a inferior, próximo da inferior, fechou abaixo da inferior)
        {
            BandasDeBollinger BandaBollinger = new BandasDeBollinger(HistóricoPapel, períodoBandaBollinger, desvioMédiaBandaBollinger);

            double bandaSuperior = BandaBollinger.BandaSuperior[BandaBollinger.BandaSuperior.Count];
            double bandaInferior = BandaBollinger.BandaInferior[BandaBollinger.BandaInferior.Count];
            double médiaBanda = BandaBollinger.MédiaDaBanda[BandaBollinger.MédiaDaBanda.Count];
            double fechamento = HistóricoPapel[HistóricoPapel.Count].PreçoFechamento;

            if (CategoriaStatus.Operação =="comprado")
            {
                if (fechamento > (1 + tolerânciaIgualdade) * bandaSuperior)
                {
                    CategoriaStatus.FechamentoBandasDeBollinger = "acima da superior";
                    PontuaçãoCategoria.FechamentoBandasDeBollinger = 0;
                }
                else if (fechamento < (1+tolerânciaIgualdade)*bandaSuperior && fechamento > (1-tolerânciaIgualdade)*bandaSuperior) //Na banda superior
                {
                    CategoriaStatus.FechamentoBandasDeBollinger = "próximo da superior";
                    PontuaçãoCategoria.FechamentoBandasDeBollinger = 0.5;
                }
                else if (fechamento > (1 + tolerânciaIgualdade) * médiaBanda && fechamento < (1 - tolerânciaIgualdade) * bandaSuperior)
                {
                    CategoriaStatus.FechamentoBandasDeBollinger = "entre a superior e a média";
                    PontuaçãoCategoria.FechamentoBandasDeBollinger = 2;
                }
                else if (fechamento <(1+tolerânciaIgualdade)*médiaBanda && fechamento > (1-tolerânciaIgualdade)*médiaBanda)
                {
                    CategoriaStatus.FechamentoBandasDeBollinger = "próximo da média";
                    PontuaçãoCategoria.FechamentoBandasDeBollinger = 4;
                }
                else if (fechamento < (1-tolerânciaIgualdade)*médiaBanda && fechamento > (1+tolerânciaIgualdade)*bandaInferior)
                {
                    CategoriaStatus.FechamentoBandasDeBollinger = "entre a inferior e a média";
                    PontuaçãoCategoria.FechamentoBandasDeBollinger = 0.5;
                }
                else if (fechamento < (1+tolerânciaIgualdade)*bandaInferior && fechamento > (1-tolerânciaIgualdade)*bandaInferior)
                {
                    CategoriaStatus.FechamentoBandasDeBollinger = "próximo da inferior";
                    PontuaçãoCategoria.FechamentoBandasDeBollinger = 1;
                }
                else if (fechamento < (1-tolerânciaIgualdade)*bandaInferior)
                {
                    CategoriaStatus.FechamentoBandasDeBollinger = "abaixo da inferior";
                    PontuaçãoCategoria.FechamentoBandasDeBollinger = 1.5;
                }
            }
            else if (CategoriaStatus.Operação =="vendido")
            {
                if (fechamento > (1 + tolerânciaIgualdade) * bandaSuperior)
                {
                    CategoriaStatus.FechamentoBandasDeBollinger = "acima da superior";
                    PontuaçãoCategoria.FechamentoBandasDeBollinger = 1.5;
                }
                else if (fechamento < (1 + tolerânciaIgualdade) * bandaSuperior && fechamento > (1 - tolerânciaIgualdade) * bandaSuperior) //Na banda superior
                {
                    CategoriaStatus.FechamentoBandasDeBollinger = "próximo da superior";
                    PontuaçãoCategoria.FechamentoBandasDeBollinger = 1;
                }
                else if (fechamento > (1 + tolerânciaIgualdade) * médiaBanda && fechamento < (1 - tolerânciaIgualdade) * bandaSuperior)
                {
                    CategoriaStatus.FechamentoBandasDeBollinger = "entre a superior e a média";
                    PontuaçãoCategoria.FechamentoBandasDeBollinger = 0.5;
                }
                else if (fechamento < (1 + tolerânciaIgualdade) * médiaBanda && fechamento > (1 - tolerânciaIgualdade) * médiaBanda)
                {
                    CategoriaStatus.FechamentoBandasDeBollinger = "próximo da média";
                    PontuaçãoCategoria.FechamentoBandasDeBollinger = 4;
                }
                else if (fechamento < (1 - tolerânciaIgualdade) * médiaBanda && fechamento > (1 + tolerânciaIgualdade) * bandaInferior)
                {
                    CategoriaStatus.FechamentoBandasDeBollinger = "entre a inferior e a média";
                    PontuaçãoCategoria.FechamentoBandasDeBollinger = 2;
                }
                else if (fechamento < (1 + tolerânciaIgualdade) * bandaInferior && fechamento > (1 - tolerânciaIgualdade) * bandaInferior)
                {
                    CategoriaStatus.FechamentoBandasDeBollinger = "próximo da inferior";
                    PontuaçãoCategoria.FechamentoBandasDeBollinger = 0.5;
                }
                else if (fechamento < (1 - tolerânciaIgualdade) * bandaInferior)
                {
                    CategoriaStatus.FechamentoBandasDeBollinger = "abaixo da inferior";
                    PontuaçãoCategoria.FechamentoBandasDeBollinger = 0;
                }
            }
            else
            {
                MessageBox.Show("Tipo de operação não informada", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
