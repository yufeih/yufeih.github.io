using BenchmarkDotNet.Running;

class Program
{
    public static void Main(string[] args)
    {
        //OlLinqLeak.Run();
        //OlLinqHang.Run();
        DocsLinqLeak.Run();

        //BenchmarkRunner.Run<ZeroCostAbstraction>();
    }
}