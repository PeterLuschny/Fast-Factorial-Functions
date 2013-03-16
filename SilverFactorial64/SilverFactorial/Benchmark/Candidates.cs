// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace SilverFactorial
{
    using XInt = Sharith.Arithmetic.XInt;

    using System;
    using System.Collections;
    
    class Candidate {

    internal static Candidate[] candList = new Candidate[] 
    {                                                                                         // benchable,primeType,recommend,simple,concurr
        new Candidate(new Sharith.Math.Factorial.ParallelPrimeSwing(), "ParallelPrimeSwing ",  0, true, true, true, false,true,1), 
        new Candidate(new Sharith.Math.Factorial.ParallelPrimeSplit(), "ParallelPrimeSplit ",  1, true, true, false,false,true,1), 
        new Candidate(new Sharith.Math.Factorial.ParallelSplit(),      "ParallelSplit      ",  2, false, false,false,false,true,10),
        new Candidate(new Sharith.Math.Factorial.PrimeSwing(),         "PrimeSwing         ",  3, true, true, true,  false,false,2),
        new Candidate(new Sharith.Math.Factorial.PrimeSwingAllInOne(), "PrimeSwingAllInOne ",  4, true, true, false, false,false,2),
        new Candidate(new Sharith.Math.Factorial.PrimeSchoenhage(),    "PrimeShoenhage     ",  5, true, true, true, false,false,1), 
        new Candidate(new Sharith.Math.Factorial.PrimeSwingList(),     "PrimeSwingList     ",  6, true, true, false, false,false,1),
        new Candidate(new Sharith.Math.Factorial.PrimeSwingCache(),    "PrimeSwingCache    ",  7, false, true, false, false,false,1),
        new Candidate(new Sharith.Math.Factorial.PrimeVardi(),         "PrimeVardi         ",  8, false, true, false, false,false,2), 
        new Candidate(new Sharith.Math.Factorial.PrimeLeenstra(),      "PrimeLeenstra      ",  9, false, true, false, false,false,2),
        new Candidate(new Sharith.Math.Factorial.Swing(),              "Swing              ", 10, true,false, true,true, false,8), 
        new Candidate(new Sharith.Math.Factorial.Balkan(),             "Balkan             ", 11, true,false, false,true, false,9),
        new Candidate(new Sharith.Math.Factorial.SquaredDiffProd(),    "SquaredDiffProd    ", 12, true,false,false,true, false,9), 
        new Candidate(new Sharith.Math.Factorial.Split(),              "Split              ", 13, true,false,false,true, false,10), 
        new Candidate(new Sharith.Math.Factorial.SwingRationalDouble(),"SwingRationalDbl   ", 14, false,false,false,false,false,11),
        new Candidate(new Sharith.Math.Factorial.CrandallPomerance(),  "CrandallPomerance  ", 15, false, false,false,false,true,13), 
        new Candidate(new Sharith.Math.Factorial.ProductRecursive(),   "ProductRecursive   ", 16, false,false,false,false,false,12),
        new Candidate(new Sharith.Math.Factorial.SwingRational(),      "SwingRational      ", 17, false,false,false,false, false,13),
        new Candidate(new Sharith.Math.Factorial.SwingDouble(),        "SwingDouble        ", 18, false,false,false,false, false,130),
        new Candidate(new Sharith.Math.Factorial.SwingSimple(),        "SwingSimple        ", 19, false,false,false,false, false,260),
        new Candidate(new Sharith.Math.Factorial.BoitenSplit(),        "BoitenSplit        ", 20, false,false,false,false,false,460),
        new Candidate(new Sharith.Math.Factorial.ProductNaive(),       "ProductNaive       ", 21, false,false,false,false,false,1000),
        new Candidate(new Sharith.Math.Factorial.MyFactorial(),        "MyFactorial        ", 22, true,false,false,false,false,20) 
    };

    //  new Candidate(new Sharith.Math.Factorial.PrimeBorwein(),     "PrimeBorwein      ", false,true, false,false,false,4),
    //  new Candidate(new Sharith.Math.Factorial.Difference(),       "Difference        ", false,false,false,false,false,21), 
    //  new Candidate(new Sharith.Math.Factorial.AdditiveSwing(),    "AdditiveSwing     ", false,false,false,false,false,40), 
    //  new Candidate(new Sharith.Math.Factorial.AdditiveMoessner(), "AdditiveMoessner  ", false,false,false,false,false,80)  
    //  new Candidate(new Sharith.Math.Factorial.SquaredDifference(),"SquaredDiff       ", false,false,false,false,false,860),
    //  new Candidate(new Sharith.Math.Factorial.ParallelSwing(),    "ParallelSwing     ", false, false,false, false,true,10),
    
     internal Hashtable results;
     // -- the reference algorithm is set by this constant, which is an index in the array above.
     internal static int INDEX_OF_REFERENCE = 3;
     internal static Candidate reference = candList[INDEX_OF_REFERENCE];
     private Sharith.Math.Factorial.IFactorialFunction fun;

     internal XInt GetValue(int n)
     {
        return fun.Factorial(n);
     }

    private Candidate(Sharith.Math.Factorial.IFactorialFunction f, string name, int index,
        bool b, bool p, bool t, bool s, bool c, int w)
    {
        fun = f;
        Init(name, index, b, p, t, s, c, w);
    }

   private void Init(string name, int index, bool b, bool p, bool t, bool s, bool c, int w)
   {
       results = new Hashtable();
       Name = name;
       Index = index;
       IsBenchable = b;
       IsPrimeType = p;
       IsRecommended = t;
       IsSimpleType = s;
       IsConcurrType = c;
       WorkLoad = w;
   }

   // Classification of the candidate
   public bool IsBenchable { get; set; }
   public bool IsPrimeType { get; set; }
   public bool IsRecommended { get; set; }
   public bool IsSimpleType { get; set; }
   public bool IsConcurrType { get; set; }
   public string Name { get; set; }
   public int Index { get; set; }
   public int WorkLoad { get; set; }

   public static int[] rankList;
   private static bool[] selected;
   public static void SetSelected(bool[] sel)
   {
       selected = sel;
   }

   internal static IEnumerable Selected
   {
       get
       {
           for (int i = 0; i < candList.Length; i++)
           {
               if (!selected[i]) continue;
               yield return candList[i];
           }
       }
   }

   internal static IEnumerable Sanity
   {
       get
       {
           for (int i = 0; i < candList.Length; i++)
           {
               if (!candList[i].IsBenchable) continue;
               yield return candList[i];
           }
       }
   }

   internal static IEnumerable Challengers
   {
       get
       {
           for (int i = 0; i < candList.Length; i++)
           {
               if (!selected[i]) continue;
               if (i == INDEX_OF_REFERENCE) continue;
               yield return candList[i];
           }
       }
    }
  }
}
