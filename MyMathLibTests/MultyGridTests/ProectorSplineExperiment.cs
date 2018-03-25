using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyMathLib
{
    [TestClass]
    public class ProectorSplineExperiment
    {
        [TestMethod]
        public void Spline4_Experiment()
        {
            //setup
            int N = 32;
            int n = 16;
            double a = 0d;
            double b = 1d;
            int index = 0;
            Vector grid_h = Vector.CreateUniformGrid(N, a, b);
            Vector grid_2h = Vector.CreateUniformGrid(n, a, b);


            BasisSpline spline_h = new BasisSpline(4, N, a, b);
            BasisSpline spline_2h = spline_h.GetBasisSpline_Down();
            Matrix A = spline_h.GetMatrix();
            Vector z;
            Vector c;
            //run
            for (index = 0; index < n; index++)
            {
                z = spline_2h.GetVectorBasis(grid_h, index);
                c = Solver.BCGSTAB(A, z, 0.0000000001d);
                Console.WriteLine(index + ")  " + c.ToString());
            }
            

            //Console.WriteLine(A.ToString());
            Console.WriteLine("index = " +  index);
            
           
            
        }

        [TestMethod]
        public void Spline4_ExperimentUniformGrid()
        {
            //setup
            int N = 17;
            int n = 9;
            double a = 0d;
            double b = 1d;
            int index = 0;
            Vector grid_h = Vector.CreateUniformGrid(N, a, b);
            Vector grid_2h = Vector.CreateUniformGrid(n, a, b);


            BasisSpline spline_h = new BasisSpline(4, grid_h, Vector.GetConstVector(2d, N),GridType.ExperimentSplineGrid);
            BasisSpline spline_2h = new BasisSpline(4, grid_2h, Vector.GetConstVector(2d, n), GridType.UniformSplineGrid);

            Matrix A = spline_h.GetMatrix();
            Vector z;
            Vector c;
            //run
            
            for (index = 0; index < 7; index++)
            {
                z = spline_2h.GetVectorBasis(grid_h, index);
                c = Solver.BCGSTAB(A, z, 0.0000000001d);
                Console.WriteLine(index + ")  " + c.ToString());
            }

            //Console.WriteLine(A.ToString());
            Console.WriteLine("index = " + index);

        }

        [TestMethod]
        public void Spline4_Experiment3()
        {
            //setup
            int N = 17;
            int n = 10;
            double a = 0d;
            double b = 1d;
            int index = 0;
            Vector grid_h = Vector.CreateUniformGrid(N, a, b);
            Vector grid_2h = Vector.CreateUniformGrid(n, a, b);


            BasisSpline spline_h = new BasisSpline(4, grid_h, Vector.GetConstVector(2d, N), GridType.ClassicSplineGrid);
            BasisSpline spline_2h = new BasisSpline(4, grid_2h, Vector.GetConstVector(2d, n), GridType.ClassicSplineGrid);

            Matrix A = spline_h.GetMatrix();
            Vector z;
            Vector c;
            //run
            for (index = 0; index < n; index++)
            {
                z = spline_2h.GetVectorBasis(grid_h, index);
                c = Solver.BCGSTAB(A, z, 0.0000000001d);
                Console.WriteLine(index + ")  " + c.ToString());
            }


            //Console.WriteLine(A.ToString());
            Console.WriteLine("index = " + index);



        }



        [TestMethod]
        public void Spline4_Experiment5()
        {
            Matrix A = new Matrix(3);

            for (int i = 0; i < 3; i++)
            {
                A[i, 0] = 1d;
            }
            Console.WriteLine(A.ToString());
        }

        

        [TestMethod]
        public void Spline4_Experiment4()
        {
            //setup
            int N = 17;
            int n = 10;
            double a = 0d;
            double b = 1d;
            int index = 0;
            Vector grid_h = Vector.CreateUniformGrid(N, a, b);
            Vector grid_2h = Vector.CreateUniformGrid(n, a, b);


            BasisSpline spline_h = new BasisSpline(4, grid_h, Vector.GetConstVector(2d, N), GridType.ClassicSplineGrid);
            BasisSpline spline_2h = new BasisSpline(4, grid_2h, Vector.GetConstVector(2d, n), GridType.ClassicSplineGrid);

            Matrix A = spline_h.GetMatrix();
            Matrix B = new Matrix(N, n);
            Vector z;
            Vector c;
            //run
            for (index = 0; index < n; index++)
            {
                z = spline_2h.GetVectorBasis(grid_h, index);
                c = Solver.BCGSTAB(A, z, 0.0000000001d);
                for (int i = 0; i < N; i++)
                {
                    B[i, index] = c[i];
                }
            }


            Console.WriteLine(Matrix.transpose( B).ToString("0.00"));
            Console.WriteLine("index = " + index);



        }


        [TestMethod]
        public void Spline4_Experiment5_CompareMatr()
        {
            //setup
            int N = 17;
            int n = 10;
            double a = 0d;
            double b = 1d;
            int index = 0;
            Vector grid_h = Vector.CreateUniformGrid(N, a, b);
            Vector grid_2h = Vector.CreateUniformGrid(n, a, b);


            BasisSpline spline_h = new BasisSpline(4,N,a,b);
            BasisSpline spline_2h = spline_h.GetBasisSpline_Down();

            Matrix A = spline_h.GetMatrix();
            Matrix B = new Matrix(N, n);
            Vector z;
            Vector c;
            //run
            for (index = 0; index < n; index++)
            {
                z = spline_2h.GetVectorBasis(grid_h, index);
                Vector zz = spline_2h.GetVectorBasis(grid_2h, index);
                c = Solver.BCGSTAB(A, z, 0.0000000001d);
                for (int i = 0; i < N; i++)
                {
                    B[i, index] = c[i];
                }
                Console.WriteLine((zz).ToString());
            }

            Console.WriteLine(spline_2h.GetMatrix());
            //Console.WriteLine((A*c).ToString());
            Console.WriteLine("index = " + index);



        }

        //метод нахождения матрицы перехода 
        [TestMethod]
        public void FindMatrixTest()
        {
            //setup
            int N = 17;
            int n = 10;
            double a = 0d;
            double b = 1d;
            
            Vector grid_h = Vector.CreateUniformGrid(N, a, b);
            Vector grid_2h = Vector.CreateUniformGrid(n, a, b);


            BasisSpline spline_h = new BasisSpline(4, N, a, b);
            BasisSpline spline_2h = spline_h.GetBasisSpline_Down();

            Matrix B = spline_2h.GetMatrix();
            Matrix BR = new Matrix(n,n);

            for (int i = 1; i < n - 1; i++)
            {
                Vector c = Solver.BCG(B, Vector.GetEn(i, n), 0.00000000001d);
                
                for (int j = 0; j < n; j++)
                {
                    BR[j, i] = c[j];
                }
            }
            BR[0, 0] = 1;
            BR[n - 1, n - 1] = 1;
            Console.WriteLine(B);
            Console.WriteLine(BR);
            Console.WriteLine(BR*B);

            Assert.AreEqual(0, 1);

        }

        [TestMethod]
        public void FindProectorMatrixNew_Test()
        {
            //setup
            int N = 17;
            int n = 10;
            double a = 0d;
            double b = 1d;

            Vector grid_h = Vector.CreateUniformGrid(N, a, b);
            Vector grid_2h = Vector.CreateUniformGrid(n, a, b);


            BasisSpline spline_h = new BasisSpline(4, N, a, b);
            BasisSpline spline_2h = spline_h.GetBasisSpline_Down();

            Matrix B = spline_h.GetMatrix();
            Matrix BR = new Matrix(N, N);

            for (int i = 1; i < N - 1; i++)
            {
                Vector c = Solver.BCG(B, Vector.GetEn(i, N), 0.00000000001d);

                for (int j = 0; j < N; j++)
                {
                    BR[j, i] = c[j];
                }
            }
            BR[0, 0] = 1;
            BR[N - 1, N - 1] = 1;

            Matrix B_2h = new Matrix(N, n);
            for (int i = 0; i < n; i++)
            {
                Vector z = spline_2h.GetVectorBasis(grid_h, i);
                for (int j = 0; j < N; j++)
                {
                    B_2h[j, i] = z[j];
                }
            }

            Console.WriteLine(B_2h);
            Console.WriteLine(BR*B_2h);
            Assert.AreEqual(0, 1);

        }

    }
}
