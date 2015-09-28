// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace SilverFactorial
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;

    using SilverFactorial.Benchmark;

    /// <summary>
    /// Interaction logic for BenchmarkWindow.xaml
    /// </summary>
    public partial class BenchmarkWindow : Window
    {
        private LoggedTextBox winsole; 
        private TestParameters test;
        private BenchmarkWorker benchmark;
        private BackgroundWorker backgroundWorker;
        private CheckBox[] algos;
        private const int NumOfCandidates = 24; 

        public BenchmarkWindow()
        {
            this.InitializeComponent();
            this.InitAlgoBoxes();

            this.StepBox.SelectedItem = "2.0";
            this.test = new TestParameters(NumOfCandidates);

            this.winsole = new LoggedTextBox(this.TextBox);
            this.benchmark = new BenchmarkWorker(this.winsole);

            this.LogToFileCheckBox.IsChecked = true;
            this.winsole.LogToFile = true;

            BenchmarkApplication.PrintAppAndSysProps(this.winsole);
            this.InitializeBackgoundWorker();
        }

        void InitAlgoBoxes()
        {
            this.algos = new CheckBox[NumOfCandidates]
                        {
                            this.Algo0, this.Algo1, this.Algo2, this.Algo3, this.Algo4, this.Algo5, this.Algo6, this.Algo7,
                            this.Algo8, this.Algo9, this.Algo10, this.Algo11, this.Algo12, this.Algo13, this.Algo14,
                            this.Algo15, this.Algo16, this.Algo17, this.Algo18, this.Algo19, this.Algo20, this.Algo21,
                            this.Algo22, this.Algo23
                        };

            int i = 0;
            foreach (var c in this.algos)
            {
                c.Content = Candidate.CandList[i++].Name;
            }

            this.TypeRecomm.IsSelected = true;
        }

        // Get the list of selected algorithms.
        // Get: stepfactor and benchmark-length
        // No validation is necessary for: benchStart and stepfactor.

        bool GetParams()
        {
            var start = this.StartBox.Text;
            if (!int.TryParse(start, out this.test.TestStart))
            {
                MessageBox.Show("start is not a valid integer.\n", "Invalid Argument Error");
                return false;
            }

            if (this.test.TestStart < 0)
            {
                MessageBox.Show("start must be a positive integer.\n", "Invalid Argument Error");
                return false;
            }

            if (this.test.TestStart > TestParameters.TestMax)
            {
                MessageBox.Show("start must <= 9000000 because n! is huge.\n", "Invalid Argument Error");
                return false;
            }

            // If the conversion fails, the return value is false and
            // the results parameter is set to zero.
            string step = this.StepBox.Text;
            if (step == "0.5") test.StepFactor = 5;
            else if (step == "1.0") test.StepFactor = 10;
            else if (step == "1.5") test.StepFactor = 15;
            else if (step == "2.0") test.StepFactor = 20;
            else if (step == "2.5") test.StepFactor = 25;
            else test.StepFactor = 30;

            var len = this.LengthBox.Text;
            if (!int.TryParse(len, out test.TestLength))
            {
                MessageBox.Show("Length is not a valid integer.\n", "Invalid Argument Error");
                return false;
            }

            this.winsole.LogToFile = (bool)this.LogToFileCheckBox.IsChecked;
            this.test.ShowFullValue = (bool)this.ShowFullValueCheckBox.IsChecked;
            this.test.Verbose = (bool)this.VerboseCheckBox.IsChecked;

            int c = 0;
            for (var i = 0; i < this.algos.Length; i++)
            {
                var check = (bool)this.algos[i].IsChecked;
                if (check)
                {
                    c++;
                }

                this.test.AlgoSelected[i] = check;
            }

            // The reference algorithm is always choosen.
            this.algos[Candidate.IndexOfReference].IsChecked = true;
            this.test.AlgoSelected[Candidate.IndexOfReference] = true;
            TestParameters.CardSelected = c;
            return true;
        }

        // Disable some input controls until the asynchronous benchmark is workDone.
        private void EnableControls(bool en)
        {
            this.LengthBox.IsEnabled = en;
            this.StartBox.IsEnabled = en;
            this.StepBox.IsEnabled = en;
            this.SanityCheck.IsEnabled = en;

            // verboseCheckBox.IsEnabled = en;
            // logToFileCheckBox.IsEnabled = en;
            // showFullValueCheckBox.IsEnabled = en;
            // benchTypeBox.IsEnabled = en;
        }

        // Set the AlgorithmCheckedListBox. 
        private void TypeSimpleSelected(object sender, RoutedEventArgs e)
        {
            var i = 0;
            foreach (var c in Candidate.CandList)
            {
                this.algos[i++].IsChecked = c.IsSimpleType;
            }
        }

        // Set the AlgorithmCheckedListBox.
        private void TypePrimeSelected(object sender, RoutedEventArgs e)
        {
            var i = 0;
            foreach (var c in Candidate.CandList)
            {
                this.algos[i++].IsChecked = c.IsPrimeType;
            }
        }

        // Set the AlgorithmCheckedListBox.
        private void TypeRecommSelected(object sender, RoutedEventArgs e)
        {
            var i = 0;
            foreach (var c in Candidate.CandList)
            {
                this.algos[i++].IsChecked = c.IsRecommended;
            }
        }

        private void TypeConcurrSelected(object sender, RoutedEventArgs e)
        {
            var i = 0;
            foreach (var c in Candidate.CandList)
            {
                this.algos[i++].IsChecked = c.IsConcurrType;
            }
        }

        private void TypeBenchSelected(object sender, RoutedEventArgs e)
        {
            var i = 0;
            foreach (var c in Candidate.CandList)
            {
                this.algos[i++].IsChecked = c.IsBenchable;
            }
        }

        // Clear the AlgorithmCheckedListBox.
        private void TypeClearSelected(object sender, RoutedEventArgs e)
        {
            foreach (var c in this.algos)
            {
                c.IsChecked = false;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (this.backgroundWorker.IsBusy)
            {
                this.backgroundWorker.CancelAsync();
            }
            this.AboutClick(null, null);
            this.backgroundWorker.Dispose();
            this.winsole.Dispose();
            base.OnClosing(e);
        }

        private void AboutClick(object sender, RoutedEventArgs e)
        {
            BenchmarkApplication.About(this.winsole);
            if (sender != null)
            {
                new BrowserForm(@"http://www.luschny.de/math/factorial/csharp/CsharpIndex.html").Show();
            }
        }

        // Call with some predefined value (say 1000).
        private void SanityCheckClick(object sender, RoutedEventArgs e)
        {
            this.DoTheBenchmark("sanity");
        }

        private void BenchmarkClick(object sender, RoutedEventArgs e)
        {
            this.DoTheBenchmark("full");
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.InfoLabel.Content = "Cancelation is in progress!";

            // Cancel the asynchronous operation.
            this.backgroundWorker.CancelAsync();

            // Disable the Cancel button.
            this.CancelBenchmark.IsEnabled = false;
        }

        //////////////////////////////////////////////////////////////////
        //  Handling of the BackgroundWorker
        //////////////////////////////////////////////////////////////////

        // Set up the BackgroundWorker object by attaching event handlers. 
        void InitializeBackgoundWorker()
        {
            this.backgroundWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            this.backgroundWorker.DoWork += this.BackgroundWorkerDoWork;
            this.backgroundWorker.RunWorkerCompleted += this.BackgroundWorkerRunWorkerCompleted;
            this.backgroundWorker.ProgressChanged += this.BackgroundWorkerProgressChanged;
        }

        // The central click: The BenchmarkWorker starts.
        // From BenchLength to StartWert by Startwert*Stepfactor
        // loop over all selected algorithms.
        void DoTheBenchmark(string type)
        {
            this.EnableControls(false);

            this.test.SanityTest = type == "sanity";

            // Get the values from the form. If they are valid ...
            if (this.test.SanityTest || this.GetParams())
            {
                this.TextBox.Clear();

                // Reset the text in the results label.
                this.InfoLabel.Content = string.Empty;

                // Disable the start button until 
                // the benchmark is workDone.
                this.BenchmarkGoButton.IsEnabled = false;

                // Enable the Cancel button while 
                // the benchmark runs.
                this.CancelBenchmark.IsEnabled = true;

                this.InfoLabel.Content = "Benchmark is running!";

                // start the asynchronous benchmark.
                this.backgroundWorker.RunWorkerAsync(this.test);
            }
            else
            {
                this.EnableControls(true);
            }
        }

        // This event handler updates the progress bar.
        void BackgroundWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.ProgressBar.Value = e.ProgressPercentage;
        }

        // This event handler is where the actual,
        // time-consuming benchmark is called.
        void BackgroundWorkerDoWork(object sender, DoWorkEventArgs eventArgs)
        {
            // MessageBox.Show("BackgroundWorker-DoWork was called.");
            // Get the BackgroundWorker that raised this event.
            using (var worker = sender as BackgroundWorker)
            {
                if (worker.CancellationPending)
                {
                    eventArgs.Cancel = true;
                }
                else
                {
                    // Assign the results of the computation to the Result 
                    // property of the DoWorkEventArgs object. This will 
                    // be available to the RunWorkerCompleted eventhandler.
                    eventArgs.Result = this.benchmark.DoTheBenchmark(worker, eventArgs, this.test);
                }
            }
        }

        // This event handler deals with the results of the benchmark.
        void BackgroundWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs eventArgs)
        {
            string msg;
            if (eventArgs.Cancelled)
            {
                // The user canceled the benchmark.
                msg = "Benchmark was canceled.";
                this.winsole.WriteRedline(msg);
            }
            else if (eventArgs.Error != null)
            {
                // There was an error during the benchmark.
                msg = "An error occurred.";
                MessageBox.Show(eventArgs.Error.Message, "Error in worker thread.");

                this.winsole.WriteLine();
                this.winsole.WriteRedline("Error in worker thread.");
                this.winsole.WriteRedline(eventArgs.Error.Message);
                this.winsole.WriteLine();
            }
            else
            {
                // The benchmark completed normally.
                msg = "Benchmark completed";
            }

            // MessageBox.Show(msg);
            this.InfoLabel.Content = msg;

            this.EnableControls(true);

            // Enable the start button.
            this.BenchmarkGoButton.IsEnabled = true;

            // Disable the Cancel button.
            this.CancelBenchmark.IsEnabled = false;

            this.test.SanityTest = false;

            this.ProgressBar.Value = 0;
        }
    }
}
