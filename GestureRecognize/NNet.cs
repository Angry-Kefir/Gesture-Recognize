using System.Windows.Forms;
using AForge.Neuro;
using AForge.Neuro.Learning;
using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;

namespace GestureRecognize
{
    public partial class NNet : Form
    {
        // ЗАГЛУШКА
        Data data;
        const int samplesNum = 100;
        const int inputsNum = 2;
        const int neuronCount = 2;
        const float Ymin = -0.009f, Ymax = 0.004f,
                    RMSmin = 0f, RMSmax = 0.003f;
        const int turnsMin = 0, turnsMax = 50;
        // ЖИЗНЬ БОЛЬ ЛОЛ

        //ActivationNetwork network = new ActivationNetwork(new ThresholdFunction(), inputsNum, 5, neuronCount);
        ActivationNetwork network = new ActivationNetwork(new SigmoidFunction(), inputsNum, 5, neuronCount);

        private void StartLearning()
        {
            //ActivationNeuron neuron = network[0][0];
            //PerceptronLearning teacher = new PerceptronLearning(network)
            //{
            //    LearningRate = 1
            //};
            BackPropagationLearning teacher = new BackPropagationLearning(network);

            Data[] inputs = new Data[samplesNum];
            int temp;

            for (int i = 0; i < samplesNum; i++)
            {
                temp = 40710 / samplesNum * (i + 1);

                inputs[i] = new Data();
                inputs[i].X.Add(data.X[temp]);
                inputs[i].Y.Add(data.Y[temp]);
                inputs[i].RMS.Add(data.RMS[temp]);
                inputs[i].Turns.Add(data.Turns[temp]);
                // inputs[i].Features.Add(data.Features[temp]);
            }

            double[][] input = new double[samplesNum][];
            double[][] output = new double[samplesNum][];

            int c = 0;

            for (int i = 0; i < samplesNum; i++)
            {
                input[i] = new double[inputsNum];
                output[i] = new double[neuronCount];

                if ((i % 2) == 0)
                {
                    // copy input
                    //input[i][0] = NormalizeData(inputs[i / 2].Y[0], Ymin, Ymax);
                    input[i][0] = NormalizeData(inputs[i / 2].RMS[0], RMSmin, RMSmax);
                    input[i][1] = NormalizeData(inputs[i / 2].Turns[0], turnsMin, turnsMax);
                    //input[i][3] = inputs[i].Features[0].deviation;
                    //input[i][4] = inputs[i].Features[0].expVal;

                    // copy output

                    if (inputs[i].X[0] >= 0d && inputs[i].X[0] < 2d)
                    {
                        output[i][0] = 0;
                        output[i][1] = 1;
                    }
                    else
                    {
                        output[i][0] = 1;
                        output[i][1] = 0;
                    }
                }
                else
                {
                    c++;

                    // copy input
                    //input[i][0] = NormalizeData(inputs[samplesNum / 2 + i - c].Y[0], Ymin, Ymax);
                    input[i][0] = NormalizeData(inputs[samplesNum / 2 + i - c].RMS[0], RMSmin, RMSmax);
                    input[i][1] = NormalizeData(inputs[samplesNum / 2 + i - c].Turns[0], turnsMin, turnsMax);
                    //input[i][3] = inputs[i].Features[0].deviation;
                    //input[i][4] = inputs[i].Features[0].expVal;

                    // copy output

                    if (inputs[samplesNum / 2 + i - c].X[0] >= 2d && inputs[samplesNum / 2 + i - c].X[0] <= 4d)
                    {
                        output[i][0] = 1;
                        output[i][1] = 0;
                    }
                    else
                    {
                        output[i][0] = 0;
                        output[i][1] = 1;
                    }
                }
            }

            while (true)
            {
                // run epoch of learning procedure
                double error = teacher.RunEpoch(input, output);

                // stop if no error
                //if (error == 0)
                if (error < 0.000002)
                    break;
            }
        }

        private void Test()
        {

            double[] inp = new double[inputsNum];
            List<double[]> outt = new List<double[]>();

            for (int i = 0; i < data.X.Count; i++)
            {
                // inp[0] = NormalizeData(data.Y[i], Ymin, Ymax);
                inp[0] = NormalizeData(data.RMS[i], RMSmin, RMSmax);
                inp[1] = NormalizeData(data.Turns[i], turnsMin, turnsMax);
                //inp[3] = data.Features[i].deviation;
                //inp[4] = data.Features[i].expVal;

                outt.Add(network.Compute(inp));

            }

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

            for (int i = 0; i < data.X.Count; i++)
            {
                out1.Points.AddXY(data.X[i], outt[i][0]);
                out2.Points.AddXY(data.X[i], outt[i][1]);
            }

            //OutChart.Series.Add(out1);
            //OutChart.Series.Add(out2);

        }



        /// <summary>
        /// Функция пропорционально переносит значение (value) из текущего диапазона значений [fromMin; fromMax] в новый диапазон [0; 1].
        /// </summary>
        /// <param name="value">Число для нормирования</param>
        /// <param name="fromMin">Нижняя граница диапазона</param>
        /// <param name="fromMax">Верхняя граница диапазона</param>
        private double NormalizeData(double value, double fromMin, double fromMax)
        {
            double temp = (value - fromMin) / (fromMax - fromMin);

            if (temp < 0) return 0d;
            else if (temp > 1) return 1d;
            else return temp;
        }

        /// <summary>
        /// Функция пропорционально переносит значение (value) из текущего диапазона значений [fromMin; fromMax] в новый диапазон [toMin; toMax].
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fromMin"></param>
        /// <param name="fromMax"></param>
        /// <param name="toMin"></param>
        /// <param name="toMax"></param>
        /// <returns></returns>
        private double NormalizeData(double value, double fromMin, double fromMax, double toMin, double toMax)
        {
            double temp = (value - fromMin) * (toMax - toMin) / (fromMax - fromMin) + toMin;

            if (temp < toMin) return toMin;
            else if (temp > toMax) return toMax;
            else return temp;
        }
    }
}
