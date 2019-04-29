using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    class TruthTable
    {
        private Proposition propositionRoot;
        private List<Proposition> propositionVariablesSet;
        public List<TruthTableRow> Rows { get; }

        public TruthTable(Proposition propositionRoot)
        {
            this.propositionRoot = propositionRoot;
            propositionVariablesSet = propositionRoot.GetVariables();
            Rows = new List<TruthTableRow>();
            CreateTruthTableRows();
        }

        public List<int> GetConvertedResultColumn()
        {
            List<int> resultColumn = new List<int>();
            foreach (TruthTableRow row in Rows)
            {
                resultColumn.Add(Convert.ToInt32(row.Result));
            }
            return resultColumn;
        }

        /// <summary>
        /// Wrapper method that makes the initial call to CreateRowsRecursively with some basic
        /// input parameters that do not change.
        /// </summary>
        private void CreateTruthTableRows()
        {
            CreateRowsRecursively(0, new bool[] { false, true }, new TruthTableRow(propositionRoot, propositionVariablesSet, propositionVariablesSet.Count));
        }

        /// <summary>
        /// Recursive method that evaluates a true and false value for every unique variable,
        /// exhausting all possible permutations and creating a row copy for every such row.
        /// This row copy is then added to the collection of rows of this truth table object.
        /// </summary>
        /// <param name="index">The current index, starting at 0, and evaluating until the index is equal to the number of unique variables - 1</param>
        /// <param name="truthSet">A constant set of the two boolean values false and true</param>
        /// <param name="row">A truth table row object that holds the combination of truth values for the abstract proposition variables.</param>
        private void CreateRowsRecursively(int index, bool[] truthSet, TruthTableRow row)
        {
            foreach (bool truthValue in truthSet)
            {
                if (index == propositionVariablesSet.Count - 1)
                {
                    // add row to the table's list of rows and return back to the calling environment
                    row.Cells[index] = truthValue;
                    TruthTableRow copy = row.Copy();
                    copy.Calculate(); // Required call to Calculate, otherwise the Result truth value can not be set :-(
                    // With the current recursive approach, only one row is constructed inside the wrapper method.
                    Rows.Add(copy);    
                }
                else
                {
                    // Not at the final variabel so we add the cell into the row and call the method again.
                    row.Cells[index] = truthValue;
                    CreateRowsRecursively(index + 1, truthSet, row);
                }
            }
        }

        public string TableHeader()
        {
            string result = "";
            string variable ="";
            for (int i = 0; i < propositionVariablesSet.Count; i++)
            {
                variable = $"{propositionVariablesSet[i]}  ";
                if (i == propositionVariablesSet.Count - 1)
                {
                    variable = $"{propositionVariablesSet[i]}  v\n";
                }
                result += variable;
            }
            return result;
        }

        public override string ToString()
        {
            string result = TableHeader();
            string row = "";

            for (int i = 0; i < Rows.Count; i++)
            {
                row = $"{Rows[i]}\n";
                if (i == Rows.Count - 1)
                {
                    row = $"{Rows[i]}";
                }
                result += row;
            }

            return result;
        }
    }
}
