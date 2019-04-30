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
            return new TruthTable(propositionRoot, propositionVariablesSet, SimplifyRowsIteratively(Rows));
        }

        private bool EqualSets(List<TruthTableRow> set1, List<TruthTableRow> set2)
        {
            // Different number of rows automatically indicates they are different.
            if (set1.Count != set2.Count)
            {
                return false;
            }
            else
            {
                // Test if every row is equal (cell values).
                for (int i = 0; i < set1.Count; i++)
                {
                    if (!set1[i].EqualTo(set2[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        private List<TruthTableRow> SimplifyRowsIteratively(List<TruthTableRow> simplifiedRowSet)
        {
            List<TruthTableRow> oldSet;
            // Create new test condition, testing if both sets are equal or not.
            do
            {
                oldSet = simplifiedRowSet;
                simplifiedRowSet = SimplifiyRowSet(simplifiedRowSet);
            } while (!(EqualSets(oldSet, simplifiedRowSet)));
            return simplifiedRowSet;
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

        private void PrintRows(List<TruthTableRow> rows)
        {
            foreach (TruthTableRow r in rows)
            {
                Console.WriteLine(r);
            }
        }

        private bool IsRowInSet(TruthTableRow row, List<TruthTableRow> rowSet)
        {
            foreach(TruthTableRow r in rowSet)
            {
                if (row.EqualTo(r))
                {
                    return true;
                }
            }
            return false;
        }

        private List<TruthTableRow> SimplifiyRowSet(List<TruthTableRow> rowSet)
        {
            // Debugging statements
            Console.WriteLine("Original: ");
            PrintRows(rowSet);
            Console.WriteLine();
            //

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
                            rowSet[i].IsSimplified = true;
                            rowSet[j].IsSimplified = true;
                            if (IsRowInSet(simplifiedRow, simplifiedSet) == false)
                            {
                                simplifiedSet.Add(simplifiedRow);
                            }
                        }
                    }
                }
                // We exhausted our option, meaning that row i was not simplified,
                // thus we have to add it to our set, ensuring one unique row is in it.
                if (rowSet[i].IsSimplified == false)
                {
                    if (IsRowInSet(rowSet[i], simplifiedSet) == false)
                    {
                        simplifiedSet.Add(rowSet[i]);
                    }
                }
            }

            // Debugging statements
            Console.WriteLine("Simplified: ");
            PrintRows(simplifiedSet);
            Console.WriteLine();
            Console.WriteLine($"{simplifiedSet.Count == 0}");
            //

            if (simplifiedSet.Count == 0)
            {
                return rowSet;
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
