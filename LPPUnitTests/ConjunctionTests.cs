using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;
using LogicAndSetTheoryApplication;

namespace LPPUnitTests
{
    public class ConjunctionTests : BinaryConnectiveTestsBase
    {
        public ConjunctionTests() : base(Conjunction.SYMBOL)
        { }

        [Theory]
        [InlineData(false, false, false)]
        [InlineData(false, true, false)]
        [InlineData(true, false, false)]
        [InlineData(true, true, true)]
        public void Calculate_CalculateAllPossibleTruthValues_ExpectedTrueWhenLeftAndRightAreTrue(bool leftTruthValue, bool rightTruthValue, bool expectedTruthValue)
        {
            // Arrange
            Conjunction conjunction = generateConjunction();
            string message = "because conjunction is true when both left and right are true";

            Calculate_DetermineAllPossibleValuesBetweenTwoPropositionVariables(conjunction, message, leftTruthValue, rightTruthValue, expectedTruthValue);
        }

        private Conjunction generateConjunction()
        {
            return (Conjunction)createBinaryConnective();
        }

        [Fact]
        public void Copy_CopyingConjunctionWithTwoRandomVariableSymbols_ExpectedDifferentReferencesForConnective()
        {
            // Arrange // Act // Assert
            Conjunction conjunction = generateConjunction();

            Copy_CopyingBinaryConnectiveWithTwoRandomVariableSymbols_ExpectedDifferentReferencesForConnective(conjunction);
        }
    }
}
