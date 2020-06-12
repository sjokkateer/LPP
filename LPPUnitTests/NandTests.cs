using LogicAndSetTheoryApplication;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LPPUnitTests
{
    public class NandTests : BinaryConnectiveTestsBase
    {
        public NandTests() : base(Nand.SYMBOL)
        { }


        private Nand generateNand()
        {
            return (Nand)createBinaryConnective();
        }

        [Theory]
        [InlineData(false, false, true)]
        [InlineData(false, true, true)]
        [InlineData(true, false, true)]
        [InlineData(true, true, false)]
        public void Calculate_CalculateAllPossibleTruthValues_ExpectedTrueWhenLeftAndRightAreTrue(bool leftTruthValue, bool rightTruthValue, bool expectedTruthValue)
        {
            // Arrange
            Nand nand = generateNand();
            string message = "because nand is only false when both left and right are true";

            // Act // Assert
            Calculate_DetermineAllPossibleValuesBetweenTwoPropositionVariables(nand, message, leftTruthValue, rightTruthValue, expectedTruthValue);
        }

        [Fact]
        public void Copy_CopyingConjunctionWithTwoRandomVariableSymbols_ExpectedDifferentReferencesForConnective()
        {
            // Arrange // Act // Assert
            Nand nand = generateNand();

            Copy_CopyingBinaryConnectiveWithTwoRandomVariableSymbols_ExpectedDifferentReferencesForConnective(nand);
        }

        [Fact]
        public void Nandify_CallToNandifyOnNandWithTwoPropositionChildren_ExpectedPropositionWithNandStructure()
        {
            TestNandify();
        }

        [Fact]
        public override void GetBoundVariables_CalledOnABinaryConnective_ExpectedNotImplementedExceptionThrown()
        {
            base.GetBoundVariables_CalledOnABinaryConnective_ExpectedNotImplementedExceptionThrown();
        }
    }
}
