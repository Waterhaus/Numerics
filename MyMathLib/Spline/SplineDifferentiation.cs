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
                    if(i - j + 1 < Size)
                        Ah[i, j] -=  CardinalSpline.Cardinal(2 * (p - 1), (i - j) + p + 1 - 1, 0, 1);
                    if (i - j - 1 >= 0)
                        Ah[i, j] -= CardinalSpline.Cardinal(2 * (p - 1), (i - j) + p - 1 - 1, 0, 1);
                } 
            }

            return Ah;
        }

        public static Vector SolveDifEquation(Vector y_knots, int degree, double h)
        {
            int N = y_knots.Length;
            int p = degree;

            Vector b = InterpolateExperiment.CalculateSkalExtendet(y_knots, degree, h);



            Matrix Ah = Create_Ah(degree, N + p - 2);

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

        public static void Solve_Test(ref Vector x,ref Vector expect,ref Vector actual,int Size, int degree)
        {

            double a = 0d;
            double b = Math.PI;
            int GridSize = Size;
            int deg = degree;
            Vector grid = Vector.CreateUniformGrid(GridSize, a, b);
            double h = MyMath.Basic.GetStep(GridSize, a, b);
            Vector y = MyMath.Basic.GetVectorFunction(GridSize, a, b, FunctionLib.line);

            Vector c = MyMathLib.Spline.SplineDifferentiation.SolveDifEquation(y, deg, h);
            //
            
            int N = GridSize;
             expect = MyMath.Basic.GetVectorFunction(N - 1, a, b, FunctionLib.constant_1);
             actual = CardinalSpline.GetVectorFunctionSpline(N - 1, a, b, c, a, h, deg);
            x = Vector.CreateUniformGrid(N - 1, a, b);

            double result = (expect - actual).Norm;


            Console.WriteLine("exp - act = " + (expect - actual).ToString());
            Console.WriteLine("||c|| = " + c.Norm.ToString("0.000000"));
            Console.WriteLine("||f - spline|| = " + result.ToString("0.000000"));

        }

        public static void Solve_Test(ref Vector x, ref Vector expect, ref Vector actual,
                                        FunctionLib.Function G, FunctionLib.Function dG, 
                                        int Size, int degree,double a_border, double b_border)
        {

            double a = a_border;
            double b = b_border;
            int GridSize = Size;
            int deg = degree;
            Vector grid = Vector.CreateUniformGrid(GridSize, a, b);
            double h = MyMath.Basic.GetStep(GridSize, a, b);
            Vector y = MyMath.Basic.GetVectorFunction(GridSize, a, b, G);

            Vector c = MyMathLib.Spline.SplineDifferentiation.SolveDifEquation(y, deg, h);
            //
            Console.WriteLine("step = " + h);
            int N = GridSize;
            expect = MyMath.Basic.GetVectorFunction(N - 1, a, b, dG);
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
