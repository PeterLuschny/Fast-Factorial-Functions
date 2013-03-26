
namespace SilverFactorial
{
    internal class TestParameters
    {
        public const int TEST_MAX = int.MaxValue;
        
        public int testLength;
        public int testStart;
        public int stepFactor;
        public static int[] testValues;
        public static int cardSelected;
        public bool[] algoSelected;
        public bool showFullValue;
        public bool verbose;
        public bool sanityTest;
        public double workLoad;

        public TestParameters(int noOfCandidates)
        {
            algoSelected = new bool[noOfCandidates];
        }

        public void Init() 
        {
            testValues = new int[testLength];
            double sum = 0;
            long value = testStart;

            for (int m = 0; m < testLength; m++)
            {
                if (value < TEST_MAX)
                {
                    testValues[m] = (int)value;
                    sum += value;
                }
                else
                {
                    testValues[m] = 1;
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
