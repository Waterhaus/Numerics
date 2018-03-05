using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyMathLibTests
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


       
    }
}
