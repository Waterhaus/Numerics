using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyMathLibTests
{
    /// <summary>
    /// Сводное описание для VectorClassTest
    /// </summary>
    [TestClass]
    public class VectorClassTest
    {
        public VectorClassTest()
        {
            //
            // TODO: добавьте здесь логику конструктора
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Получает или устанавливает контекст теста, в котором предоставляются
        ///сведения о текущем тестовом запуске и обеспечивается его функциональность.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Дополнительные атрибуты тестирования
        //
        // При написании тестов можно использовать следующие дополнительные атрибуты:
        //
        // ClassInitialize используется для выполнения кода до запуска первого теста в классе
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // ClassCleanup используется для выполнения кода после завершения работы всех тестов в классе
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // TestInitialize используется для выполнения кода перед запуском каждого теста 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // TestCleanup используется для выполнения кода после завершения каждого теста
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Skalar_v1_and_v1_returnNorm()
        {
            //настроить входные параметры
            int n = 10;
            MyMathLib.Vector v = MyMathLib.Vector.RandomVector(n);
            double expect = v.Norm;
            double EPS = 0.0000001d;
            //запустить код
            double actual = Math.Sqrt( v * v );
            //проверить на корректность

            Assert.AreEqual(expect, actual, EPS, "Скалярное произведение должно выраждаться в норму, но нет");
        }

        [TestMethod]
        public void Skalar_1_and_2_return20()
        {
            //настроить входные параметры
            int n = 10;
            MyMathLib.Vector v = MyMathLib.Vector.GetConstVector(1d,n);
            MyMathLib.Vector w = MyMathLib.Vector.GetConstVector(2d, n);

            double expect = 20d;
            double EPS = 0.000000001d;
            //запустить код
            double actual = w*v;
            //проверить на корректность

            Assert.AreEqual(expect, actual, EPS, "Скалярное произведение должно работать, но нет");
        }
    }
}
