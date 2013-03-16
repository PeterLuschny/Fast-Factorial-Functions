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

    public class PrimeVardi : IFactorialFunction 
    {
        public PrimeVardi() { }

        private PrimeSieve sieve;

        public string Name
        {
            get { return "PrimeVardi          "; }
        }              

        public XInt Factorial(int n)
        {
            if (n < 20) { return XMath.Factorial(n); }

            sieve = new PrimeSieve(n);

            return RecFactorial(n);
        }

        private XInt RecFactorial(int n)
        {
            if (n < 2) return XInt.One;

            if ((n & 1) == 1)
            {
                return RecFactorial(n - 1) * n;
            }

            return MiddleBinomial(n) * XInt.Pow(RecFactorial(n / 2), 2);
        }

        private XInt MiddleBinomial(int n) // assuming n = 2k
        {
            if (n < 50) return new XInt(binomial[n / 2]);

            int k = n / 2, pc = 0, pp = 0, exp;
            int rootN = XMath.FloorSqrt(n);

            XInt bigPrimes = sieve.GetPrimorial(k + 1, n);
            XInt smallPrimes = sieve.GetPrimorial(k / 2 + 1, n / 3);

            var primes = sieve.GetPrimeCollection(rootN + 1, n / 5);
            var primeList = new int[primes.NumberOfPrimes];

            foreach (int prime in primes)
            {
                if ((n / prime & 1) == 1)  // if n/prime is odd...
                {
                    primeList[pc++] = prime;
                }
            }
            XInt prodPrimes = XMath.Product(primeList, 0, pc);

            primes = sieve.GetPrimeCollection(1, rootN);
            var primePowers = new XInt[primes.NumberOfPrimes];

            foreach (int prime in primes)
            {
                if ((exp = ExpSum(prime, n)) > 0)
                {
                    primePowers[pp++] = XInt.Pow(prime, exp);
                }
            }
            XInt powerPrimes = XMath.Product(primePowers, 0, pp);

            return bigPrimes * smallPrimes * prodPrimes * powerPrimes;
        }

        private static int ExpSum(int p, int n)
        {
            int exp = 0, q = n / p;

            while (0 < q)
            {
                exp += q & 1;
                q /= p;
            }

            return exp;
        }

        private static long[] binomial = {
            1,2,6,20,70,252,924,3432,12870,48620,184756,705432,2704156,
            10400600,40116600,155117520,601080390,2333606220L,9075135300L,
            35345263800L,137846528820L,538257874440L,2104098963720L,
            8233430727600L,32247603683100L };
    }
}
