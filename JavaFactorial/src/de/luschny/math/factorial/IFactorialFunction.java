// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.factorial;

import de.luschny.math.arithmetic.Xint;

// An interface for the factorial function
//        n! = 1*2*3*...*n
// for nonnegative integer values n.
public interface IFactorialFunction {

    Xint factorial(int n);

    String getName();
}

// endOfIFactorialFunction
