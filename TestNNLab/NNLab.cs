using System;
using System.Windows.Forms;
using AForge.Neuro;
using AForge.Neuro.Learning;
using AForge;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Drawing;

namespace TestNNLab
{
    public partial class NNLab : Form
    {
        public NNLab()
        {
            InitializeComponent();
        }

        const int samplesNum = 20;
        Points[] inputs = new Points[samplesNum];

        ActivationNetwork network = new ActivationNetwork(new ThresholdFunction(), 2, 1);

        struct Points
        {
            public double X;
            public double Y;
            public double type;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // НЕЙРОСЕТЬ

            ActivationNeuron neuron = network[0][0];
            PerceptronLearning teacher = new PerceptronLearning(network);
            teacher.LearningRate = 1;

            StreamWriter errorsFile = null;
            StreamWriter weightsFile = null;
            errorsFile = File.CreateText("errors.csv");
            weightsFile = File.CreateText("weights.csv");

            // iterations
            int iteration = 1;

            ArrayList errorsList = new ArrayList();


            double[][] input = new double[samplesNum][];
            double[][] output = new double[samplesNum][];

            for (int i = 0; i < samplesNum; i++)
            {
                input[i] = new double[2];
                output[i] = new double[1];

                // copy input
                input[i][0] = inputs[i].X;
                input[i][1] = inputs[i].Y;
                // copy output
                output[i][0] = inputs[i].type;
            }
            
            while (true)
            {
                // save current weights
                if (weightsFile != null)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        weightsFile.Write(neuron[i] + ";");
                    }
                    weightsFile.WriteLine(neuron.Threshold);
                }

                // run epoch of learning procedure
                double error = teacher.RunEpoch(input, output);
                errorsList.Add(error);

                // show current iteration
                iterationsBox.Text = iteration.ToString();

                // save current error
                if (errorsFile != null)
                {
                    errorsFile.WriteLine(error);
                }

                // show classifier in the case of 2 dimensional data
                if ((neuron.InputsCount == 2) && (neuron[1] != 0))
                {
                    double k = -neuron[0] / neuron[1];
                    double b = -neuron.Threshold / neuron[1];
                }

                // stop if no error
                if (error == 0)
                    break;

                iteration++;
            }

            // show error's dynamics
            double[,] errors = new double[errorsList.Count, 2];

            for (int i = 0, n = errorsList.Count; i < n; i++)
            {
                errors[i, 0] = i;
                errors[i, 1] = (double)errorsList[i];
            }
            errorChart.AddDataSeries("error", Color.Red, AForge.Controls.Chart.SeriesType.ConnectedDots, 3, false);
            errorChart.RangeX = new DoubleRange(0, errorsList.Count - 1);
            errorChart.RangeY = new DoubleRange(0, samplesNum);
            errorChart.UpdateDataSeries("error", errors);


            if (errorsFile != null)
                errorsFile.Close();
            if (weightsFile != null)
                weightsFile.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // ТЕСТОВАЯ ВЫБОРКА

            int testsNum = 50;
            int testsCount = testsNum * testsNum;
            Points[] test = new Points[testsCount];

            for (int i = 0; i < testsNum; i++)
            {
                for (int j = 0; j < testsNum; j++)
                {
                    test[testsNum * j + i].X = 10d / testsNum * i;
                    test[testsNum * j + i].Y = 10d / testsNum * j;
                }
            }

            double[] inp = new double[2];

            for (int i = 0; i < testsCount; i++)
            {
                inp[0] = test[i].X;
                inp[1] = test[i].Y;
                double[] outt = network.Compute(inp);
                test[i].type = outt[0];

            }

            chart2.ChartAreas.Add(new ChartArea("Test"));
            chart2.ChartAreas["Test"].AxisY.Maximum = 10;
            chart2.ChartAreas["Test"].AxisY.Minimum = 0;
            chart2.ChartAreas["Test"].AxisY.Interval = 1;
            chart2.ChartAreas["Test"].AxisX.Maximum = 10;
            chart2.ChartAreas["Test"].AxisX.Minimum = 0;
            chart2.ChartAreas["Test"].AxisX.Interval = 1;

            Series seriesOfTests1 = new Series("Test1")
            {
                ChartType = SeriesChartType.FastPoint,
                ChartArea = "Test"

            };
            Series seriesOfTests2 = new Series("Test2")
            {
                ChartType = SeriesChartType.FastPoint,
                ChartArea = "Test"
            };

            for (int i = 0; i < testsCount; i++)
            {
                if (test[i].type < 0.5d)
                    seriesOfTests1.Points.AddXY(test[i].X, test[i].Y);
                if (test[i].type >= 0.5d)
                    seriesOfTests2.Points.AddXY(test[i].X, test[i].Y);
            }

            chart2.Series.Add(seriesOfTests1);
            chart2.Series.Add(seriesOfTests2);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            #region вариант с массивом
            //const int X = 0;
            //const int Y = 1;
            //const int type = 2;

            //int samples = 100;
            //int[,] inputXY = new int[samples, 3];

            //Random randX = new Random();
            //Task.Delay(1000);
            //Random randY = new Random();

            //for (int i = 0; i < samples; i++)
            //{
            //    inputXY[i, X] = randX.Next(0, 10);
            //    inputXY[i, Y] = randY.Next(0, 10);

            //    if (inputXY[i, Y] < 2 * inputXY[i, X] - 5)
            //        inputXY[i, type] = -1;
            //    else inputXY[i, type] = 1;
            //}

            //chart1.ChartAreas.Add(new ChartArea("Samples"));
            //Series seriesOfSamples1 = new Series("Samples1")
            //{
            //    ChartType = SeriesChartType.FastPoint,
            //    ChartArea = "Samples"
            //};
            //Series seriesOfSamples2 = new Series("Samples2")
            //{
            //    ChartType = SeriesChartType.FastPoint,
            //    ChartArea = "Samples"
            //};

            //for (int i = 0; i < samples; i++)
            //{
            //    if (inputXY[i, type] == -1)
            //        seriesOfSamples1.Points.AddXY(inputXY[i, X], inputXY[i, Y]);
            //    if (inputXY[i, type] == 1)
            //        seriesOfSamples2.Points.AddXY(inputXY[i, X], inputXY[i, Y]);
            //}

            //chart1.Series.Add(seriesOfSamples1);
            //chart1.Series.Add(seriesOfSamples2);
            #endregion

            // ОБУЧАЮЩАЯ ВЫБОРКА

            Random randX = new Random();
            Task.Delay(3000);
            Random randY = new Random();

            for (int i = 0; i < samplesNum; i++)
            {
                inputs[i].X = randX.NextDouble() * 10;
                inputs[i].Y = randY.NextDouble() * 10;

                if (inputs[i].Y < 2 * inputs[i].X - 5)
                    inputs[i].type = 0;
                else inputs[i].type = 1;
            }

            chart1.ChartAreas.Add(new ChartArea("Samples"));
            chart1.ChartAreas["Samples"].AxisY.Maximum = 10;
            chart1.ChartAreas["Samples"].AxisY.Minimum = 0;
            chart1.ChartAreas["Samples"].AxisY.Interval = 1;
            chart1.ChartAreas["Samples"].AxisX.Maximum = 10;
            chart1.ChartAreas["Samples"].AxisX.Minimum = 0;
            chart1.ChartAreas["Samples"].AxisX.Interval = 1;

            Series seriesOfSamples1 = new Series("Samples1")
            {
                ChartType = SeriesChartType.FastPoint,
                ChartArea = "Samples"
            };
            Series seriesOfSamples2 = new Series("Samples2")
            {
                ChartType = SeriesChartType.FastPoint,
                ChartArea = "Samples"
            };

            for (int i = 0; i < samplesNum; i++)
            {
                if (inputs[i].type == 0)
                    seriesOfSamples1.Points.AddXY(inputs[i].X, inputs[i].Y);
                if (inputs[i].type == 1)
                    seriesOfSamples2.Points.AddXY(inputs[i].X, inputs[i].Y);
            }

            chart1.Series.Add(seriesOfSamples1);
            chart1.Series.Add(seriesOfSamples2);
        }
    }
}
