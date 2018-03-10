using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMathLib.MultyGrid
{
    class Proektor
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

    }
}
