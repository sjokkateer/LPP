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
        public bool[] Cells { get; set; }

        public TruthTableRow(Proposition propositionRoot, List<Proposition> uniqueVariabels, int numberOfVariables)
        {
            this.propositionRoot = propositionRoot;
            this.uniqueVariables = uniqueVariabels;
            Cells = new bool[numberOfVariables];
        }

        public bool Calculate()
        {
            for (int i = 0; i < uniqueVariables.Count; i++)
            {
                uniqueVariables[i].TruthValue = Cells[i];
            }
            return propositionRoot.Calculate();
        }

        public TruthTableRow Copy()
        {
            TruthTableRow rowCopy = new TruthTableRow(propositionRoot, uniqueVariables, Cells.Length);
            for (int i = 0; i < Cells.Length; i++)
            {
                rowCopy.Cells[i] = Cells[i];
            }
            return rowCopy;
        }

        public override string ToString()
        {
            string result = "";
            string cell = "";
            int truthValue;

            for (int i = 0; i < Cells.Length; i++)
            {
                truthValue = Convert.ToInt32(Cells[i]);
                cell = $"  {truthValue}";
                if (i == 0)
                {
                    cell = $"{truthValue}";
                }
                result += cell;
            }
            result += $"  {Convert.ToInt32(Calculate())}";
            return result;
        }
    }
}
