using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMathLib
{
    class DifEquation : MyMath
    {
        //Однородная краевая задача для д.у. второго порядка  u(a) = u(b) = 0
        //Решение прогонкой
        public static Vector solveD2U(Vector f)
        {
            double[] c = new double[f.Length];
            int n = f.Length - 1;
            f[n] = 0;
            f[0] = 0;
            double C = 2d;
            double A = -1d;

            double m = -A / C;
            //m[0] = A / C;
            Vector x = new Vector(f.Length);
            c[0] = -A;
            for (int i = 1; i < n; i++)
            {
                // f[i] = f[i] + m * f[i - 1];
                c[i] = C - A * A / c[i - 1];
                if (i == 1) c[i] = C;
                if (i > 1) f[i] = f[i] - (A / c[i - 1]) * f[i - 1];

            }

            x[n] = f[n];

            for (int i = n - 1; i >= 1; i--)
            {
                if (Math.Abs(c[i]) > 0)
                    x[i] = (f[i] - A * x[i + 1]) / c[i];

            }
            x[0] = 0;

            return x;
        }

        public static Vector multiplyD2U(Vector v)
        {
            Vector b = new Vector(v.Length);

            for (int i = 1; i < b.Length - 1; i++)
            {
                b[i] = -v[i - 1] + 2d * v[i] - v[i + 1];
            }
            b[0] = v[0];
            b[b.Length - 1] = v[v.Length - 1];

            return b;

        }

        public static Vector multiplyD2U_Jacobi(Vector v, double h)
        {
            Vector b = new Vector(v.Length);

            for (int i = 1; i < b.Length - 1; i++)
            {
                b[i] = v[i] + (h * h / 4d) * (v[i - 1] - 2d * v[i] + v[i + 1]);
            }
            b[0] = v[0] - h * h * v[0];
            b[b.Length - 1] = v[v.Length - 1] - h * h * v[v.Length - 1];

            return b;

        }
    }
}
