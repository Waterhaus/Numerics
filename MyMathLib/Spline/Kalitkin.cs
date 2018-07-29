using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMathLib.Spline
{
    public class Kalitkin
    {


     

        //протестирована
        public double bsplv(double x, Grid grid, int deg, int index)
        {
            double S = 0;
            int p = deg;
            int n = index;
            int N = grid.OriginalCount;
            if (n >= N - 1) Console.WriteLine("Недопустимый узел n");
            else
            {


                for (int k = 0; k <= p + 1; k++)
                {
                    double psi = (x - grid[n + k]);
                    if (psi <= 0) psi = 0;
                    else
                        S += b(grid,deg,n, k) * Math.Pow(psi, p);

                }
            }
            return S;
        }

        public double b(Grid grid, int deg, int index, int k)
        {
            if (k == 0) return 1;

            double N = 1;
            int n = index;
            int p = deg;
            for (int l = 1; l <= p + 1; l++)
            {
                if (l != k && (n + l + p) < grid.Count)
                    N = N * (grid[n + l] - grid[n]) / (grid[n + l] - grid[n + k]);
            }
            return -N;
        }

        public double KalitkinSpline(double x,, double[] y)
        {
            double SUM = 0;
            for (int n = -p; n < N - 1; n++)
                SUM += y[n + p] * bsplv(x,);

            return SUM;

        }

        public double S(double X, Vector c)
        {
            double SUM = 0;
            for (int n = -p; n < N - 1; n++)
                SUM += c[n + p] * B(n, X);

            return SUM;

        }

        public double A(int n, int l)
        {
            double a = 0;
            int N = x.Length;
            // N = N + 1;

            for (int m = max(l, n, 1); m <= min(N - 1, n + p + 1, l + p + 1); m++)
            {
                a += r(m) * b(m - n, n) * b(m - l, l);
            }

            return a;
        }

        //протестирована
        public int max(int a, int b, int c)
        {
            int z1 = max(a, b);
            int z2 = max(b, c);

            return max(z1, z2);
        }

        //протестирована
        public int min(int a, int b, int c)
        {
            int z1 = min(a, b);
            int z2 = min(b, c);

            return min(z1, z2);
        }

        public int max(int a, int b)
        {
            if (a > b) return a; else return b;
        }

        public int min(int a, int b)
        {
            if (a < b) return a; else return b;
        }

        private double r(int m)
        {
            double pm = 1;

            return pm / (x[m + 1] - x[m - 1]);
        }

        public double gamma(int m, Vector c)
        {
            double gm = 0;

            for (int n = max(0, m - 1) - p; n < m; n++)
            {
                gm += c[n + p] * b(m - n, n);
            }
            return gm;
        }

        public double Sp(double X, Vector c)
        {
            double S = 0;

            for (int m = -p; m <= N - 2; m++)
            {
                double psi = (X - x[m]);
                if (psi < 0) psi = 0;
                S += gamma(m, c) * Math.Pow(psi, p);
            }

            return S;
        }

    }
}
