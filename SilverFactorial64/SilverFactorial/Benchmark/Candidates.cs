// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace SilverFactorial.Benchmark
{
    using System.Collections;
    using System.Linq;
    using Sharith.Factorial;
    using XInt = Sharith.Arithmetic.XInt;
    
    class Candidate {

    internal static Candidate[] CandList = {                             
                                                                     // index,bench,primeType,recommend,simple,concurr
        new Candidate(new ParallelPrimeSwing(), "ParallelSplitPrimeSwing",  0, true, true, true, false,true,1), 
        new Candidate(new ParallelPrimeSwing(), "ParallelPrimeSwing ",  1, true, true, true, false,true,1), 
        new Candidate(new ParallelPrimeSplit(), "ParallelPrimeSplit ",  2, true, true, false,false,true,1), 
        new Candidate(new PrimeSwing(),         "PrimeSwing         ",  3, true, true, true, false,false,2),
        new Candidate(new SplitPrimeSwing(),    "PrimeSwingAllInOne ",  4, false,true, false,false,false,2),
        new Candidate(new PrimeSchoenhage(),    "PrimeShoenhage     ",  5, true, true, true, false,false,2), 
        new Candidate(new PrimeSwingList(),     "PrimeSwingList     ",  6, true, true, false,false,false,1),
        new Candidate(new PrimeSwingCache(),    "PrimeSwingCache    ",  7, false,true, false,false,false,2),
        new Candidate(new PrimeVardi(),         "PrimeVardi         ",  8, false,true, false,false,false,2), 
        new Candidate(new PrimeLeenstra(),      "PrimeLeenstra      ",  9, false,true, false,false,false,2),
        new Candidate(new ParallelSwing(),      "ParallelSwing     ",  10, true, false,true, true, true,6),
        new Candidate(new ParallelSplit(),      "ParallelSplit     ",  11, true, false,false,true, true,7),
        new Candidate(new Swing(),              "Swing              ", 12, true, false,true, true, false,8), 
        new Candidate(new Balkan(),             "Balkan             ", 13, true, false,false,true, false,9),
        new Candidate(new SquaredDiffProd(),    "SquaredDiffProd    ", 14, true, false,false,true, false,9), 
        new Candidate(new Split(),              "Split              ", 15, true, false,false,true, false,11), 
        new Candidate(new SwingRationalDouble(),"SwingRationalDbl   ", 16, false,false,false,false,false,11),
        new Candidate(new CrandallPomerance(),  "CrandallPomerance  ", 17, false,false,false,false,true,13), 
        new Candidate(new ProductRecursive(),   "ProductRecursive   ", 18, false,false,false,false,false,12),
        new Candidate(new SwingRational(),      "SwingRational      ", 19, false,false,false,false,false,13),
        new Candidate(new SwingDouble(),        "SwingDouble        ", 20, false,false,false,false,false,130),
        new Candidate(new SwingSimple(),        "SwingSimple        ", 21, false,false,false,false,false,260),
        new Candidate(new BoitenSplit(),        "BoitenSplit        ", 22, false,false,false,false,false,460),
        new Candidate(new MyFactorial(),        "MyFactorial        ", 23, false,false,false,false,false,20) 
    };

    //// new Candidate(new Sharith.Math.Factorial.PrimeBorwein(),     "PrimeBorwein      ", false,true, false,false,false,4),
    //// new Candidate(new Sharith.Math.Factorial.Difference(),       "Difference        ", false,false,false,false,false,21), 
    //// new Candidate(new Sharith.Math.Factorial.AdditiveSwing(),    "AdditiveSwing     ", false,false,false,false,false,40), 
    //// new Candidate(new Sharith.Math.Factorial.AdditiveMoessner(), "AdditiveMoessner  ", false,false,false,false,false,80)  
    //// new Candidate(new Sharith.Math.Factorial.SquaredDifference(),"SquaredDiff       ", false,false,false,false,false,860),
    //// new Candidate(new Sharith.Math.Factorial.ProductNaive(),     "ProductNaive      ", false,false,false,false,false,1000),

    // -- the reference algorithm is set by this constant, which is an index in the array above.
     internal static int IndexOfReference = 1;
     internal static Candidate Reference = CandList[IndexOfReference];

     internal Hashtable Performance;
     IFactorialFunction Fun;
     internal XInt GetValue(int n) => this.Fun.Factorial(n);

    Candidate(IFactorialFunction f, string name, int index,
        bool b, bool p, bool t, bool s, bool c, int w)
    {
        this.Fun = f;
        this.Init(name, index, b, p, t, s, c, w);
    }

   void Init(string name, int index, bool b, bool p, bool t, bool s, bool c, int w)
   {
       this.Performance = new Hashtable();
       this.Name = name;
       this.Index = index;
       this.IsBenchable = b;
       this.IsPrimeType = p;
       this.IsRecommended = t;
       this.IsSimpleType = s;
       this.IsConcurrType = c;
       this.WorkLoad = w;
   }

   // Classification of the candidate
   public bool IsBenchable { get; private set; }
   public bool IsPrimeType { get; private set; }
   public bool IsRecommended { get; private set; }
   public bool IsSimpleType { get; private set; }
   public bool IsConcurrType { get; private set; }
   public string Name { get; private set; }
   public int Index { get; private set; }
   public int WorkLoad { get; private set; }

   static bool[] selected;
   public static void SetSelected(bool[] sel)
   {
       selected = sel;
   }

   internal static IEnumerable Selected => CandList.Where((t, i) => selected[i]).Cast<object>();
   internal static IEnumerable Sanity => CandList.Where(t => t.IsBenchable).Cast<object>();
   internal static IEnumerable Challengers => CandList.Where((t, i) => selected[i] && i != IndexOfReference).Cast<object>();

    }
}
