using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;

namespace PF2eSearchApp
{
    public class Program
    {
        private static string root = "https://elasticsearch.aonprd.com/";
        private static string index = "aon";
        private static List<string> targets = [
            "action",
            "ancestry",
            "archetype",
            "armor",
            "article",
            "background",
            "class",
            "creature",
            "creature-family",
            "deity",
            "equipment",
            "feat",
            "hazard",
            "rules",
            "skill",
            "shield",
            "spell",
            "source",
            "trait",
            "weapon",
            "weapon-group"];


        public static async Task Main(string[] args)
        {
            ElasticsearchClient client = new ElasticsearchClient(new Uri(root));

            var searchRequest = new SearchRequest<string>(index)
            {
                Query = new MultiMatchQuery
                {
                    Query = "feat"
                },
                From = 0,
                Size = 1000
            };
            var test = await client.SearchAsync<Ancestry>(index);

            List<Ancestry> ancestries = new List<Ancestry>();
            foreach (var hit in test.Hits)
            {
                ancestries.Add(hit.Source);
            }
        }
    }

    public class Navigation
    {
        public string label { get; set; }
        public string url { get; set; }
    }

    public class Resistance
    {
    }

    public class Ancestry
    {
        public List<string> ability { get; set; }
        public List<string> ability_flaw { get; set; }
        public List<object> breadcrumbs_spa { get; set; }
        public string category { get; set; }
        public bool exclude_from_search { get; set; }
        public int hp { get; set; }
        public string hp_raw { get; set; }
        public string id { get; set; }
        public List<string> image { get; set; }
        public List<string> language { get; set; }
        public string language_markdown { get; set; }
        public string markdown { get; set; }
        public string name { get; set; }
        public List<Navigation> navigation { get; set; }
        public string pfs { get; set; }
        public string rarity { get; set; }
        public string release_date { get; set; }
        public Resistance resistance { get; set; }
        public string search_markdown { get; set; }
        public List<string> size { get; set; }
        public List<string> source { get; set; }
        public List<string> source_raw { get; set; }
        public string source_category { get; set; }
        public string source_markdown { get; set; }
        public Speed speed { get; set; }
        public string speed_markdown { get; set; }
        public string speed_raw { get; set; }
        public string summary { get; set; }
        public string summary_markdown { get; set; }
        public string text { get; set; }
        public List<string> trait { get; set; }
        public string trait_markdown { get; set; }
        public List<string> trait_raw { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string vision { get; set; }
        public Weakness weakness { get; set; }
    }

    public class Speed
    {
        public int land { get; set; }
        public int max { get; set; }
    }

    public class Weakness
    {
    }
}