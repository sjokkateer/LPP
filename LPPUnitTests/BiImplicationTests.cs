using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using LogicAndSetTheoryApplication;
using Xunit;

namespace LPPUnitTests
{
    public class BiImplicationTests : BinaryConnectiveTestsBase
    {
        public BiImplicationTests() : base(BiImplication.SYMBOL)
        { }

        [Theory]
        [InlineData(false, false, true)]
        [InlineData(false, true, false)]
        [InlineData(true, false, false)]
        [InlineData(true, true, true)]
        public void Calculate_DetermineAllPossibleValuesBetweenTwoPropositionVariables_ExpectedToReturnTrueWhenBothHaveEqualTruthValues(bool leftTruthValue, bool rightTruthValue, bool expectedTruthValue)
        {
            // Arrange
            BiImplication biImplication = generateBiImplication();
            string message = "because bi implication is true when both left and right successor have the same truth value.";

            // Act // Assert
            Calculate_DetermineAllPossibleValuesBetweenTwoPropositionVariables(biImplication, message, leftTruthValue, rightTruthValue, expectedTruthValue);
        }

        // Provide the binary connective and the string message to the base method.
        private BiImplication generateBiImplication()
        {
            return (BiImplication)createBinaryConnective();
        }

        [Fact]
        public void Copy_CopyingBiImplicationWithTwoRandomVariableSymbols_ExpectedDifferentReferencesForConnective()
        {
            // Arrange // Act // Assert
            BiImplication biImplication = generateBiImplication();

            Copy_CopyingBinaryConnectiveWithTwoRandomVariableSymbols_ExpectedDifferentReferencesForConnective(biImplication);
        }
    }
}
