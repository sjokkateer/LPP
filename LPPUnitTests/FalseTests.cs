﻿using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using LogicAndSetTheoryApplication;

namespace LPPUnitTests
{
    public class FalseTests : PropositionConstantBase
    {
        public FalseTests() : base(new False())
        { }

        public override void ActualTruthValueShouldBe(bool actualTruthValue)
        {
            actualTruthValue.Should().BeFalse("because the constant false is always false");
        }
    }
}
