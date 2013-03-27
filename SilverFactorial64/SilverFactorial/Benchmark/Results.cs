// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace SilverFactorial
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;

    class Result : IComparable
    {
        public Candidate cand;

        public long ms;
        public long crc;
        public long eddms;

        public double Rank { get; set; }
        public int AbsRank { get; set; }

        public static int[] relRankList;
        // Item1 = average efficiency, Item2 = index of candidate
        private static Tuple<int, int>[] EffCand;
        
        public Result(Candidate cand, long msec, long check, long eddms)
        {
            this.cand = cand;
            this.ms = msec;
            this.crc = check;
            this.eddms = eddms;
        }

        public override string ToString()
        {
            return string.Format(" {0:D} ms  <{1:X}>", ms, crc);
        }

        public string GetRankAsString()
        {
            if (double.IsInfinity(Rank) || double.IsNaN(Rank)) Rank = 0;
            return string.Format(" {0:F1}", Rank).PadLeft(5, ' ');
        }

        public string GetRankAsString2()
        {
            if (double.IsInfinity(Rank) || double.IsNaN(Rank)) Rank = 0;
            return string.Format(" {0:0.00}", Rank).PadLeft(5, ' ');
        }

        public string GetTimeAsString()
        {
            return string.Format("{0:0.0}", (ms / 1000.00)).PadLeft(5, ' ');
        }

        public string GetTimeAsString2()
        {
            return string.Format("{0:0.00}", (ms / 1000.00)).PadLeft(5, ' ');
        }

        private static string DateTimeString()
        {
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", false).DateTimeFormat;
            DateTime dateTime = DateTime.Now;
            return dateTime.ToString("u", dateTimeFormat);
        }

        public string GetDecimalDigitsPerMillisecondAsString()
        {
            return string.Format("{0}", eddms);
        }

        private static int Compare(Tuple<int, int> x, Tuple<int, int> y)
        {
            if (x.Item1 == y.Item1) return 0;
            if (x.Item1 <  y.Item1) return 1;
            return -1;
        }

        private static void AverageEfficiency()
        {
            EffCand = new Tuple<int, int>[TestParameters.cardSelected];
            int[] testValues = TestParameters.testValues;
            int i = 0;

            foreach (Candidate cand in Candidate.Selected)
            {
                int sum = 0; bool first = true;

                foreach (var value in testValues)
                {
                    if (first) { first = false; continue; }
                    Result res = (Result)cand.performance[value];
                    var eddms = (int) res.eddms;
                    sum += eddms;
                }
                var anz = (testValues.Length - 1);
                var val = (anz == 0) ? sum : sum / anz;
                EffCand[i++] = new Tuple<int, int>(val, cand.Index);
            }
            Array.Sort(EffCand, Compare);
        }

        // IComparable member.
        public int CompareTo(Object o)
        {
            double or = ((Result)o).Rank;
            if (or == Rank) return 0;
            if (or < Rank) return 1;
            return -1;
        }

        // Uses CompareTo.
        private static void InsertSort(Result[] r, int low, int high)
        {
            for (int i = low; i < high; i++)
            {
                for (int j = i; j > low && (r[j - 1]).CompareTo(r[j]) > 0; j--)
                {
                    Result t = r[j]; r[j] = r[j - 1]; r[j - 1] = t;
                }
            }
            return;
        }

        public static void Sort(Result[] a, int toIndex)
        {
            CheckRange(a.Length, 0, toIndex);
            InsertSort(a, 0, toIndex);

            relRankList = new int[toIndex];
            for (int i = 0; i < toIndex; i++)
            {
                a[i].AbsRank = i + 1;
                relRankList[i] = a[i].cand.Index;
            }
        }

        private static void CheckRange(int arrayLen, int fromIndex, int toIndex)
        {
            if (fromIndex > toIndex)
                throw new System.ArgumentOutOfRangeException("n", "fromIndex("
                    + fromIndex + ") > toIndex(" + toIndex + ")");
            if (fromIndex < 0)
                throw new System.ArgumentOutOfRangeException(fromIndex.ToString());
            if (toIndex > arrayLen)
                throw new System.ArgumentOutOfRangeException(toIndex.ToString());
        }

        public static void ResultsToFile(string fileName)
        {
            var file = new FileInfo(@fileName);
            StreamWriter benchReport = file.AppendText();
            AverageEfficiency();
            WriteResults(benchReport);
            benchReport.Close();
        }

        static void WriteResults(StreamWriter file)
        {
            int[] testValues = TestParameters.testValues;

            foreach (var item in htmlHeader)
            {
                file.WriteLine(item);
            }

            for (int j = 0; j < 3; j++)
            {
                if (j == 0)
                {
                    file.WriteLine("<h3 align=\"center\">");
                    file.WriteLine("Timings (in seconds)");
                    file.WriteLine("</h3>");
                    file.WriteLine("<table cellpadding=\"4\" align=\"center\" border=\"0\" " +
                        "summary=\"benchmark results timings\">");
                }
                if (j == 2)
                {
                    file.WriteLine("<h3 align=\"center\">");
                    file.WriteLine("Efficiency (decimal digits per millisecond)");
                    file.WriteLine("</h3>");
                    file.WriteLine("<table cellpadding=\"4\" align=\"center\" border=\"0\" " +
                        "summary=\"benchmark results efficiency\">");
                }
                if (j == 1)
                {
                    file.WriteLine("<h3 align=\"center\">");
                    file.WriteLine("Time-ranking relative to 'ParallelPrimeSwing'");
                    file.WriteLine("</h3>");
                    file.WriteLine("<table cellpadding=\"4\" align=\"center\" border=\"0\" " +
                        "summary=\"benchmark results ranking\">");
                }

                file.WriteLine("<tbody>");
                file.WriteLine("<tr class=\"count\">");

                file.WriteLine("<td class=\"head\"> n! where n = </td>");
                foreach (var value in testValues)
                {
                    file.Write("<td class=\"head\">");
                    file.Write(value);
                    file.WriteLine("</td>");
                }

                file.WriteLine("</tr>");

                for (int i = 0; i < TestParameters.cardSelected; i++)
                {
                    Candidate cand = Candidate.candList[EffCand[i].Item2];

                    file.WriteLine("<tr class=\"count\">");

                    file.Write("<td class=\"fact\">");
                    file.Write(cand.Name.Trim());
                    file.WriteLine("</td>");

                    foreach (var value in testValues)
                    {
                        Result res = (Result)cand.performance[value];

                        if (j == 0)
                        {
                            if (res.AbsRank == 1) file.Write("<td class=\"count1\">");
                            else if (res.AbsRank == 2) file.Write("<td class=\"count2\">");
                            else file.Write("<td class=\"count\">");
                            file.Write(res.GetTimeAsString2());
                        }
                        else if (j == 2)
                        {
                            if (res.AbsRank == 1) file.Write("<td class=\"count1\">");
                            else if (res.AbsRank == 2) file.Write("<td class=\"count2\">");
                            else file.Write("<td class=\"count\">");
                            file.Write(res.GetDecimalDigitsPerMillisecondAsString());
                        }
                        else if (j == 1)
                        {
                            if (res.Rank <= 1.0) file.Write("<td class=\"count1\">");
                            else if (res.Rank <= 2.0) file.Write("<td class=\"count2\">");
                            else file.Write("<td class=\"count\">");
                            file.Write(res.GetRankAsString2());
                        }

                        file.WriteLine("</td>");
                    }
                    file.WriteLine("</tr>");
                }

                file.WriteLine("</tbody>");
                file.WriteLine("</table>");

                if (j == 0)
                {
                    file.WriteLine("<h5 align=\"center\">");
                    file.WriteLine("The smaller the value the better.<br>");
                    file.WriteLine("Red = best, blue = second.<br>");
                    file.WriteLine("</h5>");
                }
                else if (j == 2)
                {
                    file.WriteLine("<h5 align=\"center\">");
                    file.WriteLine("The larger the value the better.<br>");
                    file.WriteLine("Red = best, blue = second.<br>");
                    file.WriteLine("</h5>");
                }
                else if (j == 1)
                {
                    file.WriteLine("<h5 align=\"center\">");
                    file.WriteLine("The smaller the value the better.<br>");
                    file.WriteLine("Red values &lt;= 1 indicate excellent performance,<br>");
                    file.WriteLine("Blue values &lt;= 2 indicate good performance.<br>");
                    file.WriteLine("</h5>");
                }
            }

            file.WriteLine("<h3 align=\"center\">");
            file.WriteLine("Ranking by average efficiency in the test range");
            file.WriteLine("<br>");
            file.WriteLine("<span style=\"font-size:small\">(deliberately disregarding the smallest test value) </span>");
            file.WriteLine("</h3>");
            file.WriteLine("<table cellpadding=\"4\" align=\"center\" border=\"0\" " +
                    "summary=\"benchmark results average efficiency\">");
            file.WriteLine("<tbody>");
            file.WriteLine("<tr class=\"count\">");

            file.WriteLine("<td class=\"head\"> Algorithm </td>");
            file.Write("<td class=\"head\">");
            file.Write("Average Efficiency");
            file.WriteLine("</td>");
            file.WriteLine("</tr>");

            for (var k = 0; k < TestParameters.cardSelected; k++)
            {
                Candidate cand = Candidate.candList[EffCand[k].Item2];
                file.WriteLine("<tr>");
                if (k == 0) file.Write("<td class=\"fact1\">");
                else if (k == 1) file.Write("<td class=\"fact2\">");
                else file.Write("<td class=\"fact\">");
                file.Write(cand.Name.Trim());
                file.WriteLine("</td>");

                if (k == 0) file.Write("<td class=\"count1\">");
                else if (k == 1) file.Write("<td class=\"count2\">");
                else file.Write("<td class=\"count\">");
                file.Write(EffCand[k].Item1.ToString());
                file.WriteLine("</td>");
                file.WriteLine("</tr>");
            }

            file.WriteLine("</tbody>");
            file.WriteLine("</table>");
            file.WriteLine("<h5 align=\"center\">");
            file.WriteLine("The larger the value the better.<br>");
            file.WriteLine("</h5>");

            file.WriteLine();
            file.Write("<p></p><p style=\"font-size:x-small\">" + DateTimeString() + ". Visit ");
            file.WriteLine("<a href=\"http://www.luschny.de/math/factorial/FastFactorialFunctions.htm\">FFFunctions</a> for background information.</p>");
            file.WriteLine("</body>");
            file.WriteLine("</html>");
        }

        private static string[] htmlHeader = {
            "<!DOCTYPE html PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\">",
            "<html>","<head>",
            "<meta name=\"GENERATOR\" content=\"Factorial.exe\">",
            "<title>","Factorial Benchmark","</title>",
            "<style type=\"text/css\">", 
            "body { background-color:white; color: black; ",
            "font-family: Monospace, Sans-Serif, Arial, Helvetica, Verdana; }",
            "td.count  { background-color : #c0d9c0; font-weight: bold; text-align: center; }",
            "td.count1 { background-color : #c0d9c0; color : red; font-weight: bold; text-align: center; }",
            "td.count2 { background-color : #c0d9c0; color : blue; font-weight: bold; text-align: center; }",
            "td.fact  { background-color: #f0e68c; text-align: right; font-size:small;",
            "font-family: Arial, Lucida Sans Typewriter;  }",
            "td.fact1  { background-color: #f0e68c; color : red; text-align: right; font-size:small;", 
            "font-family: Arial, Lucida Sans Typewriter;  }",
            "td.fact2  { background-color: #f0e68c; color : blue; text-align: right; font-size:small;",
            "font-family: Arial, Lucida Sans Typewriter;  }",
            "td.head  { background-color: #b0c4de; color:yellow; text-align: center; }",
            "tr.count { text-align: center; font-size:small;}",
            "</style>",
            "</head>","<body>"
       };
    }
}
