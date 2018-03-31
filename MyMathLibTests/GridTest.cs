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
        public void CreateAbsolutUniformGrid_4_11_0_1()
        {
            //подготовка входных данных
            int GridSize = 11;
            double a = 0d;
            double b = 1d;
            double expect1 = -0.2d;
            double expect2 = 1.2d;
            double EPS = 0.000000001d;
            int deg = 4;
            //выполнение кода

            MyMathLib.Grid grid = new MyMathLib.Grid(deg,GridSize,a,b);
            grid.ToAbsolutUniformSplineGrid();
            Console.WriteLine("new grid" + grid);
            double actual2 = grid.Last;
            double actual1 = grid.First;

            //проверка
            Assert.AreEqual(expect1, actual1, EPS, "Неправильное расширение отрезка");
            Assert.AreEqual(expect2, actual2, EPS, "Неправильное расширение отрезка");
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

        [TestMethod]
        public void BeginEndIndex1()
        {
            //setup
            int deg = 4;
            MyMathLib.Grid grid = new MyMathLib.Grid(deg, 10, 0, 1);

            double left_exp = 0;
            double right_exp = 1d;

            //run
            double left_act = grid[grid.BeginIndex];
            double right_act = grid[grid.EndIndex];

            //cc
            Assert.AreEqual(left_exp, left_act, 0.00000000001d, "left non nono non");

            Assert.AreEqual(right_exp, right_act, 0.00000000001d, "left non nono non");

        }

        [TestMethod]
        public void BeginEndIndex2()
        {
            //setup
            int deg = 3;
            MyMathLib.Grid grid = new MyMathLib.Grid(deg, 10, 0, 1);

           

            //run
            double left = grid[grid.BeginIndex];
            double rleft = grid[grid.BeginIndex + 1];
            bool flag = rleft - left > 0;
            //cc
            Assert.AreEqual(true, flag,"NE TRUE");

           

        }
        [TestMethod]
        public void BeginEndIndex2_2()
        {
            //setup
            int deg = 4;
            MyMathLib.Grid grid = new MyMathLib.Grid(deg, 10, 0, 1);



            //run
            double left = grid[grid.BeginIndex];
            double rleft = grid[grid.BeginIndex + 1];
            bool flag = rleft - left > 0;
            //cc
            Assert.AreEqual(true, flag, "NE TRUE");



        }

        [TestMethod]
        public void BeginEndIndex3()
        {
            //setup
            int deg = 3;
            MyMathLib.Grid grid = new MyMathLib.Grid(deg, 10, 0, 1);



            //run
            double right = grid[grid.EndIndex];
            double lright = grid[grid.EndIndex - 1];
            bool flag = right - lright > 0;
            //cc
            Assert.AreEqual(true, flag, "NE TRUE");



        }
        [TestMethod]
        public void BeginEndIndex3_2()
        {
            //setup
            int deg = 4;
            MyMathLib.Grid grid = new MyMathLib.Grid(deg, 10, 0, 1);



            //run
            double right = grid[grid.EndIndex];
            double lright = grid[grid.EndIndex - 1];
            bool flag = right - lright > 0;
            //cc
            Assert.AreEqual(true, flag, "NE TRUE");



        }

        [TestMethod]
        public void CreateNewBasisSplineGrid_CountMustbe11()
        {
            //enter data
            int deg = 4;
            int GridSize = 7;
            double a = 0;
            double b = 1;

            MyMathLib.Vector knots = MyMathLib.Vector.CreateUniformGrid(GridSize, a,b);
            

            int expect = 11;
            //code
            MyMathLib.Grid tau = new MyMathLib.Grid(deg, knots, a, b);
            int actual = tau.Count;
            Console.WriteLine(tau.ToString());
            Console.WriteLine(tau.ToStringOrigin());
            //compare
            Assert.AreEqual(expect, actual, "Неверный расчет сетки для базисного сплайна");

         
        }

    }
    }
