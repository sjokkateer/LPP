using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    class TruthTableRow
    {
        private Proposition propositionRoot;
        private List<Proposition> uniqueVariables;
        public char[] Cells { get; set; }
        public bool Result { get; set; }
        public bool IsSimplified { get; set; }

        public TruthTableRow(int numberOfVariables) : this(null, null, numberOfVariables)
        { }

        public TruthTableRow(List<Proposition> uniqueVariables, int numberOfVariables) : this(null, uniqueVariables, numberOfVariables)
        { }

        public TruthTableRow(Proposition propositionRoot, List<Proposition> uniqueVariables, int numberOfVariables)
        {
            this.propositionRoot = propositionRoot;
            this.uniqueVariables = uniqueVariables;
            Cells = new char[numberOfVariables];
            IsSimplified = false;
        }

        /// <summary>
        /// Method will assign the stored truth value to each respective
        /// abstract proposition variable, and then recursively calculate
        /// the result.
        /// 
        /// The result is assigned to the result property.
        /// </summary>
        public void Calculate()
        {
            for (int i = 0; i < uniqueVariables.Count; i++)
            {
                uniqueVariables[i].TruthValue = Cells[i] == '1' ? true : false ;
            }
            Result = propositionRoot.Calculate();
        }

        public TruthTableRow Copy()
        {
            TruthTableRow rowCopy = new TruthTableRow(propositionRoot, uniqueVariables, Cells.Length);
            for (int i = 0; i < Cells.Length; i++)
            {
                rowCopy.Cells[i] = Cells[i];
            }
            rowCopy.Result = Result;
            return rowCopy;
        }

        #region Disjunctive Normal Form.
        public Proposition GetDisjunctiveNormalFormEquivalent()
        {
            List<Proposition> propositionList = new List<Proposition>();
            for (int i = 0; i < Cells.Length; i++)
            {
                // return value is null if cell held *
                // Add the obtained variable to the list such that we can process it.
                Proposition resultVariable = GetDisjunctiveNormalFormVariable(Cells[i], uniqueVariables[i]);
                if (resultVariable != null)
                {
                    propositionList.Add(resultVariable);
                }
            }
            // Create conjuncts and insert them at the first position 
            // until there is 1 proposition object left. (the root)
            while (propositionList.Count > 1)
            {
                // take vriable at pos 0 and 1 and create a conjunct between them.
                // Insert them back in the first position of the list.
                Conjunction conjunct = new Conjunction();
                // We do not need to create copies of variables since we did that beforehand.
                conjunct.LeftSuccessor = propositionList[0];
                conjunct.RightSuccessor = propositionList[1];
                // Remove the individual variables from the list.
                propositionList.RemoveAt(1);
                propositionList.RemoveAt(0);
                // Insert the conjunct into the list.
                propositionList.Add(conjunct);
            }
            return propositionList[0];
        }

        private Proposition GetDisjunctiveNormalFormVariable(char truthValue, Proposition variable)
        {
            Proposition disjunctiveNormalFormVariable = null;
            // Can omit any actions if the cell holds a don't care variable 
            // only the case for simplified truth tables.
            if (truthValue != '*')
            {
                if (truthValue == '0')
                {
                    // Negate a copy of the variable.
                    Negation negation = new Negation();
                    negation.LeftSuccessor = variable; // THIS WAS A COPY
                    disjunctiveNormalFormVariable = negation;
                }
                else if (truthValue == '1')
                {
                    // Copy the variable and add it to the expression.
                    disjunctiveNormalFormVariable = variable; // THIS WAS A COPY
                }
            }
            return disjunctiveNormalFormVariable;
        }
        #endregion

        public TruthTableRow Simplify(TruthTableRow otherRow)
        {
            // Instead of creating a memory intense copy, we create a very simple row object
            // that will only hold the truth values in each cell and a result.
            int numberOfVariables = Cells.Length;
            TruthTableRow simplifiedRow = new TruthTableRow(uniqueVariables, numberOfVariables);
            simplifiedRow.Result = Result;
            int numberOfDeferringValues = 0;
            for (int i = 0; i < Cells.Length; i++)
            {
                if (Cells[i] != otherRow.Cells[i])
                {
                    // Then we deal with a don't care variable, which we indicate by null.
                    simplifiedRow.Cells[i] = '*';
                    numberOfDeferringValues++;

                    if (numberOfDeferringValues > 1)
                    {
                        return null;
                    }
                }
                else
                {
                    simplifiedRow.Cells[i] = Cells[i];
                }
            }
            return simplifiedRow;
        }

        public bool EqualTo(TruthTableRow other)
        {
            for (int i = 0; i < Cells.Length; i++)
            {
                if (Cells[i] != other.Cells[i])
                {
                    return false;
                }
            }
            return true;
        }

        public override string ToString()
        {
            string result = "";
            string cell = "";

            for (int i = 0; i < Cells.Length; i++)
            {
                cell = $"  {Cells[i]}";
                if (i == 0)
                {
                    cell = $"{Cells[i]}";
                }
                result += cell;
            }
            result += $"  {Convert.ToInt32(Result)}";
            return result;
        }
    }
}
