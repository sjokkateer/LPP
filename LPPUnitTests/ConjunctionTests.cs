using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;
using LogicAndSetTheoryApplication;

namespace LPPUnitTests
{
    public class ConjunctionTests
    {
        [Theory]
        [InlineData(false, false, false)]
        [InlineData(false, true, false)]
        [InlineData(true, false, false)]
        [InlineData(true, true, true)]
        public void Calculate_CalculateAllPossibleTruthValues_ExpectedTrueWhenLeftAndRightAreTrue(bool leftTruthValue, bool rightTruthValue, bool expectedTruthValue)
        {
            // Arrange
            Conjunction conjunction = generateBiImplication();
            conjunction.LeftSuccessor.TruthValue = leftTruthValue;
            conjunction.RightSuccessor.TruthValue = rightTruthValue;

            // Act
            bool actualTruthValue = conjunction.Calculate();

            // Assert
            actualTruthValue.Should().Be(expectedTruthValue, "because conjunction is true when both left and right are true");
        }

        private Conjunction generateBiImplication()
        {
            return (Conjunction)PropositionGenerator.createBinaryConnectiveWithRandomSymbols('&');
        }

        [Fact]
        public void Copy_CopyingConjunctionWithTwoRandomVariableSymbols_ExpectedDifferentReferencesForConnective()
        {
            // Arrange
            Conjunction conjunction = generateBiImplication();

            // Act
            Conjunction copy = (Conjunction) conjunction.Copy();

            // Assert
            conjunction.Equals(copy).Should().BeFalse("because it is a copy of the original");
        }
    }
}
