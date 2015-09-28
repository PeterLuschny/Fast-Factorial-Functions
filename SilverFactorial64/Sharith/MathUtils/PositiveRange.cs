// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace Sharith.MathUtils
{
    using System;

    /// <summary>
    /// a PositiveRange is an interval of integers,
    /// given by a lower bound Min and an upper bound Max,
    /// such that 0 &lt;= Min and Min &lt;= Max.
    /// a PositiveRange is immutable.
    /// </summary>
    public class PositiveRange : ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the PositiveRange class,
        /// i.e. a range of integers, such that 0 &lt;= low &lt;= high.
        /// </summary>
        /// <param name="low"> The lower bound (Min) of the range.</param>
        /// <param name="high"> The upper bound (Max) of the range.</param>
        /// throws IllegalArgumentException
        public PositiveRange(int low, int high)
        {
            if (!((0 <= low) && (low <= high)))
            {
                throw new ArgumentOutOfRangeException(
                $"[{low},{high}]",
                "The order 0 <= low <= high is false.");
            }
            
            this.Min = low;
            this.Max = high;
        }

        /// <summary>
        /// Gets the lower bound (Min) of the interval.
        /// </summary>
        public readonly int Min; // { get; } 

        /// <summary>
        /// Gets upper bound (Max) of the interval.
        /// </summary>
        public readonly int Max; // { get; } 
 
        /// <summary>
        /// Represents the range as a string, formatted as "[Min,Max]".
        /// </summary>
        /// <returns>a string representation of the range.</returns>
        public override string ToString()
        {
            return $"[{this.Min},{this.Max}]";
        }
 
        /// <summary>
        /// Checks if the given value lies within the range,
        /// i.e.  Min &lt;= value and value &lt;= Max.
        /// </summary>
        /// <param name="value">The value to checked.</param>
        /// <returns>True, if the range includes the value, 
        /// false otherwise.</returns>
        public bool Contains(int value)
        {
            return (this.Min <= value) && (value <= this.Max);
        }
 
        /// <summary>
        /// Checks if the given value lies within the range,
        /// bo.exp.  Min &lt;= value and value &lt;= Max.
        /// If the value ist not contained an 
        /// ArgumentOutOfRangeException will be raised.
        /// </summary> 
        /// <param name="value">The value to checked.</param>
        /// <returns>True, if the range includes the value, 
        /// false otherwise.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public bool ContainsOrFail(int value)
        {
            if (!((this.Min <= value) && (value <= this.Max)))
            {
                throw new System.ArgumentOutOfRangeException(
                   new System.Text.StringBuilder(64).Append(this.ToString()).
                   Append(" does not contain ").Append(value).ToString());
            }
            
            return true;
        }
 
        /// <summary>
        /// Checks if the given range is a subrange,
        /// bo.exp.  this.Min &lt;= range.Min and range.Max &lt;= this.Max.
        /// </summary>
        /// <param name="range">The range to be checked.</param>
        /// <returns>True, if the given range lies within the bounds 
        /// of this range, false otherwise.</returns>
        public bool Contains(PositiveRange range)
        {
            return (this.Min <= range.Min) && (range.Max <= this.Max);
        }
 
        /// <summary>
        /// Checks if the given range is a subrange,
        /// bo.exp.  this.Min &lt;= range.Min and range.Max &lt;= this.Max.
        /// If the range ist not contained an ArgumentOutOfRangeException
        /// will be raised. </summary>
        /// <param name="range">The range to be checked.</param>
        /// <returns>True, if the given range lies within the bounds 
        /// of this range, false otherwise.</returns>
        public bool ContainsOrFail(PositiveRange range)
        {
            if (!((this.Min <= range.Min) && (range.Max <= this.Max)))
            {
                throw new System.ArgumentOutOfRangeException(
                   new System.Text.StringBuilder(64).Append(this.ToString()).
                   Append(" does not contain ").Append(range.ToString()).ToString());
            }
            
            return true;
        }
 
        /// <summary>
        /// The Size of the range.
        /// </summary>
        /// <returns>The size of the range.</returns>
        public int Size()
        {
            return this.Max - this.Min + 1;
        }
 
        /// <summary>
        /// Overrides System.Object.Clone()
        /// </summary>
        /// <returns>a clone of this range.</returns>
        public object Clone()
        {
            return new PositiveRange(this.Min, this.Max);
        }
 
        /// <summary>
        /// Compares this range to the specified object. The results
        /// is true if and only if the argument is not null and is a
        /// PositiveRange object that has the same values as this object.
        /// </summary>
        /// <param name="obj"> The object to compare with.</param>
        /// <returns> True if the object has the same values as this object; 
        /// false otherwise. </returns>
        public override bool Equals(object obj)
        {
            if (null == (object)obj)
            {
                return false;
            }
 
            // If parameter cannot be cast to PositiveRange return false.
            var that = obj as PositiveRange;
            if ((object)that == null)
            {
                return false;
            }
            
            return this.Equals(that);
        }
 
        /// <summary>
        /// Compares this range to the specified range. The results is true
        /// if and only if the argument has the same values as this object.
        /// </summary>
        /// <param name="that"> a positive range to compare with.</param>
        /// <returns> True if the given range has the same bounds as 
        /// this object; false otherwise. </returns>
        public bool Equals(PositiveRange that)
        {
            if (null == (object)that)
            {
                return false;
            }
 
            // Return true if the fields match:
            return (this.Min == that.Min) && (this.Max == that.Max);
        }
 
        // By default, the operator == tests for reference equality by 
        // determining whether two references indicate the same object. 
        // PositiveRange is immutable, so overloading operator == to compare
        // value equality instead of reference equality is useful because, 
        // as immutable objects, two ranges can be considered the same as
        // long as they have the same values Min and Max. 
 
        /// <summary>
        /// Compares this range to the specified range. The results is true
        /// if and only if the arguments have the same value.
        /// </summary>
        /// <param name="a"> a positive range.</param>
        /// <param name="b"> a positive range.</param>
        /// <returns> True if the two ranges have the same bounds;
        /// false otherwise. </returns>
        public static bool operator ==(PositiveRange a, PositiveRange b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }
 
            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }
 
            // Return true if the fields match: 
            // return a.Equals(b);
            return (a.Min == b.Min) && (a.Max == b.Max);
        }
 
        /// <summary>
        /// Compares this range to the specified range. The results is true
        /// if and only if the arguments do not have the same value.
        /// </summary>
        /// <param name="a"> a positive range.</param>
        /// <param name="b"> a positive range.</param>
        /// <returns> False if the two ranges have the same bounds;
        /// true otherwise. </returns>
        public static bool operator !=(PositiveRange a, PositiveRange b)
        {
            return !(a == b);
        }
 
        /// <summary>
        /// Compares two ranges with respect to inclusion. The results is true
        /// if and only if the right hand side contains the left hand side.
        /// </summary>
        /// <param name="a"> a positive range.</param>
        /// <param name="b"> a positive range.</param>
        /// <returns> True if b contains a.</returns>
        public static bool operator <=(PositiveRange a, PositiveRange b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }
 
            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }
 
            return b.Contains(a);
        }
 
        /// <summary>
        /// Compares two ranges with respect to inclusion. The results is true
        /// if and only if the left hand side contains the right hand side.
        /// </summary>
        /// <param name="a"> a positive range.</param>
        /// <param name="b"> a positive range.</param>
        /// <returns> True if a contains b.</returns>
        public static bool operator >=(PositiveRange a, PositiveRange b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }
 
            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }
 
            return a.Contains(b);
        }
 
        /// <summary>
        ///  a hash code value for this range.
        /// </summary>  
        /// <returns>Returns a hash code for this.</returns>
        public override int GetHashCode()
        {
            return 29 * this.Min + this.Max;
        }
    }
} 
