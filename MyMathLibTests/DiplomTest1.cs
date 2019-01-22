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
    }
}
