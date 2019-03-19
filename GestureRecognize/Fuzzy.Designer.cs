namespace GestureRecognize
{
    partial class Fuzzy
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
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Fuzzy));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.buttonTest = new System.Windows.Forms.Button();
            this.OutChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart = new AForge.Controls.Chart();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.OutChart)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(24, 395);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(75, 23);
            this.buttonTest.TabIndex = 0;
            this.buttonTest.Text = "Test";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // OutChart
            // 
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend1.Name = "Legend1";
            this.OutChart.Legends.Add(legend1);
            this.OutChart.Location = new System.Drawing.Point(12, 26);
            this.OutChart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.OutChart.Name = "OutChart";
            this.OutChart.Size = new System.Drawing.Size(776, 346);
            this.OutChart.TabIndex = 5;
            this.OutChart.Text = "Out Chart";
            // 
            // chart
            // 
            this.chart.Location = new System.Drawing.Point(795, 35);
            this.chart.Margin = new System.Windows.Forms.Padding(4);
            this.chart.Name = "chart";
            this.chart.RangeX = ((AForge.Range)(resources.GetObject("chart.RangeX")));
            this.chart.RangeY = ((AForge.Range)(resources.GetObject("chart.RangeY")));
            this.chart.Size = new System.Drawing.Size(557, 308);
            this.chart.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(114, 395);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Test2";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Fuzzy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1374, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.chart);
            this.Controls.Add(this.OutChart);
            this.Controls.Add(this.buttonTest);
            this.Name = "Fuzzy";
            this.Text = "Fuzzy";
            this.Load += new System.EventHandler(this.Fuzzy_Load);
            ((System.ComponentModel.ISupportInitialize)(this.OutChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.DataVisualization.Charting.Chart OutChart;
        private AForge.Controls.Chart chart;
        private System.Windows.Forms.Button button1;
    }
}