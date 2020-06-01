using System;
using System.Collections.Generic;
using System.Text;

namespace LogicAndSetTheoryApplication
{
    public class PropositionGenerator
    {
        private static int MAX_UNIQUE_VARIABLES = 8;

        private static int CAPITAL_A = 65;
        private static int CAPITAL_Z = 90;

        private static Random rng;

        private HashSet<Proposition> generatedPropositionVariables;

        public PropositionGenerator()
        { }

        public static Proposition CreateTautologyFromProposition(Proposition variable)
        {
            if (variable == null)
            {
                throw new NullReferenceException("A proposition is required to create a tautology from it!");
            }

            Disjunction tautology = new Disjunction();
            tautology.LeftSuccessor = variable; // A
            Negation negatedVariable = new Negation();
            negatedVariable.LeftSuccessor = variable;
            tautology.RightSuccessor = negatedVariable; // A | ~(A) == 1
            
            return tautology;
        }

        public static Proposition CreateContradictionFromProposition(Proposition variable)
        {
            if (variable == null)
            {
                throw new NullReferenceException("A proposition is required to create a contradiction from it!");
            }

            Negation negation = new Negation();
            negation.LeftSuccessor = CreateTautologyFromProposition(variable);
            
            return negation;
        }

        public static UnaryConnective CreateUnaryConnectiveWithRandomSymbol(char unaryConnectiveSymbol)
        {
            UnaryConnective unaryConnective = null;

            switch(unaryConnectiveSymbol)
            {
                case Negation.SYMBOL:
                    unaryConnective = new Negation();
                    break;
                default:
                    throw new ArgumentNullException("Could not convert symbol into a connective!");
            }

            unaryConnective.LeftSuccessor = GetRandomProposition();
            
            return unaryConnective;
        }

        public static BinaryConnective CreateBinaryConnectiveWithRandomSymbols(char binaryConnectiveSymbol)
        {
            BinaryConnective binaryConnective;

            switch(binaryConnectiveSymbol)
            {
                case Conjunction.SYMBOL:
                    binaryConnective = new Conjunction();
                    break;
                case BiImplication.SYMBOL:
                    binaryConnective = new BiImplication();
                    break;
                case Disjunction.SYMBOL:
                    binaryConnective = new Disjunction();
                    break;
                case Implication.SYMBOL:
                    binaryConnective = new Implication();
                    break;
                case Nand.SYMBOL:
                    binaryConnective = new Nand();
                    break;
                default:
                    throw new ArgumentNullException("Could not convert symbol into a connective!");
            }

            binaryConnective.LeftSuccessor = GetRandomPropositionSymbol();
            binaryConnective.RightSuccessor = GetRandomPropositionSymbol();

            return binaryConnective;
        }

        public static char GetRandomVariableLetter()
        {
            if (rng == null)
            {
                rng = new Random();
            }

            int randomCapitalLetter = rng.Next(CAPITAL_A, CAPITAL_Z + 1);

            return (char)randomCapitalLetter;
        }

        public static Proposition GetRandomPropositionSymbol()
        {
            return new Proposition(GetRandomVariableLetter());
        }

        public static char GetRandomConnectiveSymbol()
        {
            rng = new Random();
            int randomIndexOfConnectivesString = rng.Next(0, Parser.CONNECTIVES.Length);

            return Parser.CONNECTIVES[randomIndexOfConnectivesString];
        }

        public static Proposition GetRandomProposition()
        {
            return new Proposition(GetRandomVariableLetter());
        }

        public Proposition GenerateExpression()
        {
            if (rng == null)
            {
                rng = new Random();
            }

            generatedPropositionVariables = new HashSet<Proposition>();
            int numberOfVariables = rng.Next(4, MAX_UNIQUE_VARIABLES + 1);

            while (generatedPropositionVariables.Count < numberOfVariables)
            {
                generatedPropositionVariables.Add(GetRandomProposition());
            }

            int startLevel = 0;
            Proposition root = GenerateProposition(startLevel);
            return GenerateExpressionRecursively(root, startLevel + 1);
        }

        private Proposition GenerateProposition(int currentLevel)
        {
            int choice = rng.Next(7 + currentLevel);

            return GeneratePropositionByRandomChoice(choice);
        }

        public Proposition GeneratePropositionByRandomChoice(int choice)
        {
            switch (choice)
            {
                case 0:
                    return new Negation();
                case 1:
                    return new Disjunction();
                case 2:
                    return new Conjunction();
                case 3:
                    return new Implication();
                case 4:
                    return new BiImplication();
                case 5:
                    return new Nand();
                case 6:
                    // throw a coin for True of False constant.
                    return GenerateRandomConstant(rng.Next(2));
                default:
                    return GetPropositionFromSet();
            }
        }

        private Proposition GetPropositionFromSet()
        {
            int index = rng.Next(generatedPropositionVariables.Count);
            
            foreach (Proposition p in generatedPropositionVariables)
            {
                if (index == 0)
                {
                    return p;
                }

                index--;
            }

            throw new Exception("Impossible to reach code reached");
        }

        public static Proposition GenerateRandomConstant(int coinToss)
        {
            if (coinToss == 0)
            {
                return new True();
            }

            return new False();
        }

        private Proposition GenerateExpressionRecursively(Proposition result, int level)
        {
            if (!(result is UnaryConnective))
            {
                return result;
            }

            int newLevel = level + 1;

            if (level >= 5)
            {
                newLevel = level + 4;
            }

            if (result is UnaryConnective)
            {
                // Generate a random left
                UnaryConnective connective = (UnaryConnective)result;
                connective.LeftSuccessor = GenerateProposition(level);

                GenerateExpressionRecursively(connective.LeftSuccessor, newLevel);
            }

            if (result is BinaryConnective)
            {
                // Generate a random right
                BinaryConnective connective = (BinaryConnective)result;
                connective.RightSuccessor = GenerateProposition(level);

                GenerateExpressionRecursively(connective.RightSuccessor, newLevel);
            }

            return result;
        }
    }
}
