using System;
using System.Windows.Forms;

namespace lab1
{
    public partial class Form1 : Form
    {
        public int I = 9;
        private int _Mm = 2015;
        private double _Y = Math.Pow(2, 9);

        /// <summary>
        /// Количество случайных чисел, необходимых для 
        /// получения нового случайного числа
        /// </summary>
        private const int R = 10;

        /// <summary>
        /// Таблица случаных чисел
        /// </summary>
        private double[] _Rand = new double[R] {0.87, 0.33, 0.29, 0.12, 0.95,
        0.66, 0.19, 0.61, 0.27, 0.88} ;

        /// <summary>
        /// Объём выборки
        /// </summary>
        int V = 6000;

        /// <summary>
        /// Число участков разбиения
        /// </summary>
        int N = 16;

        /// <summary>
        /// Мат ожидание
        /// </summary>
        double MX = 0;

        /// <summary>
        /// Дисперсия
        /// </summary>
        double DX = 0;

        /// <summary>
        /// Второй момент
        /// </summary>
        double m2 = 0;

        /// <summary>
        /// Третий момент
        /// </summary>
        double m3 = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void generateButtonClick(object sender, EventArgs e)
        {
            double[] x = CalcValues();
            PrintDiagramm(x);
            PrintStaticFunc(x);
            MX = CalcMatOzh(x);
            DX = CalcDisp(x,MX);
            m2 = Moments(x, 2);
            m3 = Moments(x, 3);
            label1.Text = "Мат. ожидание = " + Convert.ToString(MX);
            label2.Text = "Дисперсия = " + Convert.ToString(DX);
            label3.Text = "Второй момент = " + Convert.ToString(m2);
            label4.Text = "Третий момент = " + Convert.ToString(m3);
        }

        public double CalcMatOzh(double[] x)
        {
            double M = 0;
            for (int i = 0;i < x.Length; i++)
            {
                M += x[i];
            }
            return M/x.Length;
        }

        public double CalcDisp(double[] x, double Mx)
        {
            double D = 0;
            for (int i = 0; i < x.Length; i++)
            {
                D += x[i] * x[i];
            }
            D /= x.Length;
            return (D - Mx * Mx) * x.Length / (x.Length - 1);
        }

        public double Moments(double[] x, int degree)
        {
            double sum = 0;
            for (int i = 0; i < x.Length; i++)
            {
                sum += Math.Pow(x[i], degree);
            }
            return sum / x.Length;
        }

        public double[] CalcValues()
        {
            double[] x = new double[V];
            for (int i = 0; i < V; i++)
            {
                x[i] = Rnd();
            }
            return x;
        }

        public double Rnd()
        {
            double s = 0.0;
            for (int k = 0; k < R; k ++)
            {
                s += _Rand[k]; // сумма случаных чисел
            }
            for (int k = 1; k < R; k ++)
            {
                // сдвиг случаных чисел
                _Rand[k - 1] = _Rand[k]; 
            }
            s -= Math.Truncate(s);
            _Rand[R - 1] = s;
            return s;
        }

        public int CountLessValueFromRnds(double[] x, double value)
        {
            int count = 0;

            for (int i = 0; i < x.Length - 1; i++)
            {
                if (x[i] < value)
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Вывод гистограммы частот
        /// </summary>
        /// <param name="x"></param>
        public void PrintDiagramm(double[] x)
        {
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = 1;
            chart1.ChartAreas[0].AxisY.Minimum = 0;

            double[] xData = new double[V];
            double[] yData = new double[V];

            for (int i = 0; i < N; i++)
            {
                double count = 0;
                for (int j = 0; j < x.Length; j++)
                {
                    if (x[j] >= i * ((double)1 / N) && x[j] < (i + 1) * ((double)1 / N))
                    {
                        count += 1;
                    }
                }

                xData[i] = i * ((double)1 / N);
                yData[i] = count / (V * ((double)1 / N));
            }

            chart1.Series[0].Points.DataBindXY(xData, yData);
            chart1.Series[0].Name = "Гистограмма частот";

        }

        /// <summary>
        /// Вывод статической функции
        /// </summary>
        /// <param name="x"></param>
        public void PrintStaticFunc(double[] x)
        {
            chart2.ChartAreas[0].AxisX.Minimum = 0;
            chart2.ChartAreas[0].AxisX.Maximum = 1;
            chart2.ChartAreas[0].AxisY.Minimum = 0;
            chart2.ChartAreas[0].AxisY.Maximum = 1;
            int count = 1000;

            double[] xs = new double[1000];
            double[] ys = new double[1000];
            double s = 0;
            for (int i = 0; i < 1000; i++)
            {
                double y = (double)CountLessValueFromRnds(x, s) / V;

                xs[i] = s;
                ys[i] = y;
                s += 0.001;
            }

            chart2.Series[0].Points.DataBindXY(xs, ys);
            chart2.Series[0].Name = "Статическая функция";
        }
    }
}
