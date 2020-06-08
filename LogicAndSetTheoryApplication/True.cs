using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    public class True : Constant
    {
        public const char SYMBOL = '1';

        public True() : base(SYMBOL)
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

        public override Proposition Copy()
        {
            return new True();
        }

        public override Proposition Nandify()
        {
            return this;
        }
    }
}
