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
        public bool Result { get; private set; }

        public TruthTableRow(Proposition propositionRoot, List<Proposition> uniqueVariables, int numberOfVariables)
        {
            this.propositionRoot = propositionRoot;
            this.uniqueVariables = uniqueVariables;
            Cells = new bool[numberOfVariables];
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
                uniqueVariables[i].TruthValue = Cells[i];
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
            // Here I calculated the value of the row, to append it to the string representation of the table row object.
            result += $"  {Convert.ToInt32(Result)}";
            return result;
        }
    }
}
