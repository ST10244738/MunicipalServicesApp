
using System;
using System.Collections.Generic;
using System.Linq;

namespace MunicipalServicesApp.DataStructures
{
    /// <summary>
    /// Represents one node in a general tree.
    /// Each node can contain multiple children.
    /// </summary>
    public class TreeNode<T>
    {
        public T Data { get; set; }

        public TreeNode<T>? Parent { get; private set; }

        public List<TreeNode<T>> Children { get; private set; }

        public TreeNode(T data)
        {
            Data = data;
            Children = new List<TreeNode<T>>();
        }

        /// <summary>
        /// Adds a child node to this node.
        /// </summary>
        public TreeNode<T> AddChild(T data)
        {
            TreeNode<T> child = new TreeNode<T>(data);

            child.Parent = this;

            Children.Add(child);

            return child;
        }

        /// <summary>
        /// Checks whether this node is a leaf.
        /// </summary>
        public bool IsLeaf()
        {
            return Children.Count == 0;
        }
    }

    /// <summary>
    /// Generic tree structure that supports
    /// multiple children for each node.
    /// </summary>
    public class BasicTree<T>
    {
        public TreeNode<T>? Root { get; private set; }

        public BasicTree(T rootData)
        {
            Root = new TreeNode<T>(rootData);
        }

        /// <summary>
        /// Adds a child to a specified parent node.
        /// </summary>
        public TreeNode<T> AddChild(
            TreeNode<T> parent,
            T childData)
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            return parent.AddChild(childData);
        }

        /// <summary>
        /// Searches the tree using depth-first traversal.
        /// </summary>
        public TreeNode<T>? Find(
            Predicate<T> condition)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));

            if (Root == null)
                return null;

            Stack<TreeNode<T>> stack =
                new Stack<TreeNode<T>>();

            stack.Push(Root);

            while (stack.Count > 0)
            {
                TreeNode<T> current = stack.Pop();

                if (condition(current.Data))
                    return current;

                // Reverse insertion preserves the original
                // left-to-right child order during traversal.
                for (int i = current.Children.Count - 1; i >= 0; i--)
                {
                    stack.Push(current.Children[i]);
                }
            }

            return null;
        }

        /// <summary>
        /// Returns all tree values in depth-first order.
        /// </summary>
        public List<T> TraverseDepthFirst()
        {
            List<T> results = new List<T>();

            if (Root == null)
                return results;

            Stack<TreeNode<T>> stack =
                new Stack<TreeNode<T>>();

            stack.Push(Root);

            while (stack.Count > 0)
            {
                TreeNode<T> current = stack.Pop();

                results.Add(current.Data);

                for (int i = current.Children.Count - 1; i >= 0; i--)
                {
                    stack.Push(current.Children[i]);
                }
            }

            return results;
        }

        /// <summary>
        /// Returns all tree values in breadth-first order.
        /// </summary>
        public List<T> TraverseBreadthFirst()
        {
            List<T> results = new List<T>();

            if (Root == null)
                return results;

            Queue<TreeNode<T>> queue =
                new Queue<TreeNode<T>>();

            queue.Enqueue(Root);

            while (queue.Count > 0)
            {
                TreeNode<T> current = queue.Dequeue();

                results.Add(current.Data);

                foreach (TreeNode<T> child in current.Children)
                {
                    queue.Enqueue(child);
                }
            }

            return results;
        }

        /// <summary>
        /// Counts all nodes in the tree.
        /// </summary>
        public int CountNodes()
        {
            return TraverseDepthFirst().Count;
        }
    }
}
