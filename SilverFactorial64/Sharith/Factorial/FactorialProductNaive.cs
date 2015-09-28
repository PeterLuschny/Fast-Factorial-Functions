// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace Sharith.Factorial 
{
    using XInt = Arithmetic.XInt;

    public class ProductNaive : IFactorialFunction  
    {
        public string Name => "ProductNaive        ";

        public XInt Factorial(int n)
        {
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException(
                    this.Name + ": " + nameof(n) + " >= 0 required, but was " + n);
            }

            var nFact = XInt.One;

            for (var i = 2; i <= n; i++)
            {
                nFact *= i;
            }
            return nFact;
        }
    }
} // endOfProductNaive
