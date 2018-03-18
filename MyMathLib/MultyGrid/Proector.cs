using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMathLib.MultyGrid
{
    public class Proektor
    {

        public static Vector basic_r(Vector v_h)
        {
            int N = v_h.Length;
            if (v_h.Length % 2 != 0)
            {
                //MessageBox.Show("Предупреждеие! Размерность вектора не делится на 2");
                N = (N - 1) / 2 + 1;
            }
            else
                N = v_h.Length / 2;

            Vector v_2h = new Vector(N);
            v_2h[0] = 0;
            int k = 0;
            for (int i = 1; i < v_2h.Length; i++)
            {
                k += 2;
                v_2h[i] = (1d / 4d) * v_h[k - 1] + (1d / 2d) * v_h[k];
                if (k + 1 < v_h.Length) v_2h[i] += (1d / 4d) * v_h[k + 1];
            }
            v_2h[0] = v_h[0];
            return v_2h;

        }

        public static Vector spline_r(Vector v_h)
        {
            int N = v_h.Length;

            double[] coef = new double[5];
            coef[0] = 0.125d;
            coef[1] = 0.375d;
            coef[2] = 0.375d;
            coef[3] = 0.125d;

            coef[0] = 0.0625d;
            coef[1] = 0.25d;
            coef[2] = 0.375d;
            coef[3] = 0.25d;
            coef[4] = 0.0625d;
            if (v_h.Length % 2 != 0)
            {
                //MessageBox.Show("Предупреждеие! Размерность вектора не делится на 2");
                N = (N + 1) / 2;
            }
            else
                N = v_h.Length / 2;

            Vector v_2h = new Vector(N);
            v_2h[0] = 0;
            int k = 0;

            for (int i = 1; i < v_2h.Length; i++)
            {
                k += 2;
                v_2h[i] = coef[0] * v_h[k - 2] + coef[1] * v_h[k - 1];
                if (k + 2 < v_h.Length)
                    v_2h[i] += coef[2] * v_h[k] + coef[3] * v_h[k + 1] + coef[4] * v_h[k + 2];
            }
            return v_2h;

        }

        

        public static Vector slow_spline_r(Vector v_h)
        {
            int n = MultyGrid.MultyGridMethod.DownDimentionSpline4(v_h.Length);
            
            Matrix R = 0.5d*create_proection_spline4_mat(n);
            Vector v_2h = R * v_h;

            return v_2h;
        }

        public static Matrix create_proection_spline4_mat(int n)
        {
            int p = 4;
            int N = 2 * n - (p - 1);
            
            Matrix A = new Matrix(n, N);
            double[] v1 = { 0.125, 0.5d, 0.75d, 0.5d, 0.125d };
            Vector v = new Vector(v1);
            
            double[,] minor = { { 1d, 0.5d, 0d, 0d, 0d, 0d }, { 0d, 0.5d, 0.75d, 0.1875d, 0d, 0d }, { 0d, 0d, 0.25d, 0.6875d, 0.5d, 0.125d } };
            Matrix M = new Matrix(minor);

            int k = 3;
            int index = 0;
            for (int i = 3; i < n - 3; i++)
            {
                for (int j = k; j < k + v.Length; j++)
                {
                    A[i, j] = v[index];
                    index++; 
                }
                index = 0;
                k = k + 2;
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    A[i, j] = minor[i, j];
                    A[n - 3 + i, N - 6 + j] = minor[2 - i, 5 - j];
                }
            }

            return A;
        }

    }
}
