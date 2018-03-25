using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyMathLib
{
    [TestClass]
    public class MyMathOperatorsTest
    {
        [TestMethod]
        public void ForwardDifferencesTest_0149_0()
        {
            //setup
            double[] mas = { 0, 1, 4, 9 };
            Vector div = new Vector(mas);
            double expect = 0d;
            //run
            double actual = MyMath.Operations.ForwardDifferences(div);

            //compare
            Assert.AreEqual(expect, actual, 0.0000001d, "Прямые разности считаеются не верно. Тут должен быть 0");
        }

        [TestMethod]
        public void ForwardDifferencesTest_01827_6()
        {
            //setup
            double[] mas = { 0, 1, 8, 27 };
            Vector div = new Vector(mas);
            double expect = 6d;
            //run
            double actual = MyMath.Operations.ForwardDifferences(div);

            //compare
            Assert.AreEqual(expect, actual, 0.0000001d, "Прямые разности считаеются не верно. Тут должен быть 0");
        }
    }
}
