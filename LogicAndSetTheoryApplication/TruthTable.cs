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
        public List<List<TruthTableRow>> Rows { get; }

        public TruthTable(Proposition propositionRoot)
        {
            this.propositionRoot = propositionRoot;
            propositionVariablesSet = propositionRoot.GetVariables();
            Rows = new List<List<TruthTableRow>>();
            CreateTruthTableRows();
        }
        private void CreateTruthTableRows()
        {
            // Add 2^#(elements) rows to the List of TruthTableRows.

        }
    }
}
