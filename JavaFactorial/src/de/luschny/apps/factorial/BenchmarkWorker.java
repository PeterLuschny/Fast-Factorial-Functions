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

    boolean cancelled, done;
    Throwable error;

    public WorkerCompletedEvent(boolean cancelled, boolean done, Throwable error) {
        this.cancelled = cancelled;
        this.done = done;
        this.error = error;
    }
}

class BenchmarkWorker {

    private StopWatch watch;
    private LoggedTextBox Winsole;
    private FactorialTest test;
    private BenchmarkForm monitor; // PropertyChangeListener
    private BenchmarkExecutor executor;

    public BenchmarkWorker(LoggedTextBox ws, BenchmarkForm monitor) {
        Winsole = ws;
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
                    Winsole.WriteLine();
                    Winsole.WriteLine("Sanity check is running!");
                    SanityCheck(1000);
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
                        DoTest(cand, n, test.showFullValue, test.verbose);

                        // Report progress as a percentage of the total task.
                        workDone += n * cand.workLoad;
                        int percentCompleted = Math.min(100, (int) (100 * workDone / workLoad));
                        setProgress(percentCompleted);
                    }

                    if (test.verbose) {
                        OperationCount(n);
                    }
                    RelativeRanking(n, test.cardSelected);
                }

                UsedTime(benchValues);
                PerformanceProfile(benchValues);

                String fileName = Results.resultsToFile("FactorialBench", benchValues);
                Winsole.WriteLine("\nBenchmark was saved to file ");
                Winsole.WriteLine(fileName);
                Winsole.Flush();

            } catch (Throwable e) {
                err = e;
                return null;
            }

            return null;
        }
    }

    void DoTest(Candidate cand, int n, boolean showFullValue, boolean verbose) {
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
            Winsole.WriteLine();
            Winsole.WriteLine("SUMMARY: Computed the factorial of");
            Winsole.WriteLine(n + "! = " + Xmath.asymptFactorial(n));
            Winsole.WriteLine("Algorithm used: " + f.getName());
            Winsole.WriteLine("Operations: " + Xint.getOpCountsAsString());
            Winsole.WriteLine("CheckSum: <" + Long.toHexString(checksum) + ">");
            Winsole.WriteLine("Computation in " + watch + ".");
        }

        if (showFullValue) {
            Winsole.WriteLine();
            Winsole.WriteLine("Now converting to String. Note: It takes longer to convert than to compute!");
            Winsole.WriteLine(nFact.toString());
        }
    }

    void RelativeRanking(int n, int count) {
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
            double r = res.sec / t;
            res.rank = r;
            resultList[sortLen++] = res;
        }

        Results.sort(resultList, sortLen);

        // ============================================
        // "RANKING [n=" + n + "] (rel. to PrimeSwing)"
        // ============================================

        Winsole.WriteLine();
        Winsole.WriteLine(dtrenner);
        Winsole.WriteLine("RANKING [n=" + n + "] (rel. to PrimeSwing)");
        Winsole.WriteLine(dtrenner);

        boolean flag = true;
        for (Results result : resultList) {
            double r = result.rank;

            if (flag && (r >= 2.05)) {
                Winsole.WriteLine("------------------------");
                flag = false;
            }

            Winsole.WriteLine(result.getRankAsString() + " : " + result.creator.getName().trim());
        }
    }

    void UsedTime(int[] benchValues) {
        // ======================================
        // " B E N C H M A R K - T I M I N G S  "
        // ======================================
        // TestValuesToString(benchValues)
        // ======================================

        Winsole.WriteLine();
        Winsole.WriteLine(dtrenner);
        Winsole.WriteLine("  B E N C H M A R K - T I M I N G S (sec.)");
        Winsole.WriteLine(dtrenner);
        Winsole.WriteLine(TestValuesToString(benchValues));
        Winsole.WriteLine(dtrenner);

        int i = 0;
        Iterator<Candidate> slectedCandidates = Candidate.getSelected();

        while (slectedCandidates.hasNext()) {
            Candidate cand = slectedCandidates.next();
            Winsole.Write(cand.getName());

            for (int benchValue : benchValues) {
                Results res = cand.results.get(benchValue);
                Winsole.Write(res.getTimeAsString());
            }

            Winsole.WriteLine();
            if (i++ == 4) {
                Winsole.WriteLine(etrenner);
            }
        }

        Winsole.WriteLine(dtrenner);
    }

    void PerformanceProfile(int[] benchValues) {
        // =======================================
        // "P E R F O R M A N C E - P R O F I L E"
        // =======================================
        // TestValuesToString(benchValues)
        // =======================================

        Winsole.WriteLine();
        Winsole.WriteLine(dtrenner);
        Winsole.WriteLine("P E R F O R M A N C E - P R O F I L E");
        Winsole.WriteLine(dtrenner);
        Winsole.WriteLine(TestValuesToString(benchValues));
        Winsole.WriteLine(dtrenner);

        int i = 0;

        Iterator<Candidate> selectedCandidates = Candidate.getSelected();
        while (selectedCandidates.hasNext()) {
            Candidate cand = selectedCandidates.next();
            Winsole.Write(cand.getName());

            for (int benchValue : benchValues) {
                Results res = cand.results.get(benchValue);
                Winsole.Write(res.getRankAsString());
            }

            Winsole.WriteLine();
            if (i++ == 5) {
                Winsole.WriteLine(etrenner);
            }
        }
        Winsole.WriteLine(dtrenner);
    }

    void OperationCount(int n) {
        // ============================================
        // "OPERATION COUNT [n = " + n + " ]"
        // "  MUL    mul    DIV    div    Sqr    Lsh  "
        // ============================================

        Winsole.WriteLine();
        Winsole.WriteLine(dtrenner);
        Winsole.WriteLine("OPERATION COUNT [n = " + n + " ]    ");
        Winsole.WriteLine(Results.nopHeader);
        Winsole.WriteLine(dtrenner);

        Iterator<Candidate> selectedCandidates = Candidate.getSelected();
        while (selectedCandidates.hasNext()) {
            Candidate cand = selectedCandidates.next();
            Results res = cand.results.get(n);
            Winsole.WriteLine(cand.getName());
            Winsole.WriteLine(res.nopsAsString());
        }
    }

    private static String TestValuesToString(int[] val) {
        StringBuilder sb = new StringBuilder("   n * 1000, n = ");
        for (int aVal : val) {
            sb.append("   ").append(aVal / 1000);
        }
        return sb.toString();
    }

    void SanityCheck(int length) {
        boolean ok = true;
        for (int n = 0; n < length; n++) {
            Xint r = Candidate.reference.fun.factorial(n);

            Iterator<Candidate> testCandidates = Candidate.getSanity();
            while (testCandidates.hasNext()) {
                Candidate cand = testCandidates.next();
                Xint t = cand.fun.factorial(n);
                if (t.compareTo(r) != 0) {
                    Winsole.WriteLine(cand.getName().trim() + "(" + n + ") failed!");
                    ok = false;
                }
            }
            if ((n % 10) == 0) {
                Winsole.Write(" . ");
                Winsole.Flush();
            }
            if ((n % 150) == 149) {
                Winsole.WriteLine();
            }
        }

        Winsole.WriteLine();
        Winsole.WriteLine("Well, some values will" + (ok ? " " : " not ") + "give correct results  "
                + (ok ? ";-)" : "~:("));
    }

    void SaveToFile(int n) throws IOException {
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

        Winsole.WriteLine("Factorial was saved to file: ");
        Winsole.WriteLine(fileName);
        Winsole.WriteLine();
    }
    private static String dtrenner = "================================================";
    private static String etrenner = "------------------------------------------------";
} // endOfFactorialTest
