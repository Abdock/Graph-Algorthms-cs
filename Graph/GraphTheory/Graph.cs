using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphTheory
{
    public class Graph
    {
        private class Pair
        {
            public int First;
            public int Second;

            public Pair(int first, int second)
            {
                First = first;
                Second = second;
            }
        }
        
        private Dictionary<int, List<Pair>> _graph;
        public Dictionary<int, int> Degrees { get; }
        public bool Oriented { get; }
        public bool Weighted { get; }

        public Graph(bool oriented, bool weighted)
        {
            Oriented = oriented;
            Weighted = weighted;
            _graph = new Dictionary<int, List<Pair>>();
            Degrees = new Dictionary<int, int>();
        }

        public Graph(int m, bool oriented, bool weighted) : this(oriented, weighted)
        {
            int u, v, w = 1;
            for (int i = 0; i < m; ++i)
            {
                string[] input = Console.ReadLine().Split(' ');
                u = int.Parse(input[0]);
                v = int.Parse(input[1]);
                if (weighted)
                {
                    w = int.Parse(input[2]);
                }
                if (!_graph.ContainsKey(u))
                {
                    _graph[u] = new List<Pair>();
                    Degrees[u] = 0;
                }
                _graph[u].Add(new Pair(v, w));
                ++Degrees[u];
                if (!Oriented)
                {
                    if (!_graph.ContainsKey(v))
                    {
                        _graph[v] = new List<Pair>();
                        Degrees[v] = 0;
                    }
                    _graph[v].Add(new Pair(u, w));
                    ++Degrees[v];
                }
            }
        }

        public void AddEdge(int from, int to, int weight = 1)
        {
            if (!_graph.ContainsKey(from))
            {
                _graph[from] = new List<Pair>();
                Degrees[from] = 0;
            }

            if (!Weighted)
            {
                weight = 1;
            }
            _graph[from].Add(new Pair(to, weight));
            ++Degrees[from];
            if (!Oriented)
            {
                if (!_graph.ContainsKey(to))
                {
                    _graph[to] = new List<Pair>();
                    Degrees[to] = 0;
                }
                _graph[to].Add(new Pair(from, weight));
                ++Degrees[to];
            }
        }

        public void DFS(int vertex)
        {
            HashSet<int> was = new HashSet<int>();
            DFS(vertex, was);
        }

        private void DFS(int vertex, HashSet<int> was)
        {
            was.Add(vertex);
            if (!_graph.ContainsKey(vertex))
            {
                return;
            }
            for (int i = 0; i < _graph[vertex].Count; ++i)
            {
                if (_graph.ContainsKey(_graph[vertex][i].First) && !was.Contains(_graph[vertex][i].First))
                {
                    DFS(_graph[vertex][i].First, was);
                }
            }
        }

        public Dictionary<int, int> ShortestPathFromVertex(int vertex)
        {
            return Weighted ? Dijkstra(vertex) : BFS(vertex);
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
                for (int i = 0; i < _graph[current].Count; ++i)
                {
                    int to = _graph[current][i].First;
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

                for (int i = 0; i < _graph[vertex].Count; ++i)
                {
                    int to = _graph[vertex][i].First, length = _graph[vertex][i].Second;
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