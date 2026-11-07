
using System;
using System.Collections.Generic;

namespace MunicipalServicesApp.DataStructures
{
    public enum NodeColor
    {
        Red,
        Black
    }

    public class RedBlackNode<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public NodeColor Color { get; set; }

        public RedBlackNode<TKey, TValue>? Left { get; set; }
        public RedBlackNode<TKey, TValue>? Right { get; set; }
        public RedBlackNode<TKey, TValue>? Parent { get; set; }

        public RedBlackNode(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            Color = NodeColor.Red;
        }
    }

    public class RedBlackTree<TKey, TValue>
    {
        private RedBlackNode<TKey, TValue>? root;
        private readonly IComparer<TKey> comparer;

        public int Count { get; private set; }

        public RedBlackTree()
        {
            comparer = Comparer<TKey>.Default;
            root = null;
            Count = 0;
        }

        // Insert a new key and value
        public bool Insert(TKey key, TValue value)
        {
            RedBlackNode<TKey, TValue>? parent = null;
            RedBlackNode<TKey, TValue>? current = root;

            while (current != null)
            {
                parent = current;

                int comparison = comparer.Compare(key, current.Key);

                if (comparison == 0)
                    return false;

                current = comparison < 0
                    ? current.Left
                    : current.Right;
            }

            RedBlackNode<TKey, TValue> newNode =
                new RedBlackNode<TKey, TValue>(key, value);

            newNode.Parent = parent;

            if (parent == null)
            {
                root = newNode;
            }
            else if (comparer.Compare(key, parent.Key) < 0)
            {
                parent.Left = newNode;
            }
            else
            {
                parent.Right = newNode;
            }

            FixInsertion(newNode);
            Count++;

            return true;
        }

        // Restore Red-Black Tree properties after insertion
        private void FixInsertion(RedBlackNode<TKey, TValue> node)
        {
            while (node != root &&
                   GetColor(node.Parent) == NodeColor.Red)
            {
                RedBlackNode<TKey, TValue> parent = node.Parent!;
                RedBlackNode<TKey, TValue> grandparent = parent.Parent!;

                if (parent == grandparent.Left)
                {
                    RedBlackNode<TKey, TValue>? uncle =
                        grandparent.Right;

                    if (GetColor(uncle) == NodeColor.Red)
                    {
                        parent.Color = NodeColor.Black;
                        uncle!.Color = NodeColor.Black;
                        grandparent.Color = NodeColor.Red;

                        node = grandparent;
                    }
                    else
                    {
                        if (node == parent.Right)
                        {
                            node = parent;
                            RotateLeft(node);

                            parent = node.Parent!;
                            grandparent = parent.Parent!;
                        }

                        parent.Color = NodeColor.Black;
                        grandparent.Color = NodeColor.Red;

                        RotateRight(grandparent);
                    }
                }
                else
                {
                    RedBlackNode<TKey, TValue>? uncle =
                        grandparent.Left;

                    if (GetColor(uncle) == NodeColor.Red)
                    {
                        parent.Color = NodeColor.Black;
                        uncle!.Color = NodeColor.Black;
                        grandparent.Color = NodeColor.Red;

                        node = grandparent;
                    }
                    else
                    {
                        if (node == parent.Left)
                        {
                            node = parent;
                            RotateRight(node);

                            parent = node.Parent!;
                            grandparent = parent.Parent!;
                        }

                        parent.Color = NodeColor.Black;
                        grandparent.Color = NodeColor.Red;

                        RotateLeft(grandparent);
                    }
                }
            }

            if (root != null)
                root.Color = NodeColor.Black;
        }

        // Left rotation
        private void RotateLeft(RedBlackNode<TKey, TValue> node)
        {
            RedBlackNode<TKey, TValue>? rightChild = node.Right;

            if (rightChild == null)
                return;

            node.Right = rightChild.Left;

            if (rightChild.Left != null)
                rightChild.Left.Parent = node;

            rightChild.Parent = node.Parent;

            if (node.Parent == null)
            {
                root = rightChild;
            }
            else if (node == node.Parent.Left)
            {
                node.Parent.Left = rightChild;
            }
            else
            {
                node.Parent.Right = rightChild;
            }

            rightChild.Left = node;
            node.Parent = rightChild;
        }

        // Right rotation
        private void RotateRight(RedBlackNode<TKey, TValue> node)
        {
            RedBlackNode<TKey, TValue>? leftChild = node.Left;

            if (leftChild == null)
                return;

            node.Left = leftChild.Right;

            if (leftChild.Right != null)
                leftChild.Right.Parent = node;

            leftChild.Parent = node.Parent;

            if (node.Parent == null)
            {
                root = leftChild;
            }
            else if (node == node.Parent.Right)
            {
                node.Parent.Right = leftChild;
            }
            else
            {
                node.Parent.Left = leftChild;
            }

            leftChild.Right = node;
            node.Parent = leftChild;
        }

        // Get the colour of a node
        private NodeColor GetColor(RedBlackNode<TKey, TValue>? node)
        {
            return node?.Color ?? NodeColor.Black;
        }

        // Search for a value using its key
        public bool TryGetValue(TKey key, out TValue value)
        {
            RedBlackNode<TKey, TValue>? current = root;

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

        // Return records in sorted key order
        public List<KeyValuePair<TKey, TValue>> InOrderTraversal()
        {
            List<KeyValuePair<TKey, TValue>> results =
                new List<KeyValuePair<TKey, TValue>>();

            InOrder(root, results);

            return results;
        }

        private void InOrder(
            RedBlackNode<TKey, TValue>? node,
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
    }
}
