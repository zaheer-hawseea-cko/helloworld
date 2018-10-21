using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTPS
{
    public class CustomTraceListener : TraceListener
    {
        private TextBox txt;

        public CustomTraceListener(TextBox txt)
        {
            this.txt = txt;
        }

        public override void Write(string message)
        {
            txt.Text += message;
        }

        public override void WriteLine(string message)
        {
            txt.Text += message + Environment.NewLine;
        }
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
