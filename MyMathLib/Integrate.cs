using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMathLib
{
    class Integrate
    {
        

            public static double RiemannSum(double[] y, double h)
            {
                double S = MyMath.Basic.SumArray(y);

                return S * h;
            }

            public static double Simpson(double[] y, double h)
            {
                int N = y.Length - 1;
                int n = 0;
                if (N % 2 == 0) n = N / 2;
                else
                {
                n = (N - 1) / 2;
                }

                double I = 0;
                double S1, S2;
                S1 = S2 = 0;

                for (int i = 1; i < n; i++)
                {
                    S1 += y[2 * i];
                }
                for (int i = 1; i <= n; i++)
                {
                    S2 += y[2 * i - 1];
                }

                I = (h / 3d) * (2d * S1 + 4d * S2 + y[0] + y[N]);

                return I;
            }

            


    }
}
