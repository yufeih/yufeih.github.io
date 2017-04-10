using BenchmarkDotNet.Running;

class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<ZeroCostAbstraction>();
    }
}