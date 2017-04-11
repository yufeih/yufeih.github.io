using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;
using System;

[MemoryDiagnoser]
public class DisposeStructVsClass
{
    static int i = 0;

    [Benchmark(Baseline = true)]
    public void struct_dispose()
    {
        using (new StructDisposable())
        {
            i++;
        }
    }

    [Benchmark]
    public void class_dispose()
    {
        using (new ClassDisposable())
        {
            i++;
        }
    }

    struct StructDisposable : IDisposable
    {
        public string Name;
        public long StartTicks;

        public void Dispose() { }
    }

    class ClassDisposable : IDisposable
    {
        public string Name;
        public long StartTicks;

        public void Dispose() { }
    }
}