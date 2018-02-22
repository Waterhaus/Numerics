using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMathLib
{
    public enum GridType { Uniform, BSplineStandart, ClassicQubic}

    public class Grid
    {
        private List<double> extendetGrid;
        private List<double> originGrid;
        private int degree;
        private double a_border;
        private double b_border;

        public int SplineDegree
        {
            get { return degree; }
        }

        public int Count
        {
            get { return extendetGrid.Count; }
        }

        public double Left
        {
            get { return a_border; }
        }

        public double Right
        {
            get { return b_border; }
        }

        public Grid()
        {
            extendetGrid = null;
            degree = 0;
            a_border = b_border = 0;
        }

        public Grid(int GridSize,double a, double b)
        {
            a_border = a;
            b_border = b;
            degree = 0;
            extendetGrid = CreateUniformGrid(GridSize, a, b);

        }

        public Grid(int deg, int GridSize, double a, double b)
        {
            a_border = a;
            b_border = b;
            degree = deg;
            extendetGrid = CreateBSplineClassicGrid(deg,GridSize,a,b);

        }

        public Grid(GridType type, int GridSize, double a, double b)
        {
            switch (type)
            {
                case GridType.ClassicQubic:

                    a_border = a;
                    b_border = b;
                    degree = 4;
                    extendetGrid = CreateBSplineClassicGrid(degree, GridSize, a, b);

                    break;
            }
        }
        public double this[int i]
        {
            //Метод доступа для чтения
            get
            {
                return extendetGrid[i];
            }
            //Метод доступа для установки
            /*
            set
            {
                dots[i] = value;
            }*/
        }



        public static List<double> CreateUniformGrid(int GridSize,double a, double b)
        {
            List<double> grid = new List<double>();
            double h = MyMath.Basic.GetStep(GridSize, a, b);

            for (int i = 0; i < GridSize; i++)
                grid.Add(a + i*h);

            return grid;
        }

        public static List<double> CreateBSplineClassicGrid(int deg, int GridSize, double a, double b)
        {
            List<double> grid;

            if (deg % 2 == 0)
            {

                grid = CreateUniformGrid(GridSize, a, b);
                double begin = grid[0];
                double end = grid[grid.Count - 1];
                int count = deg / 2;
                

                for (int k = 1; k <= count; k++)
                {
                    grid.Insert(0, begin);
                    grid.Add(end);
                }

            } else throw new System.ArgumentException("Степень должна быть четной!");
            

            

            return grid;

        }

        public override string ToString()
        {
            string S = "<";

            foreach (double c in extendetGrid)
                S += c.ToString("0.00") + " ";
            S += ">";
            return S;
        }
    }
}
