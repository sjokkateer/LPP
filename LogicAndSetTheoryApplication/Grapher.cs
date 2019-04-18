using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace LogicAndSetTheoryApplication
{
    static class Grapher
    {
        public static void CreateGraphOfFunction(Proposition propositionRoot, string fileName)
        {
            NumberOperands(propositionRoot);
            CreateGraph(fileName, propositionRoot.NodeLabel());
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

        private static void CreateGraph(string dotFileName, string fileContent)
        {
            CreateDotFile(dotFileName, fileContent);

            Process dot = new Process();
            dot.StartInfo.FileName = "dot.exe";
            dot.StartInfo.Arguments = $"-Tpng -o{dotFileName}.png {dotFileName}.dot";
            dot.Start();
            dot.WaitForExit();
        }

        private static void CreateDotFile(string fileName, string fileContent)
        {
            FileStream fs = new FileStream($"{fileName}.dot", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            try
            {
                if (sw != null)
                {
                    sw.WriteLine("graph logic {");
                    sw.WriteLine("\tnode [ fontname = \"Arial\" ]");
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
