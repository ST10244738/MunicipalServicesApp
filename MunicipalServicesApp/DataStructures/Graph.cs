
using System;
using System.Collections.Generic;

namespace MunicipalServicesApp.DataStructures
{
    // Represents a connection between two vertices.
    public class GraphEdge<T>
    {
        public T Destination { get; set; }
        public int Weight { get; set; }

        public GraphEdge(T destination, int weight)
        {
            Destination = destination;
            Weight = weight;
        }
    }

    // Represents an edge selected for the minimum spanning tree.
    public class MSTEdge<T>
    {
        public T Source { get; set; }
        public T Destination { get; set; }
        public int Weight { get; set; }

        public MSTEdge(T source, T destination, int weight)
        {
            Source = source;
            Destination = destination;
            Weight = weight;
        }
    }

    public class Graph<T> where T : notnull
    {
        private readonly Dictionary<T, List<GraphEdge<T>>> adjacencyList;

        public int VertexCount => adjacencyList.Count;

        public Graph()
        {
            adjacencyList = new Dictionary<T, List<GraphEdge<T>>>();
        }

        // Add a vertex to the graph.
        public void AddVertex(T vertex)
        {
            if (!adjacencyList.ContainsKey(vertex))
            {
                adjacencyList[vertex] = new List<GraphEdge<T>>();
            }
        }

        // Add an undirected weighted connection.
        public void AddEdge(T source, T destination, int weight)
        {
            if (weight < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(weight),
                    "Edge weight cannot be negative.");
            }

            AddVertex(source);
            AddVertex(destination);

            adjacencyList[source].Add(
                new GraphEdge<T>(destination, weight));

            adjacencyList[destination].Add(
                new GraphEdge<T>(source, weight));
        }

        // Breadth-First Search (BFS).
        public List<T> BreadthFirstSearch(T start)
        {
            List<T> visitedOrder = new List<T>();

            if (!adjacencyList.ContainsKey(start))
            {
                return visitedOrder;
            }

            HashSet<T> visited = new HashSet<T>();
            Queue<T> queue = new Queue<T>();

            visited.Add(start);
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                T current = queue.Dequeue();
                visitedOrder.Add(current);

                foreach (GraphEdge<T> edge in adjacencyList[current])
                {
                    if (visited.Add(edge.Destination))
                    {
                        queue.Enqueue(edge.Destination);
                    }
                }
            }

            return visitedOrder;
        }

        // Depth-First Search (DFS).
        public List<T> DepthFirstSearch(T start)
        {
            List<T> visitedOrder = new List<T>();

            if (!adjacencyList.ContainsKey(start))
            {
                return visitedOrder;
            }

            HashSet<T> visited = new HashSet<T>();
            Stack<T> stack = new Stack<T>();

            stack.Push(start);

            while (stack.Count > 0)
            {
                T current = stack.Pop();

                if (!visited.Add(current))
                {
                    continue;
                }

                visitedOrder.Add(current);

                List<GraphEdge<T>> edges = adjacencyList[current];

                // Reverse iteration preserves adjacency order.
                for (int i = edges.Count - 1; i >= 0; i--)
                {
                    T destination = edges[i].Destination;

                    if (!visited.Contains(destination))
                    {
                        stack.Push(destination);
                    }
                }
            }

            return visitedOrder;
        }

        // Prim's Minimum Spanning Tree algorithm.
        // Returns the MST for the connected component
        // containing the supplied starting vertex.
        public List<MSTEdge<T>> MinimumSpanningTreePrim(T start)
        {
            List<MSTEdge<T>> result = new List<MSTEdge<T>>();

            if (!adjacencyList.ContainsKey(start))
            {
                return result;
            }

            HashSet<T> visited = new HashSet<T>();
            visited.Add(start);

            int reachableCount = GetReachableVertexCount(start);

            while (visited.Count < reachableCount)
            {
                GraphEdge<T>? bestEdge = null;
                T bestSource = default!;
                bool edgeFound = false;

                foreach (T vertex in visited)
                {
                    foreach (GraphEdge<T> edge in adjacencyList[vertex])
                    {
                        if (visited.Contains(edge.Destination))
                        {
                            continue;
                        }

                        if (!edgeFound || edge.Weight < bestEdge!.Weight)
                        {
                            bestEdge = edge;
                            bestSource = vertex;
                            edgeFound = true;
                        }
                    }
                }

                if (!edgeFound || bestEdge == null)
                {
                    break;
                }

                result.Add(new MSTEdge<T>(
                    bestSource,
                    bestEdge.Destination,
                    bestEdge.Weight));

                visited.Add(bestEdge.Destination);
            }

            return result;
        }

        // Compatibility method for ServiceRequestDataManager.
        public List<MSTEdge<T>> PrimsMinimumSpanningTree(T start)
        {
            return MinimumSpanningTreePrim(start);
        }

        // Count vertices reachable from the starting vertex.
        // Public so ServiceRequestDataManager can access it.
        public int GetReachableVertexCount(T start)
        {
            if (!adjacencyList.ContainsKey(start))
            {
                return 0;
            }

            HashSet<T> visited = new HashSet<T>();
            Queue<T> queue = new Queue<T>();

            visited.Add(start);
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                T current = queue.Dequeue();

                foreach (GraphEdge<T> edge in adjacencyList[current])
                {
                    if (visited.Add(edge.Destination))
                    {
                        queue.Enqueue(edge.Destination);
                    }
                }
            }

            return visited.Count;
        }

        // Calculate the total weight of selected MST edges.
        public int CalculateTotalWeight(List<MSTEdge<T>> edges)
        {
            int total = 0;

            foreach (MSTEdge<T> edge in edges)
            {
                total += edge.Weight;
            }

            return total;
        }

        // Return a copy of the graph's connections.
        public Dictionary<T, List<GraphEdge<T>>> GetAdjacencyList()
        {
            Dictionary<T, List<GraphEdge<T>>> copy =
                new Dictionary<T, List<GraphEdge<T>>>();

            foreach (var entry in adjacencyList)
            {
                copy[entry.Key] =
                    new List<GraphEdge<T>>(entry.Value);
            }

            return copy;
        }
    }
}
