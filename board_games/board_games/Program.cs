/* MAIN IDEA
 * WARNING: There were many bugs caused by using any other data structure for the list of characters with frequencies than a C# List.
 * Get each individual character of a string. Find the frequencies of each character(map, set, list etc).
 * Sort them in ascending order.
 * Get from the left(so start from smallest frequency characters) elements two by two.
 * Create a root node that has as children the above nodes and the frequency the sum of the childre's frequencies, the data will be a character that excees 1 byte(never used in our case).
 * Do this until no nodes left in the List, queue or whatever you have.
 * Now, when traversing the tree on the left side we will append a 0 and on the right we will append a 1.
 * Example: acasa
 *             [(char)256, 5]
 *                |
 *                |----------------------['a', 3]
 *                |
 *             [(char)256, 2]
 *                |
 *                |-----------|
 *                |           |
 *             ['c', 1]   ['s', 1]
*/


using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;

namespace board_games
{
    public class Node
    {
        public Node? left, right;
        public char data;
        public int frequency;
        public Node(char data, int frequency)
        {
            left = right = null;
            this.data = data;
            this.frequency = frequency;
        }
    }

    public class ImplementComparator : IComparer<Node>
    {
        public int Compare(Node x, Node y)
        {
            int frequencyComparison = x.frequency.CompareTo(y.frequency);
            //if (frequencyComparison != 0)
            //{
            return frequencyComparison;
            //}
            //else
            //{
            //    return x.data.CompareTo(y.data);
            //}
        }
    }

    class Huffman
    {
        public void PrintCode(Node root, string s)
        {
            if (root == null) return;
            if (root.data != (char)256)
            {
                Console.WriteLine((int)root.data + ": " + s);
            }
            PrintCode(root.left!, s + "0");
            PrintCode(root.right!, s + "1");
        }
        public Dictionary<char, int> CalculateFrequenciesAndOrderThemInAscendingOrder(string text)
        {
            var frequencies = new Dictionary<char, int>();
            foreach (char character in text)
            {
                if (!frequencies.ContainsKey(character))
                    frequencies[character] = 0;
                frequencies[character]++;
            }

            var ordered = frequencies.OrderBy(f => f.Value).ToDictionary(f => f.Key, f => f.Value);

            return ordered;
        }

        public Node BuildHuffmanTree(Dictionary<char, int> ordered)
        {
            Node left, right, top;
            var priorityQueue = new List<Node>();

            foreach (var c in ordered)
            {
                //Console.WriteLine(c.Key + " : " + c.Value);
                priorityQueue.Add(new Node(c.Key, c.Value));
            }

            Console.WriteLine("Priority Queue:");
            foreach (var c in priorityQueue)
            {
                Console.WriteLine(c.data + "(" + (int)c.data + ")" + "; " + c.frequency);
            }
            Console.Write('\n');
            //int iteration = 0;
            while (priorityQueue.Count != 1)
            {
                /*Console.WriteLine("Iteration: " + iteration);
                foreach (var c in priorityQueue)
                {
                    Console.WriteLine(c.data + ": " + c.frequency);
                }*/

                left = priorityQueue.First();
                priorityQueue.Remove(left);

                right = priorityQueue.First();
                priorityQueue.Remove(right);

                int counter = 256;
                char val = (char)counter;
                top = new Node(val, left.frequency + right.frequency);

                //Console.WriteLine(left.data);
                //Console.WriteLine(right.data);

                top.left = left;
                top.right = right;

                priorityQueue.Add(top);
                priorityQueue.Sort(new ImplementComparator());
                //counter++;
                //iteration++;
            }

            Console.WriteLine(" Char | Huffman code ");
            Console.WriteLine("--------------------");
            PrintCode(priorityQueue.First(), "");
            return priorityQueue.First();
        }
        public string DecodeHuffmanText(string encodedText, Node root)
        {
            Node current = root;
            string decodedText = "";

            foreach (char bit in encodedText)
            {
                if (bit == '0')
                    current = current.left!;
                else if (bit == '1')
                    current = current.right!;

                if (current.left == null && current.right == null)
                {
                    decodedText += current.data;
                    current = root;
                }
            }

            return decodedText;
        }
    }

    internal class Program
    {
        static void Main()
        {
            string encodedText = "011100010010110010100111011011001110011111000";

            string filePath = "C:\\Users\\andre\\University\\Repos\\board_games\\board_games\\input.txt";
            string text = File.ReadAllText(filePath);

            Huffman huffman = new Huffman();
            Dictionary<char, int> frequencies = huffman.CalculateFrequenciesAndOrderThemInAscendingOrder(text);

            Node root = huffman.BuildHuffmanTree(frequencies);

            Huffman decoder = new Huffman();
            string decodedText = decoder.DecodeHuffmanText(encodedText, root);
            Console.Write('\n');
            Console.WriteLine("Decoded Text:\n" + decodedText);
        }
    }
}
