using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMathLib
{
    enum BasisSplineType { ForMultyGrid, Basic }

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

        public BasisSpline()
        {
            degree = 0;

        }

        public BasisSpline(int deg, Vector x_knots, Vector y_knots, GridType gridType)
        {
            degree = deg;
            grid = new Grid(degree, x_knots, x_knots[0], x_knots.Last,true);
            bool canInterpolate = true;
            switch (gridType)
            {
                case GridType.SimpleSplineGrid:
                    grid.ToSimpleUniformSplineGrid();
                    canInterpolate = false;
                    break;
                case GridType.ClassicSplineGrid:
                    grid.ToClassicSplineGrid();
                    break;
                case GridType.UniformSplineGrid:
                    grid.ToUniformSplineGrid();
                    break;
                case GridType.ExperimentSplineGrid:
                    grid.ToExperimentSplineGrid();
                    break;
                case GridType.MiddleUniformSplineGrid:
                    grid.ToMiddleUniformSplineGrid();
                    break;
                case GridType.RightUniformSplineGrid:
                    grid.ToRightUniformSplineGrid();
                    break;
                case GridType.PeriodicSpline:
                    grid.ToPeriodiclineGrid();
                    C = InterpolatePeriodic(y_knots, grid, deg);
                    canInterpolate = false;
                    break;
                default:
                    break;
            }
            Console.WriteLine("grid = " +  grid.ToString() );
            if(canInterpolate) C = Interpolate(y_knots, grid, deg);
        }

        public BasisSpline(int deg, int GridSize, double a_border, double b_border, GridType gridType)
        {
            degree = deg;
            Vector grid_h = Vector.CreateUniformGrid(GridSize, a_border, b_border);
            C = new Vector(grid_h.Length);
            type = BasisSplineType.ForMultyGrid;

            bool canInterpolate = true;

            grid = new Grid(degree, grid_h.ToArray);
            switch (gridType)
            {
                case GridType.SimpleSplineGrid:
                    grid.ToSimpleUniformSplineGrid();
                    canInterpolate = false;
                    break;

                case GridType.ClassicSplineGrid:
                    grid.ToClassicSplineGrid();
                    break;
                case GridType.UniformSplineGrid:
                    grid.ToUniformSplineGrid();
                    break;
                case GridType.ExperimentSplineGrid:
                    grid.ToExperimentSplineGrid();
                    break;
                case GridType.MiddleUniformSplineGrid:
                    grid.ToMiddleUniformSplineGrid();
                    break;
                case GridType.RightUniformSplineGrid:
                    grid.ToRightUniformSplineGrid();
                    break;
                case GridType.LeftUniformSplineGrid:
                    grid.ToLeftUniformSplineGrid();
                    break;
                default:
                    break;
            }        

            Console.WriteLine("grid = " + grid.ToString());
        }


        public BasisSpline(int deg, int GridSize, double a_border, double b_border)
        {
            degree = deg;
            Vector grid_h = Vector.CreateUniformGrid(GridSize, a_border, b_border);
            C = new Vector(grid_h.Length);
            type = BasisSplineType.ForMultyGrid;
            grid = new Grid(degree, grid_h.ToArray);
            grid.ToClassicSplineGrid();
            Console.WriteLine("grid = " + grid.ToString());
        }

        public BasisSpline GetBasisSpline_Down()
        {
            int n = MultyGrid.MultyGridMethod.DownDimentionSpline4(grid.OriginalCount);
            return new BasisSpline(degree, n, grid.Left, grid.Right);
        }

        public BasisSpline GetBasisSpline_Up()
        {
            int N = MultyGrid.MultyGridMethod.UpDimentionSpline4(grid.OriginalCount);
            return new BasisSpline(degree, N, grid.Left, grid.Right);
        }

        public void SetNewCoefs(Vector c)
        {
            //if(grid.Dimetion == c.Length)
            Vector.copy(ref C, c);
            //else Console.WriteLine("Неверное количество коэффициентов для базисных сплайнов");
        }



        public Vector GetAllBasis(double x)
        {
            Vector B = new Vector(grid.OriginalCount);

            for (int i = 0; i < B.Length; i++)
            {
                B[i] = DeBoorMethods.DeBoorB(x, grid, degree, i);
            }
            return B;
        }

        public static Vector Interpolate(Vector y_knots, Grid grid, int deg)
        {
            if (deg == 2) return y_knots;
            Matrix A = DeBoorMethods.SlowCreateInterpolationMatrix(grid, deg);
            Console.WriteLine(A);

            Vector coefs = Solver.BCGSTAB(A, y_knots, EPS);
            
            return coefs;
        }

        public static Vector InterpolatePeriodic(Vector y_knots, Grid grid, int deg)
        {
            if (deg == 2) return y_knots;
            Matrix A = DeBoorMethods.SlowCreateInterpolationPeriodicMatrix(grid, deg);
            int size = deg - 1;
            Vector spline = new Vector(size);
            for (int i = 0; i < size; i++)
            {
                spline[i] = A[2, i];
            }
            int GridSize = y_knots.Length;
            A[0, GridSize - 2] = spline[0];
            A[0, GridSize - 1] = spline[1];

            A[1, GridSize - 1] = spline[0];

            A[GridSize - 2, 0] = spline[0];
            A[GridSize - 1, 0] = spline[1];

            A[GridSize - 1, 1] = spline[0];
            Console.WriteLine(A);

            Vector coefs = Solver.BCGSTAB(A, y_knots, EPS);

            return coefs;
        }



        public static Vector SimpleInterpolate(Vector y_knots, Grid grid, int deg)
        {
            if (deg == 2) return y_knots;
            int p = grid.SplineDegree;
            int N = grid.Count - 2 * (p - 1) - 2*(p - 2);
            Matrix A = new Matrix(N);
            Vector coefs = new Vector();

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    A[i, j] =  DeBoorMethods.SimpleDeBoorB(grid.GetOrigin(i), grid, deg, j + p - 1);
                }
            }

            if(A.Length.n < 15)Console.WriteLine(A);

            coefs = Solver.BCGSTAB(A, y_knots, EPS);

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

        public double GetBasis(double x, int index)
        {   
           return DeBoorMethods.DeBoorB(x, grid, degree, index);
           
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

            //подсчет базисных сплайнов
            public static Vector bsplv_spec(double x, Grid grid, int deg, int index)
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
                    if (index + j + 1 < grid.Count) d_R[j] = grid[index + j + 1] - x; // k right values
                    else d_R[j] = 0;

                    if (index - j >= 0) d_L[j] = x - grid[index - j];     // k left values
                    else d_L[j] = 0;

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
            //delete
            public static double DeBoorExperiment(double x, Grid grid, int deg, int index)
            {
                int J = grid.Find(x);
                int p = deg;
                
                Vector b = bsplv_spec(x, grid, deg, J);

                return b[deg - 1];
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
                int originLength = tau.GetOriginArray().Length;
                if (Math.Abs(x - tau.Right) < EPS && index <= originLength - 1)
                {
                    //if (index == originLength - 1) return 1;
                    return 0;
                }

                index = index + tau.BeginIndex;

                

                if (index < J || index > J + p) return 0; //если не в suppBj от 0

                if(tau.gridType != GridType.MiddleUniformSplineGrid && tau.gridType != GridType.RightUniformSplineGrid && tau.gridType != GridType.LeftUniformSplineGrid && tau.gridType != GridType.PeriodicSpline)
                {

                    if (index == tau.BeginIndex && Math.Abs(x - tau.Left) < EPS) return 1d;
                    if (index == tau.Count - 1 && Math.Abs(x - tau.Right) < EPS) return 1d;


                }
                Vector b = bsplv_spec(x, tau, deg, J);

                return b[index - J];

            }

            //основан на bsplvb
            public static double SimpleDeBoorB(double x, Grid tau, int deg, int index)
            {
                if (x < tau.Left || x > tau.Right) return 0;  //если не лежит в отрезке то 0
                int J = tau.Find(x);
                int p = deg - 1;
                int originLength = tau.GetOriginArray().Length;
                
                index = index + tau.BeginIndex;



                if (index < J || index > J + p) return 0; //если не в suppBj от 0

                
                Vector b = bsplv_spec(x, tau, deg, J);

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
                Matrix A = new Matrix(tau.OriginalCount);

                for (int i = 0; i < tau.OriginalCount; i++)
                {
                    for (int j = 0; j < tau.OriginalCount; j++)
                    {
                        if (i == tau.OriginalCount - 1 && j == tau.OriginalCount - 1)
                        {
                            int a = 0;
                        }
                        A[i, j] = DeBoorB(tau.GetOrigin(i), tau, deg, j);
                    }
                }
                return A;
            }

            public static Matrix SlowCreateInterpolationPeriodicMatrix(Grid tau, int deg)
            {
                Matrix A = new Matrix(tau.OriginalCount);

                for (int i = 0; i < tau.OriginalCount; i++)
                {
                    for (int j = 0; j < tau.OriginalCount; j++)
                    {
                        if (i == tau.OriginalCount - 1 && j == 10 )
                        {
                            int a = 0;
                        }
                        A[i, j] = DeBoorB(tau.GetOrigin(i), tau, deg, j + deg);
                    }
                }
                return A;
            }
        }

        public class Kalitkin { }
    }
}
