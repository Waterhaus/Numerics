﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMathLib
{
    enum BasisSplineType { DeBoor, Kalitkin}

    public class BasisSpline
    {
        public Grid grid;
        private Vector C;
        private int degree;
        BasisSplineType type;
        public const double EPS = 0.00000001d;

        public BasisSpline(int deg,Grid x_knots,Vector y_knots)
        {
            degree = deg;
            grid = new Grid(degree, x_knots, x_knots.Left, x_knots.Right);
            C = Interpolate(y_knots, grid, deg);
        }


        private static Vector Interpolate(Vector y_knots,Grid grid, int deg)
        {
            Matrix A = DeBoorMethods.SlowCreateInterpolationMatrix(grid, deg);
            Vector coefs = Solver.BCGSTAB(A, y_knots, EPS);

            return coefs;
        }

        public double SlowCalculateSpline(double x)
        {
            double S = 0d;
            double B = 0;

            for (int i = 0; i < C.Length; i++)
            {
                B = DeBoorMethods.DeBoorB(x, grid, degree, i);
                S += C[i] * B;
            }
            return S;
        }

        public class DeBoorMethods
        {
            //fast calculation
            public static double[] BSPLVB(double x, Grid grid, int deg)
            {
                int index = grid.Find(x);

                double[] b = new double[deg];
                double[] copy_b = new double[deg];
                double[] d_L = new double[deg];
                double[] d_R = new double[deg];
                b[0] = 1d;
                double A, B, temp;
                A = B = temp = 0;
                for (int degree = 1; degree <= deg - 1; degree++)
                {
                    for (int i = 0; i <= degree; i++)
                    {
                        d_L[i] = x - grid[index + i - degree];
                        d_R[i] = grid[index + i + 1] - x;
                    }

                    for (int i = 0; i <= degree; i++)
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


            public static double getBSplineValue(int index, int find, Vector b)
            {
                int deg = b.Length - 2;
                if (Math.Abs(find - index) > deg) return 0;

                return b[find - index]; 
            }

         
            public static Vector  basis_spline(double x, Grid grid, int deg, int index)
            {
                int n_knots = grid.Count;
                Vector b = new Vector(deg);
                b[0] = 1;
                double[] d_R = new double[deg];
                double[] d_L = new double[deg];
                double term;
                double saved;
                for (int j = 0; j < deg - 1; j++)
                {
                    d_R[j] = grid[index + j + 1] - x; // k right values
                    d_L[j] = x - grid[index - j];     // k left values
                    saved = 0;
                    for (int r = 0; r <= j; r++)
                    {
                        term = b[r] / (d_R[r] + d_L[j - r]);
                        b[r] = saved + d_R[r] * term;
                        saved = d_L[j - r] * term;
                    }
                    b[j + 1] = saved;
                }
                return b;
            }


            //slow calculation
            public static double ClassicBasisSpline(double x, Grid grid, int deg, int index)
            {
                if (index < 0) throw new ArgumentException("Выход за границы массива");
               

                if(deg == 0)
                if (x >= grid[index] && x < grid[index + 1])
                {
                    return 1;
                }
                else return 0;

                double C1, C2;
                C1 = C2 = 0;
                double temp = 0;

                if (index + deg < grid.Count)
                {
                    temp = (grid[index + deg] - grid[index]);
                    if (temp > 0)
                        C1 = (x - grid[index]) / temp;
                }

                if (index + deg + 1 < grid.Count)
                {
                    temp = (grid[index + deg + 1] - grid[index + 1]);

                    if (temp > 0)
                        C2 = (grid[index + deg + 1] - x) / temp;
                }
              

                

                return C1 * ClassicBasisSpline(x, grid, deg - 1, index) + C2 * ClassicBasisSpline(x, grid, deg - 1, index + 1);
            }

            //основан на рекурсивном методе
            public static double StandartB(double x, Grid tau, int index)
            {
                switch (tau.Type)
                {
                    case GridType.ClassicQubic:
                        return ClassicBasisSpline(x, tau, 3, index);

                    case GridType.ClassicQuadratic:
                        return ClassicBasisSpline(x, tau, 2, index);

                    case GridType.ClassicLinear:
                        return ClassicBasisSpline(x, tau, 1, index);

                }
                return 0;
            }

            //основан на bsplvb
            public static double DeBoorB(double x, Grid tau, int deg, int index)
            {
                if (x < tau.Left || x > tau.Right) return 0;  //если не лежит в отрезке то 0
                int J = tau.Find(x);
                int p = deg - 1;
                index = index + tau.BeginIndex;


                if (index < J || index > J + p) return 0; //если не в suppBj от 0
               
                if (index == tau.BeginIndex && Math.Abs(x - tau.Left) < EPS) return 1d;
                if (index == tau.EndIndex &&  Math.Abs(x - tau.Right) < EPS) return 1d;

                Vector b = basis_spline(x, tau, deg, J);

                return b[index - J];
                
            }

            public static Vector GetVectorDeBoorB(int GridSize, double a_border, double b_border, Grid tau, int deg, int index)
            {
                Vector f = new Vector(GridSize);
                double h = MyMath.Basic.GetStep(GridSize, a_border, b_border);

                for (int i = 0; i < GridSize; i++)
                {
                    f[i] = DeBoorB(a_border + i * h,tau,deg,index);
                }
                return f;
            }

            public static Matrix SlowCreateInterpolationMatrix(Grid tau, int deg)
            {
                Matrix A = new Matrix(tau.Dimetion);

                for (int i = 0; i < tau.Dimetion; i++)
                {
                    for (int j = 0; j < tau.Dimetion; j++)
                    {
                        A[i, j] = DeBoorB(tau.GetOrigin(i), tau, deg, j);
                    }
                }
                return A;
            }
        }
    }
}
