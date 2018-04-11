using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace MyMathLib
{
   

        [TestClass]
        public class BsplineExperiment
        {
           

        [TestMethod]
        public void DeBoorB_continuity()
        {
            //setup
            int GridSize = 1000;
            double a = 0;
            double b = 1d;
            double EPS = 0.01d;

            int deg = 4;
            double[] knonts = { 0, 1d / 3d, 2d / 3d, 1 };
            Grid tau = new Grid(deg, knonts);

            //run
            Vector f = BasisSpline.DeBoorMethods.GetVectorDeBoorB(GridSize, a, b, tau, deg, 5);
            Console.WriteLine(f.ToString());
            Vector f_copy = new Vector(f.Length);
            Vector.copy(ref f_copy, f);
            for (int i = 1; i < GridSize; i++)
            {
                f[i] = f[i] - f_copy[i - 1];
            }
            f[0] = 0;
            f.Last = 0;
            
            double max = f.InfNorm;
            Console.WriteLine("NORM = " + max.ToString());
            //comp
            Assert.AreEqual(0, max, EPS, "Есть скачек! Функция не является непрерывной");
            
        }

        [TestMethod]
        public void DeBoorB_Integrate()
        {
            //setup
            int GridSize = 20;
            double a = 0;
            double b = 1d;
            double c = 10d;
            double EPS = 0.01d;
            int N = 1000;
            int index = 2;
            Vector grid1 = Vector.CreateUniformGrid(N, a, b);
            Vector grid2 = Vector.CreateUniformGrid(N, a, c);

            

            int deg = 4;

            BasisSpline spline1 = new BasisSpline(deg, GridSize, a, b);
            BasisSpline spline2 = new BasisSpline(deg, GridSize, a, c);

            //run

            for (index = 0; index < GridSize; index++)
            {
                Console.WriteLine("index = " + index);
                Vector f1 = spline1.GetVectorBasis(grid1, index);
                Vector f2 = spline2.GetVectorBasis(grid2, index);

                double I1 = MyMathLib.Integrate.RiemannSum(f1.ToArray, MyMathLib.MyMath.Basic.GetStep(N, a, b));
                double I2 = MyMathLib.Integrate.RiemannSum(f2.ToArray, MyMathLib.MyMath.Basic.GetStep(N, a, c));

                Console.WriteLine("Integral B"+index+" on [0, 1] = " + I1);
                Console.WriteLine("Integral B" + index + " on [0, 10] = " + I2);
                //comp
                Assert.IsTrue(I2 > I1, "Интеграл с функции большем носителем должен быть больше!");

            }


        }

        [TestMethod]
        public void DeBoorB_1_return1()
        {
            int deg = 4;
            double[] knonts = { 0d, 1d / 3d, 2d / 3d, 1d };
            Grid tau = new Grid(deg, knonts);
            double expect = 1d;
            double EPS = 0.00001d;
            //run
            double actual = BasisSpline.DeBoorMethods.DeBoorB(1d, tau, deg, 5);

            //comp
            Assert.AreEqual(expect, actual, EPS, "Должно обращаться в этой точке в 1");
        }

        [TestMethod]
        public void DeBoorB_3_6_return0()
        {
            int deg = 4;

            Vector grid = Vector.CreateUniformGrid(10, 0, 3);
            Grid tau = new Grid(deg, grid, 0, 3);

            double expect = 0d;
            double EPS = 0.00001d;
            //run
            double actual = BasisSpline.DeBoorMethods.DeBoorB(3d, tau, deg, 6);

            //comp
            Assert.AreEqual(expect, actual, EPS, "Должно обращаться в этой точке в 1");
        }

        [TestMethod]
        public void AbsolutUniformGridInterpolationMatrixExperimentTest()
        {
            //подготовка входных данных
            int GridSize = 11;
            double a = 0d;
            double b = 1d;
            int deg = 4;
            MyMathLib.Grid grid = new MyMathLib.Grid(deg, GridSize, a, b);
            grid.ToAbsolutUniformSplineGrid();
            double expect = 0.167d;
            //выполнение кода
            Matrix A = BasisSpline.DeBoorMethods.SlowCreateInterpolationMatrix(grid, deg);

            Console.WriteLine("beginIndex = " + grid.BeginIndex);
            Console.WriteLine("endIndex = " + grid.EndIndex);
            Console.WriteLine(grid);
            Console.WriteLine(A);
            double actual = A[0, 0];
            //compare
            Assert.AreEqual(expect, actual, 0.01d);
        }
    }
    
    
}
