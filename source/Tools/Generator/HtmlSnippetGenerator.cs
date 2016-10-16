﻿using System.Collections.Generic;
using System.IO;

namespace Pihrtsoft.Snippets.CodeGeneration
{
    public static class HtmlSnippetGenerator
    {
        private const string AttributeIdentifier = "attribute";
        private const string CommentIdentifier = "comment";
        private const string ContentIdentifier = "content";
        private const string NameIdentifier = "name";
        private const string EndIdentifier = "end";

        public static IEnumerable<Snippet> GenerateSnippets(string sourceDirectoryPath)
        {
            foreach (Snippet snippet in GenerateSnippets(SnippetSerializer.Deserialize(sourceDirectoryPath, SearchOption.AllDirectories)))
            {
                snippet.AddTag(KnownTags.AutoGenerated);
                snippet.SortCollections();
                yield return snippet;
            }
        }

        public static IEnumerable<Snippet> GenerateSnippets(IEnumerable<Snippet> snippets)
        {
            foreach (Snippet snippet in snippets)
            {
                Literal literal = snippet.Literals.Find("content");

                if (literal != null)
                {
                    yield return WithoutContent((Snippet)snippet.Clone());

                    snippet.SuffixTitle(" (with content)");
                    snippet.SuffixDescription(" (with content)");
                    snippet.SuffixShortcut(XmlSnippetGenerator.ContentShortuct);
                    snippet.SuffixFileName("_with_content");
                    snippet.AddTag(KnownTags.ExcludeFromReadme);

                    yield return snippet;
                }
                else
                {
                    yield return snippet;
                }
            }
        }

        private static Snippet WithoutContent(Snippet snippet)
        {
            snippet.ReplacePlaceholders("end", "");
            snippet.ReplacePlaceholders("content", "$end$");
            snippet.RemoveLiteral("content");

            return snippet;
        }
    }
}
