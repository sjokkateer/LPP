using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using LogicAndSetTheoryApplication;

namespace LPPUnitTests
{
    public class ImplicationTests : BinaryConnectiveTestsBase
    {
        public ImplicationTests() : base(Implication.SYMBOL)
        { }

        [Theory]
        [InlineData(false, false, true)]
        [InlineData(false, true, true)]
        [InlineData(true, false, false)]
        [InlineData(true, true, true)]
        public void Calculate_DetermineAllPossibleValuesBetweenTwoPropositionVariables_ExpectedToReturnFalseWhenLeftTrueRightFalse(bool leftTruthValue, bool rightTruthValue, bool expectedTruthValue)
        {
            // Arrange
            Implication implication = generateImplication();
            string message = "because implication is false when right successor is true and left falfe.";

            // Act // Assert
            Calculate_DetermineAllPossibleValuesBetweenTwoPropositionVariables(implication, message, leftTruthValue, rightTruthValue, expectedTruthValue);
        }

        private Implication generateImplication()
        {
            return (Implication)createBinaryConnective();
        }

        [Fact]
        public void Copy_CopyingImplicationWithTwoRandomVariableSymbols_ExpectedDifferentReferencesForConnective()
        {
            // Arrange // Act // Assert
            Implication implication = generateImplication();

            Copy_CopyingBinaryConnectiveWithTwoRandomVariableSymbols_ExpectedDifferentReferencesForConnective(implication);
        }

        [Fact]
        public void Nandify_CallToNandifyOnImplicationWithTwoPropositionChildren_ExpectedPropositionWithNandStructure()
        {
            TestNandify();
        }
    }
}
