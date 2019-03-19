using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using AForge.Fuzzy;

namespace GestureRecognize
{
    public partial class Fuzzy : Form
    {
        int CentrDefu = 100;

        LingVar RMS0, RMS1, 
            Turns0, Turns1, 
            Zeros0, Zeros1, 
            Deviation0, Deviation1,
            deltaRMS, output;

        Database fuzzyDB;

        InferenceSystem IS;

        Data[,] data;

        Scope[] scopes;

        List<float>[,,,,][] RulesCalc;
        int[,,,,] Rules;
        
        int[] nums = new int[] { 0, 6, 12, 18, 24, 30, 36, 42, 48, 54 };


        public Fuzzy()
        {
            InitializeComponent();

            FuzzyInit();
        }

        private void Fuzzy_Load(object sender, EventArgs e)
        {
            
        }

        void FuzzyInit()
        {
            InitEvenly();

            //InitCuckoo();

        }

        void InitCuckoo()
        {
            openFileDialog1.InitialDirectory = @"D:\YandexDisk\Программирование\GestureRecognize\GestureRecognize\bin\Debug";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (FileStream fs = File.OpenRead(openFileDialog1.FileName))
                {
                    scopes = (Scope[])bf.Deserialize(fs);
                }
            }

            deltaRMS = new LingVar(0, "deltarms", -4f, 23, scopes, nums);
            Turns0 = new LingVar(1, "turns0", 0, 175, scopes, nums);
            Zeros0 = new LingVar(2, "zeros0", 14, 80, scopes, nums);
            Turns1 = new LingVar(3, "turns1", 0, 175, scopes, nums);
            Zeros1 = new LingVar(4, "zeros1", 14, 80, scopes, nums);

            const int numOutTerms = 10;
            output = new LingVar("out", numOutTerms, 1, numOutTerms, 1f);

            fuzzyDB = new Database();

            fuzzyDB.AddVariable(deltaRMS.lv);
            fuzzyDB.AddVariable(Turns0.lv);
            fuzzyDB.AddVariable(Turns1.lv);
            fuzzyDB.AddVariable(Zeros0.lv);
            fuzzyDB.AddVariable(Zeros1.lv);
            fuzzyDB.AddVariable(output.lv);

            loadDataOnly();

            IS = new InferenceSystem(fuzzyDB, new CentroidDefuzzifier(CentrDefu));

            string rule = "";

            for (int i = 0; i < nums.Length; i++)
            {
                rule = "IF deltarms IS " + deltaRMS.terms[i].Name;
                rule += " AND turns0 IS " + Turns0.terms[i].Name;
                rule += " AND turns1 IS " + Turns1.terms[i].Name;
                rule += " AND zeros0 IS " + Zeros0.terms[i].Name;
                rule += " AND zeros1 IS " + Zeros1.terms[i].Name;

                rule += " THEN out IS " + output.terms[i].Name;

                IS.NewRule(i.ToString(), rule);
            }


            //chart.RangeX = new AForge.Range(14, 80);
            //chart.AddDataSeries("0", Color.Black, AForge.Controls.Chart.SeriesType.Line, 3, true);
            //chart.AddDataSeries("1", Color.Red, AForge.Controls.Chart.SeriesType.Line, 3, true);
            //chart.AddDataSeries("2", Color.Blue, AForge.Controls.Chart.SeriesType.Line, 3, true);
            //chart.AddDataSeries("3", Color.Green, AForge.Controls.Chart.SeriesType.Line, 3, true);
            //chart.AddDataSeries("4", Color.Pink, AForge.Controls.Chart.SeriesType.Line, 3, true);
            //chart.AddDataSeries("5", Color.Purple, AForge.Controls.Chart.SeriesType.Line, 3, true);
            //chart.AddDataSeries("6", Color.Orange, AForge.Controls.Chart.SeriesType.Line, 3, true);
            //chart.AddDataSeries("7", Color.Yellow, AForge.Controls.Chart.SeriesType.Line, 3, true);
            //chart.AddDataSeries("8", Color.Lime, AForge.Controls.Chart.SeriesType.Line, 3, true);
            //chart.AddDataSeries("9", Color.Coral, AForge.Controls.Chart.SeriesType.Line, 3, true);




            //// get membership of some points to the cool fuzzy set
            //double[][,] chartValues = new double[10][,];
            //for (int i = 0; i < 10; i++)
            //    chartValues[i] = new double[140, 2];

            //// showing the shape of the linguistic variable - the shape of its labels memberships from start to end
            //int j = 0;
            //for (float x = 14; x < 80; x += 0.5f, j++)
            //{
            //    double[] ys = new double[10];

            //    for (int m = 0; m < 10; m++)
            //    {
            //        //ys[m] = deltaRMS.lv.GetLabelMembership("deltarms" + (m*6).ToString(), x);
            //        ys[m] = Zeros0.lv.GetLabelMembership("zeros0" + (m * 6).ToString(), x);

            //    }

            //    for (int m = 0; m < 10; m++)
            //    {
            //        chartValues[m][j, 0] = x;
            //        chartValues[m][j, 1] = ys[m];
            //    }
            //}

            //// plot membership to a chart
            //for (int m = 0; m < 10; m++)
            //{
            //    chart.UpdateDataSeries(m.ToString(), chartValues[m]);
            //}

        }

        void InitEvenly()
        {
            const int numTerms = 10;
            const float cross = 1.8f;

            RMS0 = new LingVar("rms0", numTerms, 0.4f / 10000f, 23f / 10000f, cross);
            RMS1 = new LingVar("rms1", numTerms, 0.4f / 10000f, 23f / 10000f, cross);
            Deviation0 = new LingVar("deviation0", numTerms, 0.5f / 10000f, 34.26f / 10000f, cross);
            Deviation1 = new LingVar("deviation1", numTerms, 0.5f / 10000f, 34.26f / 10000f, cross);
            Turns0 = new LingVar("turns0", numTerms, 0, 175, cross);
            Turns1 = new LingVar("turns1", numTerms, 0, 175, cross);
            Zeros0 = new LingVar("zeros0", numTerms, 14, 80, cross);
            Zeros1 = new LingVar("zeros1", numTerms, 14, 80, cross);
            deltaRMS = new LingVar("deltarms", numTerms, -12.5f, 20.8f, cross);

            const int numOutTerms = 10;

            output = new LingVar("out", numOutTerms, 1, numOutTerms, 1.3f);

            fuzzyDB = new Database();
            //fuzzyDB.AddVariable(RMS1.lv);
            //fuzzyDB.AddVariable(RMS2.lv);
            //fuzzyDB.AddVariable(Deviation1.lv);
            //fuzzyDB.AddVariable(Deviation2.lv);
            fuzzyDB.AddVariable(Turns0.lv);
            fuzzyDB.AddVariable(Turns1.lv);
            fuzzyDB.AddVariable(Zeros0.lv);
            fuzzyDB.AddVariable(Zeros1.lv);
            fuzzyDB.AddVariable(deltaRMS.lv);
            fuzzyDB.AddVariable(output.lv);

            RulesCalc = new List<float>[numTerms, numTerms, numTerms, numTerms, numTerms][];
            Rules = new int[numTerms, numTerms, numTerms, numTerms, numTerms];

            for (int q = 0; q < numTerms; q++)
                for (int w = 0; w < numTerms; w++)
                    for (int e = 0; e < numTerms; e++)
                        for (int r = 0; r < numTerms; r++)
                            for (int t = 0; t < numTerms; t++)
                            {
                                RulesCalc[q, w, e, r, t] = new List<float>[numOutTerms] { new List<float>(), new List<float>(), new List<float>(), new List<float>(), new List<float>(), new List<float>(), new List<float>(), new List<float>(), new List<float>(), new List<float>() };
                                Rules[q, w, e, r, t] = 0;
                            }

            loadData();

            for (int q = 0; q < numTerms; q++)
                for (int w = 0; w < numTerms; w++)
                    for (int e = 0; e < numTerms; e++)
                        for (int r = 0; r < numTerms; r++)
                            for (int t = 0; t < numTerms; t++)
                            {
                                float[] average = new float[numOutTerms];      // среднее арифметическое

                                for (int i = 0; i < numOutTerms; i++) // количество листов (столбцов с цифрами) / количество выходных термов
                                {
                                    int count = RulesCalc[q, w, e, r, t][i].Count; // количество цифр в столбце

                                    if (count == 0) // если столбец пуст
                                        average[i] = 0;

                                    else // если в столбце что-то есть
                                        average[i] = RulesCalc[q, w, e, r, t][i].Average();
                                }

                                // посчитали среднее для всех столбцов, можно выбирать правило
                                float _t = average.Max();
                                if (_t != 0)
                                    Rules[q, w, e, r, t] = Array.IndexOf(average, _t) + 1;
                                else Rules[q, w, e, r, t] = 0;
                            }

            #region Заполнение нулевых ячеек таблицы правил

            //bool zers = true, zer = false;
            //double countenv = (Math.Pow(3, 5) - 1);
            //float div = 2f;

            //int ZEROSAAAAA1 = 0;
            //int ZEROSAAAAA2 = 0;

            //while (zers)
            //{
            //    ZEROSAAAAA1 = 0;
            //    for (int q = 0; q < numTerms; q++)
            //        for (int w = 0; w < numTerms; w++)
            //            for (int e = 0; e < numTerms; e++)
            //                for (int r = 0; r < numTerms; r++)
            //                    for (int t = 0; t < numTerms; t++)
            //                    {
            //                        if (Rules[q, w, e, r, t] == 0)
            //                        {
            //                            ZEROSAAAAA1++;
            //                            int[] environs = new int[numOutTerms];
            //                            int notclass = 0, inclass = 0;
            //                            int outofrange = 0;
            //                            zer = true;
            //                            int fuu = 0;

            //                            for (int a = -1; a <= 1; a++)
            //                                for (int s = -1; s <= 1; s++)
            //                                    for (int d = -1; d <= 1; d++)
            //                                        for (int f = -1; f <= 1; f++)
            //                                            for (int g = -1; g <= 1; g++)
            //                                            {
            //                                                if ((q + a >= 0 && w + s >= 0 && e + d >= 0 && r + f >= 0 && t + g >= 0) &&
            //                                                        (q + a < numTerms && w + s < numTerms && e + d < numTerms && r + f < numTerms && t + g < numTerms))
            //                                                {
            //                                                    if (!(q == a && w == s && e == d && r == f && t == g))
            //                                                    {
            //                                                        if (Rules[q + a, w + s, e + d, r + f, t + g] == 0)
            //                                                            notclass++;
            //                                                        else
            //                                                        {
            //                                                            environs[Rules[q + a, w + s, e + d, r + f, t + g] - 1]++;
            //                                                            inclass++;
            //                                                        }
            //                                                    }
            //                                                }
            //                                                else
            //                                                {
            //                                                    int fu = 0;
            //                                                    if ((q + a < 0) || (q + a >= numTerms))
            //                                                    {
            //                                                        fu++;
            //                                                    }
            //                                                    if ((w + s < 0) || (w + s >= numTerms))
            //                                                    {
            //                                                        fu++;
            //                                                    }
            //                                                    if ((e + d < 0) || (e + d >= numTerms))
            //                                                    {
            //                                                        fu++;
            //                                                    }
            //                                                    if ((r + f < 0) || (r + f >= numTerms))
            //                                                    {
            //                                                        fu++;
            //                                                    }
            //                                                    if ((t + g < 0) || (t + g >= numTerms))
            //                                                    {
            //                                                        fu++;
            //                                                    }

            //                                                    if (fu > fuu) fuu = fu;

            //                                                    outofrange++;
            //                                                }


            //                                            }

            //                            int max = environs.Max();

            //                            if ((fuu == 0) && (max != 0))
            //                            {
            //                                if (inclass > 242f / div)
            //                                {
            //                                    Rules[q, w, e, r, t] = Array.IndexOf(environs, max) + 1;
            //                                }
            //                            }
            //                            else if (fuu == 1)
            //                            {
            //                                if (inclass > 161f / div)
            //                                {
            //                                    Rules[q, w, e, r, t] = Array.IndexOf(environs, max) + 1;
            //                                }
            //                            }
            //                            else if (fuu == 2)
            //                            {
            //                                if (notclass > 107f / div)
            //                                {
            //                                    Rules[q, w, e, r, t] = Array.IndexOf(environs, max) + 1;
            //                                }
            //                            }
            //                            else if (fuu == 3)
            //                            {
            //                                if (notclass > 71f / div)
            //                                {
            //                                    Rules[q, w, e, r, t] = Array.IndexOf(environs, max) + 1;
            //                                }
            //                            }
            //                            else if (fuu == 4)
            //                            {
            //                                if (notclass > 47f / div)
            //                                {
            //                                    Rules[q, w, e, r, t] = Array.IndexOf(environs, max) + 1;
            //                                }
            //                            }
            //                            else if (fuu == 5)
            //                            {
            //                                if (notclass > 31f / div)
            //                                {
            //                                    Rules[q, w, e, r, t] = Array.IndexOf(environs, max) + 1;
            //                                }
            //                            }


            //                        }
            //                    }

            //    if (!zer) zers = false;
            //    zer = false;
            //    if (ZEROSAAAAA1 == ZEROSAAAAA2)
            //        zers = false;
            //    else
            //        ZEROSAAAAA2 = ZEROSAAAAA1;

            //    Console.WriteLine(ZEROSAAAAA1);
            //}
            #endregion


            #region Оптимизация правил
            //RulesOptimizer[] rulOpt = new RulesOptimizer[numOutTerms];
            //for (int i = 0; i < rulOpt.Length; i++)
            //{
            //    rulOpt[i] = new RulesOptimizer();
            //}

            //for (int q = 0; q < numTerms; q++)
            //    for (int w = 0; w < numTerms; w++)
            //        for (int e = 0; e < numTerms; e++)
            //            for (int r = 0; r < numTerms; r++)
            //                for (int t = 0; t < numTerms; t++)
            //                {
            //                    if (Rules[q, w, e, r, t] != 0)
            //                    {
            //                        rulOpt[Rules[q, w, e, r, t] - 1].addRule(q, w, e, r, t);
            //                    }
            //                }

            //foreach (var item in rulOpt)
            //{
            //    item.optimize();
            //}
            #endregion

            /* Формат правил
             * 
             *            лист
             *             \/ 
             * Прi = | 1 | 1 | 1 | 2 | 1 | <- массив листов
             *       | 5 | 3 |   | 4 |   |
             *       |   | 6 |   |   |   |
             * 
             * rules[i]       - List<int>[]
             * rules[i][k]    - List<int>
             * rules[i][k][n] - int
            */

            IS = new InferenceSystem(fuzzyDB, new CentroidDefuzzifier(CentrDefu));

            string rule;
            for (int q = 0; q < numTerms; q++)
                for (int w = 0; w < numTerms; w++)
                    for (int e = 0; e < numTerms; e++)
                        for (int r = 0; r < numTerms; r++)
                            for (int t = 0; t < numTerms; t++)
                            {
                                if (Rules[q, w, e, r, t] != 0)
                                {
                                    rule = "";
                                    rule += "IF turns0 IS " + Turns0.terms[q].Name;
                                    rule += " AND turns1 IS " + Turns1.terms[w].Name;
                                    rule += " AND zeros0 IS " + Zeros0.terms[e].Name;
                                    rule += " AND zeros1 IS " + Zeros1.terms[r].Name;
                                    rule += " AND deltarms IS " + deltaRMS.terms[t].Name;
                                    rule += " THEN out IS " + output.terms[Rules[q, w, e, r, t] - 1].Name;

                                    IS.NewRule(q.ToString() + w.ToString() + e.ToString() + r.ToString() + t.ToString(), rule);
                                }
                            }


            #region Тестирование для оптимизации
            //string rule = "";

            //for (int q = 0; q < rulOpt.Length; q++) // перечисляет выходы от 0 до 9
            //{
            //    for (int w = 0; w < rulOpt[q].Rules.Count; w++) // перебирает правила для каждого выхода
            //    {
            //        Clause[][] clauses = new Clause[rulOpt[q].Rules[w].Length][]; // размерность 5 х N

            //        for (int e = 0; e < rulOpt[q].Rules[w].Length; e++) // от 0 до 4 перебирает лингвистические переменные
            //        {
            //            clauses[e] = new Clause[rulOpt[q].Rules[w][e].Count]; // кол-во клауз = кол-ву вариантов значений лингвистических переменных

            //            for (int r = 0; r < clauses[e].Length; r++)
            //            {
            //                int itemp = rulOpt[q].Rules[w][e][r];

            //                switch (e)
            //                {
            //                    case 0:
            //                        clauses[e][r] = new Clause(Turns0.lv, Turns0.terms[itemp]);
            //                        break;
            //                    case 1:
            //                        clauses[e][r] = new Clause(Turns1.lv, Turns1.terms[itemp]);
            //                        break;
            //                    case 2:
            //                        clauses[e][r] = new Clause(Zeros0.lv, Zeros0.terms[itemp]);
            //                        break;
            //                    case 3:
            //                        clauses[e][r] = new Clause(Zeros1.lv, Zeros1.terms[itemp]);
            //                        break;
            //                    case 4:
            //                        clauses[e][r] = new Clause(deltaRMS.lv, deltaRMS.terms[itemp]);
            //                        break;
            //                }
            //            }
            //        }

            //        rule = "IF ";

            //        string temp;
            //        for (int e = 0; e < clauses.Length; e++)
            //        {
            //            temp = "(";
            //            for (int r = 0; r < clauses[e].Length - 1; r++)
            //            {
            //                temp += clauses[e][r] + " OR ";
            //            }
            //            temp += clauses[e][clauses[e].Length - 1] + ")";

            //            if (e == clauses.Length - 1)
            //                rule += temp;
            //            else
            //                rule += temp + " AND ";
            //        }

            //        rule += " THEN out IS " + output.terms[q].Name;

            //        //rule += "IF turns0 IS " + Turns0.terms[q].Name;
            //        //rule += " AND turns1 IS " + Turns1.terms[w].Name;
            //        //rule += " AND zeros0 IS " + Zeros0.terms[e].Name;
            //        //rule += " AND zeros1 IS " + Zeros1.terms[r].Name;
            //        //rule += " AND deltarms IS " + deltaRMS.terms[t].Name;
            //        //rule += " THEN out IS " + output.terms[Rules[q, w, e, r, t] - 1].Name;

            //        IS.NewRule(q.ToString() + w.ToString(), rule);

            //    }

            //}
            #endregion
        }

        void loadDataOnly()
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
        }

        void loadData()
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

            int numFeatures = 5, _num, k, numSamples = 5000;
            double threshold = 1.3d;        // пороговое значение для отношения амплитуд
            
            int width = data[nums[0], 0].X.Count;


            int[] addr = new int[numFeatures];
            string[] forRule = new string[numFeatures + 1];

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

                    forRule[0] = chooseTerm(data[k, 0].Turns[_num], Turns0);
                    forRule[1] = chooseTerm(data[k, 1].Turns[_num], Turns1);
                    forRule[2] = chooseTerm(data[k, 0].ZeroCrossings[_num], Zeros0);
                    forRule[3] = chooseTerm(data[k, 1].ZeroCrossings[_num], Zeros1);
                    forRule[4] = chooseTerm(dr, deltaRMS);
                    forRule[5] = chooseTerm(n + 1, output);

                    IS = new InferenceSystem(fuzzyDB, new CentroidDefuzzifier(CentrDefu));

                    string rule = "";
                    rule += "IF turns0 IS " + forRule[0];
                    rule += " AND turns1 IS " + forRule[1];
                    rule += " AND zeros0 IS " + forRule[2];
                    rule += " AND zeros1 IS " + forRule[3];
                    rule += " AND deltarms IS " + forRule[4];
                    rule += " THEN out IS " + forRule[5];

                    IS.NewRule("test", rule);

                    IS.SetInput(Turns0.lv.Name, data[k, 0].Turns[_num]);
                    IS.SetInput(Turns1.lv.Name, data[k, 1].Turns[_num]);
                    IS.SetInput(Zeros0.lv.Name, data[k, 0].ZeroCrossings[_num]);
                    IS.SetInput(Zeros1.lv.Name, data[k, 1].ZeroCrossings[_num]);
                    IS.SetInput(deltaRMS.lv.Name, (float)dr);

                    FuzzyOutput fuzzyOutput = IS.ExecuteInference("out");

                    for (int j = 0; j < addr.Length; j++)
                    {
                        addr[j] = int.Parse(forRule[j].Substring(forRule[j].Length - 1));
                    }
                    
                    RulesCalc[addr[0], addr[1], addr[2], addr[3], addr[4]][n].Add(fuzzyOutput.OutputList[0].FiringStrength);
                }
            }
        }

        string chooseTerm(double X, LingVar lingVar)
        {
            string termName = "";
            double _t1 = 0, _t2 = 0;

            for (int i = 0; i < lingVar.terms.Length; i++)
            {
                _t1 = lingVar.lv.GetLabelMembership(lingVar.terms[i].Name, (float)X);
                if (_t1 > _t2)
                {
                    _t2 = _t1;
                    termName = lingVar.terms[i].Name;
                }

            }

            return termName;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            List<double> output1 = new List<double>();
            List<double> output2 = new List<double>();

            List<int> outputFilter = new List<int>();


            for (int n = 0; n < 10; n++)
            {
                for (int i = 1000; i < data[0, 0].X.Count; i++)
                {
                    output2.Add(n + 1);
                }
            }

            openFileDialog1.InitialDirectory = @"D:\YandexDisk\Программирование\GestureRecognize\GestureRecognize\bin\Debug";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (FileStream fs = File.OpenRead(openFileDialog1.FileName))
                {
                    output1 = (List<double>)bf.Deserialize(fs);
                }
            }
            double fo = 1.1;
            double fo2 = 1.17;
            double fo3 = 1.25;

            int start;
            int end;
            int width = 700;
            int[] outs = new int[nums.Length + 1];

            for (int o = 0; o < outs.Length; o++)
                outs[o] = 0;

            for (int i = 0; i < output1.Count; i++)
            {
                if (i < width) start = 0;
                else start = i - width;

                end = i;

                for (int j = start; j < end; j++)
                {
                    if (output1[j] == -1)
                        outs[outs.Length - 1]++;
                    else
                    {
                        if ((start > 38000) && (start < 76000))
                            outs[(int)Math.Round(output1[j] * fo)]++;
                        else if ((start > 76000) && (start < 133000))
                            outs[(int)Math.Round(output1[j] * fo2)]++;
                        else if (start > 133000)
                        {
                            if (output1[j] * fo3 <= outs.Length - 1)
                            {
                                outs[(int)Math.Round(output1[j] * fo3)]++;
                            }
                            else
                            {
                                outs[outs.Length - 1]++;
                            }
                        }
                        else
                            outs[(int)Math.Round(output1[j])]++;
                    }
                }
                outputFilter.Add(Array.IndexOf(outs, outs.Max()));

                for (int o = 0; o < outs.Length; o++)
                    outs[o] = 0;
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

            for (int i = 0; i < output1.Count; i++)
            {
                //out1.Points.AddXY(i, output1[i]);
                out1.Points.AddXY(i, outputFilter[i]);
                out2.Points.AddXY(i, output2[i]);
            }

            OutChart.ChartAreas.Add("Plot");
            OutChart.ChartAreas["Plot"].AxisX.Maximum = outputFilter.Count;
            OutChart.ChartAreas["Plot"].AxisX.Minimum = 0;
            OutChart.ChartAreas["Plot"].AxisY.Maximum = 11;
            OutChart.ChartAreas["Plot"].AxisY.Minimum = -1;

            OutChart.Series.Add(out1);
            OutChart.Series.Add(out2);
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {

            double threshold = 1.3d;        // пороговое значение для отношения амплитуд
            List<double> output1 = new List<double>();
            List<double> output2 = new List<double>();

            List<int> outputFilter = new List<int>();


            int k;
            //int[] nums = new int[] { 1, 7, 13, 19, 25, 31, 37, 43, 49, 55 };
            int[] nums1 = new int[] { 0, 6, 12, 18, 24, 30, 36, 42, 48, 54 };

            for (int n = 0; n < nums1.Length; n++)
            {
                k = nums1[n];

                for (int i = 1000; i < data[k, 0].X.Count; i++)
                {
                    double r0 = data[k, 0].RMS[i], r1 = data[k, 1].RMS[i], dr;

                    if ((r0 >= r1) && (r0 / r1 >= threshold))
                        dr = r0 / r1;
                    else if ((r0 < r1) && (r1 / r0 >= threshold))
                        dr = -r1 / r0;
                    else dr = 0;

                    //dr = r0 / r1;

                    IS.SetInput("turns0", data[k, 0].Turns[i]);
                    IS.SetInput("turns1", data[k, 1].Turns[i]);
                    IS.SetInput("zeros0", data[k, 0].ZeroCrossings[i]);
                    IS.SetInput("zeros1", data[k, 1].ZeroCrossings[i]);
                    IS.SetInput("deltarms", (float)dr);
                    
                    try
                    {
                        //outputint.Add((int)Math.Round(IS.Evaluate("out"), MidpointRounding.AwayFromZero));
                        output1.Add(IS.Evaluate("out"));

                    }

                    catch
                    {
                        output1.Add(-1);
                    }

                    output2.Add(n + 1);
                }
                
            }

            //BinaryFormatter bf1 = new BinaryFormatter();
            //using (FileStream fs = File.Create("plotTest7complexdR.bin"))
            //    bf1.Serialize(fs, output1);

            int start;
            int end;
            int width = 700;
            int[] outs = new int[nums.Length + 1 ];

            for (int o = 0; o < outs.Length; o++)
                outs[o] = 0;

            for (int i = 0; i < output1.Count; i++)
            {
                if (i < width) start = 0;
                else start = i - width;

                end = i;

                for (int j = start; j < end; j++)
                {
                    if (output1[j] == -1)
                        outs[nums.Length]++;
                    else
                        outs[(int)Math.Round(output1[j]*1.3)]++;
                }
                outputFilter.Add(Array.IndexOf(outs, outs.Max()));

                for (int o = 0; o < outs.Length; o++)
                    outs[o] = 0;
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

            for (int i = 0; i < output1.Count; i++)
            {
                //out1.Points.AddXY(i, output1[i]);
                out1.Points.AddXY(i, outputFilter[i]);
                out2.Points.AddXY(i, output2[i]);
            }

            OutChart.ChartAreas.Add("Plot");
            OutChart.ChartAreas["Plot"].AxisX.Maximum = outputFilter.Count;
            OutChart.ChartAreas["Plot"].AxisX.Minimum = 0;
            OutChart.ChartAreas["Plot"].AxisY.Maximum = 11;
            OutChart.ChartAreas["Plot"].AxisY.Minimum = -1;

            OutChart.Series.Add(out1);
            OutChart.Series.Add(out2);

        }
    }
    
    class LingVar
    {
        private float min, max, range;
        public FuzzySet[] terms;
        public LinguisticVariable lv;
        

        public LingVar(string name, int numTerms, float min, float max, float cross)
        {
            this.min = min;
            this.max = max;
            range = Math.Abs(max - min);

            float left, right, center;

            terms = new FuzzySet[numTerms];
            lv = new LinguisticVariable(name, min, max);

            center = min + 0 * range / (numTerms - 1) * (1 / cross);
            right = min + (0 + 1) * range / (numTerms - 1) * (1 / cross);

            terms[0] = new FuzzySet(name + "-0", new TrapezoidalFunction(center, right, TrapezoidalFunction.EdgeType.Right));
            lv.AddLabel(terms[0]);

            for (int i = 1; i < numTerms - 1; i++)
            {
                left = min + (i - 1) * range / (numTerms - 1) * (1 / cross);
                center = min + i * range / (numTerms - 1) * (1 / cross);
                right = min + (i + 1) * range / (numTerms - 1) * (1 / cross);

                terms[i] = new FuzzySet(name + "-" + i.ToString(), new TrapezoidalFunction(left, center, right));

                lv.AddLabel(terms[i]);
            }

            left = min + (numTerms - 1 - 1) * range / (numTerms - 1) * (1 / cross);
            center = min + (numTerms - 1) * range / (numTerms - 1) * (1 / cross);

            terms[numTerms - 1] = new FuzzySet(name + "-" + (numTerms - 1).ToString(), new TrapezoidalFunction(left, center, TrapezoidalFunction.EdgeType.Left));
            lv.AddLabel(terms[numTerms - 1]);
        }

        public LingVar(int number, string name, float min, float max, Scope[] scopes, int[] d)
        {
            this.min = min;
            this.max = max;
            range = Math.Abs(max - min);

            terms = new FuzzySet[d.Length];
            lv = new LinguisticVariable(name, min, max);

            for (int i = 0; i < terms.Length; i++)
            {
                terms[i] = new FuzzySet(name + d[i].ToString(),
                    new TrapezoidalFunction(
                        (float)scopes[d[i]].min[number],
                        (float)scopes[d[i]].center[number],
                        (float)scopes[d[i]].max[number])
                    );

                lv.AddLabel(terms[i]);
            }
        }
    }

    class RulesOptimizer
    {
        public List<List<int>[]> Rules = new List<List<int>[]>();

        public RulesOptimizer()
        {

        }

        public void addRule(int l1, int l2, int l3, int l4, int l5)
        {
            Rules.Add(new List<int>[5] {
                new List<int>(),
                new List<int>(),
                new List<int>(),
                new List<int>(),
                new List<int>()
            });

            Rules[Rules.Count - 1][0].Add(l1);
            Rules[Rules.Count - 1][1].Add(l2);
            Rules[Rules.Count - 1][2].Add(l3);
            Rules[Rules.Count - 1][3].Add(l4);
            Rules[Rules.Count - 1][4].Add(l5);

        }

        public void optimize()
        {
            bool stop;
            do
            {
                stop = optimize(ref Rules);
            } while (!stop);
        }

        bool optimize(ref List<List<int>[]> rules)
        {
            bool stop = true; int rulesCount = rules.Count;
            List<List<int>[]> rules2 = new List<List<int>[]>();

            /* Формат правил
             * 
             *            лист
             *             \/ 
             * Прi = | 1 | 1 | 1 | 2 | 1 | <- массив листов
             *       | 5 | 3 |   | 4 |   |
             *       |   | 6 |   |   |   |
             * 
             * rules[i]       - List<int>[]
             * rules[i][k]    - List<int>
             * rules[i][k][n] - int
            */

            for (int i = 0; i < rules.Count; i++)
            // цикл по всем правилам начиная с 0
            {
                bool gluing = false;

                for (int j = i + 1; j < rules.Count; j++)
                // проход по всем правилам, следующим за i как в сортировке пузырьком
                {
                    int _counter = 0; // счетчик совпадающих мест
                    bool[] _compare = new bool[] { false, false, false, false, false };

                    for (int k = 0; k < _compare.Length; k++)
                    {
                        if (inclusion(rules[i][k], rules[j][k]))
                        {
                            _counter++;
                            _compare[k] = true;
                        }
                    }

                    if (_counter == 4) // если совпали все кроме одного
                    {
                        // индекс совпадения
                        int n = Array.IndexOf(_compare, false);
                        
                        rules2.Add(rules[i]);
                        foreach (var item in rules[j][n])
                        {
                            rules2[rules2.Count - 1][n].Add(item);
                        }

                        rules2[rules2.Count - 1][n].Sort();
                        rules2[rules2.Count - 1][n] = rules2[rules2.Count - 1][n].Distinct().ToList();

                        rules.Remove(rules[j]);
                        j--;
                        gluing = true;
                    }
                    else if (_counter == 5) // если совпали все
                    {
                        rules.Remove(rules[j]);
                        j--;
                    }

                }
                if (!gluing)
                    rules2.Add(rules[i]);
            }

            bool remove;
            do
            {
                remove = false;
                for (int i = 0; i < rules2.Count; i++)
                {
                    for (int j = i + 1; j < rules2.Count; j++)
                    {
                        if (equal(rules2[i], rules2[j]))
                        {
                            rules2.Remove(rules2[j]);
                            j--;
                            remove = true;
                        }
                    }
                }
            } while (remove);

            rules.Clear();
            rules.AddRange(rules2);
            rules2.Clear();

            if (rules.Count != rulesCount) stop = false;
            else stop = true;

            return stop;
        }

        #region мусор ли?
        //void optimize(ref List<string[]> rules)
        //{
        //    bool gluingGlobal = false;
        //    List<string[]> rules2 = new List<string[]>();

        //    for (int i = 0; i < rules.Count; i++)
        //    {
        //        bool gluing = false;
        //        for (int j = i + 1; j < rules.Count; j++)
        //        {
        //            int fuu = 0;
        //            bool[] condi = new bool[] { false, false, false, false, false };

        //            for (int k = 0; k < condi.Length; k++)
        //            {
        //                if (rules[i][k] == rules[j][k])
        //                {
        //                    fuu++;
        //                    condi[k] = true;
        //                }
        //            }

        //            if (fuu == 4)
        //            {
        //                int n = Array.IndexOf(condi, false);

        //                string[] str = new string[condi.Length];

        //                for (int k = 0; k < condi.Length; k++)
        //                    str[k] = rules[i][k];

        //                str[n] = rules[i][n] + "," + rules[j][n];

        //                gluing = gluingGlobal = true;
        //                rules2.Add(str);

        //                rules.Remove(rules[j]);
        //            }

        //        }
        //        if (!gluing)
        //            rules2.Add(rules[i]);

        //    }

        //    for (int i = 0; i < rules.Count; i++)
        //    {
        //        for (int j = i + 1; j < rules.Count; j++)
        //        {
        //            if (equal(rules2[i], rules2[j]))
        //            {
        //                rules2.Remove(rules2[j]);
        //            }
        //        }
        //    }

        //    rules.Clear();
        //    rules.AddRange(rules2);
        //    rules2.Clear();

        //    if (gluingGlobal)
        //        optimize(ref rules);
        //}
        #endregion

        bool equal(string[] str1, string[] str2)
        {
            bool outb = true;
            for (int i = 0; i < str1.Length; i++)
            {
                if (str1[i] != str2[i])
                    outb = false;
            }

            return outb;
        }

        bool inclusion(List<int> l1, List<int> l2)
        {
            if (l1.Count == l2.Count)
                return equal(l1, l2);
            
            else
            {
                bool incl = true;

                if (l1.Count > l2.Count)
                {
                    foreach (var item in l2)
                    {
                        if (l1.FindAll(x => x == item).Count == 0)
                            incl = false;
                    }
                }

                else
                {
                    foreach (var item in l1)
                    {
                        if (l2.FindAll(x => x == item).Count == 0)
                            incl = false;
                    }
                }

                return incl;
            }
        }

        bool equal(List<int> l1, List<int> l2)
        {
            if (l1.Count != l2.Count)
                return false;

            for (int i = 0; i < l1.Count; i++)
            {
                if (l1[i] != l2[i])
                    return false;
            }

            return true;
        }

        bool equal(List<int>[] lm1, List<int>[] lm2)
        {
            if (lm1.Length != lm2.Length)
                return false;

            for (int i = 0; i < lm1.Length; i++)
            {
                if (!equal(lm1[i], lm2[i]))
                    return false;
            }

            return true;
        }
        
    }
}
