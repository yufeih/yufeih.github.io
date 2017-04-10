using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class OlLinqLeak
{
    public static void Run()
    {
        var commits = new string[100 * 1000];
        var result = new ConcurrentBag<List<string>>();

        Parallel.For(0, 100, i =>
        {
            result.Add(commits.ToList());
        });
    }
}