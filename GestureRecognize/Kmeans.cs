using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace GestureRecognize
{
    public partial class Kmeans : Form
    {
        string[] path = new string[2];
        Data[,] data;

        public Kmeans(ref Data[,] data)
        {
            this.data = data;

            InitializeComponent();
        }

        public Kmeans()
        {
            InitializeComponent();
        }

        private void Kmeans_Load(object sender, System.EventArgs e)
        {

        }

        private void testButton_Click(object sender, System.EventArgs e)
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

            alglib.clusterizerstate s;
            alglib.kmeansreport rep;

            int[] nums = new int[] { 0, 6, 12, 18, 24 }; // для 7
            //int[] nums = new int[] { 0, 18, 42, 48, 54 }; // для 3
            //int[] nums = new int[] { 0, 6, 12, 18, 24, 30, 36, 42, 48, 54}; // все

            int sets = nums.Length, samples = 10000;
            double[,] mass = DataToArray(samples, nums);

            alglib.clusterizercreate(out s);
            alglib.clusterizersetpoints(s, mass, 2);
            alglib.clusterizersetkmeanslimits(s, 5, 0);
            alglib.clusterizerrunkmeans(s, sets, out rep);

            if (rep.terminationtype == 1)
            {
                int[,] classRes = new int[sets, sets];
                int pointsInSet = mass.GetLength(0) / sets;

                for (int n = 0; n < sets; n++)
                {
                    for (int i = 0; i < pointsInSet; i++)
                    {
                        int r = rep.cidx[i + samples * n];
                        classRes[n, r]++;
                    }
                }

                textBox1.Text += Environment.NewLine;
                textBox1.Text += Environment.NewLine;

                for (int i = 0; i < sets; i++)
                {
                    for (int j = 0; j < sets; j++)
                    {
                        textBox1.Text += classRes[i, j].ToString() + "\t";
                    }
                    textBox1.Text += Environment.NewLine;

                }
            }

            else textBox1.Text += rep.terminationtype.ToString();

        }

        private void coupleButton_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";

            openFileDialog1.InitialDirectory = @"D:\YandexDisk\Программирование\GestureRecognize\GestureRecognize\EMG_2Chs\EMG-S1";
            data = new Data[2, 2];

            for (int i = 0; i < 2; i++)
            {
                data[i, 0] = new Data();
                data[i, 1] = new Data();

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    path[i] = openFileDialog1.FileName;
                }

                data[i, 0].ReadDatasetLessX(path[i], 0);
                data[i, 1].ReadDatasetLessX(path[i], 1);
            }

            DataAnalysis DA = new DataAnalysis(ref data);
            DA.CalcWithoutDraw();

            //---------------------

            alglib.clusterizerstate s;
            alglib.kmeansreport rep;

            int sets = 2, samples = 5000;
            int incNames = 1;

            while (true)
            {
                double[,] mass = DataToArray(sets, samples);

                alglib.clusterizercreate(out s);
                alglib.clusterizersetpoints(s, mass, 2);
                alglib.clusterizersetkmeanslimits(s, 5, 0);
                alglib.clusterizerrunkmeans(s, sets, out rep);

                if (rep.terminationtype == 1)
                {
                    int[,] classRes = new int[sets, sets];
                    int pointsInSet = mass.GetLength(0) / sets;

                    for (int n = 0; n < sets; n++)
                    {
                        for (int i = 0; i < pointsInSet; i++)
                        {
                            int r = rep.cidx[i + samples * n];
                            classRes[n, r]++;
                        }
                    }

                    for (int i = 0; i < sets; i++)
                    {
                        for (int j = 0; j < sets; j++)
                        {
                            textBox1.Text += classRes[i, j].ToString() + "\t";
                        }
                        textBox1.Text += Environment.NewLine;

                    }

                    textBox1.Text += Environment.NewLine;
                }

                else textBox1.Text += rep.terminationtype.ToString();

                incNames++;
                if (incNames == 7) break;
                path[1] = path[1].Remove(path[1].Length - 5);
                path[1] = path[1].Insert(path[1].Length, incNames.ToString());
                path[1] = path[1].Insert(path[1].Length, ".csv");

                data[1, 0] = new Data();
                data[1, 1] = new Data();

                data[1, 0].ReadDatasetLessX(path[1], 0);
                data[1, 1].ReadDatasetLessX(path[1], 1);

                DA = new DataAnalysis(ref data);
                DA.CalcWithoutDraw();
            }



        }

        private void loadDataButton_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            data = new Data[60, 2];
            string[] paths = loadNames();
            
            for (int i = 0; i < 60; i++)
            {
                data[i, 0] = new Data();
                data[i, 1] = new Data();

                data[i, 0].ReadDatasetLessX(paths[i], 0);
                data[i, 1].ReadDatasetLessX(paths[i], 1);
            }

            DataAnalysis DA = new DataAnalysis(ref data);
            DA.CalcWithoutDraw();

            BinaryFormatter bf = new BinaryFormatter();

            using (FileStream fs = File.Create("test10.bin"))
                bf.Serialize(fs, data);
        }
        
        private void cntrMassButton_Click(object sender, EventArgs e)
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

            int feat = 5;
            alglib.clusterizerstate s;
            alglib.kmeansreport rep;
            Scope[] scopes = new Scope[data.GetLength(0)];

            for (int i = 0; i < data.GetLength(0); i++) // переход по датасетам от 0 до 60
            {
                double[,] mass = DataToArray(i);

                alglib.clusterizercreate(out s);
                alglib.clusterizersetpoints(s, mass, 2);
                alglib.clusterizersetkmeanslimits(s, 5, 0);
                alglib.clusterizerrunkmeans(s, 1, out rep);

                scopes[i] = new Scope(feat);
                
                for (int n = 0; n < feat; n++) // переход между координатами/признаками от 0 до 5
                {
                    // центры
                    scopes[i].center[n] = rep.c[0, n];
                     
                    // минимумы и максимумы
                    for (int j = 0; j < mass.GetLength(0); j++)
                    {
                        if (mass[j, n] < scopes[i].min[n])
                            scopes[i].min[n] = mass[j, n];

                        if (mass[j, n] > scopes[i].max[n])
                            scopes[i].max[n] = mass[j, n];
                    }
                }
            }

            BinaryFormatter bf1 = new BinaryFormatter();

            using (FileStream fs = File.Create("scopesTest7complexdR.bin"))
                bf1.Serialize(fs, scopes);
        }

        private void calcErrorButton_Click(object sender, EventArgs e)
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

            alglib.clusterizerstate s;
            alglib.kmeansreport rep;

            int sets = 2, samples = 5000;
            int cl = 9; // от 0 до 9
            int[,] errors = new int[cl + 1, cl + 1];

            for (int n = 0; n <= cl - 1; n++) // цикл по классам от HС до TR
            {
                for (int nextN = n + 1; nextN <= cl; nextN++) // цикл по следующим классам от II до TT
                {
                    for (int k = n * 6; k <= n * 6 + 5; k++) // цикл внутри одного из классов, напр. от HС-1 до HС-6
                    {
                        for (int nextK = nextN * 6; nextK <= nextN * 6 + 5; nextK++) // цикл внутри следущего класса, напр. от II-1 до II-6
                        {
                            double[,] mass = DataToArray(samples, k, nextK);

                            alglib.clusterizercreate(out s);
                            alglib.clusterizersetpoints(s, mass, 2);
                            alglib.clusterizersetkmeanslimits(s, 5, 0);
                            alglib.clusterizerrunkmeans(s, sets, out rep);

                            //----
                            //double[,] tofile = new double[samples * 2, 6];
                            //----

                            if (rep.terminationtype == 1)
                            {
                                int[,] classRes = new int[sets, sets];
                                int pointsInSet = mass.GetLength(0) / sets;

                                for (int i = 0; i < sets; i++)
                                {
                                    for (int j = 0; j < pointsInSet; j++)
                                    {
                                        int r = rep.cidx[j + samples * i];
                                        classRes[i, r]++;

                                        //----
                                        //tofile[j + samples * i, 0] = mass[j + samples * i, 0];
                                        //tofile[j + samples * i, 1] = mass[j + samples * i, 1];
                                        //tofile[j + samples * i, 2] = mass[j + samples * i, 2];
                                        //tofile[j + samples * i, 3] = mass[j + samples * i, 3];
                                        //tofile[j + samples * i, 4] = mass[j + samples * i, 4];
                                        //tofile[j + samples * i, 5] = rep.cidx[j + samples * i];
                                        //----
                                    }
                                }

                                //----
                                //BinaryFormatter bf = new BinaryFormatter();

                                //using (FileStream fs = File.Create("D:\\Data\\_Plot\\test7\\"+"7 pointsPlot" + k.ToString() + "-" + nextK.ToString() + ".bin"))
                                //    bf.Serialize(fs, tofile);
                                //----

                                int min = Min(classRes);
                                switch (min)
                                {
                                    case 0:
                                    case 3:
                                        errors[n, nextN] += classRes[0, 0] + classRes[1, 1];
                                        break;
                                    case 1:
                                    case 2:
                                        errors[n, nextN] += classRes[1, 0] + classRes[0, 1];
                                        break;
                                }
                            }

                            else textBox1.Text += rep.terminationtype.ToString();
                        }
                    }
                }
            }

            for (int i = 0; i <= cl; i++)
            {
                for (int j = 0; j <= cl; j++)
                {
                    textBox1.Text += errors[i, j].ToString() + "\t";
                }
                textBox1.Text += Environment.NewLine;

            }
            
        }

        private void pcaButton_Click(object sender, EventArgs e)
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

            int[] nums = new int[] { 0, 6, 12, 18, 24 };
            int sets = nums.Length, samples = 5000;
            double[,] mass = DataToArray(samples, nums);


            int info;
            double[] s2 = new double[sets];
            double[,] v = new double[sets, sets];
            alglib.pcabuildbasis(mass, samples * sets, sets, out info, out s2, out v);

            textBox1.Text += "0";
        }


        /// <summary>
        /// Преобразование массива данных в массив для работы алгоритма k-means
        /// </summary>
        /// <param name="sets">Количество кластеров</param>
        /// <param name="numSamples">Количество примеров</param>
        /// <returns></returns>
        double[,] DataToArray(int sets, int numSamples)
        {
            int width = data[0, 0].X.Count; // длина массива точек для расчета выборки
            double threshold = 1.3d;        // пороговое значение для отношения амплитуд

            int _num;

            double[,] result = new double[numSamples * sets, 5];

            for (int j = 0; j < sets; j++)
            {
                for (int i = 0; i < numSamples; i++)
                {
                    _num = 700 + ((width - 700 - 300) / numSamples) * i;
                    //result[i + numSamples * j, 0] = data[j, 0].RMS[n] - data[j, 1].RMS[n];
                    //result[i + numSamples * j, 1] = data[j, 0].Turns[n];
                    //result[i + numSamples * j, 2] = data[j, 0].ZeroCrossings[n];
                    //result[i + numSamples * j, 3] = data[j, 1].Turns[n];
                    //result[i + numSamples * j, 4] = data[j, 1].ZeroCrossings[n];


                    double r0 = data[j, 0].RMS[_num], r1 = data[j, 1].RMS[_num];

                    if ((r0 >= r1) && (r0 / r1 >= threshold))
                        result[i + numSamples * j, 0] = r0 / r1;
                    else if ((r0 < r1) && (r1 / r0 >= threshold)) result[i + numSamples * j, 0] = -r1 / r0;
                    else result[i + numSamples * j, 0] = 0;

                    result[i + numSamples * j, 1] = NormalizeData(data[j, 0].Turns[_num], 10, 130, 0, 5);
                    result[i + numSamples * j, 2] = NormalizeData(data[j, 0].ZeroCrossings[_num], 15, 70, 0, 5);
                    result[i + numSamples * j, 3] = NormalizeData(data[j, 1].Turns[_num], 10, 130, 0, 5);
                    result[i + numSamples * j, 4] = NormalizeData(data[j, 1].ZeroCrossings[_num], 15, 70, 0, 5);
                }


            }


            return result;
        }

        /// <summary>
        /// Преобразование массива данных в массив для работы алгоритма k-means для двух кластеров
        /// </summary>
        /// <param name="numSamples">Количество примеров</param>
        /// <param name="d1">Индекс первого кластера в Data</param>
        /// <param name="d2">Индекс второго кластера в Data</param>
        /// <returns></returns>
        double[,] DataToArray(int numSamples, int d1, int d2)
        {
            int numFeatures = 5;
            int width = data[d1, 0].X.Count;// длина массива точек для расчета выборки
            double threshold = 1.3d;        // пороговое значение для отношения амплитуд

            int _num, k;

            double[,] result = new double[numSamples * 2, numFeatures];

            for (int n = 0; n < 2; n++)
            {
                if (n == 0) k = d1;
                else k = d2;

                for (int i = 0; i < numSamples; i++)
                {
                    _num = 700 + ((width - 700 - 300) / numSamples) * i;
                    //result[i + numSamples * j, 0] = data[j, 0].RMS[n] - data[j, 1].RMS[n];
                    //result[i + numSamples * j, 1] = data[j, 0].Turns[n];
                    //result[i + numSamples * j, 2] = data[j, 0].ZeroCrossings[n];
                    //result[i + numSamples * j, 3] = data[j, 1].Turns[n];
                    //result[i + numSamples * j, 4] = data[j, 1].ZeroCrossings[n];


                    double r0 = data[k, 0].RMS[_num], r1 = data[k, 1].RMS[_num];

                    // самый простой вариант кажется лучше
                    //result[i + numSamples * n, 0] = r0 / r1;

                    if ((r0 >= r1) && (r0 / r1 >= threshold))
                        result[i + numSamples * n, 0] = r0 / r1;
                    else if ((r0 < r1) && (r1 / r0 >= threshold)) result[i + numSamples * n, 0] = -r1 / r0;
                    else result[i + numSamples * n, 0] = 0;

                    //if (r0 >= r1)
                    //{
                    //    if (r0 / r1 >= threshold) // 0 > 1
                    //        result[i + numSamples * n, 0] = r0 / r1;
                    //    result[i + numSamples * n, 0] = r1 / r0;
                    //}

                    //if (r0 < r1)
                    //{
                    //    if (r1 / r0 >= threshold)// 0 < 1
                    //        result[i + numSamples * n, 0] = -r1 / r0;
                    //    else
                    //        result[i + numSamples * n, 0] = -r0 / r1;
                    //}

                    result[i + numSamples * n, 1] = NormalizeData(data[k, 0].Turns[_num], 10, 130, 0, 5);
                    result[i + numSamples * n, 2] = NormalizeData(data[k, 0].ZeroCrossings[_num], 15, 70, 0, 5);
                    result[i + numSamples * n, 3] = NormalizeData(data[k, 1].Turns[_num], 10, 130, 0, 5);
                    result[i + numSamples * n, 4] = NormalizeData(data[k, 1].ZeroCrossings[_num], 15, 70, 0, 5);
                }
            }

            return result;
        }

        double[,] DataToArray(int numSamples, int[] d)
        {
            int numFeatures = 9;
            int width = data[d[0], 0].X.Count;// длина массива точек для расчета выборки
            double threshold = 1.3d;        // пороговое значение для отношения амплитуд

            int _num, k;

            double[,] result = new double[numSamples * d.Length, numFeatures];

            for (int n = 0; n < d.Length; n++)
            {
                k = d[n];

                for (int i = 0; i < numSamples; i++)
                {
                    _num = 700 + ((width - 700 - 300) / numSamples) * i; // перебор точек 700 - 20000

                    double r0 = data[k, 0].RMS[_num], r1 = data[k, 1].RMS[_num];

                    if ((r0 >= r1) && (r0 / r1 >= threshold))
                        result[i + numSamples * n, 4] = r0 / r1;
                    else if ((r0 < r1) && (r1 / r0 >= threshold))
                        result[i + numSamples * n, 4] = -r1 / r0;
                    else result[i + numSamples * n, 4] = 0;

                    //result[i + numSamples * n, 4] = NormalizeData(result[i + numSamples * n, 4], -3.2d, 20.6d, -13, 21);
                    result[i + numSamples * n, 1] = NormalizeData(data[k, 0].Turns[_num], 10, 130, 0, 10);
                    result[i + numSamples * n, 2] = NormalizeData(data[k, 0].ZeroCrossings[_num], 15, 70, 0, 5);
                    result[i + numSamples * n, 3] = NormalizeData(data[k, 1].Turns[_num], 10, 130, 0, 10);
                    result[i + numSamples * n, 0] = NormalizeData(data[k, 1].ZeroCrossings[_num], 15, 70, 0, 5);

                    //result[i + numSamples * n, 1] = data[k, 0].Turns[_num];
                    //result[i + numSamples * n, 2] = data[k, 0].ZeroCrossings[_num];
                    //result[i + numSamples * n, 3] = data[k, 1].Turns[_num];
                    //result[i + numSamples * n, 0] = data[k, 1].ZeroCrossings[_num];

                    result[i + numSamples * n, 5] = NormalizeData(data[k, 0].Deviation[_num], 0.00004d, 0.0035d, 0, 10);
                    result[i + numSamples * n, 6] = NormalizeData(data[k, 1].Deviation[_num], 0.00004d, 0.0035d, 0, 10);
                    result[i + numSamples * n, 7] = NormalizeData(data[k, 0].RMS[_num], 0.00005d, 0.0023d, 0, 10);
                    result[i + numSamples * n, 8] = NormalizeData(data[k, 1].RMS[_num], 0.00005d, 0.0023d, 0, 10);

                }
            }

            //double maxs1 = -500, mins1 = 500;
            //double maxs2 = -500, mins2 = 500;

            //for (int i = 0; i < numSamples * d.Length - 1; i++)
            //{
            //    if (result[i, 7] > maxs1) maxs1 = result[i, 7];
            //    if (result[i, 7] < mins1) mins1 = result[i, 7];

            //    if (result[i, 8] > maxs2) maxs2 = result[i, 8];
            //    if (result[i, 8] < mins2) mins2 = result[i, 8];
            //}

            return result;
        }

        double[,] DataToArray(int number)
        {
            int numSamples = 5000;
            int numFeatures = 5;
            int width = data[number, 0].X.Count;// длина массива точек для расчета выборки

            int _num;

            double[,] result = new double[numSamples, numFeatures];

            for (int i = 0; i < numSamples; i++)
            {
                _num = 700 + ((width - 700 - 300) / numSamples) * i;

                double r0 = data[number, 0].RMS[_num], r1 = data[number, 1].RMS[_num];


                //double dr;
                //if ((r0 >= r1) && (r0 / r1 >= 1.3))
                //    dr = r0 / r1;
                //else if ((r0 < r1) && (r1 / r0 >= 1.3))
                //    dr = -r1 / r0;
                //else dr = 0;
                //result[i, 0] = dr;

                result[i, 0] = r0 / r1;

                result[i, 1] = data[number, 0].Turns[_num];
                result[i, 2] = data[number, 0].ZeroCrossings[_num];
                result[i, 3] = data[number, 1].Turns[_num];
                result[i, 4] = data[number, 1].ZeroCrossings[_num];
            }
            return result;
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

        string[] loadNames()
        {
            string[] paths = new string[60];

            for (int i = 1; i <= 6; i++)
            {
                paths[i - 1] = @"D:\YandexDisk\Программирование\GestureRecognize\GestureRecognize\EMG_2Chs\EMG-S10\HC-";
                paths[i + 5] = @"D:\YandexDisk\Программирование\GestureRecognize\GestureRecognize\EMG_2Chs\EMG-S10\I-I";
                paths[i + 11] = @"D:\YandexDisk\Программирование\GestureRecognize\GestureRecognize\EMG_2Chs\EMG-S10\L-L";
                paths[i + 17] = @"D:\YandexDisk\Программирование\GestureRecognize\GestureRecognize\EMG_2Chs\EMG-S10\M-M";
                paths[i + 23] = @"D:\YandexDisk\Программирование\GestureRecognize\GestureRecognize\EMG_2Chs\EMG-S10\R-R";
                paths[i + 29] = @"D:\YandexDisk\Программирование\GestureRecognize\GestureRecognize\EMG_2Chs\EMG-S10\T-I";
                paths[i + 35] = @"D:\YandexDisk\Программирование\GestureRecognize\GestureRecognize\EMG_2Chs\EMG-S10\T-L";
                paths[i + 41] = @"D:\YandexDisk\Программирование\GestureRecognize\GestureRecognize\EMG_2Chs\EMG-S10\T-M";
                paths[i + 47] = @"D:\YandexDisk\Программирование\GestureRecognize\GestureRecognize\EMG_2Chs\EMG-S10\T-R";
                paths[i + 53] = @"D:\YandexDisk\Программирование\GestureRecognize\GestureRecognize\EMG_2Chs\EMG-S10\T-T";
            }

            for (int i = 0; i < 60; i += 6)
            {
                for (int j = 1; j <= 6; j++)
                {
                    paths[i + j - 1] = paths[i + j - 1].Insert(paths[i + j - 1].Length, j.ToString());
                    paths[i + j - 1] = paths[i + j - 1].Insert(paths[i + j - 1].Length, ".csv");
                }
            }

            return paths;
        }

        int Min(int[,] mass) 
        {
            int min1 = (mass[0, 0] <= mass[0, 1]) ? 0 : 1;
            int min2 = (mass[1, 0] <= mass[1, 1]) ? 2 : 3;

            return (min1 <= min2) ? min1 : min2;
        }

    }

    [Serializable]
    class Scope
    {
        public double[] center, min, max;

        public Scope(int features)
        {
            center = new double[features];
            min = new double[features];
            max = new double[features];

            for (int i = 0; i < features; i++)
            {
                min[i] = int.MaxValue;
                max[i] = int.MinValue;
                center[i] = 0;
            }
        }
    }
}
