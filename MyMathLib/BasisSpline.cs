using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMathLib
{
    enum BasisSplineType { DeBoor, Kalitkin }

    public class BasisSpline
    {
        public Grid grid;
        private Vector C;
        private int degree;
        BasisSplineType type;
        public const double EPS = 0.00000001d;

        public Vector COEFICIENT
        {
            get { return C; }
        }

        public BasisSpline(int deg, Vector x_knots, Vector y_knots, GridType gridType)
        {
            degree = deg;
            grid = new Grid(degree, x_knots, x_knots[0], x_knots.Last);
            switch (gridType)
            {
                case GridType.ClassicSplineGrid:
                    grid.ToClassicSplineGrid();
                    break;
                case GridType.UniformSplineGrid:
                    grid.ToUniformSplineGrid();
                    break;
                case GridType.ExperimentSplineGrid:
                    grid.ToExperimentSplineGrid();
                    break;
                default:
                    break;
            }
            Console.WriteLine("grid = " +  grid.ToString() );
            C = Interpolate(y_knots, grid, deg);
        }

        

        public Vector GetAllBasis(double x)
        {
            Vector B = new Vector(grid.Dimetion);

            for (int i = 0; i < B.Length; i++)
            {
                B[i] = DeBoorMethods.DeBoorB(x, grid, degree, i);
            }
            return B;
        }

        private static Vector Interpolate(Vector y_knots, Grid grid, int deg)
        {
            if (deg == 2) return y_knots;
            Matrix A = DeBoorMethods.SlowCreateInterpolationMatrix(grid, deg);
            Vector coefs = Solver.BCGSTAB(A, y_knots, EPS);
            
            return coefs;
        }

        public Matrix GetMatrix()
        {
            return DeBoorMethods.SlowCreateInterpolationMatrix(grid, degree);
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

        public double FastCalculateSpline(double x)
        {
            double S = 0d;
            double B = 0;
            if (x < grid.Left || x > grid.Right) return 0;  //если не лежит в отрезке то 0
            int J = grid.Find(x);
            int p = degree - 1;
            int index = J - grid.BeginIndex;

            if (Math.Abs(x - grid.Left) < EPS) return C[0];
            if (Math.Abs(x - grid.Right) < EPS) return C.Last;

            Vector b = DeBoorMethods.bsplv(x, grid, degree, J);

            for (int i = 0; i < degree && index + i < C.Length; i++)
            {
                S += C[index + i] * b[i];
            }


            return S;
        }

        public Vector GetVectorFunction(int GridSize, double a_border, double b_border)
        {
            Vector f = new Vector(GridSize);
            double h = MyMath.Basic.GetStep(GridSize, a_border, b_border);

            for (int i = 0; i < GridSize; i++)
            {
                f[i] = FastCalculateSpline(a_border + i * h);
            }
            return f;
        }

        public Vector GetVectorFunction(Grid tau)
        {
            Vector f = new Vector(tau.Count);


            for (int i = 0; i < f.Length; i++)
            {
                f[i] = FastCalculateSpline(tau[i]);
            }
            return f;
        }

        public Vector GetVectorFunction(Vector x_knots)
        {
            Vector f = new Vector(x_knots.Length);


            for (int i = 0; i < f.Length; i++)
            {
                f[i] = FastCalculateSpline(x_knots[i]);
            }
            return f;
        }

        public Vector GetVectorBasis(Vector x_knots,int index)
        {
            Vector f = new Vector(x_knots.Length);


            for (int i = 0; i < f.Length; i++)
            {
                f[i] = DeBoorMethods.DeBoorB(x_knots[i], grid, degree, index);
            }
            return f;
        }

        public class DeBoorMethods
        {

            //подсчет базисных сплайнов
            public static Vector bsplv(double x, Grid grid, int deg, int index)
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


                if (deg == 0)
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
                return ClassicBasisSpline(x, tau, tau.SplineDegree - 1, index);  
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
                if (index == tau.Count - 1 && Math.Abs(x - tau.Right) < EPS) return 1d;

                Vector b = bsplv(x, tau, deg, J);

                return b[index - J];

            }

            //все ненулевые значения сплайнов в точке x
            public static Vector DeBoorAllB(double x, Grid tau, int deg, int index)
            {
                Vector v = new Vector(deg);
                if (x < tau.Left || x > tau.Right) return v;  //если не лежит в отрезке то 0
                int J = tau.Find(x);
                int p = deg - 1;
                index = index + tau.BeginIndex;


                if (index < J || index > J + p) return v; //если не в suppBj от 0

                if (index == tau.BeginIndex && Math.Abs(x - tau.Left) < EPS)
                {
                    v[0] = 1d;
                    return v;
                }
                if (index == tau.Count - 1 && Math.Abs(x - tau.Right) < EPS)
                {
                    v[0] = 1d;
                    return v;
                }

                Vector b = bsplv(x, tau, deg, J);

                return b;

            }

            public static Vector GetVectorDeBoorB(int GridSize, double a_border, double b_border, Grid tau, int deg, int index)
            {
                Vector f = new Vector(GridSize);
                double h = MyMath.Basic.GetStep(GridSize, a_border, b_border);

                for (int i = 0; i < GridSize; i++)
                {
                    f[i] = DeBoorB(a_border + i * h, tau, deg, index);
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
