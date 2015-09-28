
namespace SilverFactorial.Benchmark
{
    internal class TestParameters
    {
        public const int TestMax = int.MaxValue;
        
        public int TestLength;
        public int TestStart;
        public int StepFactor;
        public static int[] TestValues;
        public static int CardSelected;
        public bool[] AlgoSelected;
        public bool ShowFullValue;
        public bool Verbose;
        public bool SanityTest;
        public double WorkLoad;

        public TestParameters(int noOfCandidates)
        {
            this.AlgoSelected = new bool[noOfCandidates];
        }

        public void Init() 
        {
            TestValues = new int[this.TestLength];
            double sum = 0;
            long value = this.TestStart;

            for (int m = 0; m < this.TestLength; m++)
            {
                if (value < TestMax)
                {
                    TestValues[m] = (int)value;
                    sum += value;
                }
                else
                {
                    TestValues[m] = 1;
                }
                value = (long)((this.StepFactor * value) / 10.0);
            }

            CardSelected = 0; this.WorkLoad = 0;

            foreach (Candidate cand in Candidate.Selected)
            {
                CardSelected++;
                this.WorkLoad += cand.WorkLoad * sum;
            }
        }
    }
}
