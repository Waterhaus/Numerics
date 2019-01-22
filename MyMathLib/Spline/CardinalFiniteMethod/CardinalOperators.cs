using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMathLib.Spline.CardinalFiniteMethod
{
    public class CardinalOperators
    {

        public static Matrix DerevetiveMatrix(int degree, int SIZE)
        {
            int N = SIZE;
            int M = N + degree - 2;
            uint p = (uint)(degree - 2);
            Matrix D = new Matrix(N, M);

            double[] val = MyMath.Basic.NegativePascalNumbers(p);

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < val.Length; j++)
                {
                    D[i, i + j] = val[j];
                }
            }

            return D;
              
        }


        public static Matrix InterpolateConditionMatrix(int degree, int SIZE, ICardinalStratagy calculate)
        {
            int N = SIZE;
            int M = N + degree - 2;
            uint p = (uint)(degree - 2);
            Matrix D = new Matrix(N, M);

            double[] val = calculate.GetCardinalValue(degree);

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < val.Length; j++)
                {
                    D[i, i + j] = val[j];
                }
            }

            return D;

        }

    }
}
