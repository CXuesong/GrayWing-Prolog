using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MwParserFromScratch.Nodes;

namespace Graywing
{
    internal static class EnWikiHelper
    {

        private static readonly string[] NoneExprs = { "", "*", "none", "unknown", "none known" };

        public static IDictionary<string, IList<string>> ParseFamily(Wikitext familyt, Wikitext familyl)
        {
            if (familyt == null) throw new ArgumentNullException(nameof(familyt));
            if (familyl == null) throw new ArgumentNullException(nameof(familyl));
            const int charsPerLine = 32;
            var keyRows = new List<string>();
            string curKeyRow = null;
            void KeysVisitor(Node node)
            {
                switch (node)
                {
                    case HtmlTag t:
                        if (string.Equals(t.Name, "br", StringComparison.OrdinalIgnoreCase))
                        {
                            keyRows.Add(curKeyRow);
                            curKeyRow = null;
                        }
                        return;
                    case PlainText pt:
                        curKeyRow += pt.Content;
                        return;
                }
                foreach (var c in node.EnumChildren()) KeysVisitor(c);
            }
            var valueRows = new List<IList<string>>();
            var curValueRow = new List<string>();
            var chars = 0;
            void ValuesVisitor(Node node)
            {
                switch (node)
                {
                    case HtmlTag t:
                        if (string.Equals(t.Name, "br", StringComparison.OrdinalIgnoreCase))
                        {
                            valueRows.Add(curValueRow);
                            curValueRow = new List<string>();
                            chars = 0;
                        }
                        return;
                    case WikiLink l:
                        var text = l.ToPlainText().Trim();
                        if (chars > 0 && chars + text.Length + 2 /* ", " */ > charsPerLine)
                        {
                            valueRows.Add(curValueRow);
                            curValueRow = new List<string>();
                            chars = 0;
                        }
                        curValueRow.Add(l.Target.ToPlainText().Trim());
                        chars += text.Length + 2;
                        return;
                }
                foreach (var c in node.EnumChildren()) ValuesVisitor(c);
            }
            KeysVisitor(familyt);
            keyRows.Add(curKeyRow);
            ValuesVisitor(familyl);
            valueRows.Add(curValueRow);
            return ParseFamily(keyRows, valueRows);
        }

        public static IDictionary<string, IList<string>> ParseFamily(IList<string> familyt, IList<IList<string>> familyl)
        {
            if (familyt == null) throw new ArgumentNullException(nameof(familyt));
            if (familyl == null) throw new ArgumentNullException(nameof(familyl));
            var dict = new Dictionary<string, IList<string>>();
            if (familyt.Count == 0 || familyl.Count == 0) return dict;
            //Debug.Assert(familyt.Count <= familyl.Count);     // Sometimes there are extra trailing <br />
            if (familyt.Count == 1)
            {
                // E.g. Brother: 
                var key = familyt[0].Trim().ToLowerInvariant();
                if (NoneExprs.Contains(key)) return dict;
            }
            var ti = 0;
            var li = 0;
            while (li < familyl.Count)
            {
                if (ti == familyt.Count && familyl[li].Count == 0)
                    break;
                var key = familyt[ti].Trim(' ', '\t', '\r', '\n', ':').ToLowerInvariant();
                ti++;
                var value = new List<string>();
                var lines = 1;
                // Estimate rows occupied by this field by looking at the caption
                while (ti < familyt.Count && string.IsNullOrWhiteSpace(familyt[ti]))
                {
                    ti++;
                    lines++;
                }
                while (li < familyl.Count && (ti >= familyt.Count || lines > 0))
                {
                    value.AddRange(familyl[li]);
                    li++;
                    lines--;
                }
                dict.Add(key, value);
            }
            return dict;
        }

        private static readonly Regex tomMatcher = new Regex(@"\b(tom|male)\b");

        private static readonly Regex sheCatMatcher = new Regex(@"\b(she-?cat|female)\b");

        public static bool? IsTom(Wikitext root)
        {
            var introLine = root.Lines.TakeWhile(l => !(l is Heading)).NonEmptyLines().FirstOrDefault();
            if (introLine == null) return null;
            var introContent = introLine.ToPlainText();
            if (tomMatcher.IsMatch(introContent)) return true;
            if (sheCatMatcher.IsMatch(introContent)) return false;
            return null;
        }

        public static IEnumerable<LineNode> NonEmptyLines(this IEnumerable<LineNode> lines)
        {
            return lines.Where(l => l.EnumChildren().OfType<PlainText>().Any(t => !string.IsNullOrWhiteSpace(t.Content)));
        }

    }
}
