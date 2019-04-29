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
            propositionVariablesSet.Sort();
            Rows = new List<TruthTableRow>();
            CreateTruthTableRows();
        }

        public TruthTable(Proposition propositionRoot, List<Proposition> propositionVariablesSet, List<TruthTableRow> simplifiedRows)
        {
            this.propositionRoot = propositionRoot;
            this.propositionVariablesSet = propositionVariablesSet;
            Rows = simplifiedRows;
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
            CreateRowsRecursively(0, new char[] { '0', '1' }, new TruthTableRow(propositionRoot, propositionVariablesSet, propositionVariablesSet.Count));
        }

        /// <summary>
        /// Recursive method that evaluates a true and false value for every unique variable,
        /// exhausting all possible permutations and creating a row copy for every such row.
        /// This row copy is then added to the collection of rows of this truth table object.
        /// </summary>
        /// <param name="index">The current index, starting at 0, and evaluating until the index is equal to the number of unique variables - 1</param>
        /// <param name="truthSet">A constant set of the two boolean values false and true</param>
        /// <param name="row">A truth table row object that holds the combination of truth values for the abstract proposition variables.</param>
        private void CreateRowsRecursively(int index, char[] truthSet, TruthTableRow row)
        {
            foreach (char truthValue in truthSet)
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

        public TruthTable Simplify()
        {
            return new TruthTable(propositionRoot, propositionVariablesSet, SimplifyRowsRecursively(-1, Rows));
        }

        private List<TruthTableRow> SimplifyRowsRecursively(int rowCount, List<TruthTableRow> simplifiedRowSet)
        {
            if (rowCount == simplifiedRowSet.Count)
            {
                return simplifiedRowSet;
            }
            else
            {
                int rowCountBeforeSimplification = simplifiedRowSet.Count;
                simplifiedRowSet = SimplifiyRowSet(simplifiedRowSet);
                return SimplifyRowsRecursively(rowCountBeforeSimplification, simplifiedRowSet);
            }
        }

        private List<TruthTableRow> SimplifiyRowSet(List<TruthTableRow> rowSet)
        {
            List<TruthTableRow> simplifiedSet = new List<TruthTableRow>();
            for (int i = 0; i < rowSet.Count; i++)
            {
                for (int j = i + 1; j < rowSet.Count; j++)
                {
                    if (rowSet[i].Result == rowSet[j].Result)
                    {
                        // Try to simplify the two by a method call.
                        // The method will return null if it was not succesfull
                        // or a new row if it was successful.
                        TruthTableRow simplifiedRow = rowSet[i].Simplify(rowSet[j]);
                        if (simplifiedRow != null)
                        {
                            // The rows have been simplified thus we can assign the IsSimplified to Rowset[i]
                            // And add the obtained row to the simplifiedSet.
                            rowSet[i].IsSimplified = true;
                            rowSet[j].IsSimplified = true;
                            simplifiedSet.Add(simplifiedRow);
                        }
                    }
                }
                if (rowSet[i].IsSimplified == false)
                {
                    simplifiedSet.Add(rowSet[i]);
                }
            }
            return simplifiedSet;
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
