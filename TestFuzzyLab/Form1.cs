using System;
using System.Windows.Forms;
using AForge.Fuzzy;

namespace TestFuzzyLab
{
    public partial class Form1 : Form
    {
        const double Ymin = -0.009d, Ymax = 0.004d,
                     RMSmin = 0d, RMSmax = 0.003d;
        const int turnsMin = 0, turnsMax = 50;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // rms
            FuzzySet fsLowRMS = new FuzzySet("Low", new TrapezoidalFunction(
                (float)RMSmin, 
                Divide(RMSmin, RMSmax, 3, 1), 
                TrapezoidalFunction.EdgeType.Right));

            FuzzySet fsAverageRMS = new FuzzySet("Average", new TrapezoidalFunction(
                (float)RMSmin,
                Divide(RMSmin, RMSmax, 3, 1),
                Divide(RMSmin, RMSmax, 3, 2), 
                (float)RMSmax));

            FuzzySet fsHighRMS = new FuzzySet("High", new TrapezoidalFunction(
                Divide(RMSmin, RMSmax, 3, 2),
                (float)RMSmax, 
                TrapezoidalFunction.EdgeType.Left));

            // повороты
            FuzzySet fsLowTurns = new FuzzySet("Low", new TrapezoidalFunction(
                turnsMin,
                Divide(turnsMin, turnsMax, 3, 1),
                TrapezoidalFunction.EdgeType.Right));

            FuzzySet fsAverageTurns = new FuzzySet("Average", new TrapezoidalFunction(
                turnsMin,
                Divide(turnsMin, turnsMax, 3, 1),
                Divide(turnsMin, turnsMax, 3, 2),
                turnsMax));

            FuzzySet fsHighTurns = new FuzzySet("High", new TrapezoidalFunction(
                Divide(turnsMin, turnsMax, 3, 2),
                turnsMax,
                TrapezoidalFunction.EdgeType.Left));


            LinguisticVariable lvRMS = new LinguisticVariable("RMS", (float)RMSmin, (float)RMSmax);
            lvRMS.AddLabel(fsLowRMS);
            lvRMS.AddLabel(fsAverageRMS);
            lvRMS.AddLabel(fsHighRMS);

            LinguisticVariable lvTurns = new LinguisticVariable("Turns", turnsMin, turnsMax);
            lvTurns.AddLabel(fsLowTurns);
            lvTurns.AddLabel(fsAverageTurns);
            lvTurns.AddLabel(fsHighTurns);

            // выход
            FuzzySet fsСlonus = new FuzzySet("True", new SingletonFunction(1));
            FuzzySet fsClonusNot = new FuzzySet("False", new SingletonFunction(0));

            LinguisticVariable lvClonus = new LinguisticVariable("Clonus", 0, 1);
            lvClonus.AddLabel(fsСlonus);
            lvClonus.AddLabel(fsClonusNot);

            Database fuzzyDB = new Database();
            fuzzyDB.AddVariable(lvClonus);
            fuzzyDB.AddVariable(lvRMS);
            fuzzyDB.AddVariable(lvTurns);

            InferenceSystem IS = new InferenceSystem(fuzzyDB, new CentroidDefuzzifier(1000));

            IS.NewRule("Rule 1", "IF RMS IS Low OR Turns IS Low THEN Clonus IS False");
             
            IS.NewRule("Rule 2", "IF RMS IS High AND Turns IS High THEN Clonus IS True");
        }

        float Divide(double min, double max, int parts, int N)
        {
            return (float)(min + N * (max - min) / parts);
        }
    }
}
