using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMathLib;
namespace MyMathLibTests
{
    [TestClass]
    public class DiplomTest1
    {
        [TestMethod]
        public void ConstructMatrixTest_1()
        {

            Matrix A = new Matrix(3);
            Matrix U = new Matrix(2, 3);
            A = Matrix.Identity(3);
            U = U + 3;
            Matrix B = MyMathLib.Spline.CardinalFiniteMethod.CardinalSolver.ConstructMatrix(A, U);
            Console.WriteLine(A);
            Console.WriteLine(U);
            Console.WriteLine(B);
        }

        [TestMethod]
        public void CardinalOperators_DerevetiveMatrix_Test1()
        {
            Matrix D = MyMathLib.Spline.CardinalFiniteMethod.CardinalOperators.DerevetiveMatrix(6, 5);
            Console.WriteLine(D);
        }


        [TestMethod]
        public void CardinalOperators_InterpolateConditionMatrix_Test1()
        {
            Matrix B = MyMathLib.Spline.CardinalFiniteMethod.CardinalOperators.InterpolateConditionMatrix(4,5,new DividedDifferencesStategy());
            Console.WriteLine(B);
        }

        [TestMethod]
        public void CardinalSpline_SplineVector_Test1()
        {
            double a = 0 , b = 1, x = 0.05;

            CardinalSpline spline = new CardinalSpline();

            Vector ksi = spline.SplineVector(x, a, b, 5, 5);
            Console.WriteLine(ksi + " sum = " + MyMath.Basic.SumArray(ksi.ToArray) );


        }

        [TestMethod]
        public void FirstStartOfCardinalSolverSystem()
        {
            int N = 10;
            MyMathLib.Spline.CardinalFiniteMethod.CardinalSolver.SolveSecondDerevetivWhithBounderyCondition(N, 0, 1
                , new DividedDifferencesStategy(), 1, FunctionLib.exp(1), FunctionLib.exp);
            MyMathLib.Spline.CardinalFiniteMethod.CardinalSolver.SolveSecondDerevetivWhithBounderyCondition(2*N, 0, 1
                , new DividedDifferencesStategy(), 1, FunctionLib.exp(1), FunctionLib.exp);
            MyMathLib.Spline.CardinalFiniteMethod.CardinalSolver.SolveSecondDerevetivWhithBounderyCondition(3*N, 0, 1
                , new DividedDifferencesStategy(), 1, FunctionLib.exp(1), FunctionLib.exp);


        }

        [TestMethod]
        public void SecondStartOfCardinalSolverSystem()
        {
            int N = 10;
            MyMathLib.Spline.CardinalFiniteMethod.CardinalSolver.D2_B_bc(N, 0, 1
                , new DividedDifferencesStategy(), 1d/2d, FunctionLib.exp(1)/2d, FunctionLib.exp);
            MyMathLib.Spline.CardinalFiniteMethod.CardinalSolver.D2_B_bc(2 * N, 0, 1
                , new DividedDifferencesStategy(), 1d/2d, FunctionLib.exp(1)/2d, FunctionLib.exp);
            MyMathLib.Spline.CardinalFiniteMethod.CardinalSolver.D2_B_bc(3 * N, 0, 1
                , new DividedDifferencesStategy(), 0.5*1, 0.5 * FunctionLib.exp(1), FunctionLib.exp);


        }

        [TestMethod]
        public void MainStartOfCardinalSolverSystem()
        {
            int N = 25;
            double a = 0;
            double b = 3.14;
            FunctionLib.Function f = (t) => 0.5*Math.Sin(t);

            for (int i = 5; i <= 5; i++)
            {
                MyMathLib.Spline.CardinalFiniteMethod.CardinalSolver.D4_B_bc(31, a, b
                , new DividedDifferencesStategy(),
                0, FunctionLib.sin(b),
                0, -FunctionLib.sin(b),
                f);
                //System.Diagnostics.Debug.WriteLine("===================");
                Console.WriteLine("=========================================");
            }
            
         

        }

        [TestMethod]
        public void SaveMatrixCardinalSolverSystem()
        {
            int N = 100;
            double a = 0;
            double b = N - 1;


            Matrix A = MyMathLib.Spline.CardinalFiniteMethod.CardinalSolver.D4_B_Matrix(N, a, b, new DividedDifferencesStategy());
            A.Save("D4_B_Test1.txt");
            A = MyMathLib.Spline.CardinalFiniteMethod.CardinalSolver.D1_Matrix(N, a, b, new DividedDifferencesStategy());
            A.Save("D1_Test1.txt");


        }
        //==========================================================================


        [TestMethod]
        public void pr_coef_test1()
        {
            for (int i = 2; i < 10; i++)
            {

                Vector c = new Vector(CardinalSpline.pr_coef(i));
                Console.WriteLine(c);

            }
        }



    }
}
