using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using LogicAndSetTheoryApplication;
using Xunit;

namespace LPPUnitTests
{
    public class BiImplicationTests
    {
        [Theory]
        [InlineData(false, false, true)]
        [InlineData(false, true, false)]
        [InlineData(true, false, false)]
        [InlineData(true, true, true)]
        public void Calculate_DetermineAllPossibleValuesBetweenTwoPropositionVariables_ExpectedToReturnTrueWhenBothHaveEqualTruthValues(bool leftTruthValue, bool rightTruthValue, bool expectedTruthValue)
        {
            // Arrange
            BiImplication biImplication = createBiImplicationWithRandomSymbols();
            biImplication.LeftSuccessor.TruthValue = leftTruthValue;
            biImplication.RightSuccessor.TruthValue = rightTruthValue;

            // Act
            bool actualTruthValue = biImplication.Calculate();

            // Assert
            actualTruthValue.Should().Be(expectedTruthValue, "because bi implication is true when both left and right successor have the same truth value.");
        }

        [Fact]
        public void Copy_CopyingBiImplicationWithTwoRandomVariableSymbols_ExpectedDifferentReferencesForAllPropositions()
        {
            // Arrange
            BiImplication biImplication = createBiImplicationWithRandomSymbols();

            // Act
            BiImplication copy = (BiImplication) biImplication.Copy();

            // Assert
            biImplication.Should().NotBe(copy, "because it is a copy.");
        }

        private BiImplication createBiImplicationWithRandomSymbols()
        {
            Proposition left = new Proposition(PropositionTests.getRandomVariableLetter());
            Proposition right = new Proposition(PropositionTests.getRandomVariableLetter());

            BiImplication biImplication = new BiImplication();

            biImplication.LeftSuccessor = left;
            biImplication.RightSuccessor = right;

            return biImplication;
        }
    }
}
