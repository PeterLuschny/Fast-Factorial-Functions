// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

using System;
using System.IO;
using System.Text;
using System.ComponentModel;
using Sharith.Arithmetic;
using Sharith.Math.MathUtils;

namespace SilverFactorial
{
    internal class BenchmarkWorker
    {
        static string nl = Environment.NewLine;

        LoggedTextBox winsole;
        System.Diagnostics.Stopwatch watch;

        public BenchmarkWorker(LoggedTextBox ws)
        {
            winsole = ws;
            watch = new System.Diagnostics.Stopwatch();
        }

        // The only connection with the GUI.
        public int DoTheBenchmark(BackgroundWorker worker, DoWorkEventArgs workEvent, TestParameters test)
        {
            if (test.sanityTest)
            {
                winsole.WriteLine("\nSanity check is running!\n");
                SanityCheck(500); 
                return 1;
            }

            Candidate.SetSelected(test.selectedAlgo);
            test.Init(Candidate.Selected);

            int[] benchValues = test.benchValues;
            double workLoad = test.workLoad, workDone = 0;

            for (int j = 0; j < test.benchLength; j++)
            {
                int n = benchValues[j];

                foreach (Candidate cand in Candidate.Selected)
                {
                    if (worker.CancellationPending)
                    {
                        workEvent.Cancel = true;
                        return 0;
                    }
                    else
                    {
                        DoTest(cand, n, test.showFullValue, test.verbose);

                        // Report progress as a percentage of the total task.
                        workDone += n * cand.WorkLoad;
                        int percentComplete = (int)(100 * workDone / workLoad);
                        worker.ReportProgress(System.Math.Min(100, percentComplete));
                    }
                }

                if (test.verbose)
                {
                    // TODO: OperationCount(n);
                }
                RelativeRanking(n, test.cardSelected);
            }
            UsedTime(benchValues, test.benchStart);
            PerformanceProfile(benchValues, test.benchStart);

            string outputDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\FactorialBenchmarks\";
            var df = new DirectoryInfo(outputDir);
            if (!df.Exists)
            {
                df = Directory.CreateDirectory(df.FullName);
            }

            string fileName = string.Format(df.FullName + "FactorialBenchmark{0}.html", DateTime.Now.ToFileTime());
            Results.ResultsToFile(fileName, benchValues);
            winsole.WriteLine("\n\nBenchmark was saved to file \n" + fileName);
            winsole.WriteLine("\n\n");
            winsole.Flush();

            try { System.Diagnostics.Process.Start(fileName); }
            catch (System.ComponentModel.Win32Exception) { }
            catch (System.Exception) { }  
				
            return 1;
        }

        void DoTest(Candidate cand, int n, bool showFullValue, bool verbose)
        {
            watch.Reset();
            watch.Start();
            XInt nFact = cand.GetValue(n);
            watch.Stop();

            int checksum = nFact.GetHashCode();
            var res = new Results(cand, watch.ElapsedMilliseconds, checksum); 
            cand.results[n] = res;

            if (verbose)
            {
                winsole.WriteRed(string.Format(
                    "\nSUMMARY: Computed the factorial \n{0}! = {1}\nAlgorithm used: {2}\nCheckSum: <{3:X}>\nComputation in {4:D} ms.\n",
                    n, XMath.AsymptFactorial(n), cand.Name, checksum, watch.ElapsedMilliseconds));
            }

            if (showFullValue)
            {
                winsole.Write(nl + "Now converting to string. Note: It takes longer to convert than to compute!" + nl + nl);
                winsole.WriteLine(nFact.ToString());
            }
        }

        void RelativeRanking(int n, int count)
        {
            if (n < 1000)
            {
                winsole.WriteLine("\nTiming too inaccurate. Result not included.\n");
                winsole.WriteLine("Please use graeter values for n.");
                return; 
            }

            var resultList = new Results[count];

            Results res = (Results)Candidate.reference.results[n];
            double t = Math.Max(res.ms, double.Epsilon);
            res.Rank = 1.0;
            resultList[0] = res; // include reference in comparison            

            int sortLen = 1;
            foreach (Candidate cand in Candidate.Challengers)
            {
                res = (Results)cand.results[n];
                double r = Math.Max(res.ms, double.Epsilon) / t;
                res.Rank = r;
                resultList[sortLen++] = res;
            }

            Results.Sort(resultList, sortLen);

            // ==================================================
            // "RANKING [n=" + n + "] (rel. to PrimeSwing)"
            // ==================================================

            winsole.Write(string.Format(
                nl + "{0}\nRANKING [n={1}] (rel. to PrimeSwing = 1.00)\n{2}" + nl,
                seperatorD, n, seperatorD));

            bool flag = true;
            foreach (var result in resultList)
            {
                double r = result.Rank;

                if (flag && (r >= 2.05))
                {
                    winsole.Write("------------------------" + nl);
                    flag = false;
                }

                winsole.Write(string.Format("{0} : {1}\n",
                    result.GetRankAsString2(), result.cand.Name.Trim()));
            }
            winsole.WriteLine(seperatorS + "\n");
        }

        void UsedTime(int[] benchValues, int benchStart)
        {
            // ======================================
            // "  B E N C H M A R K - T I M I N G S "
            // ======================================
            //    TestValuesToString(benchValues)
            // ======================================

            winsole.Write(string.Format(
            "\n\n{0}\nB E N C H M A R K - T I M I N G S (sec)\n{1}\n{2}\n",
            seperatorD, TestValuesToString(benchValues, benchStart), seperatorD));

            for (int i = 0; i < Candidate.rankList.Length; i++)
            {
                Candidate cand = Candidate.candList[Candidate.rankList[i]];
                winsole.WriteRed(cand.Name);

                for (int k = 0; k < benchValues.Length; k++)
                {
                    Results res = (Results)cand.results[benchValues[k]];
                    winsole.WriteRed(res.GetTimeAsString());
                }
                winsole.WriteLine();
            }

            winsole.WriteLine(seperatorD);
        }

        void PerformanceProfile(int[] benchValues, int benchStart)
        {
            // =======================================
            // "P E R F O R M A N C E - P R O F I L E"
            // =======================================
            //    TestValuesToString(benchValues)
            // =======================================

            winsole.Write(string.Format(
                "\n{0}\nP E R F O R M A N C E - P R O F I L E\n{1}\n{2}\n",
                seperatorD, TestValuesToString(benchValues, benchStart), seperatorD));

            int l = 0;

            for(int i = 0; i < Candidate.rankList.Length; i++)
            {
                Candidate cand = Candidate.candList[Candidate.rankList[i]];
                winsole.WriteRed(cand.Name);

                for (int k = 0; k < benchValues.Length; k++)
                {
                    Results res = (Results)cand.results[benchValues[k]];
                    winsole.WriteRed(res.GetRankAsString());
                }

                winsole.WriteLine();
                if (l++ == 3)
                {
                    winsole.WriteLine(seperatorS);
                }
            }
            winsole.WriteLine(seperatorD);
        }

        static string TestValuesToString(int[] val, int benchStart)
        {
            int scale = (int)Math.Pow(10,(int)Math.Log10(Math.Max(100,benchStart)));
            var sb = new StringBuilder(string.Format("n x {0:D},  n =  ",scale));
            for (int i = 0; i < val.Length; i++)
            {
                sb.Append(string.Format("{0:D}", val[i] / scale).PadLeft(5, ' '));
            }
            return sb.ToString();
        }

        public void SanityCheck(int length)
        {
            bool ok = true;
            for (int n = 0; n < length; n++)
            {
                XInt r = Candidate.reference.GetValue(n);

                foreach (Candidate cand in Candidate.Sanity)
                {
                    XInt t = cand.GetValue(n);
                    if (!t.Equals(r))
                    {
                        winsole.WriteLine(string.Format(
                            "\n{0}({1}) failed!", cand.Name.Trim(), n));
                        ok = false;
                    }
                }
                if ((n % 10) == 0) winsole.WriteFlush(" . ");
                if ((n % 150) == 149) winsole.WriteLine();
            }

            winsole.Write("\nWell, some values will" + (ok ? " " : " not ") +
                "give correct results  " + (ok ? ";-)\n" : "~:(\n"));
        }

        // // TODO: Operation counts.
        //void OperationCount(int n)
        //{
        //    // ============================================
        //    // "  OPERATION COUNT [n = " + n + " ]"
        //    // "  MUL    mul    DIV    div    Sqr    Lsh  "
        //    // ============================================

        //    winsole.Write(string.Format(
        //        "\n{0}\nOPERATION COUNT [n = {1}]\n",
        //        seperatorD, n));

        //    winsole.WriteLine(Results.opBanner);
        //    winsole.WriteLine(seperatorD);

        //    foreach (Candidate cand in Candidate.Selected)
        //    {
        //        Results res = (Results)cand.results[n];
        //        winsole.WriteLine(cand.Name);
        //        winsole.WriteLine(res.GetOpsAsString());
        //    }
        //}

        // // TODO: Save single value.
        //public void SaveToFile(int n)
        //{
        //    var factorial = Candidate.reference.GetValue(n);

        //    string fileName = string.Format("FactorialOf{0}.txt", n);
        //    var file = new FileInfo(@fileName);
        //    StreamWriter factorialReport = file.AppendText();

        //    factorialReport.WriteLine("Computed the factorial of ");
        //    factorialReport.WriteLine("{0}! = {1}", n, XMath.AsymptFactorial(n));
        //    factorialReport.WriteLine("CheckCode: <{0:X}> ", factorial.GetHashCode());
        //    factorialReport.WriteLine();

        //    var f = new FactorialFactors(n);
        //    f.WriteFactors(factorialReport);

        //    factorialReport.WriteLine();
        //    factorialReport.WriteLine(factorial.ToString());
        //    factorialReport.Close();

        //    winsole.WriteLine("Factorial was saved to file \n");
        //    winsole.WriteLine(fileName);
        //    winsole.WriteLine();
        //}

        private static string seperatorD = "================================================";
        private static string seperatorS = "------------------------------------------------";
    }
} // endOfFactorialTest
