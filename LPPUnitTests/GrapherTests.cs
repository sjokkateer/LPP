using System;
using System.Collections.Generic;
using System.Text;
using LogicAndSetTheoryApplication;
using Xunit;
using FluentAssertions;
using System.IO;

namespace LPPUnitTests
{
    public class GrapherTests
    {
        [Fact]
        public void CreateGraphOfProposition_NullGivenForPropositionRoot_ExpectedArgumentNullException()
        {
            // Arrange
            Proposition proposition = null;
            Action act = () => Grapher.CreateGraphOfProposition(proposition, "");

            // Act // Assert
            act.Should().Throw<ArgumentNullException>("Because it can not operate without an object of proposition");
        }
    
        [Fact]
        public void CreateGraphOfTableaux_NullGivenForSemanticTableaux_ExpectedArgumentNullException()
        {
            // Arrange
            SemanticTableaux semanticTableaux = null;
            Action act = () => Grapher.CreateGraphOfTableaux(semanticTableaux, "");

            // Act // Assert
            act.Should().Throw<ArgumentNullException>("Because it can not operate without an object of semantic tableaux");
        }

        [Fact]
        public void CreateGraphOfProposition_SmallPropositionTreeGiven_ExpectedAllPropositionsToBeNumberedAccordingToBFS()
        {
            // Arrange
            BinaryConnective root = new Conjunction();

            BinaryConnective disjunction = new Disjunction();
            root.LeftSuccessor = disjunction;
            disjunction.LeftSuccessor = new Proposition('P');
            disjunction.RightSuccessor = new Proposition('Q');

            root.RightSuccessor = new Proposition('X');

            string fileName = "test_case_proposition";

            // Act
            Grapher.CreateGraphOfProposition(root, fileName);

            // Assert
            root.NodeNumber.Should().Be(1, "Because it's the first to be labeled");
            disjunction.NodeNumber.Should().Be(2, "Because it's the second to be labeled");
            root.RightSuccessor.NodeNumber.Should().Be(3, "Because it's the third to be labeled");
            disjunction.LeftSuccessor.NodeNumber.Should().Be(4, "Because it's the fourth to be labeled");
            disjunction.RightSuccessor.NodeNumber.Should().Be(5, "Because it's the fifth to be labeled");

            File.Exists($"{fileName}.dot").Should().BeTrue("Because a dot file should be created (graphviz related though).");
            File.Exists($"{fileName}.png").Should().BeTrue("Because a png image should be created from the dot file (graphviz related though).");
        }

        [Fact]
        public void CreateGraphOfTableaux_SmallTableauxTreeGiven_ExpectedAllTableauxElementsToBeNumberedAccordingToBFS()
        {
            // Arrange
            BinaryConnective root = new Conjunction();

            BinaryConnective disjunction = new Disjunction();
            root.LeftSuccessor = disjunction;
            disjunction.LeftSuccessor = new Proposition('P');
            disjunction.RightSuccessor = new Proposition('Q');

            root.RightSuccessor = new Proposition('X');

            SemanticTableaux semanticTableaux = new SemanticTableaux(root);

            string fileName = "test_case_tableaux";

            // Act
            Grapher.CreateGraphOfTableaux(semanticTableaux, fileName);

            // Assert
            semanticTableaux.Head.NodeNumber.Should().Be(1, "Because it's the first to be labeled");
            // Since beta rule
            semanticTableaux.Head.LeftChild.NodeNumber.Should().Be(2, "Because it's the second to be labeled");
            semanticTableaux.Head.RightChild.NodeNumber.Should().Be(3, "Because it's the third to be labeled");
            // Since alpha rule
            semanticTableaux.Head.LeftChild.LeftChild.NodeNumber.Should().Be(4, "Because it's the third to be labeled");

            File.Exists($"{fileName}.dot").Should().BeTrue("Because a dot file should be created (graphviz related though).");
            File.Exists($"{fileName}.png").Should().BeTrue("Because a png image should be created from the dot file (graphviz related though).");
        }
    }
}
