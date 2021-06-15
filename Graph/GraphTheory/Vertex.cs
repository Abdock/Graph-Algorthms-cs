using System.Collections.Generic;

namespace GraphTheory
{
    public class Vertex//Class for describe the vertex of graph, will contain information about vertex, weight, degrees, neighbours for current vertex etc
    {
        public string Name { get; }
        public Dictionary<Vertex, int> Neighbours { get; }//first argument contain the neighbour and second contain weight of edge to neighbour
        public int InDegree { get; set; }//count of edges that direct from other vertex to current
        public int OutDegree { get; set; }//count of edges that direct from this vertex to other

        public Vertex(string name)
        {
            Name = name;
            Neighbours = new Dictionary<Vertex, int>();
        }
    }
}