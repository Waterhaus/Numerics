using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyMathLib
{
    [TestClass]
    public class BasisSplineTest
    {
        [TestMethod]
        public void ClassicBasis_1234grid_return_pol1()
        {
            //setup
            int deg = 2; //квадратичный сплайн
            MyMathLib.Grid tau = new MyMathLib.Grid(4, 0, 3);
            double x = 0.55d;
            double expect = x * x / 2d;
            double EPS = 0.00000001d;
            //run
            double actual = MyMathLib.BasisSpline.DeBoorMethods.ClassicBasisSpline(x, tau, deg, 0);

            //compare
            Assert.AreEqual(expect, actual, EPS, "ERROR! Не считает классический сплайн как надо");

        }

       
        [ExpectedException(typeof(ArgumentException), "Нет выброса исключения про выход за границы массива")]
        [TestMethod]
        public void ClassicBasis_jmin1_exeption()
        {
            //setup
            int deg = 2; //квадратичный сплайн
            MyMathLib.Grid tau = new MyMathLib.Grid(2, 4, 0, 3);


            //run
            MyMathLib.BasisSpline.DeBoorMethods.ClassicBasisSpline(0.5d, tau, deg, -1);

            //compare


        }


        [TestMethod]
        public void ClassicBasis_x_outof_suppBj_return0()
        {
            //setup
            int deg = 2;  //квадратичный сплайн
            MyMathLib.Grid tau = new MyMathLib.Grid(2, 4, 0, 3);

            double expect = 0d;
            //run
            double actual = MyMathLib.BasisSpline.DeBoorMethods.ClassicBasisSpline(-0.5d, tau, deg, 0);

            //compare
            Assert.AreEqual(expect, actual, 0.00000001d, "Должно быть нулем вне носителя");

        }

       

      

        

        [TestMethod]
        public void DeBoorB_behavior()
        {
            //setup
            double x = 0.1d;
            MyMathLib.Grid tau = new MyMathLib.Grid(4, 6, 0, 5);
            int index = 0;
            int deg = 4;
            Console.WriteLine("сетка = " + tau.ToString());

            //run
            double B = MyMathLib.BasisSpline.DeBoorMethods.DeBoorB(x, tau,deg, index);
            bool expect = B > 0;
            //compare
            Assert.AreEqual(expect, true);

        }

        [TestMethod]
        public void RightUniformBSplineTest()
        {
            //setup
            int GridSize = 5;
            double a = 0;
            double b = 5d;
            int N = 1000;
            double h = MyMath.Basic.GetStep(GridSize, a, b);
            int deg = 4;
            int index = 0;

            //Vector x = new Vector(MyMath.Basic.CreateUniformGrid(N, a - 2 * h, b + deg * h));
            BasisSpline spline = new BasisSpline(deg, GridSize, a, b, GridType.RightUniformSplineGrid);
            //run
            double Ba = spline.GetBasis(0.1d, index);

            //compare
            Assert.AreEqual(0d, Ba, 0.000001d);
        }


        [TestMethod]
        public void SimpleUniformBSplineTest_dim()
        {
            //setup
            int GridSize = 2;
            double a = 0;
            double b = 7d;
            int deg = 4;
            int index = 0;

            BasisSpline spline = new BasisSpline(deg, GridSize, a, b, GridType.SimpleSplineGrid);
            Console.WriteLine(new Vector(MyMath.Basic.CreateUniformGrid(GridSize, a, b))); 
            int expect = GridSize + 2 * (deg - 1) + 2*(deg - 2);
            //run
            int actual = spline.grid.Count;

            //compare
            Console.WriteLine( "expect = " + expect + "; actual = " + actual);
            Assert.AreEqual(expect,actual,"размерности совпадают");
        }


    }
}
