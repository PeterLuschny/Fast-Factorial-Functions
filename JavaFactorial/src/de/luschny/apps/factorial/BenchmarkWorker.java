// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.apps.factorial;

import de.luschny.apps.LoggedTextBox;
import de.luschny.apps.StopWatch;
import de.luschny.math.Xmath;
import de.luschny.math.arithmetic.Xint;
import de.luschny.math.factorial.FactorialFactors;
import de.luschny.math.factorial.IFactorialFunction;

import javax.swing.*;
import java.io.IOException;
import java.io.PrintWriter;
import java.util.Iterator;

class WorkerCompletedEvent {

    final boolean cancelled;
    final Throwable error;

    public WorkerCompletedEvent(boolean cancelled, boolean done, Throwable error) {
        this.cancelled = cancelled;
        boolean done1 = done;
        this.error = error;
    }
}

class BenchmarkWorker {

    private final StopWatch watch;
    private final LoggedTextBox winsole;
    private final BenchmarkForm monitor; // PropertyChangeListener
    private FactorialTest test;
    private BenchmarkExecutor executor;

    public BenchmarkWorker(LoggedTextBox ws, BenchmarkForm monitor) {
        winsole = ws;
        this.monitor = monitor;
        watch = new StopWatch();
    }

    public void execute(FactorialTest test) {
        this.test = test;
        test.init();

        executor = new BenchmarkExecutor();
        executor.addPropertyChangeListener(monitor);
        executor.execute();
    }

    public void cancelAsync() {
        executor.cancel(true);
        executor = null;
    }

    class BenchmarkExecutor extends SwingWorker<Void, Void> {

        Throwable err = null;

        // This event handler deals with the results of the benchmark.
        @Override
        public void done() {
            monitor.benchmarkCompleted(new WorkerCompletedEvent(isCancelled(), isDone(), err));
        }

        @Override
        protected Void doInBackground() {
            try {

                if (test.sanityTest) {
                    winsole.writeLine();
                    winsole.writeLine("Sanity check is running!");
                    sanityCheck(1000);
                    return null;
                }

                int[] benchValues = test.benchValues;
                double workLoad = Math.max(1, test.workLoad);
                double workDone = 0;

                for (int j = 0; j < test.benchLength; j++) {

                    int n = benchValues[j];

                    Iterator<Candidate> selectedCandidates = Candidate.getSelected();
                    while (selectedCandidates.hasNext()) {
                        if (isCancelled()) {
                            return null;
                        }

                        Candidate cand = selectedCandidates.next();
                        doTest(cand, n, test.showFullValue, test.verbose);

                        // Report progress as a percentage of the total task.
                        workDone += n * cand.workLoad;
                        int percentCompleted = Math.min(100, (int) (100 * workDone / workLoad));
                        setProgress(percentCompleted);
                    }

                    if (test.verbose) {
                        operationCount(n);
                    }
                    relativeRanking(n, test.cardSelected);
                }

                usedTime(benchValues);
                performanceProfile(benchValues);

                String fileName = Results.resultsToFile("FactorialBench", benchValues);
                winsole.writeLine("\nBenchmark was saved to file ");
                winsole.writeLine(fileName);
                winsole.flush();

            } catch (Throwable e) {
                err = e;
                return null;
            }

            return null;
        }
    }

    private void doTest(Candidate cand, int n, boolean showFullValue, boolean verbose) {
        IFactorialFunction f = cand.fun;
        Xint.clearOpCounter();

        watch.clear();
        watch.start();
        Xint nFact = f.factorial(n);
        watch.stop();

        long checksum = nFact.crcValue();
        Results res = new Results(cand, watch.getSeconds(), checksum, Xint.getOpCounts());
        cand.results.put(n, res);

        if (verbose) {
            winsole.writeLine();
            winsole.writeLine("SUMMARY: Computed the factorial of");
            winsole.writeLine(n + "! = " + Xmath.asymptFactorial(n));
            winsole.writeLine("Algorithm used: " + f.getName());
            winsole.writeLine("Operations: " + Xint.getOpCountsAsString());
            winsole.writeLine("CheckSum: <" + Long.toHexString(checksum) + ">");
            winsole.writeLine("Computation in " + watch + ".");
        }

        if (showFullValue) {
            winsole.writeLine();
            winsole.writeLine("Now converting to String. Note: It takes longer to convert than to compute!");
            winsole.writeLine(nFact.toString());
        }
    }

    private void relativeRanking(int n, int count) {
        if (n < 1000) {
            return; // too small
        }
        Results[] resultList = new Results[count];

        Results res = Candidate.reference.results.get(n);
        double t = res.sec;
        res.rank = 1.0;
        resultList[0] = res; // include reference in comparison

        int sortLen = 1;
        Iterator<Candidate> challengerCandidates = Candidate.getChallengers();

        while (challengerCandidates.hasNext()) {
            Candidate cand = challengerCandidates.next();
            res = cand.results.get(n);
            res.rank = res.sec / t;
            resultList[sortLen++] = res;
        }

        Results.sort(resultList, sortLen);

        // ============================================
        // "RANKING [n=" + n + "] (rel. to PrimeSwing)"
        // ============================================

        winsole.writeLine();
        winsole.writeLine(dtrenner);
        winsole.writeLine("RANKING [n=" + n + "] (rel. to PrimeSwing)");
        winsole.writeLine(dtrenner);

        boolean flag = true;
        for (Results result : resultList) {
            double r = result.rank;

            if (flag && (r >= 2.05)) {
                winsole.writeLine("------------------------");
                flag = false;
            }

            winsole.writeLine(result.getRankAsString() + " : " + result.creator.getName().trim());
        }
    }

    private void usedTime(int[] benchValues) {
        // ======================================
        // " B E N C H M A R K - T I M I N G S  "
        // ======================================
        // TestValuesToString(benchValues)
        // ======================================

        winsole.writeLine();
        winsole.writeLine(dtrenner);
        winsole.writeLine("  B E N C H M A R K - T I M I N G S (sec.)");
        winsole.writeLine(dtrenner);
        winsole.writeLine(testValuesToString(benchValues));
        winsole.writeLine(dtrenner);

        int i = 0;
        Iterator<Candidate> slectedCandidates = Candidate.getSelected();

        while (slectedCandidates.hasNext()) {
            Candidate cand = slectedCandidates.next();
            winsole.write(cand.getName());

            for (int benchValue : benchValues) {
                Results res = cand.results.get(benchValue);
                winsole.write(res.getTimeAsString());
            }

            winsole.writeLine();
            if (i++ == 4) {
                winsole.writeLine(etrenner);
            }
        }

        winsole.writeLine(dtrenner);
    }

    private void performanceProfile(int[] benchValues) {
        // =======================================
        // "P E R F O R M A N C E - P R O F I L E"
        // =======================================
        // TestValuesToString(benchValues)
        // =======================================

        winsole.writeLine();
        winsole.writeLine(dtrenner);
        winsole.writeLine("P E R F O R M A N C E - P R O F I L E");
        winsole.writeLine(dtrenner);
        winsole.writeLine(testValuesToString(benchValues));
        winsole.writeLine(dtrenner);

        int i = 0;

        Iterator<Candidate> selectedCandidates = Candidate.getSelected();
        while (selectedCandidates.hasNext()) {
            Candidate cand = selectedCandidates.next();
            winsole.write(cand.getName());

            for (int benchValue : benchValues) {
                Results res = cand.results.get(benchValue);
                winsole.write(res.getRankAsString());
            }

            winsole.writeLine();
            if (i++ == 5) {
                winsole.writeLine(etrenner);
            }
        }
        winsole.writeLine(dtrenner);
    }

    private void operationCount(int n) {
        // ============================================
        // "OPERATION COUNT [n = " + n + " ]"
        // "  MUL    mul    DIV    div    Sqr    Lsh  "
        // ============================================

        winsole.writeLine();
        winsole.writeLine(dtrenner);
        winsole.writeLine("OPERATION COUNT [n = " + n + " ]    ");
        winsole.writeLine(Results.nopHeader);
        winsole.writeLine(dtrenner);

        Iterator<Candidate> selectedCandidates = Candidate.getSelected();
        while (selectedCandidates.hasNext()) {
            Candidate cand = selectedCandidates.next();
            Results res = cand.results.get(n);
            winsole.writeLine(cand.getName());
            winsole.writeLine(res.nopsAsString());
        }
    }

    private static String testValuesToString(int[] val) {
        StringBuilder sb = new StringBuilder("   n * 1000, n = ");
        for (int aVal : val) {
            sb.append("   ").append(aVal / 1000);
        }
        return sb.toString();
    }

    private void sanityCheck(int length) {
        boolean ok = true;
        for (int n = 0; n < length; n++) {
            Xint r = Candidate.reference.fun.factorial(n);

            Iterator<Candidate> testCandidates = Candidate.getSanity();
            while (testCandidates.hasNext()) {
                Candidate cand = testCandidates.next();
                Xint t = cand.fun.factorial(n);
                if (t.compareTo(r) != 0) {
                    winsole.writeLine(cand.getName().trim() + "(" + n + ") failed!");
                    ok = false;
                }
            }
            if ((n % 10) == 0) {
                winsole.write(" . ");
                winsole.flush();
            }
            if ((n % 150) == 149) {
                winsole.writeLine();
            }
        }

        winsole.writeLine();
        winsole.writeLine("Well, some values will" + (ok ? " " : " not ")
                + "give correct results  " + (ok ? ";-)" : "~:("));
    }

    void saveToFile(int n) throws IOException {
        Xint factorial = Candidate.reference.fun.factorial(n);

        String fileName = "FactorialOf " + n + ".txt";
        PrintWriter factorialReport = new PrintWriter(fileName);

        factorialReport.println("Computed the factorial of ");
        factorialReport.println(n + "! = " + Xmath.asymptFactorial(n));
        factorialReport.println("CheckSum: <" + Long.toHexString(factorial.crcValue()) + ">");
        factorialReport.println();

        FactorialFactors f = new FactorialFactors(n);
        f.writeFactors(factorialReport);

        factorialReport.println();
        factorialReport.println(factorial.toString());
        factorialReport.close();

        winsole.writeLine("Factorial was saved to file: ");
        winsole.writeLine(fileName);
        winsole.writeLine();
    }
    private final static String dtrenner = "================================================";
    private final static String etrenner = "------------------------------------------------";
} // endOfFactorialTest
