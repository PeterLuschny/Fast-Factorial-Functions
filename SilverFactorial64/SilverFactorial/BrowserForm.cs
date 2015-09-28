
namespace SilverFactorial
{
    using System;
    using System.Drawing;
    using System.Security.Permissions;
    using System.Windows.Forms;

    /// <summary>
    /// The browser form.
    /// </summary>
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    internal class BrowserForm : Form
    {
        /// <summary>
        /// The back button.
        /// </summary>
        private ToolStripButton backButton;

        /// <summary>
        /// The exit tool strip menu item.
        /// </summary>
        private ToolStripMenuItem exitToolStripMenuItem;

        /// <summary>
        /// The file tool strip menu item.
        /// </summary>
        private ToolStripMenuItem fileToolStripMenuItem;

        /// <summary>
        /// The forward button.
        /// </summary>
        private ToolStripButton forwardButton;

        /// <summary>
        /// The go button.
        /// </summary>
        private ToolStripButton goButton;

        /// <summary>
        /// The home button.
        /// </summary>
        private ToolStripButton homeButton;

        /// <summary>
        /// The menu strip 1.
        /// </summary>
        private MenuStrip menuStrip1;

        /// <summary>
        /// The page setup tool strip menu item.
        /// </summary>
        private ToolStripMenuItem pageSetupToolStripMenuItem;

        /// <summary>
        /// The print button.
        /// </summary>
        private ToolStripButton printButton;

        /// <summary>
        /// The print preview tool strip menu item.
        /// </summary>
        private ToolStripMenuItem printPreviewToolStripMenuItem;

        /// <summary>
        /// The print tool strip menu item.
        /// </summary>
        private ToolStripMenuItem printToolStripMenuItem;

        /// <summary>
        /// The properties tool strip menu item.
        /// </summary>
        private ToolStripMenuItem propertiesToolStripMenuItem;

        /// <summary>
        /// The refresh button.
        /// </summary>
        private ToolStripButton refreshButton;

        /// <summary>
        /// The save as tool strip menu item.
        /// </summary>
        private ToolStripMenuItem saveAsToolStripMenuItem;

        /// <summary>
        /// The search button.
        /// </summary>
        private ToolStripButton searchButton;

        /// <summary>
        /// The status strip 1.
        /// </summary>
        private StatusStrip statusStrip1;

        /// <summary>
        /// The stop button.
        /// </summary>
        private ToolStripButton stopButton;

        /// <summary>
        /// The tool strip 1.
        /// </summary>
        private ToolStrip toolStrip1;

        /// <summary>
        /// The tool strip 2.
        /// </summary>
        private ToolStrip toolStrip2;

        /// <summary>
        /// The tool strip separator 1.
        /// </summary>
        private ToolStripSeparator toolStripSeparator1;

        /// <summary>
        /// The tool strip separator 2.
        /// </summary>
        private ToolStripSeparator toolStripSeparator2;

        /// <summary>
        /// The tool strip status label 1.
        /// </summary>
        private ToolStripStatusLabel toolStripStatusLabel1;

        /// <summary>
        /// The tool strip text box 1.
        /// </summary>
        private ToolStripTextBox toolStripTextBox1;

        /// <summary>
        /// The web browser.
        /// </summary>
        private WebBrowser webBrowser;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserForm"/> class.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        public BrowserForm(string path)
        {
            this.InitializeForm();
            this.webBrowser.CanGoBackChanged += this.WebBrowserCanGoBackChanged;
            this.webBrowser.CanGoForwardChanged += this.WebBrowserCanGoForwardChanged;
            this.webBrowser.DocumentTitleChanged += this.WebBrowserDocumentTitleChanged;
            this.webBrowser.StatusTextChanged += this.WebBrowserStatusTextChanged;
            var url = new Uri(path);
            this.webBrowser.Navigate(url);
        }

        /// <summary>
        /// The back button click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BackButtonClick(object sender, EventArgs e)
        {
            this.webBrowser.GoBack();
        }

        /// <summary>
        /// The exit tool strip menu item click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        /// <summary>
        /// The forward button click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ForwardButtonClick(object sender, EventArgs e)
        {
            this.webBrowser.GoForward();
        }

        /// <summary>
        /// The go button click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void GoButtonClick(object sender, EventArgs e)
        {
            this.Navigate(this.toolStripTextBox1.Text);
        }

        /// <summary>
        /// The home button click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void HomeButtonClick(object sender, EventArgs e)
        {
            // this.webBrowser.GoHome();
            this.Navigate(@"http://www.luschny.de/math/factorial/FastFactorialFunctions.htm");
        }

        /// <summary>
        /// The initialize form.
        /// </summary>
        private void InitializeForm()
        {
            this.webBrowser = new WebBrowser();
            this.menuStrip1 = new MenuStrip();
            this.fileToolStripMenuItem = new ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.printToolStripMenuItem = new ToolStripMenuItem();
            this.printPreviewToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.exitToolStripMenuItem = new ToolStripMenuItem();
            this.pageSetupToolStripMenuItem = new ToolStripMenuItem();
            this.propertiesToolStripMenuItem = new ToolStripMenuItem();
            this.toolStrip1 = new ToolStrip();
            this.goButton = new ToolStripButton();
            this.backButton = new ToolStripButton();
            this.forwardButton = new ToolStripButton();
            this.stopButton = new ToolStripButton();
            this.refreshButton = new ToolStripButton();
            this.homeButton = new ToolStripButton();
            this.searchButton = new ToolStripButton();
            this.printButton = new ToolStripButton();
            this.toolStrip2 = new ToolStrip();
            this.toolStripTextBox1 = new ToolStripTextBox();
            this.statusStrip1 = new StatusStrip();
            this.toolStripStatusLabel1 = new ToolStripStatusLabel();
            this.menuStrip1.Items.Add(this.fileToolStripMenuItem);
            this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.saveAsToolStripMenuItem, this.toolStripSeparator1, this.pageSetupToolStripMenuItem, this.printToolStripMenuItem, this.printPreviewToolStripMenuItem, this.toolStripSeparator2, this.propertiesToolStripMenuItem, this.exitToolStripMenuItem });
            this.fileToolStripMenuItem.Text = "&File";
            this.saveAsToolStripMenuItem.Text = "Save &As...";
            this.pageSetupToolStripMenuItem.Text = "Page Set&up...";
            this.printToolStripMenuItem.Text = "&Print...";
            this.printPreviewToolStripMenuItem.Text = "Print Pre&view...";
            this.propertiesToolStripMenuItem.Text = "Properties";
            this.exitToolStripMenuItem.Text = "E&xit";
            this.printToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.P;
            this.saveAsToolStripMenuItem.Click += this.SaveAsToolStripMenuItemClick;
            this.pageSetupToolStripMenuItem.Click += this.PageSetupToolStripMenuItemClick;
            this.printToolStripMenuItem.Click += this.PrintToolStripMenuItemClick;
            this.printPreviewToolStripMenuItem.Click += this.PrintPreviewToolStripMenuItemClick;
            this.propertiesToolStripMenuItem.Click += this.PropertiesToolStripMenuItemClick;
            this.exitToolStripMenuItem.Click += this.ExitToolStripMenuItemClick;
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.goButton, this.backButton, this.forwardButton, this.stopButton, this.refreshButton, this.homeButton, this.searchButton, this.printButton });
            this.goButton.Text = "Go";
            this.backButton.Text = "Back";
            this.forwardButton.Text = "Forward";
            this.stopButton.Text = "Stop";
            this.refreshButton.Text = "Refresh";
            this.homeButton.Text = "Home";
            this.searchButton.Text = "Search";
            this.printButton.Text = "Print";
            this.backButton.Enabled = false;
            this.forwardButton.Enabled = false;
            this.goButton.Click += this.GoButtonClick;
            this.backButton.Click += this.BackButtonClick;
            this.forwardButton.Click += this.ForwardButtonClick;
            this.stopButton.Click += this.StopButtonClick;
            this.refreshButton.Click += this.RefreshButtonClick;
            this.homeButton.Click += this.HomeButtonClick;
            this.searchButton.Click += this.SearchButtonClick;
            this.printButton.Click += this.PrintButtonClick;
            this.toolStrip2.Items.Add(this.toolStripTextBox1);
            this.toolStripTextBox1.Size = new Size(250, 0x19);
            this.toolStripTextBox1.KeyDown += this.ToolStripTextBox1KeyDown;
            this.toolStripTextBox1.Click += this.ToolStripTextBox1Click;
            this.statusStrip1.Items.Add(this.toolStripStatusLabel1);
            this.webBrowser.Dock = DockStyle.Fill;
            this.webBrowser.Navigated += this.WebBrowserNavigated;
            this.Controls.AddRange(new Control[] { this.webBrowser, this.toolStrip2, this.toolStrip1, this.menuStrip1, this.statusStrip1, this.menuStrip1 });
        }

        /// <summary>
        /// The navigate.
        /// </summary>
        /// <param name="address">
        /// The address.
        /// </param>
        private void Navigate(string address)
        {
            if (!string.IsNullOrEmpty(address) && !address.Equals("about:blank"))
            {
                if (!address.StartsWith("http://", StringComparison.Ordinal) && !address.StartsWith("https://", StringComparison.Ordinal))
                {
                    address = "http://" + address;
                }

                try
                {
                    this.webBrowser.Navigate(new Uri(address));
                }
                catch (UriFormatException)
                {
                }
            }
        }

        /// <summary>
        /// The page setup tool strip menu item click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void PageSetupToolStripMenuItemClick(object sender, EventArgs e)
        {
            this.webBrowser.ShowPageSetupDialog();
        }

        /// <summary>
        /// The print button click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void PrintButtonClick(object sender, EventArgs e)
        {
            this.webBrowser.Print();
        }

        /// <summary>
        /// The print preview tool strip menu item click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void PrintPreviewToolStripMenuItemClick(object sender, EventArgs e)
        {
            this.webBrowser.ShowPrintPreviewDialog();
        }

        /// <summary>
        /// The print tool strip menu item click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void PrintToolStripMenuItemClick(object sender, EventArgs e)
        {
            this.webBrowser.ShowPrintDialog();
        }

        /// <summary>
        /// The properties tool strip menu item click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void PropertiesToolStripMenuItemClick(object sender, EventArgs e)
        {
            this.webBrowser.ShowPropertiesDialog();
        }

        /// <summary>
        /// The refresh button click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void RefreshButtonClick(object sender, EventArgs e)
        {
            if (!this.webBrowser.Url.Equals("about:blank"))
            {
                this.webBrowser.Refresh();
            }
        }

        /// <summary>
        /// The save as tool strip menu item click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void SaveAsToolStripMenuItemClick(object sender, EventArgs e)
        {
            this.webBrowser.ShowSaveAsDialog();
        }

        /// <summary>
        /// The search button click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void SearchButtonClick(object sender, EventArgs e)
        {
            this.webBrowser.GoSearch();
        }

        /// <summary>
        /// The stop button click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void StopButtonClick(object sender, EventArgs e)
        {
            this.webBrowser.Stop();
        }

        /// <summary>
        /// The tool strip text box 1 click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ToolStripTextBox1Click(object sender, EventArgs e)
        {
            this.toolStripTextBox1.SelectAll();
        }

        /// <summary>
        /// The tool strip text box 1 key down.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ToolStripTextBox1KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Navigate(this.toolStripTextBox1.Text);
            }
        }

        /// <summary>
        /// The web browser can go back changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WebBrowserCanGoBackChanged(object sender, EventArgs e)
        {
            this.backButton.Enabled = this.webBrowser.CanGoBack;
        }

        /// <summary>
        /// The web browser can go forward changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WebBrowserCanGoForwardChanged(object sender, EventArgs e)
        {
            this.forwardButton.Enabled = this.webBrowser.CanGoForward;
        }

        /// <summary>
        /// The web browser document title changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WebBrowserDocumentTitleChanged(object sender, EventArgs e)
        {
            this.Text = this.webBrowser.DocumentTitle;
        }

        /// <summary>
        /// The web browser navigated.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WebBrowserNavigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            this.toolStripTextBox1.Text = this.webBrowser.Url.ToString();
        }

        /// <summary>
        /// The web browser status text changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WebBrowserStatusTextChanged(object sender, EventArgs e)
        {
            this.toolStripStatusLabel1.Text = this.webBrowser.StatusText;
        }
    }
}
