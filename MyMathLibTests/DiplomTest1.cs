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
    }
}
