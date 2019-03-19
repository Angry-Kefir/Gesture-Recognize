namespace GestureRecognize
{
    partial class Kmeans
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
            this.testButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.coupleButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.loadDataButton = new System.Windows.Forms.Button();
            this.calcErrorButton = new System.Windows.Forms.Button();
            this.pcaButton = new System.Windows.Forms.Button();
            this.cntrMassButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // testButton
            // 
            this.testButton.Location = new System.Drawing.Point(159, 302);
            this.testButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(61, 28);
            this.testButton.TabIndex = 0;
            this.testButton.Text = "Test";
            this.testButton.UseVisualStyleBackColor = true;
            this.testButton.Click += new System.EventHandler(this.testButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(227, 28);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(709, 294);
            this.textBox1.TabIndex = 1;
            // 
            // coupleButton
            // 
            this.coupleButton.Enabled = false;
            this.coupleButton.Location = new System.Drawing.Point(13, 267);
            this.coupleButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.coupleButton.Name = "coupleButton";
            this.coupleButton.Size = new System.Drawing.Size(139, 28);
            this.coupleButton.TabIndex = 2;
            this.coupleButton.Text = "Для пары жестов";
            this.coupleButton.UseVisualStyleBackColor = true;
            this.coupleButton.Click += new System.EventHandler(this.coupleButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // loadDataButton
            // 
            this.loadDataButton.Enabled = false;
            this.loadDataButton.Location = new System.Drawing.Point(13, 302);
            this.loadDataButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.loadDataButton.Name = "loadDataButton";
            this.loadDataButton.Size = new System.Drawing.Size(139, 28);
            this.loadDataButton.TabIndex = 3;
            this.loadDataButton.Text = "Загрузка данных";
            this.loadDataButton.UseVisualStyleBackColor = true;
            this.loadDataButton.Click += new System.EventHandler(this.loadDataButton_Click);
            // 
            // calcErrorButton
            // 
            this.calcErrorButton.Location = new System.Drawing.Point(13, 28);
            this.calcErrorButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.calcErrorButton.Name = "calcErrorButton";
            this.calcErrorButton.Size = new System.Drawing.Size(139, 28);
            this.calcErrorButton.TabIndex = 4;
            this.calcErrorButton.Text = "Расчет ошибок";
            this.calcErrorButton.UseVisualStyleBackColor = true;
            this.calcErrorButton.Click += new System.EventHandler(this.calcErrorButton_Click);
            // 
            // pcaButton
            // 
            this.pcaButton.Enabled = false;
            this.pcaButton.Location = new System.Drawing.Point(13, 218);
            this.pcaButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pcaButton.Name = "pcaButton";
            this.pcaButton.Size = new System.Drawing.Size(139, 43);
            this.pcaButton.TabIndex = 5;
            this.pcaButton.Text = "Метод главных компонент";
            this.pcaButton.UseVisualStyleBackColor = true;
            this.pcaButton.Click += new System.EventHandler(this.pcaButton_Click);
            // 
            // cntrMassButton
            // 
            this.cntrMassButton.Location = new System.Drawing.Point(13, 63);
            this.cntrMassButton.Name = "cntrMassButton";
            this.cntrMassButton.Size = new System.Drawing.Size(139, 28);
            this.cntrMassButton.TabIndex = 6;
            this.cntrMassButton.Text = "Центры масс";
            this.cntrMassButton.UseVisualStyleBackColor = true;
            this.cntrMassButton.Click += new System.EventHandler(this.cntrMassButton_Click);
            // 
            // Kmeans
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(972, 342);
            this.Controls.Add(this.cntrMassButton);
            this.Controls.Add(this.pcaButton);
            this.Controls.Add(this.calcErrorButton);
            this.Controls.Add(this.loadDataButton);
            this.Controls.Add(this.coupleButton);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.testButton);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Kmeans";
            this.Text = "Kmeans";
            this.Load += new System.EventHandler(this.Kmeans_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button testButton;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button coupleButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button loadDataButton;
        private System.Windows.Forms.Button calcErrorButton;
        private System.Windows.Forms.Button pcaButton;
        private System.Windows.Forms.Button cntrMassButton;
    }
}