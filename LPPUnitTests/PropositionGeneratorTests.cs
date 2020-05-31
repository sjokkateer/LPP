using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;
using LogicAndSetTheoryApplication;

namespace LPPUnitTests
{
    public class PropositionGeneratorTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CreateTautologyFromProposition_ValidPropositionGiven_ExpectedAllAssignmentsToResultInTrue(bool truthValue)
        {
            // Arrange
            Proposition randomProposition = PropositionGenerator.GetRandomProposition();
            Proposition tautology = PropositionGenerator.CreateTautologyFromProposition(randomProposition);
            tautology.TruthValue = truthValue;

            // Act
            bool actualTruthValue = tautology.Calculate();

            // Assert
            actualTruthValue.Should().BeTrue("Because a tautology should always evaluate to true");
        }

        [Fact]
        public void CreateTautologyFromProposition_NullGiven_ExpectedNullReferenceExceptionThrown()
        {
            // Arrange // Act
            Action act = () => PropositionGenerator.CreateTautologyFromProposition(null);

            // Assert
            act.Should().Throw<NullReferenceException>("Because create tautology requires a proposition");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CreateContradictionFromProposition_ValidPropositionGiven_ExpectedAllAssignmentsToResultInFalse(bool truthValue)
        {
            // Arrange
            Proposition randomProposition = PropositionGenerator.GetRandomProposition();
            Proposition contradiction = PropositionGenerator.CreateContradictionFromProposition(randomProposition);
            contradiction.TruthValue = truthValue;

            // Act
            bool actualTruthValue = contradiction.Calculate();

            // Assert
            actualTruthValue.Should().BeFalse("Because a contradiction should always evaluate to false");
        }

        [Fact]
        public void CreateContradictionFromProposition_NullGiven_ExpectedNullReferenceExceptionThrown()
        {
            // Arrange // Act
            Action act = () => PropositionGenerator.CreateContradictionFromProposition(null);

            // Assert
            act.Should().Throw<NullReferenceException>("Because create contradiction requires a proposition");
        }

        [Theory]
        [InlineData(Negation.SYMBOL, typeof(Negation))]
        public void CreateUnaryConnectiveWithRandomSymbols_ValidUnaryConnectiveSymbolGiven_ExpectedUnaryConnectiveReturned(char connectiveSymbol, Type type)
        {
            // Arrange // Act
            Proposition unaryConnective = PropositionGenerator.CreateUnaryConnectiveWithRandomSymbol(connectiveSymbol);

            // Assert
            unaryConnective.GetType().Should().Be(type, "Because that is the type we requested a unary connective from");
        }

        [Theory]
        [InlineData(null)]
        [InlineData('*')]
        public void CreateBinaryConnectiveWithRandomSymbols_NonExistingCreateBinaryConnectiveWithRandomSymbols_ExpectedArgumentNullExceptionThrown(char invalidConnectiveSymbol)
        {
            // Arrange // Act
            Action act = () => PropositionGenerator.CreateBinaryConnectiveWithRandomSymbols(invalidConnectiveSymbol);

            // Assert
            act.Should().Throw<ArgumentNullException>("Because an invalid binary connective symbol is given.");
        }

        [Theory]
        [InlineData(Conjunction.SYMBOL, typeof(Conjunction))]
        [InlineData(BiImplication.SYMBOL, typeof(BiImplication))]
        [InlineData(Disjunction.SYMBOL, typeof(Disjunction))]
        [InlineData(Implication.SYMBOL, typeof(Implication))]
        [InlineData(Nand.SYMBOL, typeof(Nand))]
        public void CreateBinaryConnectiveWithRandomSymbols_ValidBinaryConnectiveSymbolGiven_ExpectedBinaryConnectiveReturned(char connectiveSymbol, Type type)
        {
            // Arrange // Act
            Proposition binaryConnective = PropositionGenerator.CreateBinaryConnectiveWithRandomSymbols(connectiveSymbol);

            // Assert
            binaryConnective.GetType().Should().Be(type, "Because that is the type we requested a unary connective from");
        }

        [Theory]
        [InlineData(null)]
        [InlineData('*')]
        public void CreateUnaryConnectiveWithRandomSymbols_NonExistingUnaryConnectiveSymbolGiven_ExpectedArgumentNullExceptionThrown(char invalidConnectiveSymbol)
        {
            // Arrange // Act
            Action act = () => PropositionGenerator.CreateUnaryConnectiveWithRandomSymbol(invalidConnectiveSymbol);

            // Assert
            act.Should().Throw<ArgumentNullException>("Because an invalid unary connective symbol is given.");
        }

        [Fact]
        public void GetRandomVariableLetter_CallToGetRandomVariableLetter_ExpectedRandomCapitalLetterReturned()
        {
            // Arrange 
            string capitalAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            // Act
            char randomVariableLetter = PropositionGenerator.GetRandomVariableLetter();
            int indexOfVariableLetter = capitalAlphabet.IndexOf(randomVariableLetter);

            // Assert
            indexOfVariableLetter.Should().BeGreaterOrEqualTo(0, "Because a capital letter from the alphabet should be generated");
        }

        [Fact]
        public void GetRandomPropositionSymbol_CallToGetRandomPropositionSymbol_ExpectedRandomPropositionReturned()
        {
            // Arrange // Act
            Proposition randomProposition = PropositionGenerator.GetRandomPropositionSymbol();

            // Assert
            randomProposition.Should().BeOfType<Proposition>("Because a proposition with a random variable letter is generated");
        }

        [Fact]
        public void GetRandomConnectiveSymbol_GetRandomConnectiveSymbol_ExpectedRandomConnectiveSymbolReturned()
        {
            // Arrange // Act
            char randomConnectiveSymbol = PropositionGenerator.GetRandomConnectiveSymbol();
            int indexOfConnective = Parser.CONNECTIVES.IndexOf(randomConnectiveSymbol);

            // Assert
            indexOfConnective.Should().BeGreaterOrEqualTo(0, "Because a random connective symbol is generated");
        }

        [Theory]
        [InlineData(0, typeof(Negation))]
        [InlineData(1, typeof(Disjunction))]
        [InlineData(2, typeof(Conjunction))]
        [InlineData(3, typeof(Implication))]
        [InlineData(4, typeof(BiImplication))]
        [InlineData(5, typeof(Nand))]
        [InlineData(7, typeof(Proposition))]
        [InlineData(13, typeof(Proposition))]
        [InlineData(198, typeof(Proposition))]
        public void GeneratePropositionByRandomChoice_PositiveIntegerGiven_ExpectedCorrespondingObjectReturned(int choice, Type type)
        {
            // Arrange // Act
            Proposition generatedProposition = PropositionGenerator.GeneratePropositionByRandomChoice(choice);

            // Assert
            generatedProposition.GetType().Should().Be(type, "Because based on that generated integer a specific proposition should be returned");
        }

        [Fact]
        public void GeneratePropositionByRandomChoice_IntegerForConstantGiven_ExpectedConstantReturned()
        {
            // Arrange // Act
            int constantChoice = 6;
            Proposition generatedProposition = PropositionGenerator.GeneratePropositionByRandomChoice(constantChoice);

            // Assert
            (generatedProposition is Constant).Should().BeTrue("Because based on that integer one of the constants should be returned.");
        }

        [Theory]
        [InlineData(0, typeof(True))]
        [InlineData(1, typeof(False))]
        public void GenerateRandomConstant_IntegerForConstantGiven_ExpectedTrueOrFalseReturned(int coinFlip, Type type)
        {
            // Arrange // Act
            Proposition generatedProposition = PropositionGenerator.GenerateRandomConstant(coinFlip);

            // Assert
            generatedProposition.GetType().Should().Be(type, "Because based on that integer either True or False is returned.");
        }

        [Fact]
        public void GenerateExpression_RandomExpressionGeneratedAndPassedToLogicAppForParsing_ExpectedAllHashCodesToMatch()
        {
            // Arrange
            // re-use logic app for not repeating large procedure and for all the hash codes.
            LogicApp logicApp = new LogicApp(new Parser());

            // Act
            Proposition randomExpression = PropositionGenerator.GenerateExpression();
            logicApp.Parse(randomExpression);

            bool hashCodesMatched = logicApp.HashCodesMatched();

            // Assert
            hashCodesMatched.Should().BeTrue("Because a valid expression should be generated");
        }
    }
}
