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
using System.Diagnostics;
using Microsoft.Win32;

namespace SilverFactorial
{
    /// <summary>
    /// Interaction logic for Application.xaml
    /// </summary>
    public partial class BenchmarkApplication : Application
    {
        public static void PrintAppAndSysProps(LoggedTextBox textBox)
        {
            TextWriter writer = textBox.GetStreamWriter();
            var ci = new Microsoft.VisualBasic.Devices.ComputerInfo();

            writer.WriteLine("<<--  Execution Environment Information -->>");
            writer.WriteLine();
            writer.WriteLine("OS-Version              -> {0}", Environment.OSVersion.ToString());
            writer.WriteLine("Runtime                 -> {0}", Environment.Version.ToString());
            writer.WriteLine("Is64BitProcess          -> {0}", Environment.Is64BitProcess);
            writer.WriteLine("Is64BitOperatingSystem  -> {0}", Environment.Is64BitOperatingSystem);
            writer.WriteLine("InstalledUICulture      -> {0}", ci.InstalledUICulture);
            writer.WriteLine("MachineName             -> {0}", Environment.MachineName);
            writer.WriteLine();
            writer.WriteLine("<<-- Application Information -->>");
            writer.WriteLine();
            writer.WriteLine("Application.name        -> Factorial Function Benchmark - C# Gui");
            writer.WriteLine("Application.id          -> Sharit.Application.Factorial");
            writer.WriteLine("Application.title       -> Factorial Function Benchmark");
            writer.WriteLine("Application.version     -> 3.1.20110620");
            writer.WriteLine("Application.vendor      -> Peter Luschny");
            writer.WriteLine("Application.homepage    -> " + @"http://www.Luschny.de/math/factorial/FastFactorialFunctions.htm");
            writer.WriteLine("Application.description -> How fast can we compute the factorial function?");
            writer.WriteLine();
            writer.WriteLine("<<-- Runtime Information -->>");
            writer.WriteLine();
            writer.WriteLine("Processor               -> {0}", ProcessorInfo());
            writer.WriteLine("ProcessorCount          -> {0}", Environment.ProcessorCount.ToString());
            writer.WriteLine("TotalPhysicalMemory     -> {0}", ci.TotalPhysicalMemory);
            writer.WriteLine("TotalVirtualMemory      -> {0}", ci.TotalVirtualMemory);
            writer.WriteLine("AvailablePhysicalMemory	-> {0}", ci.AvailablePhysicalMemory);
            writer.WriteLine("AvailableVirtualMemory	-> {0}", ci.AvailableVirtualMemory);
            writer.WriteLine();
            TimerProperties(writer);
            writer.WriteLine();
            writer.WriteLine("<<-- Use Information -->>");

            textBox.WriteLine();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", false).DateTimeFormat;
            DateTime dateTime = DateTime.Now;

            textBox.WriteLine("Application started at {0}", dateTime.ToString("u", dateTimeFormat));
            textBox.WriteLine();
            textBox.WriteLine("Hello {0}, \n \n   welcome to the Factorial Benchmark!", Environment.UserName);
            textBox.WriteLine("\n   For background information visit \n");
            textBox.WriteLine(@"http://www.luschny.de/math/factorial/FastFactorialFunctions.htm");
            textBox.WriteLine();
            textBox.WriteLine("\n   The arithmetic used is MPIR 2.6. Make sure that the library fits your CPU!");
            // textBox.WriteLine("\n\n   Not only the timings also the relative \n   rankings can differ considerably if a \n   different arithmetic (for example \n   BigIntegers from .NET) is used!\n");
            textBox.WriteLine("\n\n   To include your own factorial function into the benchmark edit the file MyFactorial.cs and recompile.");
            textBox.WriteLine();
        }

        public static void TimerProperties(TextWriter writer)
        {
            writer.Write("Operations timed using the ");
            if (Stopwatch.IsHighResolution)
            {
                writer.WriteLine("system's high-resolution performance counter.");
            }
            else
            {
                writer.WriteLine(" DateTime class.");
            }

            long frequency = Stopwatch.Frequency;
            long nanosecPerTick = (1000L * 1000L * 1000L) / frequency;
            writer.WriteLine("Timer frequency in ticks per second = {0}.", frequency);
            writer.WriteLine("Timer is accurate within {0} nanoseconds.", nanosecPerTick);
        }

        public static string ProcessorInfo()
        {
            string procname = null;
            RegistryKey reg = 
                Registry.LocalMachine.OpenSubKey(@"Hardware\Description\System\CentralProcessor\0", RegistryKeyPermissionCheck.ReadSubTree);   

            if (reg != null)
            {
                procname = (string)reg.GetValue("ProcessorNameString");
                if (procname == null)
                {
                    procname = "not available"; 
                }
            }
            return procname;
        }

        public static void About(LoggedTextBox textBox)
        {
           textBox.WriteLine("\n=========================================");
           textBox.WriteLine("\n     This is Sharith.SilverFactorial     ");
           textBox.WriteLine("\n       Version 3.0-Build-2013-03-08      ");
           textBox.WriteLine("\n     Copyright (C) 2013 Peter Luschny    ");
           textBox.WriteLine("\n                License:                 ");
           textBox.WriteLine("\n   Creative Commons Attr.-ShareAlike 3.0 ");
           textBox.WriteLine("\n   http://www.luschny.de/math/factorial/ ");
           textBox.WriteLine("\n        FastFactorialFunctions.htm       ");
           textBox.WriteLine("\n Please send comments and bug reports to ");
           textBox.WriteLine("\n          peter(at)luschny.de            ");
           textBox.WriteLine("\n       Contribute your solution!         ");
           textBox.WriteLine("\n=========================================\n");
        }
    }
}        
 