using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace SilverFactorial
{
    internal class TestParameters
    {
        public const int BENCH_MAX = int.MaxValue;

        public bool[] selectedAlgo;
        public int benchLength;
        public int benchStart;
        public int stepFactor;
        public bool showFullValue;
        public bool verbose;
        public bool sanityTest;

        public double workLoad;
        public int[] benchValues;
        public int cardSelected;

        public TestParameters(int noOfCandidates)
        {
            selectedAlgo = new bool[noOfCandidates];
        }

        public void Init(IEnumerable selCand)
        {
            benchValues = new int[benchLength];
            double sum = 0;
            long value = benchStart;

            for (int m = 0; m < benchLength; m++)
            {
                if (value < BENCH_MAX)
                {
                    benchValues[m] = (int)value;
                    sum += value;
                }
                else
                {
                    benchValues[m] = 1;
                }
                value = (long)((stepFactor * value) / 10.0);
            }

            cardSelected = 0; workLoad = 0;

            foreach (Candidate cand in Candidate.Selected)
            {
                cardSelected++;
                workLoad += cand.WorkLoad * sum;
            }
        }
    }
}