using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    public class TruthTable
    {
        private Proposition propositionRoot;
        private List<Proposition> propositionVariablesSet;
        public List<ITruthTableRow> Rows { get; }

        public TruthTable(Proposition propositionRoot)
        {
            this.propositionRoot = propositionRoot;
            // Transparent caching?
            if (propositionRoot.UniqueVariableSet == null)
            {
                propositionVariablesSet = propositionRoot.GetVariables();
            }
            else
            {
                propositionVariablesSet = propositionRoot.UniqueVariableSet;
            }
            // Place them in alphabetic order
            propositionVariablesSet.Sort();
            Rows = new List<ITruthTableRow>();
            CreateTruthTableRows();
        }

        public TruthTable(Proposition propositionRoot, List<Proposition> propositionVariablesSet, List<ITruthTableRow> simplifiedRows)
        {
            this.propositionRoot = propositionRoot;
            this.propositionVariablesSet = propositionVariablesSet;
            Rows = simplifiedRows;
        }

        public TruthTable(List<ITruthTableRow> rows)
        {
            Rows = rows;
        }

        // Is used by the hashcode calculator
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
            TruthTableRow initialRow = new TruthTableRow(propositionRoot, propositionVariablesSet, propositionVariablesSet.Count);
            if (initialRow.Cells.Length == 0)
            {
                // We had no variables and are dealing with one of the constants.
                // return an empty row with only a result value.
                initialRow.Calculate();
                Rows.Add(initialRow);
            }
            else
            {
                CreateRowsRecursively(0, new char[] { '0', '1' }, initialRow);
            }   
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
            // This is something we only have to execute if we have abstract proposition variables.
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
                    // Not at the final variable so we add the cell into the row and call the method again.
                    row.Cells[index] = truthValue;
                    CreateRowsRecursively(index + 1, truthSet, row);
                }
            }
        }

        public TruthTable Simplify()
        {
            return new TruthTable(propositionRoot, propositionVariablesSet, SimplifyRowsIteratively(Rows));
        }

        private bool EqualSets(List<ITruthTableRow> set1, List<ITruthTableRow> set2)
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

        private List<ITruthTableRow> SimplifyRowsIteratively(List<ITruthTableRow> simplifiedRowSet)
        {
            List<ITruthTableRow> oldSet;
            // Create new test condition, testing if both sets are equal or not.
            do
            {
                oldSet = simplifiedRowSet;
                simplifiedRowSet = SimplifiyRowSet(simplifiedRowSet);
            } while (!(EqualSets(oldSet, simplifiedRowSet)));
            return simplifiedRowSet;
        }

        private List<ITruthTableRow> SimplifyRowsRecursively(int rowCount, List<ITruthTableRow> simplifiedRowSet)
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

        private void PrintRows(List<ITruthTableRow> rows)
        {
            foreach (TruthTableRow r in rows)
            {
                Console.WriteLine(r);
            }
        }

        private bool IsRowInSet(ITruthTableRow row, List<ITruthTableRow> rowSet)
        {
            foreach(ITruthTableRow r in rowSet)
            {
                if (row.EqualTo(r))
                {
                    return true;
                }
            }
            return false;
        }

        private List<ITruthTableRow> SimplifiyRowSet(List<ITruthTableRow> rowSet)
        {
            List<ITruthTableRow> simplifiedSet = new List<ITruthTableRow>();
            for (int i = 0; i < rowSet.Count; i++)
            {
                for (int j = 0; j < rowSet.Count; j++)
                {
                    // Skip comparing the same row.
                    if (j != i)
                    {
                        // Only attempt to simplify the row if they have the same result value.
                        if (rowSet[i].Result == rowSet[j].Result)
                        {
                            // Try to simplify the two by a method call.
                            // The method will return null if it was not succesfull
                            // or a new row if it was successful.
                            ITruthTableRow simplifiedRow = rowSet[i].Simplify(rowSet[j]);
                            if (simplifiedRow != null)
                            {
                                // Only assign is simplified to row i since that's the one under inspection.
                                rowSet[i].IsSimplified = true;
                                //rowSet[j].IsSimplified = true;

                                // Here we ensure that we only return unique rows,
                                // that means rows with all different symbols in the cells.1
                                if (IsRowInSet(simplifiedRow, simplifiedSet) == false)
                                {
                                    simplifiedSet.Add(simplifiedRow);
                                }
                            }
                        }
                    }
                }

                // We exhausted our option, meaning that row i was either simplified or not simplified,
                // thus we have to add it to our set, ensuring one unique row is in it.
                if (rowSet[i].IsSimplified == false)
                {
                    if (IsRowInSet(rowSet[i], simplifiedSet) == false)
                    {
                        simplifiedSet.Add(rowSet[i]);
                    }
                }
            }

            // If the set could not be simplified any further we can rewturn the original.
            if (simplifiedSet.Count == 0)
            {
                return rowSet;
            }

            // Else return the set of simplified rows.
            return simplifiedSet;
        }

        #region Disjunctive Normal Form
        public Proposition CreateDisjunctiveNormalForm()
        {
            List<Proposition> propositionList = new List<Proposition>();
            foreach (TruthTableRow row in Rows)
            {
                // if and only if the row has a result of 1.
                if (row.Result == true)
                {
                    // Then we want to convert that row into a new expression.
                    // and use it to construct a disjunct until the end.
                    propositionList.Add(row.GetDisjunctiveNormalFormEquivalent());
                }
            }
            while (propositionList.Count > 1)
            {
                // take vriable at pos 0 and 1 and create a disjunct between them.
                // Insert them back in the first position of the list.
                Disjunction disjunct = new Disjunction();
                // We do not need to create copies of variables since we did that beforehand.
                disjunct.LeftSuccessor = propositionList[0];
                disjunct.RightSuccessor = propositionList[1];
                // Remove the individual variables from the list.
                propositionList.RemoveAt(1);
                propositionList.RemoveAt(0);
                // Insert the conjunct into the list.
                propositionList.Add(disjunct);
            }
            if (propositionList.Count > 0)
            {
                return propositionList[0];
            }
            foreach (TruthTableRow r in Rows)
            {
                Console.WriteLine(r);
                Console.WriteLine(r.Result);
            }
            
            return new False();
        }
        #endregion

        public string TableHeader()
        {
            string result = "";
            for (int i = 0; i < propositionVariablesSet.Count; i++)
            { 
                result += $"{propositionVariablesSet[i]}  ";
            }
            result += "v\n";
            return result;
        }

        public override string ToString()
        {
            string result = TableHeader();
            for (int i = 0; i < Rows.Count; i++)
            {
                result += $"{Rows[i]}\n";
            }
            return result;
        }
    }
}
