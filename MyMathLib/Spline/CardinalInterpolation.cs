using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;


namespace MyMathLib.Spline
{
    public class CardinalInterpolation
    {

        private static double[] GetCardinalValue(int degree)
        {
            double[] mas = new double[degree - 1];
            for (int i = 0; i < degree - 1; i++)
            {
                mas[i] = CardinalSpline.Cardinal(degree, (i + 1) , 0d, 1d);

            }
            return mas;
        }

        public static Matrix CreateInterpolationMatrix(double[] ksi, int size)
        {
            int N = size;
            int p = ksi.Length + 1;
            Matrix A = new Matrix(N, N + p - 2);

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < ksi.Length; j++)
                {
                    A[i, i + j] = ksi[j];
                }
            }
            return A;

        }

        private static Matrix CreateKSIMatrix(int degree, double h, int size)
        {
            int N = size;
            int p = degree;
            double[] ksi = GetCardinalValue(2 * degree);
            Console.WriteLine("deg = " + 2 * degree + " ksi = " + new Vector(ksi).ToString("0.000000"));
            ksi[degree - 1] = 2 * ksi[degree - 1];
            Matrix A = new Matrix(N + p - 2, N + p - 2);

            for (int i = 0; i < N + p - 2; i++)
            {
                for (int j = 0; j < ksi.Length; j++)
                {
                    if (i + j - p + 1 >= 0 && i + j - p + 1 < N + p - 2)
                        A[i, i + j - p + 1] = ksi[j];
                }
            }
            return A;

        }

        private static Matrix Create_LagrangeInterpolationMatrix(int degree, double h, int size)
        {
            double[] ksi_p = GetCardinalValue(degree);
            Matrix B = CreateInterpolationMatrix(ksi_p, size);
            Matrix KSI = CreateKSIMatrix(degree, h, size);

            int N = size;
            int p = degree;

            Matrix S = new Matrix(2 * N + p - 2);

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N + p - 2; j++)
                {
                    S[i, j] = B[i, j];
                    S[N + j, N + p - 2 + i] = -B[i, j];
                }
            }

            for (int i = 0; i < N + p - 2; i++)
            {
                for (int j = 0; j < N + p - 2; j++)
                {
                    S[N + i, j] = h * KSI[i, j];
                }
            }

            return S;

        }


        private static Vector Integrate(Vector f, int degree, double h)
        {
            int N = f.Length;
            int p = degree;
            double[] ksi = GetCardinalValue(degree);
     
            Vector I = new Vector(N + p - 2);
            double f0 = 0;
            double fN = 0;

            for (int i = 0; i < I.Length; i++)
                {
                    for (int j = 1; j < ksi.Length - 1; j++)
                    {
                    if(i + j - p + 1 >= 0 && i + j - p + 1 < N)
                    {
                        I[i] += f[i + j - p + 1] * ksi[j];
                        
                    }
                    if (i - p + 1 >= 0) f0 = f[i - p + 1];
                    if (i - p + ksi.Length < N) fN = f[i - p + ksi.Length];
                    I[i] += (f0 + fN) / 2d;
                    I[i] *= h;
                }
                }
        
    


            //Console.WriteLine("skal = " + I);
            return I;
        }

        public static Vector StartCardinalInterpolation(Vector y_knots, int degree, double h)
        {
            int N = y_knots.Length;
            int p = degree;
            Vector b = new Vector(2 * N + p - 2);
            for (int i = 0; i < N; i++)
            {
                b[i] = y_knots[i];
            }
            Vector I = Integrate(y_knots, degree, h);

            for (int i = N; i < b.Length; i++)
            {

                b[i] = I[i - N];

            }

            Matrix A = Create_LagrangeInterpolationMatrix(degree, h, N);
            // 
            double EPS = 0.0000001d;
            Vector coefs = Solver.BCGSTAB((Matrix.Transpose(A)) * A, (Matrix.Transpose(A)) * b, EPS);

       

            Vector c = new Vector(N + p - 2);
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = coefs[i];
            }
            return c;
        }

    }
}
