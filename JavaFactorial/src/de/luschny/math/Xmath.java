// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math;

import de.luschny.math.arithmetic.Xint;

/**
 * Class declaration
 * 
 * @author
 * @version %I%, %G%
 */
public final class Xmath {

    /**
     * Method declaration
     * 
     * @param w
     * @return
     */
    public static int bitCount(int w) {
        w -= (0xaaaaaaaa & w) >>> 1;
        w = (w & 0x33333333) + ((w >>> 2) & 0x33333333);
        w = w + (w >>> 4) & 0x0f0f0f0f;
        w += w >>> 8;
        w += w >>> 16;

        return w & 0xff;
    }

    /**
     * Method declaration
     * 
     * @param w
     * @return
     */
    // Binaeres Suchen - Entscheidungsbaum (5 Tests, selten 6)
    public static int bitLen(int w) {
        return w < 1 << 15 ? (w < 1 << 7 ? (w < 1 << 3 ? (w < 1 << 1 ? (w < 1 << 0 ? (w < 0 ? 32 : 0) : 1)
                : (w < 1 << 2 ? 2 : 3)) : (w < 1 << 5 ? (w < 1 << 4 ? 4 : 5) : (w < 1 << 6 ? 6 : 7)))
                : (w < 1 << 11 ? (w < 1 << 9 ? (w < 1 << 8 ? 8 : 9) : (w < 1 << 10 ? 10 : 11))
                : (w < 1 << 13 ? (w < 1 << 12 ? 12 : 13) : (w < 1 << 14 ? 14 : 15))))
                : (w < 1 << 23 ? (w < 1 << 19 ? (w < 1 << 17 ? (w < 1 << 16 ? 16 : 17) : (w < 1 << 18 ? 18 : 19))
                : (w < 1 << 21 ? (w < 1 << 20 ? 20 : 21) : (w < 1 << 22 ? 22 : 23)))
                : (w < 1 << 27 ? (w < 1 << 25 ? (w < 1 << 24 ? 24 : 25) : (w < 1 << 26 ? 26 : 27))
                : (w < 1 << 29 ? (w < 1 << 28 ? 28 : 29) : (w < 1 << 30 ? 30 : 31))));
    }
    /**
     * Method declaration
     * 
     * @param n
     * @return
     * @throws IllegalArgumentException
     *             <tt>20 &lt; n</tt>.
     */
    private static long[] smallFactorials = {1, 1, 2, 6, 24, 120, 720, 5040, 40320, 362880, 3628800, 39916800, 479001600,
        6227020800L, 87178291200L, 1307674368000L, 20922789888000L, 355687428096000L,
        6402373705728000L, 121645100408832000L, 2432902008176640000L};

    public static Xint Factorial(int n) {
        if (n > 19) {
            throw new IllegalArgumentException("n < 20 required");
        }
        if (n < 0) {
            throw new IllegalArgumentException("0 <= n required");
        }

        return Xint.valueOf(smallFactorials[n]);
    }

    /**
     * status :: hack
     * 
     * @param n
     * @return
     */
    public static int piHighBound(long n) {
        if (n < 17) {
            return 6;
        }
        return (int) Math.floor(n / (Math.log(n) - 1.5));
    }

    /**
     * @param n
     * @return
     */
    public static long naiveSwingFactorial(int n) {
        if (n < 1) {
            return 1;
        }

        if ((n & 1) == 1) {
            return naiveSwingFactorial(n - 1) * n;
        }

        return (naiveSwingFactorial(n - 1) << 2) / n;
    }

    /**
     * @param value
     * @return <tt>log<sub>10</sub>value</tt>.
     */
    public static double log10(double value) {
        return Math.log(value) * 0.43429448190325176;
    }

    /**
     * @param value
     * @return <tt>log<sub>2</sub>value</tt>.
     */
    public static double log2(double value) {
        return Math.log(value) * 1.4426950408889634;
    }

    /**
     * @param n
     * @return (int) floor(log[2](n))
     */
    public static int floorLog2(int n) {
        if (n <= 0) {
            throw new IllegalArgumentException("n > 0 required");
        }
        return bitLen(n) - 1;
    }

    /**
     * @param n
     * @return (int) ceil(log[2](n))
     */
    public static int ceilLog2(int n) {
        int ret = floorLog2(n);
        if (n != (1 << ret)) {
            ret++;
        }
        return ret;
    }

    /**
     * Berechnung der groessten Zahl, deren Quadrat n nicht uebersteigt.
     * 
     * @param n
     * @return (int) floor(sqrt(n))
     */
    public static int sqrt(int n) {
        if (n < 0) {
            throw new IllegalArgumentException("arg >= 0 required");
        }
        if (n == 0) {
            return 0;
        }

        int unten, oben = bitLen(n);

        do {

            unten = n / oben;
            oben += unten;
            oben >>= 1;
        } while (oben > unten);

        return oben;
    }

    /**
     * Method declaration
     * 
     * @param n
     * @return
     */
    public static int omegaSwingHighBound(int n) {
        if (n < 4) {
            return 6;
        }

        return (int) Math.floor(Math.sqrt(n) + n / (log2(n) - 1.0));
    }

    /**
     * @param x
     * @return
     */
    public static double asymptFactorial(double x) {
        // asymptotic error of order O(x^-5)
        // double ln2Pi = 1.8378770664093455 = Math.log(2 * Math.PI);
        double a = x + x + 1;
        return (1.8378770664093455 + Math.log(a / 2) * a - a - (1 - 7 / (30 * a * a)) / (6 * a)) / 2;
    }

    /**
     * @param x
     * @return
     */
    public static String asymptFactorial(int x) {
        // error of order O(x^-5)
        return exp(asymptFactorial((double) x));
    }

    /**
     * @param x
     * @return
     */
    public static String exp(double x) {
        double l = x * 0.43429448190325176; // x/Math.log(10);
        double e = Math.floor(l);
        double m = Math.pow(10, l - e);
        String mat = (Double.toString(m)).substring(0, 6);
        return mat + " E+" + Integer.toString((int) e);
    }

    /**
     * @param x
     * @return
     */
    public static double asymptSwingingFactorial(double x) {
        // not yet implemented
        return x * 0;
    }

    public static long gcd(long a, long b) {
        long x, y;

        if (a < 0) {
            a = -a;
        }
        if (b < 0) {
            b = -b;
        }

        if (a >= b) {
            x = a;
            y = b;
        } else {
            x = b;
            y = a;
        }

        while (y != 0) {
            long t = x % y;
            x = y;
            y = t;
        }
        return x;
    }

    public static double round(double d, int scale, int mode) {
        java.math.BigDecimal bd = new java.math.BigDecimal(Double.toString(d));
        return (bd.setScale(scale, mode)).doubleValue();
    }

    public static double round2(double d) {
        return round(d, 2, java.math.BigDecimal.ROUND_HALF_UP);
    }

    /**
     * We don't need a constructor.
     */
    private Xmath() {
    }
}
// endOfXmath
