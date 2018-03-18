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
            Vector grid = Vector.CreateUniformGrid(GridSize, a, b);
            Vector y = MyMath.Basic.GetVectorFunction(GridSize, a, b, FunctionLib.tsin);

            //run
            BasisSpline bspline = new BasisSpline(deg, grid, y,GridType.ClassicSplineGrid);

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
            Vector grid = Vector.CreateUniformGrid(GridSize, a, b);
            Vector y = MyMath.Basic.GetVectorFunction(GridSize, a, b, FunctionLib.tsin);

            //run
            BasisSpline bspline = new BasisSpline(deg, grid, y,GridType.ClassicSplineGrid);

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
        public void Interpolate_slow_deg4_tsin_PROJECTION_test()
        {
            //setup
            double EPS = 0.1d;
            double a = 0d;
            double b = 3d;
            int GridSize = 17;
            int deg = 4;
            Vector grid = Vector.CreateUniformGrid(GridSize, a, b);
            Vector y = MyMath.Basic.GetVectorFunction(GridSize, a, b, FunctionLib.tsin);

            //run
            BasisSpline bspline_h = new BasisSpline(deg,GridSize,a,b);
            Vector c_h = BasisSpline.Interpolate(y, bspline_h.grid, deg);
            bspline_h.SetNewCoefs(c_h);
            Vector c_2h = MultyGrid.Proektor.slow_spline_r(c_h);
            BasisSpline bspline_2h = bspline_h.GetBasisSpline_Down();
            bspline_2h.SetNewCoefs(c_2h);
            //compare
            int N = 50;
            Vector expect = MyMath.Basic.GetVectorFunction(N, a, b, FunctionLib.tsin);
            Vector actual = bspline_2h.GetVectorFunction(N, a, b);

            Console.WriteLine((expect - actual).ToString());
            double result = (expect - actual).Norm;
            Console.WriteLine("Степень сплайна = " + deg);
            Console.WriteLine("Сетка по которой построен сплайн = " + bspline_2h.grid.ToString());
            Console.WriteLine("значения х = " + grid.ToString());
            Console.WriteLine("значение f(x) = " + y.ToString());
            Console.WriteLine("coef h = " + c_h.ToString());
            Console.WriteLine("coef 2h = " + bspline_2h.COEFICIENT.ToString());
            Console.WriteLine("||f - spline|| = " + result.ToString("0.0000"));
            Assert.AreEqual(0, result, EPS, "Плохая интерполяция!");

        }

        [TestMethod]
        public void Interpolate_slow_deg4_tsin_CompareCoefs_test()
        {
            //setup
            double EPS = 0.1d;
            double a = 0d;
            double b = 3d;
            int GridSize = 17;
            int deg = 4;
            Vector grid = Vector.CreateUniformGrid(GridSize, a, b);
            Vector y = MyMath.Basic.GetVectorFunction(GridSize, a, b, FunctionLib.tsin);

            //run
            BasisSpline bspline_h = new BasisSpline(deg, GridSize, a, b);
            Vector c_h = BasisSpline.Interpolate(y, bspline_h.grid, deg);
            bspline_h.SetNewCoefs(c_h);
            Vector c_2h_actual = MultyGrid.Proektor.slow_spline_r(c_h);
            
            BasisSpline bspline_2h = bspline_h.GetBasisSpline_Down();
            Vector y_2h = MyMath.Basic.GetVectorFunction(c_2h_actual.Length, a, b, FunctionLib.tsin);
            Vector c_2h_expect = BasisSpline.Interpolate(y_2h, bspline_2h.grid, deg);
            bspline_2h.SetNewCoefs(c_2h_actual);
            //compare


            Console.WriteLine((c_2h_expect - c_2h_actual).ToString());
            double result = (c_2h_expect - c_2h_actual).Norm;
            Console.WriteLine("Степень сплайна = " + deg);
            Console.WriteLine("Сетка по которой построен сплайн = " + bspline_2h.grid.ToString());
           
            Console.WriteLine("actual c 2h = " + c_2h_actual.ToString());
            Console.WriteLine("expect c 2h = " + c_2h_expect.ToString());
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
            Vector grid = Vector.CreateUniformGrid(GridSize, a, b);
            Vector y = MyMath.Basic.GetVectorFunction(GridSize, a, b, FunctionLib.tttsin);

            //run
            BasisSpline bspline = new BasisSpline(deg, grid, y,GridType.ClassicSplineGrid);

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
            Vector grid = Vector.CreateUniformGrid(GridSize, a, b);
            Vector y = MyMath.Basic.GetVectorFunction(GridSize, a, b, FunctionLib.tttsin);

            //run
            BasisSpline bspline = new BasisSpline(deg, grid, y,GridType.ClassicSplineGrid);

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
