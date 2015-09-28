// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace Sharith.Factorial
{
    using System.Threading.Tasks;

    using XInt = Arithmetic.XInt;

    #region Swing

    //// Same algorithm as PrimeSwing but an alternative
    //// more standalone implementation.

    public class SplitPrimeSwing : IFactorialFunction
    {
        public string Name => "PrimeSwingAllInOne  ";

        private Primes prime;
        private Factors factors;

        //// Swing(n) = OddSwing(n) << (int)MathFun.BitCount(n);

        private XInt OddSwing(uint n)
        {
            if (n < SmallOddSwing.Length)
            {
                return new XInt(SmallOddSwing[n]);
            }

            var rootN = MathFun.FloorSqrt(n);
            this.factors.Init();

            this.factors.SetMax(rootN);
            this.prime.Factorizer(3, rootN, p =>
                {
                    var q = n;
                    while ((q /= p) > 0)
                        if ((q & 1) == 1) { this.factors.Add(p); }
                });

            this.factors.SetMax(n / 3);
            this.prime.Factorizer(rootN + 1, n / 3, p =>
            {
                if (((n / p) & 1) == 1) { this.factors.Add(p); }
            });

            this.factors.SetMax(n);
            this.prime.Factorizer(n / 2 + 1, n, p =>
            {
                this.factors.Add(p);
            });

            return this.factors.Product();
        }

    #endregion
    #region factorial

        public XInt Factorial(int N)
        {
            var n = (uint)N;
            var exp2 = (int)(n - MathFun.BitCount(n));

            if (n < SmallOddFactorial.Length)
            {
                return new XInt(SmallOddFactorial[n]) << exp2;
            }

            if (n >= SmallOddSwing.Length)
            {
                this.prime = new Primes(n);
                this.factors = new Factors(n);
            }

            return this.OddFactorial(n) << exp2;
        }

        private XInt OddFactorial(uint n)
        {
            if (n < SmallOddFactorial.Length)
            {
                return new XInt(SmallOddFactorial[n]);
            }

            return XInt.Pow(this.OddFactorial(n / 2), 2) * this.OddSwing(n);
        }

        static readonly ulong[] SmallOddSwing = {
  1, 1, 1, 3, 3, 15, 5, 35, 35, 315, 63, 693, 231, 3003, 429, 6435, 6435,
  109395, 12155, 230945, 46189, 969969, 88179, 2028117, 676039, 16900975,
  1300075, 35102025, 5014575, 145422675, 9694845, 300540195, 300540195,
  9917826435, 583401555, 20419054425, 2268783825, 83945001525, 4418157975,
  172308161025, 34461632205, 1412926920405, 67282234305, 2893136075115,
  263012370465, 11835556670925, 514589420475, 24185702762325, 8061900920775,
  395033145117975, 15801325804719, 805867616040669, 61989816618513,
  3285460280781189, 121683714103007, 6692604275665385, 956086325095055,
  54496920530418135, 1879204156221315, 110873045217057585, 7391536347803839,
  450883717216034179, 14544636039226909, 916312070471295267, 916312070471295267 };

        static readonly ulong[] SmallOddFactorial = {
  1, 1, 1, 3, 3, 15, 45, 315, 315, 2835, 14175, 155925, 467775, 6081075,
  42567525, 638512875, 638512875, 10854718875, 97692469875, 1856156927625,
  9280784638125, 194896477400625, 2143861251406875, 49308808782358125,
  147926426347074375, 3698160658676859375 };
    }

    #endregion
    #region primes

    class Primes
    {
        const int BitsPerInt = 32;
        const int Mask = BitsPerInt - 1;
        const int Log2Int = 5;

        private static readonly uint[] PrimesOnBits = {
           1762821248u, 848611808u, 3299549660u, 2510511646u };

        private readonly uint[] isComposite;
        public delegate void Visitor(uint x);

        public void Factorizer(uint min, uint max, Visitor visitor)
        {
            // isComposite[0] ... isComposite[n] includes
            // 5 <= prime numbers <= 96*(n+1) + 1

            if (min <= 2) visitor(2);
            if (min <= 3) visitor(3);

            var absPos = (int)((min + (min + 1) % 2) / 3 - 1);
            var index = absPos / BitsPerInt;
            var bitPos = absPos % BitsPerInt;
            bool toggle = (bitPos & 1) == 1;
            var prime = (uint)(5 + 3 * (BitsPerInt * index + bitPos) - (bitPos & 1));

            while (prime <= max)
            {
                uint bitField = this.isComposite[index++] >> bitPos;
                for (; bitPos < BitsPerInt; bitPos++)
                {
                    if ((bitField & 1) == 0)
                    {
                        visitor(prime);
                    }
                    prime += (toggle = !toggle) ? 2u : 4u;
                    if (prime > max) return;
                    bitField >>= 1;
                }
                bitPos = 0;
            }
        }

        /// Prime number sieve, Eratosthenes (276-194 b.t. )
        /// This implementation considers only multiples of primes
        /// greater than 3, so the smallest value has to be mapped to 5.
        /// Note: There is no multiplication operation in this function
        /// and *no call to a sqrt* function.

        public Primes(uint n)
        {
            if (n < 386) { this.isComposite = PrimesOnBits; return; }

            this.isComposite = new uint[(n / (3 * BitsPerInt)) + 1];
            int d1 = 8, d2 = 8, p1 = 3, p2 = 7, s = 7, s2 = 3;
            int l = 0, c = 1, max = (int)n / 3;
            bool toggle = false;

            while (s < max)  // --  scan the sieve
            {
                // --  if a prime is found ...
                if ((this.isComposite[l >> Log2Int] & (1u << (l++ & Mask))) == 0)
                {
                    var inc = p1 + p2;  // --  ... cancel its multiples

                    for (c = s; c < max; c += inc)
                    {               // --  ... set c as composite
                        this.isComposite[c >> Log2Int] |= 1u << (c & Mask);
                    }

                    for (c = s + s2; c < max; c += inc)
                    {
                        this.isComposite[c >> Log2Int] |= 1u << (c & Mask);
                    }
                }

                if (toggle = !toggle) { s += d2; d1 += 16; p1 += 2; p2 += 2; s2 = p2; }
                else { s += d1; d2 += 8; p1 += 2; p2 += 6; s2 = p1; }
            }
        }

        public XInt GetPrimorial(uint min, uint max)
        {
            var mem = (int)(0.63 * max / System.Math.Log(max));
            var plist = new long[mem];
            var size = 0;
            if (min <= 2) plist[size++] = 2;
            if (min <= 3) plist[size++] = 3;

            var absPos = (int)((min + (min + 1) % 2) / 3 - 1);
            var index = absPos / BitsPerInt;
            var bitPos = absPos % BitsPerInt;
            bool toggle = (bitPos & 1) == 1;
            var prime = (uint)(5 + 3 * (BitsPerInt * index + bitPos) - (bitPos & 1));

            while (prime <= max)
            {
                var bitField = this.isComposite[index++] >> bitPos;
                for (; bitPos < BitsPerInt; bitPos++)
                {
                    if ((bitField & 1) == 0)
                    {
                        plist[size++] = prime;
                    }
                    prime += (toggle = !toggle) ? 2u : 4u;
                    if (prime > max)
                    {
                        return MathFun.Product(plist, 0, size);
                    }
                    bitField >>= 1;
                }
                bitPos = 0;
            }
            return XInt.Zero;
        }
    }

    #endregion
    #region support

    class Factors
    {
        private readonly long[] factors;
        private long maxProd, prod;
        private int count;

        public Factors(uint n)
        {
            var mem = (int)(0.63 * n / System.Math.Log(n));
            this.factors = new long[mem];
        }

        public void Init()
        {
            this.maxProd = 1;
            this.prod = 1;
            this.count = 0;
        }

        public void SetMax(long max)
        {
            this.maxProd = long.MaxValue / max;

            if (this.prod >= this.maxProd)
            {
                this.factors[this.count++] = this.prod;
                this.prod = 1;
            }
        }

        public void Add(long prime)
        {
            if (this.prod < this.maxProd)
            {
                this.prod *= prime;
            }
            else
            {
                this.factors[this.count++] = this.prod;
                this.prod = prime;
            }
        }

        public XInt Product()
        {
            this.factors[this.count++] = this.prod;
            return MathFun.Product(this.factors, 0, this.count);
        }
    }

    static public class MathFun
    {
        // Calibrate the treshhold
        private const int ThresholdProductSerial = 512;

        static public XInt Product(long[] seq, int start, int len)
        {
            if (len <= ThresholdProductSerial)
            {
                var rprod = new XInt(seq[start]);

                for (var i = start + 1; i < start + len; i++)
                {
                    rprod *= seq[i];
                }
                return rprod;
            }
            else
            {
                var halfLen = len / 2;
                var rprod = XInt.Zero;
                var lprod = XInt.Zero;

                Parallel.Invoke(
                    () => { rprod = Product(seq, start, halfLen); },
                    () => { lprod = Product(seq, start + halfLen, len - halfLen); }
                );
                return lprod * rprod;
            }
        }

        /// Bit count
        /// sometimes referred to as the population count.
        public static uint BitCount(uint w)
        {
            w = w - ((0xaaaaaaaa & w) >> 1);
            w = (w & 0x33333333) + ((w >> 2) & 0x33333333);
            w = w + (w >> 4) & 0x0f0f0f0f;
            w += w >> 8;
            w += w >> 16;

            return w & 0xff;
        }

        public static uint FloorSqrt(uint n)
        {
            uint a, b;
            a = b = n;
            do
            {
                a = b;
                b = (n / a + a) / 2;
            } while (b < a);
            return a;
        }
    }
    #endregion
}
