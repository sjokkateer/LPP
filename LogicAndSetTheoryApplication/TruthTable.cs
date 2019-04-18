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

        public TruthTable(Proposition propositionRoot)
        {
            this.propositionRoot = propositionRoot;
            propositionVariablesSet = propositionRoot.GetVariables();
            CreateTruthTableRows();
        }
        private void CreateTruthTableRows()
        {
            
        }
    }
}
