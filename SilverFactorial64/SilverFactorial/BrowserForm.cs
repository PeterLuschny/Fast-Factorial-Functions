using System;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;

namespace SilverFactorial
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    internal class BrowserForm : Form
    {
        private ToolStripButton backButton;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripButton forwardButton;
        private ToolStripButton goButton;
        private ToolStripButton homeButton;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem pageSetupToolStripMenuItem;
        private ToolStripButton printButton;
        private ToolStripMenuItem printPreviewToolStripMenuItem;
        private ToolStripMenuItem printToolStripMenuItem;
        private ToolStripMenuItem propertiesToolStripMenuItem;
        private ToolStripButton refreshButton;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripButton searchButton;
        private StatusStrip statusStrip1;
        private ToolStripButton stopButton;
        private ToolStrip toolStrip1;
        private ToolStrip toolStrip2;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripTextBox toolStripTextBox1;
        private WebBrowser webBrowser;

        public BrowserForm(string path)
        {
            this.InitializeForm();
            this.webBrowser.CanGoBackChanged += new EventHandler(this.webBrowser_CanGoBackChanged);
            this.webBrowser.CanGoForwardChanged += new EventHandler(this.webBrowser_CanGoForwardChanged);
            this.webBrowser.DocumentTitleChanged += new EventHandler(this.webBrowser_DocumentTitleChanged);
            this.webBrowser.StatusTextChanged += new EventHandler(this.webBrowser_StatusTextChanged);
            Uri url = new Uri(path);
            this.webBrowser.Navigate(url);
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            this.webBrowser.GoBack();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void forwardButton_Click(object sender, EventArgs e)
        {
            this.webBrowser.GoForward();
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            this.Navigate(this.toolStripTextBox1.Text);
        }

        private void homeButton_Click(object sender, EventArgs e)
        {
            // this.webBrowser.GoHome();
            this.Navigate(@"http://www.luschny.de/math/factorial/FastFactorialFunctions.htm");
        }

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
            this.saveAsToolStripMenuItem.Click += new EventHandler(this.saveAsToolStripMenuItem_Click);
            this.pageSetupToolStripMenuItem.Click += new EventHandler(this.pageSetupToolStripMenuItem_Click);
            this.printToolStripMenuItem.Click += new EventHandler(this.printToolStripMenuItem_Click);
            this.printPreviewToolStripMenuItem.Click += new EventHandler(this.printPreviewToolStripMenuItem_Click);
            this.propertiesToolStripMenuItem.Click += new EventHandler(this.propertiesToolStripMenuItem_Click);
            this.exitToolStripMenuItem.Click += new EventHandler(this.exitToolStripMenuItem_Click);
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
            this.goButton.Click += new EventHandler(this.goButton_Click);
            this.backButton.Click += new EventHandler(this.backButton_Click);
            this.forwardButton.Click += new EventHandler(this.forwardButton_Click);
            this.stopButton.Click += new EventHandler(this.stopButton_Click);
            this.refreshButton.Click += new EventHandler(this.refreshButton_Click);
            this.homeButton.Click += new EventHandler(this.homeButton_Click);
            this.searchButton.Click += new EventHandler(this.searchButton_Click);
            this.printButton.Click += new EventHandler(this.printButton_Click);
            this.toolStrip2.Items.Add(this.toolStripTextBox1);
            this.toolStripTextBox1.Size = new Size(250, 0x19);
            this.toolStripTextBox1.KeyDown += new KeyEventHandler(this.toolStripTextBox1_KeyDown);
            this.toolStripTextBox1.Click += new EventHandler(this.toolStripTextBox1_Click);
            this.statusStrip1.Items.Add(this.toolStripStatusLabel1);
            this.webBrowser.Dock = DockStyle.Fill;
            this.webBrowser.Navigated += new WebBrowserNavigatedEventHandler(this.webBrowser_Navigated);
            base.Controls.AddRange(new Control[] { this.webBrowser, this.toolStrip2, this.toolStrip1, this.menuStrip1, this.statusStrip1, this.menuStrip1 });
        }

        private void Navigate(string address)
        {
            if (!string.IsNullOrEmpty(address) && !address.Equals("about:blank"))
            {
                if (!address.StartsWith("http://") && !address.StartsWith("https://"))
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

        private void pageSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.webBrowser.ShowPageSetupDialog();
        }

        private void printButton_Click(object sender, EventArgs e)
        {
            this.webBrowser.Print();
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.webBrowser.ShowPrintPreviewDialog();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.webBrowser.ShowPrintDialog();
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.webBrowser.ShowPropertiesDialog();
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            if (!this.webBrowser.Url.Equals("about:blank"))
            {
                this.webBrowser.Refresh();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.webBrowser.ShowSaveAsDialog();
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            this.webBrowser.GoSearch();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            this.webBrowser.Stop();
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {
            this.toolStripTextBox1.SelectAll();
        }

        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Navigate(this.toolStripTextBox1.Text);
            }
        }

        private void webBrowser_CanGoBackChanged(object sender, EventArgs e)
        {
            this.backButton.Enabled = this.webBrowser.CanGoBack;
        }

        private void webBrowser_CanGoForwardChanged(object sender, EventArgs e)
        {
            this.forwardButton.Enabled = this.webBrowser.CanGoForward;
        }

        private void webBrowser_DocumentTitleChanged(object sender, EventArgs e)
        {
            this.Text = this.webBrowser.DocumentTitle;
        }

        private void webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            this.toolStripTextBox1.Text = this.webBrowser.Url.ToString();
        }

        private void webBrowser_StatusTextChanged(object sender, EventArgs e)
        {
            this.toolStripStatusLabel1.Text = this.webBrowser.StatusText;
        }
    }
}
