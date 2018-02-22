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
            int deg = 2;
            MyMathLib.Grid tau = new MyMathLib.Grid(4, 0, 3);
            double x = 0.55d;
            double expect = x * x / 2d;
            double EPS = 0.00000001d;
            //run
            double actual = MyMathLib.BasisSpline.DeBoorMethods.ClassicBasisSpline(x, tau, deg, 0);

            //compare
            Assert.AreEqual(expect, actual, EPS, "ERROR! Не считает классический сплайн как надо");

        }

        [TestMethod]
        public void ClassicBasis_1234grid_return_pol2()
        {
            //setup
            int deg = 2;
            MyMathLib.Grid tau = new MyMathLib.Grid(MyMathLib.GridType.ClassicQubic,4, 0, 3);
            double x = 2.5d;
            double expect = (3d - x)*(3d - x) / 2d;
            double EPS = 0.00000001d;
            //run
            double actual = MyMathLib.BasisSpline.DeBoorMethods.ClassicBasisSpline(x, tau, deg, 2);

            //compare
            Assert.AreEqual(expect, actual, EPS, "ERROR! Не считает классический сплайн как надо" + tau.ToString());

        }
    }
}
