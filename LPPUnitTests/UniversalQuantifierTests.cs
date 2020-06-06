using System;
using System.Collections.Generic;
using System.Text;
using LogicAndSetTheoryApplication;
using Xunit;
using FluentAssertions;
using System.Diagnostics;

namespace LPPUnitTests
{
    public class UniversalQuantifierTests: QuantifierTestsBase
    {
        protected override char GetBoundVariable()
        {
            return 'x';
        }

        protected override Quantifier GetQuantifier()
        {
            return new UniversalQuantifier(GetBoundVariable());
        }
    }
}
