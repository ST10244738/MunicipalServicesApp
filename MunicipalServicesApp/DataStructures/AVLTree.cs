
using System;
using System.Collections.Generic;

namespace MunicipalServicesApp.DataStructures
{
    public class AVLNode<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public AVLNode<TKey, TValue>? Left { get; set; }
        public AVLNode<TKey, TValue>? Right { get; set; }

        public int Height { get; set; }

        public AVLNode(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            Height = 1;
        }
    }

    public class AVLTree<TKey, TValue>
    {
        private AVLNode<TKey, TValue>? root;
        private readonly IComparer<TKey> comparer;

        public int Count { get; private set; }

        public AVLTree()
        {
            comparer = Comparer<TKey>.Default;
            root = null;
            Count = 0;
        }

        // Insert a new key and value
        public bool Insert(TKey key, TValue value)
        {
            bool inserted = false;
            root = InsertNode(root, key, value, ref inserted);

            if (inserted)
                Count++;

            return inserted;
        }

        private AVLNode<TKey, TValue>? InsertNode(
            AVLNode<TKey, TValue>? node,
            TKey key,
            TValue value,
            ref bool inserted)
        {
            if (node == null)
            {
                inserted = true;
                return new AVLNode<TKey, TValue>(key, value);
            }

            int comparison = comparer.Compare(key, node.Key);

            if (comparison < 0)
                node.Left = InsertNode(node.Left, key, value, ref inserted);
            else if (comparison > 0)
                node.Right = InsertNode(node.Right, key, value, ref inserted);
            else
                return node;

            return Balance(node);
        }

        // Search for a value using its key
        public bool TryGetValue(TKey key, out TValue value)
        {
            AVLNode<TKey, TValue>? current = root;

            while (current != null)
            {
                int comparison = comparer.Compare(key, current.Key);

                if (comparison == 0)
                {
                    value = current.Value;
                    return true;
                }

                current = comparison < 0
                    ? current.Left
                    : current.Right;
            }

            value = default!;
            return false;
        }

        public bool ContainsKey(TKey key)
        {
            return TryGetValue(key, out _);
        }

        // Remove a key from the tree
        public bool Remove(TKey key)
        {
            bool removed = false;
            root = RemoveNode(root, key, ref removed);

            if (removed)
                Count--;

            return removed;
        }

        private AVLNode<TKey, TValue>? RemoveNode(
            AVLNode<TKey, TValue>? node,
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

                if (node.Left == null)
                    return node.Right;

                if (node.Right == null)
                    return node.Left;

                AVLNode<TKey, TValue> successor =
                    FindMinimum(node.Right);

                node.Key = successor.Key;
                node.Value = successor.Value;

                bool successorRemoved = false;

                node.Right = RemoveNode(
                    node.Right,
                    successor.Key,
                    ref successorRemoved);
            }

            return Balance(node);
        }

        private AVLNode<TKey, TValue> FindMinimum(
            AVLNode<TKey, TValue> node)
        {
            while (node.Left != null)
                node = node.Left;

            return node;
        }

        // Calculate node height
        private int GetNodeHeight(AVLNode<TKey, TValue>? node)
        {
            return node?.Height ?? 0;
        }

        // Calculate balance factor
        private int GetBalanceFactor(AVLNode<TKey, TValue>? node)
        {
            if (node == null)
                return 0;

            return GetNodeHeight(node.Left)
                 - GetNodeHeight(node.Right);
        }

        // Update node height
        private void UpdateHeight(AVLNode<TKey, TValue> node)
        {
            node.Height = 1 + Math.Max(
                GetNodeHeight(node.Left),
                GetNodeHeight(node.Right));
        }

        // Balance the tree when necessary
        private AVLNode<TKey, TValue> Balance(
            AVLNode<TKey, TValue> node)
        {
            UpdateHeight(node);

            int balanceFactor = GetBalanceFactor(node);

            // Left-heavy tree
            if (balanceFactor > 1)
            {
                if (GetBalanceFactor(node.Left) < 0)
                    node.Left = RotateLeft(node.Left!);

                return RotateRight(node);
            }

            // Right-heavy tree
            if (balanceFactor < -1)
            {
                if (GetBalanceFactor(node.Right) > 0)
                    node.Right = RotateRight(node.Right!);

                return RotateLeft(node);
            }

            return node;
        }

        // Right rotation
        private AVLNode<TKey, TValue> RotateRight(
            AVLNode<TKey, TValue> y)
        {
            AVLNode<TKey, TValue> x = y.Left!;
            AVLNode<TKey, TValue>? temporary = x.Right;

            x.Right = y;
            y.Left = temporary;

            UpdateHeight(y);
            UpdateHeight(x);

            return x;
        }

        // Left rotation
        private AVLNode<TKey, TValue> RotateLeft(
            AVLNode<TKey, TValue> x)
        {
            AVLNode<TKey, TValue> y = x.Right!;
            AVLNode<TKey, TValue>? temporary = y.Left;

            y.Left = x;
            x.Right = temporary;

            UpdateHeight(x);
            UpdateHeight(y);

            return y;
        }

        // Return all values in sorted key order
        public List<KeyValuePair<TKey, TValue>> InOrderTraversal()
        {
            List<KeyValuePair<TKey, TValue>> results =
                new List<KeyValuePair<TKey, TValue>>();

            InOrder(root, results);

            return results;
        }

        private void InOrder(
            AVLNode<TKey, TValue>? node,
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

        // Get the overall tree height
        public int GetHeight()
        {
            return GetNodeHeight(root);
        }
    }
}
