using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMathLib.Spline
{
    public class CardinalDifferentialEquation
    {
        private static double[] GetCardinalValue(int degree)
        {
            double[] mas = new double[degree - 1];
            for (int i = 0; i < degree - 1; i++)
            {
                mas[i] = CardinalSpline.Cardinal(degree, (i + 1), 0d, 1d);

            }
            return mas;
        }

        public static Matrix CreateKSIMatrix(int degree, double h, int size)
        {
            int N = size;
            int p = degree;
            double[] ksi = GetCardinalValue(2 * degree);
            Console.WriteLine("deg = " + 2 * degree + " ksi = " + new Vector(ksi).ToString("0.000000"));
            
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

        public static Matrix FirstDirevetive(int N)
        {
            Matrix D = new Matrix(N + 1, N);

            for (int i = 0; i < N ; i++)
            {
                D[i, i] = -1;
                D[i + 1, i] = 1;
            }

            return D;
        }


        public static Matrix SecondDirevetive(int N)
        {
            Matrix D = new Matrix(N + 2, N);

            for (int i = 0; i < N; i++)
            {
                D[i, i] = -1;
                D[i + 1, i] = 2;
                D[i + 2, i] = -1;
            }

            return D;
        }

    }
}
