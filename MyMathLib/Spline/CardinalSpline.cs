using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMathLib
{

    /*
      public static double[] GetCardinalValue(int degree)
        {
            double[] mas = new double[degree - 1];
            double h = 1;
            for (int i = 0; i < degree - 1; i++)
            {
                mas[i] = CardinalSpline.Cardinal(degree, (i + 1) * h, 0, h);

            }
            return mas;
        }
         */


    public interface ICardinalStratagy
    {
        double   Cardinal(int degree, double x, double t_i, double h);
        double[] GetCardinalValue(int degree);

    }

    //Алгоритм подсчета базисных сплайнов через разделенные разности
    public class DividedDifferencesStategy : ICardinalStratagy
    {
        private double ForwardDifferences(double[] div)
        {


            for (int i = 0; i < div.Length; i++)
            {
                for (int j = 0; j < div.Length - i - 1; j++)
                {
                    div[j] = div[j + 1] - div[j];
                }
            }

            return div[0];

        }

        private double DividedDifferences(double[] v)
        {
            int k = v.Length - 2;
            double temp = (double)MyMathLib.MyMath.Basic.Factorial(k);

            return ForwardDifferences(v) / temp;

        }

        private double positive(double x)
        {
            if (x > 0) return x;
            else return 0;
        }

        private double pol(double x, double t, int n)
        {
            double f = Math.Pow(positive(t - x), n - 1);

            return (f);
        }

        private double BasisCardinal(int degree, double x)
        {
            double p = (double)degree;
            //x out of supp
            if (x < 0 || x > p) return 0;
            if (degree == 1)
            {
                return 1d;
            }
            double t_i = Math.Floor(x);
            double[] y = new double[degree + 1];

            for (int i = 0; i < y.Length; i++)
            {
                y[i] = pol(x, i, degree);
            }

            double div_y = DividedDifferences(y);

            return div_y;


        }

        public double Cardinal(int degree, double x, double t_i, double h)
        {
            return BasisCardinal(degree, (x - t_i) / h);
        }

        public double[] GetCardinalValue(int degree)
        {
            double[] mas = new double[degree - 1];
            double h = 1;
            for (int i = 0; i < degree - 1; i++)
            {
                mas[i] = Cardinal(degree, (i + 1) * h, 0, h);

            }
            return mas;
        }
    }

    public class CardinalSpline
    {
        private ICardinalStratagy calculate;
        private static ICardinalStratagy static_calculate;

        static CardinalSpline()
        {
            static_calculate = new DividedDifferencesStategy();
        }
        public CardinalSpline()
        {
            calculate = new DividedDifferencesStategy();
        }

        public CardinalSpline(ICardinalStratagy strategy)
        {
            this.calculate = strategy;
        }

        public static double Cardinal(int degree, double x, double t_i, double h)
        {
            return static_calculate.Cardinal(degree, x, t_i, h);
        }

        //Функция считает значение сплайн-функции в одной точке
        public double CalculateSpline(double x, Vector c, double a_border, double h, int degree)
        {
            double a_start = a_border - ((double)(degree - 1)) * h;
            int p = degree;
            if (x < a_start) return 0d;
            double S = 0d;

            for (int i = 0; i < c.Length; i++)
            {
                S = S + c[i] * calculate.Cardinal(degree, x, a_start + i * h, h);
            }
            return S;
        }

        public Vector SplineVector(double x, double a_border, double b_border, int SIZE, int degree)
        {
            Vector ksi = new Vector(SIZE + degree - 2);
            double h = MyMath.Basic.GetStep(SIZE, a_border, b_border);
            double a_start = a_border - ((double)(degree - 1)) * h;
            for (int i = 0; i < ksi.Length; i++)
            {
                ksi[i] = calculate.Cardinal(degree, x, a_start + i*h, h);
            }

            return ksi;

        }


        //Функция возвращает массив точек(значений функции) на отрезке [a b]
        public Vector GetVectorFunction(int GridSize, double a_border, double b_border, Vector c, double step, int degree)
        {
            Vector f = new Vector(GridSize);
            double h = MyMath.Basic.GetStep(GridSize, a_border, b_border);

            for (int i = 0; i < GridSize; i++)
            {
                f[i] = CalculateSpline(a_border + i * h, c, a_border, step, degree);
            }
            return f;
        }


        public static double[] pr_coef(int degree)
        {
            int n = degree;
            double[] coef = new double[n + 1];
            for (int k = 0; k <= n; k++)
            {
                coef[k] = MyMath.Basic.BinomialCoeff(n, k) / Math.Pow(2d, n - 1);
            }
            return coef;
                
        }
      




    }
}
