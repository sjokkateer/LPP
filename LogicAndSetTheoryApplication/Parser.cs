using System.Collections.Generic;
using System.Linq;
using System;

namespace LogicAndSetTheoryApplication
{
    class Parser
    {
        private const string CONNECTIVES = "~>=&|";
        private const string VARIABLES = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private string proposition;

        private List<char> connectives;
        private List<Symbol> symbols;

        public Parser(string proposition)
        {
            this.proposition = proposition;

            connectives = new List<char>();
            symbols = new List<Symbol>();
        }

        public Symbol Parse()
        {
            ParseHelper(proposition);
            return symbols[0];
        }

        private void ParseHelper(string s)
        {
            if (s != string.Empty)
            {
                if (CONNECTIVES.Contains(s[0]))
                {
                    connectives.Add(s[0]);
                }
                else if (VARIABLES.Contains(s[0]))
                {
                    Symbol symbol = new Symbol(s[0]);
                    symbols.Add(symbol);
                }
                else if (s[0] == '(')
                {
                    connectives.Add('(');
                }
                else if (s[0] == ')')
                {
                    connectives.RemoveAt(connectives.Count - 1);
                    char currentOperator = connectives[connectives.Count - 1];
                    connectives.RemoveAt(connectives.Count - 1);
                    CreateConnective(currentOperator);
                }
                ParseHelper(s.Substring(1));
            }
        }

        private void CreateConnective(char connective)
        {
            Symbol result = null;
            switch (connective)
            {
                case '~':
                    result = new Negation();
                    break;
                case '>':
                    result = new Implication();
                    break;
                case '=':
                    result = new BiImplication();
                    break;
                case '&':
                    result = new Conjunction();
                    break;
                case '|':
                    result = new Disjunction();
                    break;
            }
            if (result is BinaryConnective)
            {
                ((BinaryConnective)result).LeftSuccessor = symbols[symbols.Count - 2];
                ((BinaryConnective)result).RightSuccessor = symbols[symbols.Count - 1];
                symbols.RemoveAt(symbols.Count - 1);
                symbols.RemoveAt(symbols.Count - 1);
            }
            else
            {
                ((UnaryConnective)result).LeftSuccessor = symbols[symbols.Count - 1];
                symbols.RemoveAt(symbols.Count - 1);
            }
            symbols.Add(result);
        }
    }
}
