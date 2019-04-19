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
        private void CreateTruthTableRows()
        {
            CreateRowsRecursively(0, new bool[] { false, true }, new TruthTableRow(propositionRoot, propositionVariablesSet, propositionVariablesSet.Count));
        }

        private void CreateRowsRecursively(int index, bool[] truthSet, TruthTableRow row)
        {
            foreach (bool truthValue in truthSet)
            {
                if (index == propositionVariablesSet.Count - 1)
                {
                    // add row to the table's list of rows and return back to the calling environment
                    row.Cells[index] = truthValue;
                    Rows.Add(row.Copy()); // Is there a cleaner/better way instead of copying, I am aware the same memory reference for the row is being used otherwise and values overwritten.
                }
                else
                {
                    // Not at the final variabel so we add the cell into the row and call the method again.
                    row.Cells[index] = truthValue;
                    CreateRowsRecursively(index + 1, truthSet, row);
                }
            }
        }

        public override string ToString()
        {
            string result = "";
            string variable = "";

            for (int i = 0; i < propositionVariablesSet.Count; i++)
            {
                variable = $"{propositionVariablesSet[i]}  ";
                if (i == propositionVariablesSet.Count - 1)
                {
                    variable = $"{propositionVariablesSet[i]}  v\n";
                }
                result += variable;
            }

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
