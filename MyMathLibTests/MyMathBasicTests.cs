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
    }
}
