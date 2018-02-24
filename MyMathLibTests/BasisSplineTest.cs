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
        [ExpectedException(typeof(ArgumentException), "Нет выброса исключения про выход за границы массива")]
        [TestMethod]
        public void ClassicBasis_jmin1_exeption()
        {
            //setup
            int deg = 2; //кубический сплайн
            MyMathLib.Grid tau = new MyMathLib.Grid(4, 0, 3);


            //run
            MyMathLib.BasisSpline.DeBoorMethods.ClassicBasisSpline(0.5d, tau, deg, -1);

            //compare
           

        }


        [TestMethod]
        public void ClassicBasis_x_outof_suppBj_return0()
        {
            //setup
            int deg = 2; //кубический сплайн
            MyMathLib.Grid tau = new MyMathLib.Grid(4, 0, 3);

            double expect = 0d;
            //run
            double actual = MyMathLib.BasisSpline.DeBoorMethods.ClassicBasisSpline(-0.5d, tau, deg, 0);

            //compare
            Assert.AreEqual(expect, actual,0.00000001d,"Должно быть нулем вне носителя");

        }

        [TestMethod]
        public void ClassicBasis_x_sum_return1()
        {
            //setup
            Random r = new Random();
            int deg = 2; //кубический сплайн
            int OriginalDotsCount = 4;
            MyMathLib.Grid tau = new MyMathLib.Grid(MyMathLib.GridType.ClassicQubic, OriginalDotsCount, 0, 3);
            double x = 3d*r.NextDouble();

            double expect = 1d;
            //run
            double actual = 0;

            for(int index = 0; index < 4; index++)
            actual += MyMathLib.BasisSpline.DeBoorMethods.ClassicBasisSpline(x, tau, deg, index);

            //compare
            Assert.AreEqual(expect, actual, 0.00000001d, "В любой точке сумма сплайнов должна ровняться 1");

        }

        [TestMethod]
        public void BSPLVB_x_sumB_return1()
        {
            //setup
            int deg = 3; //кубический сплайн
            MyMathLib.Grid tau = new MyMathLib.Grid(MyMathLib.GridType.ClassicQubic,4, 0, 3);
            double x = 0.5d;
            double expect = 1d;
            //run
            double []mas = MyMathLib.BasisSpline.DeBoorMethods.BSPLVB(x, tau, deg);
            double actual = MyMathLib.MyMath.Basic.SumArray(mas);

            //compare
            Assert.AreEqual(expect, actual, 0.00000001d, "Сумма сплайнов должна ровняться 1");

        }

        [TestMethod]
        public void BSPLVB_comparewithClassic_returnCBx()
        {
            //setup
            int deg = 3; //кубический сплайн
            MyMathLib.Grid tau = new MyMathLib.Grid(MyMathLib.GridType.ClassicQubic, 4, 0, 3);
            double x = 1.25d;
            double expect = MyMathLib.BasisSpline.DeBoorMethods.StandartB(x,tau,1);
            //run
            double[] mas = MyMathLib.BasisSpline.DeBoorMethods.BSPLVB(x, tau, deg);
            double actual = mas[0];

            //compare
            Assert.AreEqual(expect, actual, 0.00000001d, "Должны совпадать рекурсивный и bsplvb");

        }

    }
}
