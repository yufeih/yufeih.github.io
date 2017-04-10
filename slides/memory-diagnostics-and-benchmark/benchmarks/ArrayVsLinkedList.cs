using System;
using System.Collections.Generic;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class ArrayVsLinkedList
{
    [Params(16, 64, 128, 1024, 2059)]
    public int Size { get; set; }

    private readonly Random random = new Random(0);
    private List<int> _array;
    private LinkedList<int> _linkedList;
    private ListNode<int> _singleLinkedList;

    [Setup]
    public void SetupData()
    {
        _array = new List<int>(Size);
        _linkedList = new LinkedList<int>();
        _singleLinkedList = new ListNode<int>();

        var singleLinkedList = _singleLinkedList;

        for (var i = 0; i < Size; i++)
        {
            _array.Add(i);
            _linkedList.AddLast(i);
            singleLinkedList = singleLinkedList.AddLast(i);
        }
    }

    [Benchmark]
    public object array_random_remove()
    {
        for (var i = 0; i < Size; i++)
        {
            _array.Remove(random.Next(Size));
        }
        return _array;
    }

    [Benchmark]
    public object linked_list_random_remove()
    {
        for (var i = 0; i < Size; i++)
        {
            _linkedList.Remove(random.Next(Size));
        }
        return _linkedList;
    }

    [Benchmark]
    public object single_linked_list_random_remove()
    {
        for (var i = 0; i < Size; i++)
        {
            _singleLinkedList.Remove(random.Next(Size));
        }
        return _singleLinkedList;
    }

    class ListNode<T>
    {
        public T Value;
        public ListNode<T> Next;

        public ListNode<T> AddLast(T value)
        {
            return Next = new ListNode<T> { Value = value };
        }

        public void Remove(T value)
        {
            var node = this;

            while (node.Next != null)
            {
                if (EqualityComparer<T>.Default.Equals(node.Next.Value, value))
                {
                    node.Next = node.Next.Next;
                    break;
                }
                node = node.Next;
            }
        }
    }
}