using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormatarHistóricoCotações
{
    class Categorias
    {

        public string Operação;                                 //1o método: Tipo de operação (comprado, vendido)

        public double PreçoDoAtivo;                             //2o método: Preço do ativo

        public bool VerificaPreço;                              //3o método: Verificar preço (pode comprar?)

        public string TendênciaÍndice;                          //4o método: TENDÊNCIA DO ÍNDICE ANALISADO (IBOV -> alta, indefinição, baixa)

        public string TendênciaCandel;                          //5o método: CANDEL TENDÊNCIA (alta, indefinição, baixa) (padrão do candel -> martel, enforcado, estrela cadente, etc.)

        public string TendênciaPapel;                           //6o método: TENDÊNDIA INTERMEDIÁRIA ou do Papel (alta, indefinição, baixa)

        public string FechamentoEAberturarAnteriorPapel;        //7o método: Compar o FECHAMENTO ATUAL e a AERTURA ANTERIOR (Fec > Abe, Fec = Abe, Fec < Abe)

        public string FechamentoCandel;                         //8o método: FECHAMENTO DO CANDEL (Fechou em alta, próximo da abertura, fechou em baixa)

        public string AberturaEMédiaLenta;                      //9o método: ABERTURA EM RELAÇÃO A  MÉDIA LENTA (Abe > MMELenta, Abe = MMELenta, Abe < MMELenta)

        public string AberturaEMédiaIntermediária;              //10o método: ABERTURA EM RELAÇÃO A  MÉDIA INTERMEDIÁRIA (Abe > MMEInter, Abe = MMEInter, Abe < MMEInter)

        public string AberturaEMédiaRápida;                     //11o método: ABERTURA EM RELAÇÃO A  MÉDIA RÁPIDA (Abe > MMERápida, Abe = MMERápida, Abe < MMERápida)

        public string FechamentoEMédiaLenta;                    //12o método: FECHAMENTO EM RELAÇÃO A  MÉDIA LENTA (Fec > MMELenta, Fec = MMELenta, Fec < MMELenta)

        public string FechamentoEMédiaIntermediária;            //13o método: FECHAMENTO EM RELAÇÃO A  MÉDIA INTERMEDIÁRIA (Fec > MMEInter, Fec = MMEInter, Fec < MMEInter)

        public string FechamentoEMédiaRápida;                   //14o método: FECHAMENTO EM RELAÇÃO A  MÉDIA RÁPIDA (Fec > MMERÁPIDA, Fec = MMERÁPIDA, Fec < MMERÁPIDA)

        public string CandelEMédias;                            //15o método: PROXIMIDADE DO CANDEL & MÉDIAS MÓVEIS (Muito acima, pouco acima, próximo das médias, pouco abaixo, Muito abaixo)

        public string MédiaRápidaeLenta;                        //16o método: MÉDIA RÁPIDA vs. MÉDIA LENTA (MMERápida > MMELenta, MMERápida = MMELenta, MMERápida < MMELenta)

        public string FechamentoBandasDeBollinger;              //17o método: BANDAS DE BOLLINGER (Fechou acima da superior, fechou próxima da superior, entre a superior e a média, próximo da média, entre a média e a inferior, próximo da inferior, fechou abaixo da inferior)

        public string PicosMACD;                                //18o método: HISTOGRAMA MACD (PICOS) (Inconsistência positiva, Indefinição, Inconsistência negativa)

        public string HistogramaMACD;                           //19o método: HISTOGRAMA MACD (Muito sobre comprado, sobre comprado, indefinido, sobre vendido, muito sobre vendido)

        public double ValorIFR;                                 //20o método: IFR (IFR>65% , 45< IFR<65%, IFR<45% )

        public string VolumeFinanceiro;                         //21o método: Volume financeiro (Acima da média, próximo da média, abaixo da média)

        public double NúmeroDeOperações;                        //22o método: Nº DE OPERAÇÃO (>2000, 1000< No < 2000, <1000)

        public string ForçaDaTendênciaDMI;                      //23o método: DMI ( FORÇA DA TENDÊNCIA ) (Muito aberto e positivo, pouco aberto e positivo, próximas, pouco aberto e negativo, muito aberto e negativo)

        public string DireçãoDaTendênciaDMI;                    //24o método: DMI ( DIREÇÃO DA TENDÊNCIA ) (ADX < 30 e subindo, ADX > 30 e subindo, ADX caindo)
    }
}
