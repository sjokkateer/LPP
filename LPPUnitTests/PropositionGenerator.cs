using System;
using System.Collections.Generic;
using System.Text;
using LogicAndSetTheoryApplication;

namespace LPPUnitTests
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
                default:
                    throw new ArgumentNullException("Could not convert symbol into a connective!");
            }

            binaryConnective.LeftSuccessor = GetRandomPropositionSymbol();
            binaryConnective.RightSuccessor = GetRandomPropositionSymbol();

            return binaryConnective;
        }

        public static char GetRandomVariableLetter()
        {
            rng = new Random();
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
    }
}
