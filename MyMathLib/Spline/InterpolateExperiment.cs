using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMathLib.Spline
{
    public class InterpolateExperiment
    {

        public static double[] GetCardinalValue(int degree, double h)
        {
            double[] mas = new double[degree - 1];
            for (int i = 0; i < degree - 1; i++)
            {
                mas[i] = CardinalSpline.Cardinal(degree, (i + 1) * h, 0, h);

            }
            return mas;
        }

        public static Matrix CreatePeriodicInterpolationMatrix(double[] ksi, int size)
        {
            int degree = ksi.Length + 1;
            Matrix A = new Matrix(size);
            

            int N = size - 1;
            int p = degree - 1;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < ksi.Length; j++)
                {
                    if (i - j >= 0)
                        if (!(i > N - p + 1 && (i - j) > N - p + 1))
                            A[i, i - j] = ksi[j];
                }
            }

            N = A.Length.n;
            for (int j = 1; j < ksi.Length; j++)
            {
                for (int i = 0; i + j < ksi.Length; i++)
                {
                    A[j - 1, N - degree + 1 + i + j] = ksi[ksi.Length - 1 - i];
                    A[N - degree + 1 + i + j, j - 1] = ksi[ksi.Length - 1 - i];
                }
            }



            return A;

        }

        public static Matrix MinInterpolationMatrix(int degree, double h, int size)
        {
            double[] ksi_p = GetCardinalValue(degree, h);
            double[] mas = GetCardinalValue(2 * degree, h);
            double[] ksi_2p = new double[ksi_p.Length + 1];

            int p = degree - 1;
            for (int i = 0; i < ksi_2p.Length; i++)
            {
                ksi_2p[i] = mas[p + i];
            }
            ksi_2p[0] = 2 * ksi_2p[0];
            Console.WriteLine("all vec " + new Vector(mas).ToString("0.000000"));
            Console.WriteLine("half vec " + new Vector(ksi_2p).ToString("0.000000"));

            Matrix A0 = CreatePeriodicInterpolationMatrix(ksi_p, size);
            //Console.WriteLine( A0);
            Matrix KSI = CreatePeriodicInterpolationMatrix(ksi_2p, size);
            //Console.WriteLine(KSI);


            Matrix S = new Matrix(2 * size);

            int N = size - 1;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    S[i, j] = A0[i, j];
                    S[i + size, j + size] = -A0[i, j];
                    S[i + size, j] = KSI[i, j];
                }
            }
            //Console.WriteLine(S);
            return S;
        }


        public static Vector Interpolate(Vector y_knots, int degree, double h)
        {
            
            Vector b = new Vector(2*y_knots.Length);
            for (int i = 0; i < y_knots.Length; i++)
            {
                b[i] = y_knots[i];
            }
            Matrix A = MinInterpolationMatrix(degree, h, y_knots.Length);
            //Console.WriteLine(A);
            double EPS = 0.000001d;
            Vector coefs = Solver.BCGSTAB(A, b, EPS);

            return coefs;
        }

    }
}
