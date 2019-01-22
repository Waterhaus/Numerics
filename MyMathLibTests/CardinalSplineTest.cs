using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyMathLib
{
    [TestClass]
    public class CardinalSplineTest
    {
  


        [TestMethod]
        public void MinInterpolationMatrixTest()
        {
            //setup
            int degree = 4;
            int size = 9;
            double h = 1;


            //run
            Spline.InterpolateExperiment.MinInterpolationMatrix(degree, h, size);
        }


        [TestMethod]
        public void CreateInterpolationMatrixTest()
        {
            //setup
            int degree = 4;
            int size = 9;
            double h = 1;
            double[] ksi = Spline.InterpolateExperiment.GetCardinalValue(degree, h);


            //run
            Matrix A = Spline.InterpolateExperiment.CreateInterpolationMatrix(ksi, size);
            Console.WriteLine(A);
        }


        [TestMethod]
        public void MultiplyBSplineMatrixTest()
        {
            //setup
            int degree = 4;
            int size = 9;
            double h = 1;
            double[] ksi = Spline.InterpolateExperiment.GetCardinalValue(degree, h);
            Matrix A = Spline.InterpolateExperiment.CreateInterpolationMatrix(ksi, size);
            Vector one = Vector.GetConstVector(1d, size + degree - 2);
            //run
            double result = (A * one - Spline.InterpolateExperiment.MultiplyBSplineMatrix(one, degree, h, size)).Norm;

            //compare
            Assert.AreEqual(0d, result, 0.000001, "Функции не эквивалентны!");
        }

        [TestMethod]
        public void CreateKSIMatrixTest()
        {
            //setup
            int degree = 2;
            int size = 9;
            double h = 1;



            //run
            Matrix A = Spline.InterpolateExperiment.CreateKSIMatrix(degree, h, size);
            Console.WriteLine(A);
        }


        [TestMethod]
        public void MultiplyKSIMatrixTest()
        {
            //setup
            int degree = 2;
            int size = 9;
            double h = 1;
            Matrix A = Spline.InterpolateExperiment.CreateKSIMatrix(degree, h, size);
            Vector one = Vector.GetConstVector(1, size);

            //run
            double result = (A * one - Spline.InterpolateExperiment.MultiplyKSIMatrix(one, degree, h, size)).Norm;

            //compare
            Assert.AreEqual(0d, result, 0.000001, "Функции не эквивалентны!");
        }

        [TestMethod]
        public void Create_InterpolationMatrix_Test()
        {
            //setup
            int degree = 5;
            int size = 9;
            double h = 1;



            //run
            Matrix A = Spline.InterpolateExperiment.Create_LagrangeInterpolationMatrix(degree, h, size);
            Console.WriteLine(A);
        }

        [TestMethod]
        public void MultiplyLagrangeMatrixTest()
        {
            //setup
            int degree = 4;
            int size = 3000;
            double h = 0.0001d;
            
            Matrix A = Spline.InterpolateExperiment.Create_LagrangeInterpolationMatrix(degree, h, size);
            Vector one = Vector.GetConstVector(1d, 2*size + degree - 2);
            Vector r = Vector.RandomVector(2 * size + degree - 2);
            //run
            Vector anser = A * r - Spline.InterpolateExperiment.Multiply_LagrangeInterpolationMatrix(r, degree, h, size);
            double result = (anser).Norm;
            Console.WriteLine((A*r).Norm);
            //compare
            Console.WriteLine("result = " + result);
            Assert.AreEqual(0d, result, 0.000001d, "Функции не эквивалентны!");
        }

    }
}
