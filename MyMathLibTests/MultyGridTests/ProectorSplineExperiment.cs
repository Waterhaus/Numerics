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
            

            BasisSpline spline_h = new BasisSpline(4, grid_h, Vector.GetConstVector(2d, N));
            BasisSpline spline_2h = new BasisSpline(4, grid_2h, Vector.GetConstVector(2d, n));

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
    }
}
