using System;
using System.Collections.Generic;
using System.Text;
using LogicAndSetTheoryApplication;
using Xunit;
using FluentAssertions;
using System.Linq;

namespace LPPUnitTests
{
    public class PredicateTests
    {
        private static char PREDICATE_SYMBOL = 'P';

        [Fact]
        public void Constructor_EmptyListOfVariablesGiven_ExpectedArgumentExceptionThrown()
        {
            // Arrange
            List<char> variables = new List<char>();
            Action act = () => new Predicate(PREDICATE_SYMBOL, variables);

            // Act // Assert
            act.Should().Throw<ArgumentException>("Because it relies on a list of at least 1 variable");
        }

        [Fact]
        public void Constructor_NullGivenAsListOfVariables_ExpectedArgumentExceptionThrown()
        {
            // Arrange
            List<char> variables = null;
            Action act = () => new Predicate(PREDICATE_SYMBOL, variables);

            // Act // Assert
            act.Should().Throw<ArgumentException>("Because it relies on a list at least 1 variable");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(11)]
        [InlineData(512)]
        public void Constructor_ListWithNNumberOfVariablesGiven_ExpectedNumberOfVariablesInDictionaryToBeN(int numberOfUniqueVariables)
        {
            // Arrange
            List<char> variables = new List<char>();

            for (int i = 0; i < numberOfUniqueVariables && i < Predicate.LOWER_CASE_ALPHABET.Length; i++)
            {
                variables.Add(Predicate.LOWER_CASE_ALPHABET[i]);
            }

            Predicate predicate = new Predicate(PREDICATE_SYMBOL, variables);

            // Act
            int actualNumberOfVariables = predicate.GetBoundVariables().Count;
            int expectedNumberOfVariables = numberOfUniqueVariables;

            if (numberOfUniqueVariables > Predicate.LOWER_CASE_ALPHABET.Length)
            {
                expectedNumberOfVariables = Predicate.LOWER_CASE_ALPHABET.Length;
            }

            // Assert
            actualNumberOfVariables.Should().Be(expectedNumberOfVariables, "Because every given variable should be used as possible bound variable");
        }

        // Now we need a replace method.
        // This should replace a symbol and return true if it existed and got replaced
        // if it did not exist or was already replaced it should return false.
        [Fact]
        public void Replace_ReplacingAnExistingBoundVariableByANewVariable_ExpectedTrueReturned()
        {
            // Arrange
            List<char> variables = new List<char>()
            {
                'a'
            };

            Predicate predicate = new Predicate(PREDICATE_SYMBOL, variables);

            // Act
            bool wasReplaced = predicate.Replace('a', 'r');

            // Assert
            wasReplaced.Should().BeTrue("Because a existed in the collection of variables and thus should be successfully replaced");
        }

        [Fact]
        public void Replace_ReplacingANonExistingBoundVariableByANewVariable_ExpectedFalseReturned()
        {
            // Arrange
            List<char> variables = new List<char>()
            {
                'a'
            };

            Predicate predicate = new Predicate(PREDICATE_SYMBOL, variables);

            // Act
            char nonExistingVariable = 'x';
            bool wasReplaced = predicate.Replace(nonExistingVariable, 'r');

            // Assert
            wasReplaced.Should().BeFalse($"Because {nonExistingVariable} does not exist in the collection of variables");
        }

        [Fact]
        public void Replace_ReplacingAnAlreadyReplacedExistingBoundVariableByANewVariable_ExpectedFalseReturned()
        {
            // Arrange
            List<char> variables = new List<char>()
            {
                'a'
            };

            Predicate predicate = new Predicate(PREDICATE_SYMBOL, variables);

            // Act
            char variableToBeReplaced = 'a';
            predicate.Replace(variableToBeReplaced, 'z');

            bool wasReplaced = predicate.Replace(variableToBeReplaced, 'h');

            // Assert
            wasReplaced.Should().BeFalse($"Because {variableToBeReplaced} was already replaced once");
        }

        [Fact]
        public void Replace_ReplacingAVariableWithAVariableThatAlreadyExistsAsKey_ExpectedFalseReturned()
        {
            // Arrange
            List<char> variables = new List<char>()
            {
                'a',
                'z'
            };

            Predicate predicate = new Predicate(PREDICATE_SYMBOL, variables);

            // Act
            char variableToBeReplaced = 'a';
            bool wasReplaced = predicate.Replace(variableToBeReplaced, 'z');

            // Assert
            wasReplaced.Should().BeFalse($"Because {variableToBeReplaced} was already present once");
        }

        [Fact]
        public void Replace_ReplacingAVariableWithAValueThathAlreadyExistsAsItemValue_ExpectedFalseReturned()
        {
            // Arrange
            List<char> variables = new List<char>()
            {
                'a',
                'z'
            };

            Predicate predicate = new Predicate(PREDICATE_SYMBOL, variables);

            // Act
            char replacementVariable = 'r';
            predicate.Replace('a', replacementVariable);
            bool wasReplaced = predicate.Replace('z', replacementVariable);

            // Assert
            wasReplaced.Should().BeFalse($"Because {replacementVariable} was already present once");
        }

        [Theory]
        [InlineData("P(z)", 'z')]
        [InlineData("P(a, b)", 'a', 'b')]
        [InlineData("P(a, c)", 'a', 'a', 'c')]
        [InlineData("P(a, c, b)", 'a', 'a', 'c', 'c', 'c', 'b')]
        public void ToString_BeforeReplacementOfVariable_ExpectedOriginalVariablesInString(string expectedString, params char[] variables)
        {
            // Arrange
            Predicate predicate = new Predicate(PREDICATE_SYMBOL, variables.ToList());

            // Act
            string actualString = predicate.ToString();

            // Assert
            actualString.Should().Be(expectedString, "Because that's the format that will be used");
        }

        [Fact]
        public void ToString_OneVariableAfterBeingReplaced_ExpectedReplacementVariableInString()
        {
            // Arrange
            List<char> variables = new List<char>()
            {
                'z'
            };

            Predicate predicate = new Predicate(PREDICATE_SYMBOL, variables.ToList());

            // Act
            predicate.Replace('z', 'g');
            string actualString = predicate.ToString();
            string expectedString = "P(g)";

            // Assert
            actualString.Should().Be(expectedString, "Because the replaced variable should now be displayed in the string");
        }

        [Fact]
        public void ToString_MultipleVariablesReplaced_ExpectedReplacementVariablesInString()
        {
            // Arrange
            List<char> variables = new List<char>()
            {
                'z',
                'l'
            };

            Predicate predicate = new Predicate(PREDICATE_SYMBOL, variables.ToList());

            // Act
            predicate.Replace('l', 'x');
            predicate.Replace('z', 'u');

            string actualString = predicate.ToString();
            string expectedString = "P(u, x)";

            // Assert
            actualString.Should().Be(expectedString, "Because the replaced variable should now be displayed in the string");
        }

        [Fact]
        public void ToString_MultipleVariablesOnlyOneReplaced_ExpectedReplacementVariableInString()
        {
            // Arrange
            List<char> variables = new List<char>()
            {
                'e',
                'w',
                'q'
            };

            Predicate predicate = new Predicate(PREDICATE_SYMBOL, variables);

            // Act
            predicate.Replace('w', 'b');

            string actualString = predicate.ToString();
            string expectedString = "P(e, b, q)";

            // Assert
            actualString.Should().Be(expectedString, "Because the replaced variable should now be displayed in the string");
        }

        [Fact]
        public void IsReplaced_AfterReplacingExistingBoundVariable_ShouldReturnTrue()
        {
            // Arrange
            List<char> variables = new List<char>()
            {
                'p',
                'x',
                'j'
            };

            Predicate predicate = new Predicate(PREDICATE_SYMBOL, variables);
            predicate.Replace('x', 'a');

            // Act
            bool isReplaced = predicate.IsReplaced('x');

            // Assert
            isReplaced.Should().BeTrue("Since the bound variable x should be replaced");
        }

        [Fact]
        public void IsReplaced_AfterReplacingNonExistingBoundVariable_ShouldReturnFalse()
        {
            // Arrange
            List<char> variables = new List<char>()
            {
                'p',
                'x',
                'j'
            };

            Predicate predicate = new Predicate(PREDICATE_SYMBOL, variables);
            predicate.Replace('z', 'i');

            // Act
            bool isReplaced = predicate.IsReplaced('z');

            // Assert
            isReplaced.Should().BeFalse("Since the bound variable z does not exist and should not be replaced");
        }

        [Fact]
        public void IsReplaced_AfterNotReplacingExistingBoundVariable_ShouldReturnFalse()
        {
            // Arrange
            List<char> variables = new List<char>()
            {
                'p',
                'x',
                'j'
            };

            Predicate predicate = new Predicate(PREDICATE_SYMBOL, variables);

            // Act
            bool isReplaced = predicate.IsReplaced('x');

            // Assert
            isReplaced.Should().BeFalse("Since the bound variable x was not replaced");
        }
    }
}
