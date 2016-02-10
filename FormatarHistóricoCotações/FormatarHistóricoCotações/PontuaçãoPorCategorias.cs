using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormatarHistóricoCotações
{
    class PontuaçãoPorCategorias
    {
        public double Operação;                                 //1o método: Tipo de operação (comprado, vendido)

        public double PreçoDoAtivo;                             //2o método: Preço do ativo

        public double VerificaPreço;                            //3o método: Verificar preço (pode comprar?)

        public double TendênciaÍndice;                          //4o método: TENDÊNCIA DO ÍNDICE ANALISADO (IBOV -> alta, indefinição, baixa)

        public double TendeênciaCandel;                         //5o método: CANDEL TENDÊNCIA (alta, indefinição, baixa) (padrão do candel -> martel, enforcado, estrela cadente, etc.)

        public double TendênciaPapel;                           //6o método: TENDÊNDIA INTERMEDIÁRIA ou do Papel (alta, indefinição, baixa)

        public double FechamentoEAberturarAnteriorPapel;        //7o método: Compar o FECHAMENTO ATUAL e a AERTURA ANTERIOR (Fec > Abe, Fec = Abe, Fec < Abe)

        public double FechamentoCandel;                         //8o método: FECHAMENTO DO CANDEL (Fechou em alta, próximo da abertura, fechou em baixa)

        public double AberturaEMédiaLenta;                      //9o método: ABERTURA EM RELAÇÃO A  MÉDIA LENTA (Abe > MMELenta, Abe = MMELenta, Abe < MMELenta)

        public double AberturaEMédiaIntermediária;              //10o método: ABERTURA EM RELAÇÃO A  MÉDIA INTERMEDIÁRIA (Abe > MMEInter, Abe = MMEInter, Abe < MMEInter)

        public double AberturaEMédiaRápida;                     //11o método: ABERTURA EM RELAÇÃO A  MÉDIA RÁPIDA (Abe > MMERápida, Abe = MMERápida, Abe < MMERápida)

        public double FechamentoEMédiaLenta;                    //12o método: FECHAMENTO EM RELAÇÃO A  MÉDIA LENTA (Fec > MMELenta, Fec = MMELenta, Fec < MMELenta)

        public double FechamentoEMédiaIntermediária;            //13o método: FECHAMENTO EM RELAÇÃO A  MÉDIA INTERMEDIÁRIA (Fec > MMEInter, Fec = MMEInter, Fec < MMEInter)

        public double FechamentoEMédiaRápida;                   //14o método: FECHAMENTO EM RELAÇÃO A  MÉDIA RÁPIDA (Fec > MMERÁPIDA, Fec = MMERÁPIDA, Fec < MMERÁPIDA)

        public double CandelEMédias;                            //15o método: PROXIMIDADE DO CANDEL & MÉDIAS MÓVEIS (Muito acima, pouco acima, próximo das médias, pouco abaixo, Muito abaixo)

        public double MédiaRápidaeLenta;                        //16o método: MÉDIA RÁPIDA vs. MÉDIA LENTA (MMERápida > MMELenta, MMERápida = MMELenta, MMERápida < MMELenta)

        public double FechamentoBandasDeBollinger;              //17o método: BANDAS DE BOLLINGER (Fechou acima da superior, fechou próxima da superior, entre a superior e a média, próximo da média, entre a média e a inferior, próximo da inferior, fechou abaixo da inferior)

        public double PicosMACD;                                //18o método: HISTOGRAMA MACD (PICOS) (Inconsistência positiva, Indefinição, Inconsistência negativa)

        public double HistogramaMACD;                           //19o método: HISTOGRAMA MACD (Muito sobre comprado, sobre comprado, indefinido, sobre vendido, muito sobre vendido)

        public double ValorIFR;                                 //20o método: IFR (IFR>65% , 45< IFR<65%, IFR<45% )

        public double VolumeFinanceiro;                         //21o método: Volume financeiro (Acima da média, próximo da média, abaixo da média)

        public double NúmeroDeOperações;                        //22o método: Nº DE OPERAÇÃO (>2000, 1000< No < 2000, <1000) Total de negócios

        public double ForçaDaTendênciaDMI;                      //23o método: DMI ( FORÇA DA TENDÊNCIA ) (Muito aberto e positivo, pouco aberto e positivo, próximas, pouco aberto e negativo, muito aberto e negativo)

        public double DireçãoDaTendênciaDMI;                    //24o método: DMI ( DIREÇÃO DA TENDÊNCIA ) (ADX < 30 e subindo, ADX > 30 e subindo, ADX caindo)
    }
}
