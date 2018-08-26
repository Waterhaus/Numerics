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

            Console.WriteLine(A*Matrix.transpose(A));
           
        }
        [TestMethod]
        public void InterpolationMatrixTest2()
        {
            //setup
            double EPS = 0.1d;
            double a = 0d;
            double b = 1d;
            int GridSize = 18;
            int deg = 5;
            Vector grid = Vector.CreateUniformGrid(GridSize, a, b);
            double h = MyMath.Basic.GetStep(GridSize, a, b);
            Vector y = MyMath.Basic.GetVectorFunction(GridSize, a, b, FunctionLib.one_minus_t_sin);
            Grid tau = new Grid(deg, grid, grid[0], grid.Last, true);
            tau.ToPeriodiclineGrid();

            double[] ksi = Spline.InterpolateExperiment.GetCardinalValue(deg,h);
            Vector one = Vector.GetConstVector(1.0, GridSize);
            //run
            Matrix A = CardinalSpline.CreateInterpolationMatrix(tau, h, deg);
            Matrix B = Spline.InterpolateExperiment.CreatePeriodicInterpolationMatrix(ksi,GridSize);

            double result = ((A - B) * one).Norm;
            

            Console.WriteLine("resurt = " + result);
            Assert.AreEqual(0, result, 0.00001d);
        }

        [TestMethod]
        public void MinInterpolationMatrixTest()
        {
            //setup
            int degree = 4;
            int size = 9;
            double h = 1;


            //run
            Spline.InterpolateExperiment.MinInterpolationMatrix(degree, h, size);
        }


        [TestMethod]
        public void CreateInterpolationMatrixTest()
        {
            //setup
            int degree = 4;
            int size = 9;
            double h = 1;
            double[] ksi = Spline.InterpolateExperiment.GetCardinalValue(degree, h);


            //run
            Matrix A = Spline.InterpolateExperiment.CreateInterpolationMatrix(ksi, size);
            Console.WriteLine(A);
        }


        [TestMethod]
        public void MultiplyBSplineMatrixTest()
        {
            //setup
            int degree = 4;
            int size = 9;
            double h = 1;
            double[] ksi = Spline.InterpolateExperiment.GetCardinalValue(degree, h);
            Matrix A = Spline.InterpolateExperiment.CreateInterpolationMatrix(ksi, size);
            Vector one = Vector.GetConstVector(1d, size + degree - 2);
            //run
            double result = (A * one - Spline.InterpolateExperiment.MultiplyBSplineMatrix(one, degree, h, size)).Norm;

            //compare
            Assert.AreEqual(0d, result, 0.000001, "Функции не эквивалентны!");
        }

        [TestMethod]
        public void CreateKSIMatrixTest()
        {
            //setup
            int degree = 2;
            int size = 9;
            double h = 1;



            //run
            Matrix A = Spline.InterpolateExperiment.CreateKSIMatrix(degree, h, size);
            Console.WriteLine(A);
        }


        [TestMethod]
        public void MultiplyKSIMatrixTest()
        {
            //setup
            int degree = 2;
            int size = 9;
            double h = 1;
            Matrix A = Spline.InterpolateExperiment.CreateKSIMatrix(degree, h, size);
            Vector one = Vector.GetConstVector(1, size);

            //run
            double result = (A * one - Spline.InterpolateExperiment.MultiplyKSIMatrix(one, degree, h, size)).Norm;

            //compare
            Assert.AreEqual(0d, result, 0.000001, "Функции не эквивалентны!");
        }

        [TestMethod]
        public void Create_InterpolationMatrix_Test()
        {
            //setup
            int degree = 5;
            int size = 9;
            double h = 1;



            //run
            Matrix A = Spline.InterpolateExperiment.Create_LagrangeInterpolationMatrix(degree, h, size);
            Console.WriteLine(A);
        }

        [TestMethod]
        public void MultiplyLagrangeMatrixTest()
        {
            //setup
            int degree = 4;
            int size = 3000;
            double h = 0.0001d;
            
            Matrix A = Spline.InterpolateExperiment.Create_LagrangeInterpolationMatrix(degree, h, size);
            Vector one = Vector.GetConstVector(1d, 2*size + degree - 2);
            Vector r = Vector.RandomVector(2 * size + degree - 2);
            //run
            Vector anser = A * r - Spline.InterpolateExperiment.Multiply_LagrangeInterpolationMatrix(r, degree, h, size);
            double result = (anser).Norm;
            Console.WriteLine((A*r).Norm);
            //compare
            Console.WriteLine("result = " + result);
            Assert.AreEqual(0d, result, 0.000001d, "Функции не эквивалентны!");
        }

        [TestMethod]
        public void MIN_Interpolation_Test()
        {
            //setup
            double EPS = 0.1d;
            double a = 0d;
            double b = Math.PI;
            int GridSize = 200;
            int deg = 4;
            Vector grid = Vector.CreateUniformGrid(GridSize, a, b);
            double h = MyMath.Basic.GetStep(GridSize, a, b);
            Vector y = MyMath.Basic.GetVectorFunction(GridSize, a, b, FunctionLib.sin);
            Grid tau = new Grid(deg, grid, grid[0], grid.Last, true);
            tau.ToPeriodiclineGrid();
            //run
            //Vector c = CardinalSpline.Interpolate(y, tau, deg, h);

            Vector min_c = Spline.InterpolateExperiment.MIN_Interpolate(y, deg, h);
           // Console.WriteLine("c = " + c);
            Console.WriteLine("min_c = " + min_c);
            Console.WriteLine("Степень сплайна = " + deg);
            //compare
            int N = 2 * GridSize;
            Vector expect = MyMath.Basic.GetVectorFunction(N - 1, a, b, FunctionLib.sin);
            Vector actual = CardinalSpline.GetVectorFunctionSpline(N - 1, a, b, min_c, a, h, deg);

            Vector bf = CardinalSpline.GetVectorFunctionSpline(GridSize, a, b, min_c, a, h, deg);


            double result = (expect - actual).Norm;
            double interpolation = (y - bf).Norm;
          
           
            Console.WriteLine("значение f(x) = " + y.ToString());
            Console.WriteLine("значение bf(x) = " + bf.ToString());
            //Console.WriteLine("exp - act = " + (expect - actual).ToString());
            Console.WriteLine("||c|| = " + min_c.Norm.ToString("0.000000"));
            Console.WriteLine("||f - spline|| = " + result.ToString("0.000000"));
            Assert.AreEqual(0, interpolation, EPS, "Плохая интерполяция!");
        }

        [TestMethod]
        public void Quick_Interpolation_Test()
        {
            //setup
            double EPS = 0.1d;
            double a = 0d;
            double b =  Math.PI;
            int GridSize = 18;
            int deg = 4;
            Vector grid = Vector.CreateUniformGrid(GridSize, a, b);
            double h = MyMath.Basic.GetStep(GridSize, a, b);
            Vector y = MyMath.Basic.GetVectorFunction(GridSize, a, b, FunctionLib.sin);
            Grid tau = new Grid(deg, grid, grid[0], grid.Last, true);
            tau.ToPeriodiclineGrid();
            //run
            //Vector c = CardinalSpline.Interpolate(y, tau, deg, h);

            Vector min_c = Spline.InterpolateExperiment.Quick_Interpolate(y, deg, h);
            // Console.WriteLine("c = " + c);
            Console.WriteLine("min_c = " + min_c);
            Console.WriteLine("Степень сплайна = " + deg);
            //compare
            int N = 10 * GridSize;
            Vector expect = MyMath.Basic.GetVectorFunction(N - 1, a, b, FunctionLib.sin);
            Vector actual = CardinalSpline.GetVectorFunctionSpline(N - 1, a, b, min_c, a, h, deg);

            Vector bf = CardinalSpline.GetVectorFunctionSpline(GridSize, a, b, min_c, a, h, deg);


            double result = (expect - actual).Norm;
            double interpolation = (y - bf).Norm;


            Console.WriteLine("значение f(x) = " + y.ToString());
            Console.WriteLine("значение bf(x) = " + bf.ToString());
            //Console.WriteLine("exp - act = " + (expect - actual).ToString());
            Console.WriteLine("||c|| = " + min_c.Norm.ToString("0.000000"));
            Console.WriteLine("||f - spline|| = " + result.ToString("0.000000"));
            Assert.AreEqual(0, interpolation, EPS, "Плохая интерполяция!");
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
            int N = 2*GridSize + 1;
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
        public void MinInterpolationTest()
        {
            //setup
            double EPS = 0.1d;
            double a = 0d;
            double b = Math.PI;
            int GridSize = 9;
            int deg = 4;
            Vector grid = Vector.CreateUniformGrid(GridSize, a, b);
            double h = MyMath.Basic.GetStep(GridSize, a, b);
            Vector y = MyMath.Basic.GetVectorFunction(GridSize, a, b, FunctionLib.sin);
            Grid tau = new Grid(deg, grid, grid[0], grid.Last, true);
            tau.ToPeriodiclineGrid();
            //run
            Vector c = CardinalSpline.Interpolate(y, tau, deg, h);

            Vector min_c = Spline.InterpolateExperiment.Interpolate(y, deg, h);
            Console.WriteLine("c = " + c);
            Console.WriteLine("min_c = " + min_c);
            Console.WriteLine("Степень сплайна = " + deg);
            //compare
            int N = 2 * GridSize;
            //Vector expect = MyMath.Basic.GetVectorFunction(N, a, b, FunctionLib.sin);
            //Vector actual = CardinalSpline.GetVectorFunction(N, a, b, c, a, h, deg);

            //Vector bf = CardinalSpline.GetVectorFunction(GridSize, a, b, c, a, h, deg);


            //double result = (expect - actual).Norm;
            //double interpolation = (y - bf).Norm;
            ///Console.WriteLine("Сетка по которой построен сплайн = " + bspline.grid.ToString());
            //Console.WriteLine("значения х = " + grid.ToString());
            Console.WriteLine("значение f(x) = " + y.ToString());
            //Console.WriteLine("значение bf(x) = " + bf.ToString());
            //Console.WriteLine("exp - act = " + (expect - actual).ToString());
            //Console.WriteLine("coef = " + bspline.COEFICIENT.ToString());
            //Console.WriteLine("||f - spline|| = " + result.ToString("0.0000"));
            //Assert.AreEqual(0, interpolation, EPS, "Плохая интерполяция!");
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

        [TestMethod]
        public void GetCoef_2h_Test()
        {
            int degree = 6;
            Vector c = Spline.InterpolateExperiment.GetCoef_2h(degree);
            Console.WriteLine("c = " + c);
        }
    }
}
