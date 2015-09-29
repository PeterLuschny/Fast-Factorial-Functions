// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de

/* The Poor Man's Implementation of the factorial.
 * All math is on board, no additional libraries
 * are needed. Good enough to compute the factorial
 * up to n=10000 in a few seconds.
 */
package de.luschny.math.factorial;

public class FactorialPoorMans {

    public FactorialPoorMans() {
    }
    private long N;

    public String factorial(int n) {
        if (n < 0) {
            throw new ArithmeticException("Factorial: n has to be >= 0, but was " + n);
        }

        if (n < 2) {
            return "1";
        }

        DecInteger p = new DecInteger(1);
        DecInteger r = new DecInteger(1);

        N = 1;
        int h = 0, shift = 0, high = 1;
        int log2n = (int) Math.floor(Math.log(n) / Math.log(2));

        while (h != n) {
            shift += h;
            h = n >> log2n--;
            int len = high;
            high = (h & 1) == 1 ? h : h - 1;
            len = (high - len) / 2;

            if (len > 0) {
                p = p.multiply(product(len));
                r = r.multiply(p);
            }
        }

        r = r.multiply(DecInteger.pow2(shift));
        return r.toString();
    }

    private DecInteger product(int n) {
        int m = n / 2;
        if (m == 0) {
            return new DecInteger(N += 2);
        }
        if (n == 2) {
            return new DecInteger((N += 2) * (N += 2));
        }
        return product(n - m).multiply(product(m));
    }
} // endOfPoorMansFactorial

class DecInteger {

    private static final long mod = 100000000L;
    private final int[] digits;
    private final int digitsLength;

    public DecInteger(long value) {
        digits = new int[]{(int) value, (int) (value >>> 32)};
        digitsLength = 2;
    }

    private DecInteger(int[] digits, int length) {
        this.digits = digits;
        digitsLength = length;
    }

    static public DecInteger pow2(int e) {
        if (e < 31) {
            return new DecInteger((int) Math.pow(2, e));
        }
        return pow2(e / 2).multiply(pow2(e - e / 2));
    }

    public DecInteger multiply(DecInteger b) {
        int alen = this.digitsLength, blen = b.digitsLength;
        int clen = alen + blen;
        int[] digit = new int[clen];

        for (int i = 0; i < alen; i++) {
            long temp = 0;
            for (int j = 0; j < blen; j++) {
                temp = temp + ((long) this.digits[i]) * ((long) b.digits[j]) + digit[i + j];
                digit[i + j] = (int) (temp % mod);
                temp = temp / mod;
            }
            digit[i + blen] = (int) temp;
        }

        int k = clen - 1;
        while (digit[k] == 0) {
            k--;
        }

        return new DecInteger(digit, k + 1);
    }

    @Override
    public String toString() {
        StringBuilder sb = new StringBuilder(digitsLength * 10);
        sb = sb.append(digits[digitsLength - 1]);
        for (int j = digitsLength - 2; j >= 0; j--) {
            sb = sb.append(Integer.toString(digits[j] + (int) mod).substring(1));
        }
        return sb.toString();
    }
}
// public static void main (String[] arguments)
// {
// int n = 1000;
// if (arguments.length == 0)
// {
// System.out.println(
// "Please enter next time an argument. Computing 1000! now.");
// }
// else
// {
// n = Integer.parseInt(arguments[0]);
// }
//
// FactorialPoorMans f = new FactorialPoorMans();
// System.out.println(n + "! = " + f.factorial(n));
// }
