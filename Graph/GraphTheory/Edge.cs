namespace GraphTheory
{
    public class Edge
    {
        public Vertex From { get; }
        
        public Vertex To { get; }

        public int Weight { get; set; }

        public Edge(Vertex from, Vertex to, int weight)
        {
            From = from;
            To = to;
            Weight = weight;
        }
    }
}