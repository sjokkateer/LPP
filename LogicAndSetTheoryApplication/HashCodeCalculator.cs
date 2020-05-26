using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    public class HashCodeCalculator
    {
        private int hashBase;
        private List<int> convertedResultColumn;
        public int HashBase
        {
            get { return hashBase; }
            set
            {
                if (value != 0 && value % 2 == 0)
                {
                    hashBase = value;
                }
                else
                {
                    throw new Exception("The hash base must be a power of 2.");
                }
            }
        }
        public string HashCode { get; }

        public HashCodeCalculator(List<int> convertedResultColumn, int hashBase)
        {
            this.convertedResultColumn = convertedResultColumn;
            HashBase = hashBase;
            HashCode = RecursiveHashCode(1);
        }

        private string RecursiveHashCode(int index)
        {
            int sum = 0;
            // Continue off from index, initially index will be 0.
            for (int i = index; i <= convertedResultColumn.Count; i++)
            {
                // One index earlier since we start counting from 1.
                sum += convertedResultColumn[i - 1] * Convert.ToInt32(Math.Pow(2, (i - 1) % Math.Log(HashBase, 2)));
                // For example if the index = 1 and the number of columns = 1. we can determine the character and return.
                if (i == convertedResultColumn.Count)
                {
                    // Last binary digit, we can covert the current sum to a character and return the final char.
                    return DetermineCharacter(sum);
                }
                // If index i is a multiple of 2^Log(HashBase, 2)
                else if (i % Math.Log(HashBase, 2) == 0)
                {
                    // Go one level deeper to obtain the last character first and then return it to the stack.
                    // To concatenate all characters to obtain the final hash code.
                    // Determine the current character and append it.
                    return RecursiveHashCode(i + 1) + DetermineCharacter(sum);
                }
            }
            return null;
        }

        private string DetermineCharacter(int value)
        {
            switch (value)
            {
                case 10:
                    return "A";
                case 11:
                    return "B";
                case 12:
                    return "C";
                case 13:
                    return "D";
                case 14:
                    return "E";
                case 15:
                    return "F";
                default:
                    return Convert.ToString(value);
            }
        }

        public override string ToString()
        {
            string result = string.Empty;
            string line = string.Empty;
            for (int i = 0; i < convertedResultColumn.Count; i++)
            {
                line = $"row {i}: {convertedResultColumn[i]}\n";
                if (i == convertedResultColumn.Count - 1)
                {
                    line = $"row {i}: {convertedResultColumn[i]}";
                }
                result += line;
            }
            return result;
        }
    }
}
