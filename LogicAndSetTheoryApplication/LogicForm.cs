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

        public LogicForm()
        {
            InitializeComponent();
            // Create three variables compare and return the result of comparison.
            Symbol a = new Symbol('A');
            Symbol a2 = new Symbol('A');
            Symbol b = new Symbol('B');
            Symbol b2 = new Symbol('B');

            Console.WriteLine($"Comparing a and a2: result = {a == a2}");
            Console.WriteLine($"Comparing a and b: result = {a == b}");
            Console.WriteLine($"Comparing b and b2: result = {b == b2}");

            List<Symbol> unique = new List<Symbol>() { a, a2, b, b2 };
            var newUnique = unique.Distinct();

            Console.WriteLine();
            foreach(var item in newUnique)
            {
                Console.WriteLine(item);
            }
        }

        private void parseBtn_Click(object sender, EventArgs e)
        {
            string proposition = propositionTbx.Text;
            propositionParser = new Parser(proposition);
            propositionRoot = propositionParser.Parse();
            infixTbx.Text = propositionRoot.ToString();

            uniqueVariablesLb.Text = "Unique Variables:";
            List<Symbol> uniqueVariablesSet = propositionRoot.GetVariables();
            uniqueVariablesSet.Sort();
            foreach (Symbol s in uniqueVariablesSet)
            {
                uniqueVariablesLb.Text += $" {s}";
            }
        }

        private void CreateGraphOfExpression(Symbol propositionRoot, string dotFileName)
        {
            Grapher.CreateGraphOfFunction(propositionRoot, dotFileName);
            string windowsPath = Environment.GetEnvironmentVariable("windir");
            Process.Start($@"{windowsPath}\system32\mspaint.exe", $"{dotFileName}.png");
        }

        private void viewTreeBtn_Click(object sender, EventArgs e)
        {
            if (propositionRoot != null)
            {
                CreateGraphOfExpression(propositionRoot, "Proposition");
            }
        }
    }
}
