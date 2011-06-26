// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.arithmetic;

import org.apfloat.Apint;
import org.apfloat.ApintMath;

public class Xint {

    private static final Counter cnt;
    private static final int radix = 2;
    public static final Xint ONE, ZERO;

    static {
        ZERO = new Xint(0);
        ONE = new Xint(1);
        cnt = new Counter();
    }

    public final String getName() {
        return "Arithmetic: Apfloat 1.5.2";
    }
    private Apint api;

    private Xint(long n) {
        api = new Apint(n, radix);
    }

    public Xint(Apint n) {
        api = n;
    }

    public static Xint valueOf(long n) {
        return new Xint(n);
    }

    public Xint add(Xint b) {
        cnt.ADD++;
        return new Xint(api.add(b.api));
    }

    public Xint add(long b) {
        cnt.add++;
        return new Xint(api.add(new Apint(b, radix)));
    }

    public Xint subtract(Xint a) {
        cnt.SUB++;
        return new Xint(api.subtract(a.api));
    }

    public Xint subtract(long b) {
        cnt.sub++;
        return new Xint(api.subtract(new Apint(b, radix)));
    }

    public Xint multiply(Xint b) {
        cnt.MUL++;
        return new Xint(api.multiply(b.api));
    }

    public Xint multiply(long b) {
        cnt.mul++;
        return new Xint(api.multiply(new Apint(b, radix)));
    }

    public Xint square() {
        cnt.sqr++;
        return new Xint(api.multiply(api));
    }

    public Xint divide(Xint a) {
        cnt.DIV++;
        return new Xint(api.divide(a.api));
    }

    public Xint divide(long b) {
        cnt.div++;
        return new Xint(api.divide(new Apint(b, radix)));
    }

    public int signum() {
        return api.signum();
    }

    public Xint toPowerOf(int n) {
        cnt.sqr++;
        return new Xint(ApintMath.pow(api, n));
    }

    public Xint shiftLeft(int b) {
        cnt.lsh++;
        return new Xint(ApintMath.scale(api, b));
    }

    public Xint ONE() {
        return ONE;
    }

    public Xint ZERO() {
        return ZERO;
    }

    public boolean isONE() {
        return api.intValue() == 1;
    }

    public boolean isZERO() {
        return api.intValue() == 0;
    }

    public long crcValue() {
        return 0xffffffffL & api.hashCode();
    }

    @Override
    public String toString() {
        return api.toRadix(10).toString();
    }

    public String toHexString() {
        return api.toRadix(16).toString();
    }

    public int compareTo(Xint b) {
        return api.compareTo(b.api);
    }

    public Xint gcd(Xint b) {
        return new Xint(ApintMath.gcd(api, b.api));
    }

    public Xint multiply(int[] a, int length) {
        return this.multiply(product(a, 0, length));
    }

    // <returns>a[start]*a[start+1]*...*a[start+length-1]</returns>
    public static Xint product(int[] a, int start, int length) {
    // Assert((0 <= start) & (length <= a.length),

        if (length == 0) {
            return Xint.ONE;
        }

        int len = (length + 1) / 2;
        long[] b = new long[len];

        int i, j, k;

        for (k = 0, i = start, j = start + length - 1; i < j; i++, k++, j--) {
            b[k] = a[i] * (long) a[j];
        }

        if (i == j) {
            b[k++] = a[j];
        }

        // Assert(k > 0)

        // if(k > PARALLEL_THRESHOLD)
        // {
        // var pro = Task.Factory.StartNew<Xint>(() =>
        // {
        // return RecProduct(b, (k - 1) / 2 + 1, k - 1);
        // });
        //
        // var left = RecProduct(b, 0, (k - 1) / 2);
        // var right = pro.Result;
        // return left * right;
        // }

        return recProduct(b, 0, k - 1);
    }

    public static Xint product(int[] a) {
        return product(a, 0, a.length);
    }

    public static Xint product(long[] a) {
        int n = a.length;

        // if (n > PARALLEL_THRESHOLD)
        // {
        // var pro = Task.Factory.StartNew<Xint>(() =>
        // {
        // return recProduct(a, (n - 1) / 2 + 1, n - 1);
        // });
        //
        // var left = RecProduct(a, 0, (n - 1) / 2);
        // var right = pro.Result;
        // return left * right;
        // }

        return recProduct(a, 0, n - 1);
    }

    private static Xint recProduct(long[] s, int n, int m) {
        if (n > m) {
            return Xint.ONE;
        }
        if (n == m) {
            return new Xint(s[n]);
        }

        int k = (n + m) >> 1;
        return recProduct(s, n, k).multiply(recProduct(s, k + 1, m));
    }

    private static Xint bigRecProduct(Xint[] s, int n, int m) {
        if (n > m) {
            return Xint.ONE;
        }
        if (n == m) {
            return s[n];
        }

        int k = (n + m) >> 1;
        return bigRecProduct(s, n, k).multiply(bigRecProduct(s, k + 1, m));
    }

    public static Xint product(Xint[] a, int start, int length) {
    // Assert((0 <= start) & (length <= a.Length),

        return bigRecProduct(a, start, length - 1);
    }

    public static Xint product(Xint[] a) {
        return bigRecProduct(a, 0, a.length - 1);
    }

    // =========== OperationCounter ===========================
    public static void clearOpCounter() {
        cnt.clearOpCounter();
    }

    public static int[] getOpCounts() {
        return cnt.getOpCounts();
    }

    public static String getOpCountsAsString() {
        return cnt.toString();
    }
} // endOfLargeInteger

class Counter {

    public int ADD;
    public int add;
    public int SUB;
    public int sub;
    public int MUL;
    public int mul;
    public int DIV;
    public int div;
    public int lsh;
    public int rsh;
    public int sqr;
    public int neu;
    public boolean flag;

    public void clearOpCounter() {
        ADD = add = SUB = sub = MUL = mul = DIV = div = sqr = lsh = rsh = neu = 0;
        flag = false;
    }

    public int[] getOpCounts() {
        return new int[]{MUL, mul, DIV, div, sqr, lsh};
    }

    @Override
    public String toString() {
        StringBuilder sb = new StringBuilder();
        if (ADD != 0) {
            sb.append(ADD).append(" ADD, ");
        }
        if (add != 0) {
            sb.append(add).append(" add, ");
        }
        if (SUB != 0) {
            sb.append(SUB).append(" SUB, ");
        }
        if (sub != 0) {
            sb.append(sub).append(" sub, ");
        }
        if (MUL != 0) {
            sb.append(MUL).append(" MUL, ");
        }
        if (mul != 0) {
            sb.append(mul).append(" mul, ");
        }
        if (DIV != 0) {
            sb.append(DIV).append(" DIV, ");
        }
        if (div != 0) {
            sb.append(div).append(" div, ");
        }
        if (sqr != 0) {
            sb.append(sqr).append(" sqr, ");
        }
        if (lsh != 0) {
            sb.append(lsh).append(" lsh, ");
        }
        if (rsh != 0) {
            sb.append(rsh).append(" rsh, ");
        }
        if (neu != 0) {
            sb.append(neu).append(" new. ");
        }
        return sb.toString();
    }
}
