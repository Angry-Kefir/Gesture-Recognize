using System.Windows.Forms;
using System;
using System.Windows.Forms.DataVisualization.Charting;

namespace GestureRecognize
{
    public partial class Form1 : Form
    {

        //const double Ymin = -0.009d, Ymax = 0.004d,
        //             RMSmin = 0d, RMSmax = 0.003d;
        const float  Ymin = -0.009f, Ymax = 0.004f,
                     RMSmin = 0f, RMSmax = 0.003f;

        const int turnsMin = 0, turnsMax = 50;

        DataAnalysis DA;
        Kmeans KM;
        Fuzzy fuzzy;
        KNN wKNN;

        Data[,] data;
        //Data dataCh1 = new Data();
        //Data dataCh2 = new Data();

        string path;
        bool dataLoaded = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void kmeansButton_Click(object sender, EventArgs e)
        {
            if (dataLoaded)
            {
                KM = new Kmeans(ref data);
                KM.Show();
            }

            else
            {
                KM = new Kmeans();
                KM.Show();
            }
        }


        private void Form1_Load(object sender, System.EventArgs e)
        {

            openFileDialog1.InitialDirectory = @"D:\YandexDisk\Программирование\GestureRecognize\GestureRecognize\EMG_2Chs\EMG-S1";

        }

        private void LoadButtonClick(object sender, EventArgs e)
        {
            data = new Data[decimal.ToInt32(countDataset.Value), 2];

            for (int i = 0; i < decimal.ToInt32(countDataset.Value); i++)
            {
                data[i, 0] = new Data();
                data[i, 1] = new Data();

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    path = openFileDialog1.FileName;
                }

                data[i, 0].ReadDatasetLessX(path, 0);
                data[i, 1].ReadDatasetLessX(path, 1);
            }

            dataLoaded = true;
        }

        private void AnalysisButtonClick(object sender, EventArgs e)
        {
            DA = new DataAnalysis(ref data);
            DA.Show();
        }

        private void Fuzzybutton_Click(object sender, EventArgs e)
        {
            fuzzy = new Fuzzy();
            fuzzy.Show();
        }

        private void knnButton_Click(object sender, EventArgs e)
        {
            wKNN = new KNN();
            wKNN.Show();
        }

    }
}