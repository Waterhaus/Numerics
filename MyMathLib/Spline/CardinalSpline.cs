using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMathLib
{
    class CardinalSpline
    {

        public static double ForwardDifferences(double[] div)
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
        public static double DividedDifferences(double[] v)
        {
            int k = v.Length - 2;
            double temp = (double)MyMathLib.MyMath.Basic.Factorial(k);

            return ForwardDifferences(v) / temp;

        }

        public static double positive(double x)
        {
            if (x > 0) return x;
            else return 0;
        }

        public static double pol(double x, double t,int n)
        {
            double f = Math.Pow(positive(t - x), n - 1);

            return (f);
        }

        public static double BasisCardinal(int degree, double x)
        {
            double p = (double)degree;
            //x out of supp
            if (x < 0 || x > p) return 0;
            double t_i = Math.Floor(x);
            double[] y = new double[degree + 1];

            for (int i = 0; i < y.Length; i++)
            {
                y[i] = pol(x, i, degree);
            }

            double div_y = DividedDifferences(y);

            return div_y; 
            

        }

        public static double Cardinal(int degree, double x, double t_i, double h)
        {
            return BasisCardinal(degree, (x - t_i) / h);
        }



    }
}
