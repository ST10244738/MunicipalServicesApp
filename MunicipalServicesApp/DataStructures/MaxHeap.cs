
using System;
using System.Collections.Generic;

namespace MunicipalServicesApp.DataStructures
{
    public class MaxHeap<T>
    {
        private readonly List<T> items;
        private readonly IComparer<T> comparer;

        public int Count => items.Count;

        public bool IsEmpty => items.Count == 0;

        public MaxHeap() : this(Comparer<T>.Default)
        {
        }

        public MaxHeap(IComparer<T> comparer)
        {
            this.comparer = comparer ??
                throw new ArgumentNullException(nameof(comparer));

            items = new List<T>();
        }

        // Add an item to the heap
        public void Insert(T item)
        {
            items.Add(item);
            HeapifyUp(items.Count - 1);
        }

        // Return the highest-priority item without removing it
        public bool TryPeek(out T item)
        {
            if (IsEmpty)
            {
                item = default!;
                return false;
            }

            item = items[0];
            return true;
        }

        // Remove and return the highest-priority item
        public bool TryExtractMax(out T item)
        {
            if (IsEmpty)
            {
                item = default!;
                return false;
            }

            item = items[0];

            if (items.Count == 1)
            {
                items.RemoveAt(0);
                return true;
            }

            items[0] = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);

            HeapifyDown(0);

            return true;
        }

        // Move an item upwards to maintain heap order
        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;

                if (comparer.Compare(
                    items[index],
                    items[parentIndex]) <= 0)
                {
                    break;
                }

                Swap(index, parentIndex);
                index = parentIndex;
            }
        }

        // Move an item downwards to maintain heap order
        private void HeapifyDown(int index)
        {
            while (true)
            {
                int leftChild = (2 * index) + 1;
                int rightChild = (2 * index) + 2;

                int largest = index;

                if (leftChild < items.Count &&
                    comparer.Compare(
                        items[leftChild],
                        items[largest]) > 0)
                {
                    largest = leftChild;
                }

                if (rightChild < items.Count &&
                    comparer.Compare(
                        items[rightChild],
                        items[largest]) > 0)
                {
                    largest = rightChild;
                }

                if (largest == index)
                    break;

                Swap(index, largest);
                index = largest;
            }
        }

        // Swap two items
        private void Swap(int firstIndex, int secondIndex)
        {
            T temporary = items[firstIndex];
            items[firstIndex] = items[secondIndex];
            items[secondIndex] = temporary;
        }

        // Return a snapshot of the heap's internal array
        public List<T> GetItems()
        {
            return new List<T>(items);
        }

        // Remove all items
        public void Clear()
        {
            items.Clear();
        }
    }
}
