
using System;
using System.Diagnostics;

namespace MunicipalServicesApp.DataStructures
{
    public static class GraphTest
    {
        public static void RunTests()
        {
            Graph<string> graph = new Graph<string>();

            // Add municipal departments and example connections
            graph.AddEdge("Water", "Roads", 4);
            graph.AddEdge("Water", "Electricity", 2);
            graph.AddEdge("Roads", "Waste", 5);
            graph.AddEdge("Electricity", "Parks", 3);
            graph.AddEdge("Parks", "Waste", 1);
            graph.AddEdge("Roads", "Parks", 6);

            // Test BFS
            var bfs = graph.BreadthFirstSearch("Water");

            Debug.WriteLine("BREADTH-FIRST SEARCH:");
            Debug.WriteLine(string.Join(" -> ", bfs));

            // Test DFS
            var dfs = graph.DepthFirstSearch("Water");

            Debug.WriteLine("DEPTH-FIRST SEARCH:");
            Debug.WriteLine(string.Join(" -> ", dfs));

            // Test Minimum Spanning Tree
            var mst = graph.MinimumSpanningTreePrim("Water");

            Debug.WriteLine("MINIMUM SPANNING TREE:");

            foreach (var edge in mst)
            {
                Debug.WriteLine(
                    $"{edge.Source} -> {edge.Destination}: {edge.Weight}");
            }

            Debug.WriteLine(
                "Total MST weight: " +
                graph.CalculateTotalWeight(mst));
        }
    }
}
