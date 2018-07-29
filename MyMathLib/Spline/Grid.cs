using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMathLib
{
    public enum GridType { SimpleSplineGrid , ClassicSplineGrid, UniformSplineGrid, ExperimentSplineGrid, MiddleUniformSplineGrid, RightUniformSplineGrid, LeftUniformSplineGrid }

    public class Grid
    {
        private List<double> extendetGrid;
        private List<double> originGrid;
        private int degree;
        private double a_border;
        private double b_border;
        private int dim;

        private int beginIndex;
        private int endIndex;
       
        public GridType gridType;


        public int SplineDegree
        {
            get { return degree; }
        }

        //true size
        public int Count
        {
            get { return extendetGrid.Count; }
        }

        public int OriginalCount
        {
            get { return dim; }
        }

        //левая граница отрезка [a b]
        public double Left
        {
            get { return a_border; }
        }

        //правая граница отрезка [a b]
        public double Right
        {
            get { return b_border; }
        }

        public double First
        {
            get { return extendetGrid[0]; }
        }

        public double Last
        {
            get { return extendetGrid[extendetGrid.Count - 1]; }
        }

        public int BeginIndex
        {
            get
            {
                return beginIndex;
            }
        }

        public int EndIndex
        {
            get
            {
                return endIndex;
            }
        }

        public Grid()
        {
            extendetGrid = null;
            originGrid = null;
            endIndex = beginIndex = 0;
            dim = 0;
            degree = 0;
            a_border = b_border = 0;
        }

        public Grid(int GridSize,double a, double b)
        {
            a_border = a;
            b_border = b;
            degree = 0;
            extendetGrid = CreateUniformGrid(GridSize, a, b);
            originGrid = new List<double>(extendetGrid);
            dim = GridSize;

            beginIndex = FindBeginIndex(extendetGrid);
            endIndex = FindEndIndex(extendetGrid);
        }

        public Grid(int deg, int GridSize, double a, double b)
        {
            a_border = a;
            b_border = b;
            degree = deg;
            extendetGrid = CreateBSplineClassicGrid(deg,GridSize,a,b);
            originGrid = CreateUniformGrid(GridSize, a, b);
            dim = GridSize;

            beginIndex = FindBeginIndex(extendetGrid);
            endIndex = FindEndIndex(extendetGrid);
        }

        public Grid(int deg, double[] tau)
        {
            a_border = tau[0];
            b_border = tau[tau.Length - 1];
            degree = deg;
            extendetGrid = CreateBSplineClassicGrid(deg, tau);
            originGrid = new List<double>(tau);
            dim = tau.Length;

            beginIndex = FindBeginIndex(extendetGrid);
            endIndex = FindEndIndex(extendetGrid);
        }

        public Grid(int deg, Vector tau)
        {
            a_border = tau[0];
            b_border = tau[tau.Length - 1];
            degree = deg;
            extendetGrid = CreateBSplineClassicGrid(deg, tau.ToArray);
            originGrid = new List<double>(tau.ToArray);
            dim = tau.Length;

            beginIndex = FindBeginIndex(extendetGrid);
            endIndex = FindEndIndex(extendetGrid);
        }

        public Grid(int deg, Vector origin, double a, double b)
        {
            a_border = a;
            b_border = b;
            degree = deg;
            dim = origin.Length;

            int GridSize = dim - degree + 2;
            extendetGrid = CreateNewBasisSplineGrid(deg, GridSize, a, b);
            originGrid = new List<double>(origin.ToArray);
            

            beginIndex = FindBeginIndex(extendetGrid);
            endIndex = FindEndIndex(extendetGrid);
        }

        public Grid(int deg, Vector origin, double a, double b, bool isEmpty)
        {
            a_border = a;
            b_border = b;
            degree = deg;
            dim = origin.Length;

            extendetGrid = new List<double>();
            originGrid = new List<double>(origin.ToArray);
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


        public double GetOrigin(int index)
        {
            return originGrid[index];
        }
        public double[] GetOriginArray()
        {
            return originGrid.ToArray();
        }

        public double[] ToArray()
        {
            return extendetGrid.ToArray();
        }

        public void ToClassicSplineGrid()
        {
            gridType = GridType.ClassicSplineGrid;
            extendetGrid.Clear();


            int GridSize = dim - degree + 2;
            extendetGrid = CreateNewBasisSplineGrid(degree, GridSize, a_border, b_border);


            beginIndex = FindBeginIndex(extendetGrid);
            endIndex = FindEndIndex(extendetGrid);
        }

        public void ToUniformSplineGrid()
        {
            gridType = GridType.UniformSplineGrid;
            extendetGrid.Clear();

            int p1 = degree / 2;
            int p2 = p1;
            if (p1 + p2 < degree) p1 = p1 + 1;
            extendetGrid = new List<double>(originGrid);
            Expend(p1, p2, ref extendetGrid);

            beginIndex = FindBeginIndex(extendetGrid);
            endIndex = FindEndIndex(extendetGrid);
        }

        //расширение отрезка [a, b]
        public void ToMiddleUniformSplineGrid()
        {
            gridType = GridType.MiddleUniformSplineGrid;
            extendetGrid.Clear();

            int p1 = degree / 2;
            int p2 = p1;
            if (p1 + p2 < degree) p1 = p1 + 1;
            extendetGrid = new List<double>(originGrid);
            double h = MyMath.Basic.GetStep(originGrid.Count, originGrid[0], originGrid[originGrid.Count - 1]);
            Expend(h,p1, p2, ref extendetGrid);

            beginIndex = p1;
            endIndex = extendetGrid.Count - p2 - 1; //???????
        }


        //расширение отрезка [a, b]
        public void ToSimpleUniformSplineGrid()
        {
            gridType = GridType.SimpleSplineGrid;
            extendetGrid.Clear();

            int p1 = degree - 1;
            int p2 = degree - 1;
            
            extendetGrid = new List<double>(originGrid);
            double h = MyMath.Basic.GetStep(originGrid.Count, originGrid[0], originGrid[originGrid.Count - 1]);
            Expend(h,p1 - 1, p2 - 1,ref extendetGrid);
            ConstExpend( p1, p2, ref extendetGrid);

            beginIndex = p1;
            endIndex = extendetGrid.Count - p2 - 1; //???????
        }

        //расширение отрезка [a, b]
        public void ToRightUniformSplineGrid()
        {
            gridType = GridType.RightUniformSplineGrid;
            extendetGrid.Clear();

            int p1 = 0;
            int p2 = degree;
            
            extendetGrid = new List<double>(originGrid);
            double h = MyMath.Basic.GetStep(originGrid.Count, originGrid[0], originGrid[originGrid.Count - 1]);
            Expend(h, p1, p2, ref extendetGrid);

            beginIndex = 0;
            endIndex = extendetGrid.Count - 1; //???????
        }


        //расширение отрезка [a, b]
        public void ToLeftUniformSplineGrid()
        {
            gridType = GridType.LeftUniformSplineGrid;
            extendetGrid.Clear();

            int p1 = degree;
            int p2 = 0;

            extendetGrid = new List<double>(originGrid);
            double h = MyMath.Basic.GetStep(originGrid.Count, originGrid[0], originGrid[originGrid.Count - 1]);
            Expend(h, p1, p2, ref extendetGrid);

            beginIndex = p1;
            endIndex = extendetGrid.Count - 1; //???????
        }
        public void ToExperimentSplineGrid()
        {
            gridType = GridType.UniformSplineGrid;
            extendetGrid.Clear();

            int p1 = degree / 2;
            int p2 = p1;
            if (p1 + p2 < degree) p1 = p1 + 1;
            extendetGrid = new List<double>(originGrid);
            Expend(p1, p2, ref extendetGrid);
            extendetGrid[3] = extendetGrid[4];
            beginIndex = FindBeginIndex(extendetGrid);
            endIndex = FindEndIndex(extendetGrid);
        }

        public static List<double> CreateUniformGrid(int GridSize,double a, double b)
        {
            List<double> grid = new List<double>();
            double h = MyMath.Basic.GetStep(GridSize, a, b);

            for (int i = 0; i < GridSize; i++)
                grid.Add(a + i*h);

            return grid;
        }


        public static void Expend(int deg, ref List<double> grid)
        {
            if (deg % 2 == 0)
            {

                double begin = grid[0];
                double end = grid[grid.Count - 1];
                int count = deg / 2;


                for (int k = 1; k <= count; k++)
                {
                    grid.Insert(0, begin);
                    grid.Add(end);
                }

            }
            else
            {

                double begin = grid[0];
                double end = grid[grid.Count - 1];

                int count = (deg - 1) / 2;
                for (int k = 1; k <= count + 1; k++)
                {
                    grid.Insert(0, begin);

                }

                for (int k = 1; k <= count; k++)
                {
                    grid.Add(end);

                }
            }


        }

        public static void Expend(int left_count, int right_count, ref List<double> grid)
        {
            

                double begin = grid[0];
                double end = grid[grid.Count - 1];
                


                for (int k = 1; k <= left_count; k++)
                {
                    grid.Insert(0, begin);
                   
                }
                for (int k = 1; k <= right_count; k++)
                {
                    grid.Add(end);
                }


        }

        public static void Expend(double step, int left_count, int right_count, ref List<double> grid)
        {


            double begin = grid[0];
            double end = grid[grid.Count - 1];



            for (int k = 1; k <= left_count; k++)
            {
                grid.Insert(0, begin - k*step);

            }
            for (int k = 1; k <= right_count; k++)
            {
                grid.Add(end + k*step);
            }


        }

        public static void ConstExpend(int left_count, int right_count, ref List<double> grid)
        {


            double begin = grid[0];
            double end = grid[grid.Count - 1];



            for (int k = 1; k <= left_count; k++)
            {
                grid.Insert(0, begin);

            }
            for (int k = 1; k <= right_count; k++)
            {
                grid.Add(end);
            }


        }

        public static int FindBeginIndex(List<double> mas)
        {
            int index = -1;
            for (index = 0; index < mas.Count - 1; index++)
            {
                if (!mas[index].Equals(mas[index + 1])) return index;
            }
            return index;
        }

        public static int FindEndIndex(List<double> mas)
        {
            int index = -1;
            for (index = mas.Count - 1; index >= 1; index--)
            {
                if (!mas[index].Equals(mas[index - 1])) return index;
            }
            return index;
        }


        public static List<double> CreateBSplineClassicGrid(int deg, int GridSize, double a, double b)
        {
            List<double> grid = CreateUniformGrid(GridSize, a, b);
            Expend(deg, ref grid);
            
            return grid;

        }


        public static List<double> CreateNewBasisSplineGrid(int deg, int GridSize, double a, double b)
        {
            int p = deg - 1;
            List<double> grid = CreateUniformGrid(GridSize, a, b);
            Expend(p,p, ref grid);

            return grid;

        }

        public static List<double> CreateBSplineClassicGrid(int deg, double[] tau)
        {
            List<double> grid = new List<double>(tau);
            Expend(deg, ref grid);

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

        public string ToStringOrigin()
        {
            string S = "<";

            foreach (double c in originGrid)
                S += c.ToString("0.00") + " ";
            S += ">";
            return S;
        }

        public int Find(double x)
        {
            double EPS = 0.00000001d;
            if (x < extendetGrid[0] || x > extendetGrid[extendetGrid.Count - 1]) throw new ArgumentException("Точка х вне границ отрезка!");

            int index = FindBeginIndex(extendetGrid);
            for ( ; index < extendetGrid.Count - 1; index++)
            {
                if (x >= extendetGrid[index] && x < extendetGrid[index + 1])
                    return index;
            }

            
            return FindEndIndex(extendetGrid);

           
        }
    }
}
