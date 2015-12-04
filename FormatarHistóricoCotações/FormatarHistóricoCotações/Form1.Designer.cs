namespace FormatarHistóricoCotações
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.ConcatenaArquivos = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // ConcatenaArquivos
            // 
            this.ConcatenaArquivos.Location = new System.Drawing.Point(237, 189);
            this.ConcatenaArquivos.Name = "ConcatenaArquivos";
            this.ConcatenaArquivos.Size = new System.Drawing.Size(117, 48);
            this.ConcatenaArquivos.TabIndex = 2;
            this.ConcatenaArquivos.Text = "Concatena arquivos Históricos";
            this.ConcatenaArquivos.UseVisualStyleBackColor = true;
            this.ConcatenaArquivos.Click += new System.EventHandler(this.ConcatenaArquivos_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 249);
            this.Controls.Add(this.ConcatenaArquivos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "METE (Modelo e Estratégia Trade Erasmo)";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button ConcatenaArquivos;
    }
}

