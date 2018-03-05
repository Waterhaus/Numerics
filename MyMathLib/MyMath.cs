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

        }

      


    }
}
