// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.apps.factorial;

import java.io.File;
import java.io.IOException;
import java.io.PrintWriter;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.Iterator;

public class Results {

    Candidate creator;
    public double sec;
    public double rank;
    public long crc;
    public int absRank;
    public int[] nops;

    // public String opstring;
    Results(Candidate creator, double sec, long check, int[] nops) {
        this.creator = creator;
        this.sec = sec;
        this.crc = check;
        this.nops = nops;
        // this.opstring = opstring;
    }

    @Override
    public String toString() {
        return String.format("$0%7.2f Sec <$1%s>", sec, Long.toHexString(crc));
    }
    public static String nopHeader = "   MUL     mul     DIV     div     Sqr     Lsh";

    public String nopsAsString() {
        StringBuilder sb = new StringBuilder();
        for (int nop : nops) {
            sb.append(String.format("%6d |", nop));
        }
        return sb.toString();
    }

    public String getRankAsString() {
        return String.format("%5.1f", rank);
    }

    public String getTimeAsString() {
        return String.format("%5.1f", sec);
    }

    void setRank(double rank) {
        this.rank = rank;
    }

    double getRank() {
        return rank;
    }

    public int compareTo(Object o) {
        final double or = ((Results) o).rank;
        if (or == rank) {
            return 0;
        }
        if (or < rank) {
            return 1;
        }
        return -1;
    }

    public static void sort(Results[] a, int toIndex) {
        checkRange(a.length, 0, toIndex);
        insertSort(a, 0, toIndex);
        for (int i = 0; i < toIndex; i++) {
            a[i].absRank = i + 1;
        }
    }

    private static void insertSort(Results[] r, int low, int high) {
        for (int i = low; i < high; i++) {
            for (int j = i; j > low && (r[j - 1]).compareTo(r[j]) > 0; j--) {
                Results t = r[j];
                r[j] = r[j - 1];
                r[j - 1] = t;
            }
        }
    }

    private static void checkRange(int arrayLen, int fromIndex, int toIndex) {
        if (fromIndex > toIndex) {
            throw new IllegalArgumentException("fromIndex(" + fromIndex + ") > toIndex(" + toIndex + ")");
        }
        if (fromIndex < 0) {
            throw new ArrayIndexOutOfBoundsException(fromIndex);
        }
        if (toIndex > arrayLen) {
            throw new ArrayIndexOutOfBoundsException(toIndex);
        }
    }

    public static String resultsToFile(String prefix, int[] benchValues) {

        SimpleDateFormat df = new SimpleDateFormat("yyyyMMddHHmmss");
        Date dateStruct = Calendar.getInstance().getTime();
        String curDir = System.getProperty("user.dir");
        String logDir = curDir + File.separator + "log";
        String fileName = prefix + df.format(dateStruct) + ".html";

        try {
            boolean exists = (new File(logDir)).exists();
            if (!exists) {
                boolean success = (new File(logDir)).mkdir();
                if (success) {
                    fileName = logDir + File.separator + fileName;
                }
            } else {
                fileName = logDir + File.separator + fileName;
            }

            PrintWriter benchReport = new PrintWriter(fileName);
            writeResults(benchReport, benchValues);
            benchReport.close();
        } catch (IOException e) {
            System.err.println(e.toString());
        }

        return fileName;
    }

    static void writeResults(PrintWriter file, int[] benchValues) {
        for (String item : htmlHeader) {
            file.println(item);
        }

        for (int j = 0; j < 2; j++) {
            if (j == 0) {
                file.println("<h3 align=\"center\">");
                file.println("Java Factorial Benchmark - Timings (in seconds)");
                file.println("</h3>");
                file.println("<table cellpadding=\"7\" align=\"center\" border=\"0\" "
                        + "summary=\"benchmark results timings\">");
            } else {
                file.println("<br>");
                file.println("<h3 align=\"center\">");
                file.println("Java Factorial Benchmark - Ranking (relative to 'PrimeSwing')");
                file.println("</h3>");
                file.println("<table cellpadding=\"7\" align=\"center\" border=\"0\" "
                        + "summary=\"benchmark results ranking\">");
            }

            file.println("<tbody>");
            file.println("<tr class=\"count\">");
            file.println("<td class=\"head\"> N! where N = </td>");

            for (int value : benchValues) {
                file.print("<td class=\"head\">");
                file.print(value);
                file.println("</td>");
            }

            file.println("</tr>");

            Iterator<Candidate> selectedCandidates = Candidate.getSelected();
            while (selectedCandidates.hasNext()) {
                Candidate cand = selectedCandidates.next();

                file.println("<tr class=\"count\">");

                file.print("<td class=\"fact\">");
                file.print(cand.getName().trim());
                file.println("</td>");

                for (int value : benchValues) {
                    Results res = cand.results.get(value);

                    if (j == 0) {
                        if (res.absRank == 1) {
                            file.print("<td class=\"count1\">");
                        } else if (res.absRank == 2) {
                            file.print("<td class=\"count2\">");
                        } else {
                            file.print("<td class=\"count\">");
                        }

                        file.print(res.getTimeAsString());

                    } else {
                        if (res.rank < 1.05) {
                            file.print("<td class=\"count1\">");
                        } else if (res.rank < 2.05) {
                            file.print("<td class=\"count2\">");
                        } else {
                            file.print("<td class=\"count\">");
                        }

                        file.print(res.getRankAsString());
                    }

                    file.println("</td>");
                }

                file.println("</tr>");
            }
            file.println("</tbody>");
            file.println("</table>");

            if (j == 1) {
                file.println("<h5 align=\"center\">");
                file.println("Timing: Red = first, blue = second.<br>");
                file.println("Ranking: The smaller the value the better.<br>");
                file.println("Values p &lt;= 1 (red) indicate excellent performance,<br>");
                file.println("values p &lt;= 2 (blue) indicate good performance.<br>");
                file.println("</h5>");
            }
        }

        file.println("</body>");
        file.println("</html>");
    }
    private static String[] htmlHeader = {"<!DOCTYPE html PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\">", "<html>", "<head>",
        "<meta name=\"GENERATOR\" content=\"FactorialBenchmark\">", "<title>", "Factorial Benchmark",
        "</title>", "<style type=\"text/css\">", "body     { background-color:white; color: black; ",
        "font-family: Monospace, Sans-Serif, Arial, Helvetica, Verdana; }",
        "td.count  { background-color : #c0d9c0; font-weight: bold }",
        "td.count1 { background-color : #c0d9c0; color : red; font-weight: bold }",
        "td.count2 { background-color : #c0d9c0; color : blue; font-weight: bold }",
        "td.fact  { background-color: #f0e68c; text-align: right; ",
        "font-family: Arial, Lucida Sans Typewriter; font-size:small; }",
        "td.head  { background-color: #b0c4de; text-align: center; }", "tr.count { text-align: center; }",
        "</style>", "</head>", "<body>"};
}
