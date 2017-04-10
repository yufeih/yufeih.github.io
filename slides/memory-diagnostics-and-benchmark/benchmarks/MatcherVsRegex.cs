using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;
using Microsoft.DocAsCode.MarkdownLite;

[MemoryDiagnoser]
public class MatcherVsRegex
{
    [Params(
        "### This is a heading",
        "This is not a heading")]
    public string Markdown { get; set; }

    private MarkdownParsingContext context;

    private static readonly MarkdownHeadingBlockRule html = new MarkdownHeadingBlockRule();

    [Setup]
    public void SetupData()
    {
        context = new MarkdownParsingContext(SourceInfo.Create(Markdown, "a.md"));
    }

    [Benchmark(Baseline = true)]
    public object regex()
    {
        return html.Heading.Match(context.CurrentMarkdown)?.Length;
    }

    [Benchmark]
    public object matcher()
    {
        return context.Match(html.HeadingMatcher)?.Length;
    }
}