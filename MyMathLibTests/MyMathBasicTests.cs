using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MyMathLibTests
{
    [TestClass]
    public class MyMathBasicTests
    {
     

        [TestMethod]
        public void SumArray_arrayof1and10len_10returned()
        {
            //задать(настроить) параметры

            double EPS = 0.00000001d;
            double[] array = new double[10];
            for(int i = 0; i < array.Length; i++)
            {
                array[i] = 1d;
            }
            List<double> list = new List<double>(array); 
            double expect = 10d;
            
            //запустить код
            double actual_mas = MyMathLib.MyMath.Basic.SumArray(array);
            double actual_list = MyMathLib.MyMath.Basic.SumArray(list);

            //проверить правильность
            Assert.AreEqual(expect, actual_mas, EPS,"Не корректно суммирует обычный массив");
            Assert.AreEqual(expect, actual_list, EPS, "Не корректно суммирует список");
        }

        [ExpectedException(typeof(ArgumentNullException),"Нет выброса исключения про null аргумент")]
        [TestMethod]
        public void SumArray_arraynull_exeptionreturned()
        {
            //задать(настроить) параметры

            double EPS = 0.00000001d;
            double[] array = null;         
            //List<double> list = null;
          

            //запустить код
            MyMathLib.MyMath.Basic.SumArray(array);
            

            //проверить правильность
       }


        [ExpectedException(typeof(ArgumentNullException), "Нет выброса исключения про null аргумент")]
        [TestMethod]
        public void SumArray_listnull_exeptionreturned()
        {
            //задать(настроить) параметры

            double EPS = 0.00000001d;
            
            List<double> list = null;

            //запустить код
            MyMathLib.MyMath.Basic.SumArray(list);

            //проверить правильность
        }


        [TestMethod]
        public void GetStep_10and0and1_return10()
        {
            //задать(настроить) параметры
            double expect = 1d;
            double EPS = 0.00000001d;
            //запустить код
            double h = MyMathLib.MyMath.Basic.GetStep(10, 0, 1);
            double actual = 0 + 9 * h;
            //проверить правильность

            Assert.AreEqual(expect, actual, EPS, "Не корректно находит шаг");
        }

        [ExpectedException(typeof(ArgumentException), "Границы a и b равны")]
        [TestMethod]
        public void GetStep_1and1_returnexeption()
        {
            //задать(настроить) параметры
            
            //запустить код
            MyMathLib.MyMath.Basic.GetStep(10, 1, 1);

            //проверить правильность
        }


    }
}
