using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;
using LogicAndSetTheoryApplication;

namespace LPPUnitTests
{
    public class ExistentialQuantifierTests: QuantifierTestsBase
    {
        protected override char GetBoundVariable()
        {
            return 'y';
        }

        protected override Quantifier GetQuantifier()
        {
            return new ExistentialQuantifier(GetBoundVariable());
        }
    }
}
