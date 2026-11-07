
using System;
using System.Collections.Generic;
using System.Linq;
using MunicipalServicesApp.Models;

namespace MunicipalServicesApp.DataStructures
{
    public class ServiceRequestDataManager
    {
        // --------------------------------------------------
        // BASIC TREE
        // Organises municipal departments hierarchically.
        // --------------------------------------------------

        public BasicTree<string> DepartmentHierarchy
        { get; private set; }

        // --------------------------------------------------
        // BINARY SEARCH TREE
        // Searches requests using reference numbers.
        // --------------------------------------------------

        public BinarySearchTree<string, ServiceRequest> SearchTree
        { get; private set; }

        // --------------------------------------------------
        // AVL TREE
        // Provides balanced request searching.
        // --------------------------------------------------

        public AVLTree<string, ServiceRequest> BalancedTree
        { get; private set; }

        // --------------------------------------------------
        // RED-BLACK TREE
        // Provides another balanced search index.
        // --------------------------------------------------

        public RedBlackTree<string, ServiceRequest> RedBlackIndex
        { get; private set; }

        // --------------------------------------------------
        // BINARY TREE
        // Organises service request records.
        // --------------------------------------------------

        public BinaryTree<ServiceRequest> RequestBinaryTree
        { get; private set; }

        // --------------------------------------------------
        // MAX HEAP
        // Prioritises urgent service requests.
        // --------------------------------------------------

        public MaxHeap<ServiceRequest> PriorityHeap
        { get; private set; }

        // --------------------------------------------------
        // GRAPH
        // Represents relationships between departments.
        // --------------------------------------------------

        public Graph<string> DepartmentGraph
        { get; private set; }

        // --------------------------------------------------
        // CONSTRUCTOR
        // --------------------------------------------------

        public ServiceRequestDataManager()
        {
            InitialiseStructures();

            LoadServiceRequests();
        }

        // --------------------------------------------------
        // INITIALISE ALL DATA STRUCTURES
        // --------------------------------------------------

        private void InitialiseStructures()
        {
            // Basic Tree
            DepartmentHierarchy =
                new BasicTree<string>("Municipality");

            // Binary Search Tree
            SearchTree =
                new BinarySearchTree<string, ServiceRequest>();

            // AVL Tree
            BalancedTree =
                new AVLTree<string, ServiceRequest>();

            // Red-Black Tree
            RedBlackIndex =
                new RedBlackTree<string, ServiceRequest>();

            // Binary Tree
            RequestBinaryTree =
                new BinaryTree<ServiceRequest>();

            // Max Heap
            PriorityHeap =
                new MaxHeap<ServiceRequest>(
                    Comparer<ServiceRequest>.Create(
                        (first, second) =>
                        {
                            // Higher priority value comes first.
                            int priorityComparison =
                                first.Priority.CompareTo(
                                    second.Priority);

                            if (priorityComparison != 0)
                                return priorityComparison;

                            // Older requests come first
                            // when priorities are equal.
                            return second.DateSubmitted.CompareTo(
                                first.DateSubmitted);
                        }));

            // Graph
            DepartmentGraph = new Graph<string>();
        }

        // --------------------------------------------------
        // LOAD SERVICE REQUESTS
        // Rebuilds structures to prevent duplicate records.
        // --------------------------------------------------

        public void LoadServiceRequests()
        {
            // Reinitialise structures before loading.
            InitialiseStructures();

            IReadOnlyList<ServiceRequest> requests =
                ServiceRequestRepository.GetAll();

            // Add the department hierarchy.
            TreeNode<string>? municipalityRoot =
                DepartmentHierarchy.Root;

            if (municipalityRoot != null)
            {
                HashSet<string> addedDepartments =
                    new HashSet<string>(
                        StringComparer.OrdinalIgnoreCase);

                foreach (ServiceRequest request in requests)
                {
                    string department =
                        request.Department.ToString();

                    if (addedDepartments.Add(department))
                    {
                        DepartmentHierarchy.AddChild(
                            municipalityRoot,
                            department);
                    }
                }
            }

            // Add requests to the other structures.
            foreach (ServiceRequest request in requests)
            {
                string reference =
                    request.ReferenceNumber;

                // Binary Search Tree
                SearchTree.Insert(reference, request);

                // AVL Tree
                BalancedTree.Insert(reference, request);

                // Red-Black Tree
                RedBlackIndex.Insert(reference, request);

                // Binary Tree
                RequestBinaryTree.Insert(request);

                // Max Heap
                PriorityHeap.Insert(request);

                // Graph vertices
                DepartmentGraph.AddVertex(
                    request.Department.ToString());
            }
        }

        // --------------------------------------------------
        // BASIC TREE OPERATIONS
        // --------------------------------------------------

        public TreeNode<string>? FindDepartment(
            string departmentName)
        {
            return DepartmentHierarchy.Find(
                department =>
                    string.Equals(
                        department,
                        departmentName,
                        StringComparison.OrdinalIgnoreCase));
        }

        public List<string> GetDepartmentHierarchyDepthFirst()
        {
            return DepartmentHierarchy.TraverseDepthFirst();
        }

        public List<string> GetDepartmentHierarchyBreadthFirst()
        {
            return DepartmentHierarchy.TraverseBreadthFirst();
        }

        public int GetDepartmentHierarchyCount()
        {
            return DepartmentHierarchy.CountNodes();
        }

        // --------------------------------------------------
        // BINARY SEARCH TREE OPERATIONS
        // --------------------------------------------------

        public ServiceRequest? FindUsingBST(
            string referenceNumber)
        {
            if (SearchTree.TryGetValue(
                referenceNumber,
                out ServiceRequest request))
            {
                return request;
            }

            return null;
        }

        public List<KeyValuePair<string, ServiceRequest>>
            GetRequestsInSortedOrder()
        {
            return SearchTree.InOrderTraversal();
        }

        // --------------------------------------------------
        // AVL TREE OPERATIONS
        // --------------------------------------------------

        public ServiceRequest? FindUsingAVL(
            string referenceNumber)
        {
            if (BalancedTree.TryGetValue(
                referenceNumber,
                out ServiceRequest request))
            {
                return request;
            }

            return null;
        }

        // --------------------------------------------------
        // RED-BLACK TREE OPERATIONS
        // --------------------------------------------------

        public ServiceRequest? FindUsingRedBlackTree(
            string referenceNumber)
        {
            if (RedBlackIndex.TryGetValue(
                referenceNumber,
                out ServiceRequest request))
            {
                return request;
            }

            return null;
        }

        // --------------------------------------------------
        // BINARY TREE OPERATIONS
        // --------------------------------------------------

        public int GetBinaryTreeRequestCount()
        {
            return RequestBinaryTree.CountNodes();
        }

        public List<ServiceRequest> GetBinaryTreeInOrder()
        {
            return RequestBinaryTree.InOrderTraversal();
        }

        // --------------------------------------------------
        // MAX HEAP OPERATIONS
        // --------------------------------------------------

        public ServiceRequest? GetMostUrgentRequest()
        {
            if (PriorityHeap.TryPeek(
                out ServiceRequest request))
            {
                return request;
            }

            return null;
        }

        public ServiceRequest? ProcessMostUrgentRequest()
        {
            if (PriorityHeap.TryExtractMax(
                out ServiceRequest request))
            {
                return request;
            }

            return null;
        }

        // --------------------------------------------------
        // GRAPH OPERATIONS
        // --------------------------------------------------

        public void ConnectDepartments(
            string source,
            string destination,
            int weight)
        {
            DepartmentGraph.AddEdge(
                source,
                destination,
                weight);
        }

        public List<string> GetDepartmentBFS(
            string startDepartment)
        {
            return DepartmentGraph.BreadthFirstSearch(
                startDepartment);
        }

        public List<string> GetDepartmentDFS(
            string startDepartment)
        {
            return DepartmentGraph.DepthFirstSearch(
                startDepartment);
        }

        public int GetReachableDepartmentCount(
            string startDepartment)
        {
            return DepartmentGraph.GetReachableVertexCount(
                startDepartment);
        }

        public List<MSTEdge<string>> GetDepartmentMinimumSpanningTree(
            string startDepartment)
        {
            return DepartmentGraph.PrimsMinimumSpanningTree(
                startDepartment);
        }

        public int GetDepartmentNetworkTotalWeight(
            string startDepartment)
        {
            List<MSTEdge<string>> minimumSpanningTree =
                GetDepartmentMinimumSpanningTree(
                    startDepartment);

            return DepartmentGraph.CalculateTotalWeight(
                minimumSpanningTree);
        }
    }
}
