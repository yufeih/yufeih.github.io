using System;
using System.Linq;
using System.Collections.Generic;

static class OlLinqHang
{
    public static object Run()
    {
        var files = new FileTranslationResponse[10 * 1000];

        var allFiles = files.SelectMany(
            file => Enumerable
                .Range(0, 50)
                .Select(i => new FileTranslationResponse("a", "b", 1, "c", null, new string[2])));

        return allFiles.Union(allFiles).ToList();
    }

    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        HashSet<TKey> seenKeys = new HashSet<TKey>();
        foreach (TSource element in source)
        {
            if (seenKeys.Add(keySelector(element)))
            {
                yield return element;
            }
        }
    }

    public struct FileTranslationResponse
    {
        public readonly string TranslationType;
        public readonly string SourceHash;
        public readonly string Locale;

        public readonly IReadOnlyList<string> LocFileIds;
        public readonly string SourcePath;
        public readonly string SourceVersion;
        public readonly string Correlation;
        public readonly DateTime LastUpdatedAt;
        public readonly int RevisionNumber;
        public readonly Exception Exception;

        public FileTranslationResponse(
            string sourceHash,
            string locale,
            int revisionNumber,
            string sourcePath,
            string translationType = null,
            IReadOnlyList<string> locFileIds = null,
            Exception exception = null,
            string sourceVersion = null,
            string correlation = null,
            DateTime lastUpdatedAt = default(DateTime))
        {
            Locale = locale;
            SourceHash = sourceHash;
            TranslationType = translationType;
            LocFileIds = locFileIds;
            LastUpdatedAt = lastUpdatedAt;
            Correlation = correlation;
            SourcePath = sourcePath;
            SourceVersion = sourceVersion;
            RevisionNumber = revisionNumber;
            Exception = exception;
        }
    }
}