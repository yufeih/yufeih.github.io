using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog;

class DocsLogLeak
{
    public static void Run()
    {
        var scopes = new BlockingCollection<(IDisposable, TaskCompletionSource<int>)>();

        new Thread(_ =>
        {
            while (true)
            {
                var (scope, tcs) = scopes.Take();
                scope.Dispose();
                tcs.TrySetResult(0);
            }

        }).Start();

        var loggerFactory = new LoggerFactory();
        loggerFactory.AddSerilog();

        var logger = loggerFactory.CreateLogger("category");
        var tasks = new List<Task>();

        for (var i = 0; i < 100; i++)
        {
            var scope = logger.BeginScope("scope name");
            var tcs = new TaskCompletionSource<int>();

            logger.LogInformation("begin context");

            scopes.Add((scope, tcs));
            tasks.Add(tcs.Task);
        }

        Task.WaitAll(tasks.ToArray());
    }
}