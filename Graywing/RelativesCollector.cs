using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MwParserFromScratch;
using MwParserFromScratch.Nodes;
using WikiClientLibrary.Client;
using WikiClientLibrary.Generators;
using WikiClientLibrary.Pages;
using WikiClientLibrary.Sites;
using static Graywing.PrologHelper;

namespace Graywing
{
    public class RelativesCollector
    {

        private readonly WikiClient wikiClient = new WikiClient
        {
            ClientUserAgent = "GrayWing/1.0",
            Logger = WikiLogger.Default
        };

        private readonly WikitextParser wikiParser = new WikitextParser();

        public RelativesCollector()
        {

        }

        public string EndpointUrl { get; set; } = "http://warriors.wikia.com/api.php";

        public async Task CollectAsync(string outputPath)
        {
            var site = await WikiSite.CreateAsync(wikiClient, EndpointUrl);
            site.Logger = WikiLogger.Default;
            var gen = new CategoryMembersGenerator(site, "Characters")
            {
                PagingSize = 50,
                MemberTypes = CategoryMemberTypes.Page,
            };
            var counter = 0;
            using (var writer = File.CreateText(outputPath))
            {
                writer.WriteLine("% Powered by Graywing Prolog Generator.");
                writer.WriteLine("% Generated from Warriors Wiki on {0:O}.", DateTime.Now);
                writer.WriteLine();
                writer.WriteLine(":- discontiguous[name/2, male/1, female/1, belongsto/2, child/3, apprentice/2].");
                writer.WriteLine();
                await gen.EnumPagesAsync(PageQueryOptions.FetchContent)
                    //.Take(100)
                    .ForEachAsync(page =>
                    {
                        counter++;
                        Console.WriteLine("{0}: {1}", counter, page.Title);
                        writer.Write("% ");
                        writer.WriteLine(page.Title);
                        foreach (var l in FactsFromPage(page))
                        {
                            writer.WriteLine(l);
                        }
                        writer.WriteLine();
                        writer.Flush();
                    });
            }
        }

        private static string StripDabTitle(string fullTitle)
        {
            var parts = fullTitle.Split('(', 2);
            return parts[0].Trim();
        }

        public IEnumerable<string> FactsFromPage(WikiPage page)
        {
            var root = wikiParser.Parse(page.Content);
            var infoboxCat = root.EnumDescendants().OfType<Template>()
                .FirstOrDefault(t => MwParserUtility.NormalizeTitle(t.Name) == "Charcat");
            if (infoboxCat == null)
            {
                Console.WriteLine("No {{Charcat}} found.");
                yield return "% No {{Charcat}} found.";
                yield break;
            }
            var atom = AtomExpr(page.Title.Trim());
            yield return $"name({atom}, \"{StripDabTitle(page.Title)}\").";
            switch (EnWikiHelper.IsTom(root))
            {
                case true:
                    yield return $"male({atom}).";
                    break;
                case false:
                    yield return $"female({atom}).";
                    break;
                default:
                    break;
            }
            var affie = infoboxCat.Arguments["affie"];
            if (affie != null)
            {
                foreach (var aff in affie.EnumDescendants().OfType<WikiLink>())
                    yield return $"belongsto({atom}, {AtomExpr(aff.Target.ToPlainText().Trim())}).";
            }
            var familyt = infoboxCat.Arguments["familyt"];
            var familyl = infoboxCat.Arguments["familyl"];
            if (familyt != null && familyl != null)
            {
                var familyDict = EnWikiHelper.ParseFamily(familyt.Value, familyl.Value);
                familyDict.TryGetValue("mother", out var mothers);
                familyDict.TryGetValue("father", out var fathers);
                var mother = mothers?.FirstOrDefault();
                var father = fathers?.FirstOrDefault();
                if (mother != null || father != null)
                {
                    Debug.Assert(mothers == null || mothers.Count <= 1);
                    Debug.Assert(fathers == null || fathers.Count <= 1);
                    yield return $"child({atom}, {AtomExpr(father ?? "x")}, {AtomExpr(mother ?? "x")}).";
                }
                else
                {
                    yield return "% No parent found.";
                }
            }
            else
            {
                Console.WriteLine("No familyt/familyl.");
                yield return "% No {{Charcat |familyl= |familyt= }} found.";
            }
            var apps = infoboxCat.Arguments["mentor"];
            if (apps != null)
            {
                foreach (var appr in apps.EnumDescendants().OfType<WikiLink>())
                    yield return $"apprentice({atom}, {AtomExpr(appr.Target.ToPlainText().Trim())}).";
            }
        }

    }
}
