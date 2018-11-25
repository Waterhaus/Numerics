using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMathLib;
namespace MyMathLibTest
{
    [TestClass]
    public class SplineDifferentiationTest
    {
        [TestMethod]
        public void SplineDifferentiation_Test()
        {
            Matrix Ah = MyMathLib.Spline.SplineDifferentiation.Create_Ah(4, 10);

            Console.WriteLine(Ah);
        }

        [TestMethod]
        public void Solve_Test()
        {
           
            double a = 0d;
            double b = Math.PI;
            int GridSize = 20;
            int deg = 4;
            Vector grid = Vector.CreateUniformGrid(GridSize, a, b);
            double h = MyMath.Basic.GetStep(GridSize, a, b);
            Vector y = MyMath.Basic.GetVectorFunction(GridSize, a, b, FunctionLib.constant_1);

            Vector c = MyMathLib.Spline.SplineDifferentiation.SolveDifEquation(y,1d,deg,h);
            //

            int N =  GridSize;
            Vector expect = MyMath.Basic.GetVectorFunction(N - 1, a, b, FunctionLib.line);
            Vector actual = CardinalSpline.GetVectorFunctionSpline(N - 1, a, b, c, a, h, deg);


            double result = (expect - actual).Norm;

       
            Console.WriteLine("exp - act = " + (expect - actual).ToString());
            Console.WriteLine("||c|| = " + c.Norm.ToString("0.000000"));
            Console.WriteLine("||f - spline|| = " + result.ToString("0.000000"));

        }


        [TestMethod]
        public void SplineDifferentiation_D2_Test()
        {
            Matrix Ah = MyMathLib.Spline.SplineDifferentiation.Create_D2_Ah(3, 10);

            Console.WriteLine(Ah);
        }

        [TestMethod]
        public void SplineDifferentiation_D2_Test2()
        {

            double a_border = 0;
            double b_border = 1;
            int N = 20;
            double y0 = 1, y1 = Math.Exp(1);
            double h = MyMath.Basic.GetStep(N, a_border, b_border);
            int p = 4;
            Vector c_lambda = MyMathLib.Spline.SplineDifferentiation.SolveDifD2Equation(N, FunctionLib.exp, a_border, b_border, y0, y1,p);
            if (c_lambda.Length > N)
            {
                Console.WriteLine("c_lambda = " + c_lambda);
                Vector c = c_lambda.GetPartVector(0, N + p - 3);
                Console.WriteLine("anser = " + c);
                Vector actual = CardinalSpline.GetVectorFunctionSplineNW(N, a_border, b_border, c, h, p);
                Console.WriteLine(actual);
                Console.WriteLine(MyMath.Basic.GetVectorFunction(N, a_border, b_border, FunctionLib.exp));
            }
        }

        [TestMethod]
        public void Test()
        {
            double a_border = 0;
            double b_border = 1;
            int N = 10;
            double y0 = 0, y1 = 1;
            double h = MyMath.Basic.GetStep(N, a_border, b_border);
            int p = 4;
            
            Vector c = MyMathLib.Spline.InterpolateExperiment.Cardinal_Interpolation_Complete_Test(FunctionLib.exp, p, N, a_border,b_border);
            Console.WriteLine(MyMath.Basic.GetVectorFunction(N - 1, a_border, b_border, FunctionLib.exp));
            
            Matrix Ah = MyMathLib.Spline.SplineDifferentiation.Create_D2_Ah(p, N + p - 2);
            Console.WriteLine(Ah);
            Console.WriteLine("c = " + c);
            Console.WriteLine("Ac = " + Ah*c);

        }

        [TestMethod]
        public void Solve_D2_Test()
        {

            double a = 0d;
            double b = 1d;
            int GridSize = 40;
            int deg = 4;
            Vector grid = Vector.CreateUniformGrid(GridSize, a, b);
            double h = MyMath.Basic.GetStep(GridSize, a, b);
            Vector y = MyMath.Basic.GetVectorFunction(GridSize, a, b, FunctionLib.exp);

            Vector c = MyMathLib.Spline.SplineDifferentiation.SolveDifEquation(y, deg, h);
            //

            int N = GridSize;
            Vector expect = MyMath.Basic.GetVectorFunction(N - 1, a, b, FunctionLib.exp);
            Vector actual = CardinalSpline.GetVectorFunctionSpline(N - 1, a, b, c, a, h, deg);


            double result = (expect - actual).Norm;


            Console.WriteLine("exp - act = " + (expect - actual).ToString());
            Console.WriteLine("||c|| = " + c.Norm.ToString("0.000000"));
            Console.WriteLine("||f - spline|| = " + result.ToString("0.000000"));

        }

    }
}
