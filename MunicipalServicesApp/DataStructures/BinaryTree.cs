
using System;
using System.Collections.Generic;

namespace MunicipalServicesApp.DataStructures
{
    /// <summary>
    /// Represents a node in a binary tree.
    /// Each node can have a maximum of two children.
    /// </summary>
    public class BinaryTreeNode<T>
    {
        public T Data { get; set; }

        public BinaryTreeNode<T>? Left { get; set; }

        public BinaryTreeNode<T>? Right { get; set; }

        public BinaryTreeNode(T data)
        {
            Data = data;
        }
    }

    /// <summary>
    /// Generic binary tree implementation.
    /// Supports insertion, searching and traversal.
    /// </summary>
    public class BinaryTree<T>
    {
        public BinaryTreeNode<T>? Root { get; private set; }

        public BinaryTree()
        {
            Root = null;
        }

        /// <summary>
        /// Inserts a node using level-order insertion.
        /// This keeps the tree filled from left to right.
        /// </summary>
        public void Insert(T data)
        {
            BinaryTreeNode<T> newNode =
                new BinaryTreeNode<T>(data);

            if (Root == null)
            {
                Root = newNode;
                return;
            }

            Queue<BinaryTreeNode<T>> queue =
                new Queue<BinaryTreeNode<T>>();

            queue.Enqueue(Root);

            while (queue.Count > 0)
            {
                BinaryTreeNode<T> current = queue.Dequeue();

                if (current.Left == null)
                {
                    current.Left = newNode;
                    return;
                }

                queue.Enqueue(current.Left);

                if (current.Right == null)
                {
                    current.Right = newNode;
                    return;
                }

                queue.Enqueue(current.Right);
            }
        }

        /// <summary>
        /// Searches the binary tree using breadth-first search.
        /// </summary>
        public BinaryTreeNode<T>? Find(Predicate<T> condition)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));

            if (Root == null)
                return null;

            Queue<BinaryTreeNode<T>> queue =
                new Queue<BinaryTreeNode<T>>();

            queue.Enqueue(Root);

            while (queue.Count > 0)
            {
                BinaryTreeNode<T> current = queue.Dequeue();

                if (condition(current.Data))
                    return current;

                if (current.Left != null)
                    queue.Enqueue(current.Left);

                if (current.Right != null)
                    queue.Enqueue(current.Right);
            }

            return null;
        }

        /// <summary>
        /// Visits the root before its children.
        /// </summary>
        public List<T> PreOrderTraversal()
        {
            List<T> results = new List<T>();

            PreOrder(Root, results);

            return results;
        }

        private void PreOrder(
            BinaryTreeNode<T>? node,
            List<T> results)
        {
            if (node == null)
                return;

            results.Add(node.Data);

            PreOrder(node.Left, results);
            PreOrder(node.Right, results);
        }

        /// <summary>
        /// Visits the left subtree, root and right subtree.
        /// </summary>
        public List<T> InOrderTraversal()
        {
            List<T> results = new List<T>();

            InOrder(Root, results);

            return results;
        }

        private void InOrder(
            BinaryTreeNode<T>? node,
            List<T> results)
        {
            if (node == null)
                return;

            InOrder(node.Left, results);

            results.Add(node.Data);

            InOrder(node.Right, results);
        }

        /// <summary>
        /// Visits both children before the root.
        /// </summary>
        public List<T> PostOrderTraversal()
        {
            List<T> results = new List<T>();

            PostOrder(Root, results);

            return results;
        }

        private void PostOrder(
            BinaryTreeNode<T>? node,
            List<T> results)
        {
            if (node == null)
                return;

            PostOrder(node.Left, results);
            PostOrder(node.Right, results);

            results.Add(node.Data);
        }

        /// <summary>
        /// Counts the nodes in the binary tree.
        /// </summary>
        public int CountNodes()
        {
            return Count(Root);
        }

        private int Count(BinaryTreeNode<T>? node)
        {
            if (node == null)
                return 0;

            return 1 + Count(node.Left) + Count(node.Right);
        }

        /// <summary>
        /// Calculates the height of the binary tree.
        /// An empty tree has height 0.
        /// </summary>
        public int GetHeight()
        {
            return Height(Root);
        }

        private int Height(BinaryTreeNode<T>? node)
        {
            if (node == null)
                return 0;

            return 1 + Math.Max(
                Height(node.Left),
                Height(node.Right));
        }
    }
}
