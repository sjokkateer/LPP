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
            // Act
            Action act = () => UNARY_CONNECTIVE.ToString();

            // Assert
            act.Should().Throw<NullReferenceException>("Because printing the infix form woudl not make sense without a successor");
        }
    }
}
