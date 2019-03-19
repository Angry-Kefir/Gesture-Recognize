namespace GestureRecognize
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.loadButton = new System.Windows.Forms.Button();
            this.fuzzyButton = new System.Windows.Forms.Button();
            this.AnalysisButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.kmeansButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.countDataset = new System.Windows.Forms.NumericUpDown();
            this.knnButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.countDataset)).BeginInit();
            this.SuspendLayout();
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(12, 11);
            this.loadButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(83, 48);
            this.loadButton.TabIndex = 4;
            this.loadButton.Text = "Ввод данных";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.LoadButtonClick);
            // 
            // fuzzyButton
            // 
            this.fuzzyButton.Location = new System.Drawing.Point(195, 79);
            this.fuzzyButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.fuzzyButton.Name = "fuzzyButton";
            this.fuzzyButton.Size = new System.Drawing.Size(83, 48);
            this.fuzzyButton.TabIndex = 19;
            this.fuzzyButton.Text = "НЛВ";
            this.fuzzyButton.UseVisualStyleBackColor = true;
            this.fuzzyButton.Click += new System.EventHandler(this.Fuzzybutton_Click);
            // 
            // AnalysisButton
            // 
            this.AnalysisButton.Location = new System.Drawing.Point(16, 79);
            this.AnalysisButton.Margin = new System.Windows.Forms.Padding(4);
            this.AnalysisButton.Name = "AnalysisButton";
            this.AnalysisButton.Size = new System.Drawing.Size(83, 48);
            this.AnalysisButton.TabIndex = 20;
            this.AnalysisButton.Text = "Анализ данных";
            this.AnalysisButton.UseVisualStyleBackColor = true;
            this.AnalysisButton.Click += new System.EventHandler(this.AnalysisButtonClick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // kmeansButton
            // 
            this.kmeansButton.Location = new System.Drawing.Point(105, 79);
            this.kmeansButton.Margin = new System.Windows.Forms.Padding(4);
            this.kmeansButton.Name = "kmeansButton";
            this.kmeansButton.Size = new System.Drawing.Size(83, 48);
            this.kmeansButton.TabIndex = 21;
            this.kmeansButton.Text = "Метод k-средних";
            this.kmeansButton.UseVisualStyleBackColor = true;
            this.kmeansButton.Click += new System.EventHandler(this.kmeansButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(104, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 17);
            this.label1.TabIndex = 23;
            this.label1.Text = "Число наборов данных:";
            // 
            // countDataset
            // 
            this.countDataset.Location = new System.Drawing.Point(105, 34);
            this.countDataset.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.countDataset.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.countDataset.Name = "countDataset";
            this.countDataset.Size = new System.Drawing.Size(93, 22);
            this.countDataset.TabIndex = 22;
            this.countDataset.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // knnButton
            // 
            this.knnButton.Location = new System.Drawing.Point(105, 143);
            this.knnButton.Name = "knnButton";
            this.knnButton.Size = new System.Drawing.Size(83, 48);
            this.knnButton.TabIndex = 24;
            this.knnButton.Text = "Метод k соседей";
            this.knnButton.UseVisualStyleBackColor = true;
            this.knnButton.Click += new System.EventHandler(this.knnButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 212);
            this.Controls.Add(this.knnButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.countDataset);
            this.Controls.Add(this.kmeansButton);
            this.Controls.Add(this.AnalysisButton);
            this.Controls.Add(this.fuzzyButton);
            this.Controls.Add(this.loadButton);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.countDataset)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Button fuzzyButton;
        private System.Windows.Forms.Button AnalysisButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button kmeansButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown countDataset;
        private System.Windows.Forms.Button knnButton;
    }
}

