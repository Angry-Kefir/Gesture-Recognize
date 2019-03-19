using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;

namespace GestureRecognize
{
    [Serializable]
    public partial class Data
    {
        #region ПЕРЕМЕННЫЕ

        /// <summary>
        /// Координаты сигнала по Х (время)
        /// </summary>
        public List<double> X = new List<double>();
        /// <summary>
        /// Координаты сигнала по Y (величина напряжения)
        /// </summary>
        public List<double> Y = new List<double>();

        /// <summary>
        /// Среднее квадратическое значение сигнала
        /// </summary>
        public List<double> RMS = new List<double>();
        /// <summary>
        /// Математическое ожидание сигнала
        /// </summary>
        public List<double> ExpVal = new List<double>();
        /// <summary>
        /// Дисперсия сигнала
        /// </summary>
        public List<double> Variance = new List<double>();
        /// <summary>
        /// Среднее квадратическое отклонение сигнала
        /// </summary>
        public List<double> Deviation = new List<double>();

        public List<int> Turns = new List<int>();           // количество поворотов 
        public List<double> PeakValues = new List<double>();// величины пиков поворотов
        public List<int> ZeroCrossings = new List<int>();   // количество пересечений нуля

        //public List<double> Mean = new List<double>();      // среднее по пикам
        // бесполезная фигня

        const double maxValue = 0.01;
        #endregion

        #region ОТКРЫТЫЕ МЕТОДЫ

        /// <summary>
        /// Считывание графика сигнала из файла по одному из каналов.
        /// Одна точка графика занимает в файле одну строку, координаты Y различных каналов записаны через "," .
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="channel"></param>
        public void ReadDatasetLessX(string path, int channel)
        {
            var temp = 0;

            StreamReader objReader = new StreamReader(path);
            string sLine = "";
            List<string> arrText = new List<string>();

            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null)
                    arrText.Add(sLine);
            }

            objReader.Close();

            NumberFormatInfo provider = new NumberFormatInfo
            {
                NumberDecimalSeparator = "."
            };

            foreach (string s in arrText)
            {
                string[] numbers = s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                Y.Add(Convert.ToDouble(numbers[channel], provider));

                X.Add(temp);
                temp++;
            }
        }

        /// <summary>
        /// Ввод синусоиды в качестве данных.
        /// </summary>
        public void InputSin()
        {
            for (double i = 0; i < 6.5d; i += 6.5d / 1000)
            {
                X.Add(i);
                Y.Add(Math.Sin(2 * i));
            }
        }

        /// <summary>
        /// Расчет признаков сигнала в окне.
        /// </summary>
        /// <param name="values">Список точек сигнала (коорд. Y)</param>
        /// <param name="width">Ширина окна</param>
        public void CalculateFeatures(List<double> values, int width)
        {
            int start;
            int end;

            for (int i = 0; i < X.Count; i++)
            {
                if (i < width) start = 0;
                else start = i - Decimal.ToInt32(width);

                end = i;

                CalcFeatureSpace(values, start, end);
            }
        }

        /// <summary>
        /// Расчет среднего квадратического значения сигнала в окне.
        /// </summary>
        /// <param name="width">Ширина окна</param>
        public void CalculateRMS(int width)
        {
            int start;
            int end;

            for (int i = 0; i < X.Count; i++)
            {
                if (i < width) start = 0;
                else start = i - Decimal.ToInt32(width);

                end = i;

                RMS.Add(RootMeanSquare(Y, start, end));
            }
        }

        /// <summary>
        /// Построение гистограммы сигнала в выбранном временном промежутке.
        /// </summary>
        /// <param name="startTime">Начальное время [с]</param>
        /// <param name="endTime">Конечное время [с]</param>
        /// <returns></returns>
        public int[] Histogram(double startTime, double endTime, int numBins)
        {
            //double widthBin = Math.Abs(Y.Max()) / (numBins - 1);
            double widthBin = maxValue / numBins;

            double dT = X[1] - X[0];

            uint group;
            int[] histo = new int[numBins];

            for (int i = (int)(startTime / dT); i < (int)(endTime / dT); i++)
            {
                group = (uint)(Math.Abs(Y[i]) / widthBin);
                //group = (uint)(RMS[i] / widthBin);
                histo[group]++;
            }

            int maxVal = histo.Max();

            for (int i = 0; i < histo.Length; i++)
                histo[i] = (histo[i] * 100) / maxVal;

            return histo;
        }

        /// <summary>
        /// Построение гистограммы сигнала в выбранном временном промежутке.
        /// </summary>
        /// <param name="startTime">Начальное время [с]</param>
        /// <param name="endTime">Конечное время [с]</param>
        /// <param name="magnitude">Минимальная величина перепада</param>
        /// <returns></returns>
        public int[] Histogram(double magnitude, int numBins)
        {
            TurnsCounter(Y, magnitude);

            //double widthBin = Math.Abs(PeakValues.Max()) / (numBins - 1);
            double widthBin = maxValue / numBins;

            uint group;
            int[] histo = new int[numBins];

            for (int i = 0; i < PeakValues.Count - 1; i++)
            {
                group = (uint)(Math.Abs(PeakValues[i]) / widthBin);
                histo[group]++;
            }

            int maxVal = histo.Max();

            for (int i = 0; i < histo.Length; i++)
                histo[i] = (histo[i] * 100) / maxVal;

            return histo;
        }

        /// <summary>
        /// Подсчет количества поворотов сигнала в окне с выбором минимальной величины перепада.
        /// </summary>
        /// <param name="width">Ширина окна</param>
        /// <param name="magnitude">Минимальная величина перепада</param>
        public void CountTurns(int width, double magnitude)
        {
            int start;
            int end;

            for (int i = 0; i < X.Count; i++)
            {
                if (i < width) start = 0;
                else start = i - Decimal.ToInt32(width);

                end = i;

                Turns.Add(TurnsCounter(Y, start, end, magnitude));
            }
        }

        /// <summary>
        /// Подсчет количества переходов сигнала через ноль.
        /// </summary>
        /// <param name="width"></param>
        public void CountZeroCrossing(int width)
        {
            int start;
            int end;

            for (int i = 0; i < X.Count; i++)
            {
                if (i < width) start = 0;
                else start = i - Decimal.ToInt32(width);

                end = i;

                ZeroCrossings.Add(ZeroCrossing(Y, start, end));
            }
        }

        #endregion

        #region ЗАКРЫТЫЕ МЕТОДЫ

        /// <summary>
        /// Расчет признаков сигнала между двумя временными точками.
        /// </summary>
        /// <param name="values">Список точек сигнала (коорд. Y)</param>
        /// <param name="start">Начальная точка отсчета</param>
        /// <param name="end">Конечная точка отсчета</param>
        /// <returns></returns>
        private void CalcFeatureSpace(List<double> values, int start, int end)
        {
            // Расчет пространства признаков в окне от start до end

            double _sumExpVal = 0, _sumVariance = 0; // _sumMean = 0;

            for (int i = start; i < end; i++)
            {
                _sumExpVal += values[i];
                //_sumMean += PeakValues[i];
            }

            ExpVal.Add(_sumExpVal / (end - start));          // мат. ожидание
            //Mean.Add(_sumMean / (end - start));

            for (int i = start; i < end; i++)
                _sumVariance += Math.Pow(values[i] - ExpVal[ExpVal.Count - 1], 2);

            Variance.Add(_sumVariance / (end - start - 1));         // дисперсия

            Deviation.Add(Math.Sqrt(Variance[Variance.Count - 1])); // СКО

            //rValue.kurtosis = (CentralMathMoment(values, start, end, 4) / Math.Pow(rValue.deviation, 4)) - 3; // эксцесс

            //rValue.skewness = CentralMathMoment(values, start, end, 3) / Math.Pow(rValue.deviation, 3);       // асимметрия

        }

        /// <summary>
        /// Расчет среднеквадратического значения сигнала между двумя временными точками.
        /// </summary>
        /// <param name="values">Список точек сигнала (коорд. Y)</param>
        /// <param name="start">Начальная точка отсчета</param>
        /// <param name="end">Конечная точка отсчета</param>
        /// <returns></returns>
        private double RootMeanSquare(List<double> values, int start, int end)
        {
            // Расчет среднеквадратического значения сигнала
            //      в окне от start до end

            double sum = 0;
            for (int i = start; i < end; i++)
            {
                sum += values[i] * values[i];
            }
            return Math.Sqrt(sum / (end - start));
        }

        /// <summary>
        /// Расчет количества поворотов сигнала между двумя временными точками.
        /// </summary>
        /// <param name="values">Список точек сигнала (коорд. Y)</param>
        /// <param name="start">Начальная точка отсчета</param>
        /// <param name="end">Конечная точка отсчета</param>
        /// <param name="magnitude">Минимальная величина перепада</param>
        /// <returns></returns>
        private int TurnsCounter(List<double> values, int start, int end, double magnitude)
        {
            int counter = 0;

            bool direction = false;
            bool oldDirection = false;

            double turnPointValue;
            double oldTurnPointValue = 0d;

            for (int i = start; i < end; i++)
            {
                if (values[i] != 0 && values[i + 1] != 0)
                {

                    #region определение направления роста
                    if (Math.Sign(values[i]) == Math.Sign(values[i + 1]))
                    {
                        if (Math.Sign(values[i]) == 1)
                        {
                            if (values[i] > values[i + 1])
                            {
                                direction = true;
                            }

                            if (values[i] < values[i + 1])
                            {
                                direction = false;
                            }
                        }

                        if (Math.Sign(values[i]) == -1)
                        {
                            if (values[i] > values[i + 1])
                            {
                                direction = false;
                            }

                            if (values[i] < values[i + 1])
                            {
                                direction = true;
                            }
                        }
                    }

                    else if (Math.Sign(values[i]) == 1)
                    {
                        direction = false;
                    }

                    else if (Math.Sign(values[i]) == -1)
                    {
                        direction = true;
                    }
                    #endregion


                    if (direction != oldDirection)
                    {
                        turnPointValue = values[i];

                        if (Math.Abs(turnPointValue - oldTurnPointValue) > magnitude)
                        {
                            counter++;
                            oldTurnPointValue = turnPointValue;
                        }
                    }

                    oldDirection = direction;
                }
            }

            return counter;
        }

        /// <summary>
        /// Расчет количества поворотов сигнала между двумя временными точками.
        /// </summary>
        /// <param name="values">Список точек сигнала (коорд. Y)</param>
        /// <param name="start">Начальная точка отсчета</param>
        /// <param name="end">Конечная точка отсчета</param>
        /// <param name="magnitude">Минимальная величина перепада</param>
        /// <returns></returns>
        private void TurnsCounter(List<double> values, double magnitude)
        {
            bool direction = false;
            bool oldDirection = false;

            double turnPointValue;
            double oldTurnPointValue = 0d;

            for (int i = 0; i < X.Count - 1; i++)
            {
                if (values[i] != 0 && values[i + 1] != 0)
                {
                    #region определение направления роста
                    if (Math.Sign(values[i]) == Math.Sign(values[i + 1]))
                    {
                        if (Math.Sign(values[i]) == 1)
                        {
                            if (values[i] > values[i + 1])
                            {
                                direction = true;
                            }

                            if (values[i] < values[i + 1])
                            {
                                direction = false;
                            }
                        }

                        if (Math.Sign(values[i]) == -1)
                        {
                            if (values[i] > values[i + 1])
                            {
                                direction = false;
                            }

                            if (values[i] < values[i + 1])
                            {
                                direction = true;
                            }
                        }
                    }

                    else if (Math.Sign(values[i]) == 1)
                    {
                        direction = false;
                    }

                    else if (Math.Sign(values[i]) == -1)
                    {
                        direction = true;
                    }
                    #endregion

                    if (direction != oldDirection)
                    {
                        turnPointValue = values[i];

                        if (Math.Abs(turnPointValue - oldTurnPointValue) > magnitude)
                        {
                            PeakValues.Add(values[i]);
                            oldTurnPointValue = turnPointValue;
                        }
                        else PeakValues.Add(0d); // не уверена в необходимости
                    }

                    else PeakValues.Add(0d); // не уверена в необходимости

                    oldDirection = direction;
                }
                else PeakValues.Add(0d); // не уверена в необходимости
            }
        }

        /// <summary>
        /// Расчет количества переходов сигнала через ноль между двумя временными точками.
        /// </summary>
        /// <param name="values">Список точек сигнала (коорд. Y)</param>
        /// <param name="start">Начальная точка отсчета</param>
        /// <param name="end">Конечная точка отсчета</param>
        /// <returns></returns>
        private int ZeroCrossing(List<double> values, int start, int end)
        {
            int counter = 0;

            for (int i = start; i < end; i++)
            {
                if (Math.Sign(values[i]) != Math.Sign(values[i + 1]))
                {
                    counter++;
                }
            }

            return counter;
        }

        private double CentralMathMoment(List<double> values, int start, int end, int N)
        {
            double sum = 0, C;

            for (int i = 0; i < N; i++)
            {
                C = BinomialСoefficient(N, i);

                sum += Math.Pow(-1, i) * StartMathMoment(values, start, end, 1) *
                    StartMathMoment(values, start, end, N - i) * C;
            }

            return sum;
        }

        private double StartMathMoment(List<double> values, int start, int end, int k)
        {
            double sum = 0;

            for (int i = start; i < end; i++)
            {
                sum += Math.Pow(values[i], k);
            }

            return sum / (end - start);
        }

        private double BinomialСoefficient(int n, int k)
        {
            //   k      n!
            // C n = --------
            //       k!(n-k)!

            return Factorial(n) / (Factorial(k) * Factorial(n - k));
        }

        private long Factorial(long x)
        {
            return (x == 0) ? 1 : x * Factorial(x - 1);
        }

        #endregion

        public Data(Data original)
        {
            X = new List<double>(original.X);
            Y = new List<double>(original.Y);

            RMS = new List<double>(original.RMS);
            ExpVal = new List<double>(original.ExpVal);
            Variance = new List<double>(original.Variance);
            Deviation = new List<double>(original.Deviation);

            Turns = new List<int>(original.Turns);
            PeakValues = new List<double>(original.PeakValues);
            ZeroCrossings = new List<int>(original.ZeroCrossings);
        }

        public Data()
        {

        }
    }
}
