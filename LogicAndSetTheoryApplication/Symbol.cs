using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    class Symbol : IEquatable<Symbol>, IComparable<Symbol>
    {
        public int NodeNumber { get; set; }

        public object Data { get; }

        public Symbol(object data)
        {
            Data = data;
        }

        public virtual List<Symbol> GetVariables()
        {
            return new List<Symbol>() { this };
        }

        public override string ToString()
        {
            return Data.ToString();
        }

        public virtual string NodeLabel()
        {
            return $"\tnode{NodeNumber} [ label = \"{Data}\" ]\n";
        }

        public bool Equals(Symbol other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (GetType() != other.GetType())
            {
                return false;
            }

            return (char)Data == (char)other.Data;
        }

        public static bool operator== (Symbol s1, Symbol s2)
        {
            if (ReferenceEquals(s1, null))
            {
                if (ReferenceEquals(s2, null))
                {
                    return true;
                }
            }
            return s1.Equals(s2);
        }

        public static bool operator !=(Symbol s1, Symbol s2)
        {
            return !(s1 == s2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// Returns the hashcode of Data (Data is of character type)
        /// </returns>
        public override int GetHashCode()
        {
            return Data.GetHashCode();
        }

        public int CompareTo(Symbol other)
        {
            char ownVariable = (char)Data;
            char otherVariable = (char)other.Data;

            return ownVariable.CompareTo(otherVariable);
        }
    }
}