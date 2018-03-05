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

        [TestMethod]
        public void ClassicBasis_1234grid_return_pol2()
        {
            //setup
            int deg = 2;
            MyMathLib.Grid tau = new MyMathLib.Grid(MyMathLib.GridType.ClassicQuadratic, 4, 0, 3);
            double x = 2.6d;
            double expect = (3d - x) * (3d - x) / 2d;
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
            int deg = 2; //квадратичный сплайн
            MyMathLib.Grid tau = new MyMathLib.Grid(MyMathLib.GridType.ClassicQuadratic, 4, 0, 3);


            //run
            MyMathLib.BasisSpline.DeBoorMethods.ClassicBasisSpline(0.5d, tau, deg, -1);

            //compare


        }


        [TestMethod]
        public void ClassicBasis_x_outof_suppBj_return0()
        {
            //setup
            int deg = 2;  //квадратичный сплайн
            MyMathLib.Grid tau = new MyMathLib.Grid(MyMathLib.GridType.ClassicQuadratic, 4, 0, 3);

            double expect = 0d;
            //run
            double actual = MyMathLib.BasisSpline.DeBoorMethods.ClassicBasisSpline(-0.5d, tau, deg, 0);

            //compare
            Assert.AreEqual(expect, actual, 0.00000001d, "Должно быть нулем вне носителя");

        }

       

        

        [TestMethod]
        public void BSPLVB_x_sumB_return1()
        {
            //setup
            int deg = 3; //кубический сплайн
            MyMathLib.Grid tau = new MyMathLib.Grid(MyMathLib.GridType.ClassicQuadratic, 4, 0, 3);
            double x = 0.5d;
            double expect = 1d;
            //run
            double[] mas = MyMathLib.BasisSpline.DeBoorMethods.BSPLVB(x, tau, deg);
            double actual = MyMathLib.MyMath.Basic.SumArray(mas);

            //compare
            Assert.AreEqual(expect, actual, 0.00000001d, "Сумма сплайнов должна ровняться 1");

        }

        [TestMethod]
        public void BSPLVB_comparewithClassic_returnCBx()
        {
            //setup
            int deg = 3; //квадратичный сплайн
            MyMathLib.Grid tau = new MyMathLib.Grid(MyMathLib.GridType.ClassicQuadratic, 4, 0, 3);
            double x = 0.25d;
            double expect = MyMathLib.BasisSpline.DeBoorMethods.StandartB(x, tau, 0);
            //run
            double[] mas = MyMathLib.BasisSpline.DeBoorMethods.BSPLVB(x, tau, deg);
            MyMathLib.Vector v = new MyMathLib.Vector(mas);
            Console.WriteLine("b = " + v.ToString());
            double actual = mas[0];

            //compare
            Assert.AreEqual(expect, actual, 0.00000001d, "Должны совпадать рекурсивный и bsplvb");

        }
        
       

      

        [TestMethod]
        public void DeBoorB_x_like_StandartB()
        {
            //setup

            MyMathLib.Grid tau = new MyMathLib.Grid(MyMathLib.GridType.ClassicQuadratic, 4, 0, 3);
            double x = 1.25d;
            int index = 2;
            int deg = 3;
            double expect = MyMathLib.BasisSpline.DeBoorMethods.StandartB(x, tau, index);
            //run

            double actual = MyMathLib.BasisSpline.DeBoorMethods.DeBoorB(x, tau, deg, index);

            //compare
            Assert.AreEqual(expect, actual, 0.00000001d, "Должны совпадать StandartB и DeBoorB");

        }

        [TestMethod]
        public void DeBoorB_behavior()
        {
            //setup
            double x = 0.1d;
            MyMathLib.Grid tau = new MyMathLib.Grid(MyMathLib.GridType.ClassicQubic, 6, 0, 5);
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
