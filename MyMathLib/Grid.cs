using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMathLib
{
    public enum GridType { Uniform, BSplineStandart}

    public class Grid
    {
        private List<double> dots;
        private int degree;
        private double a_border;
        private double b_border;

        public int SplineDegree
        {
            get { return degree; }
        }

        public Grid()
        {
            dots = null;
            degree = 0;
            a_border = b_border = 0;
        }
    }
}
