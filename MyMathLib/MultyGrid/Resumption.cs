using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMathLib.MultyGrid
{
    class Resumption
    {
        public static Vector basic_p(Vector v_2h)
        {
            Vector v_h = new Vector(2 * v_2h.Length - 1);
            int k = 0;
            for (int i = 0; i < v_h.Length; i++)
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
            return v_h;
        }

    }
}

