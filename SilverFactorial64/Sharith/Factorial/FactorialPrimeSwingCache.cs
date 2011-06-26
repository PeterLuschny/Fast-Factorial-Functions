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
    using System.Collections.Generic;
    using Sharith.Math.Primes;
    using XMath = Sharith.Math.MathUtils.XMath;

    public class PrimeSwingCache : IFactorialFunction 
    {
        public PrimeSwingCache() { }

        private Dictionary<int, CachedPrimorial> cache;
        private PrimeSieve sieve;
        private int[] primeList;

        public string Name
        {
            get { return "PrimeSwingCache     "; }
        }

        public XInt Factorial(int n)
        {
            if (n < 20) { return XMath.Factorial(n); }

            cache = new Dictionary<int, CachedPrimorial>();
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

            //-- Not commutative!! 
            return Swing(n) * XInt.Pow(RecFactorial(n / 2),2);
        }

        private XInt Swing(int n)
        {
            if (n < 33) return smallOddSwing[n];

            int count = 0, rootN = XMath.FloorSqrt(n);
            int j = 1, low, high;

            XInt prod = XInt.One;

            while (true)
            {
                high = n / j++;
                low = n / j++;

                if (low < rootN) { low = rootN; }
                if (high - low < 32) break;

                XInt primorial = GetPrimorial(low + 1, high);
                prod *= primorial;
            }

            var primes = sieve.GetPrimeCollection(3, high);

            foreach (int prime in primes)
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

            return prod * XMath.Product(primeList, 0, count);
        }

        XInt GetPrimorial(int low, int high)
        {
            CachedPrimorial lowPrimorial;
            XInt primorial;

            if (cache.TryGetValue(low, out lowPrimorial))
            {
                //-- This is the catch! The intervals expand.
                int mid = lowPrimorial.High + 1;
                XInt highPrimorial = sieve.GetPrimorial(mid, high);
                primorial = highPrimorial * lowPrimorial.Value;
            }
            else
            {
                primorial = sieve.GetPrimorial(low, high);
            }

            cache[low] = new CachedPrimorial(high, primorial);
            return primorial;
        }

        private static XInt[] smallOddSwing = {1, 1, 1, 3, 3, 15, 5, 35, 35,
        315, 63, 693, 231, 3003, 429, 6435, 6435, 109395, 12155, 230945,
        46189, 969969, 88179, 2028117, 676039, 16900975, 1300075, 35102025,
        5014575, 145422675, 9694845, 300540195, 300540195};

        private struct CachedPrimorial
        {
            public int  High;  // class { get; set; } 
            public XInt Value; // class { get; set; } 

            public CachedPrimorial(int highBound, XInt val)
            {
                High = highBound;
                Value = val;
            }
        }
    }
} // endOfFactorialPrimeSwingCache

