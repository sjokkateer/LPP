using System;
using System.Collections.Generic;
using System.Text;

namespace LogicAndSetTheoryApplication
{
    public class PropositionGenerator
    {
        private static int CAPITAL_A = 65;
        private static int CAPITAL_Z = 90;

        private static Random rng;

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

        public static char GetRandomConnective()
        {
            rng = new Random();
            int randomIndexOfConnectivesString = rng.Next(0, Parser.CONNECTIVES.Length);

            return Parser.CONNECTIVES[randomIndexOfConnectivesString];
        }

        public static Proposition GetRandomProposition()
        {
            return new Proposition(GetRandomVariableLetter());
        }

        public static Proposition GenerateExpression()
        {
            if (rng == null)
            {
                rng = new Random();
            }

            int startLevel = 0;
            Proposition root = GenerateProposition(startLevel);
            return GenerateExpressionRecursively(root, startLevel + 1);
        }

        public static Proposition GenerateProposition(int currentLevel)
        {
            Proposition generatedProposition = null;
            int choice = rng.Next(7 + currentLevel);

            switch(choice)
            {
                case 0:
                    generatedProposition = new Negation();
                    break;
                case 1:
                    generatedProposition = new Disjunction();
                    break;
                case 2:
                    generatedProposition = new Conjunction();
                    break;
                case 3:
                    generatedProposition = new Implication();
                    break;
                case 4:
                    generatedProposition = new BiImplication();
                    break;
                case 5:
                    generatedProposition = new Nand();
                    break;
                case 6:
                    // throw a coin for True of False constant.
                    if (rng.Next(2) == 0)
                    {
                        generatedProposition = new True();
                    }
                    else
                    {
                        generatedProposition = new False();
                    }

                    break;
                default:
                    generatedProposition = GetRandomProposition();
                    break;
            }

            return generatedProposition;
        }

        private static Proposition GenerateExpressionRecursively(Proposition result, int level)
        {
            if (!(result is UnaryConnective))
            {
                return result;
            }

            int newLevel = level + 1;

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
