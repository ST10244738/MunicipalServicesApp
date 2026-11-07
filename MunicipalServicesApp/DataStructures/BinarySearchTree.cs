
using System;
using System.Collections.Generic;

namespace MunicipalServicesApp.DataStructures
{
    /// <summary>
    /// Represents a node in a Binary Search Tree.
    /// </summary>
    public class BSTNode<TKey, TValue>
    {
        public TKey Key { get; set; }

        public TValue Value { get; set; }

        public BSTNode<TKey, TValue>? Left { get; set; }

        public BSTNode<TKey, TValue>? Right { get; set; }

        public BSTNode(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

    /// <summary>
    /// Binary Search Tree implementation.
    /// Keys are maintained in sorted order.
    /// </summary>
    public class BinarySearchTree<TKey, TValue>
    {
        private BSTNode<TKey, TValue>? root;

        private readonly IComparer<TKey> comparer;

        public int Count { get; private set; }

        public BinarySearchTree()
        {
            comparer = Comparer<TKey>.Default;
            root = null;
            Count = 0;
        }

        /// <summary>
        /// Inserts a key and value into the tree.
        /// Duplicate keys are not inserted.
        /// </summary>
        public bool Insert(TKey key, TValue value)
        {
            if (root == null)
            {
                root = new BSTNode<TKey, TValue>(key, value);
                Count++;
                return true;
            }

            BSTNode<TKey, TValue>? current = root;
            BSTNode<TKey, TValue>? parent = null;

            while (current != null)
            {
                parent = current;

                int comparison = comparer.Compare(key, current.Key);

                if (comparison == 0)
                    return false;

                if (comparison < 0)
                    current = current.Left;
                else
                    current = current.Right;
            }

            BSTNode<TKey, TValue> newNode =
                new BSTNode<TKey, TValue>(key, value);

            if (comparer.Compare(key, parent!.Key) < 0)
                parent.Left = newNode;
            else
                parent.Right = newNode;

            Count++;
            return true;
        }

        /// <summary>
        /// Searches for a key and returns its associated value.
        /// </summary>
        public bool TryGetValue(TKey key, out TValue value)
        {
            BSTNode<TKey, TValue>? current = root;

            while (current != null)
            {
                int comparison = comparer.Compare(key, current.Key);

                if (comparison == 0)
                {
                    value = current.Value;
                    return true;
                }

                if (comparison < 0)
                    current = current.Left;
                else
                    current = current.Right;
            }

            value = default!;
            return false;
        }

        /// <summary>
        /// Checks whether a key exists in the tree.
        /// </summary>
        public bool ContainsKey(TKey key)
        {
            return TryGetValue(key, out _);
        }

        /// <summary>
        /// Returns all key-value pairs in ascending key order.
        /// </summary>
        public List<KeyValuePair<TKey, TValue>> InOrderTraversal()
        {
            List<KeyValuePair<TKey, TValue>> results =
                new List<KeyValuePair<TKey, TValue>>();

            InOrder(root, results);

            return results;
        }

        private void InOrder(
            BSTNode<TKey, TValue>? node,
            List<KeyValuePair<TKey, TValue>> results)
        {
            if (node == null)
                return;

            InOrder(node.Left, results);

            results.Add(
                new KeyValuePair<TKey, TValue>(
                    node.Key,
                    node.Value));

            InOrder(node.Right, results);
        }

        /// <summary>
        /// Removes a key from the tree.
        /// </summary>
        public bool Remove(TKey key)
        {
            bool removed = false;

            root = RemoveNode(root, key, ref removed);

            if (removed)
                Count--;

            return removed;
        }

        private BSTNode<TKey, TValue>? RemoveNode(
            BSTNode<TKey, TValue>? node,
            TKey key,
            ref bool removed)
        {
            if (node == null)
                return null;

            int comparison = comparer.Compare(key, node.Key);

            if (comparison < 0)
            {
                node.Left = RemoveNode(node.Left, key, ref removed);
            }
            else if (comparison > 0)
            {
                node.Right = RemoveNode(node.Right, key, ref removed);
            }
            else
            {
                removed = true;

                // No left child
                if (node.Left == null)
                    return node.Right;

                // No right child
                if (node.Right == null)
                    return node.Left;

                // Two children: replace with smallest
                // node from the right subtree.
                BSTNode<TKey, TValue> successor =
                    FindMinimum(node.Right);

                node.Key = successor.Key;
                node.Value = successor.Value;

                bool successorRemoved = false;

                node.Right = RemoveNode(
                    node.Right,
                    successor.Key,
                    ref successorRemoved);
            }

            return node;
        }

        private BSTNode<TKey, TValue> FindMinimum(
            BSTNode<TKey, TValue> node)
        {
            while (node.Left != null)
            {
                node = node.Left;
            }

            return node;
        }

        /// <summary>
        /// Returns the height of the tree.
        /// An empty tree has height zero.
        /// </summary>
        public int GetHeight()
        {
            return CalculateHeight(root);
        }

        private int CalculateHeight(BSTNode<TKey, TValue>? node)
        {
            if (node == null)
                return 0;

            return 1 + Math.Max(
                CalculateHeight(node.Left),
                CalculateHeight(node.Right));
        }
    }
}
