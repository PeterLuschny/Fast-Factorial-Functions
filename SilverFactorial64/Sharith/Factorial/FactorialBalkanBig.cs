// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2013-02-18

namespace Sharith.Factorial
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using XInt = Arithmetic.XInt;

    public class BalkanBig : IFactorialFunction
    {
        public string Name => "BalkanBig           ";

        public XInt Factorial(int n)
        {
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException(
                    this.Name + ": " + nameof(n) + " >= 0 required, but was " + n);
            }

            if (n < 7)
            {
                return (XInt)(new int[] { 1, 1, 2, 6, 24, 120, 720 })[n];
            }

            int i = 1, loop = n / 2;
            var f = new XInt[loop + (n & 1)];

            f[0] = loop;
            if ((n & 1) == 1)
            {
                f[loop] = n;
            }

            XInt s = loop, t;

            for (var inc = loop - 1; inc > 0; inc--)
            {
                s += inc;
                t = s;

                while (t.IsEven())
                {
                    loop++;
                    t /= 2;
                }

                f[i++] = t;
            }

            return SplitProduct(f) << loop;
        }

        static XInt SplitProduct(IReadOnlyList<XInt> f)
        {
            var last = f.Count - 1;

            var leftTask = Task.Factory.StartNew<XInt>(() => RecMul(f, 0, last / 2));
            var right = RecMul(f, last / 2 + 1, last);

            return leftTask.Result * right;
        }

        static XInt RecMul(IReadOnlyList<XInt> f, int n, int m)
        {
            if (m == n + 1)
            {
                return f[n] * f[m];
            }

            if (m == n + 2)
            {
                return f[n] * f[n + 1] * f[m];
            }

            int k = (n + m) / 2;

            return RecMul(f, n, k) * RecMul(f, k + 1, m);
        }

    } // endOfFactorialBalkanBig
}
