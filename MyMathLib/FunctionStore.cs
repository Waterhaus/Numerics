﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMathLib
{
    public class FunctionLib
    {
        public delegate double Function(double t);
        public delegate object UniFunction(object t);
        public delegate double MyFunctionDoubleDelegate(params object[] args);

        public static double polinom_deg3_1(double t)
        {
            return t * (t - 4d) * (t - 4d);
        }

        public static double line_1(double t)
        {
            return 16d - 6d * t;
        }

        public static double sin_3pit(double t)
        {
            double c = Math.PI * 3d;
            return Math.Sin(c * t);
        }

        

        public static double constant_1(double t)
        {
            return 1d;

        }

        public static double sin(double t)
        {
            return Math.Sin(t);
        }

        public static double tsin(double t)
        {
            return t * Math.Sin(t);
        }

        public static double one_minus_t_sin(double t)
        {
            return (1d - t) * Math.Sin(t);
        }


        public static double tttsin(double t)
        {
            return t * t * t * Math.Sin(t);
        }

        public static double step_01(double t)
        {
            if (t >= -0.5d && t <= 0.5d)
            {
                return 1d;
            }
            else return 0d;
        }

        public static double conv_tri_test(double t)
        {
            if (t >= -1d && t <= 0d)
            {
                return t + 1d;
            }
            if (t > 0d && t <= 1d) return 1d - t;

            return 0d;
        }

        public static double test_func1(double t)
        {
            return t * (10d - t) * Math.Exp(3d * t - t * t / 10d);
        }

    }
}
