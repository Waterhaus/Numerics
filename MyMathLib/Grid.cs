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

        public static List<double> CreateUniformGrid(int GridSize,double a, double b)
        {
            List<double> grid = new List<double>();
            double h = MyMath.Basic.GetStep(GridSize, a, b);

            for (int i = 0; i < GridSize; i++)
                grid.Add(a + i*h);

            return grid;
        }
    }
}
