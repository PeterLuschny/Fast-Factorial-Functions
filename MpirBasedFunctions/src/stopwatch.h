// Author & Copyright : Peter Luschny
// Created: 2010-01-15
// License: LGPL version 2.1 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0

#ifndef __STOPWATCH_H
#define __STOPWATCH_H

#define __BOOST

static unsigned long int was, now;

#ifdef __BOOST

#include <boost/date_time/posix_time/posix_time.hpp>

class StopWatch {
public:

static void Start()
{
    boost::posix_time::ptime tnow = boost::posix_time::microsec_clock::local_time();
    now = (unsigned long int) tnow.time_of_day().total_milliseconds();
}

static void ElapsedTime()
{
    was = now; Start();
    double ela = (double)(now - was) / 1000;
    std::cout << ela << " sec " ;
}
};

#else

#include <sys/resource.h>

class StopWatch {
public:

static void Start()
{
    struct rusage rus;
    getrusage (0, &rus);
    now = (unsigned long int) (rus.ru_utime.tv_sec * 1000
        + rus.ru_utime.tv_usec / 1000 );
}

static void ElapsedTime()
{
    was = now; Start();
    double ela = (double)(now - was) / 1000;
    std::cout << ela << " sec " ;
}
};

#endif // __BOOST
#endif // __STOPWATCH_H
