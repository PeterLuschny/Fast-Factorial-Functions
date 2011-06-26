// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

using System;
using System.Globalization;
using System.IO;
using System.Windows;

namespace SilverFactorial
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class BenchmarkApplication : Application
    {
        public static void PrintAppAndSysProps(LoggedTextBox textBox)
        {
            StreamWriter streamWriter = textBox.GetStreamWriter();
            var ci = new Microsoft.VisualBasic.Devices.ComputerInfo();

            streamWriter.WriteLine("<<--  Execution Environment Information -->>");
            streamWriter.WriteLine();
            streamWriter.WriteLine("OS-Version              -> {0}", Environment.OSVersion.ToString());
            streamWriter.WriteLine("Runtime                 -> {0}", Environment.Version.ToString());
            streamWriter.WriteLine("Is64BitProcess          -> {0}", Environment.Is64BitProcess);
            streamWriter.WriteLine("Is64BitOperatingSystem  -> {0}", Environment.Is64BitOperatingSystem);
            streamWriter.WriteLine("InstalledUICulture      -> {0}", ci.InstalledUICulture);
            streamWriter.WriteLine();
            streamWriter.WriteLine("<<-- Application Information -->>");
            streamWriter.WriteLine();
            streamWriter.WriteLine("Application.name        -> Factorial Function Benchmark - C# Gui");
            streamWriter.WriteLine("Application.id          -> Sharit.Application.Factorial");
            streamWriter.WriteLine("Application.title       -> Factorial Function Benchmark");
            streamWriter.WriteLine("Application.version     -> 3.1.20110620");
            streamWriter.WriteLine("Application.vendor      -> Peter Luschny");
            streamWriter.WriteLine("Application.homepage    -> " + @"http://www.Luschny.de/math/factorial/FastFactorialFunctions.htm");
            streamWriter.WriteLine("Application.description -> How fast can we compute the factorial function?");
            streamWriter.WriteLine();
            streamWriter.WriteLine("<<-- Runtime Information -->>");
            streamWriter.WriteLine();
            streamWriter.WriteLine("MachineName             -> {0}", Environment.MachineName);
            streamWriter.WriteLine("ProcessorCount          -> {0}", Environment.ProcessorCount.ToString());
            streamWriter.WriteLine("TotalPhysicalMemory     -> {0}", ci.TotalPhysicalMemory);
            streamWriter.WriteLine("TotalVirtualMemory      -> {0}", ci.TotalVirtualMemory);
            streamWriter.WriteLine("AvailablePhysicalMemory	-> {0}", ci.AvailablePhysicalMemory);
            streamWriter.WriteLine("AvailableVirtualMemory	-> {0}", ci.AvailableVirtualMemory);
            streamWriter.WriteLine();
            streamWriter.WriteLine("<<-- Use Information -->>");

            textBox.WriteLine();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", false).DateTimeFormat;
            DateTime dateTime = DateTime.Now;

            textBox.WriteLine("Application started at {0}", dateTime.ToString("u", dateTimeFormat));
            textBox.WriteLine();
            textBox.WriteLine("Hello {0}! \n\n   Welcome to the Factorial Benchmark.", Environment.UserName);
            textBox.WriteLine("\n   To include your own factorial function into the benchmark edit the file MyFactorial.cs and recompile.");
            textBox.WriteLine();
            textBox.WriteLine("\n   For background information visit\n");
            textBox.WriteLine(@"http://www.luschny.de/math/factorial/FastFactorialFunctions.htm");
            textBox.WriteLine();
#if(MPIR)
            textBox.WriteLine("\n   Info: The arithmetic used is MPIR 2.4.\n");
#else
            textBox.WriteLine("\n   Info: The arithmetic used is C# BigInteger.\n");
#endif
            textBox.WriteLine("   Not only the timings also the relative rankings can differ considerably if a different arithmetic is used.\n");
            textBox.WriteLine();
        }
    }
}        
 