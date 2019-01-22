using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMathLib.Spline.CardinalFiniteMethod
{
    public class CardinalSolver
    {


        //Первая тестовая функция для решения дифференциального уравнения второго порядка
        public static void SolveSecondDerevitiveEquation(   int GridSize, double a_border, double b_border,
                                                            ICardinalStratagy calculate,
                                                            double u0, double u1, FunctionLib.Function function)
        {
            int N = GridSize;
            int degree = 4;

            double h = MyMath.Basic.GetStep(N, a_border, b_border);
            CardinalSpline spline = new CardinalSpline(calculate);

            Vector f = MyMath.Basic.GetVectorFunction(N, a_border, b_border, function);
            Matrix D = CardinalDifferentialEquation.SecondDirevetive(N);
            Matrix DD = D * Matrix.Transpose(D);
            //   Console.WriteLine(DD);
            Matrix A = (1d / (h * h)) * DD;

            Console.WriteLine(D * f);
            Vector Dy = D * f;
            Vector b = new Vector(N + 4);
            for (int i = 0; i < N + 2; i++)
            {
                b[i] = Dy[i];
            }
            b[N + 2] = u0;
            b[N + 3] = u1;
            Console.WriteLine("b = " + b);
            Matrix B = new Matrix(N + 4);
            for (int i = 0; i < N + 2; i++)
            {
                for (int j = 0; j < N + 2; j++)
                {
                    B[i, j] = A[i, j];
                }
            }
            for (int i = 0; i < N + 2; i++)
            {
                B[N + 2, i] = calculate.Cardinal(degree, a_border, a_border + (i - degree + 1) * h, h);
                B[N + 3, i] = calculate.Cardinal(degree, b_border, a_border + (i - degree + 1) * h, h);

                B[i, N + 2] = -B[N + 2, i];
                B[i, N + 3] = -B[N + 3, i];

            }

      
        }


        //Нужным образом собираем матрицу для последующего решения задачи (интерполяция или решение дифура)
        public static Matrix ConstructMatrix(Matrix A, Matrix U)
        {
            int m = A.Length.m;
            int k = U.Length.n;

            Matrix B = Matrix.Transpose(A) * A;
            Matrix F = new Matrix(m + k);

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    F[i, j] = B[i, j];
                }
            }

            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    F[m + i, j] = U[i, j];
                    F[j,m + i] = -U[i, j];
                }
            }

            return F;
        }

    }
}
