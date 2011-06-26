// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.apps.factorial;

import com.jgoodies.looks.plastic.PlasticXPLookAndFeel;

import javax.swing.*;
import java.text.DateFormat;
import java.util.Calendar;
import java.util.Date;

public final class BenchmarkApplication {
    // / <summary>
    // / The main entry point for the application.
    // / </summary>

    public static void main(String args[]) {

        if (System.getProperty("java.version").compareTo("1.6") < 0) {
            System.out.println("This program requires Java 1.6 or later.");
            System.out.println("You can download Java 1.6 from http://java.sun.com");
            return;
        }

        try {
            UIManager.setLookAndFeel(new PlasticXPLookAndFeel());
        } catch (Exception ignored) {
        }

        java.awt.EventQueue.invokeLater(new Runnable() {

            @Override
            public void run() {
                new BenchmarkForm().setVisible(true);
            }
        });
    }

    public static void printAppAndSysProps(java.io.PrintStream ps) {

        final String[] props = {"os.name", "os.version", "java.vendor", "java.version", "java.runtime.version", "java.vm.version",
            "java.vm.vendor", "java.vm.name", "java.vm.info", "java.class.version"};

        ps.println("<<-- System Environment Information -->>");
        ps.println();
        for (int i = 0; i < 2; i++) {
            ps.print(props[i]);
            ps.print("->");
            ps.print(System.getProperty(props[i], "not available."));
            ps.println();
        }
        ps.println();
        ps.println("<<-- Java Execution Environment Information -->>");
        ps.println();
        for (int i = 2; i < props.length; i++) {
            ps.print(props[i]);
            ps.print("->");
            ps.print(System.getProperty(props[i], "not available."));
            ps.println();
        }
        ps.println();
        ps.println("<<-- Application Information -->>");
        ps.println();
        ps.println("Application.name        -> Factorial Function Benchmark - Java Gui");
        ps.println("Application.id          -> de.luschny.apps.factorial");
        ps.println("Application.title       -> Factorial Function Benchmark");
        ps.println("Application.version     -> 3.1.20080620");
        ps.println("Application.vendor      -> Java Jungle");
        ps.println("Application.vendorId    -> de.javajungle");
        ps.println("Application.homepage    -> http://www.luschny.de/math/factorial/FastFactorialFunctions.htm");
        ps.println("Application.description -> How fast can we compute the factorial function?");
        ps.println("Application.lookAndFeel -> PlasticXPLookAndFeel");
        ps.println();
        ps.println("<<-- Runtime Information -->>");
        ps.println();
        Runtime rt = Runtime.getRuntime();
        ps.println("Runtime total memory     -> " + rt.totalMemory());
        ps.println("Runtime free  memory     -> " + rt.freeMemory());
        ps.println("Number of processors     -> " + rt.availableProcessors());
        ps.println();
        ps.println("<<-- Use Information -->>");
        ps.println();
        // get date and time with the default formatting style
        // of the default local
        Date dateStruct = Calendar.getInstance().getTime();
        String dateString = DateFormat.getDateTimeInstance().format(dateStruct);

        // Signal the start of Application
        ps.println("Application started at " + dateString);
        ps.println();
        ps.println("Hello " + System.getProperty("user.name", "Hello") + "!");
        ps.println("Welcome to the Factorial Benchmark.");
        ps.println();
    }
}
