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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.ConcatenaArquivos = new System.Windows.Forms.Button();
            this.gBoxTipoSaída = new System.Windows.Forms.GroupBox();
            this.radioButtonTXT = new System.Windows.Forms.RadioButton();
            this.radioButtonXML = new System.Windows.Forms.RadioButton();
            this.listBoxPapeis = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.testarConsultaXML = new System.Windows.Forms.Button();
            this.dvgXML = new System.Windows.Forms.DataGridView();
            this.grafico1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.gráfico2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.gráfico3 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.gBoxTipoSaída.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvgXML)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grafico1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gráfico2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gráfico3)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // ConcatenaArquivos
            // 
            this.ConcatenaArquivos.Location = new System.Drawing.Point(12, 334);
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
            this.gBoxTipoSaída.Location = new System.Drawing.Point(12, 269);
            this.gBoxTipoSaída.Name = "gBoxTipoSaída";
            this.gBoxTipoSaída.Size = new System.Drawing.Size(147, 59);
            this.gBoxTipoSaída.TabIndex = 3;
            this.gBoxTipoSaída.TabStop = false;
            this.gBoxTipoSaída.Text = "Tipo de arquivo de saída";
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
            // radioButtonXML
            // 
            this.radioButtonXML.AutoSize = true;
            this.radioButtonXML.Checked = true;
            this.radioButtonXML.Location = new System.Drawing.Point(18, 28);
            this.radioButtonXML.Name = "radioButtonXML";
            this.radioButtonXML.Size = new System.Drawing.Size(47, 17);
            this.radioButtonXML.TabIndex = 0;
            this.radioButtonXML.TabStop = true;
            this.radioButtonXML.Text = "*.xml";
            this.radioButtonXML.UseVisualStyleBackColor = true;
            // 
            // listBoxPapeis
            // 
            this.listBoxPapeis.FormattingEnabled = true;
            this.listBoxPapeis.Location = new System.Drawing.Point(12, 25);
            this.listBoxPapeis.Name = "listBoxPapeis";
            this.listBoxPapeis.Size = new System.Drawing.Size(147, 238);
            this.listBoxPapeis.TabIndex = 4;
            this.listBoxPapeis.SelectedIndexChanged += new System.EventHandler(this.listBoxPapeis_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "label1";
            // 
            // testarConsultaXML
            // 
            this.testarConsultaXML.Location = new System.Drawing.Point(192, 334);
            this.testarConsultaXML.Name = "testarConsultaXML";
            this.testarConsultaXML.Size = new System.Drawing.Size(123, 48);
            this.testarConsultaXML.TabIndex = 6;
            this.testarConsultaXML.Text = "Testar consulta *.xml";
            this.testarConsultaXML.UseVisualStyleBackColor = true;
            this.testarConsultaXML.Click += new System.EventHandler(this.ConsultaXML_Click);
            // 
            // dvgXML
            // 
            this.dvgXML.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dvgXML.Location = new System.Drawing.Point(192, 25);
            this.dvgXML.Name = "dvgXML";
            this.dvgXML.Size = new System.Drawing.Size(281, 238);
            this.dvgXML.TabIndex = 7;
            // 
            // grafico1
            // 
            chartArea1.AxisX.LabelAutoFitStyle = ((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles)(((((((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.IncreaseFont | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont) 
            | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.StaggeredLabels) 
            | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.LabelsAngleStep30) 
            | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.LabelsAngleStep45) 
            | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.LabelsAngleStep90) 
            | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.WordWrap)));
            chartArea1.AxisY.IsStartedFromZero = false;
            chartArea1.AxisY.MajorTickMark.Enabled = false;
            chartArea1.Name = "ChartArea1";
            this.grafico1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            legend1.Position.Auto = false;
            legend1.Position.Height = 10.25641F;
            legend1.Position.Width = 18.44864F;
            legend1.Position.X = 10F;
            legend1.Position.Y = 5F;
            this.grafico1.Legends.Add(legend1);
            this.grafico1.Location = new System.Drawing.Point(511, 25);
            this.grafico1.Name = "grafico1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
            series1.IsXValueIndexed = true;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series1.YValuesPerPoint = 4;
            series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.grafico1.Series.Add(series1);
            this.grafico1.Size = new System.Drawing.Size(478, 235);
            this.grafico1.TabIndex = 9;
            this.grafico1.Text = "chart1";
            title1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            title1.ForeColor = System.Drawing.Color.Blue;
            title1.Name = "Title1";
            title1.Text = "Ação";
            this.grafico1.Titles.Add(title1);
            // 
            // gráfico2
            // 
            chartArea2.Name = "ChartArea1";
            chartArea2.Position.Auto = false;
            chartArea2.Position.Height = 85F;
            chartArea2.Position.Width = 94F;
            chartArea2.Position.X = 3F;
            chartArea2.Position.Y = 15F;
            this.gráfico2.ChartAreas.Add(chartArea2);
            legend2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            legend2.IsTextAutoFit = false;
            legend2.Name = "Legend1";
            legend2.Position.Auto = false;
            legend2.Position.Height = 23.76238F;
            legend2.Position.Width = 17.40042F;
            legend2.Position.X = 10F;
            this.gráfico2.Legends.Add(legend2);
            this.gráfico2.Location = new System.Drawing.Point(511, 266);
            this.gráfico2.Name = "gráfico2";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "MACD";
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Date;
            this.gráfico2.Series.Add(series2);
            this.gráfico2.Size = new System.Drawing.Size(478, 143);
            this.gráfico2.TabIndex = 10;
            this.gráfico2.Text = "chart1";
            // 
            // gráfico3
            // 
            chartArea3.Name = "ChartArea1";
            chartArea3.Position.Auto = false;
            chartArea3.Position.Height = 85F;
            chartArea3.Position.Width = 94F;
            chartArea3.Position.X = 3F;
            chartArea3.Position.Y = 15F;
            this.gráfico3.ChartAreas.Add(chartArea3);
            legend3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            legend3.IsTextAutoFit = false;
            legend3.Name = "Legend1";
            legend3.Position.Auto = false;
            legend3.Position.Height = 23.76238F;
            legend3.Position.Width = 17.40042F;
            legend3.Position.X = 10F;
            this.gráfico3.Legends.Add(legend3);
            this.gráfico3.Location = new System.Drawing.Point(511, 415);
            this.gráfico3.Name = "gráfico3";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "VOLUME";
            series3.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Date;
            this.gráfico3.Series.Add(series3);
            this.gráfico3.Size = new System.Drawing.Size(478, 103);
            this.gráfico3.TabIndex = 11;
            this.gráfico3.Text = "chart1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1001, 530);
            this.Controls.Add(this.gráfico3);
            this.Controls.Add(this.gráfico2);
            this.Controls.Add(this.grafico1);
            this.Controls.Add(this.dvgXML);
            this.Controls.Add(this.testarConsultaXML);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxPapeis);
            this.Controls.Add(this.gBoxTipoSaída);
            this.Controls.Add(this.ConcatenaArquivos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Form1";
            this.Text = "METE (Modelo e Estratégia Trade Erasmo)";
            this.gBoxTipoSaída.ResumeLayout(false);
            this.gBoxTipoSaída.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvgXML)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grafico1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gráfico2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gráfico3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button ConcatenaArquivos;
        private System.Windows.Forms.GroupBox gBoxTipoSaída;
        private System.Windows.Forms.RadioButton radioButtonTXT;
        private System.Windows.Forms.RadioButton radioButtonXML;
        private System.Windows.Forms.ListBox listBoxPapeis;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button testarConsultaXML;
        private System.Windows.Forms.DataGridView dvgXML;
        private System.Windows.Forms.DataVisualization.Charting.Chart grafico1;
        private System.Windows.Forms.DataVisualization.Charting.Chart gráfico2;
        private System.Windows.Forms.DataVisualization.Charting.Chart gráfico3;
    }
}

