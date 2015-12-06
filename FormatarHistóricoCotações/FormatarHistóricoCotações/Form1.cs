using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace FormatarHistóricoCotações
{
    public partial class Form1 : Form
    {
        //string CaminhoDoArquivo;
        string CaminhoDoDiretorio;
        //string CaminhoParaSalvarArqivo;
        bool tipoDeSaida;
        HistóricoCotação histórico; 
        
        public Form1()
        {
            InitializeComponent();
            histórico = new HistóricoCotação();
        }

        private void ConcatenaArquivos_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = @"C:\Users\Erasmo\Documents\GitHub\AnaliseMercado";
            openFileDialog1.Filter = "Arquivos de Texto (*.txt)|*.txt";
            openFileDialog1.FileName = "AAAA.txt";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = false;
            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                FileInfo infoArquivo = new FileInfo(Path.GetFullPath(openFileDialog1.FileName));
                CaminhoDoDiretorio = infoArquivo.DirectoryName;
            }

            if (radioButtonTXT.Checked)
            {
                tipoDeSaida = true;
            }else if (radioButtonXML.Checked)
            {
                tipoDeSaida = false;
            }

            histórico.ConcatenaArquivos(CaminhoDoDiretorio,tipoDeSaida);
        }
    }
}
