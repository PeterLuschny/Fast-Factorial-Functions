// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace SilverFactorial
{
    /// <summary>
    /// Interaction logic for BenchmarkWindow.xaml
    /// </summary>
    public partial class BenchmarkWindow : Window
    {
        LoggedTextBox winsole; 
        TestParameters test;
        BenchmarkWorker benchmark;
        BackgroundWorker backgroundWorker;
        CheckBox[] algos;
        const int NUM_OF_CANDIDATES = 23; 

        public BenchmarkWindow()
        {
            InitializeComponent();
            Candidate.SetMPArithmetic(false);
            InitAlgoBoxes();

            stepBox.SelectedItem = "2.0";
            test = new TestParameters(NUM_OF_CANDIDATES); 

            winsole = new LoggedTextBox(this.textBox, "FactorialBench");
            benchmark = new BenchmarkWorker(winsole);

            logToFileCheckBox.IsChecked = true;
            winsole.LogToFile = true;

            BenchmarkApplication.PrintAppAndSysProps(winsole);
            InitializeBackgoundWorker();
        }

        private void InitAlgoBoxes()
        {
            algos = new CheckBox[NUM_OF_CANDIDATES] 
            {algo0,algo1,algo2,algo3,algo4,algo5,algo6,algo7,
            algo8,algo9,algo10,algo11,algo12,algo13,algo14,
            algo15,algo16,algo17,algo18,algo19,algo20,algo21,algo22};

            int i = 0;
            foreach (CheckBox c in algos)
            {
                c.Content = Candidate.candList[i++].Name;
            }

            TypeRecomm.IsSelected = true;
        }

        // Get the list of selected algorithms.
        // Get: stepfactor and benchmark-length
        // No validation is necessary for: benchStart and stepfactor.

        private bool GetParams()
        {
            string start = startBox.Text; 
            if (!int.TryParse(start, out test.benchStart))
            {
                MessageBox.Show("start is not a valid integer.\n", "Invalid Argument Error");
                return false;
            }

            if (test.benchStart < 0)
            {
                MessageBox.Show("start must be a positive integer.\n", "Invalid Argument Error");
                return false;
            }

            if (test.benchStart > TestParameters.BENCH_MAX)
            {
                MessageBox.Show("start must <= 9000000 because n! is huge.\n", "Invalid Argument Error");
                return false;
            }

            // If the conversion fails, the return value is false and
            // the results parameter is set to zero.
            string step = stepBox.Text;
                 if (step == "0.5") test.stepFactor = 5;
            else if (step == "1.0") test.stepFactor = 10;
            else if (step == "1.5") test.stepFactor = 15;
            else if (step == "2.0") test.stepFactor = 20;
            else if (step == "2.5") test.stepFactor = 25;
            else test.stepFactor = 30;

            string len = lengthBox.Text;
            if (!int.TryParse(len, out test.benchLength))
            {
                MessageBox.Show("Length is not a valid integer.\n", "Invalid Argument Error");
                return false;
            }

            winsole.LogToFile = (bool)logToFileCheckBox.IsChecked;
            test.showFullValue = (bool)showFullValueCheckBox.IsChecked;
            test.verbose = (bool)verboseCheckBox.IsChecked;

            int c = 0;
            for (int i = 0; i < algos.Length; i++)
            {
                bool check = (bool)algos[i].IsChecked;
                if (check) c++;
                test.selectedAlgo[i] = check;
            }

            // The reference algorithm is always choosen.
            algos[Candidate.INDEX_OF_REFERENCE].IsChecked = true;
            test.selectedAlgo[Candidate.INDEX_OF_REFERENCE] = true;
            test.cardSelected = c;
            return true;
        }

        // Disable some input controls until the asynchronous benchmark is workDone.
        private void EnableControls(bool en)
        {
            lengthBox.IsEnabled = en;
            startBox.IsEnabled = en;
            stepBox.IsEnabled = en;
            sanityCheck.IsEnabled = en;

            // verboseCheckBox.IsEnabled = en;
            // logToFileCheckBox.IsEnabled = en;
            // showFullValueCheckBox.IsEnabled = en;
            // benchTypeBox.IsEnabled = en;
        }

        private void LogToFileCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            winsole.LogToFile = (bool)logToFileCheckBox.IsChecked;
        }

        // Set the AlgorithmCheckedListBox. 
        private void TypeSimple_Selected(object sender, RoutedEventArgs e)
        {
            int i = 0;
            foreach (Candidate c in Candidate.candList)
            {
                algos[i++].IsChecked = c.IsSimpleType;
            }
        }

        // Set the AlgorithmCheckedListBox.
        private void TypePrime_Selected(object sender, RoutedEventArgs e)
        {
            int i = 0;
            foreach (Candidate c in Candidate.candList)
            {
                algos[i++].IsChecked = c.IsPrimeType;
            }
        }

        // Set the AlgorithmCheckedListBox.
        private void TypeRecomm_Selected(object sender, RoutedEventArgs e)
        {
            int i = 0;
            foreach (Candidate c in Candidate.candList)
            {
                algos[i++].IsChecked = c.IsRecommended;
            }
        }

        // Clear the AlgorithmCheckedListBox.
        private void TypeClear_Selected(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox c in algos)
            {
                c.IsChecked = false; 
            }
        }

        private void TypeConcurr_Selected(object sender, RoutedEventArgs e)
        {
            int i = 0;
            foreach (Candidate c in Candidate.candList)
            {
                algos[i++].IsChecked = c.IsConcurrType;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
            }
            About_Click(null, null);
            backgroundWorker.Dispose();
            winsole.Dispose();
            base.OnClosing(e);
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            winsole.WriteLine(about);
        }

        // Call with some predefined value (say 1000).
        private void SanityCheck_Click(object sender, RoutedEventArgs e)
        {
            DoTheBenchmark("sanity");
        }

        private void Benchmark_Click(object sender, RoutedEventArgs e)
        {
            DoTheBenchmark("full");
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            infoLabel.Content = "Cancelation is in progress!";

            // Cancel the asynchronous operation.
            backgroundWorker.CancelAsync();

            // Disable the Cancel button.
            cancelBenchmark.IsEnabled = false;
        }

        //////////////////////////////////////////////////////////////////
        //  Handling of the BackgroundWorker
        //////////////////////////////////////////////////////////////////

        // Set up the BackgroundWorker object by attaching event handlers. 
        private void InitializeBackgoundWorker()
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);
        }

        // The central click: The BenchmarkWorker starts.
        // From BenchLength to StartWert by Startwert*Stepfactor
        // loop over all selected algorithms.
        private void DoTheBenchmark(string type)
        {
            EnableControls(false);

            test.sanityTest = type == "sanity";

            // Get the values from the form. If they are valid ...
            if (test.sanityTest || GetParams())
            {
                textBox.Clear();

                // Reset the text in the results label.
                infoLabel.Content = string.Empty;

                // Disable the start button until 
                // the benchmark is workDone.
                benchmarkGoButton.IsEnabled = false;

                // Enable the Cancel button while 
                // the benchmark runs.
                cancelBenchmark.IsEnabled = true;

                infoLabel.Content = "Benchmark is running!";

                // start the asynchronous benchmark.
                backgroundWorker.RunWorkerAsync(test);
            }
            else
            {
                EnableControls(true);
            }
        }

        // This event handler updates the progress bar.
        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        // This event handler is where the actual,
        // time-consuming benchmark is called.
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs eventArgs)
        {
            // MessageBox.Show("BackgroundWorker-DoWork was called.");
            // Get the BackgroundWorker that raised this event.
            BackgroundWorker bgWorker = sender as BackgroundWorker;

            if (bgWorker.CancellationPending)
            {
                eventArgs.Cancel = true;
            }
            else
            {
                // Assign the results of the computation to the Result 
                // property of the DoWorkEventArgs object. This will 
                // be available to the RunWorkerCompleted eventhandler.
                eventArgs.Result = benchmark.DoTheBenchmark(bgWorker, eventArgs, test);
            }
        }

        // This event handler deals with the results of the benchmark.
        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs eventArgs)
        {
            string msg;
            if (eventArgs.Cancelled)
            {
                // The user canceled the benchmark.
                msg = "Benchmark was canceled.";
                winsole.WriteRedLine(msg);
            }
            else if (eventArgs.Error != null)
            {
                // There was an error during the benchmark.
                msg = "An error occurred.";
                MessageBox.Show(eventArgs.Error.Message, "Error in worker thread.");

                winsole.WriteLine();
                winsole.WriteRedLine("Error in worker thread.");
                winsole.WriteRedLine(eventArgs.Error.Message);
                winsole.WriteLine();
            }
            else
            {
                // The benchmark completed normally.
                msg = "Benchmark completed";
            }

            // MessageBox.Show(msg);
            infoLabel.Content = msg;

            EnableControls(true);

            // Enable the start button.
            benchmarkGoButton.IsEnabled = true;

            // Disable the Cancel button.
            cancelBenchmark.IsEnabled = false;

            test.sanityTest = false;

            progressBar.Value = 0;
        }

        static string about =
           "\n----------------------------------------"+
           "\n    This is Sharith.SilverFactorial     "+
           "\n      Version 2.0-Build-2011-06-16      "+
           "\n----------------------------------------"+
           "\n    Copyright (C) 2011 Peter Luschny    "+
           "\n               License:                 "+
           "\n  Creative Commons Attr.-ShareAlike 3.0 \n" +
           "\n  http://www.luschny.de/math/factorial/  "+
           "\n       FastFactorialFunctions.htm       \n"+
           "\nPlease send comments and bug reports to "+
           "\n         peter(at)luschny.de            \n"+
           "\n      Contribute your solution!         "+
           "\n----------------------------------------\n";
    }
}
