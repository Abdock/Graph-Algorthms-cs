using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphTheory
{
    public class Graph
    {
        private readonly Dictionary<int, Vertex> _graph;

        public List<Vertex> Vertices { get; }
        public bool Oriented { get; }
        public bool Weighted { get; }

        public Graph(bool oriented, bool weighted)
        {
            Vertices = new List<Vertex>();
            Oriented = oriented;
            Weighted = weighted;
            _graph = new Dictionary<int, Vertex>();
        }

        public Graph(bool oriented, bool weighted, params Edge[] edges) : this(oriented, weighted)
        {
            int w = 1;
            for (int i = 0; i < edges.Length; ++i)
            {
                Vertices.Add(edges[i].From);
                Vertices.Add(edges[i].To);
                int u = edges[i].From.GetHashCode();
                int v = edges[i].To.GetHashCode();
                if (weighted)
                {
                    w = edges[i].Weight;
                }
                if (!_graph.ContainsKey(u))
                {
                    _graph[u] = edges[i].From;
                }

                if (!_graph.ContainsKey(v))
                {
                    _graph[v] = edges[i].To;
                }
                _graph[u].Neighbours[_graph[v]] = w;
                ++_graph[u].OutDegree;
                ++_graph[v].InDegree;
                
                if (!Oriented)
                {
                    _graph[v].Neighbours[_graph[u]] = w;
                    ++_graph[v].OutDegree;
                    ++_graph[u].InDegree;
                }
            }
        }

        public void AddEdge(Vertex from, Vertex to, int weight = 1)
        {
            int u = from.Name.GetHashCode();
            int v = from.Name.GetHashCode();
            if (!_graph.ContainsKey(u))
            {
                _graph[u] = from;
            }
            if (!_graph.ContainsKey(v))
            {
                _graph[v] = to;
            }
            if (!Weighted)
            {
                weight = 1;
            }
            _graph[u].Neighbours[to] = weight;
            ++_graph[u].OutDegree;
            ++_graph[v].InDegree;
            if (!Oriented)
            {
                _graph[v].Neighbours[from] = weight;
                ++_graph[v].OutDegree;
                ++_graph[u].InDegree;
            }
        }

        public void AddEdge(string from, string to, int weight)
        {
            AddEdge(new Vertex(from), new Vertex(to), weight);
        }

        public void DFS(Vertex vertex, Delegate preprocess, Delegate inPreLoop, Delegate inPostLoop, Delegate postprocess)
        {
            DFS(vertex.Name, preprocess, inPreLoop, inPostLoop, postprocess);
        }

        public void DFS(string vertex, Delegate preprocess, Delegate inPreLoop, Delegate inPostLoop,
            Delegate postprocess)
        {
            HashSet<int> was = new HashSet<int>();
            DFS(vertex.GetHashCode(), was, preprocess, inPreLoop, inPostLoop, postprocess);
        }

        private void DFS(int vertex, HashSet<int> was, Delegate pre, Delegate inPreLoop, Delegate inPostLoop, Delegate post)
        {
            was.Add(vertex);
            pre.DynamicInvoke(vertex, was);
            foreach (var currentVertex in _graph[vertex].Neighbours)
            {
                if (!was.Contains(currentVertex.Key.Name.GetHashCode()))
                {
                    inPreLoop.DynamicInvoke(vertex, was, currentVertex.Key, currentVertex.Value);
                    DFS(currentVertex.Key.Name.GetHashCode(), was, pre, inPreLoop, inPostLoop, post);
                    inPostLoop.DynamicInvoke(vertex, was, currentVertex.Key, currentVertex.Value);
                }
            }
            post.DynamicInvoke(vertex, was);
        }

        public Dictionary<Vertex, Dictionary<Vertex, int>> ShortestPathBetweenAllPairVertices()
        {
            var matrix = new Dictionary<Vertex, Dictionary<Vertex, int>>();
            foreach (var vertex1 in Vertices)
            {
                foreach (var vertex2 in Vertices)
                {
                    try
                    {
                        matrix[vertex1][vertex2] = _graph[vertex1.Name.GetHashCode()].Neighbours[vertex2];
                    }
                    catch (NullReferenceException exception)
                    {
                        matrix[vertex1][vertex2] = int.MaxValue;
                    }
                }
            }

            foreach (var vertex in Vertices)
            {
                foreach (var vertex1 in Vertices)
                {
                    foreach (var vertex2 in Vertices)
                    {
                        if (matrix[vertex1][vertex] < int.MaxValue && matrix[vertex][vertex2] < int.MaxValue)
                        {
                            matrix[vertex1][vertex2] = Math.Min(matrix[vertex1][vertex2], matrix[vertex1][vertex] + matrix[vertex][vertex2]);
                        }
                        else
                        {
                            matrix[vertex1].Remove(vertex2);
                        }
                    }
                }
            }

            foreach (var (key, _) in matrix)
            {
                if (matrix[key].Count == 0)
                {
                    matrix.Remove(key);
                }
            }
            return matrix;
        }

        public Dictionary<int, int> ShortestPathFromVertex(Vertex vertex)
        {
            return ShortestPathFromVertex(vertex.Name);
        }

        public Dictionary<int, int> ShortestPathFromVertex(string vertex)
        {
            return Weighted ? Dijkstra(vertex.GetHashCode()) : BFS(vertex.GetHashCode());
        }

        private Dictionary<int, int> BFS(int start)
        {
            if (!_graph.ContainsKey(start))
            {
                return new Dictionary<int, int>();
            }
            Queue<int> queue = new Queue<int>();
            Dictionary<int, int> distances = new Dictionary<int, int>(){{start, 0}};
            queue.Enqueue(start);
            HashSet<int> was = new HashSet<int> {start};
            while (queue.Count > 0)
            {
                int current = queue.Dequeue();
                foreach(var vertex in _graph[current].Neighbours)
                {
                    int to = vertex.Key.Name.GetHashCode();
                    if (!was.Contains(to))
                    {
                        was.Add(to);
                        distances[to] = distances[current] + 1;
                        queue.Enqueue(to);
                    }
                }
            }
            return distances;
        }

        private Dictionary<int, int> Dijkstra(int start)
        {
            if (!_graph.ContainsKey(start))
            {
                return new Dictionary<int, int>();
            }
            var distances = new Dictionary<int, int>() {{start, 0}};
            var queue = new SortedList<int, int>() {{0, start}};
            while (queue.Count > 0)
            {
                var current = queue.First();
                queue.RemoveAt(0);
                int vertex = current.Value, distance = current.Key;
                if (!distances.ContainsKey(vertex) || distances[vertex] < distance)
                {
                    continue;
                }

                foreach (var next in _graph[vertex].Neighbours)
                {
                    int to = next.Key.Name.GetHashCode(), length = next.Value;
                    if (!distances.ContainsKey(to) || distances[to] > distance + length)
                    {
                        distances[to] = distance + length;
                        queue[distances[to]] = to;
                    }
                }
            }
            return distances;
        }
    }
}