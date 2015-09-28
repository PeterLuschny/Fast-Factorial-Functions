// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace Sharith.Factorial 
{
    using System;
    using System.Threading.Tasks;

    using XInt = Arithmetic.XInt;
    using XMath = MathUtils.XMath;

    public class ParallelSwing : IFactorialFunction 
    {
        public string Name => "ParallelSwing         ";

        private XInt oddFactNdiv4, oddFactNdiv2;
        private const int Smallswing = 33;
        private const int Smallfact = 17;
        private Task<XInt> oddSwingTask;

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
            if (n < Smallfact)
            {
                return SmallOddFactorial[n];
            }
            
            var sqrOddFact = this.OddFactorial(n / 2);
            XInt oddFact;

            if (n < Smallswing)
            {
                oddFact = XInt.Pow(sqrOddFact, 2) * SmallOddSwing[n];
            }
            else
            {
                var ndiv4 = n / 4;
                var oddFactNd4 = ndiv4 < Smallfact ? SmallOddFactorial[ndiv4] : this.oddFactNdiv4;
                this.oddSwingTask = Task.Factory.StartNew<XInt>( () => OddSwing(n, oddFactNd4));

                sqrOddFact = XInt.Pow(sqrOddFact, 2);
                oddFact = sqrOddFact * this.oddSwingTask.Result;
            }

            this.oddFactNdiv4 = this.oddFactNdiv2;
            this.oddFactNdiv2 = oddFact;
            return oddFact;
        }

        static XInt OddSwing(int n, XInt oddFactNdiv4)
        {
            var len = (n - 1) / 4;
            if ((n % 4) != 2) len++;

            //-- if type(n, odd) then high = n else high = n-1.
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

    } // endOfFactorialParallelSwing
}
