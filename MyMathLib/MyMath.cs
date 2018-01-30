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

        }
    }
}
