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


            Console.WriteLine(Matrix.transpose( B).ToString());
            Console.WriteLine("index = " + index);



        }

    }
}
