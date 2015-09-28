// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace Sharith.Factorial 
{
    using System.Collections.Generic;
    using Sharith.Primes;

    using XInt  = Arithmetic.XInt;
    using XMath = MathUtils.XMath;

    public class PrimeSwingCache : IFactorialFunction 
    {
        public string Name => "PrimeSwingCache     ";

        private Dictionary<int, CachedPrimorial> cache;
        private PrimeSieve sieve;
        private int[] primeList;

        public XInt Factorial(int n)
        {
            if (n < 20) { return XMath.Factorial(n); }

            this.cache = new Dictionary<int, CachedPrimorial>();
            this.sieve = new PrimeSieve(n);

            var pLen = (int)(2.0 * (XMath.FloorSqrt(n)
                     + (double)n / (XMath.Log2(n) - 1)));
            this.primeList = new int[pLen];

            var exp2 = n - XMath.BitCount(n);
            return this.RecFactorial(n) << exp2;
        }

        private XInt RecFactorial(int n)
        {
            if (n < 2) return XInt.One;

            //-- Not commutative!! 
            return this.Swing(n) * XInt.Pow(this.RecFactorial(n / 2), 2);
        }

        private XInt Swing(int n)
        {
            if (n < 33) return SmallOddSwing[n];

            var count = 0;
            var rootN = XMath.FloorSqrt(n);
            var j = 1;
            var prod = XInt.One;
            int high;

            while (true)
            {
                high = n / j++;
                var low = n / j++;

                if (low < rootN) { low = rootN; }
                if (high - low < 32) break;

                var primorial = this.GetPrimorial(low + 1, high);
                prod *= primorial;
            }

            var primes = this.sieve.GetPrimeCollection(3, high);

            foreach (var prime in primes)
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
                    this.primeList[count++] = p;
                }
            }

            return prod * XMath.Product(this.primeList, 0, count);
        }

        XInt GetPrimorial(int low, int high)
        {
            CachedPrimorial lowPrimorial;
            XInt primorial;

            if (this.cache.TryGetValue(low, out lowPrimorial))
            {
                //-- This is the catch! The intervals expand.
                var mid = lowPrimorial.High + 1;
                var highPrimorial = this.sieve.GetPrimorial(mid, high);
                primorial = highPrimorial * lowPrimorial.Value;
            }
            else
            {
                primorial = this.sieve.GetPrimorial(low, high);
            }

            this.cache[low] = new CachedPrimorial(high, primorial);
            return primorial;
        }

        static readonly XInt[] SmallOddSwing = {1, 1, 1, 3, 3, 15, 5, 35, 35,
        315, 63, 693, 231, 3003, 429, 6435, 6435, 109395, 12155, 230945,
        46189, 969969, 88179, 2028117, 676039, 16900975, 1300075, 35102025,
        5014575, 145422675, 9694845, 300540195, 300540195};

        private struct CachedPrimorial
        {
            public readonly int  High;  // class { get; set; } 
            public readonly XInt Value; // class { get; set; } 

            public CachedPrimorial(int highBound, XInt val)
            {
                this.High = highBound;
                this.Value = val;
            }
        }
    }
} // endOfFactorialPrimeSwingCache

