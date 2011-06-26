// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.util;

import java.util.logging.Logger;

/**
 * Components that need to log can implement this
 * interface to be provided Loggers.
 * Note that we use 'enableLogging()' instead of 'setLogger()'.
 * The reason is, that this enables a quick refactoring
 * if org.apache.avalon.framework.logger is to be used.
 */
public interface ILoggable {

    /**
     * Provide a component with a logger.
     *
     * @param logger The Logger to be used to log the messages.
     */
    void enableLogging(Logger logger);
}
