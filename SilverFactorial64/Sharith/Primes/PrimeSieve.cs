/// -------- ToujoursEnBeta
/// Author & Copyright : Peter Luschny
/// License: LGPL version 3.0 or (at your option)
/// Creative Commons Attribution-ShareAlike 3.0
/// Comments mail to: peter(at)luschny.de
/// Created: 2010-03-01

namespace Sharith.Math.Primes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using Sharith.Math.MathUtils;

    using XInt = Sharith.Arithmetic.XInt;
    using XMath = Sharith.Math.MathUtils.XMath;

/// <summary>
/// This class implements a prime number sieve using
/// an algorithm given by Eratosthenes (276-194 B.C.)
/// Author:  Peter Luschny
/// Version: 2008-09-12
/// </summary>
public class PrimeSieve : IPrimeSieve
{
    readonly int[] primes;
    PositiveRange sieveRange;
    PositiveRange primeRange;
    int numberOfPrimes;
 
    /// <summary>
    /// Initializes a new instance of the PrimeSieve.
    /// </summary>
    /// <param name="upTo">
    /// The upper bound of the sieveRange to be sieved, this means,
    /// the sieveRange [1,n] is searched for prime numbers.
    /// </param>
    public PrimeSieve(int upTo)
    {
        primes = new int[GetPiHighBound(upTo)];
        sieveRange = new PositiveRange(1, upTo);
 
        numberOfPrimes = MakePrimeList(upTo);
        primeRange = new PositiveRange(1, numberOfPrimes);
    }
 
    /// <summary>
    /// Prime number sieve, Eratosthenes (276-194 b. t.)
    /// This implementation considers only multiples of primes
    /// greater than 3, so the smallest value has to be mapped to 5.
    /// Note: There is no multiplication operation in this function.
    /// </summary>
    /// <param name="composite">After execution of the function this
    /// boolean array includes all composite numbers in [5,n]
    /// disregarding multiples of 2 and 3.
    /// </param>
    private static void SieveOfEratosthenes(bool[] composite)
    {
        int d1 = 8;
        int d2 = 8;
        int p1 = 3;
        int p2 = 7;
        int s = 7;
        int s2 = 3;
        int n = 0;
        int len = composite.Length;
        bool toggle = false;
 
        while (s < len)             // --  scan the sieve
        {
            if (!composite[n++])    // --  if a prime is found
            {                       // --  cancel its multiples
                int inc = p1 + p2;
 
                for (int k = s; k < len; k += inc)
                {
                    composite[k] = true;
                }
 
                for (int k = s + s2; k < len; k += inc)
                {
                    composite[k] = true;
                }
            }
 
            if (toggle = !toggle)
            {
                s += d2;
                d1 += 16;
                p1 += 2;
                p2 += 2;
                s2 = p2;
            }
            else
            {
                s += d1;
                d2 += 8;
                p1 += 2;
                p2 += 6;
                s2 = p1;
            }
        }
    }
 
    /// <summary>
    /// Transforms the sieve of Eratosthenes into the
    /// sequence of prime numbers.
    /// </summary>
    /// <param name="n">Upper bound of the sieve.</param>
    /// <returns>Number of primes found.</returns>
    private int MakePrimeList(int n)
    {
        bool[] composite = new bool[n / 3];
 
        SieveOfEratosthenes(composite);
 
        int[] primes = this.primes;     // -- on stack for eff.
        bool toggle = false;
        int p = 5, i = 0, j = 2;
 
        primes[0] = 2;
        primes[1] = 3;
 
        while (p <= n)
        {
            if (!composite[i++])
            {
                primes[j++] = p;
            }
            // -- never mind, it's ok.
            p += (toggle = !toggle) ? 2 : 4;
        }
 
        return j; // number of primes
    }
 
    /// <summary>
    /// GetPiHighBound estimates the number of primes &lt;= n.
    /// </summary>
    /// <param name="n">The upper bound of the intervall under
    /// consideration.</param>
    /// <returns>a simple estimate of the number of primes &lt;= n.
    /// </returns>
    private static int GetPiHighBound(long n)
    {
        if (n < 17) return 6;
        return (int)System.Math.Floor(((double)n) / (System.Math.Log(n) - 1.5));
    }
 
    /// <summary>
    /// Get the n-th prime number.
    /// </summary>
    /// <param name="n">The index of the prime number.</param>
    /// <returns>The n-th prime number.</returns>
    public int GetNthPrime(int n)
    {
        // Handle potential under- or overflow
        primeRange.ContainsOrFail(n);
        return primes[n - 1];
    }
 
    /// <summary>
    /// Checks if a given number is prime.
    /// </summary>
    /// <param name="cand">The number to be checked.</param>
    /// <returns>True iff the given number is prime.</returns>
    public bool IsPrime(int cand)
    {
        sieveRange.ContainsOrFail(cand);
        // The candidate is interpreted as an one point interval!
        return (new PrimeCollection(this, cand)).IsPrime();
    }
 
    /// <summary>
    /// The default collection from the full sieve.
    /// </summary>
    /// <returns>The prime number collection.</returns>
    public IPrimeCollection GetPrimeCollection()
    {
        return new PrimeCollection(this);
    }
 
    /// <summary>
    /// Gives the collection of the prime numbers in the given intervall.
    /// </summary>
    /// <param name="low">The lower bound of the collection interval.</param>
    /// <param name="high">The higher bound of the collection interval.</param>
    /// <returns>The collection of the prime numbers between low and high.</returns>
    public IPrimeCollection GetPrimeCollection(int low, int high)
    {
        return new PrimeCollection(this, new PositiveRange(low, high));
    }
 
    /// <summary>
    /// Gives the collection of the prime numbers in the given range.
    /// </summary>
    /// <param name="range">The range of the collection.</param>
    /// <returns>The prime number collection.</returns>
    public IPrimeCollection GetPrimeCollection(PositiveRange range)
    {
        return new PrimeCollection(this, range);
    }
 
    /// <summary>
    /// Gives the Product of the prime numbers in the given sieveRange.
    /// </summary>
    /// <param name="low">The lower bound of the collection.</param>
    /// <param name="high">The upper bound of the collection.</param>
    /// <returns>The Product of the prime numbers between low and high.
    /// </returns>
    public XInt GetPrimorial(int low, int high)
    {
        return GetPrimorial(new PositiveRange(low, high));
    }

    /// <summary>
    /// Computes the Product of the prime numbers in the given sieveRange.
    /// </summary>
    /// <param name="range">The sieveRange of the enumeration.</param>
    /// <returns>The Product of the prime numbers in the enumeration.
    /// </returns>
    public XInt GetPrimorial(PositiveRange range)
    {
        int start, size;
        var pc = new PrimeCollection(this, range);
        if (pc.GetSliceParameters(out start, out size))
        {
            return XInt.One;
        }

        return XMath.Product(primes, start, size);
    }    
 
    ////////////////////// Private Inner Class ///////////////////////
 
    /// <summary>
    ///  PrimeCollection
    ///  @author Peter Luschny
    ///  @version 2008-09-12
    /// </summary>
    internal class PrimeCollection : IPrimeCollection
    {
        readonly PrimeSieve sieve;
        readonly PositiveRange enumRange;
        readonly PositiveRange primeRange;
        readonly int start, end;
        readonly bool isPrime;
        int state, next, current;
 
        /// <summary>
        /// Initializes the prime collection for the given sieve.
        /// </summary>
        /// <param name="sieve">The sieve, the prime numbers of which
        /// are to be represented by a collection.</param>
        public PrimeCollection(PrimeSieve sieve)
        {
            this.sieve = sieve;
            end = sieve.numberOfPrimes - 1;
 
            enumRange = sieve.sieveRange;
            primeRange = sieve.primeRange;
        }
 
        /// <summary>
        /// Initializes a collection over a subrange of the range
        /// of the sieve. An exception is thrown, if the range given
        /// is not contained in the range of the sieve.
        /// </summary>
        /// <param name="sieve">Prime number sieve to be used.</param>
        /// <param name="enumRange">Range of collection.</param>
        public PrimeCollection(PrimeSieve sieve, PositiveRange enumRange)
        {
            sieve.sieveRange.ContainsOrFail(enumRange);
 
            this.sieve = sieve;
            this.enumRange = enumRange;
 
            int nops = sieve.numberOfPrimes;
            start = IndexOf(enumRange.Min - 1, 0,    nops - 1);
            end   = IndexOf(enumRange.Max,     start, nops) - 1;
 
            if (end < start) //--  PrimeRange is empty.
            {
                end = -1; 
                primeRange = new PositiveRange(0, 0);
            }
            else
            {
                primeRange = new PositiveRange(start + 1, end + 1);
            }
        }
 
        /// <summary>
        /// Initializes a collection consisting out of a single value.
        /// An integer cand is prime iff there is a prime number
        /// in the range (cand-1, cand].
        /// </summary>
        /// <param name="sieve">The sieve to be used.</param>
        /// <param name="cand">The prime number candidate.</param>
        public PrimeCollection(PrimeSieve sieve, int cand)
        {
            // Note, that this PrimeCollection is not made public 
            // via PrimeSieve. It is used only to test primality. 
            // It is a private constructor only for PrimeSieve.
 
            this.sieve = sieve;
            primeRange = enumRange = null;
 
            start = IndexOf(cand - 1, 0, sieve.numberOfPrimes);
            end = sieve.primes[start] == cand ? start + 1 : start;
 
            isPrime = start < end;
        }
 
        /// <summary>
        /// Affirms the primality of a collection of type (cand-1, cand].
        /// </summary>
        /// <returns>True if cand is prime.</returns>
        public bool IsPrime()
        {
            return isPrime;
        }
 
        /// <summary>
        /// Provides an enumerator of the current prime number collection.
        /// This enumerator is thread save.
        /// </summary>
        /// <returns>An enumerator of the current prime number collection.
        /// </returns>
        IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            PrimeCollection result = this;
 
            // Make the Enumerator threadsave!
            if (0 != Interlocked.CompareExchange(ref state, 1, 0))
            {
                result = new PrimeCollection(sieve, enumRange);
                result.state = 1;
            }
            result.next = result.start;
            return result;
        }
 
        // Implementing the IEnumerator<int> interface requires implementing
        // the nongeneric IEnumerator interface also. The Current property
        // appears on both interfaces, and has different return types.
 
        /// <summary>
        /// Provides an enumerator of the current prime number collection.
        /// This enumerator is thread save.
        /// </summary>
        /// <returns>An enumerator of the current prime number 
        /// collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            IEnumerable<int> enumerable = this;
            return enumerable.GetEnumerator();
        }
 
        /// <summary>
        /// The next prime number in the collection.
        /// </summary>
        /// <returns> The current prime number in the collection.</returns>
        object IEnumerator.Current
        {
            get { return sieve.primes[current]; }
        }
 
        /// <summary>
        /// The next prime number in the collection.
        /// </summary>
        /// <returns> The current prime number in the collection.</returns>
        int IEnumerator<int>.Current
        {
            get { return sieve.primes[current]; }
        }
 
        /// <summary>
        /// Checks the current status of the enumerator.
        /// </summary>
        /// <returns>True iff there are more prime numbers to
        /// be enumerated.</returns>
        bool IEnumerator.MoveNext()
        {
            switch (state)
            {
                case 1:
                    if (next > end) goto case 2;
                    current = next++; return true;
                case 2:
                    state = 2; return false;
                default:
                    throw new InvalidOperationException();
            }
        }
 
        /// <summary>
        /// Stop the enumerator before releasing resources.
        /// </summary>
        public void Dispose()
        {
            state = 2;
            // This object will be cleaned up by the Dispose method.
            // Therefore, a call to GC.SupressFinalize takes this object off
            // the finalization queue and prevents finalization code for
            // this object from executing a second time.
            GC.SuppressFinalize(this);
        }
 
        /// <summary>
        /// Throws an NotImplementedException.
        /// </summary>
        void IEnumerator.Reset()
        {
            throw new NotImplementedException();
        }
 
        /// <summary>
        /// Identifies the index of a prime number.
        /// Uses a (modified!) binary search.
        /// </summary>
        /// <param name="value">The prime number given.</param>
        /// <param name="low">Lower bound for the index.</param>
        /// <param name="high">Upper bound for the index.</param>
        /// <returns>The index of the prime number.</returns>
        private int IndexOf(int value, int low, int high)
        {
            int[] data = sieve.primes;
 
            while (low < high)
            {
                int mid = low + (high - low) / 2;
 
                if (data[mid] < value)
                {
                    low = mid + 1;
                }
                else
                {
                    high = mid;
                }
            }
 
            if (low >= data.Length)
            {
                return low;
            }
 
            if (data[low] == value)
            {
                low++;
            }
 
            return low;
        }
 
        /// <summary>
        /// Gives the prime numbers in the collection as an array.
        /// </summary>
        /// <returns>An array of prime numbers representing the collection.
        /// </returns>
        public int[] ToArray()
        {
            int primeCard = primeRange.Size();
            int[] primeList = new int[primeCard];
 
            System.Array.Copy(sieve.primes, start, primeList, 0, primeCard);
 
            return primeList;
        }
       
        /// <summary>
        /// Gives the start and size of the prime range.
        /// </summary>
        /// <param name="begin">start of the prime range.</param>
        /// <param name="size">Size of the prime range.</param>
        /// <returns>Prime range is empty.</returns>
        public bool GetSliceParameters(out int begin, out int size)
        {
            bool empty = 0 == primeRange.Max; // If the primeRange is empty...
            begin = empty ? 0 : start;
            size = empty ? 0 : primeRange.Size();
            return empty;
        }
 
        /// <summary>
        /// Gets the number of primes in the current collection.
        /// </summary>
        /// <value>The number of primes in the current collection.
        /// </value>
        public int NumberOfPrimes
        {
            get
            {
                if (end == -1) return 0;  // If primeRange is empty...
                return primeRange.Size();
            }
        }
 
        /// <summary>
        /// Gives the sieve range of the current collection.
        /// </summary>
        /// <value>Interval that was sieved.</value>
        public PositiveRange SieveRange
        {
            get { return (PositiveRange)enumRange.Clone(); }
        }
 
        /// <summary>
        /// Gets the range of the indices of the prime numbers
        /// in the collection.
        /// </summary>
        /// <value>Range of indices.</value>
        public PositiveRange PrimeRange
        {
            get { return (PositiveRange) primeRange.Clone(); }
        }
 
        /// <summary>
        /// Writes the primes collection to a file.
        /// </summary>
        /// <param name="fileName">Name of the file to write to.
        /// </param>
        /// <returns>The full name of the file.</returns>
        public string ToFile(string fileName)
        {
            FileInfo file = new FileInfo(@fileName);
            StreamWriter primeReport = file.AppendText();
            WritePrimes(primeReport);
            primeReport.Close();
            return file.FullName;
        }
 
        /// <summary>
        /// Writes the primes collection to a stream.
        /// </summary>
        /// <param name="file">The name of the file where the collection
        /// is to be stored.</param>
        private void WritePrimes(StreamWriter file)
        {
            file.WriteLine("SieveRange   {0} : {1}",
                enumRange.ToString(), enumRange.Size());
            file.WriteLine("PrimeRange   {0} : {1}",
                primeRange.ToString(), primeRange.Size());
            file.WriteLine("PrimeDensity {0:F3}",
                (double)primeRange.Size() / (double)enumRange.Size());
 
            int primeOrdinal = start;
            int primeLow = sieve.primes[start];
            int lim = (primeLow / 100) * 100;
 
            foreach (int prime in this)
            {
                primeOrdinal++;
                if (prime >= lim)
                {
                    lim += 100;
                    file.Write(file.NewLine);
                    file.Write("<");
                    file.Write(primeOrdinal);
                    file.Write(".> ");
                }
                file.Write(prime);
                file.Write(" ");
            }
            file.Write(file.NewLine);
            file.Write(file.NewLine);
        }

    }   // endOfPrimeCollection
} }  // endOfPrimeSieve
