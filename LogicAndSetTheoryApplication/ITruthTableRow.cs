using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    public interface ITruthTableRow
    {
        char[] Cells { get; set; }
        bool IsSimplified { get; set; }
        bool Result { get; set; }
        ITruthTableRow Simplify(ITruthTableRow ttr);
        bool EqualTo(ITruthTableRow other);
    }
}
