// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

using System;
using System.Text;
using System.IO;
using System.Globalization;

namespace SilverFactorial
{
    class Results : IComparable
    {
        public long ms;
        public long crc;
        
        public Candidate cand;
        public double Rank { get; set; }
        public int AbsRank { get; set; }

        public Results(Candidate cand, long msec, long check) 
        {
            this.cand = cand;
            this.ms = msec;
            this.crc = check;
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

        public int CompareTo(Object o)
        {
            double or = ((Results)o).Rank;
            if (or == Rank) return 0;
            if (or < Rank) return 1;
            return -1;
        }

        public static void Sort(Results[] a, int toIndex)
        {
            CheckRange(a.Length, 0, toIndex);
            InsertSort(a, 0, toIndex);

            var rankList = new int[toIndex];
            for (int i = 0; i < toIndex; i++)
            {
                a[i].AbsRank = i + 1;
                rankList[i] = a[i].cand.Index;
            }
            Candidate.rankList = rankList;
        }

        private static void InsertSort(Results[] r, int low, int high)
        {
            for (int i = low; i < high; i++)
            {
                for (int j = i; j > low && (r[j - 1]).CompareTo(r[j]) > 0; j--)
                {
                    Results t = r[j]; r[j] = r[j - 1]; r[j - 1] = t;
                }
            }
            return;
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

        public static void ResultsToFile(string fileName, int[] benchValues)
        {
            var file = new FileInfo(@fileName);
            StreamWriter benchReport = file.AppendText();
            WriteResults(benchReport, benchValues);
            benchReport.Close();
        }

        private static string DateTimeString()
        {
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", false).DateTimeFormat;
            DateTime dateTime = DateTime.Now;
            return dateTime.ToString("u", dateTimeFormat);
        }

        static void WriteResults(StreamWriter file, int[] benchValues)
        {
            foreach (var item in htmlHeader)
            {
                file.WriteLine(item);
            }

            for (int j = 0; j < 2; j++)
            {
                if (j == 0)
                {
                    file.WriteLine("<h3 align=\"center\">");
                    file.WriteLine("C# Factorial Benchmark - Timings (in seconds)");
                    file.WriteLine("</h3>");
                    file.WriteLine("<table cellpadding=\"7\" align=\"center\" border=\"0\" " +
                        "summary=\"benchmark results timings\">");
                }
                else
                {
                    file.WriteLine("<br>");
                    file.WriteLine("<h3 align=\"center\">");                  
                    file.WriteLine("C# Factorial Benchmark - Ranking (relative to 'PrimeSwing')");
                    file.WriteLine("</h3>");
                    file.WriteLine("<table cellpadding=\"7\" align=\"center\" border=\"0\" " +
                        "summary=\"benchmark results ranking\">");
                }

                file.WriteLine("<tbody>");
                file.WriteLine("<tr class=\"count\">");
                file.WriteLine("<td class=\"head\"> n! where n = </td>");

                foreach (var value in benchValues)
                {
                    file.Write("<td class=\"head\">");
                    file.Write(value);
                    file.WriteLine("</td>");
                }

                file.WriteLine("</tr>");

                for(int i = 0; i < Candidate.rankList.Length; i++)
                {
                    Candidate cand = Candidate.candList[Candidate.rankList[i]];

                    file.WriteLine("<tr class=\"count\">");

                    file.Write("<td class=\"fact\">");
                    file.Write(cand.Name.Trim());
                    file.WriteLine("</td>");

                    foreach (var value in benchValues)
                    {
                        Results res = (Results)cand.results[value];

                        if (j == 0)
                        {
                            if (res.AbsRank == 1) file.Write("<td class=\"count1\">");
                            else if (res.AbsRank == 2) file.Write("<td class=\"count2\">");
                            else file.Write("<td class=\"count\">");
                            file.Write(res.GetTimeAsString2());
                        }
                        else
                        {
                            if (res.Rank < 1.05) file.Write("<td class=\"count1\">");
                            else if (res.Rank < 2.05) file.Write("<td class=\"count2\">");
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
                    file.WriteLine("Timing: Red = first, blue = second.<br>");
                    file.WriteLine("</h5>");
                }
                if (j == 1)
                {
                    file.WriteLine("<h5 align=\"center\">");
                    file.WriteLine("Ranking: The smaller the value the better.<br>");
                    file.WriteLine("Red values &lt;= 1 indicate excellent performance,<br>");
                    file.WriteLine("Blue values &lt;= 2 indicate good performance.<br>");
                    file.WriteLine("</h5>");
                }
            }

            file.WriteLine();
            file.Write("<p></p><p style=\"font-size:small\">" + DateTimeString() + ". Visit ");
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
            "td.count  { background-color : #c0d9c0; font-weight: bold }",
            "td.count1 { background-color : #c0d9c0; color : red; font-weight: bold }",
            "td.count2 { background-color : #c0d9c0; color : blue; font-weight: bold }",
            "td.fact  { background-color: #f0e68c; text-align: right; ",
            "font-family: Arial, Lucida Sans Typewriter; font-size:small; }",
            "td.head  { background-color: #b0c4de; text-align: center; }",
            "tr.count { text-align: center; }",
            "</style>",
            "</head>","<body>"
       };
    }

    // TODO: operation counts.
    //public string GetOpsAsString()
    //{
    //    if (null == opCounts) return " * ";
    //    var sb = new StringBuilder(80);
    //    foreach(var opcount in opCounts)
    //    {
    //        sb.Append((string.Format("{0:D}|", opcount)).PadLeft(7, ' '));
    //    }
    //    return sb.ToString();
    //}

    //static public string[] opType = { " MUL", " mul", " DIV", " div", " Sqr", " Lsh" };
    //static public string opBanner = "  MUL    mul    DIV    div    Sqr    Lsh ";

    //public string NotZeroNopsAsString()
    //{
    //    if (null == opCounts) return " * ";
    //    var sb = new StringBuilder(64);
    //    for (int k = 0; k < opCounts.Length; k++)
    //    {
    //        if (opCounts[k] != 0)
    //        {
    //            sb.Append(string.Format("{0:D}{1}, ", opCounts[k], opType[k]));
    //        }
    //    }
    //    return sb.ToString();
    //}
}
