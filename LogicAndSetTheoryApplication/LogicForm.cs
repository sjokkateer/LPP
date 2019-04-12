using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        }

        private void parseBtn_Click(object sender, EventArgs e)
        {
            string proposition = propositionTbx.Text;
            propositionParser = new Parser(proposition);
            propositionRoot = propositionParser.Parse();

            propositionResultLb.Text = propositionRoot.ToString();
        }
    }
}
