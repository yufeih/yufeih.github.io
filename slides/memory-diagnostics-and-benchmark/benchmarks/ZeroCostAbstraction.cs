using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;
using System.Linq;
using System.Collections.Generic;

[MemoryDiagnoser]
public class ZeroCostAbstraction
{
    private static readonly List<int> items = Enumerable.Range(0, 8192).ToList();

    [Benchmark(Baseline = true)]
    public int for_loop()
    {
        var sum = 0;
        for (var i = 0; i < items.Count; i++)
        {
            sum += items[i];
        }
        return sum;
    }

    [Benchmark]
    public int for_each()
    {
        var sum = 0;
        foreach (var i in items)
        {
            sum += i;
        }
        return sum;
    }

    [Benchmark]
    public int linq()
    {
        return items.Sum();
    }

    [Benchmark]
    public int for_each_enumerable()
    {
        return SumEnumerable(items);
    }

    [Benchmark]
    public int for_each_readonlylist()
    {
        return SumReadOnlyList(items);
    }

    public static int SumEnumerable<T>(IEnumerable<T> enumerable)
    {
        var sum = 0;
        foreach (var i in items)
        {
            sum += i;
        }
        return sum;
    }

    public static int SumReadOnlyList<T>(IReadOnlyList<T> list)
    {
        var sum = 0;
        for (var i = 0; i < items.Count; i++)
        {
            sum += items[i];
        }
        return sum;
    }
}