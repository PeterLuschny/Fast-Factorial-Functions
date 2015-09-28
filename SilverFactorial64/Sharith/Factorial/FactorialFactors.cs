// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace Sharith.Factorial
{
    using System.IO;
    using Sharith.Primes;
    using XMath = MathUtils.XMath;

    public class FactorialFactors
    {
        public FactorialFactors()
        {
        }

        public FactorialFactors(int n)
        {
            this.Factor(n);
        }

        int[] primeList;
        int[] multiList;
        int n, card;

        public int Factor(int n)
        {
            this.n = n;

            var lgN = XMath.FloorLog2(n);
            var piN = 2 + (15 * n) / (8 * (lgN - 1));

            this.primeList = new int[piN];
            this.multiList = new int[piN];

            return this.card = this.PrimeFactors(n);
        }

        public void SaveToFile(string fileName)
        {
            var file = new FileInfo(@fileName);
            var stream = file.AppendText();
            this.WriteFactors(stream);
            stream.Close();
        }

        public void WriteFactors(TextWriter file)
        {
            if (file == null)
            {
                return;
            }
            file.WriteLine("The prime factors of {0}! ", this.n);

            var sum = this.n - XMath.BitCount(this.n);
            file.Write("2^{0}", sum);

            for (var p = 0; p < this.card; p++)
            {
                int f = this.primeList[p], m = this.multiList[p];
                sum += m;
                if (m > 1)
                {
                    file.Write("*{0}^{1}", f, m);
                }
                else
                {
                    file.Write("*{0}", f);
                }
            }

            file.WriteLine();
            file.WriteLine("Number of different factors: {0} ", this.card);
            file.WriteLine("Number of all factors:       {0} ", sum);
        }

        private int PrimeFactors(int n)
        {
            var sieve = new PrimeSieve(n);
            var primeCollection = sieve.GetPrimeCollection(2, n);

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
}
