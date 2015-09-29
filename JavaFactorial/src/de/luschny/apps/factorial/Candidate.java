// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.apps.factorial;

import de.luschny.math.factorial.*;

import java.util.Hashtable;
import java.util.Iterator;
import java.util.NoSuchElementException;

public class Candidate {

    final IFactorialFunction fun;
    final Hashtable<Integer, Results> results;
    private final String name;
    private final Character id;
    public final int workLoad;
    private boolean benchmark;
    private final boolean primeType;
    private final boolean topFive;
    private final boolean simple;
    private final boolean concur;

    private Candidate(IFactorialFunction f, String name, boolean b, boolean p,
                      boolean t, boolean s, boolean c, int w, char hotkey) {
        results = new Hashtable<>();

        fun = f;
        this.name = name;
        benchmark = b;
        primeType = p;
        topFive = t;
        simple = s;
        concur = c;
        workLoad = w;
        id = Character.toUpperCase(hotkey);
    }

    public String getName() {
        return fun.getName();
    }

    public Character getId() {
        return id;
    }

    public void setBenchmark(boolean c) {
        benchmark = c;
    }

    public boolean isRecommended() {
        return topFive;
    }

    public boolean isPrimeType() {
        return primeType;
    }

    public boolean isSimple() {
        return simple;
    }
    private static final Candidate[] candArray = new Candidate[]{
        // notlame,primeType,recommended,simple,concur
        new Candidate(new FactorialParallelPrimeSwing(), "ParallelPrimeSwing", true, true, true, false, true, 1, 'u'), // 0
        new Candidate(new FactorialParallelPrimeSplit(), "ParallelPrimeSplit", true, true, false, false, true, 1, 't'), // 1
        new Candidate(new FactorialParallelSwing(), "ParallelSwing    ", true, false, false, false, true, 1, 'r'), // 2
        new Candidate(new FactorialParallelSplit(), "ParallelSplit     ", true, false, true, false, true, 1, 's'), // 3
        new Candidate(new FactorialPrimeSwing(), "PrimeSwing        ", true, true, true, false, false, 1, 'b'), // 4
        new Candidate(new FactorialPrimeSchoenhage(), "PrimeShoenhage    ", true, true, false, false, false, 1, 'a'), // 5
        //new Candidate(new FactorialXPrimeSchoenhage(), "XPrimeShoenhage   ", true, true, false, false, false, 1, 'c'), // 5
        new Candidate(new FactorialPrimeSwingList(), "PrimeSwingList    ", true, true, false, false, false, 1, 'c'), // 6
        new Candidate(new FactorialPrimeSwingCache(), "PrimeSwingCache   ", true, true, false, false, false, 1, 'd'), // 7
        new Candidate(new FactorialPrimeVardi(), "PrimeVardi        ", true, true, false, false, false, 2, 'e'), // 8
        new Candidate(new FactorialPrimeLeenstra(), "PrimeLeenstra     ", true, true, false, false, false, 2, 'f'), // 9
        new Candidate(new FactorialPrimeBorwein(), "PrimeBorwein      ", true, true, false, false, false, 4, 'g'), // 10
        new Candidate(new FactorialSplit(), "Split             ", true, false, true, true, false, 4, 'h'), // 11
        new Candidate(new FactorialSwing(), "Swing             ", true, false, false, true, false, 30, 'o'), // 12
        new Candidate(new FactorialSquaredDiffProd(), "SquaredDiffProd   ", true, false, false, true, false, 9, 'j'), // 13
        new Candidate(new FactorialHyper(), "Hyper             ", true, false, false, false, false, 8, 'i'), // 14
        new Candidate(new FactorialProductRecursive(), "ProductRecursive  ", false, false, false, false, false, 10, 'k'),// 15
        new Candidate(new FactorialSwingRationalDouble(), "SwingRationalDbl  ", false, false, false, false, false, 15, 'l'),// 16
        new Candidate(new FactorialSwingRational(), "SwingRational     ", false, false, false, false, false, 16, 'm'),// 17
        new Candidate(new FactorialSwingDouble(), "SwingDouble       ", false, false, false, false, false, 25, 'n'),// 18
        new Candidate(new FactorialBoitenSplit(), "BoitenSplit       ", false, false, false, false, false, 45, 'p'),// 19
        new Candidate(new FactorialSquaredDiff(), "SquaredDiff       ", false, false, false, false, false, 50, 'q') // 20
    // new Candidate(new FactorialProductNaive(), false,false, false, false, 80, 'r'), //21
    // new Candidate(new FactorialDifference(), false,false, false, false, 120, 's'), //22
    // new Candidate(new FactorialAdditiveSwing(), false,false, false, false, 150, 't'), //23
    // new Candidate(new FactorialAdditiveMoessner(),false, false, false, false, 200, 'u') //24
    };
    private static boolean[] selected;

    static public void setSelected(boolean[] sel) {
        selected = sel;
    }
    static final int IndexOfReference = 4;
    static final Candidate reference = candArray[IndexOfReference];

    static public Iterator<Candidate> getSelected() {
        return new Iterator<Candidate>() {

            int currentIndex = -1;

            @Override
            public boolean hasNext() {
                while (++currentIndex < candArray.length) {
                    if (selected[currentIndex]) {
                        return true;
                    }
                }
                currentIndex = -1;
                return false;
            }

            @Override
            public Candidate next() {
                if (currentIndex == -1) {
                    throw new NoSuchElementException();
                }

                return candArray[currentIndex];
            }

            @Override
            public void remove() {
                throw new java.lang.UnsupportedOperationException();
            }
        };
    }

    static public Iterator<Candidate> getSanity() {
        return new Iterator<Candidate>() {

            int currentIndex = -1;

            @Override
            public boolean hasNext() {
                while (++currentIndex < candArray.length) {
                    if (candArray[currentIndex].benchmark) {
                        return true;
                    }
                }
                currentIndex = -1;
                return false;
            }

            @Override
            public Candidate next() {
                if (currentIndex == -1) {
                    throw new NoSuchElementException();
                }

                return candArray[currentIndex];
            }

            @Override
            public void remove() {
                throw new java.lang.UnsupportedOperationException();
            }
        };
    }

    static public Iterator<Candidate> getChallengers() {
        return new Iterator<Candidate>() {

            int currentIndex = -1;

            @Override
            public boolean hasNext() {
                while (++currentIndex < candArray.length) {
                    if (selected[currentIndex] && currentIndex != IndexOfReference) {
                        return true;
                    }
                }
                currentIndex = -1;
                return false;
            }

            @Override
            public Candidate next() {
                if (currentIndex == -1) {
                    throw new NoSuchElementException();
                }

                return candArray[currentIndex];
            }

            @Override
            public void remove() {
                throw new java.lang.UnsupportedOperationException();
            }
        };
    }

    static boolean[] getPrimeAlgos() {
        boolean[] PrimeAlgos = new boolean[candArray.length];

        int i = 0;
        for (Candidate cand : candArray) {
            PrimeAlgos[i++] = cand.primeType;
        }
        return PrimeAlgos;
    }

    static boolean[] getSimpleAlgos() {
        boolean[] SimpleAlgos = new boolean[candArray.length];

        int i = 0;
        for (Candidate cand : candArray) {
            SimpleAlgos[i++] = cand.simple;
        }
        return SimpleAlgos;
    }

    static boolean[] getRecommendedAlgos() {
        boolean[] RecommendedAlgos = new boolean[candArray.length];

        int i = 0;
        for (Candidate cand : candArray) {
            RecommendedAlgos[i++] = cand.topFive;
        }
        return RecommendedAlgos;
    }

    static boolean[] getLameAlgos() {
        boolean[] LameAlgos = new boolean[candArray.length];

        int i = 0;
        for (Candidate cand : candArray) {
            LameAlgos[i++] = !cand.benchmark;
        }
        return LameAlgos;
    }

    static boolean[] getParallelAlgos() {
        boolean[] ParallelAlgos = new boolean[candArray.length];

        int i = 0;
        for (Candidate cand : candArray) {
            ParallelAlgos[i++] = cand.concur;
        }
        return ParallelAlgos;
    }

    static String[] getNames() {
        String[] AlgoNames = new String[candArray.length];

        int i = 0;
        for (Candidate cand : candArray) {
            AlgoNames[i++] = cand.name;
        }
        return AlgoNames;
    }
}
