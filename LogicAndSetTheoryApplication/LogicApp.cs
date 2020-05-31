using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    public class LogicApp: LogicAppBase
    {
        private const int HASH_CODE_BASE = 16;

        private List<Conjunction> missingVariableList;
        private HashCodeCalculator hashCodeCalculator;
        
        private List<string> hashCodes;
        public static readonly List<string> hashCodeLabels = new List<string>()
        { 
            "Original",
            "Simplified",
            "Nand Simplified",
            "Original DNF",
            "Original Simplified DNF",
            "Original Simplified DNF Nand"
        };
        public List<string> HashCodes
        {
            get
            {
                // return a copy of the original list.
                return new List<string>(hashCodes);
            }
        }

        public List<Proposition> Variables
        {
            get
            {
                if (Root == null)
                {
                    throw new NullReferenceException("Need a proposition expression first to obtain variables!");
                }

                return Root.GetVariables();
            }
        }
        public TruthTable TruthTable { get; private set; }
        public Proposition DisjunctiveNormalForm { get; private set; }
        public TruthTable SimplifiedTruthTable { get; private set; }
        public Proposition SimplifiedDisjunctiveNormalForm { get; private set; }
        public Proposition Nandified { get; private set; }

        public LogicApp(Parser parser): base(parser)
        {
            hashCodeCalculator = new HashCodeCalculator(HASH_CODE_BASE);
        }

        protected override void ExecuteParsingActivities()
        { 
            // New list of all the required hashcodes.
            hashCodes = new List<string>();

            // 1
            TruthTable = new TruthTable(Root);
            hashCodeCalculator.GenerateHashCode(TruthTable.GetConvertedResultColumn());
            hashCodes.Add(hashCodeCalculator.HashCode);

            // 2
            SimplifiedTruthTable = CreateSimplifiedTruthTable();
            
            // Convert the table to DNF such that we have a proposition to work with.
            Proposition simplified = SimplifiedTruthTable.CreateDisjunctiveNormalForm();
            TruthTable simplifiedTt = new TruthTable(simplified);
            // Check the hash code for the proposition.
            hashCodeCalculator.GenerateHashCode(simplifiedTt.GetConvertedResultColumn());
            hashCodes.Add(hashCodeCalculator.HashCode);

            // 3 
            Nandified = Root.Nandify();
            TruthTable nandifiedTruthTable = new TruthTable(Nandified);
            TruthTable nandifiedSimplified = nandifiedTruthTable.Simplify();
            
            // Same as for simplified truth table.
            Proposition nandSimpleDnf = nandifiedSimplified.CreateDisjunctiveNormalForm();
            TruthTable nandSimpleDnfTt = new TruthTable(nandSimpleDnf);

            hashCodeCalculator.GenerateHashCode(nandSimpleDnfTt.GetConvertedResultColumn());
            hashCodes.Add(hashCodeCalculator.HashCode);

            // 4 
            DisjunctiveNormalForm = CreateDisjunctiveNormalForm();
            TruthTable disjunctiveTruthTable = new TruthTable(DisjunctiveNormalForm);
            hashCodeCalculator.GenerateHashCode(disjunctiveTruthTable.GetConvertedResultColumn());
            hashCodes.Add(hashCodeCalculator.HashCode);

            // 5
            Proposition simplifiedDisjunctiveNormal = CreateSimplifiedDisjunctiveNormalForm();
            TruthTable simplifiedDisjunctiveNormalTruthTable = new TruthTable(simplifiedDisjunctiveNormal);
            hashCodeCalculator.GenerateHashCode(simplifiedDisjunctiveNormalTruthTable.GetConvertedResultColumn());
            hashCodes.Add(hashCodeCalculator.HashCode);

            // 6
            Proposition nandifiedSimplifiedDisjunctiveNormal = simplifiedDisjunctiveNormal.Nandify();
            TruthTable nandifiedSimplifiedDisjunctiveNormalTruthTable = new TruthTable(nandifiedSimplifiedDisjunctiveNormal);
            hashCodeCalculator.GenerateHashCode(nandifiedSimplifiedDisjunctiveNormalTruthTable.GetConvertedResultColumn());
            hashCodes.Add(hashCodeCalculator.HashCode);
        }

        private Proposition CreateDisjunctiveNormalForm()
        {
            Proposition disjunctiveProposition = TruthTable.CreateDisjunctiveNormalForm();
            
            if (disjunctiveProposition == null)
            {
                // This should mean we only have 1 result value in the result column, a 0 for DNF
                // Create a contradiction
                Proposition tautologyOrContradiction = PropositionGenerator.CreateContradictionFromProposition(Variables[0]);
                tautologyOrContradiction.UniqueVariableSet = Variables;
                disjunctiveProposition = tautologyOrContradiction;
            }

            return disjunctiveProposition;
        }

        private TruthTable CreateSimplifiedTruthTable()
        {
            return TruthTable.Simplify();
        }

        private Proposition CreateSimplifiedDisjunctiveNormalForm()
        {
            missingVariableList = new List<Conjunction>();
            Proposition simplifiedDisjunctiveProposition = SimplifiedTruthTable.CreateDisjunctiveNormalForm();

            // If the returned proposition is not null, we check if we are not missing any variables that got 
            // left out by us simplifying a proposition.
            if (simplifiedDisjunctiveProposition != null)
            {
                // Create truth table of the simplified disjunctive
                // Create a new hash calculator with the result column, base 16.
                // Add hash and bcde to the list boxes.
                List<Proposition> simplifiedDisjunctiveUniqueVariables = simplifiedDisjunctiveProposition.GetVariables();
                int addedVariables = 0;
                while (simplifiedDisjunctiveUniqueVariables.Count + addedVariables < Variables.Count)
                {
                    // Create an extra placeholder variable on the proposition to fill up the left out variable spot.
                    // Important that the variabel IS unique, such that it adds to the number of combinations of truth values.
                    foreach (char alphabetCharacter in "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
                    {
                        foreach (Proposition uniqueVariable in Variables)
                        {
                            if (alphabetCharacter == uniqueVariable.Data)
                            {
                                break;
                            }
                        }
                        // Exhausted without breaking out of the loop thus we are looking at a unique variable.
                        Proposition newVariable = new Proposition(alphabetCharacter);
                        // Since this # of variables resulted in no changes in the outcome of the result
                        // it should not matter too much on what binary operator is applied.
                        Conjunction conjunction = new Conjunction();
                        
                        Disjunction tautology = (Disjunction)PropositionGenerator.CreateTautologyFromProposition(newVariable);
                        conjunction.LeftSuccessor = tautology;
                        missingVariableList.Add(conjunction);

                        // So if we add a tautology with the original expression, we take the old variable into consideration
                        // in the truth table but this variable will not change the result values of the truth table but will
                        // ensure that our hash codes will line up with the same number of bits.
                        addedVariables++;
                        break;
                    }
                }
                // Now we have all missing variables in the list of missing variables.
                // Thus we can add to the right successor of each index, the previous or the root expression.
                int lastIndex = missingVariableList.Count - 1;
                Proposition propositionToAppend = null;
                for (int i = lastIndex; i >= 0; i--)
                {
                    if (i == lastIndex)
                    {
                        propositionToAppend = simplifiedDisjunctiveProposition;
                    }

                    missingVariableList[i].RightSuccessor = propositionToAppend;
                    propositionToAppend = missingVariableList[i];
                }               
            }
            else
            {
                // Otherwise we are dealing with a tautology or contradiction.
                int tautOrContra = SimplifiedTruthTable.GetConvertedResultColumn()[0];
                Proposition tautologyOrContradiction = null;
                
                if (tautOrContra == 1)
                {
                    // Create a tautology but it should hold the same variable set as the original proposition.
                    tautologyOrContradiction = PropositionGenerator.CreateTautologyFromProposition(Variables[0]);
                }
                else
                {
                    // Create a contradiction
                    tautologyOrContradiction = PropositionGenerator.CreateContradictionFromProposition(Variables[0]);
                }

                tautologyOrContradiction.UniqueVariableSet = Variables;
                simplifiedDisjunctiveProposition = tautologyOrContradiction;
            }

            return simplifiedDisjunctiveProposition;
        }
        
        public bool HashCodesMatched()
        {
            return HashCodesMatched(HashCodes, HashCodes.Count - 1);
        }
        private bool HashCodesMatched(List<string> hashList, int index)
        {
            if (index == 0)
            {
                return true;
            }
            else
            {
                // Compare the two adjacent hashcodes and make a call downwards.
                return hashList[index] == hashList[index - 1] && HashCodesMatched(hashList, index - 1);
            }
        }

        protected override string DotFileName()
        {
            return "Proposition";
        }

        public override void CreateGraphImage()
        {
            Grapher.CreateGraphOfProposition(Root.Copy(), DotFileName());
            base.CreateGraphImage();
        }
    }
}
