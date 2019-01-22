using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMathLib.MultyGrid
{
    public class Resumption
    {
        public static Vector basic_p(Vector v_2h)
        {
            Vector v_h = new Vector(2 * v_2h.Length);
            int k = 0;
            for (int i = 0; i < v_h.Length - 1; i++)
            {
                if (i % 2 == 0)
                {
                    v_h[i] = v_2h[i / 2];
                }
                else
                {
                    k = (i - 1) / 2;
                    v_h[i] = (v_2h[k] + v_2h[k + 1]) / 2d;
                }

            }
            v_h.Last = v_2h.Last;
            return v_h;
        }

        public static Vector spline4_p(Vector v_2h)
        {
            Vector v_h = new Vector(2 * v_2h.Length);



            return v_h;

        }

        public static Vector slow_spline4_p(Vector v_2h)
        {
            int n = v_2h.Length;
            int N = MultyGrid.MultyGridMethod.UpDimentionSpline4(n);

            Matrix R = Proektor.create_proection_spline4_mat(n);
            Matrix P = Matrix.Transpose(R);

            return P * v_2h;
        }

    }
}

