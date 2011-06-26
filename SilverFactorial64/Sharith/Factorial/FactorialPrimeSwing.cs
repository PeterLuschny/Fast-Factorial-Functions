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
    using Sharith.Math.Primes;
    using XMath = Sharith.Math.MathUtils.XMath;

    public class PrimeSwing : IFactorialFunction 
    {
        public PrimeSwing() { }

        private PrimeSieve sieve;
        private int[] primeList;

        public string Name
        {
            get { return "PrimeSwing          "; }
        }   

        public XInt Factorial(int n)
        {
            if (n < 20) { return XMath.Factorial(n); }

            sieve = new PrimeSieve(n);
            int pLen = (int)(2.0 * (XMath.FloorSqrt(n)
                     + (double)n / (XMath.Log2(n) - 1)));
            primeList = new int[pLen];
            
            int exp2 = n - XMath.BitCount(n);
            return RecFactorial(n) << exp2;
        }

        private XInt RecFactorial(int n)
        {
            if (n < 2) return XInt.One;

            return XInt.Pow(RecFactorial(n / 2), 2) * Swing(n);
        }

        private XInt Swing(int n)
        {
            if (n < 33) return smallOddSwing[n];

            int count = 0, rootN = XMath.FloorSqrt(n);

            var aPrimes = sieve.GetPrimeCollection(3, rootN);
            var bPrimes = sieve.GetPrimeCollection(rootN + 1, n / 3);

            foreach (int prime in aPrimes)
            {
                int q = n, p = 1;

                while ((q /= prime) > 0)
                {
                    if ((q & 1) == 1)
                    {
                        p *= prime;
                    }
                }

                if (p > 1)
                {
                    primeList[count++] = p;
                }
            }

            foreach (int prime in bPrimes)
            {
                if (((n / prime) & 1) == 1)
                {
                    primeList[count++] = prime;
                }
            }

            XInt primorial = sieve.GetPrimorial(n / 2 + 1, n);
            return primorial * XMath.Product(primeList, 0, count);
        }

        private static XInt[] smallOddSwing = {
            1,1,1,3,3,15,5,35,35,315,63,693,231,3003,429,6435,6435,109395,
            12155,230945,46189,969969,88179,2028117,676039,16900975,1300075,
            35102025,5014575,145422675,9694845,300540195,300540195 };
    }
} // endOfFactorialPrimeSwingLuschny
