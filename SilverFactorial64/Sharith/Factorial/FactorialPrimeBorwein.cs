/// -------- ToujoursEnBeta
/// Author & Copyright : Peter Luschny
/// License: LGPL version 3.0 or (at your option)
/// Creative Commons Attribution-ShareAlike 3.0
/// Comments mail to: peter(at)luschny.de
/// Created: 2010-03-01

namespace Sharith.Math.Factorial 
{  
    using Sharith.Math.Primes;
    using XMath = Sharith.Math.MathUtils.XMath;
    using XInt = Sharith.Arithmetic.XInt;

    public class PrimeBorwein : IFactorialFunction 
    {
        public PrimeBorwein() { }

        public string Name
        {
            get { return "PrimeBorwein        "; }
        }

        private int[] primeList;
        private int[] multiList;

        public XInt Factorial(int n)
        {
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException("n",
                Name + ": n >= 0 required, but was " + n);
            }

            if (n < 20) { return XMath.Factorial(n); }

            int lgN = XMath.FloorLog2(n);
            int piN = 2 + (15 * n) / (8 * (lgN - 1));

            primeList = new int[piN];
            multiList = new int[piN];

            int len = PrimeFactors(n);
            int exp2 = n - XMath.BitCount(n);

            return RepeatedSquare(len, 1) << exp2;
        }

        private XInt RepeatedSquare(int len, int k)
        {
            if (len == 0) return XInt.One;

            int i = 0, mult = multiList[0];

            while (mult > 1)
            {
                if ((mult & 1) == 1)  // is mult odd ?
                {
                    primeList[len++] = primeList[i];
                }

                multiList[i++] = mult >> 1;
                mult = multiList[i];
            }

            XInt p = XMath.Product(primeList, i, len - i);
            return XInt.Pow(p, k) * RepeatedSquare(i, 2 * k);
        }

        private int PrimeFactors(int n)
        {
            var sieve = new PrimeSieve(n);
            var primeCollection = sieve.GetPrimeCollection(3, n);

            int maxBound = n / 2, count = 0;

            foreach (int prime in primeCollection)
            {
                int m = prime > maxBound ? 1 : 0;

                if (prime <= maxBound)
                {
                    int q = n;
                    while (q >= prime)
                    {
                        m += q /= prime;
                    }
                }
                primeList[count] = prime;
                multiList[count++] = m;
            }
            return count;
        }
    }
} // endOfFactorialPrimeBorwein
