using System.Windows.Forms;

namespace TestKmeans
{
    public partial class Kmeans : Form
    {
        public Kmeans()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            alglib.clusterizerstate s;
            alglib.kmeansreport rep;
            double[,] xy = new double[,] { { 1, 1, 1, 1 }, { 1, 2, 3, 4 }, { 4, 1, -1, -4 }, { 2, 3, 4, 5 }, { 4, 1.5, -1.9, -9 } };

            alglib.clusterizercreate(out s);
            alglib.clusterizersetpoints(s, xy, 2);
            alglib.clusterizersetkmeanslimits(s, 5, 0);
            alglib.clusterizerrunkmeans(s, 3, out rep);

            textBox1.Text += rep.terminationtype.ToString();
            

            alglib.clusterizersetpoints(s, xy, 0);
            alglib.clusterizerrunkmeans(s, 2, out rep);
            textBox1.Text += rep.terminationtype.ToString(); // -5

        }
    }
}
