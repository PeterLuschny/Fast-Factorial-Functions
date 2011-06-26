// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.apps.factorial;

import java.util.Iterator;

public class FactorialTest {

    public static int BenchMax = 100000000;
    public boolean[] selectedAlgo;
    public int benchLength;
    public int benchStart;
    public double stepFactor;
    public boolean showFullValue;
    public boolean verbose;
    public double workLoad;
    public int[] benchValues;
    public int cardSelected;
    public boolean sanityTest;

    public void init() {
        Candidate.setSelected(selectedAlgo);

        benchValues = new int[benchLength];
        double sum = 0;
        long value = benchStart;

        for (int m = 0; m < benchLength; m++) {
            if (value < BenchMax) {
                benchValues[m] = (int) value;
                sum += value;
            } else {
                benchValues[m] = 1;
            }
            value = (long) (value * stepFactor);
        }

        cardSelected = 0;
        workLoad = 0;

        Iterator<Candidate> selectedCandidates = Candidate.getSelected();
        while (selectedCandidates.hasNext()) {
            Candidate cand = selectedCandidates.next();
            cardSelected++;
            workLoad += cand.workLoad * sum;
        }
    }

    // Do not expose the class Candidate to the BenchmarkForm
    public static boolean[] getPrimeAlgos() {
        return Candidate.getPrimeAlgos();
    }

    public static boolean[] getSimpleAlgos() {
        return Candidate.getSimpleAlgos();
    }

    public static boolean[] getRecommendedAlgos() {
        return Candidate.getRecommendedAlgos();
    }

    public static boolean[] getLameAlgos() {
        return Candidate.getLameAlgos();
    }

    public static boolean[] getParallelAlgos() {
        return Candidate.getParallelAlgos();
    }

    public static String[] getNames() {
        return Candidate.getNames();
    }
}
