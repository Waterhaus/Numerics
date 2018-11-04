using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMathLib
{
    public class Integrate
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


        public static double TrapezoidMethod(Vector f, double h)
        {
            double S = 0;

            for (int i = 1; i < f.Length - 1; i++)
            {
                S += f[i];
            }
            S += (f[0] + f.Last) / 2d;
            S *= h;

            return S;
        }


        public static double IntegrateSurface(Matrix F, double h_i, double h_j)
        {
            double S = 0;

            Vector I = new Vector(F.Length.n);
            int n = F.Length.m - 1;
            for (int i = 0; i < F.Length.n; i++)
            {
                for (int j = 1; j < F.Length.m - 1; j++)
                {
                    I[i] += F[i, j];
                }
                I[i] += (F[i,0] + F[i,n]) / 2d;
                I[i] *= h_j;
            }

            S = TrapezoidMethod(I, h_i);
            return S;
        }
            


    }
}
