using System;
using System.Collections.Generic;
using System.Text;
using LogicAndSetTheoryApplication;
using FluentAssertions;
using Xunit;

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
            return PropositionGenerator.CreateBinaryConnectiveWithRandomSymbols(symbol);
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
            bool sameReference = originalConenctive == copy;

            // Assert
            originalConenctive.Equals(copy).Should().BeTrue("Because all the data should be equal");
            sameReference.Should().BeFalse("Because a copy should be a different object reference");
        }

        public virtual void TestNandify()
        {
            // Arrange
            Proposition validProposition = PropositionGenerator.CreateBinaryConnectiveWithRandomSymbols(symbol);

            // Act
            Proposition nandifiedProposition = validProposition.Nandify();

            // Assert
            NandChecker.hasNandStructure(new List<Proposition>() { nandifiedProposition });
        }

        public virtual void GetBoundVariables_CalledOnABinaryConnective_ExpectedNotImplementedExceptionThrown()
        {
            // Arrange
            BinaryConnective binaryConnective = createBinaryConnective();
            Action act = () => binaryConnective.GetBoundVariables();

            // Act // Assert
            act.Should().Throw<NotImplementedException>("Because it is not clear what should happen when calling this on a binary connective");
        }
    }
}
