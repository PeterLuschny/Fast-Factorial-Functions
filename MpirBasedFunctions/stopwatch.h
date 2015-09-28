// Author & Copyright : Peter Luschny
// Created: 2010-01-15
// License: LGPL version 2.1 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0

#ifndef __STOPWATCH_H
#define __STOPWATCH_H

#include <chrono>
#include <iostream>

typedef std::chrono::high_resolution_clock Time;
typedef std::chrono::milliseconds ms;
typedef std::chrono::duration<float> fsec;

auto was = Time::now();

class StopWatch {
public:

static void Start()
{
   was = Time::now();
}

static void ElapsedTime()
{
   auto now = Time::now();

   fsec fs = now - was;
   ms d = std::chrono::duration_cast<ms>(fs);
   std::cout << fs.count() << " s ";
}
};

#endif // __STOPWATCH_H

