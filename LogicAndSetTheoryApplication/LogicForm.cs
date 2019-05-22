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
        private Proposition propositionRoot;
        private Parser propositionParser;
        private TruthTable truthTable;
        private TruthTable simplifiedTruthTable;
        private HashCodeCalculator hashCalc;
        private List<string> hashList;

        public LogicForm()
        {
            InitializeComponent();
            hashList = new List<string>();
        }

        private void parseBtn_Click(object sender, EventArgs e)
        {
            ResetHashListBoxes();

            string proposition = propositionTbx.Text;
            propositionParser = new Parser(proposition);
            propositionRoot = propositionParser.Parse();
            infixTbx.Text = propositionRoot.ToString();

            uniqueVariablesLb.Text = "Unique Variables:";
            List<Proposition> uniqueVariablesSet = propositionRoot.GetVariables();
            uniqueVariablesSet.Sort();
            uniqueVariablesTbx.Text = "";
            foreach (Proposition s in uniqueVariablesSet)
            {
                uniqueVariablesTbx.Text += $" {s}";
            }
            truthTable = new TruthTable(propositionRoot);
            AddTruthTable(truthTableLbx, truthTable);
            hashCalc = new HashCodeCalculator(truthTable.GetConvertedResultColumn(), 16);
            hashList.Add(hashCalc.HashCode);
            hashCodeTbx.Text = hashCalc.HashCode;
            hashesListBox.Items.Add("Proposition:");
            hashCodesListbox.Items.Add($"{hashCalc.HashCode}");
            hashCodeTbx.BackColor = Color.LightGreen;

            // < Disjunctive normal form >
            Proposition disjunctiveProposition = truthTable.CreateDisjunctiveNormalForm();
            if (disjunctiveProposition != null)
            {
                disjunctiveFormTbx.Text = disjunctiveProposition.ToString();
                TruthTable disjunctiveTruthTable = new TruthTable(disjunctiveProposition);
                hashCalc = new HashCodeCalculator(disjunctiveTruthTable.GetConvertedResultColumn(), 16);
                hashList.Add(hashCalc.HashCode);
                hashesListBox.Items.Add($"Disjunctive normal:");
                hashCodesListbox.Items.Add($"{hashCalc.HashCode}");
            }
            else
            {
                // Reset the content of their textboxes.
                disjunctiveFormTbx.Text = string.Empty;
            }

            // Simplified truth table.
            simplifiedTruthTable = truthTable.Simplify();
            AddTruthTable(simplifiedTruthTableLbx, simplifiedTruthTable);
            // < Disjunctive normal form >
            Proposition simplifiedDisjunctiveProposition = simplifiedTruthTable.CreateDisjunctiveNormalForm();
            if (simplifiedDisjunctiveProposition != null)
            {
                simplifiedDisjunctiveFormTbx.Text = simplifiedDisjunctiveProposition.ToString();
                // Create truth table of the simplified disjunctive
                // Create a new hash calculator with the result column, base 16.
                // Add hash and bcde to the list boxes.
                List<Proposition> simplifiedDisjunctiveUniqueVariables = simplifiedDisjunctiveProposition.GetVariables();
                if (simplifiedDisjunctiveUniqueVariables.Count < uniqueVariablesSet.Count)
                {
                    // Create an extra placeholder variable on the proposition to fill up the left out variable spot.
                    // Important that the variabel IS unique, such that it adds to the number of combinations of truth values.
                    foreach (char alphabetCharacter in "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
                    {
                        foreach (Proposition uniqueVariable in uniqueVariablesSet)
                        {
                            if (alphabetCharacter == (char)uniqueVariable.Data)
                            {
                                break;
                            }
                        }
                        // Exhausted without breaking out of the loop thus we are looking at a unique variable.
                        Proposition newVariable = new Proposition(alphabetCharacter);
                        // Since this # of variables resulted in no changes in the outcome of the result
                        // it should not matter too much on what binary operator is applied.
                        Console.WriteLine();
                        Console.WriteLine($"Adding variable: {newVariable}");
                        Console.WriteLine();
                        Conjunction conjunction = new Conjunction();
                        Disjunction tautology = new Disjunction();
                        tautology.LeftSuccessor = newVariable; // A
                        Negation negatedVariable = new Negation();
                        negatedVariable.LeftSuccessor = newVariable;
                        tautology.RightSuccessor = negatedVariable; // A | ~(A) == 1
                        conjunction.LeftSuccessor = tautology;
                        // So if we add a tautology with the original expression, we take the old variable into consideration
                        // in the truth table but this variable will not change the result values of the truth table but will
                        // ensure that our hash codes will line up with the same number of bits.
                        conjunction.RightSuccessor = simplifiedDisjunctiveProposition;
                        simplifiedDisjunctiveProposition = conjunction;
                        break;
                    }
                }
                Console.WriteLine($"Simplified Normal Proposition: {simplifiedDisjunctiveProposition}");
                Console.WriteLine();
                TruthTable simplifiedDisjunctiveTruthTable = new TruthTable(simplifiedDisjunctiveProposition);
                hashCalc = new HashCodeCalculator(simplifiedDisjunctiveTruthTable.GetConvertedResultColumn(), 16);
                hashList.Add(hashCalc.HashCode);
                Console.WriteLine($"Simplified Normal Proposition TT:");
                Console.WriteLine(simplifiedDisjunctiveTruthTable);
                Console.WriteLine();
                hashesListBox.Items.Add($"Simplified Disjunctive normal:");
                hashCodesListbox.Items.Add($"{hashCalc.HashCode}");
            }
            else
            {
                // Reset the content of their textboxes.
                simplifiedDisjunctiveFormTbx.Text = string.Empty;
            }

            Proposition nandified = propositionRoot.Nandify();
            if (nandified != null)
            {
                nandifiedTbx.Text = nandified.ToString();
                TruthTable tt = new TruthTable(nandified);
                hashCalc = new HashCodeCalculator(tt.GetConvertedResultColumn(), 16);
                hashList.Add(hashCalc.HashCode);
                hashesListBox.Items.Add($"NAND:");
                hashCodesListbox.Items.Add($"{hashCalc.HashCode}");

                TruthTable simplifiedNandTable = tt.Simplify();
                Console.WriteLine(simplifiedNandTable);
                hashCalc = new HashCodeCalculator(simplifiedNandTable.GetConvertedResultColumn(), 16);
                hashList.Add(hashCalc.HashCode);
                hashesListBox.Items.Add($"Simplified NAND:");
                hashCodesListbox.Items.Add($"{hashCalc.HashCode}");

                Proposition nandifiedDisjunctiveNormal = simplifiedNandTable.CreateDisjunctiveNormalForm();
            }
            else
            {
                // Reset the content of their textboxes.
                nandifiedTbx.Text = string.Empty;
            }

        }

        private void AddTruthTable(ListBox truthTableLbx, TruthTable truthTable)
        {
            truthTableLbx.Items.Clear();
            if (truthTable != null)
            {
                truthTableLbx.Items.Add(truthTable.TableHeader());
                foreach (TruthTableRow row in truthTable.Rows)
                {
                    truthTableLbx.Items.Add(row);
                }
            }
        }

        private void ResetHashListBoxes()
        {
            hashesListBox.Items.Clear();
            hashesListBox.Items.Add("Hashes:");
            hashesListBox.Items.Add("");

            hashCodesListbox.Items.Clear();
            hashCodesListbox.Items.Add("Codes:");
            hashCodesListbox.Items.Add("");
        }

        private void CreateGraphOfExpression(Proposition propositionRoot, string dotFileName)
        {
            Grapher.CreateGraphOfFunction(propositionRoot, dotFileName);
            Process.Start($"{dotFileName}.png");
        }

        private void viewTreeBtn_Click(object sender, EventArgs e)
        {
            if (propositionRoot != null)
            {
                CreateGraphOfExpression(propositionRoot.Copy(), "Proposition");
            }
        }
    }
}
