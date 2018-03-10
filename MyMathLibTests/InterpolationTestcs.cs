using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyMathLib
{
    [TestClass]
    public class InterpolationTestcs
    {
        [TestMethod]
        public void Interpolate_slow_deg2_tsin()
        {
            //setup
            double EPS = 0.1d;
            double a = 0d;
            double b = 3d;
            int GridSize = 20;
            int deg = 2;
            Grid grid = new Grid(GridSize, a, b);
            Vector y = MyMath.Basic.GetVectorFunction(GridSize, a, b, FunctionLib.tsin);

            //run
            BasisSpline bspline = new BasisSpline(deg, grid, y);

            //compare
            int N = 150;
            Vector expect = MyMath.Basic.GetVectorFunction(N, a, b, FunctionLib.tsin);
            Vector actual = bspline.GetVectorFunction(N, a, b);

            double result = (expect - actual).Norm;
            Console.WriteLine("Степень сплайна = " + deg);
            Console.WriteLine("Сетка по которой построен сплайн = " + bspline.grid.ToString());
            Console.WriteLine("значения х = " + grid.ToString());
            Console.WriteLine("значение f(x) = " + y.ToString());
            Console.WriteLine("coef = " + bspline.COEFICIENT.ToString());
            Console.WriteLine("||f - spline|| = " + result.ToString("0.0000"));
            Assert.AreEqual(0, result, EPS, "Плохая интерполяция!");

        }

        

        [TestMethod]
        public void Interpolate_slow_deg4_tsin()
        {
            //setup
            double EPS = 0.1d;
            double a = 0d;
            double b = 3d;
            int GridSize = 10;
            int deg = 4;
            Grid grid = new Grid(GridSize, a, b);
            Vector y = MyMath.Basic.GetVectorFunction(GridSize, a, b, FunctionLib.tsin);

            //run
            BasisSpline bspline = new BasisSpline(deg, grid, y);

            //compare
            int N = 150;
            Vector expect = MyMath.Basic.GetVectorFunction(N, a, b, FunctionLib.tsin);
            Vector actual = bspline.GetVectorFunction(N, a, b);

            double result = (expect - actual).Norm;
            Console.WriteLine("Степень сплайна = " + deg);
            Console.WriteLine("Сетка по которой построен сплайн = " + bspline.grid.ToString());
            Console.WriteLine("значения х = " + grid.ToString());
            Console.WriteLine("значение f(x) = " + y.ToString());
            Console.WriteLine("coef = " + bspline.COEFICIENT.ToString());
            Console.WriteLine("||f - spline|| = " + result.ToString("0.0000"));
            Assert.AreEqual(0, result, EPS, "Плохая интерполяция!");

        }

        [TestMethod]
        public void Interpolate_slow_deg4_tttsin()
        {
            //setup
            double EPS = 0.1d;
            double a = 0d;
            double b = 3d;
            int GridSize = 15;
            int deg = 4;
            Grid grid = new Grid(GridSize, a, b);
            Vector y = MyMath.Basic.GetVectorFunction(GridSize, a, b, FunctionLib.tttsin);

            //run
            BasisSpline bspline = new BasisSpline(deg, grid, y);

            //compare
            int N = 150;
            Vector expect = MyMath.Basic.GetVectorFunction(N, a, b, FunctionLib.tttsin);
            Vector actual = bspline.GetVectorFunction(N, a, b);

            double result = (expect - actual).Norm;
            Console.WriteLine("Степень сплайна = " + deg);
            Console.WriteLine("Сетка по которой построен сплайн = " + bspline.grid.ToString());
            Console.WriteLine("значения х = " + grid.ToString());
            Console.WriteLine("coef = " + bspline.COEFICIENT.ToString());
            Console.WriteLine("значение f(x) = " + y.ToString());
            Console.WriteLine("||f - spline|| = " + result.ToString("0.0000"));
            Assert.AreEqual(0, result, EPS, "Плохая интерполяция!");

        }

        [TestMethod]
        public void Interpolate_slow_deg6_tttsin()
        {
            //setup
            double EPS = 0.1d;
            double a = 0d;
            double b = 3d;
            int GridSize = 15;
            int deg = 6;
            Grid grid = new Grid(GridSize, a, b);
            Vector y = MyMath.Basic.GetVectorFunction(GridSize, a, b, FunctionLib.tttsin);

            //run
            BasisSpline bspline = new BasisSpline(deg, grid, y);

            //compare
            int N = 150;
            Vector expect = MyMath.Basic.GetVectorFunction(N, a, b, FunctionLib.tttsin);
            Vector actual = bspline.GetVectorFunction(N, a, b);

            double result = (expect - actual).Norm;
            Console.WriteLine("Степень сплайна = " + deg);
            Console.WriteLine("Сетка по которой построен сплайн = " + bspline.grid.ToString());
            Console.WriteLine("значения х = " + grid.ToString());
            Console.WriteLine("coef = " + bspline.COEFICIENT.ToString());
            Console.WriteLine("значение f(x) = " + y.ToString());
            Console.WriteLine("||f - spline|| = " + result.ToString("0.0000"));
            Assert.AreEqual(0, result, EPS, "Плохая интерполяция!");

        }


    }
}
