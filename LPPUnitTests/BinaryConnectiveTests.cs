using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using LogicAndSetTheoryApplication;
using FluentAssertions;

namespace LPPUnitTests
{
    public class BinaryConnectiveTests
    {
        private static BinaryConnective BINARY_CONNECTIVE = new BinaryConnective('{');
        private static Proposition SUCCESSOR = new Proposition('R');
        
        [Fact]
        public void ToString_LeftSuccessorNotSet_ExpectedNullReferenceException()
        {
            // Arrange
            BINARY_CONNECTIVE.LeftSuccessor = null;
            BINARY_CONNECTIVE.RightSuccessor = SUCCESSOR;

            // Act
            Action act = () => BINARY_CONNECTIVE.ToString();

            // Assert
            act.Should().Throw<NullReferenceException>("Because both the successors are required!");
        }

        [Fact]
        public void ToString_RightSuccessorNotSet_ExpectedNullReferenceException()
        {
            // Arrange
            BINARY_CONNECTIVE.RightSuccessor = null;
            BINARY_CONNECTIVE.LeftSuccessor = SUCCESSOR;

            // Act
            Action act = () => BINARY_CONNECTIVE.ToString();

            // Assert
            act.Should().Throw<NullReferenceException>("Because both the successors are required!");
        }

        [Fact]
        public void ToString_BothSuccessorsAssigned_ExpectedToStringReturned()
        {
            // Arrange
            BINARY_CONNECTIVE.LeftSuccessor = SUCCESSOR;
            BINARY_CONNECTIVE.RightSuccessor = SUCCESSOR;

            // Act
            string actualToString = BINARY_CONNECTIVE.ToString();
            
            string openParenth = actualToString.Substring(0, 1);
            string symbol = actualToString.Substring(actualToString.IndexOf(BINARY_CONNECTIVE.Data), 1);
            string closeParenth = actualToString.Substring(actualToString.Length - 1, 1);

            // Assert
            openParenth.Should().Be("(", "Because the expression starts with an open parenthesis.");
            symbol.Should().Be(BINARY_CONNECTIVE.Data.ToString(), "Because between the left and right successor, a connective is placed.");
            closeParenth.Should().Be(")", "Because the expression is ended with a closing parenthesis.");
        }

        [Fact]
        public void ToPrefixString_BothSuccessorsAssigned_ExpectedPrefixStringReturned()
        {
            // Arrange
            BINARY_CONNECTIVE.LeftSuccessor = SUCCESSOR;
            BINARY_CONNECTIVE.RightSuccessor = SUCCESSOR;

            // Act
            string actualToPrefixString = BINARY_CONNECTIVE.ToPrefixString();

            string expectedToPrefixString = $"{BINARY_CONNECTIVE.Data}({SUCCESSOR}, {SUCCESSOR})";

            // Assert
            actualToPrefixString.Should().Be(expectedToPrefixString, "Because the expression starts with a connective symbol.");
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void ToPrefixString_BothSuccessorsNotAssigned_ExpectedNullReferenceExceptionThrown(bool leftAssigned, bool rightAssigned)
        {
            // Arrange
            if (leftAssigned)
            {
                BINARY_CONNECTIVE.LeftSuccessor = SUCCESSOR;
                BINARY_CONNECTIVE.RightSuccessor = null;
            }

            if (rightAssigned)
            {
                BINARY_CONNECTIVE.RightSuccessor = SUCCESSOR;
                BINARY_CONNECTIVE.LeftSuccessor = null;
            }

            Action act = () => BINARY_CONNECTIVE.ToPrefixString();

            // Act // Assert
            act.Should().Throw<NullReferenceException>("Because both successors have to be assigned for a binary connective");
        }
    }
}