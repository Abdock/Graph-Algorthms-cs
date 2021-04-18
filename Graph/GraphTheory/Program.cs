using System;

namespace GraphTheory
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph( false, true);
            graph.AddEdge(1, 3, 5);
            graph.AddEdge(1, 2, 10);
            graph.AddEdge(2, 4, 4);
            graph.AddEdge(2, 3, 4);
            graph.AddEdge(1, 5, 5);
            graph.AddEdge(2, 4, 9);
            graph.AddEdge(2, 6, 0);
            graph.AddEdge(1, 6, 5);
            graph.AddEdge(3, 4, 8);
            graph.AddEdge(3, 7, 10);
            graph.AddEdge(2, 7, 11);
            graph.AddEdge(1, 7, 6);
            var d = graph.ShortestPathFromVertex(1);
            foreach (var item in d)
            {
                Console.WriteLine(item.Key + " " + item.Value);
            }
        }
    }
}