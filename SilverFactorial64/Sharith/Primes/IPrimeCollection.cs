/// -------- ToujoursEnBeta
/// Author & Copyright : Peter Luschny
/// License: LGPL version 3.0 or (at your option)
/// Creative Commons Attribution-ShareAlike 3.0
/// Comments mail to: peter(at)luschny.de
/// Created: 2010-03-01

using System.Collections.Generic;
using Sharith.Math.MathUtils;
 
namespace Sharith.Math.Primes
{
     /// <summary>
     ///  An interface for enumerating a prime number sieve.
     ///
     ///  An IPrimeCollection is both IEnumerable&lt;int&gt;
     ///  and IDisposable as well as an IEnumerator&lt;int&gt;.
     /// <blockquote><pre>
     ///
     /// To understand the difference between "SieveRange" and "PrimeRange"
     /// better, let us consider the following example:
     ///
     /// If the SieveRange is 10..20, then the PrimeRange is 5..8,
     /// because of the following table
     ///
     ///                    &lt;-     SieveRange           -&gt;
     /// 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23
     /// x,1,2,x,3,x,4,x,x, x, 5, x, 6, x, x, x, 7, x, 8, x, x, x, 9
     ///                    &lt;-     PrimeRange           -&gt;
     ///
     /// Thus the smallest prime in the SieveRange is the 5-th prime and
     /// the largest prime in this SieveRange is the 8-th prime.
     ///
     /// </pre></blockquote>
     ///
     /// version cs-2004-09-03
     /// author Peter Luschny
     ///
     /// </summary>
    public interface IPrimeCollection : IEnumerator<int>, IEnumerable<int> 
    {
        /// <summary>
        /// Gets the number of primes in the enumeration.
        /// </summary>
        int NumberOfPrimes { get; }
 
        /// <summary>
        /// Gets the intervall proceeded by the sieve.
        /// </summary>
        PositiveRange SieveRange { get; }
 
        /// <summary>
        /// Gets the SieveRange of the indices of the prime numbers in the enumeration.
        /// </summary>
        PositiveRange PrimeRange { get; }
 
        /// <summary>
        /// Returns the primes in the SieveRange under consideration
        /// as an array.
        /// </summary>
        /// <returns>An array of the primes in the SieveRange.</returns>
        int[] ToArray();
 
        /// <summary>
        /// Writes the prime number enumeration (somewhat formatted)
        /// to a file.
        /// </summary>
        /// <param name="fileName">The name of the file where the
        /// enumeration is to be stored.</param>
        /// <returns>Full path of the file written to.</returns>
        string ToFile(string fileName);
    }
}
