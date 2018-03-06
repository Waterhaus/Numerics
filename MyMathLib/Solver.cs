using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMathLib
{
    //no test
    public class Solver
    {
        public static Vector BCG(Matrix A, Vector b, double EPS)
        {
            Vector x = new Vector(b.Length);
            Vector xx = new Vector(b.Length);
            Vector r = new Vector(b.Length);
            Vector rr = new Vector(b.Length);
            Vector r1 = new Vector(b.Length);
            Vector rr1 = new Vector(b.Length);

            Vector p = new Vector(b.Length);
            Vector pp = new Vector(b.Length);

            for (int i = 0; i < b.Length; i++)
            {
                r[i] = b[i];
                rr[i] = r[i];
                x[i] = 0;
                p[i] = r[i];
                pp[i] = rr[i];
            }


            Vector h = new Vector(b.Length);

            double alpha = 0;
            double betta = 0;

            Matrix TA = new Matrix(A.Length.n);
            Matrix.copy(ref TA, Matrix.transpose(A));

            if (MyMath.Basic.skal(r, rr) != 0)
                for (int j = 0; ; j++)
                {

                    //1
                    Vector Ap = A * p;
                    Vector Atp = TA * pp;

                    alpha = MyMath.Basic.skal(r, rr) / MyMath.Basic.skal(Ap, pp);

                    //2
                    Vector.copy(ref h, p);
                    x = x + alpha * h;

                    //3

                    r1 = r - alpha * Ap;
                    rr1 = rr - alpha * Atp;


                    betta = MyMath.Basic.skal(r1, rr1) / MyMath.Basic.skal(r, rr);

                    //5-6
                  
                    if (r1.Norm <= EPS) { Console.WriteLine("BCG: Решение достигнуто"); break; }
                    if (betta == 0) { Console.WriteLine("BCG: При данном rr0 метод не сходится "); break; }

                    //7
                    Vector.copy(ref h, p);
                    p = r1 + betta * h;
                    Vector.copy(ref h, pp);
                    pp = rr1 + betta * h;
                    r = r1;
                    rr = rr1;

                }
            else Console.WriteLine("BCG: (r0, rr0)=0 - Выберите другое приближение");
            return x;

        }

        public static Vector BCGSTAB(Matrix A, Vector b, double EPS)
        {
            Vector r = new Vector(b.Length);
            Vector rr = new Vector(b.Length);
            Vector p = new Vector(b.Length);

            Vector x = new Vector(b.Length);

            Vector v = new Vector(b.Length);
            Vector s = new Vector(b.Length);
            Vector t = new Vector(b.Length);
            Vector temp = new Vector(b.Length);

            for (int i = 0; i < b.Length; i++)
            {
                r[i] = b[i];
                rr[i] = r[i];
                x[i] = 0;
                p[i] = 0;
                v[i] = 0;

            }

            double rho = 1;
            double rho_old = 1;
            double betta = 0;
            double alpha = 1;
            double omega = 1;

            rho = MyMath.Basic.skal(r, rr);
            rho_old = 1;

            if (Math.Abs(rho) > 0)
                for (int j = 0; ; j++)
                {
                    //Console.WriteLine(x.ToString());
                    /*1*/
                    rho = MyMath.Basic.skal(rr, r);
                    if (rho == 0) { Console.WriteLine("При данном rr0 метод не сходится "); break; }
                    //MessageBox.Show(rho.ToString("0.0") + " and " + rho_old.ToString("0.0"));

                    /*2*/
                    betta = (rho / rho_old) * (alpha / omega);
                    /*3*/
                    temp = p - omega * v;
                    p = r + betta * temp;
                    /*4*/
                    v = A * p;
                    /*5*/
                    alpha = rho / MyMath.Basic.skal(rr, v);

                    /*6*/
                    s = r - alpha * v;
                    /*7*/
                    t = A * s;
                    /*8*/
                    double ts = MyMath.Basic.skal(t, s);
                    double tt = MyMath.Basic.skal(t, t);
                    omega = ts / tt;
                    /*9*/

                    x = x + alpha * p;

                    x = x + omega * s;
                    /*10*/

                    r = s - omega * t;
                    rho_old = rho;
                    //MessageBox.Show("MyMath.Basic.skal(t, s) = " + MyMath.Basic.skal(t, s).ToString());
                    //
                    if (s.Norm < EPS) { Console.WriteLine("BCGSTAB: решение достигнуто"); return x; }

                    if (Math.Abs(omega) < EPS) { Console.WriteLine("omega == 0"); return x; }
                    //if (r.Norm2SQ <= EPS) {  MessageBox.Show("BCG: Решение достигнуто"); break; }

                }
            return x;
        }
    }
}
