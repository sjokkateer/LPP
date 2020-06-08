using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    public class False : Constant
    {
        public const char SYMBOL = '0';

        public False() : base(SYMBOL)
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

        public override Proposition Copy()
        {
            return new False();
        }

        public override Proposition Nandify()
        {
            return this;
        }
    }
}
