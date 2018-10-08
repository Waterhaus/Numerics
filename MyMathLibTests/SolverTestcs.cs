using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMathLib;
namespace MyMathLibTests
{
    [TestClass]
    public class SolverTestcs
    {
        [TestMethod]
        public void TridiagonalMatrixAlgorithmTest()
        {
            //setup
            int n = 200;
            Matrix A = new Matrix(n);
            Vector x = Vector.RandomVector(n);
            Vector c = Vector.GetConstVector(3d, n);
            Vector a = Vector.GetConstVector(1d, n);
            a[0] = 0;
            Vector b = Vector.GetConstVector(1d, n);
            b[n - 1] = 0;
            for (int i = 0; i < n; i++)
            {
                A[i, i] = 3d;
                if (i - 1 >= 0) A[i, i - 1] = 1d;
                if (i + 1 < n) A[i, i + 1] = 1d;
            }

            Vector f = A * x;
           // Console.WriteLine(A);
            //run

            Vector anser = Solver.TridiagonalMatrixAlgorithm(a, c, b, f);
            //Console.WriteLine((anser - x).ToString("0.0000000"));
            //compare
            Assert.AreEqual(0d, (anser - x).Norm, 0.00001d);
        }
    }
}
