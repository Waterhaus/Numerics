﻿using System;
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

        [TestMethod]
        public void CreateBSplineClassicGrid_2_10_0_1_return2()
        {
            //enter data
            int deg = 2;
            int GridSize = 10;
            double a = 0;
            double b = 1;
            int expect = 2;
            //code
            List<double> grid = MyMathLib.Grid.CreateBSplineClassicGrid(deg, GridSize, a, b);
            int i = 0;
            for (i = 0; grid[i] == grid[i + 1]; i++);

            int j = 0;
            for (j = 0; grid[grid.Count - 1 - j] == grid[grid.Count - 2 - j]; j++) ;

            //compare
            Assert.AreEqual(expect, i + 1,"Не достаточно одинаковых элементов в начале");

            Assert.AreEqual(expect, j + 1, "Не достаточно одинаковых элементов в конце");
        }

        [TestMethod]
        public void CreateBSplineClassicGrid_4_10_0_1_return3()
        {
            //enter data
            int deg = 4;
            int GridSize = 10;
            double a = 0;
            double b = 1;
            int expect = 3;
            //code
            List<double> grid = MyMathLib.Grid.CreateBSplineClassicGrid(deg, GridSize, a, b);
            int i = 0;
            for (i = 0; grid[i] == grid[i + 1]; i++) ;

            int j = 0;
            for (j = 0; grid[grid.Count - 1 - j] == grid[grid.Count - 2 - j]; j++) ;

            //compare
            Assert.AreEqual(expect, i + 1, "Не достаточно одинаковых элементов в начале");

            Assert.AreEqual(expect, j + 1, "Не достаточно одинаковых элементов в конце");
        }

    }
    }
