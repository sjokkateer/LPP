using System;
using Xunit;
using LogicAndSetTheoryApplication;

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
        [InlineData("=(1, |(A, U))", typeof(BiImplication))]
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
    }
}
