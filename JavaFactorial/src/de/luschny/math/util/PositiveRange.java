// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.util;

/**
 * A PositiveRange is an interval of integers, given by a lower bound L and an
 * upper bound U, such that 0 <= L and L <= U. A PositiveRange is immutable.
 */
public final class PositiveRange implements Cloneable {

    private final int min;
    private final int max;

    /**
     * Creates a PositiveRange, i.e. a range of integers, such that 0 <= low <=
     * high.
     * 
     * @param low
     *            The low bound (minimum) of the range.
     * @param high
     *            The high bound (maximum) of the range.
     * @throws IllegalArgumentException
     */
    public PositiveRange(final int low, final int high) {
        if (!(0 <= low && low <= high)) {
            // Yes, we force the arguments to be ordered
            // to make the calling code more readable.
            throw new IllegalArgumentException("The condition 0 <= " + low + " <= " + high + " is  false.");
        }

        this.min = low;
        this.max = high;
    }

    /**
     * Creates and returns a copy of this object.
     * 
     * @return A copy of this PositiveRange.
     */
    @Override
    public Object clone() {
        try {
            return super.clone();
        } catch (CloneNotSupportedException e) {
            throw new InternalError(e.toString());
        }
    }

    /**
     * Get the lower bound (minimum) of the range.
     * 
     * @return The minimum of the range.
     */
    public int getMin() {
        return min;
    }

    /**
     * Get upper bound (maximum) of the range.
     * 
     * @return The maximum of the range.
     */
    public int getMax() {
        return max;
    }

    /**
     * <p>
     * Gets the range as a <code>String</code>.
     * </p>
     * <p>
     * The format of the String is '[min, max]'.
     * </p>
     * 
     * @return the <code>String</code> representation of this range.
     */
    @Override
    public String toString() {
        return "[" + min + ',' + max + ']';
    }

    /**
     * Checks, if the given value lies within the range, i.e. min &lt;= value
     * and value &lt;= max.
     * 
     * @param value
     *            The value to checked.
     * @return True, if the range includes the value, false otherwise.
     * @see PositiveRange#containsOrFail(int value)
     */
    public boolean contains(final int value) {
        return min <= value && value <= max;
    }

    /**
     * Checks, if the given value lies within the range, i.e. min &lt;= value
     * and value &lt;= max. If the value is not contained an
     * IndexOutOfBoundsException will be raised.
     * 
     * @param value
     *            The value to checked.
     * @return True, if the range includes the value, false otherwise.
     * @throws IndexOutOfBoundsException
     */
    public boolean containsOrFail(final int value) {
        if (!(min <= value && value <= max)) {
            throw new IndexOutOfBoundsException(this.toString() + " does not contain " + value);
        }
        return true;
    }

    /**
     * Checks, if the given range is a subrange, i.e. this.min &lt;= range.min
     * and range.max &lt;= this.max.
     * 
     * @param range
     *            The range to be checked.
     * @return True, if the given range lies within this range, false otherwise.
     * @see PositiveRange#containsOrFail(PositiveRange range)
     */
    public boolean contains(final PositiveRange range) {
        return min <= range.min && range.max <= max;
    }

    /**
     * Checks, if the given range is a subrange, i.e. this.min &lt;= range.min
     * and range.max &lt;= this.max. If the range is not contained an
     * IndexOutOfBoundsException will be raised.
     * 
     * @param range
     *            The range to be checked.
     * @return True, if the given range lies within this range, otherwise an
     *         IndexOutOfBoundsException is thrown.
     * @throws IndexOutOfBoundsException
     */
    public boolean containsOrFail(final PositiveRange range) {
        if (!(min <= range.min && range.max <= max)) {
            throw new IndexOutOfBoundsException(this.toString() + " does not contain " + range.toString());
        }
        return true;
    }

    /**
     * Computes the size of the range.
     * 
     * @return The size of the range.
     */
    public int size() {
        return max - min + 1;
    }

    /**
     * Compares this range to the specified object. The result is true if and
     * only if the argument is not null and is an range object that has the same
     * bound values as this object.
     * 
     * @param o
     *            The object to compare with.
     * @return True if the objects are the same; false otherwise.
     */
    @Override
    public boolean equals(Object o) {
        if (this == o) {
            return true;
        }
        if (!(o instanceof PositiveRange)) {
            return false;
        }

        PositiveRange positiveRange = (PositiveRange) o;

        if (max != positiveRange.max) {
            return false;
        }
        return min == positiveRange.min;

    }

    /**
     * A hash code value for this range.
     * 
     * @return Returns a hash code for this.
     */
    @Override
    public int hashCode() {
        return 29 * min + max;
    }
}