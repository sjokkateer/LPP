using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using LogicAndSetTheoryApplication;
using FluentAssertions;

namespace LPPUnitTests
{
    abstract public class QuantifierTestsBase
    {
        [Fact]
        public void GetVariables_ExpectedNotImplementedExceptionThrown()
        {
            // Arrange
            Quantifier quantifier = GetQuantifier();
            Action act = () => quantifier.GetVariables();

            // Act // Assert
            act.Should().Throw<NotImplementedException>("Because proposition variables are not a part of this class.");
        }

        [Fact]
        public void Nandify_ExpectedNotImplementedExceptionThrown()
        {
            // Arrange
            Quantifier quantifier = GetQuantifier();
            Action act = () => quantifier.Nandify();

            // Act // Assert
            act.Should().Throw<NotImplementedException>("Because that does not apply to first order logic.");
        }

        [Fact]
        public void Calculate_ExpectedNotImplementedExceptionThrown()
        {
            // Arrange
            Quantifier quantifier = GetQuantifier();
            Action act = () => quantifier.Calculate();

            // Act // Assert
            act.Should().Throw<NotImplementedException>("Because this will not be implemented by us.");
        }

        abstract protected Quantifier GetQuantifier();
        abstract protected char GetBoundVariable();

        [Fact]
        public void Constructor_PassingNonAlphabeticCharacter_ExpectedArgumentExceptionThrown()
        {
            // Arrange
            Action act = () => new ExistentialQuantifier('-');

            // Act // Assert
            act.Should().Throw<ArgumentException>("Because bound variables should be lower cased alphabetical characters");
        }

        [Fact]
        public void Constructor_PassingAlphabeticCharacter_ExpectedVariableAssignedAndObjectConstructed()
        {
            // Arrange
            Quantifier quantifier = GetQuantifier();

            // Act
            char actualBoundVariable = quantifier.GetBoundVariable();

            // Assert
            actualBoundVariable.Should().Be(GetBoundVariable(), "Because the character should be assigned");
        }

        [Fact]
        public void ToString_WithoutLeftSuccessorSet_ExpectedArgumentNullExceptionThrown()
        {
            // Arrange
            Quantifier quantifier = GetQuantifier();
            Action act = () => quantifier.ToString();

            // Act // Assert
            act.Should().Throw<NullReferenceException>("Because it would not make sense to display a quantifier without it's related predicate");
        }

        [Fact]
        public void ToString_WithPredicateAsLeftSuccessor_ExpectedProperStringReturned()
        {
            // Arrange
            Quantifier quantifier = GetQuantifier();
            Predicate predicate = new Predicate('P', new List<char>() { GetBoundVariable() });
            quantifier.LeftSuccessor = predicate;

            // Act
            string actualString = quantifier.ToString();
            string expectedString = $"{quantifier.GetSymbol()}.{GetBoundVariable()}({predicate})";

            // Assert
            actualString.Should().Be(expectedString, "Because that's the format we expect");
        }

        [Fact]
        public void ToString_WithPropositionConnectiveAsLeftSuccessor_ExpectedProperStringReturned()
        {
            // Arrange
            Quantifier quantifier = GetQuantifier();
            Conjunction conjunction = new Conjunction();
            Predicate leftPredicate = new Predicate('P', new List<char>() { GetBoundVariable() });
            Predicate rightPredicate = new Predicate('Q', new List<char>() { GetBoundVariable() });

            quantifier.LeftSuccessor = conjunction;
            conjunction.LeftSuccessor = leftPredicate;
            conjunction.RightSuccessor = rightPredicate;

            // Act
            string actualString = quantifier.ToString();
            string expectedString = $"{quantifier.GetSymbol()}.{GetBoundVariable()}({conjunction})";

            // Assert
            actualString.Should().Be(expectedString, "Because that's the format we expect");
        }
    }
}
