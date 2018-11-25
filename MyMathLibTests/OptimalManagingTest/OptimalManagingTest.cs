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

       
    }
}
