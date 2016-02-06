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

        public List<double> PontuaçãoCategoria { get; private set; } // São categorias para análise e cada categoria tem um peso correspondente
        public List<string> StatusCategoria { get; private set; }    // Cada categoria tem um status. Exemplo: A categoria "HISTOGRAMA MACD (PICOS)" pode ter 3 status (INCONSSITÊNCIA POSITIVA,INDEFINIÇÃO,INCONSSITÊNCIA NEGATIVA)

        public AnálisePapel(List<Papeis> HistóricoPapel, string operação)
        {//Chamar todos os privados métodos para alimentar as listas públicas
            
            //1o método: Tipo de operação (comprado, vendido)
            TipoDeOperação(operação);

            //2o método: Preço do ativo
            PreçoDoAtivo(HistóricoPapel);

            //3o método: Verificar preço (pode comprar?)

            //4o método: TENDÊNCIA DO ÍNDICE ANALISADO (IBOV -> alta, indefinição, baixa)

            //5o método: CANDEL TENDÊNCIA (alta, indefinição, baixa) (padrão do candel -> martel, enforcado, estrela cadente, etc.)

            //6o método: TENDÊNDIA INTERMEDIÁRIA ou do Papel (alta, indefinição, baixa)

            //7o método: Compar o FECHAMENTO ATUAL e a AERTURA ANTERIOR (Fec > Abe, Fec = Abe, Fec < Abe)

            //8o método: FECHAMENTO DO CANDEL (Fechou em alta, próximo da abertura, fechou em baixa)

            //9o método: ABERTURA EM RELAÇÃO A  MÉDIA LENTA (Abe > MMELenta, Abe = MMELenta, Abe < MMELenta)

            //10o método: ABERTURA EM RELAÇÃO A  MÉDIA INTERMEDIÁRIA (Abe > MMEInter, Abe = MMEInter, Abe < MMEInter)

            //11o método: ABERTURA EM RELAÇÃO A  MÉDIA RÁPIDA (Abe > MMERápida, Abe = MMERápida, Abe < MMERápida)

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

        private void TipoDeOperação(string operação) //1o método: Tipo de operação (comprado, vendido)
        {
            StatusCategoria.Add(operação); //Primeiro elemento da lista
        }

        private void PreçoDoAtivo(List<Papeis> HistóricoPapel)
        {
            throw new NotImplementedException();
        }
    }
}
