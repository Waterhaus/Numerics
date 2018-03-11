using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyMathLib
{
    [TestClass]
    public class PRTest
    {
        [TestMethod]
        public void basic_p_basic_r_dimention_test()
        {
            //setup
            Vector vec_old = Vector.GetConstVector(1d, 16);
            Console.WriteLine("old = " + vec_old.ToString());
            //run
            Vector vec_new = MultyGrid.Resumption.basic_p( MultyGrid.Proektor.basic_r(vec_old));
            Console.WriteLine("new = " + vec_new.ToString());
            //compare
            Assert.AreEqual(vec_old.Length, vec_new.Length, "Размерности должны совпадать");

        }

        [TestMethod]
        public void basic_r_norm_safe_test()
        {
            //setup
            Vector vec_old = Vector.GetConstVector(1d, 64);
            double h_old = 1d / 64d;
            double h_new = 1d / 4d;
            Console.WriteLine("old = " + vec_old.ToString());
            //run
            Vector vec_new = new Vector(64);
            Vector.copy(ref vec_new, vec_old);
            for (int i = 0; i < 4; i++)
            {
                vec_new = MultyGrid.Proektor.basic_r(vec_new);
            }
            
            Console.WriteLine("new = " + vec_new.ToString());
            //compare
            Assert.AreEqual(Math.Sqrt(h_old)*vec_old.Norm, Math.Sqrt(h_new) * vec_new.Norm,0.0000001d, "Вторая норма не должна меняться");

        }
    }
}
