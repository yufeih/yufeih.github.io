using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;
using System.Threading;

public class StaticVsThreadLocalVsAsyncLocal
{
    static int _static;
    static ThreadLocal<int> _threadLocal = new ThreadLocal<int>();
    static AsyncLocal<int> _asyncLocal = new AsyncLocal<int>();

    [Benchmark(Baseline = true)]
    public int plain_static() => _static++;

    [Benchmark]
    public int thread_local() => _threadLocal.Value++;

    [Benchmark]
    public int async_local() => _asyncLocal.Value++;
}