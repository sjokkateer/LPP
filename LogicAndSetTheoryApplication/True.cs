using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    class True : Proposition
    {
        public True() : base('1')
        {
            TruthValue = true;
        }

        public override List<Proposition> GetVariables()
        {
            return new List<Proposition>();
        }

        public override bool Calculate()
        {
            return true;
        }

        public override Proposition Nandify()
        {
            return this;
        }
    }
}
