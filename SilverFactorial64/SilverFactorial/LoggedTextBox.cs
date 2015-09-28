// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace SilverFactorial
{
    using System;
    using System.IO;
    using System.Windows.Controls;

    // This delegate enables asynchronous calls for setting
    // the text property on a TextBox control.
    delegate void AppendTextCallback(string text);

    public class LoggedTextBox 
    {
        StreamWriter streamWriter;

        TextBox textBox;
        AppendTextCallback appendText;

        public LoggedTextBox(TextBox textBox)
        {
            string outputDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\BenchmarkApplication\";
            var df = new DirectoryInfo(outputDir);
            if (! df.Exists) 
            {
                df = Directory.CreateDirectory(df.FullName);
            }

            string fileName = string.Format(df.FullName + "BenchmarkApplication{0}.log", DateTime.Now.ToFileTime());
            var logFile = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.None);
            this.streamWriter = new StreamWriter(logFile);
            this.textBox = textBox;

            this.appendText = delegate(string text) 
            {
                textBox.AppendText(text);
                textBox.ScrollToEnd();
                textBox.Focus();
            };
        }

        private void AddToBox(string text)
        {
            textBox.Dispatcher.Invoke(appendText, text);
        }

        public StreamWriter GetStreamWriter() 
        { 
            return streamWriter; 
        }

        public void Write(string text)
        {
            if (LogToFile) streamWriter.Write(text);
            AddToBox(text);
        }

        public void WriteFlush(string text)
        {
            if (LogToFile) streamWriter.Write(text);
            AddToBox(text);
        }

        public void WriteLine(string text)
        {
            if (LogToFile) streamWriter.WriteLine(text);
            AddToBox(text);
        }

        public void WriteLine(string format, string message)
        {
            string text = string.Format(format, message) + Environment.NewLine;
            if (LogToFile) streamWriter.WriteLine(text);
            AddToBox(text);
        }

        public void WriteLine()
        {
            if (LogToFile) streamWriter.Write(streamWriter.NewLine);
            AddToBox(Environment.NewLine);
        }

        public void WriteRed(string text)
        {
            if (LogToFile) streamWriter.Write(text);
            AddToBox(text);
        }

        public void WriteRedline(string text)
        {
            if (LogToFile) streamWriter.WriteLine(text);
            AddToBox(text);
        }

        public void Flush()
        {
            streamWriter.Flush();
        }

        public void Dispose()
        {
            streamWriter.Flush();
            streamWriter.Close();
        }

        public bool LogToFile
        {
            get;
            set;  
        }
    }
}   // endOfLoggedTextBox
