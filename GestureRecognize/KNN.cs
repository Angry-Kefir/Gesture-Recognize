using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Accord;
using Accord.MachineLearning;
using Accord.Statistics.Analysis;

namespace GestureRecognize
{
    public partial class KNN : Form
    {
        Data[,] data;
        KNearestNeighbors knn;
        int[] nums = new int[] { 0, 6, 12, 18, 24, 30, 36, 42, 48, 54 };
        double[][] inputs;
        int[] outputs;

        public KNN()
        {
            InitializeComponent();
        }

        private void KNN_Load(object sender, EventArgs e)
        {
            int numFeatures = 5, numSamples = 5000;

            inputs = new double[numSamples * nums.Length][];
            outputs = new int[numSamples * nums.Length];

            loadData(ref inputs, ref outputs, numFeatures, numSamples);

            knn = new KNearestNeighbors(k: nums.Length);
            knn.Learn(inputs, outputs);
                        
            //var cm = GeneralConfusionMatrix.Estimate(knn, inputs, outputs);
            
            //double error = cm.Error; 
            //double acc = cm.Accuracy; 
            //double kappa = cm.Kappa;  
        }

        void loadData(ref double[][] inputs, ref int[] outputs, int numFeatures, int numSamples)
        {
            openFileDialog1.InitialDirectory = @"D:\YandexDisk\Программирование\GestureRecognize\GestureRecognize\bin\Debug";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (FileStream fs = File.OpenRead(openFileDialog1.FileName))
                {
                    data = (Data[,])bf.Deserialize(fs);
                }
            }

            int _num, k;
            double threshold = 1.3d;        // пороговое значение для отношения амплитуд

            int width = data[nums[0], 0].X.Count;
            
            for (int n = 0; n < nums.Length; n++)
            {
                k = nums[n];
                for (int i = 0; i < numSamples; i++)
                {
                    _num = 700 + ((width - 700 - 300) / numSamples) * i; // перебор точек 700 - 20000

                    double r0 = data[k, 0].RMS[_num], r1 = data[k, 1].RMS[_num], dr;

                    if ((r0 >= r1) && (r0 / r1 >= threshold))
                        dr = r0 / r1;
                    else if ((r0 < r1) && (r1 / r0 >= threshold))
                        dr = -r1 / r0;
                    else dr = 0;

                    //dr = r0 / r1;

                    inputs[i + numSamples * n] = new double[numFeatures];

                    inputs[i + numSamples * n][0] = data[k, 0].Turns[_num];
                    inputs[i + numSamples * n][1] = data[k, 1].Turns[_num];
                    inputs[i + numSamples * n][2] = data[k, 0].ZeroCrossings[_num];
                    inputs[i + numSamples * n][3] = data[k, 1].ZeroCrossings[_num];
                    inputs[i + numSamples * n][4] = dr;

                    outputs[i + numSamples * n] = n;                    
                }
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            double threshold = 1.3d;        // пороговое значение для отношения амплитуд
            List<int> output1 = new List<int>();
            List<int> output2 = new List<int>();

            //List<int> outputFilter = new List<int>();

            int[] nums = new int[] { 1, 7, 13, 19, 25, 31, 37, 43, 49, 55 };
            //int[] nums = new int[] { 0, 6, 12, 18, 24, 30, 36, 42, 48, 54 };

            int k;

            for (int n = 0; n < nums.Length; n++)
            {
                k = nums[n];

                for (int i = 1000; i < data[k, 0].X.Count; i++)
                {
                    double r0 = data[k, 0].RMS[i], r1 = data[k, 1].RMS[i], dr;

                    if ((r0 >= r1) && (r0 / r1 >= threshold))
                        dr = r0 / r1;
                    else if ((r0 < r1) && (r1 / r0 >= threshold))
                        dr = -r1 / r0;
                    else dr = 0;

                    //dr = r0 / r1;

                    double[] input =
                    {
                        data[k, 0].Turns[i],
                        data[k, 1].Turns[i],
                        data[k, 0].ZeroCrossings[i],
                        data[k, 1].ZeroCrossings[i],
                        dr
                    };

                    output1.Add(knn.Decide(input)+1);

                    output2.Add(n+1);
                }
            }

            //int start;
            //int end;
            //int width = 700;
            //int[] outs = new int[nums.Length];

            //for (int o = 0; o < outs.Length; o++)
            //    outs[o] = 0;

            //for (int i = 0; i < output1.Count; i++)
            //{
            //    if (i < width) start = 0;
            //    else start = i - width;

            //    end = i;

            //    for (int j = start; j < end; j++)
            //        outs[output1[j]]++;

            //    outputFilter.Add(Array.IndexOf(outs, outs.Max())+1);

            //    for (int o = 0; o < outs.Length; o++)
            //        outs[o] = 0;
            //}

            Series out1 = new Series("Выход1")
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "Plot"
            };

            Series out2 = new Series("Выход2")
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "Plot"
            };

            for (int i = 0; i < output1.Count; i++)
            {
                //out1.Points.AddXY(i, outputFilter[i]);
                out1.Points.AddXY(i, output1[i]);
                out2.Points.AddXY(i, output2[i]);

            }

            OutChart.ChartAreas.Add("Plot");
            OutChart.ChartAreas["Plot"].AxisX.Maximum = output1.Count;
            OutChart.ChartAreas["Plot"].AxisX.Minimum = 0;
            OutChart.ChartAreas["Plot"].AxisY.Maximum = 11;
            OutChart.ChartAreas["Plot"].AxisY.Minimum = -1;

            OutChart.Series.Add(out1);
            OutChart.Series.Add(out2);

        }
    }
}
