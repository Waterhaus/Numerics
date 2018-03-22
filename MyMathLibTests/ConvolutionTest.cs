using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyMathLib
{
    [TestClass]
    public class ConvolutionTest
    {
        [TestMethod]
        public void CompareConvolutionWhitSetFunctionTest()
        {
            //setup
            int GridSize = 1000;
            double a = -1.5d;
            double b = 1.5d;
            Vector grid = Vector.CreateUniformGrid(GridSize, a, b);
            
            Vector expect = MyMath.Basic.GetVectorFunction(GridSize, a, b, FunctionLib.conv_tri_test);
            //run
            Vector actual = MyMath.Operations.GetConvolutionVector(FunctionLib.step_01, FunctionLib.step_01, GridSize, a, b);
            
            //compare
            double NORM = (expect - actual).Norm;
            Console.WriteLine("Норма разности = " + NORM );
            Assert.AreEqual(0d, NORM, 0.1d, "Не правильно прводит конволюцию");
        }

        [TestMethod]
        public void CompareConvolutionInZeroPointTest()
        {
            //setup
            int GridSize = 600;
            double a = -2d;
            double b = 2d;
            double expect = 1d;
            Grid grid = new Grid(GridSize, a, b);
            //run
            double actual = MyMath.Operations.Convolution(0d, FunctionLib.step_01, FunctionLib.step_01,grid);
           

            //compare
            
            Assert.AreEqual(expect, actual, 0.005d, "Не правильно прводит конволюцию. Должна быть 1 в точке 0");
        }
    }
}
