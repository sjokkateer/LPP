using System;
using System.Collections.Generic;
using System.Text;
using LogicAndSetTheoryApplication;
using FluentAssertions;

namespace LPPUnitTests
{
    public class NandChecker
    {
        public static void hasNandStructure(List<Proposition> queue)
        {
            // The root should always be nand for a nandified expressions.
            queue[0].Should().BeOfType<Nand>("because the root is always a nand");

            // BFS Traversal over the proposition tree
            Proposition current;

            do
            {
                current = pop(queue);

                if (current is BinaryConnective)
                {
                    // Assert non terminal node is a Nand connective.
                    current.Should().BeOfType<Nand>("because every connective is a Nand");
                    addChildren((BinaryConnective)current, queue);
                }
                else
                {
                    // Assert that terminal nodes are the proposition variable.
                    current.Should().BeOfType<Proposition>("because the terminal nodes should be regular proposition variables");
                }

            } while (queue.Count > 0);
        }

        private static Proposition pop(List<Proposition> queue)
        {
            int head = 0;
            Proposition firstSymbol = null;

            if (queue.Count > 0)
            {
                firstSymbol = queue[head];
                queue.RemoveAt(head);
            }

            return firstSymbol;
        }

        private static void addChildren(BinaryConnective binaryConnective, List<Proposition> queue)
        {
            Proposition leftSuccessor = binaryConnective.LeftSuccessor;
            Proposition rightSuccessor = binaryConnective.RightSuccessor;

            if (leftSuccessor != null)
            {
                queue.Add(leftSuccessor);
            }

            if (rightSuccessor != null)
            {
                queue.Add(rightSuccessor);
            }
        }
    }
}
