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
            Matrix Ah = MyMathLib.Spline.SplineDifferentiation.Create_Ah(3, 10);

            Console.WriteLine(Ah);
        }

        [TestMethod]
        public void Solve_Test()
        {
           
            double a = 0d;
            double b = Math.PI;
            int GridSize = 40;
            int deg = 4;
            Vector grid = Vector.CreateUniformGrid(GridSize, a, b);
            double h = MyMath.Basic.GetStep(GridSize, a, b);
            Vector y = MyMath.Basic.GetVectorFunction(GridSize, a, b, FunctionLib.line);

            Vector c = MyMathLib.Spline.SplineDifferentiation.SolveDifEquation(y,deg,h);
            //

            int N =  GridSize;
            Vector expect = MyMath.Basic.GetVectorFunction(N - 1, a, b, FunctionLib.constant_1);
            Vector actual = CardinalSpline.GetVectorFunctionSpline(N - 1, a, b, c, a, h, deg);


            double result = (expect - actual).Norm;

       
            Console.WriteLine("exp - act = " + (expect - actual).ToString());
            Console.WriteLine("||c|| = " + c.Norm.ToString("0.000000"));
            Console.WriteLine("||f - spline|| = " + result.ToString("0.000000"));

        }



    }
}
