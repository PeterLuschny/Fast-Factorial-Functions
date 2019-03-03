CC=gcc
CXX=g++
RM=rm -f
CPPFLAGS=-g -std=c++11 -pthread -I/usr/local/include

LDFLAGS=-g -std=c++11 -pthread
LDLIBS=-L/usr/local/lib -lgmp -lmpir

SRCS=binomial.cpp double_fac_ui.cpp parallelbinomial.cpp parallelschoenhage.cpp primeswing.cpp doublefactorial.cpp main.cpp paralleldoublefactorial.cpp parallelswing.cpp schoenhage.cpp

OBJS=$(subst .cpp,.o,$(SRCS))

all: main

main: $(OBJS)
	    $(CXX) $(LDFLAGS) -o main $(OBJS) $(LDLIBS) 

depend: .depend

.depend: $(SRCS)
	$(RM) ./.depend
	$(CXX) $(CPPFLAGS) -MM $^>>./.depend;

clean:
	    $(RM) $(OBJS)

distclean: clean
	    $(RM) *~ .depend

include .depend
