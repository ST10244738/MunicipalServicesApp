
using System;
using System.Diagnostics;
using System.Linq;
using MunicipalServicesApp.Models;

namespace MunicipalServicesApp.DataStructures
{
    public static class ServiceRequestDataManagerTest
    {
        public static void RunTests()
        {
            ServiceRequestDataManager manager =
                new ServiceRequestDataManager();

            Debug.WriteLine("");
            Debug.WriteLine("==============================================");
            Debug.WriteLine(" SERVICE REQUEST DATA STRUCTURE TESTS");
            Debug.WriteLine("==============================================");

            // TEST 1: Repository records
            Debug.WriteLine("");
            Debug.WriteLine("TEST 1: Repository");

            Debug.Assert(
                ServiceRequestRepository.GetAll().Count == 5,
                "Repository should contain five sample requests.");

            Debug.WriteLine(
                "Repository records: " +
                ServiceRequestRepository.GetAll().Count);

            // TEST 2: Basic Tree - Department Hierarchy
            Debug.WriteLine("");
            Debug.WriteLine("TEST 2: Basic Tree");

            Debug.Assert(
                manager.DepartmentHierarchy.CountNodes() == 5,
                "Basic Tree should contain the municipality and four departments.");

            Debug.WriteLine(
                "Department hierarchy nodes: " +
                manager.DepartmentHierarchy.CountNodes());

            Debug.WriteLine("Department hierarchy (DFS):");

            foreach (var department in
                manager.GetDepartmentHierarchyDepthFirst())
            {
                Debug.WriteLine(department);
            }

            Debug.WriteLine("Department hierarchy (BFS):");

            foreach (var department in
                manager.GetDepartmentHierarchyBreadthFirst())
            {
                Debug.WriteLine(department);
            }

            // TEST 3: Binary Search Tree
            Debug.WriteLine("");
            Debug.WriteLine("TEST 3: Binary Search Tree");

            var bstRequest =
                manager.FindUsingBST("SR-2026-0001");

            Debug.Assert(
                bstRequest != null,
                "BST should find SR-2026-0001.");

            Debug.WriteLine(
                "BST search: " +
                (bstRequest?.ReferenceNumber ?? "Not found"));

            // TEST 4: AVL Tree
            Debug.WriteLine("");
            Debug.WriteLine("TEST 4: AVL Tree");

            var avlRequest =
                manager.FindUsingAVL("SR-2026-0002");

            Debug.Assert(
                avlRequest != null,
                "AVL should find SR-2026-0002.");

            Debug.WriteLine(
                "AVL search: " +
                (avlRequest?.ReferenceNumber ?? "Not found"));

            // TEST 5: Red-Black Tree
            Debug.WriteLine("");
            Debug.WriteLine("TEST 5: Red-Black Tree");

            var redBlackRequest =
                manager.FindUsingRedBlackTree("SR-2026-0003");

            Debug.Assert(
                redBlackRequest != null,
                "Red-Black Tree should find SR-2026-0003.");

            Debug.WriteLine(
                "Red-Black search: " +
                (redBlackRequest?.ReferenceNumber ?? "Not found"));

            // TEST 6: Binary Tree
            Debug.WriteLine("");
            Debug.WriteLine("TEST 6: Binary Tree");

            int binaryTreeCount =
                manager.RequestBinaryTree.CountNodes();

            Debug.Assert(
                binaryTreeCount == 5,
                "Binary Tree should contain five requests.");

            Debug.WriteLine(
                "Binary Tree records: " + binaryTreeCount);

            // TEST 7: Max Heap
            Debug.WriteLine("");
            Debug.WriteLine("TEST 7: Max Heap");

            var urgentRequest =
                manager.GetMostUrgentRequest();

            Debug.Assert(
                urgentRequest != null &&
                urgentRequest.Priority == ServiceRequestPriority.Critical,
                "Max Heap should return the critical request first.");

            Debug.WriteLine(
                "Highest priority request: " +
                (urgentRequest?.ReferenceNumber ?? "Not found"));

            Debug.WriteLine(
                "Highest priority: " +
                urgentRequest?.Priority);

            // TEST 8: Sorted BST traversal
            Debug.WriteLine("");
            Debug.WriteLine("TEST 8: BST Sorted Traversal");

            var sortedRequests =
                manager.GetRequestsInSortedOrder();

            Debug.Assert(
                sortedRequests.Count == 5,
                "BST traversal should return five requests.");

            Debug.WriteLine("Requests in reference-number order:");

            foreach (var item in sortedRequests)
            {
                Debug.WriteLine(item.Key);
            }

            // TEST 9: Graph department registration
            Debug.WriteLine("");
            Debug.WriteLine("TEST 9: Graph Vertices");

            var departments =
                manager.DepartmentGraph
                    .GetAdjacencyList()
                    .Keys
                    .ToList();

            Debug.Assert(
                departments.Count == 4,
                "Department graph should contain four departments.");

            Debug.WriteLine(
                "Department graph vertices: " +
                manager.DepartmentGraph.VertexCount);

            // TEST 10: Add sample connections for graph testing
            Debug.WriteLine("");
            Debug.WriteLine("TEST 10: Graph Connections");

            // These weights are illustrative test values,
            // not actual municipal costs or distances.
            for (int i = 1; i < departments.Count; i++)
            {
                manager.DepartmentGraph.AddEdge(
                    departments[0],
                    departments[i],
                    i);
            }

            Debug.WriteLine(
                "Sample department connections added.");

            // TEST 11: Breadth-First Search
            Debug.WriteLine("");
            Debug.WriteLine("TEST 11: Breadth-First Search");

            var bfsResult =
                manager.DepartmentGraph.BreadthFirstSearch(
                    departments[0]);

            Debug.Assert(
                bfsResult.Count == 4,
                "BFS should visit all four connected departments.");

            Debug.WriteLine("BFS traversal:");

            foreach (var department in bfsResult)
            {
                Debug.WriteLine(department);
            }

            // TEST 12: Depth-First Search
            Debug.WriteLine("");
            Debug.WriteLine("TEST 12: Depth-First Search");

            var dfsResult =
                manager.DepartmentGraph.DepthFirstSearch(
                    departments[0]);

            Debug.Assert(
                dfsResult.Count == 4,
                "DFS should visit all four connected departments.");

            Debug.WriteLine("DFS traversal:");

            foreach (var department in dfsResult)
            {
                Debug.WriteLine(department);
            }

            // TEST 13: Reachable department count
            Debug.WriteLine("");
            Debug.WriteLine("TEST 13: Reachable Vertices");

            int reachableCount =
                manager.DepartmentGraph.GetReachableVertexCount(
                    departments[0]);

            Debug.Assert(
                reachableCount == 4,
                "All four departments should be reachable.");

            Debug.WriteLine(
                "Reachable departments: " + reachableCount);

            // TEST 14: Prim's Minimum Spanning Tree
            Debug.WriteLine("");
            Debug.WriteLine("TEST 14: Minimum Spanning Tree");

            var minimumSpanningTree =
                manager.DepartmentGraph.MinimumSpanningTreePrim(
                    departments[0]);

            Debug.Assert(
                minimumSpanningTree.Count == 3,
                "MST should contain three edges for four connected vertices.");

            int totalWeight =
                manager.DepartmentGraph.CalculateTotalWeight(
                    minimumSpanningTree);

            Debug.WriteLine("Selected MST connections:");

            foreach (var edge in minimumSpanningTree)
            {
                Debug.WriteLine(
                    edge.Source + " -> " +
                    edge.Destination + " | Weight: " +
                    edge.Weight);
            }

            Debug.WriteLine(
                "MST total weight: " + totalWeight);

            // Final test summary
            Debug.WriteLine("");
            Debug.WriteLine("==============================================");
            Debug.WriteLine(" ALL TESTS COMPLETED");
            Debug.WriteLine("==============================================");
        }
    }
}
