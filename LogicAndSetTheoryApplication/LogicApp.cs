using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    public class LogicApp: LogicAppBase
    {
        private List<Conjunction> missingVariableList;

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
        { }

        protected override void ExecuteParsingActivities()
        { 
            TruthTable = new TruthTable(Root);
            DisjunctiveNormalForm = CreateDisjunctiveNormalForm();
            SimplifiedTruthTable = CreateSimplifiedTruthTable();

            // TODO: Add all other additional activities
           // SimplifiedDisjunctiveNormalForm = CreateSimplifiedDisjunctiveNormalForm();

            // Regular NANDIFY
            //Nandified = CreateNandified();

            // NANDIFY 5 and eval to get 6
            //Proposition nandifiedFive = simplifiedDisjunctiveProposition.Nandify();
        }

        //private Proposition CreateNandified()
        //{
        //    // Need original nandified -> simplified
        //    Proposition nandified = Root.Nandify();
        //    if (nandified != null)
        //    {
        //        nandifiedTbx.Text = nandified.ToString();
        //        TruthTable tt = new TruthTable(nandified);
        //        AddHashCodeInfo(nandified, "NAND", 16, tt); // ORIGINAL + NAND + EVALUATE 

        //        TruthTable simplifiedNandTt = tt.Simplify();
        //        Proposition simplifiedNandDnf = simplifiedNandTt.CreateDisjunctiveNormalForm();
        //        // If it is null we deal with a tautology or contradiction!
        //        if (simplifiedNandDnf == null)
        //        {
        //            // This should mean we only have 1 result value in the result column, either a 1 or 0
        //            int tautOrContra = simplifiedNandTt.GetConvertedResultColumn()[0];
        //            Proposition tautologyOrContradiction = null;
        //            if (tautOrContra == 1)
        //            {
        //                // Create a tautology but it should hold the same variable set as the original proposition.
        //                tautologyOrContradiction = PropositionGenerator.CreateTautologyFromProposition(logicApp.Variables[0]);
        //            }
        //            else
        //            {
        //                // Create a contradiction
        //                tautologyOrContradiction = PropositionGenerator.CreateContradictionFromProposition(logicApp.Variables[0]);
        //            }
        //            tautologyOrContradiction.UniqueVariableSet = logicApp.Variables;
        //            AddHashCodeInfo(tautologyOrContradiction, "Simplified NAND Normal", 16);
        //        }
        //        else
        //        {

        //            if (missingVariableList.Count > 0)
        //            {
        //                missingVariableList[missingVariableList.Count - 1].RightSuccessor = simplifiedNandDnf;
        //                AddHashCodeInfo(missingVariableList[0], "Simplified NAND Normal", 16);
        //            }
        //            else
        //            {
        //                // Keeping the original the original.
        //                AddHashCodeInfo(simplifiedNandDnf, "Simplified NAND Normal", 16); // 3. ORIGINAL + NAND + EVALUATE + SIMPLIFY + NORMALIZE
        //            }
        //        }
        //    }
        //}

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
