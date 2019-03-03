This project has to be linked against the MPIR library.
http://mpir.org/

Look at mpir-devel for the latest informations.
https://groups.google.com/forum/#!forum/mpir-devel

I used Brian Gladman's fork for testing with Windows.
https://github.com/BrianGladman/mpir

The provided makefile assumes you install mpir library into
/usr/local/lib and header files into /usr/local/include
If you choose a different location, then edit the CPPFLAGS and
LDFLAGS in makefile accordingly.

Compiling with makefile has been tested on SLES 12SP3 and
gcc 4.8.5
