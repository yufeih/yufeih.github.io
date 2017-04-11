using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Markdig;
using Microsoft.DocAsCode.Build.Engine;
using Microsoft.DocAsCode.Plugins;

[MemoryDiagnoser]
public class DocfxMarkdownNewVsOldVsMarkdig
{
    private string _markdown;
    private IMarkdownService _new;
    private IMarkdownService _old;

    [Setup]
    public void SetupData()
    {
        _markdown = File.ReadAllText("benchmark/markdown.md");
        _new = new DfmServiceProvider().CreateMarkdownService(new MarkdownServiceParameters());
        _old = new DfmLegacyServiceProvider().CreateMarkdownService(new MarkdownServiceParameters());
    }

    [Benchmark]
    public object docfx_new() => _new.Markup(_markdown, "");

    [Benchmark]
    public object docfx_old() => _old.Markup(_markdown, "");

    [Benchmark(Baseline = true)]
    public object markdig() => Markdown.ToHtml(_markdown);
}