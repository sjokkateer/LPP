using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    class BinaryConnective : UnaryConnective
    {
        public Proposition RightSuccessor { get; set; }

        public BinaryConnective(object data) : base(data)
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
            
            // This looks daunting but it just is not, since it's documented. Right?
            return leftChildVariables.Except(rightChildVariables). // Set difference gets applied { A } \ { B }
                    Union(rightChildVariables.Except(leftChildVariables)). // { B } \ { A }
                        Union(leftChildVariables.Intersect(rightChildVariables)).ToList(); // { A } Intersection { B }, resulting in a set containing all unique elements (unordered though)
        }

        public override string ToString()
        {
            string result = "(";
            result += LeftSuccessor;
            result += $" {Data} ";
            result += RightSuccessor;
            result += ")";
            return result;
        }

        public override string NodeLabel()
        {
            string result = base.NodeLabel();
            result += $"\tnode{NodeNumber} -- node{RightSuccessor.NodeNumber}\n";
            result += RightSuccessor.NodeLabel();
            return result;
        }
    }
}
