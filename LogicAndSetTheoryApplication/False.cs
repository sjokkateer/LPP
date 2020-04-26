using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    public class False : Proposition
    {
        public False() : base('0')
        {
            TruthValue = false;
        }

        public override List<Proposition> GetVariables()
        {
            return new List<Proposition>();
        }

        public override bool Calculate()
        {
            return false;
        }

        public override Proposition Nandify()
        {
            return this;
        }
    }
}
