using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMathLib.Spline
{
    public class InterpolateExperiment
    {

        public static double[] GetCardinalValue(int degree, double h)
        {
            double[] mas = new double[degree - 1];
            for (int i = 0; i < degree - 1; i++)
            {
                mas[i] = CardinalSpline.Cardinal(degree, (i + 1) * h, 0, h);

            }
            return mas;
        }


        public static double[] GetCardinalValue(int degree)
        {
            double[] mas = new double[degree - 1];
            double h = 1;
            for (int i = 0; i < degree - 1; i++)
            {
                mas[i] = CardinalSpline.Cardinal(degree, (i + 1) * h, 0, h);

            }
            return mas;
        }


        public static Vector GetCoef_2h(int degree)
        {
            double[] ksi = GetCardinalValue(degree, 1d);
            Matrix A = new Matrix(degree + 1);
            Vector y = new Vector(degree + 1);

            for (int i = 0; i < y.Length; i++)
            {
                y[i] = CardinalSpline.Cardinal(degree, i + 1, 0d, 2d);
            }

            for (int i = 0; i < degree + 1; i++)
            {
                for (int j = 0; j < ksi.Length; j++)
                {
                    if (i - j >= 0)
                        A[i, i - j] = ksi[ksi.Length - 1 - j];
                }
            }
            Matrix A_t = Matrix.Transpose(A);
            Console.WriteLine("y = " + y);
            Console.WriteLine("A_T*A = " + A);

            Vector c = new Vector();
            c = Solver.BCGSTAB(A, y, 0.0000001d);
            Console.WriteLine("SUM c  = " + MyMath.Basic.SumArray(c.ToArray));
            return c;
        }

        public static Matrix CreatePeriodicInterpolationMatrix(double[] ksi, int size)
        {
            int degree = ksi.Length + 1;
            Matrix A = new Matrix(size);


            int N = size - 1;
            int p = degree - 1;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < ksi.Length; j++)
                {
                    if (i - j >= 0)
                        if (!(i > N - p + 1 && (i - j) > N - p + 1))
                            A[i, i - j] = ksi[j];
                }
            }

            N = A.Length.n;
            for (int j = 1; j < ksi.Length; j++)
            {
                for (int i = 0; i + j < ksi.Length; i++)
                {
                    A[j - 1, N - degree + 1 + i + j] = ksi[ksi.Length - 1 - i];
                    A[N - degree + 1 + i + j, j - 1] = ksi[ksi.Length - 1 - i];
                }
            }



            return A;

        }


        public static Matrix CreateInterpolationMatrix(double[] ksi, int size)
        {
            int N = size;
            int p = ksi.Length + 1;
            Matrix A = new Matrix(N, N + p - 2);

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < ksi.Length; j++)
                {
                    A[i, i + j] = ksi[j];
                }
            }
            return A;

        }

        public static Vector MultiplyBSplineMatrix(Vector x, int degree, double h, int size)
        {
            int N = size;
            int p = degree;
            double[] ksi = GetCardinalValue(degree, h);


            Vector y = new Vector(N);
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < ksi.Length; j++)
                {
                    //A[i, i + j] = ksi[j];
                    y[i] = y[i] + x[i + j] * ksi[j];
                }
            }
            return y;

        }

        public static Matrix CreateKSIMatrix(int degree, double h, int size)
        {
            int N = size;
            int p = degree;
            double[] ksi = GetCardinalValue(2 * degree, h);
            Console.WriteLine("deg = " + 2 * degree + " ksi = " + new Vector(ksi).ToString("0.000000"));
            ksi[degree - 1] = 2 * ksi[degree - 1];
            Matrix A = new Matrix(N + p - 2, N + p - 2);

            for (int i = 0; i < N + p - 2; i++)
            {
                for (int j = 0; j < ksi.Length; j++)
                {
                    if (i + j - p + 1 >= 0 && i + j - p + 1 < N + p - 2)
                        A[i, i + j - p + 1] = ksi[j];
                }
            }
            return A;

        }



        public static Vector MultiplyKSIMatrix(Vector x, int degree, double h, int size)
        {
            int N = size;
            int p = degree;
            double[] ksi = GetCardinalValue(2 * degree, h);
            Console.WriteLine("deg = " + 2 * degree + " ksi = " + new Vector(ksi).ToString("0.000000"));
            ksi[degree - 1] = 2 * ksi[degree - 1];

            Vector y = new Vector(N + p - 2);

            for (int i = 0; i < N + p - 2; i++)
            {
                for (int j = 0; j < ksi.Length; j++)
                {
                    if (i + j - p + 1 >= 0 && i + j - p + 1 < N + p - 2)
                    {
                        //A[i, i + j - p + 1] = ksi[j];
                        y[i] = y[i] + ksi[j] * x[i + j - p + 1];
                    }

                }
            }
            return y;

        }


        public static Matrix Create_LagrangeInterpolationMatrix(int degree, double h, int size)
        {
            double[] ksi_p = GetCardinalValue(degree, h);
            Matrix B = CreateInterpolationMatrix(ksi_p, size);
            Matrix KSI = CreateKSIMatrix(degree, h, size);

            int N = size;
            int p = degree;

            Matrix S = new Matrix(2 * N + p - 2);

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N + p - 2; j++)
                {
                    S[i, j] = B[i, j];
                    S[N + j, N + p - 2 + i] = -B[i, j];
                }
            }

            for (int i = 0; i < N + p - 2; i++)
            {
                for (int j = 0; j < N + p - 2; j++)
                {
                    S[N + i, j] = h * KSI[i, j];
                }
            }

            return S;

        }

        public static Vector CalculateSkal(Vector f, int degree, double h)
        {
            int N = f.Length;
            int p = degree;
            double[] ksi = GetCardinalValue(degree - 1);
            double[] mas = new double[ksi.Length + 1];

            for (int i = 0; i < ksi.Length; i++)
            {
                mas[i] = ksi[i];
            }
            for (int i = 0; i < ksi.Length; i++)
            {
                mas[i + 1] = mas[i + 1] - ksi[i];
            }

            Console.WriteLine("ksi.len = " + ksi.Length);
            Vector I = new Vector(N + p - 2);


            if (degree == 2)
            {
                for (int i = 0; i < I.Length; i++)
                {
                    for (int j = 0; j < 1; j++)
                    {

                        // Console.WriteLine(" i - j = " + (i - j) + " ksi.Length - 1 - j = " + (ksi.Length - 1 - j));
                        if (i - j >= 0 && i - j + 1 < f.Length)
                        {
                            I[i] += 2.0 * (f[i - j] - f[i - j + 1]) * h * h * h;

                        }


                    }


                }
            }
            else
            {
                for (int i = 0; i < I.Length; i++)
                {
                    for (int j = 0; j < mas.Length; j++)
                    {
                        if (i - j >= 0 && i - j < f.Length)
                            I[i] += 2.0 * f[i - j] * mas[j] * h * h * h;

                    }


                }
            }


            //Console.WriteLine("skal = " + I);
            return I;
        }

        public static Vector Interpolate_By_CardinalSpline(Vector y_knots, int degree, double h)
        {
            int N = y_knots.Length;
            int p = degree;
            Vector b = new Vector(2 * N + p - 2);
            for (int i = 0; i < N; i++)
            {
                b[i] = y_knots[i];
            }
            Vector I = CalculateSkal(y_knots, degree, h);

            for (int i = N; i < b.Length; i++)
            {

                b[i] = I[i - N];

            }

            Matrix A = Create_LagrangeInterpolationMatrix(degree, h, N);
            // 
            double EPS = 0.0000001d;
            Vector coefs = Solver.BCGSTAB((Matrix.Transpose(A)) * A, (Matrix.Transpose(A)) * b, EPS);

            //Console.WriteLine("b = " + b);
            //Console.WriteLine("A*c - b" + (A*coefs- b));

            Vector c = new Vector(N + p - 2);
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = coefs[i];
            }
            return c;
        }




        public static Vector Multiply_LagrangeInterpolationMatrix(Vector x, int degree, double h, int size)
        {
            int N = size;
            int p = degree;
            double[] ksi = GetCardinalValue(degree, h);


            Vector y = new Vector(2 * N + p - 2);

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < ksi.Length; j++)
                {
                    //A[i, i + j] = ksi[j];
                    y[i] = y[i] + x[i + j] * ksi[j];

                    y[N + i + j] = y[N + i + j] + x[N + p - 2 + i] * (-ksi[j]);
                }
            }

            //---------------
            double[] ksi_2p = GetCardinalValue(2 * degree, h);
            // Console.WriteLine("deg = " + 2 * degree + " ksi = " + new Vector(ksi).ToString("0.000000"));
            ksi_2p[degree - 1] = 2 * ksi_2p[degree - 1];



            for (int i = 0; i < N + p - 2; i++)
            {
                for (int j = 0; j < ksi_2p.Length; j++)
                {
                    if (i + j - p + 1 >= 0 && i + j - p + 1 < N + p - 2)
                    {
                        //A[i, i + j - p + 1] = ksi[j];
                        y[N + i] = y[N + i] + ksi_2p[j] * x[i + j - p + 1];


                    }

                }
            }


            return y;

        }

        public static Matrix MinInterpolationMatrix(int degree, double h, int size)
        {
            double[] ksi_p = GetCardinalValue(degree, h);
            double[] mas = GetCardinalValue(2 * degree, h);
            double[] ksi_2p = new double[ksi_p.Length + 1];

            int p = degree - 1;
            for (int i = 0; i < ksi_2p.Length; i++)
            {
                ksi_2p[i] = mas[p + i];
            }
            ksi_2p[0] = 2 * ksi_2p[0];
            Console.WriteLine("all vec " + new Vector(mas).ToString("0.000000"));
            Console.WriteLine("half vec " + new Vector(ksi_2p).ToString("0.000000"));

            Matrix A0 = CreatePeriodicInterpolationMatrix(ksi_p, size);
            //Console.WriteLine( A0);
            Matrix KSI = CreatePeriodicInterpolationMatrix(ksi_2p, size);
            //Console.WriteLine(KSI);


            Matrix S = new Matrix(2 * size);

            int N = size - 1;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    S[i, j] = A0[i, j];
                    S[i + size, j + size] = -A0[i, j];
                    S[i + size, j] = KSI[i, j];
                }
            }
            //Console.WriteLine(S);
            return S;
        }


        public static Vector Interpolate(Vector y_knots, int degree, double h)
        {

            Vector b = new Vector(2 * y_knots.Length);
            for (int i = 0; i < y_knots.Length; i++)
            {
                b[i] = y_knots[i];
            }
            Matrix A = MinInterpolationMatrix(degree, h, y_knots.Length);
            //Console.WriteLine(A);
            double EPS = 0.000001d;
            Vector coefs = Solver.BCGSTAB(A, b, EPS);

            return coefs;
        }




        public static Vector InterpolateWith_BCGSTAB(Vector b, int degree, double h, int size, double EPS)
        {
            Vector r = new Vector(b.Length);
            Vector rr = new Vector(b.Length);
            Vector p = new Vector(b.Length);

            Vector x = Multiply_LagrangeInterpolationMatrix(b, degree, h, size);

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
                    //v = A * p;
                    v = Multiply_LagrangeInterpolationMatrix(p, degree, h, size);
                    /*5*/
                    alpha = rho / MyMath.Basic.skal(rr, v);

                    /*6*/
                    s = r - alpha * v;
                    /*7*/
                    //t = A * s;
                    t = Multiply_LagrangeInterpolationMatrix(s, degree, h, size);
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

        public static Vector Quick_Interpolate(Vector y_knots, int degree, double h)
        {
            int N = y_knots.Length;
            int p = degree;
            Vector b = new Vector(2 * N + p - 2);
            for (int i = 0; i < y_knots.Length; i++)
            {
                b[i] = y_knots[i];
            }

            double EPS = 0.000000001d;
            Vector coefs = InterpolateWith_BCGSTAB(b, degree, h, N, EPS);
            Console.WriteLine("b = " + b);
            Console.WriteLine("A*c - b" + (Multiply_LagrangeInterpolationMatrix(coefs, degree, h, N) - b));

            Vector c = new Vector(N + p - 2);
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = coefs[i];
            }

            return c;
        }



        public static void MIN_Interpolation_Test(FunctionLib.Function func, ref Vector x, ref Vector expect, ref Vector actual, int degree, int Size, double a_border, double b_border)
        {
            //setup

            double a = a_border;
            double b = b_border;
            int GridSize = Size;
            int deg = degree;
            Vector grid = Vector.CreateUniformGrid(GridSize, a, b);
            double h = MyMath.Basic.GetStep(GridSize, a, b);
            Vector y = MyMath.Basic.GetVectorFunction(GridSize, a, b, func);
            Grid tau = new Grid(deg, grid, grid[0], grid.Last, true);
            tau.ToPeriodiclineGrid();
            //run


            Vector min_c = Spline.InterpolateExperiment.Interpolate_By_CardinalSpline(y, deg, h);
            // Console.WriteLine("c = " + c);
            Console.WriteLine("min_c = " + min_c);
            Console.WriteLine("Степень сплайна = " + deg);
            //compare
            int N = 10 * GridSize;
            expect = MyMath.Basic.GetVectorFunction(N - 1, a, b, func);
            CardinalSpline spline = new CardinalSpline();
            actual = spline.GetVectorFunction(N - 1, a, b, min_c, h, deg);
            x = Vector.CreateUniformGrid(N - 1, a, b);
            Vector bf = spline.GetVectorFunction(GridSize, a, b, min_c, h, deg);


            double result = (expect - actual).Norm;
            double interpolation = (y - bf).Norm;


            Console.WriteLine("значение f(x) = " + y.ToString());
            Console.WriteLine("значение bf(x) = " + bf.ToString());
            Console.WriteLine("||c|| = " + min_c.Norm.ToString("0.000000"));
            Console.WriteLine("||f - spline|| = " + result.ToString("0.000000"));

        }




        public static Vector Cardinal_Interpolation_Complete_Test(FunctionLib.Function func, int degree, int Size, double a_border, double b_border)
        {
            //setup

            double a = a_border;
            double b = b_border;
            int GridSize = Size;
            int deg = degree;
            Vector grid = Vector.CreateUniformGrid(GridSize, a, b);
            double h = MyMath.Basic.GetStep(GridSize, a, b);
            Vector y = MyMath.Basic.GetVectorFunction(GridSize, a, b, func);
            Grid tau = new Grid(deg, grid, grid[0], grid.Last, true);
            tau.ToPeriodiclineGrid();
            //run


            Vector min_c = Spline.InterpolateExperiment.Interpolate_By_CardinalSpline(y, deg, h);
            // Console.WriteLine("c = " + c);
            Console.WriteLine("min_c = " + min_c);
            Console.WriteLine("Степень сплайна = " + deg);
            //compare
            int N = 10 * GridSize;
            Vector expect = MyMath.Basic.GetVectorFunction(N - 1, a, b, func);
            CardinalSpline spline = new CardinalSpline();
            Vector actual = spline.GetVectorFunction(N - 1, a, b, min_c, h, deg);

            Vector bf = spline.GetVectorFunction(GridSize, a, b, min_c, h, deg);


            double result = (expect - actual).Norm;
            double interpolation = (y - bf).Norm;


            Console.WriteLine("значение f(x) = " + y.ToString());
            Console.WriteLine("значение bf(x) = " + bf.ToString());
            Console.WriteLine("||c|| = " + min_c.Norm.ToString("0.000000"));
            Console.WriteLine("||f - spline|| = " + result.ToString("0.000000"));
            return min_c;
        }
    }
}
