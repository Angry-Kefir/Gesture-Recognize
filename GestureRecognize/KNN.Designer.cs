namespace GestureRecognize
{
    partial class KNN
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
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.OutChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.OutChart)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // OutChart
            // 
            legend2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend2.Name = "Legend1";
            this.OutChart.Legends.Add(legend2);
            this.OutChart.Location = new System.Drawing.Point(12, 11);
            this.OutChart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.OutChart.Name = "OutChart";
            this.OutChart.Size = new System.Drawing.Size(776, 346);
            this.OutChart.TabIndex = 6;
            this.OutChart.Text = "Out Chart";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(44, 384);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // KNN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.OutChart);
            this.Name = "KNN";
            this.Text = "KNN";
            this.Load += new System.EventHandler(this.KNN_Load);
            ((System.ComponentModel.ISupportInitialize)(this.OutChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.DataVisualization.Charting.Chart OutChart;
        private System.Windows.Forms.Button button1;
    }
}