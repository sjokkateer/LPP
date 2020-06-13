using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;
using LogicAndSetTheoryApplication;

namespace LPPUnitTests
{
    public class UnaryConnectiveTests
    {
        private static UnaryConnective UNARY_CONNECTIVE = new UnaryConnective('/');
        private static Proposition PROPOSITION_SUCCESSOR = new Proposition('H');
        private static BinaryConnective BINARY_CONNECTIVE_SUCCESSOR = new BinaryConnective('-');

        [Fact]
        public void ToString_UnaryConnectiveWithPropositionAsSuccessor_()
        {
            // Arrange
            UNARY_CONNECTIVE.LeftSuccessor = PROPOSITION_SUCCESSOR;

            // Act
            string actualToString = UNARY_CONNECTIVE.ToString();
            string expectedToString = $"{UNARY_CONNECTIVE.Data}({PROPOSITION_SUCCESSOR})";

            // Assert
            actualToString.Should().Be(expectedToString, "Because the unary connectives only has a proposition as successor.");
        }

        [Fact]
        public void ToString_UnaryConnectiveWithBinaryConnectiveAsSuccessor_()
        {
            // Arrange
            UNARY_CONNECTIVE.LeftSuccessor = BINARY_CONNECTIVE_SUCCESSOR;
            BINARY_CONNECTIVE_SUCCESSOR.LeftSuccessor = PROPOSITION_SUCCESSOR;
            BINARY_CONNECTIVE_SUCCESSOR.RightSuccessor = PROPOSITION_SUCCESSOR;

            // Act
            string actualToString = UNARY_CONNECTIVE.ToString();
            
            string firstChar = actualToString.Substring(0, 1);
            string secondChar = actualToString.Substring(1, 1);
            string lastChar = actualToString.Substring(actualToString.Length - 1, 1);

            // Assert
            firstChar.Should().Be(UNARY_CONNECTIVE.Data.ToString(), "Because the proposition starts off with the unary connective.");
            secondChar.Should().Be("(", "Because after the connective symbol the parenth should open.");
            lastChar.Should().Be(")", "Because at the end of the expression the parenth should be closed.");
        }

        [Fact]
        public void ToString_UnaryConnectiveWithNoSuccessorSet_ExpectedNullReferenceException()
        {
            // Arrange
            UNARY_CONNECTIVE.LeftSuccessor = null;

            // Act
            Action act = () => UNARY_CONNECTIVE.ToString();

            // Assert
            act.Should().Throw<NullReferenceException>("Because printing the infix form woudl not make sense without a successor");
        }

        [Fact]
        public void GetBoundVariables_UnaryConnectiveWithPropositionAsSuccessor_ExpectedNotImplementedExceptionThrown()
        {
            // Arrange
            UnaryConnective negation = PropositionGenerator.CreateUnaryConnectiveWithRandomSymbol(Negation.SYMBOL);
            Action act = () => negation.GetBoundVariables();

            // Act // Assert
            act.Should().Throw<NotImplementedException>("Because an abstract proposition does not have bound variables related to it.");
        }

        [Fact]
        public void GetBoundVariables_UnaryConnectiveWithPredicateAsSuccessor_NonEmptyListOfVariablesReturned()
        {
            // Arrange
            UnaryConnective negation = PropositionGenerator.CreateUnaryConnectiveWithRandomSymbol(Negation.SYMBOL);
            negation.LeftSuccessor = new Predicate('T', new List<char>() { 'r', 't' });

            // Act 
            List<char> boundVariables = negation.GetBoundVariables();
            int expectedNumberOfBoundVariables = 2;
            int actualNumberOfBoundVariables = boundVariables.Count;

            // Assert
            actualNumberOfBoundVariables.Should().Be(expectedNumberOfBoundVariables, $"Because a predicate is assigned as successor with {expectedNumberOfBoundVariables} variables assigned");
        }

        [Fact]
        public void ToPrefixString_LeftSuccessorAssigned_ExpectedPrefixStringReturned()
        {
            // Arrange
            Negation negation = new Negation();
            negation.LeftSuccessor = new Predicate('Q', new List<char>() { 'x', 'y' });

            // Act
            string actualToPrefixString = negation.ToPrefixString();

            string expectedToPrefixString = "~(Q(x, y))";

            // Assert
            actualToPrefixString.Should().Be(expectedToPrefixString, "Because that's the prefix format.");
        }

        [Fact]
        public void ToPrefixString_LeftSuccessorNotAssigned_ExpectedNullReferenceExceptionThrown()
        {
            // Arrange
            Negation negation = new Negation();
            Action act = () => negation.ToPrefixString();

            // Act // Assert
            act.Should().Throw<NullReferenceException>("Because calling this method does not make sense without a successor");
        }
    }
}
