using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMathLib.Spline
{
    public class SplineDifferentiation
    {

        public static Matrix Create_Ah(int degree, int Size)
        {
            Matrix Ah = new Matrix(Size);
            int p = degree;
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Ah[i, j] = 2.0 * CardinalSpline.Cardinal(2 * (p - 1), (i - j) + p - 1, 0, 1);
                    //if(i - j + 1 < Size)
                        Ah[i, j] -=  CardinalSpline.Cardinal(2 * (p - 1), (i - j) + p - 1 + 1, 0, 1);
                    //if (i - j - 1 >= 0)
                        Ah[i, j] -= CardinalSpline.Cardinal(2 * (p - 1), (i - j) + p - 1 - 1, 0, 1);
                } 
            }

            return Ah;
        }


        public static Matrix Create_D2_Ah(int degree, int Size)
        {
            Matrix Ah = new Matrix(Size);
            int p = degree;
            int q = p - 2;
            for (int k = 0; k < Size; k++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Ah[k, j] = 6d * CardinalSpline.Cardinal(2 * q, (k - j) + q, 0, 1);
                    //if (j + 1 < Size)
                        Ah[k, j] -= 4d*CardinalSpline.Cardinal(2 * q, (k - j) + q + 1, 0, 1);
                    //if (j - 1 >= 0)
                        Ah[k, j] -= 4d*CardinalSpline.Cardinal(2 * q, (k - j) + q - 1, 0, 1);
                   //if ( j + 2 < Size)
                        Ah[k, j] +=  CardinalSpline.Cardinal(2 * q, (k - j) + q + 2, 0, 1);
                   //if (j - 2 >= 0)
                        Ah[k, j] +=  CardinalSpline.Cardinal(2 * q, (k - j) + q - 2, 0, 1);

                }
            }

            return Ah;
        }

        public static Vector SolveDifEquation(Vector y_knots, int degree, double h)
        {
            int N = y_knots.Length;
            int p = degree;

            Vector b = InterpolateExperiment.CalculateSkal(y_knots, degree, h);

            b[0] = y0;
                        
            Matrix Ah = Create_Ah(degree, N + p - 2);
            double[] ksi = InterpolateExperiment.GetCardinalValue(degree);
            for (int i = 0; i < ksi.Length; i++)
            {

                Ah[0, i + 1] = 0;
                Ah[0, i] = ksi[i];
            }


            if (N < 23)
            {
                Console.WriteLine("matrix = " + Environment.NewLine + Ah);
                Console.WriteLine("skal(y,psi) = " + b);

            }
            // 
            double EPS = 0.0000001d;
            Vector c = Solver.BCGSTAB(Ah, b, EPS);
            Console.WriteLine("anser = " + c);
                     
            return c;
        }


        public static double GetIntegral(FunctionLib.Function f, int degree, double t_i, double h)
        {
            double a = t_i;
            double b = t_i + degree * h;
            double step = MyMath.Basic.GetStep(100, a, b);
            double S = 0;
            for (int i = 0; i < 100; i++)
            {
                S += f(t_i + i * step) * CardinalSpline.Cardinal(degree, t_i + i * step, t_i, h)*step;
            }
            return S;
        }


        public static double GetIntegral(FunctionLib.Function f, int degree, double t_i, double h, double a_bord, double b_bord)
        {
            double a = t_i;
            double b = t_i + degree * h;
            int N = 200;
            double step = MyMath.Basic.GetStep(N, a, b);
            double S = 0;
            for (int i = 0; i < N; i++)
            {
                if(t_i + i * step > a_bord && t_i + i * step < b_bord)
                S = S + f(t_i + i * step) * CardinalSpline.Cardinal(degree, t_i + i * step, t_i, h)*step;
            }
            return S;
        }

        public static Vector SolveDifD2Equation(int N,FunctionLib.Function f,double a_border,double b_border,
            double y0, double y1, int degree)
        {
            //int N = y_knots.Length;
            int p = degree;
            double h = MyMath.Basic.GetStep(N, a_border, b_border);
            Vector b = new Vector(N + p);

            for (int i = 0; i < N + p - 2; i++)
            {
                double t_start = a_border - (degree - 1) * h;
                b[i] = GetIntegral(f, p - 2, t_start + i*h, h) 
                    - 2d * GetIntegral(f, p - 2, t_start + (i+1)*h, h)
                    + GetIntegral(f, p - 2, t_start + (i+2)*h, h);
                b[i] = -2d*b[i];
            }
            b[N + p - 2] = y0;
            b[N + p - 1] = y1;

            Matrix Ah = Create_D2_Ah(degree, N + p - 2);
            Matrix D2 = new Matrix(N + p);
            for (int i = 0; i < N + p - 2; i++)
            {
                for (int j = 0; j < N+p-2; j++)
                {
                    D2[i, j] = (1 / (h * h)) * Ah[i, j];
                }
            }
            for (int i = 0; i < N+p-2; i++)
            {
                D2[N + p - 2, i] = CardinalSpline.Cardinal(degree, a_border, a_border + (i - p + 1)* h, h);
                D2[N + p - 1, i] = CardinalSpline.Cardinal(degree, b_border, a_border + (i - p + 1) * h, h);
                D2[i, N + p - 2] = D2[N + p - 2, i];
                D2[i, N + p - 1] = D2[ N + p - 1, i];
            }

            if (N < 20)
            {
                Console.WriteLine("matrix = " + Environment.NewLine + D2);
                Console.WriteLine("skal(y,psi) = " + b);

            }
            // 
            double EPS = 0.00001d;
            Vector c = Solver.BCGSTAB(D2,b, EPS);
            Console.WriteLine("D2*c = " + D2*c);

            return c;
        }



        public static void Solve_Test(ref Vector x,ref Vector expect,ref Vector actual,int Size, int degree)
        {

            double a = 0d;
            double b = Math.PI;
            int GridSize = Size;
            int deg = degree;
            Vector grid = Vector.CreateUniformGrid(GridSize, a, b);
            double h = MyMath.Basic.GetStep(GridSize, a, b);
            Vector y = MyMath.Basic.GetVectorFunction(GridSize, a, b, FunctionLib.constant_1);

            Vector c = MyMathLib.Spline.SplineDifferentiation.SolveDifEquation(y,0f, deg, h);
            //
            
            int N = GridSize;
             expect = MyMath.Basic.GetVectorFunction(N - 1, a, b, FunctionLib.line);
             actual = CardinalSpline.GetVectorFunctionSpline(N - 1, a, b, c, a, h, deg);
            x = Vector.CreateUniformGrid(N - 1, a, b);

            double result = (expect - actual).Norm;


            Console.WriteLine("exp - act = " + (expect - actual).ToString());
            Console.WriteLine("||c|| = " + c.Norm.ToString("0.000000"));
            Console.WriteLine("||f - spline|| = " + result.ToString("0.000000"));

        }

        public static void Solve_Test(ref Vector x, ref Vector expect, ref Vector actual,
                                        FunctionLib.Function dG, FunctionLib.Function G, 
                                        int Size, int degree,double a_border, double b_border)
        {

            double a = a_border;
            double b = b_border;
            int GridSize = Size;
            int deg = degree;
            Vector grid = Vector.CreateUniformGrid(GridSize, a, b);
            double h = MyMath.Basic.GetStep(GridSize, a, b);
            Vector y = MyMath.Basic.GetVectorFunction(GridSize, a, b, dG);

            Vector c = MyMathLib.Spline.SplineDifferentiation.SolveDifEquation(y,G(a_border), deg, h);
            //
            Console.WriteLine("step = " + h);
            int N = GridSize;
            expect = MyMath.Basic.GetVectorFunction(N - 1, a, b, G);
            actual = CardinalSpline.GetVectorFunctionSpline(N - 1, a, b, c, a, h, deg);
            x = Vector.CreateUniformGrid(N - 1, a, b);

            Vector err = expect - actual;
            double result = err.Norm;
            double max = err.InfNorm;

            Console.WriteLine("exp - act = " + (expect - actual).ToString());
            Console.WriteLine("||c|| = " + c.Norm.ToString("0.000"));
            Console.WriteLine("||f - spline|| = " + result.ToString("0.000"));
            Console.WriteLine("max|f - spline| = " + max.ToString("0.000"));

        }
    }
}
