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
    }
    
    
}
