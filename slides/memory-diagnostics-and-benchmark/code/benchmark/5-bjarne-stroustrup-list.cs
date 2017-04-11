using System;
using System.Collections.Generic;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;

public class BjarneStroustrupList
{
    // https://isocpp.org/blog/2014/06/stroustrup-lists
    // Insert a sequence of random integers into a sorted sequence,
    // then remove those elements one by one as determined by a random sequece of positions,
    // Do you use a vector (a contiguously allocated sequence of elements) or a linked list? 
    [Params(16, 1024, 16 * 1024)]
    public int Size { get; set; }
    
    private Random _random = new Random(0);
    private int[] _randomNumbers;

    private List<int> _array;
    private LinkedList<int> _linkedList;

    [Setup]
    public void SetupData()
    {
        _array = new List<int>(Size);
        _linkedList = new LinkedList<int>();

        for (var i = 0; i < Size; i++)
        {
            _array.Add(i);
            _linkedList.AddLast(i);
        }

        _randomNumbers = new int[10];
        for (var i = 0; i < _randomNumbers.Length; i++)
        {
            _randomNumbers[i] = _random.Next(Size);
        }
    }

    [Benchmark(Baseline = true)]
    public object array()
    {
        foreach (var n in _randomNumbers)
        {
            var index = _array.IndexOf(n);
            if (index >= 0)
                _array.Insert(index, n);
            else
                _array.Add(n);
        }

        foreach (var n in _randomNumbers)
        {
            _array.Remove(n);
        }

        return _array;
    }

    [Benchmark]
    public object list()
    {
        foreach (var n in _randomNumbers)
        {
            var node = _linkedList.Find(n);
            if (node != null)
                _linkedList.AddBefore(node, n);
            else
                _linkedList.AddLast(n);
        }

        foreach (var n in _randomNumbers)
        {
            _linkedList.Remove(_linkedList.Find(n));
        }

        return _linkedList;
    }
}