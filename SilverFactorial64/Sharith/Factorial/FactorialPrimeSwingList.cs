// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace Sharith.Factorial 
{
    using Sharith.Primes;

    using XInt = Arithmetic.XInt;
    using XMath = MathUtils.XMath;

    public class PrimeSwingList : IFactorialFunction 
    {
        public string Name => "PrimeSwingList      ";

        private int[][] primeList;
        private int[] listLength;
        private int[] tower;
        private int[] bound;

        public XInt Factorial(int n)
        {
            if (n < 20) { return XMath.Factorial(n); }

            var log2N = XMath.FloorLog2(n);
            var j = log2N;
            var hN = n;

            this.primeList = new int[log2N][];
            this.listLength = new int[log2N];
            this.bound = new int[log2N];
            this.tower = new int[log2N + 1];

            while (true)
            {
                this.tower[j] = hN;
                if (hN == 1) break;
                this.bound[--j] = hN / 3;
                var pLen = hN < 4 ? 6 : (int)(2.0 * (XMath.FloorSqrt(hN)
                         + (double) hN / (XMath.Log2(hN) - 1)));
                this.primeList[j] = new int[pLen];
                hN >>= 1;
            }

            this.tower[0] = 2;
            this.PrimeFactors(n);

            var init = this.listLength[0] == 0 ? 1 : 3;
            var oddFactorial = new XInt(init);
            
            for (var i = 1; i < log2N; i++)
            {
                oddFactorial = XInt.Pow(oddFactorial, 2)
                    * XMath.Product(this.primeList[i], 0, this.listLength[i]);
            }
            return oddFactorial << (n - XMath.BitCount(n));
        }

        private void PrimeFactors(int n)
        {
            var maxBound = n / 3;
            var lastList = this.primeList.Length - 1;
            var start = this.tower[1] == 2 ? 1 : 0;
            var sieve = new PrimeSieve(n);

            for (var section = start; section < this.primeList.Length; section++)
            {
                var primes = sieve.GetPrimeCollection(this.tower[section] + 1, this.tower[section + 1]);

                foreach (var prime in primes)
                {
                    this.primeList[section][this.listLength[section]++] = prime;
                    if (prime > maxBound) continue;

                    var np = n;
                    do
                    {
                        var k = lastList;
                        var q = np /= prime;

                        do if ((q & 1) == 1) //if q is odd
                            {
                                this.primeList[k][this.listLength[k]++] = prime;
                            }
                        while (((q >>= 1) > 0) && (prime <= this.bound[--k]));

                    } while (prime <= np);
                }
            }
        }
    }
} // endOfFactorialPrimeSwingList
