using System.Collections.Generic;

namespace GraphTheory
{
    public class Vertex//Class for describe the vertex of graph, will contain information about vertex, weight, degrees, neighbours for current vertex etc
    {
        public Dictionary<Vertex, int> Neighbours { get; set; }//first argument contain the neighbour and second contain weight of edge to neighbour
        public int Degree { get; set; }
    }
}