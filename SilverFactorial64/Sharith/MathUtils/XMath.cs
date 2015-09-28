// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace Sharith.MathUtils
{
    using System;
    using System.Threading.Tasks;

    using XInt = Sharith.Arithmetic.XInt;

    public static class XMath
    {
        /// <summary>
        /// Bit count
        /// </summary>
        /// <param name="w"></param>
        /// <returns>Number of bits.</returns>
        public static int BitCount(int w)
        {
            w -= (int)((0xaaaaaaaa & w) >> 1);
            w = (w & 0x33333333) + ((w >> 2) & 0x33333333);
            w = w + (w >> 4) & 0x0f0f0f0f;
            w += w >> 8;
            w += w >> 16;

            return w & 0xff;
        }

        /// <summary>
        /// Calculates the bit length of an integer.
        /// </summary>
        /// <param name="w"></param>
        /// <returns>bit length</returns>
        public static int BitLength(int w)
        {
            return (w < 1 << 15
            ? (w < 1 << 7 ? (w < 1 << 3 ? (w < 1 << 1 ? (w < 1 << 0 ? (w < 0 ? 32 : 0) : 1)
            : (w < 1 << 2 ? 2 : 3)) : (w < 1 << 5 ? (w < 1 << 4 ? 4 : 5) : (w < 1 << 6 ? 6 : 7)))
            : (w < 1 << 11 ? (w < 1 << 9 ? (w < 1 << 8 ? 8 : 9) : (w < 1 << 10 ? 10 : 11))
            : (w < 1 << 13 ? (w < 1 << 12 ? 12 : 13) : (w < 1 << 14 ? 14 : 15))))
            : (w < 1 << 23
            ? (w < 1 << 19 ? (w < 1 << 17 ? (w < 1 << 16 ? 16 : 17) : (w < 1 << 18 ? 18 : 19))
            : (w < 1 << 21 ? (w < 1 << 20 ? 20 : 21) : (w < 1 << 22 ? 22 : 23)))
            : (w < 1 << 27
            ? (w < 1 << 25 ? (w < 1 << 24 ? 24 : 25) : (w < 1 << 26 ? 26 : 27))
            : (w < 1 << 29 ? (w < 1 << 28 ? 28 : 29)
            : (w < 1 << 30 ? 30 : 31)))));
        }

        public static int BitLength(byte b)
        {
            return b < 8 ? b < 2 ? b < 1 ? 0 : 1 : b < 4 ? 2 : 3 : b < 32
                 ? b < 16 ? 4 : 5 : b < 64 ? 6 : 7;
        }

        //public static int BitLength(XInt n)
        //{
        //    byte[] bytes = (n.Sign * n).ToByteArray();
        //    int i = bytes.Length - 1;
        //    return (i << 3) + BitLength(bytes[i]);
        //}

        /// <summary>
        /// High bound for number of primes not exeeding n.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int PiHighBound(long n)
        {
            if (n < 17)
            {
                return 6;
            }
            return (int)System.Math.Floor(((double)n)
                                          / (System.Math.Log(n) - 1.5));
        }

        /// <summary>
        /// Logarithm to base 10.
        /// </summary>
        /// <param name="value"></param>
        /// <returns><tt>Log<sub>10</sub>value</tt>.</returns>
        //public static double Log10(double value)
        //{
        //    return System.Math.Log(value) * 0.43429448190325176;
        //}

        /// <summary>
        /// Logarithm to base 2.
        /// </summary>
        /// <param name="value"></param>
        /// <returns><tt>Log<sub>2</sub>value</tt>.</returns>
        public static double Log2(double value)
        {
            return System.Math.Log(value) * 1.4426950408889634;
        }

        /// <summary>
        /// Floor of the binary logarithm.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int FloorLog2(int n)
        {
            if (n <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n) + " >= 0 required");
            }
            return BitLength(n) - 1;
        }

        /// <summary>
        /// Floor of the square root.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int FloorSqrt(int n)
        {
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(n) + " >= 0 required");
            }
            return (int)System.Math.Floor(System.Math.Sqrt(n));
        }

        /// <summary>
        /// Ceiling of the binary logarithm.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int CeilLog2(int n)
        {
            int ret = FloorLog2(n);
            if (n != (1 << ret)) ret++;
            return ret;
        }

        /// <summary>
        /// Berechnung der groessten Zahl, deren Quadrat n nicht uebersteigt.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int Sqrt(int n)
        {
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(n) + " >= 0 required");
            }

            if (n == 0) return 0;

            int unten, oben = BitLength(n);

            do
            {
                unten = n / oben;
                oben += unten;
                oben >>= 1;
            }
            while (oben > unten);

            return oben;
        }

        public static int OmegaSwingHighBound(int n)
        {
            if (n < 4)
            {
                return 6;
            }
            return (int)System.Math.Floor(System.Math.Sqrt(n)
                                          + (double)n / (Log2(n) - 1));
        }

        public static double AsymptFactorial(double x)
        {
            // error of order O(x^-5)
            // double ln2Pi = 1.8378770664093455 = Math.log(2 * Math.PI);
            double a = x + x + 1;
            return (1.8378770664093455 + System.Math.Log(a / 2) * a - a
                    - (1 - 7 / (30 * a * a)) / (6 * a)) / 2;
        }

        public static string AsymptFactorial(int x)
        {
            // error of order O(x^-5)
            return Exp(AsymptFactorial((double)x));
        }

        public static long ExactDecimalDigitsPerMillisecond(int n, long ms)
        {
            double x = AsymptFactorial((double)n);
            double l = x * 0.43429448190325176D; // x/Math.log(10);
            double lsd = ms == 0 ? l : l / (double)ms;
            return (long)Math.Truncate(0.5 + lsd);
        }

        public static string Exp(double x)
        {
            double l = x * 0.43429448190325176D; // x/Math.log(10);
            double e = System.Math.Floor(l);
            double m = System.Math.Pow(10, l - e);
            string mat = (Convert.ToString(m)).Substring(0, 6);
            return mat + " E+" + Convert.ToString((int)e);
        }

        public static double AsymptSwingingFactorial(double x)
        {
            // TODO: not yet implemented
            return 1;
        }

        public static void ClearOpCounter()
        {
            // --
        }
        public static long[] GetOpCounts()
        {
            return new long[] { 1 };
        }

        private static long[] smallFactorials = {
        1, 1, 2, 6, 24, 120, 720, 5040, 40320, 362880, 3628800, 39916800, 
        479001600, 6227020800, 87178291200, 1307674368000, 20922789888000, 
        355687428096000, 6402373705728000, 121645100408832000, 2432902008176640000 };

        public static XInt Factorial(int n)
        {
            if (n > 20)
            {
                throw new System.ArgumentOutOfRangeException("max n is 20 but was " + n.ToString());
            }
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException("min " + nameof(n) + " is 0");
            }

            return new XInt(smallFactorials[n]);
        }

        const int ParallelThreshold = 1024;

        // <returns>a[start]*a[start+1]*...*a[start+length-1]</returns>
        public static XInt Product(int[] a, int start, int length)
        {
            if (length == 0) return XInt.One;

            int len = (length + 1) / 2;
            long[] b = new long[len];

            int i, j, k;

            for (k = 0, i = start, j = start + length - 1; i < j; i++, k++, j--)
            {
                b[k] = a[i] * (long)a[j];
            }

            if (i == j) b[k++] = a[j];

            if (k > ParallelThreshold)
            {
                var task = Task.Factory.StartNew<XInt>(() =>
                {
                    return RecProduct(b, (k - 1) / 2 + 1, k - 1);
                });

                var left = RecProduct(b, 0, (k - 1) / 2);
                var right = task.Result;
                return left * right;
            }

            return RecProduct(b, 0, k - 1);
        }

        public static XInt Product(int[] a, int start, int length, int increment)
        {
            if (length == 0) return XInt.One;

            int len = (1 + (length + 1) / 2) / increment;
            long[] b = new long[len];

            int i, k = 0;
            bool toggel = false;

            for (i = start; i < start+length; i += increment)
            {
                if ((toggel = !toggel)) b[k] = a[i];
                else b[k++] *= (long)a[i];
            }
       
            if (len > ParallelThreshold)
            {
                var task = Task.Factory.StartNew<XInt>(() =>
                {
                    return RecProduct(b, (len - 1) / 2 + 1, len - 1);
                });

                var left = RecProduct(b, 0, (len - 1) / 2);
                var right = task.Result;
                return left * right;
            }

            return RecProduct(b, 0, len - 1);
        }

        public static XInt Product(int[] a)
        {
            return Product(a, 0, a.Length);
        }

        public static XInt Product(long[] a, int len)
        {
            int n = len - 1;
            if (len > ParallelThreshold)
            {
                var task = Task.Factory.StartNew<XInt>(() =>
                {
                    return RecProduct(a, n / 2 + 1, n);
                });

                var left = RecProduct(a, 0, n / 2);
                var right = task.Result;
                return left * right;
            }

            return RecProduct(a, 0, n);
        }

        public static XInt Product(long[] a)
        {
            return Product(a, a.Length);
        }

        public static XInt RecProduct(long[] s, int n, int m)
        {
            if (n > m)
            {
                return XInt.One;
            }
            if (n == m)
            {
                return new XInt(s[n]);
            }

            int k = (n + m) >> 1;
            return RecProduct(s, n, k) * RecProduct(s, k + 1, m);
        }

        private static XInt BigRecProduct(XInt[] s, int n, int m)
        {
            if (n > m)
            {
                return XInt.One;
            }
            if (n == m)
            {
                return s[n];
            }

            int k = (n + m) >> 1;
            return BigRecProduct(s, n, k) * BigRecProduct(s, k + 1, m);
        }

        public static XInt Product(XInt[] a, int start, int len)
        {
            int n = len - 1;
            if (len > ParallelThreshold)
            {
                var task = Task.Factory.StartNew<XInt>(() =>
                {
                    return BigRecProduct(a, start + n / 2 + 1, start + n);
                });

                var left = BigRecProduct(a, start, start + n / 2);
                return left * task.Result;
            }

            return BigRecProduct(a, start, n);
        }

        public static XInt Product(XInt[] a)
        {
            return BigRecProduct(a, 0, a.Length - 1);
        }

    } 
} 


//static public class MathFun
//{
//    // Calibrate the treshhold
//    private const int THRESHOLD_PRODUCT_SERIAL = 128;

//    //static public XInt ProductSerial(long[] seq, int start, int len)
//    //{
//    //    var prod = new XInt(seq[start]);
//    //    for (int i = start + 1; i < start + len; i++)
//    //    {
//    //        prod *= seq[i];
//    //    }
//    //    return prod;
//    //}

//    static public XInt Product(long[] seq, int start, int len)
//    {
//        if (len <= THRESHOLD_PRODUCT_SERIAL)
//        {
//            // return ProductSerial(seq, start, len);
//            var rprod = new XInt(seq[start]);

//            for (int i = start + 1; i < start + len; i++)
//            {
//                rprod *= seq[i];
//            }
//            return rprod;
//        }
//        else
//        {
//            int halfLen = len / 2;

//            Task<XInt> task = Task.Factory.StartNew(() => Product(seq, start, halfLen));
//            var rprod = Product(seq, start + halfLen, len - halfLen);
//            return task.Result * rprod;

//            // rprod = XInt.Zero; XInt lprod = XInt.Zero;
//            //Parallel.Invoke(
//            //    () => { rprod = Product(seq, start, halfLen); },
//            //    () => { lprod = Product(seq, start + halfLen, len - halfLen); }
//            //);
//            //rprod = lprod * rprod;
//        }
//    }

//    private MathFun() { }

// System.Diagnostics.Debug.Assert((0 <= start) & (length <= a.Length),
// System.Reflection.MethodBase.GetCurrentMethod().Name + " failed: range not subrange of a");
//}

