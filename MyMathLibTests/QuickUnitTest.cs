using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMathLib;
using MyMathLib.Spline;


namespace MyMathLibTests
{
    [TestClass]
    public class QuickUnitTest
    {
        [TestMethod]
        public void FirstDirevitiveTestMethod()
        {
            Matrix D = CardinalDifferentialEquation.FirstDirevetive(3);
            Matrix KSI = CardinalDifferentialEquation.CreateKSIMatrix(2, 1, 4);
            Matrix DD = D * Matrix.transpose(D);
            Console.WriteLine(D);
            Console.WriteLine(DD);
           // DD[0, 0] = 2d;
            //DD[3, 3] = 2d;
            Console.WriteLine(DD);
            
            //KSI[0, 0] -= KSI[0, 1];
           // KSI[3, 3] -= KSI[0, 1];
            Console.WriteLine(KSI);
            Console.WriteLine("KSI*D = "+KSI * D);
            Console.WriteLine(KSI*DD);

            Matrix A = KSI * DD;
            for (int i = 0; i < 4; i++)
            {
                A[i, i] *= 1d;
            }
            //A[0, 0] = 1.5;
            //A[3, 3] = 1.5;

            Console.WriteLine(A);
            Vector x = Vector.GetConstVector(1, 3);
            Console.WriteLine((KSI * D )* x);
        }

        [TestMethod]
        public void Test()
        {
            int N = 10;
            int degree = 2;
            double h = 1;
            double a_border = 0, b_border = 1;
            double u0 = 2;
            Matrix D = CardinalDifferentialEquation.FirstDirevetive(N);
            Matrix KSI = CardinalDifferentialEquation.CreateKSIMatrix(degree, h, N + 1);
            Matrix DD = D * Matrix.transpose(D);
            KSI[0, 0] += KSI[0, 1];
            KSI[N, N] += KSI[0, 1];
            Console.WriteLine( DD);
            Console.WriteLine(KSI);
            Matrix A = KSI * DD;
            for (int i = 0; i < N + 1; i++)
            {
                A[i, i] *= 1d;
            }
            //A[0, 0] = 1.5;
           // A[N, N] = 1.5;
            Console.WriteLine(A);
            Vector y = Vector.GetConstVector(1d, N);
            //for (int i = 0; i < N; i++)
            //{
            //    y[i] = i * h;
            //}
            Console.WriteLine(D * y);
            Vector Dy = (KSI*D) * y;
            Vector b = new Vector(N + 2);
            for (int i = 0; i < N + 1; i++)
            {
                b[i] = 2d*Dy[i];
            }
            b[N + 1] = u0;
            Console.WriteLine("b = " + b);
            Matrix B = new Matrix(N + 2);
            for (int i = 0; i < N + 1; i++)
            {
                for (int j = 0; j < N + 1; j++)
                {
                    B[i, j] = A[i, j];
                }
            }
            for (int i = 0; i < N; i++)
            {
                B[N + 1, i] = CardinalSpline.Cardinal(degree + 1, a_border, a_border + (i - (degree + 1) + 1) * h, h);
                
                B[i, N + 1] = -B[N + 1, i];
          
            }
           
            Console.WriteLine(B);
            Vector x = Vector.GetConstVector(2d, N + 2);
            x[N + 1] = 0;
            for (int i = 0; i < N + 1; i++)
            {
                x[i] = i + 1;
            }
            Console.WriteLine("x = " + x);
            Console.WriteLine("B*x = " + B*x);
            Console.WriteLine("b = " + b);
            Vector c = Solver.BCG(B, b, 0.00000001d);
            Console.WriteLine("anser = " + c);
            Console.WriteLine(B*c);
        }


    }
}
