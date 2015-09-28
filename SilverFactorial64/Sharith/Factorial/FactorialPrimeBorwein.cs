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

    public class PrimeBorwein : IFactorialFunction 
    {
        public string Name => "PrimeBorwein        ";

        int[] primeList;
        int[] multiList;

        public XInt Factorial(int n)
        {
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException(
                    this.Name + ": " + nameof(n) + " >= 0 required, but was " + n);
            }

            if (n < 20) { return XMath.Factorial(n); }

            var lgN = XMath.FloorLog2(n);
            var piN = 2 + (15 * n) / (8 * (lgN - 1));

            this.primeList = new int[piN];
            this.multiList = new int[piN];

            var len = this.PrimeFactors(n);
            var exp2 = n - XMath.BitCount(n);

            return this.RepeatedSquare(len, 1) << exp2;
        }

        private XInt RepeatedSquare(int len, int k)
        {
            if (len == 0) return XInt.One;

            int i = 0, mult = this.multiList[0];

            while (mult > 1)
            {
                if ((mult & 1) == 1)  // is mult odd ?
                {
                    this.primeList[len++] = this.primeList[i];
                }

                this.multiList[i++] = mult >> 1;
                mult = this.multiList[i];
            }

            var p = XMath.Product(this.primeList, i, len - i);
            return XInt.Pow(p, k) * this.RepeatedSquare(i, 2 * k);
        }

        private int PrimeFactors(int n)
        {
            var sieve = new PrimeSieve(n);
            var primeCollection = sieve.GetPrimeCollection(3, n);

            int maxBound = n / 2, count = 0;

            foreach (var prime in primeCollection)
            {
                var m = prime > maxBound ? 1 : 0;

                if (prime <= maxBound)
                {
                    var q = n;
                    while (q >= prime)
                    {
                        m += q /= prime;
                    }
                }
                this.primeList[count] = prime;
                this.multiList[count++] = m;
            }
            return count;
        }
    }
} // endOfFactorialPrimeBorwein
