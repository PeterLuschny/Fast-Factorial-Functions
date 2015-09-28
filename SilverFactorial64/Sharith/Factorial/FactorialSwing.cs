// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace Sharith.Factorial 
{
    using System;

    using XInt = Sharith.Arithmetic.XInt;
    using XMath = Sharith.MathUtils.XMath;

    public class Swing : IFactorialFunction 
    {
        public string Name => "Swing               ";

        private XInt oddFactNdiv4, oddFactNdiv2;
        private const int Smallswing = 33;
        private const int Smallfact = 17;

        public XInt Factorial(int n)
        {
            if (n < 0)
            {
                throw new ArithmeticException(
                this.Name + ": " + nameof(n) + " >= 0 required, but was " + n);
            }

            this.oddFactNdiv4 = this.oddFactNdiv2 = XInt.One;

            return this.OddFactorial(n) << (n - XMath.BitCount(n));
        }

        private XInt OddFactorial(int n)
        {
            XInt oddFact;

            if (n < Smallfact)
            {
                oddFact = SmallOddFactorial[n];
            }
            else
            {
                var sqrOddFact = this.OddFactorial(n / 2);
                var ndiv4 = n / 4;
                var oddFactNd4 = ndiv4 < Smallfact 
                               ? SmallOddFactorial[ndiv4] 
                               : this.oddFactNdiv4;

                oddFact = XInt.Pow(sqrOddFact, 2) * OddSwing(n, oddFactNd4); 
            }

            this.oddFactNdiv4 = this.oddFactNdiv2;
            this.oddFactNdiv2 = oddFact;
            return oddFact;
        }

        static XInt OddSwing(int n, XInt oddFactNdiv4)
        {
            if (n < Smallswing) return SmallOddSwing[n];

            var len = (n - 1) / 4;
            if ((n % 4) != 2) len++;
            var high = n - ((n + 1) & 1);

            return Product(high, len) / oddFactNdiv4;
        }

        static XInt Product(int m, int len)
        {
            if (len == 1) return new XInt(m);
            if (len == 2) return new XInt((long)m * (m - 2));

            var hlen = len >> 1;
            return Product(m - hlen * 2, len - hlen) * Product(m, hlen);
        }

        static readonly XInt[] SmallOddSwing = {
            1,1,1,3,3,15,5,35,35,315,63,693,231,3003,429,6435,6435,109395,
            12155,230945,46189,969969,88179,2028117,676039,16900975,1300075,
            35102025,5014575,145422675,9694845,300540195,300540195 };

        static readonly XInt[] SmallOddFactorial = {
            1,1,1,3,3,15,45,315,315,2835,14175,155925,467775,6081075,
            42567525,638512875,638512875 };

    } // endOfFactorialSwing
}
