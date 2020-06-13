using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    public class BinaryConnective : UnaryConnective
    {
        public Proposition RightSuccessor { get; set; }

        public BinaryConnective(char data) : base(data)
        {
            RightSuccessor = null;
        }

        /// <summary>
        /// This returns the result set of: (A\B) U (B\A) U (A Intersection B)
        /// 
        /// Example:
        /// 
        /// A = { A, B }
        /// B = { A, C }
        /// 
        /// A\B = { B } || The elements that are in A but not in B
        /// B\A = { C } || The elements that are in B but not in A
        /// A Intersection B = { A } || The elements that are in both A and B
        /// 
        /// result = { B, C, A }! All unique variables from two different sets.
        /// </summary>
        /// <returns>A list representing the set of all unique variables in the proposition formula.</returns>
        public override List<Proposition> GetVariables()
        {
            List<Proposition> leftChildVariables = LeftSuccessor.GetVariables();
            List<Proposition> rightChildVariables = RightSuccessor.GetVariables();
            
            return leftChildVariables.Except(rightChildVariables). // Set difference gets applied { A } \ { B }
                    Union(rightChildVariables.Except(leftChildVariables)). // { B } \ { A }
                        Union(leftChildVariables.Intersect(rightChildVariables)).ToList(); // { A } Intersection { B }, resulting in a set containing all unique elements (unordered though)
        }

        public override List<char> GetBoundVariables()
        {
            throw new NotImplementedException("It is unclear what it means to get bound variables with a binary connective.");
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj)) return false;
            if (GetType() != obj.GetType()) return false;

            BinaryConnective binaryConnective = (BinaryConnective)obj;
            return binaryConnective.RightSuccessor.Equals(RightSuccessor);
        }

        public override string ToString()
        {
            if (LeftSuccessor == null || RightSuccessor == null)
            {
                throw new NullReferenceException("Both left and right successor must not be null!");
            }

            string result = "(";
            result += LeftSuccessor;
            result += $" {Data} ";
            result += RightSuccessor;
            result += ")";
            return result;
        }

        public override string ToPrefixString()
        {
            if (LeftSuccessor == null || RightSuccessor == null)
            {
                throw new NullReferenceException("Both left and right successor must not be null!");
            }

            string result = $"{Data}(";
            result += LeftSuccessor.ToPrefixString();
            result += ", ";
            result += RightSuccessor.ToPrefixString();
            result += ")";
            return result;
        }

        [ExcludeFromCodeCoverage]
        public override string NodeLabel()
        {
            string result = base.NodeLabel();
            result += $"\tnode{NodeNumber} -- node{RightSuccessor.NodeNumber}\n";
            result += RightSuccessor.NodeLabel();
            return result;
        }

        public override bool Replace(char originalVariable, char newVariable)
        {
            bool replaced = base.Replace(originalVariable, newVariable);

            if (RightSuccessor.GetType() != typeof(Proposition))
            {
                replaced = RightSuccessor.Replace(originalVariable, newVariable);
            }

            return replaced;
        }

        public override bool IsAbstractProposition()
        {
            return base.IsAbstractProposition() && RightSuccessor.IsAbstractProposition();
        }
    }
}
