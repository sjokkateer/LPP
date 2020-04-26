using System;
using Xunit;
using LogicAndSetTheoryApplication;
using System.Collections.Generic;
using FluentAssertions;

namespace LPPUnitTests
{
    public class ParserTests
    {
        private Parser parser;

        [Theory]
        [InlineData("~(A)", typeof(Negation))]
        [InlineData("&(A, B)", typeof(Conjunction))]
        [InlineData("U", typeof(Proposition))]
        [InlineData("1", typeof(True))]
        [InlineData("0", typeof(False))]
        [InlineData("=(1, |(A, U))", typeof(BiImplication))]
        [InlineData(">(X, ~(A))", typeof(Implication))]
        public void Parse_DifferentValidPrefixPropositions_SuccessfullyParsedPropositionRootReturned(string proposition, object typeOfRoot)
        {
            // Arrange
            parser = new Parser(proposition);

            // Act
            Proposition root = parser.Parse();

            // Assert
            Assert.Equal(root.GetType(), typeOfRoot);
        }

        // For errors, some invalid strings.
        // Empty string
        // null input
        // Open and closing parenthesis missmatch.

        // Parse an expression containing the same variable character multiple times.
        // Test that the reference is the same for all occurences.
        [Fact]
        public void Parse_SameSymbolCharacterInExpression_ShouldBeSameObject()
        {
            // Arrange
            char randomSymbol = PropositionTests.getRandomVariableLetter();
            char randomConnective = PropositionTests.getRandomConnective();

            string proposition = $"{randomConnective}({randomSymbol}, {randomSymbol})";
            int expectedNumberOfVariables = 1;
            
            parser = new Parser(proposition);

            // Act
            Proposition root = parser.Parse();
            List<Proposition> expressionVariables = root.GetVariables();

            // Assert
            expressionVariables.Count.Should().Be(expectedNumberOfVariables, "because it is the same symbol character");
            expressionVariables[0].Data.Should().Be(randomSymbol, "because that is the symbol given to the proposition by the parser");
        }

        [Fact]
        public void Parse_UniqueSymbolCharactersInExpression_ShouldResultInUniqueVariables()
        {
            // Arrange
            string proposition = "~(&(|(A, B), C))";
            int expectedNumberOfVariables = 3;

            parser = new Parser(proposition);

            // Act
            Proposition root = parser.Parse();
            List<Proposition> expressionVariables = root.GetVariables();

            // Assert
            expressionVariables.Count.Should().Be(expectedNumberOfVariables, "because it is the same symbol character");
        }
    }
}
