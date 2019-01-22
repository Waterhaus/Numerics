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
            Matrix DD = D * Matrix.Transpose(D);
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
            double u0 = 1;
            Matrix D = CardinalDifferentialEquation.FirstDirevetive(N);
            Matrix KSI = CardinalDifferentialEquation.CreateKSIMatrix(degree, h, N + 1);
            Matrix DD = D * Matrix.Transpose(D);
            //KSI[0, 0] -= KSI[0, 1];
            //KSI[N, N] -= KSI[0, 1];
            //DD[0, 0] = DD[N, N] = 2d;
            Console.WriteLine( DD);
            Console.WriteLine(KSI);
            Matrix A = KSI * DD;
            //KSI[0, 0] += KSI[0, 1];
            //KSI[N, N] += KSI[0, 1];
            for (int i = 0; i < N + 1; i++)
            {
                A[i, i] *= 1d;
            }
            A = 2d * A;
            //A[0, 0] = 1.5;
           // A[N, N] = 1.5;
            Console.WriteLine(A);
            Vector y = Vector.GetConstVector(1d, N);
            for (int i = 0; i < N; i++)
            {
                y[i] = i * h;
            }
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


        [TestMethod]
        public void Test2()
        {
            int N = 10;
            int degree = 2;
            double h = 1;
            double a_border = 0, b_border = 1;
            double u0 = 1;
            Matrix D = CardinalDifferentialEquation.FirstDirevetive(N);
            Matrix KSI = CardinalDifferentialEquation.CreateKSIMatrix(degree, h, N + 1);
            Matrix DD = D * Matrix.Transpose(D);
            //KSI[0, 0] -= KSI[0, 1];
            //KSI[N, N] -= KSI[0, 1];
            //DD[0, 0] = DD[N, N] = 2d;
            Console.WriteLine(DD);
            Console.WriteLine(KSI);
            Matrix A = DD;
            //KSI[0, 0] += KSI[0, 1];
            //KSI[N, N] += KSI[0, 1];
            for (int i = 0; i < N + 1; i++)
            {
                A[i, i] *= 1d;
            }
            //A = 2d * A;
            //A[0, 0] = 1.5;
            // A[N, N] = 1.5;
            Console.WriteLine(A);
            Vector y = Vector.GetConstVector(1d, N);
            for (int i = 0; i < N; i++)
            {
                y[i] = i * h;
            }
            Console.WriteLine(D * y);
            Vector Dy = D * y;
            Vector b = new Vector(N + 2);
            for (int i = 0; i < N + 1; i++)
            {
                b[i] =  Dy[i];
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
            Console.WriteLine("B*x = " + B * x);
            Console.WriteLine("b = " + b);
            Vector c = Solver.BCG(B, b, 0.00000001d);
            Console.WriteLine("anser = " + c);
            Console.WriteLine(B * c);
        }

        [TestMethod]
        public void SinTest()
        {
            int N = 50;
            int degree = 2;
            
            double a_border = 0, b_border = 6.29;
            double h = MyMath.Basic.GetStep(N, a_border, b_border);
            double u0 = 0;
            Vector f = MyMath.Basic.GetVectorFunction(N, a_border, b_border, FunctionLib.cos);
            Matrix D = CardinalDifferentialEquation.FirstDirevetive(N);
            //Matrix KSI = CardinalDifferentialEquation.CreateKSIMatrix(degree, h, N + 1);
            Matrix DD = D * Matrix.Transpose(D);
            //KSI[0, 0] -= KSI[0, 1];
            //KSI[N, N] -= KSI[0, 1];
            //DD[0, 0] = DD[N, N] = 2d;
            Console.WriteLine(DD);
            //Console.WriteLine(KSI);
            Matrix A = (1d/h)*DD;
           // Console.WriteLine(A);
            
            Console.WriteLine(D * f);
            Vector Dy = D * f;
            Vector b = new Vector(N + 2);
            for (int i = 0; i < N + 1; i++)
            {
                b[i] = Dy[i];
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

            //Console.WriteLine(B);
            Console.WriteLine("b = " + b);
            Vector c = Solver.BCG(B, b, 0.00000001d);
            Console.WriteLine("anser = " + c);
            Console.WriteLine("Ax = " + B * c);

            Vector d = c.SubVector(0,N+1);
            CardinalSpline spline = new CardinalSpline();
            Vector anser = spline.GetVectorFunction(100, a_border, b_border, d, h, degree + 1);
            Vector expect = MyMath.Basic.GetVectorFunction(100, a_border, b_border, FunctionLib.sin);

            Console.WriteLine("||u - u_spl|| = " + (expect - anser).Norm);
        }


        [TestMethod]
        public void SecondDerevitiveSinTest()
        {
            int N = 50;
            int degree = 2;

            double a_border = 0, b_border = 6.29;
            double h = MyMath.Basic.GetStep(N, a_border, b_border);
            double u0 = FunctionLib.cos(a_border), u1 = FunctionLib.cos(b_border);

            Vector f = MyMath.Basic.GetVectorFunction(N, a_border, b_border, FunctionLib.cos);
            Matrix D = CardinalDifferentialEquation.SecondDirevetive(N);
            Matrix DD = D * Matrix.Transpose(D);
         //   Console.WriteLine(DD);
            Matrix A = (1d / (h*h)) * DD;
            
            Console.WriteLine(D * f);
            Vector Dy = D * f;
            Vector b = new Vector(N + 4);
            for (int i = 0; i < N + 2; i++)
            {
                b[i] = Dy[i];
            }
            b[N + 2] = u0;
            b[N + 3] = u1;
            Console.WriteLine("b = " + b);
            Matrix B = new Matrix(N + 4);
            for (int i = 0; i < N + 2; i++)
            {
                for (int j = 0; j < N + 2; j++)
                {
                    B[i, j] = A[i, j];
                }
            }
            for (int i = 0; i < N + 2; i++)
            {
                B[N + 2, i] = CardinalSpline.Cardinal(degree + 2, a_border, a_border + (i - (degree + 2) + 1) * h, h);
                B[N + 3, i] = CardinalSpline.Cardinal(degree + 2, b_border, a_border + (i - (degree + 2) + 1) * h, h);

                B[i, N + 2] = -B[N + 2, i];
                B[i, N + 3] = -B[N + 3, i];

            }

           // Console.WriteLine(B);
            //Console.WriteLine("b = " + b);
            Vector c = Solver.BCG(B, b, 0.00000001d);
            Console.WriteLine("anser = " + c);
            Console.WriteLine("Ax = " + B * c);

            Vector d = c.SubVector(0, N + 2);

            CardinalSpline spline = new CardinalSpline();

            Vector anser = spline.GetVectorFunction(10, a_border, b_border, d, h, degree + 2);
            Vector expect = MyMath.Basic.GetVectorFunction(10, a_border, b_border, FunctionLib.cos);
            Console.WriteLine(anser); Console.WriteLine(    expect);

            Console.WriteLine("||u - u_spl|| = " + (expect - anser).Norm);
        }
    }
}
