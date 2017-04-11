using System;
using System.Collections.Generic;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;
using System.Linq;

public class BinarySearchVsDictionaryVsScan
{
    [Params(16, 128, 512)]
    public int Size { get; set; }

    private readonly Random _random = new Random(0);
    private List<int> _array;
    private HashSet<int> _dictionary;

    [Setup]
    public void SetupData()
    {
        _array = Enumerable.Range(0, Size).ToList();
        _dictionary = new HashSet<int>(Enumerable.Range(0, Size));
    }

    [Benchmark(Baseline = true)]
    public int scan()
    {
        var temp = 0;
        for (var i = 0; i < 100; i++)
        {
            temp |= _array.IndexOf(_random.Next(Size));
        }
        return temp;
    }

    [Benchmark]
    public int binary_search()
    {
        var temp = 0;
        for (var i = 0; i < 100; i++)
        {
            temp |= _array.BinarySearch(_random.Next(Size));
        }
        return temp;
    }

    [Benchmark]
    public bool dictionary_lookup()
    {
        var temp = false;
        for (var i = 0; i < 100; i++)
        {
            temp |= _dictionary.Contains(_random.Next(Size));
        }
        return temp;
    }
}