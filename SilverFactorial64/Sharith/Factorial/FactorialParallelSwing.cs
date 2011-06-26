// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

#if(MPIR)
namespace SharithMP.Math.Factorial 
{
    using XInt = Sharith.Arithmetic.XInt;
#else
    namespace Sharith.Math.Factorial {
    using XInt = System.Numerics.BigInteger;
#endif
    using System;
    using System.Threading.Tasks;
    using XMath = Sharith.Math.MathUtils.XMath;

    public class ParallelSwing : IFactorialFunction 
    {
        public ParallelSwing() { }

        public string Name
        {
            get { return "ParallelSwing         "; }
        }

        private XInt oddFactNdiv4, oddFactNdiv2;
        private const int SMALLSWING = 33;
        private const int SMALLFACT = 17;
        private Task<XInt> oddSwingTask;

        public XInt Factorial(int n)
        {
            if (n < 0)
            {
                throw new ArithmeticException(
                "Factorial: n has to be >= 0, but was " + n);
            }

            oddFactNdiv4 = oddFactNdiv2 = XInt.One;
            return OddFactorial(n) << (n - XMath.BitCount(n));
        }

        private XInt OddFactorial(int n)
        {
            XInt oddFact;

            if (n < SMALLFACT)
            {
                return smallOddFactorial[n];
            }
            else
            {
                XInt sqrOddFact = OddFactorial(n / 2);

                if (n < SMALLSWING)
                {
                    oddFact = XInt.Pow(sqrOddFact, 2) * smallOddSwing[n];
                }
                else
                {
                    int ndiv4 = n / 4;
                    var oddFactNd4 = ndiv4 < SMALLFACT ? smallOddFactorial[ndiv4] : oddFactNdiv4;
                    oddSwingTask = Task.Factory.StartNew<XInt>( () => 
                    {
                        return OddSwing(n, oddFactNd4); 
                    });

                    sqrOddFact = XInt.Pow(sqrOddFact, 2);
                    oddFact = sqrOddFact * oddSwingTask.Result;
                }

                oddFactNdiv4 = oddFactNdiv2;
                oddFactNdiv2 = oddFact;
                return oddFact;
            }
        }

        private XInt OddSwing(int n, XInt oddFactNdiv4)
        {
            int len = (n - 1) / 4;
            if ((n % 4) != 2) len++;

            //-- if type(n,odd) then high=n; else high=n-1;
            int high = n - ((n + 1) & 1);

            return Product(high, len) / oddFactNdiv4;
        }

        private static XInt Product(int m, int len)
        {
            const int SEQUENTIAL_THRESHOLD = 100;

            if (len == 1) return new XInt(m);
            if (len == 2) return new XInt((long)m * (m - 2));

            int hlen = len >> 1;

            if (hlen < SEQUENTIAL_THRESHOLD)
            {
                return Product(m - hlen * 2, len - hlen) * Product(m, hlen);
            }

            var left = Task.Factory.StartNew<XInt>(() => Product(m - hlen * 2, len - hlen));
            var right = Product(m, hlen);

            return left.Result * right;
        }

        private static XInt[] smallOddSwing = {
            1,1,1,3,3,15,5,35,35,315,63,693,231,3003,429,6435,6435,109395,
            12155,230945,46189,969969,88179,2028117,676039,16900975,1300075,
            35102025,5014575,145422675,9694845,300540195,300540195 };

        private static XInt[] smallOddFactorial = {
            1,1,1,3,3,15,45,315,315,2835,14175,155925,467775,6081075,
            42567525,638512875,638512875 };

    } // endOfFactorialParallelSwing
}
