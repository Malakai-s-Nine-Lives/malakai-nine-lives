using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CustomHeap<T> where T : IHeapItem<T>
{
    T[] items;  // Items in our heap
    int count;  // Number of items in the heap

    public CustomHeap(int maxHeapSize)
    {
        items = new T[maxHeapSize];  // Initialize heap
    }

    public void Add(T item)  // Add item to heap
    {
        item.HeapIndex = count;  // Get heap index from count
        items[count] = item;  // Add item
        SortUp(item);  // Sort heap
        count++;  // Update count
    }

    public T Pop()  // Remove item from heap
    {
        T firstItem = items[0];  // Get item on top
        count--;  // Decrement count
        items[0] = items[count];  // Move last value to top
        items[0].HeapIndex = 0;
        sortHeap(items[0]);  // Reheapify
        return firstItem;  // Return what we popped
    }

    void sortHeap(T item)  // Sort the heap at item from top to down
    {
        while(true)
        {
            // Get left and righ children
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;  // The point we will swap from

            // Preform the sorting depending on the values of the children
            if (childIndexLeft < count)
            {
                swapIndex = childIndexLeft;
                if (childIndexRight < count)
                {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                // Compare before sorting to be sure we are making the correct move
                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                {
                    return;
                }
            } else
            {
                return;
            }
        }
    }

    public void UpdateItem(T item)  // UpdateItem in Heap
    {
        sortHeap(item);  // Add item by reheapifying
    }

    public int Count  // Get size of heap
    {
        get
        {
            return count;
        }
    }

    public bool Contains(T item)  // Check if item exists in heap
    {
        return Equals(items[item.HeapIndex], item);
    }

    void SortUp(T item)  // Sort the heap at item from down to up
    {
        int parentIndex = (item.HeapIndex - 1) / 2;  // Get parent index
        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)  // Compare item with parent
            {
                Swap(item, parentItem);  // Swap if needed
            } else
            {
                break;
            }
        }
    }

    void Swap(T itemA, T itemB)  // Swap values in the heap
    {
        // Update values in heap 
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;  // temp value for swap

        // Update heap index after swap
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>  // Interface for generic heap
{
    // Need to create getter and setters for heap index if implementing this heap in other programs.
    int HeapIndex  
    {
        get;
        set;
    }
}
