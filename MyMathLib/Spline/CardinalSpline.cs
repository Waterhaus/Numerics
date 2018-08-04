using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMathLib
{
    public class CardinalSpline
    {

        private static double ForwardDifferences(double[] div)
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
        private static double DividedDifferences(double[] v)
        {
            int k = v.Length - 2;
            double temp = (double)MyMathLib.MyMath.Basic.Factorial(k);

            return ForwardDifferences(v) / temp;

        }

        private static double positive(double x)
        {
            if (x > 0) return x;
            else return 0;
        }

        private static double pol(double x, double t,int n)
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

        public static Matrix CreateInterpolationMatrix(Grid tau,double h, int degree)
        {
            Matrix A = new Matrix(tau.OriginalCount);

            double[] mas = new double[degree - 1];
            for (int i = 0; i < degree - 1; i++)
            {
                mas[i] = Cardinal(degree, (i+1)*h, 0, h);
                Console.Write(mas[i] + Environment.NewLine);
            }

            
            for (int i = 0; i < tau.OriginalCount; i++)
            {
                for (int j = 0; j < tau.OriginalCount; j++)
                {
                   
                    A[i, j] = Cardinal(degree,tau.GetOrigin(j)+(degree - 1)*h, tau.GetOrigin(i),h);
                }
            }

            int N = A.Length.n;
            for (int j = 1; j < mas.Length; j++)
            {
                for (int i = 0; i + j < mas.Length; i++)
                {
                    A[j - 1, N - degree + 1 + i + j] = mas[i + j - 1];
                }
            }

            return A;
        }


        public static Vector Interpolate(Vector y_knots, Grid grid, int degree, double h)
        {
            if (degree == 2) return y_knots;
            Matrix A = CreateInterpolationMatrix(grid, h, degree);
            double EPS = 0.0001d;
            Vector coefs = Solver.BCGSTAB(A, y_knots, EPS);

            return coefs;
        }



        public static double CalculateCardinalSpline(double x, Vector c, double a, double h, int degree)
        {
            if (x < a) return 0d;
            int index = (int)Math.Floor((x - a) / h);
            double S = 0d;

            if (index < degree - 1)
            {
              
            }
        }

    }
}
