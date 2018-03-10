using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMathLib.MultyGrid
{
    class MultyGridMethod
    {
        public class MGM_in
        {
            public Vector b;
            public int nu;
            public int gamma;
            public double h;
            public int N;
            public double EPS = 0.001d;
            public int level;
            public Vector z;



            public MGM_in(Vector f, int NU, double step)
            {
                N = f.Length;
                Vector.copy(ref b, f);
                nu = NU;
                h = step;
            }

            public MGM_in(int Level, Vector z_level, Vector d, int NU, double step, int Gamma)
            {
                N = z_level.Length;
                Vector.copy(ref b, d);
                Vector.copy(ref z, z_level);
                nu = NU;
                h = step;
                gamma = Gamma;
                level = Level;
            }

        }



        public static Vector MGMFull(MGM_in setup)
        {
            Vector r = Vector.GetConstVector(1d, setup.N);
            Vector z = new Vector();
            Vector.copy(ref setup.z, setup.b);


            while (r.Norm > 0.001d)
            {
                //MessageBox.Show("||r|| = " + r.Norm2SQ.ToString("0.0000000"));
                z = BasicMGM(setup);
                r = DifEquation.multiplyD2U(z) - setup.b;

                Vector.copy(ref setup.z, z);

            }
            return z;
        }

        public static Vector BasicMGM(MGM_in setup)
        {
            double h = setup.h;
            // Vector g = ((h * h) / 4d) * setup.b;
            Vector z = setup.z;
            double W = (h * h) / 4d;
            Vector d_temp;// = DifferenceEquations.multiplyD2U(z) - setup.b;


            if (setup.level == 0)
                return setup.z; //точное решение на грубой сетке
            else
            {

                //ню базовых итераций
                for (int k = 0; k < setup.nu; k++)
                {
                    d_temp = DifEquation.multiplyD2U(z) - setup.b;
                    z = z - W * d_temp;

                }

                //коррекция
                Vector d_2h = Proektor.basic_r(DifEquation.multiplyD2U(z) - setup.b);
                Vector e_2h = new Vector(d_2h.Length);
                //Решакем Ае = d
                for (int i = 0; i < setup.gamma; i++)
                {
                    MGM_in set = new MGM_in(setup.level - 1, e_2h, d_2h, setup.nu, 2d * h, setup.gamma);
                    e_2h = BasicMGM(set);
                }

                //z = z - pe
                z = z - Resumption.basic_p(e_2h);

                //постсглаживание

                for (int k = 0; k < setup.nu; k++)
                {
                    d_temp = DifEquation.multiplyD2U(z) - setup.b;
                    z = z - W * d_temp;
                }
                return z;
            }
        }
        //-------------------------------------------------------------------------------------------

        public static Vector BasicTwoGM(MGM_in setup)
        {
            double h = setup.h;
            double W = ((h * h) / 4d);
            Vector z = new Vector(setup.N);
            Vector e_h;
            Vector d_temp;
            do
            {
                for (int k = 0; k < setup.nu; k++)
                {
                    d_temp = DifEquation.multiplyD2U(z) - setup.b;
                    z = z - W * d_temp;
                }

                //проектирование ошибки
                Vector d_h = DifEquation.multiplyD2U(z) - setup.b;
                Vector d_2h = Proektor.basic_r(d_h);

                //решаем систему на грубой сетке прогонкой
                Vector e_2h = DifEquation.solveD2U(d_2h);

                //продолжаем на исходную сетку 
                e_h = Resumption.basic_p(e_2h);

                z = z - e_h;

                /*
                for (int k = 0; k < setup.nu; k++)
                {
                    z = DifferenceEquations.multiplyD2U_Jacobi(z, h) + g;
                }*/
                //r = DifferenceEquations.multiplyD2U(z) - setup.b;
                //MessageBox.Show(e_h.Norm2SQ.ToString("0.0000000"));
            } while (e_h.Norm > setup.EPS);


            return z;
        }

    }
}
