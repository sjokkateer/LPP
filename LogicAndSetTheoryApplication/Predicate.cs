using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    public class Predicate: Proposition
    {
        public static string LOWER_CASE_ALPHABET = "abcdefghijklmnopqrstuvwxyz";

        private Dictionary<char, char> variables;
        // So this will take a variable letter as symbol P, Q, R whatsoever
        public Predicate(char symbol, List<char> variables): base(symbol)
        {
            if (variables == null || variables.Count == 0)
            {
                throw new ArgumentException("This class relies on variables and needs at least 1 in the list!");
            }

            this.variables = new Dictionary<char, char>();

            foreach (char variable in variables)
            {
                if (!this.variables.ContainsKey(variable))
                {
                    this.variables.Add(variable, default);
                }
            }
        }

        public override List<Proposition> GetVariables()
        {
            throw new NotImplementedException("Is not part of first order logic");
        }

        public override List<char> GetBoundVariables()
        {
            List<char> variables = new List<char>();
            
            foreach (KeyValuePair<char, char> item in this.variables)
            {
                variables.Add(item.Key);
            }

            return variables;
        }

        public override bool Replace(char originalVariable, char newVariable)
        {
            bool replaced = false;

            // The new variable should not be present, neither as key and also not as value.
            if (!variables.ContainsKey(newVariable) && !variables.ContainsValue(newVariable))
            {
                if (variables.ContainsKey(originalVariable) && variables[originalVariable] == default)
                {
                    variables[originalVariable] = newVariable;
                    replaced = true;
                }
            }

            return replaced;
        }

        public bool IsReplaced(char boundVariable)
        {
            if (variables.ContainsKey(boundVariable))
            {
                return variables[boundVariable] != default;
            }

            return false;
        }

        // Should override ToString otherwise only the predicate variable
        // is printed
        public override string ToString()
        {
            string result = base.ToString();
            result += "(";

            foreach (KeyValuePair<char, char> item in variables)
            {
                result += GetVariable(item.Key);
                result += ", ";
            }

            // Ommit trailing comma
            result = result.Substring(0, result.Length - 2);
            result += ")";

            return result;
        }

        private char GetVariable(char key)
        {
            return variables[key] != default ? variables[key] : key;
        }

        public override bool IsAbstractProposition()
        {
            return false;
        }
    }
}
