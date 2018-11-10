using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;


namespace MyMathLib.Spline
{
    public class CardinalInterpolation
    {
        //public static Vector StartCardinalInterpolation(Vector y_knots, int degree, double h)
        //{
        //    int N = y_knots.Length;
        //    int p = degree;
        //    Vector b = new Vector(2 * N + p - 2);
        //    for (int i = 0; i < N; i++)
        //    {
        //        b[i] = y_knots[i];
        //    }
        //    Vector I = CalculateIntegral(y_knots, degree, h);

        //    for (int i = N; i < b.Length; i++)
        //    {

        //        b[i] = I[i - N];

        //    }

        //    Matrix A = Create_LagrangeInterpolationMatrix(degree, h, N);
        //    // 
        //    double EPS = 0.0000001d;
        //    Vector coefs = Solver.BCGSTAB((Matrix.transpose(A)) * A, (Matrix.transpose(A)) * b, EPS);

        //    //Console.WriteLine("b = " + b);
        //    //Console.WriteLine("A*c - b" + (A*coefs- b));

        //    Vector c = new Vector(N + p - 2);
        //    for (int i = 0; i < c.Length; i++)
        //    {
        //        c[i] = coefs[i];
        //    }
        //    return c;
        //}

    }
}
