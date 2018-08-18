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
           
        }


        [TestMethod]
        public void InterpolationTest()
        {
            //setup
            double EPS = 0.1d;
            double a = 0d;
            double b = Math.PI;
            int GridSize = 15;
            int deg = 4;
            Vector grid = Vector.CreateUniformGrid(GridSize, a, b);
            double h = MyMath.Basic.GetStep(GridSize, a, b);
            Vector y = MyMath.Basic.GetVectorFunction(GridSize, a, b, FunctionLib.sin);
            Grid tau = new Grid(deg, grid, grid[0], grid.Last, true);
            tau.ToPeriodiclineGrid();
            //run
            Vector c = CardinalSpline.Interpolate(y, tau, deg, h);
            Matrix A = CardinalSpline.CreateInterpolationMatrix(tau, h, deg);
            //Console.WriteLine(A);
            Console.WriteLine("c = " + c);
            Console.WriteLine("Ac = " + A*c);
            Console.WriteLine("Степень сплайна = " + deg);
            //compare
            int N = 2*GridSize;
            Vector expect = MyMath.Basic.GetVectorFunction(N, a, b, FunctionLib.sin);
            Vector actual = CardinalSpline.GetVectorFunction(N, a, b,c,a,h,deg);

            Vector bf = CardinalSpline.GetVectorFunction(GridSize, a, b, c, a, h, deg);


            double result = (expect - actual).Norm;
            double interpolation = (y - bf).Norm;
            //Console.WriteLine("Сетка по которой построен сплайн = " + bspline.grid.ToString());
            //Console.WriteLine("значения х = " + grid.ToString());
            Console.WriteLine("значение f(x) = " + y.ToString());
            Console.WriteLine("значение bf(x) = " + bf.ToString());
            Console.WriteLine("exp - act = " + (expect - actual).ToString());
            //Console.WriteLine("coef = " + bspline.COEFICIENT.ToString());
            Console.WriteLine("||f - spline|| = " + result.ToString("0.0000"));
            Assert.AreEqual(0, interpolation, EPS, "Плохая интерполяция!");
        }

        [TestMethod]
        public void ReflectionInterpolationTest()
        {
            //setup
            double EPS = 0.1d;
            double a = 0d;
            double b = 1d;
            int GridSize = 20;
            int deg = 4;
            Vector grid = Vector.CreateUniformGrid(2*GridSize, a, b + b - a);
            double h = MyMath.Basic.GetStep(GridSize, a, b);
            Vector y = MyMath.Basic.GetVectorFunction(GridSize, a, b, FunctionLib.test_func1);
            Grid tau = new Grid(deg, grid, grid[0], grid.Last, true);
            tau.ToPeriodiclineGrid();
            //run
            Vector refl_y = MyMath.Operations.Reflect(y);
            Console.WriteLine("y = " + y);
            Console.WriteLine("ref_y = " + refl_y);
            Vector temp_c = CardinalSpline.Interpolate(refl_y, tau, deg, h);
            Vector c = MyMath.Operations.Deflect(temp_c);

            Console.WriteLine("c = " + c);
     
            ////compare
            int N = 150;
            Vector expect = MyMath.Basic.GetVectorFunction(N, a, b, FunctionLib.tttsin);
            Vector actual = CardinalSpline.GetVectorFunction(N, a, b, c, a, h, deg);

            Vector bf = CardinalSpline.GetVectorFunction(GridSize, a, b, c, a, h, deg);


            //double result = (expect - actual).Norm;
            double interpolation = (y - bf).Norm;
            ////Console.WriteLine("Сетка по которой построен сплайн = " + bspline.grid.ToString());
            ////Console.WriteLine("значения х = " + grid.ToString());
            //Console.WriteLine("значение f(x) = " + y.ToString());
            //Console.WriteLine("значение bf(x) = " + bf.ToString());
            //Console.WriteLine("f - bf = " + (bf - y));
            //Console.WriteLine("||f - spline|| = " + result.ToString("0.0000"));
            Assert.AreEqual(0, interpolation, EPS, "Плохая интерполяция!");
        }
    }
}
