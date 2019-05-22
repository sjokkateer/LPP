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
        private List<string> hashList;
        private List<Conjunction> missingVariableList;

        public LogicForm()
        {
            InitializeComponent();
            hashList = new List<string>();
            missingVariableList = new List<Conjunction>();
        }

        private void AddHashCodeInfo(Proposition proposition, string typeOfProposition, int hashBase, TruthTable tt = null)
        {
            if (tt == null)
            {
                tt = new TruthTable(proposition);
            }
            HashCodeCalculator hashCodeCalculator = new HashCodeCalculator(tt.GetConvertedResultColumn(), hashBase);
            hashList.Add(hashCodeCalculator.HashCode);
            hashesListBox.Items.Add($"{typeOfProposition}: ");
            hashCodesListbox.Items.Add($"{hashCodeCalculator.HashCode}");
        }

        private void parseBtn_Click(object sender, EventArgs e)
        {
            ResetHashRelatedItems();
            // Reset the missing variable list.
            missingVariableList = new List<Conjunction>();

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
            // This is specific to the original proposition.
            AddTruthTable(truthTableLbx, truthTable);
            AddHashCodeInfo(propositionRoot, "Proposition" , 16, truthTable); // 1. ORIGINAL EVALUATE.
            // Need to keep this as exclusive for the original proposition.
            hashCodeTbx.Text = hashList[0]; // <---- THIS IS NOT GOOD CODING!
            hashCodeTbx.BackColor = Color.LightGreen;

            // < Disjunctive normal form >
            // New proposition (DNF) and we repeat almost identical code.
            Proposition disjunctiveProposition = truthTable.CreateDisjunctiveNormalForm();
            if (disjunctiveProposition == null)
            {
                // This should mean we only have 1 result value in the result column, a 0 for DNF
                // Create a contradiction
                Proposition tautologyOrContradiction = CreateContradiction(uniqueVariablesSet[0]);
                tautologyOrContradiction.UniqueVariableSet = uniqueVariablesSet;
                disjunctiveProposition = tautologyOrContradiction;
            }
            // Parameters (TextBox tbx, Proposition proposition, hashCalculator)
            disjunctiveFormTbx.Text = disjunctiveProposition.ToString();
            AddHashCodeInfo(disjunctiveProposition, "Disjunctive normal", 16); // 4. ORIGINAL + EVALUATE + NORMALIZE + EVALUATE.

            // Simplified truth table.
            simplifiedTruthTable = truthTable.Simplify();
            AddTruthTable(simplifiedTruthTableLbx, simplifiedTruthTable);
            // < Disjunctive normal form >
            Proposition simplifiedDisjunctiveProposition = simplifiedTruthTable.CreateDisjunctiveNormalForm();
            // If the returned proposition is not null, we check if we are not missing any variables that got 
            // left out by us simplifying a proposition.
            if (simplifiedDisjunctiveProposition != null)
            {
                simplifiedDisjunctiveFormTbx.Text = simplifiedDisjunctiveProposition.ToString();
                // Create truth table of the simplified disjunctive
                // Create a new hash calculator with the result column, base 16.
                // Add hash and bcde to the list boxes.
                List<Proposition> simplifiedDisjunctiveUniqueVariables = simplifiedDisjunctiveProposition.GetVariables();
                int addedVariables = 0;
                while (simplifiedDisjunctiveUniqueVariables.Count + addedVariables < uniqueVariablesSet.Count)
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
                        Conjunction conjunction = new Conjunction();
                        Disjunction tautology = new Disjunction();
                        tautology.LeftSuccessor = newVariable; // A
                        Negation negatedVariable = new Negation();
                        negatedVariable.LeftSuccessor = newVariable;
                        tautology.RightSuccessor = negatedVariable; // A | ~(A) == 1
                        conjunction.LeftSuccessor = tautology;
                        missingVariableList.Add(conjunction);
                        // So if we add a tautology with the original expression, we take the old variable into consideration
                        // in the truth table but this variable will not change the result values of the truth table but will
                        // ensure that our hash codes will line up with the same number of bits.
                        addedVariables++;
                        break;
                    }
                }
                // Now we have all missing variables in the list of missing variables.
                // Thus we can add to the right successor of each index, the previous or the root expression.
                int lastIndex = missingVariableList.Count - 1;
                Proposition propositionToAppend = null;
                for (int i = lastIndex; i >= 0; i--)
                {
                    if (i == lastIndex)
                    {
                        propositionToAppend = simplifiedDisjunctiveProposition;
                    }
                    missingVariableList[i].RightSuccessor = propositionToAppend;
                    propositionToAppend = missingVariableList[i];
                }

                if (missingVariableList.Count > 0)
                {
                    AddHashCodeInfo(missingVariableList[0], "Simplified Disjunctive Normal", 16);
                }
                else
                {
                    // Keeping the original the original.
                    AddHashCodeInfo(simplifiedDisjunctiveProposition, "Simplified Disjunctive Normal", 16); // 5. ORIGINAL + SIMPLIFY + NORMALIZE + EVALUATE
                }

                // NANDIFY 5 and eval to get 6
                Proposition nandifiedFive = simplifiedDisjunctiveProposition.Nandify();
                if (missingVariableList.Count > 0)
                {
                    missingVariableList[missingVariableList.Count - 1].RightSuccessor = nandifiedFive;
                    AddHashCodeInfo(missingVariableList[0], "Simplified Disjunctive Normal NAND", 16);
                }
                else
                {
                    // Keeping the original the original.
                    AddHashCodeInfo(nandifiedFive, "Simplified Disjunctive Normal NAND", 16); // 6. ORIGINAL + SIMPLIFY + NORMALIZE + EVALUATE
                }
            }
            else
            {
                // Otherwise we are dealing with a tautology or contradiction.
                int tautOrContra = simplifiedTruthTable.GetConvertedResultColumn()[0];
                Proposition tautologyOrContradiction = null;
                if (tautOrContra == 1)
                {
                    // Create a tautology but it should hold the same variable set as the original proposition.
                    tautologyOrContradiction = CreateTautology(uniqueVariablesSet[0]);
                }
                else
                {
                    // Create a contradiction
                    tautologyOrContradiction = CreateContradiction(uniqueVariablesSet[0]);
                }
                tautologyOrContradiction.UniqueVariableSet = uniqueVariablesSet;
                simplifiedDisjunctiveProposition = tautologyOrContradiction;

            }

            Proposition nandified = propositionRoot.Nandify();
            if (nandified != null)
            {
                nandifiedTbx.Text = nandified.ToString();
                TruthTable tt = new TruthTable(nandified);
                AddHashCodeInfo(nandified, "NAND", 16, tt); // ORIGINAL + NAND + EVALUATE 

                TruthTable simplifiedNandTt = tt.Simplify();
                Proposition simplifiedNandDnf = simplifiedNandTt.CreateDisjunctiveNormalForm();
                // If it is null we deal with a tautology or contradiction!
                if (simplifiedNandDnf == null)
                {
                    // This should mean we only have 1 result value in the result column, either a 1 or 0
                    int tautOrContra = simplifiedNandTt.GetConvertedResultColumn()[0];
                    Proposition tautologyOrContradiction = null;
                    if (tautOrContra == 1)
                    {
                        // Create a tautology but it should hold the same variable set as the original proposition.
                        tautologyOrContradiction = CreateTautology(uniqueVariablesSet[0]);
                    }
                    else
                    {
                        // Create a contradiction
                        tautologyOrContradiction = CreateContradiction(uniqueVariablesSet[0]);
                    }
                    tautologyOrContradiction.UniqueVariableSet = uniqueVariablesSet;
                    AddHashCodeInfo(tautologyOrContradiction, "Simplified NAND Normal", 16);
                }
                else
                {

                    if (missingVariableList.Count > 0)
                    {
                        missingVariableList[missingVariableList.Count - 1].RightSuccessor = simplifiedNandDnf;
                        AddHashCodeInfo(missingVariableList[0], "Simplified NAND Normal", 16);
                    }
                    else
                    {
                        // Keeping the original the original.
                        AddHashCodeInfo(simplifiedNandDnf, "Simplified NAND Normal", 16); // 3. ORIGINAL + NAND + EVALUATE + SIMPLIFY + NORMALIZE
                    }
                }
            }
            else
            {
                // Reset the content of their textboxes.
                nandifiedTbx.Text = string.Empty;
            }

            Console.WriteLine(hashList.Count);
            // Test if all hashcodes are equal.
            if (hashCodesMatched(hashList, hashList.Count - 1))
            {
                hashCodeValidationTbx.Text = "Succes! All hashes match.";
                hashCodeValidationTbx.BackColor = Color.LightGreen;
            }
            else
            {
                hashCodeValidationTbx.Text = "Failure! Not all hashes match.";
                hashCodeValidationTbx.BackColor = Color.LightPink;
            }
        }

        private bool hashCodesMatched(List<string> hashList, int index)
        {
            if (index == 0)
            {
                return true;
            }
            else
            {
                // Compare the two adjacent hashcodes and make a call downwards.
                return hashList[index] == hashList[index - 1] && hashCodesMatched(hashList, index - 1);
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

        private Proposition CreateTautology(Proposition variable)
        {
            Conjunction conjunction = new Conjunction();
            Disjunction tautology = new Disjunction();
            tautology.LeftSuccessor = variable; // A
            Negation negatedVariable = new Negation();
            negatedVariable.LeftSuccessor = variable;
            tautology.RightSuccessor = negatedVariable; // A | ~(A) == 1
            return tautology;
        }

        private Proposition CreateContradiction(Proposition variable)
        {
            Negation negation = new Negation();
            negation.LeftSuccessor = CreateTautology(variable);
            return negation;
        }

        private void ResetHashRelatedItems()
        {
            // The list of hashes used for comparing the hash codes to ensure the program is working correctly.
            hashList.Clear();

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
