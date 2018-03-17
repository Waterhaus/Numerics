using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyMathLib
{
    [TestClass]
    public class PRTest
    {
        [TestMethod]
        public void basic_p_basic_r_dimention_test()
        {
            //setup
            Vector vec_old = Vector.GetConstVector(1d, 16);
            Console.WriteLine("old = " + vec_old.ToString());
            //run
            Vector vec_new = MultyGrid.Resumption.basic_p( MultyGrid.Proektor.basic_r(vec_old));
            Console.WriteLine("new = " + vec_new.ToString());
            //compare
            Assert.AreEqual(vec_old.Length, vec_new.Length, "Размерности должны совпадать");

        }

        [TestMethod]
        public void create_proection_spline4_mat_test()
        {
            //setup
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
            Matrix B = new Matrix(n, N);
            Vector z;
            Vector c;
            //run
            for (index = 0; index < n; index++)
            {
                z = spline_2h.GetVectorBasis(grid_h, index);
                c = Solver.BCGSTAB(A, z, 0.0000000001d);
                for (int i = 0; i < N; i++)
                {
                    B[index, i] = c[i];
                }
            }



            //run
            Matrix CREATE_B = MultyGrid.Proektor.create_proection_spline4_mat(n);

            Matrix D = B - CREATE_B;
            double actual = D.NORMF;
            Console.WriteLine("полученная " + B.ToString());
            Console.WriteLine("созданная "+ CREATE_B.ToString());
            //compare
            Assert.AreEqual(0d, actual, 0.00000001d, "Эти матрицы не равны!");

        }

        [TestMethod]
        public void basic_r_norm_safe_test()
        {
            //setup
            Vector vec_old = Vector.GetConstVector(1d, 64);
            double h_old = 1d / 64d;
            double h_new = 1d / 4d;
            Console.WriteLine("old = " + vec_old.ToString());
            //run
            Vector vec_new = new Vector(64);
            Vector.copy(ref vec_new, vec_old);
            for (int i = 0; i < 4; i++)
            {
                vec_new = MultyGrid.Proektor.basic_r(vec_new);
            }
            
            Console.WriteLine("new = " + vec_new.ToString());
            //compare
            Assert.AreEqual(Math.Sqrt(h_old)*vec_old.Norm, Math.Sqrt(h_new) * vec_new.Norm,0.0000001d, "Вторая норма не должна меняться");

        }
    }
}
