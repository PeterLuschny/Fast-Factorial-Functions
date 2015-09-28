// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace Sharith.Factorial 
{
    using Sharith.Primes;

    using XInt = Sharith.Arithmetic.XInt;
    using XMath = Sharith.MathUtils.XMath;

    public class PrimeLeenstra : IFactorialFunction 
    {
        public string Name => "PrimeLeenstra       ";

        public XInt Factorial(int n)
        {
            if (n < 20) { return XMath.Factorial(n); }

            var rootN = XMath.FloorSqrt(n);
            var log2N = XMath.FloorLog2(n);
            var section = new XInt[log2N + 1]; 

            for (var i = 0; i < section.Length; i++)
            {
                section[i] = XInt.One;
            }

            var sieve = new PrimeSieve(n);
            var primes = sieve.GetPrimeCollection(3, rootN);

            foreach (var prime in primes)
            {
                int k = 0, m = 0, q = n;

                do
                {
                    m += q /= prime;

                } while (q >= 1);

                while (m > 0)
                {
                    if ((m & 1) == 1)
                    {
                        section[k] *= prime;
                    }
                    m = m / 2;
                    k++;
                }
            }

            var j = 2; 
            var low = n;

            while (low != rootN)
            {
                var high = low;
                low = n / j++;

                if (low < rootN) { low = rootN; }

                var primorial = sieve.GetPrimorial(low + 1, high);

                if (primorial != XInt.One)
                {
                    int k = 0, m = j - 2;

                    while (m > 0)
                    {
                        if ((m & 1) == 1)
                        {
                            section[k] *= primorial;
                        }
                        m = m / 2;
                        k++;
                    }
                }
            }

            var factorial = section[log2N];
            for (var i = log2N - 1; i >= 0; --i)
            {
                factorial = XInt.Pow(factorial,2) * section[i];
            }

            var exp2N = n - XMath.BitCount(n);
            return factorial << exp2N;
        }
    }
} // endOfFactorialPrimeLeenstra
