﻿namespace FormatarHistóricoCotações
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
            this.gBoxTipoSaída = new System.Windows.Forms.GroupBox();
            this.radioButtonXML = new System.Windows.Forms.RadioButton();
            this.radioButtonTXT = new System.Windows.Forms.RadioButton();
            this.gBoxTipoSaída.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // ConcatenaArquivos
            // 
            this.ConcatenaArquivos.Location = new System.Drawing.Point(18, 105);
            this.ConcatenaArquivos.Name = "ConcatenaArquivos";
            this.ConcatenaArquivos.Size = new System.Drawing.Size(147, 48);
            this.ConcatenaArquivos.TabIndex = 2;
            this.ConcatenaArquivos.Text = "Gerar Histórico Completo";
            this.ConcatenaArquivos.UseVisualStyleBackColor = true;
            this.ConcatenaArquivos.Click += new System.EventHandler(this.ConcatenaArquivos_Click);
            // 
            // gBoxTipoSaída
            // 
            this.gBoxTipoSaída.Controls.Add(this.radioButtonTXT);
            this.gBoxTipoSaída.Controls.Add(this.radioButtonXML);
            this.gBoxTipoSaída.Location = new System.Drawing.Point(18, 40);
            this.gBoxTipoSaída.Name = "gBoxTipoSaída";
            this.gBoxTipoSaída.Size = new System.Drawing.Size(147, 59);
            this.gBoxTipoSaída.TabIndex = 3;
            this.gBoxTipoSaída.TabStop = false;
            this.gBoxTipoSaída.Text = "Tipo de arquivo de saída";
            // 
            // radioButtonXML
            // 
            this.radioButtonXML.AutoSize = true;
            this.radioButtonXML.Location = new System.Drawing.Point(18, 28);
            this.radioButtonXML.Name = "radioButtonXML";
            this.radioButtonXML.Size = new System.Drawing.Size(47, 17);
            this.radioButtonXML.TabIndex = 0;
            this.radioButtonXML.TabStop = true;
            this.radioButtonXML.Text = "*.xml";
            this.radioButtonXML.UseVisualStyleBackColor = true;
            // 
            // radioButtonTXT
            // 
            this.radioButtonTXT.AutoSize = true;
            this.radioButtonTXT.Location = new System.Drawing.Point(81, 28);
            this.radioButtonTXT.Name = "radioButtonTXT";
            this.radioButtonTXT.Size = new System.Drawing.Size(43, 17);
            this.radioButtonTXT.TabIndex = 1;
            this.radioButtonTXT.TabStop = true;
            this.radioButtonTXT.Text = "*.txt";
            this.radioButtonTXT.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 257);
            this.Controls.Add(this.gBoxTipoSaída);
            this.Controls.Add(this.ConcatenaArquivos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "METE (Modelo e Estratégia Trade Erasmo)";
            this.gBoxTipoSaída.ResumeLayout(false);
            this.gBoxTipoSaída.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button ConcatenaArquivos;
        private System.Windows.Forms.GroupBox gBoxTipoSaída;
        private System.Windows.Forms.RadioButton radioButtonTXT;
        private System.Windows.Forms.RadioButton radioButtonXML;
    }
}

