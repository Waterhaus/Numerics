using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMathLib;
namespace MyMathLibTests.OptimalManagingTest
{
    [TestClass]
    public class OptimalManagingTest
    {
        public class TestFunction1
        {
            public static double aa = 1d;
            public static double U(double x, double t)
            {
                return Math.Exp(x + 2 * t);
            }

            public static double phi(double x)
            {
                return U(x, 0);
            }

            public static double f(double x, double t)
            {
                return (2d - aa)* Math.Exp(2d * t + x);
            }

            public static double p(double t)
            {
                return 9*t;
            }

            public static double y(double x)
            {
                return 7d;
            }
        };

        [TestMethod]
        public void Test1()
        {

            MyMathLib.OptimalManaging.OptimalManaging optm = new MyMathLib.OptimalManaging.OptimalManaging(1d, 1d, TestFunction1.aa, 1d, 100, 25, 0.1, 0.1,
               TestFunction1.y, TestFunction1.p, TestFunction1.phi, TestFunction1.f);

            Vector calc_p = optm.CalculateIteration();
            double J = optm.Functional_J(optm.calc_u);
            Console.WriteLine(calc_p);
            Console.WriteLine("J1 = " + J);

            calc_p = optm.CalculateIteration();
            J = optm.Functional_J(optm.calc_u);
            Console.WriteLine(calc_p);
            Console.WriteLine("J2 = " + J);


            for (int i = 0; i < 100; i++)
            {

                calc_p = optm.CalculateIteration();
                J = optm.Functional_J(optm.calc_u);
                Console.WriteLine(calc_p);
                Console.WriteLine("J" + (i + 2) +" = " + J);
            }

        }

    }
}
