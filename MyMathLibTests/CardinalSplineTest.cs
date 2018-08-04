using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyMathLib
{
    [TestClass]
    public class CardinalSplineTest
    {
        [TestMethod]
        public void InterpolationMatrixTest()
        {
            //setup
            double EPS = 0.1d;
            double a = 0d;
            double b = 1d;
            int GridSize = 18;
            int deg = 6;
            Vector grid = Vector.CreateUniformGrid(GridSize, a, b);
            double h = MyMath.Basic.GetStep(GridSize, a, b);
            Vector y = MyMath.Basic.GetVectorFunction(GridSize, a, b, FunctionLib.one_minus_t_sin);
            Grid tau = new Grid(deg, grid, grid[0], grid.Last, true);
            tau.ToPeriodiclineGrid();
            //run
            Matrix A = CardinalSpline.CreateInterpolationMatrix(tau, h, deg);

            Console.WriteLine(A);
            //compare
            //int N = 150;
            //Vector expect = MyMath.Basic.GetVectorFunction(N, a, b, FunctionLib.one_minus_t_sin);
            //Vector actual = bspline.GetVectorFunction(N, a, b);

            //Vector bf = bspline.GetVectorFunction(grid);


            //double result = (expect - actual).Norm;
            //Console.WriteLine("Степень сплайна = " + deg);
            //Console.WriteLine("Сетка по которой построен сплайн = " + bspline.grid.ToString());
            //Console.WriteLine("значения х = " + grid.ToString());
            //Console.WriteLine("значение f(x) = " + y.ToString());
            //Console.WriteLine("значение bf(x) = " + bf.ToString());
            //Console.WriteLine("coef = " + bspline.COEFICIENT.ToString());
            //Console.WriteLine("||f - spline|| = " + result.ToString("0.0000"));
            //Assert.AreEqual(0, result, EPS, "Плохая интерполяция!");
        }


        [TestMethod]
        public void InterpolationTest()
        {
            //setup
            double EPS = 0.1d;
            double a = 0d;
            double b = 1d;
            int GridSize = 18;
            int deg = 6;
            Vector grid = Vector.CreateUniformGrid(GridSize, a, b);
            double h = MyMath.Basic.GetStep(GridSize, a, b);
            Vector y = MyMath.Basic.GetVectorFunction(GridSize, a, b, FunctionLib.one_minus_t_sin);
            Grid tau = new Grid(deg, grid, grid[0], grid.Last, true);
            tau.ToPeriodiclineGrid();
            //run
            Vector c = CardinalSpline.Interpolate(y, tau, deg, h);

            Console.WriteLine(c);
            //compare
            //int N = 150;
            //Vector expect = MyMath.Basic.GetVectorFunction(N, a, b, FunctionLib.one_minus_t_sin);
            //Vector actual = bspline.GetVectorFunction(N, a, b);

            //Vector bf = bspline.GetVectorFunction(grid);


            //double result = (expect - actual).Norm;
            //Console.WriteLine("Степень сплайна = " + deg);
            //Console.WriteLine("Сетка по которой построен сплайн = " + bspline.grid.ToString());
            //Console.WriteLine("значения х = " + grid.ToString());
            //Console.WriteLine("значение f(x) = " + y.ToString());
            //Console.WriteLine("значение bf(x) = " + bf.ToString());
            //Console.WriteLine("coef = " + bspline.COEFICIENT.ToString());
            //Console.WriteLine("||f - spline|| = " + result.ToString("0.0000"));
            //Assert.AreEqual(0, result, EPS, "Плохая интерполяция!");
        }
    }
}
