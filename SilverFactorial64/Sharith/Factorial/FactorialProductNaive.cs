/// -------- ToujoursEnBeta
/// Author & Copyright : Peter Luschny
/// License: LGPL version 3.0 or (at your option)
/// Creative Commons Attribution-ShareAlike 3.0
/// Comments mail to: peter(at)luschny.de
/// Created: 2010-03-01

namespace Sharith.Math.Factorial 
{
    using XInt = Sharith.Arithmetic.XInt;

    public class ProductNaive : IFactorialFunction  
    {
        public ProductNaive() { }

        public string Name
        {
            get { return "ProductNaive        "; }
        }

        public XInt Factorial(int n)
        {
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException("n",
                Name + ": n >= 0 required, but was " + n);
            }

            XInt nFact = XInt.One;

            for (int i = 2; i <= n; i++)
            {
                nFact *= i;
            }
            return nFact;
        }
    }
}   // endOfProductNaive
