using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace LogicAndSetTheoryApplication
{
    public static class Grapher
    {
        public static void CreateGraphOfProposition(Proposition propositionRoot, string fileName)
        {
            if (propositionRoot == null)
            {
                throw new ArgumentNullException("Proposition root cannot be null!");
            }

            NumberOperands(propositionRoot);
            CreateGraph(fileName, propositionRoot.NodeLabel());
        }

        public static void CreateGraphOfTableaux(SemanticTableaux semanticTableaux, string fileName)
        {
            if (semanticTableaux == null)
            {
                throw new ArgumentNullException("Semantic tableaux cannot be null!");
            }

            NumberElements(semanticTableaux);
            CreateGraph(fileName, semanticTableaux.Head.NodeLabel(), "rectangle");
        }

        private static void NumberElements(SemanticTableaux semanticTableaux)
        {
            List<SemanticTableauxElement> queue = new List<SemanticTableauxElement>();

            queue.Add(semanticTableaux.Head);
            int nodeCounter = 1;
            SemanticTableauxElement currentElement = null;

            while (queue.Count > 0)
            {
                currentElement = queue[0];
                queue.RemoveAt(0);

                currentElement.NodeNumber = nodeCounter;
                
                if (currentElement.LeftChild != null)
                {
                    queue.Add(currentElement.LeftChild);
                }

                if (currentElement.RightChild != null)
                {
                    queue.Add(currentElement.RightChild);
                }

                nodeCounter++;
            }
        }

        private static void NumberOperands(Proposition expressionRoot)
        {
            List<Proposition> queue = new List<Proposition>();
            Proposition currentSymbol = expressionRoot;
            queue.Add(currentSymbol);

            int nodeCounter = 1;
            while (currentSymbol != null)
            {
                currentSymbol.NodeNumber = nodeCounter;

                if (currentSymbol is BinaryConnective)
                {
                    queue.Add(((BinaryConnective)currentSymbol).LeftSuccessor);
                    queue.Add(((BinaryConnective)currentSymbol).RightSuccessor);
                }
                else if (currentSymbol is UnaryConnective)
                {
                    queue.Add(((UnaryConnective)currentSymbol).LeftSuccessor);
                }

                queue.RemoveAt(0);
                if (queue.Count > 0)
                {
                    currentSymbol = queue[0];
                }
                else
                {
                    currentSymbol = null;
                }
                nodeCounter++;
            }
        }

        private static void CreateGraph(string dotFileName, string fileContent, string shape = "")
        {
            CreateDotFile(dotFileName, fileContent, shape);

            Process dot = new Process();
            dot.StartInfo.FileName = "dot.exe";
            dot.StartInfo.Arguments = $"-Tpng -o{dotFileName}.png {dotFileName}.dot";
            dot.Start();
            dot.WaitForExit();
        }

        private static void CreateDotFile(string fileName, string fileContent, string shape)
        {
            FileStream fs = new FileStream($"{fileName}.dot", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            try
            {
                if (sw != null)
                {
                    sw.WriteLine("graph logic {");
                    
                    if (shape == string.Empty)
                    { 
                        sw.WriteLine("\tnode [ fontname = \"Arial\" ]");
                    }
                    else
                    {
                        sw.WriteLine($"\tnode [ fontname = \"Arial\", shape={shape}]");
                    }

                    sw.Write(fileContent);
                    sw.WriteLine("}");
                    sw.Close();
                }
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
        }
    }
}
