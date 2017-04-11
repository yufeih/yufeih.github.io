using System.Diagnostics;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using LibGit2Sharp;

[MemoryDiagnoser]
public class GitShowExeVsLibGit
{
    [Benchmark]
    public string exe()
    {
        var psi = new ProcessStartInfo
        {
            FileName = "git.exe",
            Arguments = "show c90e807bd40bacd7e96672fb94679842ea220479:slides/functional-thinking/index.html",
            UseShellExecute = false,
            RedirectStandardOutput = true,
        };

        using (var process = Process.Start(psi))
        {
            return process.StandardOutput.ReadToEnd();
        }
    }

    [Benchmark]
    public string libgit()
    {
        using (var repo = new Repository(Repository.Discover(Directory.GetCurrentDirectory())))
        {
            var commit = repo.Lookup<Commit>("c90e807bd40bacd7e96672fb94679842ea220479");
            var blob = (Blob)commit.Tree["slides/functional-thinking/index.html"].Target;
            return blob.GetContentText();
        }
    }
}