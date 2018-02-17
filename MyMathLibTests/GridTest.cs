using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MyMathLibTests
{
    [TestClass]
    public class GridTest
    {
        [TestMethod]
        public void CreateUniformGrid_10_0_2_return2()
        {
            //подготовка входных данных
            int GridSize = 10;
            double a = 0d;
            double b = 2d;
            double expect = 2d;
            double EPS = 0.000000001d;

            //выполнение кода
            List<double> grid = MyMathLib.Grid.CreateUniformGrid(GridSize, a, b);
            double actual = grid[grid.Count - 1];

            //проверка
            Assert.AreEqual(expect, actual, EPS, "Должна была получиться длина отрезка");
        }

        [TestMethod]
        public void CreateUniformGrid_10_0_2_returnSUM()
        {
            //подготовка входных данных
            int GridSize = 10;
            double a = 0d;
            double b = 2d;
            double expect = a*(GridSize) + ((b-a)/ 2d)*GridSize;
            double EPS = 0.000000001d;

            //выполнение кода
            List<double> grid = MyMathLib.Grid.CreateUniformGrid(GridSize, a, b);
            double actual = MyMathLib.MyMath.Basic.SumArray(grid);

            //проверка
            Assert.AreEqual(expect, actual, EPS, "Сумма должна совпадать со значением формулы");
        }


        [TestMethod]
        public void CreateUniformGrid_25_3_13_returnSUM()
        {
            //подготовка входных данных
            int GridSize = 10;
            double a = 3d;
            double b = 13d;
            double expect = a * (GridSize ) + ((b - a) / 2d) * GridSize;
            double EPS = 0.000000001d;

            //выполнение кода
            List<double> grid = MyMathLib.Grid.CreateUniformGrid(GridSize, a, b);
            double actual = MyMathLib.MyMath.Basic.SumArray(grid);

            //проверка
            Assert.AreEqual(expect, actual, EPS, "Сумма должна совпадать со значением формулы");
        }


        [TestMethod]
        public void CreateUniformGrid_10_return10()
        {
            //входные данные
            int GridSize = 10;
            double a = 0;
            double b = 1;
            int expect = 10;

            //code
            int actual = MyMathLib.Grid.CreateUniformGrid(GridSize, a, b).Count;

            //compare
            Assert.AreEqual(expect, actual);

        }



    }
    }
