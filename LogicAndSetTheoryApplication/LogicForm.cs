using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogicAndSetTheoryApplication
{
    public partial class LogicForm : Form
    {
        private Symbol propositionRoot;
        private Parser propositionParser;

        private const string PATHTOPHOTO = "";

        public LogicForm()
        {
            InitializeComponent();
        }

        private void parseBtn_Click(object sender, EventArgs e)
        {
            string proposition = propositionTbx.Text;
            propositionParser = new Parser(proposition);
            propositionRoot = propositionParser.Parse();

            propositionResultLb.Text = propositionRoot.ToString();
            CreateGraphOfExpression(propositionRoot, "Proposition");
        }

        private void CreateGraphOfExpression(Symbol propositionRoot, string dotFileName)
        {
            Grapher.CreateGraphOfFunction(propositionRoot, dotFileName);
            string windowsPath = Environment.GetEnvironmentVariable("windir");
            Process.Start($@"{windowsPath}\system32\mspaint.exe", $"{dotFileName}.png");
        }
    }
}
