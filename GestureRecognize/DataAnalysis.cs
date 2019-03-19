using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GestureRecognize
{
    public partial class DataAnalysis : Form
    {
        Data[,] data;

        public DataAnalysis(ref Data[,] data)
        {
            this.data = data;

            InitializeComponent();           
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            double minData = -0.005d, maxData = 0.005d,
                minRMS = -0.0001d, maxRMS = 0.001d, 
                minTurn = 0d, maxTurn = 140d;
            // 1 канал
            DataChart1.ChartAreas.Add("Plot");
            DataChart1.ChartAreas["Plot"].AxisY.Maximum = maxData;
            DataChart1.ChartAreas["Plot"].AxisY.Minimum = minData;

            FeaturesChart1.ChartAreas.Add("Plot");
            FeaturesChart1.ChartAreas["Plot"].AxisY.Minimum = minRMS;
            FeaturesChart1.ChartAreas["Plot"].AxisY.Maximum = maxRMS;
            FrequencyChart1.ChartAreas.Add("Plot");
            FrequencyChart1.ChartAreas["Plot"].AxisY.Minimum = minTurn;
            FrequencyChart1.ChartAreas["Plot"].AxisY.Maximum = maxTurn;
            HistogramChart1.ChartAreas.Add("Histo");

            // 2 канал
            DataChart2.ChartAreas.Add("Plot");
            DataChart2.ChartAreas["Plot"].AxisY.Maximum = maxData;
            DataChart2.ChartAreas["Plot"].AxisY.Minimum = minData;

            FeaturesChart2.ChartAreas.Add("Plot");
            FeaturesChart2.ChartAreas["Plot"].AxisY.Minimum = minRMS;
            FeaturesChart2.ChartAreas["Plot"].AxisY.Maximum = maxRMS;
            FrequencyChart2.ChartAreas.Add("Plot");
            FrequencyChart2.ChartAreas["Plot"].AxisY.Minimum = minTurn;
            FrequencyChart2.ChartAreas["Plot"].AxisY.Maximum = maxTurn;
            HistogramChart2.ChartAreas.Add("Histo");

            Series plot1 = new Series("Сигнал1")
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "Plot"
            };

            Series plot2 = new Series("Сигнал2")
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "Plot"
            };

            DataChart1.Series.Add(plot1);
            DataChart2.Series.Add(plot2);

            for (int i = 0; i < data[0, 0].X.Count; i++)
            {
                DataChart1.Series[0].Points.AddXY(data[0, 0].X[i], data[0, 0].Y[i]);
                DataChart2.Series[0].Points.AddXY(data[0, 1].X[i], data[0, 1].Y[i]);
            }
        }


        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            data[0, 0].CalculateRMS(decimal.ToInt32(WindowWidth.Value));
            data[0, 1].CalculateRMS(decimal.ToInt32(WindowWidth.Value));
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Series rms1 = new Series("Ср. квадр. знач.")
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "Plot"
            };

            Series rms2 = new Series("Ср. квадр. знач.")
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "Plot"
            };

            for (int i = 0; i < data[0, 0].X.Count; i++)
            {
                rms1.Points.AddXY(data[0, 0].X[i], data[0, 0].RMS[i]);
                rms2.Points.AddXY(data[0, 1].X[i], data[0, 1].RMS[i]);
            }

            DataChart1.Series.Add(rms1);
            DataChart2.Series.Add(rms2);
        }
        
        private void rmsButton_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            data[0, 0].CalculateFeatures(data[0, 0].Y, decimal.ToInt32(WindowWidth.Value));
            data[0, 1].CalculateFeatures(data[0, 1].Y, decimal.ToInt32(WindowWidth.Value));
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Series expVal1 = new Series("Мат. ожидание")
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "Plot"
            };

            Series variance1 = new Series("Дисперсия")
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "Plot"
            };

            Series deviation1 = new Series("СКО")
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "Plot"
            };

            Series mean1 = new Series("Ср. линия")
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "Plot"
            };

            Series expVal2 = new Series("Мат. ожидание")
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "Plot"
            };

            Series variance2 = new Series("Дисперсия")
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "Plot"
            };

            Series deviation2 = new Series("СКО")
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "Plot"
            };

            Series mean2 = new Series("Ср. линия")
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "Plot"
            };

            for (int i = 0; i < data[0, 0].X.Count; i++)
            {
                expVal1.Points.AddXY(data[0, 0].X[i], data[0, 0].ExpVal[i]);
                variance1.Points.AddXY(data[0, 0].X[i], data[0, 0].Variance[i]);
                deviation1.Points.AddXY(data[0, 0].X[i], data[0, 0].Deviation[i]);

                expVal2.Points.AddXY(data[0, 1].X[i], data[0, 1].ExpVal[i]);
                variance2.Points.AddXY(data[0, 1].X[i], data[0, 1].Variance[i]);
                deviation2.Points.AddXY(data[0, 1].X[i], data[0, 1].Deviation[i]);

                //mean1.Points.AddXY(data[0, 0].X[i], data[0, 0].Mean[i]*100);
                //mean2.Points.AddXY(data[0, 1].X[i], data[0, 1].Mean[i]*100);


            }

            FeaturesChart1.Series.Add(expVal1);
            //DataChart1.Series.Add(expVal1);
            DataChart1.Series.Add(mean1);
            FeaturesChart1.Series.Add(variance1);
            FeaturesChart1.Series.Add(deviation1);

            FeaturesChart2.Series.Add(expVal2);
            //DataChart2.Series.Add(expVal2);
            DataChart2.Series.Add(mean2);
            FeaturesChart2.Series.Add(variance2);
            FeaturesChart2.Series.Add(deviation2);
        }

        private void featuresButton_Click(object sender, EventArgs e)
        {
            backgroundWorker2.RunWorkerAsync();
        }

        private void histogramButton_Click(object sender, EventArgs e)
        {
            int end = data[0,0].X.Count - 1;

            int[] sumHis1 = data[0,0].Histogram(0.0001d, 50);
            int[] sumHis2 = data[0,1].Histogram(0.0001d, 50);

            Series sumHisto1 = new Series("Общ. гистограмма 1")
            {
                ChartType = SeriesChartType.Column,
                ChartArea = "Histo"
            };
            Series sumHisto2 = new Series("Общ. гистограмма 2")
            {
                ChartType = SeriesChartType.Column,
                ChartArea = "Histo"
            };

            for (int i = 0; i < sumHis1.Length; i++)
            {
                sumHisto1.Points.AddXY(i, sumHis1[i]);
                sumHisto2.Points.AddXY(i, sumHis2[i]);
            }

            HistogramChart1.Series.Add(sumHisto1);
            HistogramChart2.Series.Add(sumHisto2);

        }
        
        private void FrequencyButton_Click(object sender, EventArgs e)
        {
            data[0,0].CountTurns(decimal.ToInt32(WindowWidth.Value), 0.0001d);
            data[0,0].CountZeroCrossing(decimal.ToInt32(WindowWidth.Value));

            data[0,1].CountTurns(decimal.ToInt32(WindowWidth.Value), 0.0001d);
            data[0,1].CountZeroCrossing(decimal.ToInt32(WindowWidth.Value));

            Series turns1 = new Series("Число поворотов")
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "Plot"
            };

            Series zeros1 = new Series("Число пересечений нуля")
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "Plot"
            };

            Series turns2 = new Series("Число поворотов")
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "Plot"
            };

            Series zeros2 = new Series("Число пересечений нуля")
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "Plot"
            };

            for (int i = 0; i < data[0,0].X.Count; i++)
            {
                turns1.Points.AddXY(data[0,0].X[i], data[0,0].Turns[i]);
                zeros1.Points.AddXY(data[0,0].X[i], data[0,0].ZeroCrossings[i]);

                turns2.Points.AddXY(data[0,1].X[i], data[0,1].Turns[i]);
                zeros2.Points.AddXY(data[0,1].X[i], data[0,1].ZeroCrossings[i]);
            }

            FrequencyChart1.Series.Add(turns1);
            FrequencyChart1.Series.Add(zeros1);

            FrequencyChart2.Series.Add(turns2);
            FrequencyChart2.Series.Add(zeros2);
        }
        
        private void MinmaxButton_Click(object sender, EventArgs e)
        {
            int digits = 10;

            Data copy1 = new Data(data[0,0]);
            Data copy2 = new Data(data[0,1]);
            
            copy1.RMS.RemoveRange(0, (int)WindowWidth.Value);
            copy1.ExpVal.RemoveRange(0, (int)WindowWidth.Value);
            copy1.Deviation.RemoveRange(0, (int)WindowWidth.Value);
            copy1.Turns.RemoveRange(0, (int)WindowWidth.Value);
            copy1.ZeroCrossings.RemoveRange(0, (int)WindowWidth.Value);
            
            copy2.RMS.RemoveRange(0, (int)WindowWidth.Value);
            copy2.ExpVal.RemoveRange(0, (int)WindowWidth.Value);
            copy2.Deviation.RemoveRange(0, (int)WindowWidth.Value);
            copy2.Turns.RemoveRange(0, (int)WindowWidth.Value);
            copy2.ZeroCrossings.RemoveRange(0, (int)WindowWidth.Value);
            
            for (int i = 0; i < copy1.Y.Count; i++)
            {
                copy1.Y[i] = Math.Round(copy1.Y[i], digits);
                copy2.Y[i] = Math.Round(copy2.Y[i], digits);
            }

            for (int i = 0; i < copy1.RMS.Count; i++)
            {
                copy1.RMS[i] = Math.Round(copy1.RMS[i], digits);
                copy1.ExpVal[i] = Math.Round(copy1.ExpVal[i], digits);
                copy1.Deviation[i] = Math.Round(copy1.Deviation[i], digits);

                copy2.RMS[i] = Math.Round(copy2.RMS[i], digits);
                copy2.ExpVal[i] = Math.Round(copy2.ExpVal[i], digits);
                copy2.Deviation[i] = Math.Round(copy2.Deviation[i], digits);
            }

            SignalTextbox.Text += copy1.Y.Min().ToString() + " " + copy1.Y.Max().ToString();
            SignalTextbox.Text += Environment.NewLine;
            SignalTextbox.Text += copy2.Y.Min().ToString() + " " + copy2.Y.Max().ToString();

            RmsTextbox.Text += copy1.RMS.Min().ToString() + " " + copy1.RMS.Max().ToString();
            RmsTextbox.Text += Environment.NewLine;
            RmsTextbox.Text += copy2.RMS.Min().ToString() + " " + copy2.RMS.Max().ToString();

            ExpValTextbox.Text += copy1.ExpVal.Min().ToString() + " " + copy1.ExpVal.Max().ToString();
            ExpValTextbox.Text += Environment.NewLine;
            ExpValTextbox.Text += copy2.ExpVal.Min().ToString() + " " + copy2.ExpVal.Max().ToString();

            DeviationTextbox.Text += copy1.Deviation.Min().ToString() + " " + copy1.Deviation.Max().ToString();
            DeviationTextbox.Text += Environment.NewLine;
            DeviationTextbox.Text += copy2.Deviation.Min().ToString() + " " + copy2.Deviation.Max().ToString();

            TurnsTextbox.Text += copy1.Turns.Min().ToString() + " " + copy1.Turns.Max().ToString();
            TurnsTextbox.Text += Environment.NewLine;
            TurnsTextbox.Text += copy2.Turns.Min().ToString() + " " + copy2.Turns.Max().ToString();

            ZeroCrossTextbox.Text += copy1.ZeroCrossings.Min().ToString() + " " + copy1.ZeroCrossings.Max().ToString();
            ZeroCrossTextbox.Text += Environment.NewLine;
            ZeroCrossTextbox.Text += copy2.ZeroCrossings.Min().ToString() + " " + copy2.ZeroCrossings.Max().ToString();


            textBox1.Text += copy1.Y.Min().ToString() + " " + copy1.Y.Max().ToString();
            textBox1.Text += Environment.NewLine;
            textBox1.Text += copy1.RMS.Min().ToString() + " " + copy1.RMS.Max().ToString();
            textBox1.Text += Environment.NewLine;
            textBox1.Text += copy1.ExpVal.Min().ToString() + " " + copy1.ExpVal.Max().ToString();
            textBox1.Text += Environment.NewLine;
            textBox1.Text += copy1.Deviation.Min().ToString() + " " + copy1.Deviation.Max().ToString();
            textBox1.Text += Environment.NewLine;
            textBox1.Text += copy1.Turns.Min().ToString() + " " + copy1.Turns.Max().ToString();
            textBox1.Text += Environment.NewLine;
            textBox1.Text += copy1.ZeroCrossings.Min().ToString() + " " + copy1.ZeroCrossings.Max().ToString();
            textBox1.Text += Environment.NewLine;
            textBox1.Text += copy2.Y.Min().ToString() + " " + copy2.Y.Max().ToString();
            textBox1.Text += Environment.NewLine;
            textBox1.Text += copy2.RMS.Min().ToString() + " " + copy2.RMS.Max().ToString();
            textBox1.Text += Environment.NewLine;
            textBox1.Text += copy2.ExpVal.Min().ToString() + " " + copy2.ExpVal.Max().ToString();
            textBox1.Text += Environment.NewLine;
            textBox1.Text += copy2.Deviation.Min().ToString() + " " + copy2.Deviation.Max().ToString();
            textBox1.Text += Environment.NewLine;
            textBox1.Text += copy2.Turns.Min().ToString() + " " + copy2.Turns.Max().ToString();
            textBox1.Text += Environment.NewLine;
            textBox1.Text += copy2.ZeroCrossings.Min().ToString() + " " + copy2.ZeroCrossings.Max().ToString();

        }

        private void clickAllButton_Click(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                rmsButton_Click(sender, e);
                FrequencyButton_Click(sender, e);
                histogramButton_Click(sender, e);
                featuresButton_Click(sender, e);
                MinmaxButton_Click(sender, e);
            }

            else CalcWithoutDraw();


        }

        public void CalcWithoutDraw()
        {
            int count = data.GetLength(0);

            for (int i = 0; i < count; i++)
            {
                data[i, 0].CalculateRMS(decimal.ToInt32(WindowWidth.Value));
                data[i, 0].CalculateFeatures(data[i, 0].Y, decimal.ToInt32(WindowWidth.Value));
                data[i, 0].CountTurns(decimal.ToInt32(WindowWidth.Value), 0.0001d);
                data[i, 0].CountZeroCrossing(decimal.ToInt32(WindowWidth.Value));

                data[i, 1].CalculateRMS(decimal.ToInt32(WindowWidth.Value));
                data[i, 1].CalculateFeatures(data[i, 1].Y, decimal.ToInt32(WindowWidth.Value));
                data[i, 1].CountTurns(decimal.ToInt32(WindowWidth.Value), 0.0001d);
                data[i, 1].CountZeroCrossing(decimal.ToInt32(WindowWidth.Value));
            }

            Close();
        }

        public void CalcWithoutDraw(int N)
        {
            data[N, 0].CalculateRMS(decimal.ToInt32(WindowWidth.Value));
            data[N, 0].CalculateFeatures(data[N, 0].Y, decimal.ToInt32(WindowWidth.Value));
            data[N, 0].CountTurns(decimal.ToInt32(WindowWidth.Value), 0.0001d);
            data[N, 0].CountZeroCrossing(decimal.ToInt32(WindowWidth.Value));

            data[N, 1].CalculateRMS(decimal.ToInt32(WindowWidth.Value));
            data[N, 1].CalculateFeatures(data[N, 1].Y, decimal.ToInt32(WindowWidth.Value));
            data[N, 1].CountTurns(decimal.ToInt32(WindowWidth.Value), 0.0001d);
            data[N, 1].CountZeroCrossing(decimal.ToInt32(WindowWidth.Value));
        }

       
    }
}
