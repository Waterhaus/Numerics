using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMathLib
{
    public class MyMath
    {
        public class Basic
        {

            public static double SumArray(double[] arr)
            {
                double S = 0;

                if (arr == null)
                {
                    throw new System.ArgumentNullException("Parameter cannot be null", "arr");
                }

                for (int i = 0; i < arr.Length; i++)
                {
                    S = S + arr[i];
                }
                return S;
            }

            public static double SumArray(List<double> arr)
            {
                double S = 0;

                if (arr == null)
                {
                    throw new System.ArgumentNullException("Parameter cannot be null", "arr");
                }

                for (int i = 0; i < arr.Count; i++)
                {
                    S = S + arr[i];
                }
                return S;
            }

            //Возвращает шаг сетки на отрезке [a b] с соответсвующим числом точек
            public static double GetStep(int GridSize, double a_border, double b_border)
            {

                if (a_border.Equals(b_border))
                    throw new System.ArgumentException("Ошибка: границы отрезка равны. a=b");

                if (a_border > b_border)
                {
                    double temp = a_border;
                    a_border = b_border;
                    b_border = temp;
                }

                return (b_border - a_border) / (GridSize - 1);
            }

            //no tests
            public static Vector GetVectorFunction(int GridSize, double a_border, double b_border, FunctionLib.Function F)
            {
                Vector f = new Vector(GridSize);
                double h = GetStep(GridSize, a_border, b_border);

                for (int i = 0; i < GridSize; i++)
                {
                    f[i] = F(a_border + i * h);
                }
                return f;
            }

            //no test
            public static double skal(Vector x, Vector y)
            {
                double S = 0;
                for (int i = 0; i < x.Length; i++)
                {
                    S += x[i] * y[i];
                }
                return S;
            }

            public static double[] CreateUniformGrid(int GridSize, double a, double b)
            {
                double[] grid = new double[GridSize];
                double h = GetStep(GridSize, a, b);

                for (int i = 0; i < GridSize; i++)
                    grid[i] =  a + i * h;

                return grid;
            }
        }

        public class Operations
        {
            public static double Convolution(double x, FunctionLib.Function f,FunctionLib.Function g,int GridSize, double a)
            {
                
                Vector grid = Vector.CreateUniformGrid(GridSize, a, x);
                Vector fg = new Vector(GridSize);
                double h = grid[1] - grid[0];

                for (int i = 0; i < GridSize; i++)
                {
                    fg[i] = f(grid[i]) * g(x - grid[i]);
                }


                return Integrate.Simpson(fg.ToArray,h);
            }

            public static double Convolution(double x, FunctionLib.Function f, FunctionLib.Function g, Grid grid)
            {
                int GridSize = grid.Count;
                
                Vector fg = new Vector(GridSize);
                double h = grid[1] - grid[0];

                for (int i = 0; i < GridSize; i++)
                {
                    fg[i] = f(grid[i]) * g(x - grid[i]);
                }
                

                return Integrate.Simpson(fg.ToArray, h);
            }

            Vector Convolution(Vector f, Vector g, int lenA, int lenB, int[] lenC)
            {
                int nconv;
                int i, j, i1;
                double tmp;
                Vector C;

                //allocated convolution array	
                nconv = lenA + lenB - 1;
                C = new Vector(nconv);

                //convolution process
                for (i = 0; i < nconv; i++)
                {
                    i1 = i;
                    tmp = 0d;
                    for (j = 0; j < lenB; j++)
                    {
                        if (i1 >= 0 && i1 < lenA)
                            tmp = tmp + (f[i1] * g[j]);

                        i1 = i1 - 1;
                        C[i] = tmp;
                    }
                }

    
                //return convolution array
                return C;
            }

            public static Vector GetConvolutionVector(FunctionLib.Function f, FunctionLib.Function g, int GridSize, double a, double b)
            {
                Vector fg = new Vector(GridSize);
                Grid grid = new Grid(GridSize, a, b);

                for (int i = 4; i < GridSize; i++)
                {
                    fg[i] = Convolution(grid[i], f, g, grid);
                }

                return fg;
            }





        }


    }
}
