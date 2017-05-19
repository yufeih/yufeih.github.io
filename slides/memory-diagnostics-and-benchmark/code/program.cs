using BenchmarkDotNet.Running;

class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<JilSerializeDynamicVsGenericSpecialization>();

        //OlLinqLeak.Run();
        //OlLinqHang.Run();
        //DocsLogLeak.Run();
    } 
}