using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMathLib
{
    enum BasisSplineType { DeBoor, Kalitkin}

    public class BasisSpline
    {
        Grid grid;
        BasisSplineType type;


        public class DeBoorMethods
        {
            //fast calculation
            public static double[] BSPLVB(double x, Grid grid, int deg, int index)
            {

                double[] b = new double[deg];
                double[] copy_b = new double[deg];
                double[] d_L = new double[deg];
                double[] d_R = new double[deg];
                b[0] = 1d;
                double A, B, temp;
                A = B = temp = 0;
                for (int k = 1; k <= deg - 1; k++)
                {
                    for (int i = 0; i <= k; i++)
                    {
                        d_L[i] = x - grid[index + i - k];
                        d_R[i] = grid[index + i + 1] - x;
                    }

                    for (int i = 0; i <= k; i++)
                    {
                        A = B = temp = 0;
                        if (i - 1 >= 0) A = d_L[i] / (d_R[i - 1] + d_L[i]);
                        if (i + 1 < d_L.Length) B = d_R[i] / (d_R[i] + d_L[i + 1]);

                        if (i - 1 >= 0) temp = A * b[i - 1];
                        temp += B * b[i];
                        copy_b[i] = temp;

                    }
                    for (int r = 0; r < b.Length; r++)
                        b[r] = copy_b[r];
                }
                return b;

            }

            //slow calculation
            public static double ClassicBasisSpline(double x, Grid grid, int deg, int index)
            {
                if ( index < 0) throw new ArgumentException("Выход за границы массива");
               

                if(deg == 0)
                if (x >= grid[index] && x < grid[index + 1])
                {
                    return 1;
                }
                else return 0;

                double C1, C2;
                C1 = C2 = 0;

                double temp = (grid[index + deg] - grid[index]);
                if (deg + index < grid.Count && temp > 0)
                    C1 = (x - grid[index]) / temp;

                temp = (grid[index + deg + 1] - grid[index + 1]);

                if (deg + index + 1 < grid.Count && temp > 0)
                    C2 = (grid[index + deg + 1] - x) /temp ;

                return C1 * ClassicBasisSpline(x, grid, deg - 1, index) + C2 * ClassicBasisSpline(x, grid, deg - 1, index + 1);
            }
        }
    }
}
