using System;
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


        public static double tttsin(double t)
        {
            return t * t * t * Math.Sin(t);
        }
    }
}
