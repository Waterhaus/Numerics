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

        public static Matrix CreateKSIMatrix(int degree,double h, int size)
        {
            int N = size;
            int p = degree;
            double[] ksi = GetCardinalValue(2 * degree, h);
            Console.WriteLine("deg = " + 2 * degree + " ksi = " + new Vector(ksi).ToString("0.000000"));
            ksi[degree - 1] = 2 * ksi[degree - 1];
            Matrix A = new Matrix(N + p - 2, N + p - 2);
           
            for (int i = 0; i < N + p - 2; i++)
            {
                for (int j = 0; j < ksi.Length; j++)
                {
                    if(i + j - p + 1 >= 0 && i + j - p + 1 < N + p - 2)
                    A[i, i + j - p + 1] = ksi[j];
                }
            }
            return A;

        }


        public static Matrix Create_LagrangeInterpolationMatrix(int degree, double h, int size)
        {
            double[] ksi_p = GetCardinalValue(degree, h);
            Matrix B = CreateInterpolationMatrix(ksi_p, size);
            Matrix KSI = CreateKSIMatrix(degree, h, size);

            int N = size;
            int p = degree;

            Matrix S = new Matrix(2 * N + p - 2);

            for (int i = 0; i < N ; i++)
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
                    S[N + i, j] = KSI[i, j];
                }
            }

            return S;

        }



        public static Vector MIN_Interpolate(Vector y_knots, int degree, double h)
        {
            int N = y_knots.Length;
            int p = degree;
            Vector b = new Vector(2 *N + p - 2);
            for (int i = 0; i < y_knots.Length; i++)
            {
                b[i] = y_knots[i];
            }
            Matrix A = Create_LagrangeInterpolationMatrix(degree, h, N);
           // Console.WriteLine(A);
            double EPS = 0.0000001d;
            Vector coefs = Solver.BCGSTAB(A, b, EPS);

            return coefs;
        }




        public static Vector Multiply_LagrangeInterpolationMatrix(Vector v,int degree, double h, int size)
        {
            double[] ksi_p = GetCardinalValue(degree, h);
            Matrix B = CreateInterpolationMatrix(ksi_p, size);
            Matrix KSI = CreateKSIMatrix(degree, h, size);
            


            int N = size;
            int p = degree;

            
            Vector b = new Vector(2 * N + p - 2);

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N + p - 2; j++)
                {
                    //S[i, j] = B[i, j];
                    b[i] += B[i, j] * v[j];
                    //S[N + j, N + p - 2 + i] = -B[i, j];
                    b[N + j] += v[N + p - 2 + i] * (-B[i, j]);
                }
            }

            for (int i = 0; i < N + p - 2; i++)
            {
                for (int j = 0; j < N + p - 2; j++)
                {
                    //S[N + i, j] = KSI[i, j];
                    b[N + i] += v[j] * KSI[i, j];
                }
            }

            return b;

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
