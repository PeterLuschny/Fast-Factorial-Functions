// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace SilverFactorial.Benchmark
{
    using System;
    using System.Globalization;
    using System.IO;

    class Result : IComparable
    {
        public readonly Candidate Cand;
        public readonly long Ms;
        public readonly long Crc;
        public readonly long Eddms;

        public double Rank { get; set; }
        public int AbsRank { get; set; }

        public static int[] RelRankList;
        // Item1 = average efficiency, Item2 = index of candidate
        private static Tuple<int, int>[] effCand;
        
        public Result(Candidate cand, long msec, long check, long eddms)
        {
            this.Cand = cand;
            this.Ms = msec;
            this.Crc = check;
            this.Eddms = eddms;
        }

        public override string ToString()
        {
            return $" {this.Ms:D} ms  <{this.Crc:X}>";
        }

        public string GetRankAsString()
        {
            if (double.IsInfinity(this.Rank) || double.IsNaN(this.Rank)) this.Rank = 0;
            return $" {this.Rank:F1}".PadLeft(5, ' ');
        }

        public string GetRankAsString2()
        {
            if (double.IsInfinity(this.Rank) || double.IsNaN(this.Rank)) this.Rank = 0;
            return $" {this.Rank:0.00}".PadLeft(5, ' ');
        }

        public string GetTimeAsString()
        {
            return $"{(this.Ms / 1000.00):0.0}".PadLeft(5, ' ');
        }

        public string GetTimeAsString2()
        {
            return $"{(this.Ms / 1000.00):0.00}".PadLeft(5, ' ');
        }

        private static string DateTimeString()
        {
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", false).DateTimeFormat;
            DateTime dateTime = DateTime.Now;
            return dateTime.ToString("u", dateTimeFormat);
        }

        public string GetDecimalDigitsPerMillisecondAsString()
        {
            return $"{this.Eddms}";
        }

        private static int Compare(Tuple<int, int> x, Tuple<int, int> y)
        {
            if (x.Item1 == y.Item1) return 0;
            if (x.Item1 <  y.Item1) return 1;
            return -1;
        }

        private static void AverageEfficiency()
        {
            effCand = new Tuple<int, int>[TestParameters.CardSelected];
            var testValues = TestParameters.TestValues;
            var i = 0;

            if (testValues.Length == 1)
            {
                foreach (Candidate cand in Candidate.Selected)
                {
                    var res = (Result)cand.Performance[testValues[0]];
                    effCand[i++] = new Tuple<int, int>((int)res.Eddms, cand.Index);
                }
            }
            else
            {
                foreach (Candidate cand in Candidate.Selected)
                {
                    int sum = 0; bool first = true;

                    foreach (var value in testValues)
                    {
                        if (first) { first = false; continue; }
                        var res = (Result)cand.Performance[value];
                        var eddms = (int)res.Eddms;
                        sum += eddms;
                    }
                    var val = sum / (testValues.Length - 1);
                    effCand[i++] = new Tuple<int, int>(val, cand.Index);
                }
            }
            Array.Sort(effCand, Compare);
        }

        // IComparable member.
        public int CompareTo(Object o)
        {
            double or = ((Result)o).Rank;
            if (or == this.Rank) return 0;
            if (or < this.Rank) return 1;
            return -1;
        }

        // Uses CompareTo.
        private static void InsertSort(Result[] r, int low, int high)
        {
            for (var i = low; i < high; i++)
            {
                for (var j = i; j > low && (r[j - 1]).CompareTo(r[j]) > 0; j--)
                {
                    var t = r[j]; r[j] = r[j - 1]; r[j - 1] = t;
                }
            }
            return;
        }

        public static void Sort(Result[] a, int toIndex)
        {
            CheckRange(a.Length, 0, toIndex);
            InsertSort(a, 0, toIndex);

            RelRankList = new int[toIndex];
            for (int i = 0; i < toIndex; i++)
            {
                a[i].AbsRank = i + 1;
                RelRankList[i] = a[i].Cand.Index;
            }
        }

        private static void CheckRange(int arrayLen, int fromIndex, int toIndex)
        {
            if (fromIndex > toIndex)
                throw new System.ArgumentOutOfRangeException(" fromIndex("
                    + fromIndex + ") > toIndex(" + toIndex + ")");
            if (fromIndex < 0)
                throw new System.ArgumentOutOfRangeException(fromIndex.ToString());
            if (toIndex > arrayLen)
                throw new System.ArgumentOutOfRangeException(toIndex.ToString());
        }

        public static void ResultsToFile(string fileName)
        {
            var file = new FileInfo(@fileName);
            var benchReport = file.AppendText();
            AverageEfficiency();
            WriteResults(benchReport);
            benchReport.Close();
        }

        static void WriteResults(StreamWriter file)
        {
            var testValues = TestParameters.TestValues;

            foreach (var item in HtmlHeader)
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

                for (var i = 0; i < TestParameters.CardSelected; i++)
                {
                    var cand = Candidate.CandList[effCand[i].Item2];

                    file.WriteLine("<tr class=\"count\">");

                    file.Write("<td class=\"fact\">");
                    file.Write(cand.Name.Trim());
                    file.WriteLine("</td>");

                    foreach (var value in testValues)
                    {
                        var res = (Result)cand.Performance[value];

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

            for (var k = 0; k < TestParameters.CardSelected; k++)
            {
                var cand = Candidate.CandList[effCand[k].Item2];
                file.WriteLine("<tr>");
                if (k == 0) file.Write("<td class=\"fact1\">");
                else if (k == 1) file.Write("<td class=\"fact2\">");
                else file.Write("<td class=\"fact\">");
                file.Write(cand.Name.Trim());
                file.WriteLine("</td>");

                if (k == 0) file.Write("<td class=\"count1\">");
                else if (k == 1) file.Write("<td class=\"count2\">");
                else file.Write("<td class=\"count\">");
                file.Write(effCand[k].Item1.ToString());
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

        static readonly string[] HtmlHeader = {
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
