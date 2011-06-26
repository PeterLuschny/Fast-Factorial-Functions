/// -------- ToujoursEnBeta
/// Author & Copyright : Peter Luschny
/// License: LGPL version 3.0 or (at your option)
/// Creative Commons Attribution-ShareAlike 3.0
/// Comments mail to: peter(at)luschny.de
/// Created: 2010-03-01

#if(MPIR)
using Xint = MpirWrapper.Xint;
#else
using Xint = System.Numerics.BigInteger;
#endif
using Sharith.Math.Primes;

namespace Sharith.Math.Factorial
{
    public sealed class FastBinomial
    {
        private FastBinomial() { }

        public static Xint Binomial(int n, int k)
        {
            if (0 > k || k > n)
            {
                throw new System.ArgumentOutOfRangeException("n",
                 "Binomial: 0 <= k and k <= n required, but n was "
                 + n + " and k was " + k);
            }

            if (k > n / 2) { k = n - k; }

            int rootN = (int)System.Math.Floor(System.Math.Sqrt(n));
            Xint binom = Xint.One;

            var primeCollection = new PrimeSieve(n).GetPrimeCollection();

            foreach (int prime in primeCollection) // Equivalent to a nextPrime() function.
            {
                if (prime > n - k)
                {
                    binom *= prime;
                    continue;
                }

                if (prime > n / 2)
                {
                    continue;
                }

                if (prime > rootN)
                {
                    if (n % prime < k % prime)
                    {
                        binom *= prime;
                    }
                    continue;
                }

                int exp = 0, r = 0, N = n, K = k;

                while (N > 0)
                {
                    r = (N % prime) < (K % prime + r) ? 1 : 0;
                    exp += r;
                    N /= prime;
                    K /= prime;
                }

                if (exp > 0)
                {
                    binom *= Xint.Pow((Xint) prime, exp);
                }
            }
            return binom;
        }

        //static void Main()
        //{
        //    int n = 5673, k = 1239;
        //    // int n = 567, k = 123;

        //    Xint b = Binomial(n, k);
        //    Console.WriteLine("(" + n + "," + k + ") -> " + b);
        //    Console.ReadLine();
        //}
    }
}
