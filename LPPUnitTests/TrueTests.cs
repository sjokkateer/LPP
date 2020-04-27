using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using LogicAndSetTheoryApplication;

namespace LPPUnitTests
{
    public class TrueTests : PropositionConstantBase
    {
        public TrueTests() : base(new True())
        { }

        public override void ActualTruthValueShouldBe(bool actualTruthValue)
        {
            actualTruthValue.Should().BeTrue("because the constant true is always true");
        }
    }
}
