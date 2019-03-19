using AForge.Fuzzy;
using System;
using System.Windows.Forms;

namespace FuzzyTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*
            LinguisticVariable lvTemperature = new LinguisticVariable("Temperature", 0, 80);
            
            TrapezoidalFunction function1 = new TrapezoidalFunction(10, 15, TrapezoidalFunction.EdgeType.Right);
            FuzzySet fsCold = new FuzzySet("Cold", function1);
            TrapezoidalFunction function2 = new TrapezoidalFunction(10, 15, 20, 25);
            FuzzySet fsCool = new FuzzySet("Cool", function2);
            TrapezoidalFunction function3 = new TrapezoidalFunction(20, 25, 30, 35);
            FuzzySet fsWarm = new FuzzySet("Warm", function3);
            TrapezoidalFunction function4 = new TrapezoidalFunction(30, 35, TrapezoidalFunction.EdgeType.Left);
            FuzzySet fsHot = new FuzzySet("Hot", function4);

            lvTemperature.AddLabel(fsCold);
            lvTemperature.AddLabel(fsCool);
            lvTemperature.AddLabel(fsWarm);
            lvTemperature.AddLabel(fsHot);
            
            textBox1.Text += "Cold; Cool; Warm; Hot" + Environment.NewLine;
            for (float x = 0; x < 80; x += 0.2f)
            {
                float y1 = lvTemperature.GetLabelMembership("Cold", x);
                float y2 = lvTemperature.GetLabelMembership("Cool", x);
                float y3 = lvTemperature.GetLabelMembership("Warm", x);
                float y4 = lvTemperature.GetLabelMembership("Hot", x);

                textBox1.Text += y1.ToString() + " " + y2.ToString() + " " + y3.ToString() + " " + y4.ToString() + Environment.NewLine;
            }
            */

            /*
            FuzzySet fsNear = new FuzzySet("Near", new TrapezoidalFunction(15, 50, TrapezoidalFunction.EdgeType.Right));
            FuzzySet fsMedium = new FuzzySet("Medium", new TrapezoidalFunction(15, 50, 60, 100));
            FuzzySet fsFar = new FuzzySet("Far", new TrapezoidalFunction(60, 100, TrapezoidalFunction.EdgeType.Left));

            LinguisticVariable lvFront = new LinguisticVariable("FrontalDistance", 0, 120);
            lvFront.AddLabel(fsNear);
            lvFront.AddLabel(fsMedium);
            lvFront.AddLabel(fsFar);

            FuzzySet fsZero = new FuzzySet("Zero", new TrapezoidalFunction(-10, 5, 5, 10));
            FuzzySet fsLP = new FuzzySet("LittlePositive", new TrapezoidalFunction(5, 10, 20, 25));
            FuzzySet fsP = new FuzzySet("Positive", new TrapezoidalFunction(20, 25, 35, 40));
            FuzzySet fsVP = new FuzzySet("VeryPositive", new TrapezoidalFunction(35, 40, TrapezoidalFunction.EdgeType.Left));

            LinguisticVariable lvAngle = new LinguisticVariable("Angle", -10, 50);
            lvAngle.AddLabel(fsZero);
            lvAngle.AddLabel(fsLP);
            lvAngle.AddLabel(fsP);
            lvAngle.AddLabel(fsVP);

            Database fuzzyDB = new Database();
            fuzzyDB.AddVariable(lvFront);
            fuzzyDB.AddVariable(lvAngle);

            InferenceSystem IS = new InferenceSystem(fuzzyDB, new CentroidDefuzzifier(1000));

            IS.NewRule("Rule 1", "IF FrontalDistance IS Far THEN Angle IS Zero");
            IS.NewRule("Rule 2", "IF FrontalDistance IS Near THEN Angle IS Positive");
            
            IS.SetInput("FrontalDistance", 20);
            float a = IS.Evaluate("Angle"); // -> 29.999

            try
            {
                FuzzyOutput fuzzyOutput = IS.ExecuteInference("Angle"); // -> 0.87 Positive

                foreach (FuzzyOutput.OutputConstraint oc in fuzzyOutput.OutputList)
                {
                    Console.WriteLine(oc.Label + " - " + oc.FiringStrength.ToString());
                }
            }
            catch (Exception)
            {
            }*/


            FuzzySet fsxM2 = new FuzzySet("xM2", new TrapezoidalFunction(-150, -80, TrapezoidalFunction.EdgeType.Right));
            FuzzySet fsxM1 = new FuzzySet("xM1", new TrapezoidalFunction(-90f, -65f, 0f));
            FuzzySet fsxS = new FuzzySet("xS", new TrapezoidalFunction(-30f, 0, 30f));
            FuzzySet fsxD1 = new FuzzySet("xD1", new TrapezoidalFunction(0f, 65f, 90f));
            FuzzySet fsxD2 = new FuzzySet("xD2", new TrapezoidalFunction(80, 150, TrapezoidalFunction.EdgeType.Left));

            LinguisticVariable lvX = new LinguisticVariable("XDist", -150, 150);
            lvX.AddLabel(fsxM2);
            lvX.AddLabel(fsxM1);
            lvX.AddLabel(fsxS);
            lvX.AddLabel(fsxD1);
            lvX.AddLabel(fsxD2);


            FuzzySet fsfM3 = new FuzzySet("fM3", new TrapezoidalFunction(-190, -140, -90));
            FuzzySet fsfM2 = new FuzzySet("fM2", new TrapezoidalFunction(-140f, -90f, -45f));
            FuzzySet fsfM1 = new FuzzySet("fM1", new TrapezoidalFunction(-90f, -45f, 0f));
            FuzzySet fsfS = new FuzzySet("fS", new TrapezoidalFunction(-30f, 0, 30f));
            FuzzySet fsfD1 = new FuzzySet("fD1", new TrapezoidalFunction(0f, 45f, 90f));
            FuzzySet fsfD2 = new FuzzySet("fD2", new TrapezoidalFunction(45f, 90f, 140f));
            FuzzySet fsfD3 = new FuzzySet("fD3", new TrapezoidalFunction(90, 140, 190));

            LinguisticVariable lvF = new LinguisticVariable("FAngle", -190, 190);
            lvF.AddLabel(fsfM3);
            lvF.AddLabel(fsfM2);
            lvF.AddLabel(fsfM1);
            lvF.AddLabel(fsfS);
            lvF.AddLabel(fsfD1);
            lvF.AddLabel(fsfD2);
            lvF.AddLabel(fsfD3);


            FuzzySet fsaM3 = new FuzzySet("aM3", new TrapezoidalFunction(-45, -30, TrapezoidalFunction.EdgeType.Right));
            FuzzySet fsaM2 = new FuzzySet("aM2", new TrapezoidalFunction(-45f, -30f, -5f));
            FuzzySet fsaM1 = new FuzzySet("aM1", new TrapezoidalFunction(-30f, -17f, -5f));
            FuzzySet fsaS = new FuzzySet("aS", new TrapezoidalFunction(-5f, 0, 5f));
            FuzzySet fsaD1 = new FuzzySet("aD1", new TrapezoidalFunction(5f, 17f, 30f));
            FuzzySet fsaD2 = new FuzzySet("aD2", new TrapezoidalFunction(5f, 30f, 45f));
            FuzzySet fsaD3 = new FuzzySet("aD3", new TrapezoidalFunction(30, 45, TrapezoidalFunction.EdgeType.Left));

            LinguisticVariable lvA = new LinguisticVariable("OutAngle", -45, 45);
            lvA.AddLabel(fsaM3);
            lvA.AddLabel(fsaM2);
            lvA.AddLabel(fsaM1);
            lvA.AddLabel(fsaS);
            lvA.AddLabel(fsaD1);
            lvA.AddLabel(fsaD2);
            lvA.AddLabel(fsaD3);

            Database fuzzyDB = new Database();
            fuzzyDB.AddVariable(lvX);
            fuzzyDB.AddVariable(lvF);
            fuzzyDB.AddVariable(lvA);

            /*
            float x = -100, F = 180, A = 45;
            float Px = 0, Pf = 0, Pa = 0;
            int Nx = -1, Nf = -1, Na = -1;

            float _t = lvX.GetLabelMembership("xM2", x);
            if (_t > Px)
            {
                Px = _t;
                Nx = 0;
            }

            _t = lvX.GetLabelMembership("xM1", x);
            if (_t > Px)
            {
                Px = _t;
                Nx = 1;
            }

            _t = lvX.GetLabelMembership("xS", x);
            if (_t > Px)
            {
                Px = _t;
                Nx = 2;
            }

            _t = lvX.GetLabelMembership("xD1", x);
            if (_t > Px)
            {
                Px = _t;
                Nx = 3;
            }

            _t = lvX.GetLabelMembership("xD2", x);
            if (_t > Px)
            {
                Px = _t;
                Nx = 4;
            }*/



            InferenceSystem IS = new InferenceSystem(fuzzyDB, new CentroidDefuzzifier(1000));

            IS.NewRule("Rule 1", "IF XDist IS xM2 AND FAngle IS fD3 THEN OutAngle IS aD3");
            //Rule r = IS.GetRule("Rule 1");
            //Clause cl = r.Output;

            IS.SetInput("XDist", -100);
            IS.SetInput("FAngle", 180);

            float fu = lvX.GetLabelMembership("xM2", -100); // 0.29
            float fuu = lvF.GetLabelMembership("fD3", 180); // 0.2

            float a = IS.Evaluate("OutAngle"); // -> 38.2

            try
            {
                FuzzyOutput fuzzyOutput = IS.ExecuteInference("OutAngle"); // -> 0.2 ad3

                foreach (FuzzyOutput.OutputConstraint oc in fuzzyOutput.OutputList)
                {
                    Console.WriteLine(oc.Label + " - " + oc.FiringStrength.ToString());
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
