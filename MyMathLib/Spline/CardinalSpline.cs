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
               
            }

            int N = tau.OriginalCount - 1;
            int p = degree - 1;
            for (int i = 0; i < tau.OriginalCount; i++)
            {
                for (int j = 0; j < tau.OriginalCount; j++)
                {
                    if(!(j > N - p + 1 && i > N - p + 1))
                    A[i, j] = Cardinal(degree,tau.GetOrigin(j)+(degree - 1)*h, tau.GetOrigin(i),h);
                }
            }

            N = A.Length.n;
            for (int j = 1; j < mas.Length; j++)
            {
                for (int i = 0; i + j < mas.Length; i++)
                {
                    A[j - 1, N - degree + 1 + i + j] = mas[i];
                    A[N - degree + 1 + i + j, j - 1] = mas[i];
                }
            }
            
            return A;
        }


        public static Vector Interpolate(Vector y_knots, Grid grid, int degree, double h)
        {
            if (degree == 2) return y_knots;
           
            Matrix A = CreateInterpolationMatrix(grid, h, degree);
            Console.WriteLine(A);
            double EPS = 0.000001d;
            Vector coefs = Solver.BCGSTAB(A, y_knots, EPS);

            return coefs;
        }


        public static double GetCoef(int index, Vector c)
        {
            if (index < 0) return c[c.Length + index];
            return c[index];
        }
        public static double CalculateCardinalSpline(double x, Vector c, double a, double h, int degree)
        {
            if (x < a) return 0d;
            int index = (int)Math.Floor((x - a + h/100d) / h);
            double S = 0d;

            for (int i = index - degree + 2; i <= index; i++)
            {
               // Console.Write(Cardinal(degree, x, a + (i - 1) * h, h).ToString("0.000") + " ");
              //  Console.Write(GetCoef(i, c).ToString("0.000") + " ");
               // Console.Write("; ");
                S = S + GetCoef(i, c) * Cardinal(degree, x, a + (i - 1) * h, h);
            }
           // Console.WriteLine("S = " + S + Environment.NewLine);
            return S;
        }

        public static Vector GetVectorFunction(int GridSize, double a_border, double b_border, Vector c, double t_0, double step, int degree)
        {
            Vector f = new Vector(GridSize);
            double h = MyMath.Basic.GetStep(GridSize, a_border, b_border);

            for (int i = 0; i < GridSize; i++)
            {
                f[i] = CalculateCardinalSpline(a_border + i * h,c,t_0,step,degree);
            }
            return f;
        }

    }
}
