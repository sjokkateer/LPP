using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    class TruthTableCell
    {
        private Proposition variable;
        private bool truthValue;

        public TruthTableCell(Proposition variable, bool truthValue)
        {
            this.variable = variable;
            this.truthValue = truthValue;
        }

        /// <summary>
        /// Method is responsible for assigning the truthValue to the proposition variable
        /// it holds. Such that it is ready for being used in calculation.
        /// </summary>
        public void Substititue()
        {
            variable.TruthValue = truthValue;
        }

        public override string ToString()
        {
            return $"{Convert.ToByte(truthValue)}";
        }
    }
}
