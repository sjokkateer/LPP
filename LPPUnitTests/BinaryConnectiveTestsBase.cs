using System;
using System.Collections.Generic;
using System.Text;
using LogicAndSetTheoryApplication;
using FluentAssertions;

namespace LPPUnitTests
{
    public class BinaryConnectiveTestsBase
    {
        protected char symbol;

        public BinaryConnectiveTestsBase(char symbol)
        {
            this.symbol = symbol;
        }

        protected BinaryConnective createBinaryConnective()
        {
            return PropositionGenerator.createBinaryConnectiveWithRandomSymbols(symbol);
        }

        protected void Calculate_DetermineAllPossibleValuesBetweenTwoPropositionVariables(BinaryConnective connective, string message, bool left, bool right, bool expected)
        {
            // Arrange
            connective.LeftSuccessor.TruthValue = left;
            connective.RightSuccessor.TruthValue = right;

            // Act
            bool actualTruthValue = connective.Calculate();

            // Assert
            actualTruthValue.Should().Be(expected, message);
        }

        protected void Copy_CopyingBinaryConnectiveWithTwoRandomVariableSymbols_ExpectedDifferentReferencesForConnective(BinaryConnective originalConenctive)
        {
            // Act
            Proposition copy = originalConenctive.Copy();

            // Assert
            originalConenctive.Equals(copy).Should().BeFalse("because it is a copy.");
        }
    }
}
