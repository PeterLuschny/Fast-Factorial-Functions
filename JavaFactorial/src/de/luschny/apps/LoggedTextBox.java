// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.apps;

import javax.swing.*;
import java.io.File;
import java.io.IOException;
import java.io.PrintStream;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;

public class LoggedTextBox {

    public boolean logToFile;
    PrintStream ps;
    JTextArea textArea;
    StringBuilder sb;

    public LoggedTextBox(JTextArea box, String name) throws IOException {
        textArea = box;
        sb = new StringBuilder();
        SimpleDateFormat df = new SimpleDateFormat("yyyyMMddHHmmss");
        Date dateStruct = Calendar.getInstance().getTime();

        String curDir = System.getProperty("user.dir");
        String logDir = curDir + File.separator + "log";
        String fileName = name + df.format(dateStruct) + ".log";

        try {
            boolean exists = (new File(logDir)).exists();
            if (!exists) {
                // Create one directory
                boolean success = (new File(logDir)).mkdir();
                if (success) {
                    fileName = logDir + File.separator + fileName;
                }
            } else {
                fileName = logDir + File.separator + fileName;
            }
        } catch (Exception e) { //
        }

        ps = new PrintStream(fileName);
    }

    public PrintStream getPrintStream() {
        return ps;
    }

    public void setLogToFile(boolean val) {
        logToFile = val;
    }

    public void Write(String str) {
        if (logToFile) {
            ps.print(str);
        }
        sb.append(str);
    }

    public void WriteLine(String str) {
        if (logToFile) {
            ps.println(str);
        }
        SwingUtilities.invokeLater(new AppendTextCallback(sb.append(str).append("\n").toString()));
        sb = new StringBuilder();
    }

    public void WriteLine() {
        if (logToFile) {
            ps.println();
        }

        if (sb.length() == 0) {
            SwingUtilities.invokeLater(new AppendTextCallback("\n"));
            return;
        }

        SwingUtilities.invokeLater(new AppendTextCallback(sb.append("\n").toString()));
        sb = new StringBuilder();
    }

    public void Flush() {
        ps.flush();
        if (sb.length() > 0) {
            SwingUtilities.invokeLater(new AppendTextCallback(sb.toString()));
        }
        sb = new StringBuilder();
    }

    public void Dispose() {
        ps.flush();
        ps.close();
    }

    // /////////////////////////////////////////////
    // This class is a delegate that enables asynchronous calls
    // for appending the text on a TextBox control.
    private class AppendTextCallback implements Runnable {

        private String txt;

        AppendTextCallback(String txt) {
            this.txt = txt;
        }

        @Override
        public void run() {
            textArea.append(txt);
            textArea.setCaretPosition(textArea.getDocument().getLength());
        }
    }
} // endOfLoggedTextBox
