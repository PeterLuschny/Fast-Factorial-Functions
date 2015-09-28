// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01


namespace Sharith.Arithmetic
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Wrapper for MPIR's mpz_t type.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct MpzT
    {
        internal Int32 _mp_alloc;
        internal Int32 _mp_size;
        internal IntPtr ptr;
    }

    /// <summary>
    /// Mini-Wrapper for some of MPIR's BigInt functions.
    /// </summary>
    public class XInt
    {
        private MpzT impl;

        public static readonly XInt One = new XInt(1);
        public static readonly XInt Zero = new XInt(0);
        public static readonly XInt MinusOne = new XInt(-1);

        ~XInt()
        {
            mpz_clear(ref impl);
        }

        public object Clone()
        {
            return new XInt(this);
        }

        public XInt()
        {
            mpz_init(ref impl);
        }

        public XInt(Int32 value)
        {
            mpz_init_set_si(ref impl, value);
        }

        public XInt(UInt32 value)
        {
            mpz_init_set_ui(ref impl, value);
        }

        public XInt(Int64 value)
        {
            if (value > 0)
            {
                if (value <= UInt32.MaxValue)
                {
                    mpz_init_set_ui(ref this.impl, (UInt32)value);
                }
                else
                {
                    mpz_init_set_ui(ref this.impl, (UInt32)(value >> 32));
                    mpz_mul_2exp(ref this.impl, ref this.impl, 32);
                    mpz_add_ui(ref this.impl, ref this.impl, (UInt32)value);
                }
            }
            else if (value == 0)
            {
                mpz_init(ref this.impl);
            }
            else // value < 0
            {
                var absValue = (Int64)(-value);

                if (absValue <= UInt32.MaxValue)
                {
                    mpz_init_set_si(ref this.impl, (Int32)absValue);
                }
                else
                {
                    mpz_init_set_si(ref this.impl, (Int32)(absValue >> 32));
                    mpz_mul_2exp(ref this.impl, ref this.impl, 32);
                    mpz_add_ui(ref this.impl, ref this.impl, (UInt32)absValue);
                }
            }
        }

        public XInt(UInt64 value)
        {
            if (value == 0)
            {
                mpz_init(ref this.impl);
            }
            else
            {
                if (value <= UInt32.MaxValue)
                {
                    mpz_init_set_ui(ref this.impl, (UInt32)value);
                }
                else
                {
                    mpz_init_set_ui(ref this.impl, (UInt32)(value >> 32));
                    mpz_mul_2exp(ref this.impl, ref this.impl, 32);
                    mpz_add_ui(ref this.impl, ref this.impl, (UInt32)value);
                }
            }
        }

        public XInt(XInt value)
        {
            if (ReferenceEquals(value, null))
                throw new NullReferenceException();

            mpz_init(ref impl);
            mpz_set(ref impl, ref value.impl);
        }

        public XInt(String value)
        {
            // marshal string
            IntPtr unmanagedString = Marshal.StringToHGlobalAnsi(value);
            mpz_init_set_str(ref impl, unmanagedString, 10);
            // free unmanaged space
            Marshal.FreeHGlobal(unmanagedString);
        }

        public Int32 IntValue
        {
            get { return mpz_get_si(ref impl); }
            set { mpz_set_si(ref impl, value); }
        }

        public Int32 CompareTo(Int32 other)
        {
            return mpz_cmp_si(ref impl, other);
        }

        public Int32 CompareTo(XInt other)
        {
            return mpz_cmp(ref impl, ref other.impl);
        }

        public bool Equals(XInt other)
        {
            if (object.ReferenceEquals(other, null))
                return false;

            return mpz_cmp(ref other.impl, ref this.impl) == 0;
        }

        public static XInt operator %(XInt x, XInt mod)
        {
            var result = new XInt();
            mpz_mod(ref result.impl, ref x.impl, ref mod.impl);
            return result;
        }

        public static XInt operator *(XInt x, XInt y)
        {
            var result = new XInt();
            mpz_mul(ref result.impl, ref x.impl, ref y.impl);
            return result;
        }

        public static XInt operator *(Int32 x, XInt y)
        {
            var z = new XInt();
            mpz_mul_si(ref z.impl, ref y.impl, x);
            return z;
        }

        public static XInt operator *(XInt x, Int32 y)
        {
            var z = new XInt();
            mpz_mul_si(ref z.impl, ref x.impl, y);
            return z;
        }

        public static XInt operator *(UInt32 x, XInt y)
        {
            var z = new XInt();
            mpz_mul_ui(ref z.impl, ref y.impl, x);
            return z;
        }

        public static XInt operator *(XInt x, UInt32 y)
        {
            var z = new XInt();
            mpz_mul_ui(ref z.impl, ref x.impl, y);
            return z;
        }

        public static XInt operator +(XInt x, XInt y)
        {
            var result = new XInt();
            mpz_add(ref result.impl, ref x.impl, ref y.impl);
            return result;
        }

        public static XInt operator +(XInt x, UInt32 y)
        {
            var result = new XInt();
            mpz_add_ui(ref result.impl, ref x.impl,  y);
            return result;
        }

        public static XInt operator -(XInt x, XInt y)
        {
            var result = new XInt();
            mpz_sub(ref result.impl, ref x.impl, ref y.impl);
            return result;
        }

        public static XInt operator /(XInt x, XInt y)
        {
            var result = new XInt();
            mpz_tdiv_q(ref result.impl, ref x.impl, ref y.impl);
            return result;
        }

        public static XInt operator <<(XInt x, Int32 shiftAmount)
        {
            var z = new XInt();
            mpz_mul_2exp(ref z.impl, ref x.impl, (UInt32)shiftAmount);
            return z;
        }

        public static bool operator <(XInt x, XInt y)
        {
            return mpz_cmp(ref x.impl, ref y.impl) < 0;
        }

        public static bool operator >(XInt x, XInt y)
        {
            return mpz_cmp(ref x.impl, ref y.impl) > 0;
        }

        public static bool operator >=(XInt x, XInt y)
        {
            return mpz_cmp(ref x.impl, ref y.impl) >= 0;
        }

        public static bool operator <=(XInt x, XInt y)
        {
            return mpz_cmp(ref x.impl, ref y.impl) <= 0;
        }

        public static bool operator ==(XInt x, XInt y)
        {
            return mpz_cmp(ref x.impl, ref y.impl) == 0;
        }

        public static bool operator !=(XInt x, XInt y)
        {
            return mpz_cmp(ref x.impl, ref y.impl) != 0;
        }
        
        public bool IsEven()
        {
            if(this.impl._mp_size == 0) return true;
            unsafe
            {
                var p = (Int32*)this.impl.ptr;
                return (*p & 1) == 0;
            }
        }

        public static XInt operator ++(XInt op)
        {
            return op + One;
        }

        public static XInt operator --(XInt op)
        {
            return op + MinusOne;
        }

        public override Int32 GetHashCode()
        {
            Int32 hash = 0;
            Int32 count = this.impl._mp_size;
            count = count >= 0 ? count : -count;

            unsafe
            {
                var p = (Int32*)this.impl.ptr;

                while (count-- > 0)
                {
                    hash ^= *p;
                    ++p;
                }
            }

            return hash;
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, null)) return false;
            if (this.GetType() != obj.GetType()) return false;
            var intObj = (XInt)obj; 
            return mpz_cmp(ref intObj.impl, ref this.impl) == 0;
        }

        public static XInt Sqrt(XInt i)
        {
            var result = new XInt();
            mpz_sqrt(ref result.impl, ref i.impl);
            return result;
        }

        public XInt Sqrt()
        {
            return Sqrt(this);
        }

        public static XInt Pow(XInt bas, UInt32 exp)
        {
            var result = new XInt();
            mpz_pow_ui(ref result.impl, ref bas.impl, exp);
            return result;
        }

        public static XInt Pow(XInt bas, Int32 exp)
        {
            if (exp == 2) return bas * bas;
            if (exp >= 0) return Pow(bas, (UInt32)exp);
            throw new ArgumentOutOfRangeException(nameof(exp));
        }

        public static XInt Pow(UInt32 bas, UInt32 exp)
        {
            var result = new XInt();
            mpz_ui_pow_ui(ref result.impl, bas, exp);
            return result;
        }

        public XInt PowMod(XInt exp, XInt mod)
        {
            var result = new XInt();
            mpz_powm(ref result.impl, ref this.impl, ref exp.impl, ref mod.impl);
            return result;
        }

        public XInt Pow(Int32 n)
        {
            return Pow(this, n);
        }

        public static XInt GreatestCommonDivisor(XInt x, XInt y)
        {
            var z = new XInt();
            mpz_gcd(ref z.impl, ref x.impl, ref y.impl);
            return z;
        }

        public string ToString(IFormatProvider provider)
        {
            return ToString();
        }

        public Int32 SizeInBase(Int32 basis)
        {
            return mpz_sizeinbase(ref impl, basis);
        }

        public static implicit operator XInt(Int64 value)
        {
            return new XInt(value);
        }

        public static implicit operator XInt(UInt64 value)
        {
            return new XInt(value);
        }

        public static implicit operator XInt(Int32 value)
        {
            return new XInt(value);
        }

        public static implicit operator XInt(UInt32 value)
        {
            return new XInt(value);
        }

        public static XInt Factorial(Int32 x)
        {
            var z = new XInt();
            mpz_fac_ui(ref z.impl, (UInt32)x);
            return z;
        }
        
        #region DLL imports

        private const string Mpir = "mpir.dll";

        [DllImport(Mpir, EntryPoint = "__gmpz_init")]
        private static extern void mpz_init(ref MpzT value);

        [DllImport(Mpir, EntryPoint = "__gmpz_init_set_si")]
        private static extern void mpz_init_set_si(ref MpzT value, Int32 v);

        [DllImport(Mpir, EntryPoint = "__gmpz_init_set_ui")]
        private static extern void mpz_init_set_ui(ref MpzT rop, UInt32 v);

        [DllImport(Mpir, EntryPoint = "__gmpz_init_set_str")]
        private static extern Int32 mpz_init_set_str(ref MpzT rop, IntPtr s, Int32 basis);

        [DllImport(Mpir, EntryPoint = "__gmpz_clear")]
        private static extern void mpz_clear(ref MpzT src);

        [DllImport(Mpir, EntryPoint = "__gmpz_mul_si")]
        private static extern void mpz_mul_si(ref MpzT dest, ref MpzT src, Int32 val);

        [DllImport(Mpir, EntryPoint = "__gmpz_mul_ui")]
        private static extern void mpz_mul_ui(ref MpzT dest, ref MpzT src, UInt32 val);

        [DllImport(Mpir, EntryPoint = "__gmpz_mul")]
        private static extern void mpz_mul(ref MpzT dest, ref MpzT x, ref MpzT y);

        [DllImport(Mpir, EntryPoint = "__gmpz_add")]
        private static extern void mpz_add(ref MpzT dest, ref MpzT src, ref MpzT src2);

        [DllImport(Mpir, EntryPoint = "__gmpz_add_ui")]
        private static extern void mpz_add_ui(ref MpzT dest, ref MpzT src, UInt32 val);

        [DllImport(Mpir, EntryPoint = "__gmpz_tdiv_q")]
        private static extern void mpz_tdiv_q(ref MpzT dest, ref MpzT src, ref MpzT src2);

        [DllImport(Mpir, EntryPoint = "__gmpz_set")]
        private static extern void mpz_set(ref MpzT dest, ref MpzT src);

        [DllImport(Mpir, EntryPoint = "__gmpz_set_si")]
        private static extern void mpz_set_si(ref MpzT src, Int32 value);

        [DllImport(Mpir, EntryPoint = "__gmpz_set_str")]
        private static extern Int32 mpz_set_str(ref MpzT rop, IntPtr s, Int32 sbase);

        [DllImport(Mpir, EntryPoint = "__gmpz_get_si")]
        private static extern Int32 mpz_get_si(ref MpzT src);

        [DllImport(Mpir, EntryPoint = "__gmpz_get_d")]
        private static extern double mpz_get_d(ref MpzT src);

        [DllImport(Mpir, EntryPoint = "__gmpz_get_str", CharSet = CharSet.Ansi)]
        private static extern IntPtr mpz_get_str(IntPtr outString, Int32 _base, ref MpzT src);

        [DllImport(Mpir, EntryPoint = "__gmpz_sizeinbase")]
        internal static extern Int32 mpz_sizeinbase(ref MpzT src, Int32 _base);

        [DllImport(Mpir, EntryPoint = "__gmpz_cmp")]
        private static extern Int32 mpz_cmp(ref MpzT x, ref MpzT y);

        [DllImport(Mpir, EntryPoint = "__gmpz_cmp_d")]
        private static extern Int32 mpz_cmp_d(ref MpzT x, double y);

        [DllImport(Mpir, EntryPoint = "__gmpz_cmp_si")]
        private static extern Int32 mpz_cmp_si(ref MpzT x, Int32 y);

        [DllImport(Mpir, EntryPoint = "__gmpz_sub")]
        private static extern void mpz_sub(ref MpzT rop, ref MpzT x, ref MpzT y);

        [DllImport(Mpir, EntryPoint = "__gmpz_sqrt")]
        private static extern void mpz_sqrt(ref MpzT rop, ref MpzT op);

        [DllImport(Mpir, EntryPoint = "__gmpz_pow_ui")]
        private static extern void mpz_pow_ui(ref MpzT rop, ref MpzT op, UInt32 exp);

        [DllImport(Mpir, EntryPoint = "__gmpz_powm")]
        private static extern void mpz_powm(ref MpzT rop, ref MpzT bas, ref MpzT exp, ref MpzT mod);

        [DllImport(Mpir, EntryPoint = "__gmpz_mod")]
        private static extern void mpz_mod(ref MpzT rop, ref MpzT x, ref MpzT mod);

        [DllImport(Mpir, EntryPoint = "__gmpz_gcd")]
        private static extern void mpz_gcd(ref MpzT rop, ref MpzT op1, ref MpzT op2);

        [DllImport(Mpir, EntryPoint = "__gmpz_ui_pow_ui")]
        private static extern void mpz_ui_pow_ui(ref MpzT rop, UInt32 bas, UInt32 exp);

        [DllImport(Mpir, EntryPoint = "__gmpz_invert")]
        private static extern Int32 mpz_invert(ref MpzT rop, ref MpzT x, ref MpzT y);

        [DllImport(Mpir, EntryPoint = "__gmpz_mul_2exp")]
        private static extern Int32 mpz_mul_2exp(ref MpzT rop, ref MpzT x, UInt32 shift);

        [DllImport(Mpir, EntryPoint = "__gmpz_fac_ui")]
        private static extern void mpz_fac_ui(ref MpzT rop, UInt32 op);

        #endregion

        private const string Ffflib = "FastFactorialLibrary.dll";

        [DllImport(Ffflib, EntryPoint = "PrimeSwing::ParallelFactorial")]
        private static extern void _PrimeSwingParallelFactorial(ref MpzT fact, ulong n);

        [DllImport(Ffflib, EntryPoint = "Schoenhage::ParallelFactorial")]
        private static extern void _SchoenhageParallelFactorial(ref MpzT fact, ulong n);

        public static XInt PrimeSwingParallelFactorial(Int32 x)
        {
            var z = new XInt();
            _PrimeSwingParallelFactorial(ref z.impl, (UInt32)x);
            return z;
        }

        public static XInt SchoenhageParallelFactorial(Int32 x)
        {
            var z = new XInt();
            _SchoenhageParallelFactorial(ref z.impl, (UInt32)x);
            return z;
        }
    }
}
