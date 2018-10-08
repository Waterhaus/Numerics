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


        public static Vector solveSlice(Vector u_old, Vector f, FunctionLib.Function p, double nu, double h, double tau, double aa, int iter_t)
        {
            int n = u_old.Length;
            Vector u_mid = new Vector(n);
            Vector u_up = new Vector(n);
            Vector u_down = new Vector(n);
            Vector F = new Vector(n);
            double left = -aa * tau / (h * h);
            double right = -aa * tau / (h * h);
            double middle = 1d + 2d * aa * tau / (h * h);

            for (int i = 1; i < n - 1; i++)
            {
                F[i] = tau * f[i] + u_old[i];
                u_mid[i] = middle;
                u_up[i] = left;
                u_down[i] = right;
            }
            u_down[0] = 0;
            u_mid[0] = -1d / h;
            u_up[0] = 1d / h;
            F[0] = 0;

            u_down[n - 1] = 1d / h;
            u_mid[n - 1] = (-1d / h) + nu;
            u_up[0] = 0;
            F[0] = nu*p(iter_t*tau);

            Vector u = Solver.TridiagonalMatrixAlgorithm(u_down, u_mid, u_up, F);
            return u;

        }


        public static Vector getSolution(FunctionLib.Function p, FunctionLib.Function2d f, FunctionLib.Function phi,
                                        double nu, double L, double TIME, 
                                        double aa, int N, int M)
        {
            double h = MyMath.Basic.GetStep(N, 0d, L);
            double tau = MyMath.Basic.GetStep(M, 0d, TIME);
            Vector u0 = new Vector(N);
            Vector fu = new Vector(N);

            for (int i = 0; i < N; i++)
            {
                u0[i] = phi(i * h);
                
            }

            for (int iter_t = 0; iter_t < M; iter_t++)
            {
                for (int i = 0; i < N; i++)
                {
                    fu[i] = f(i * h, iter_t * tau);
                }
                u0 = solveSlice(u0, fu, p, nu, h, tau, aa, iter_t);
            }

            return u0;

        }
    }
}
