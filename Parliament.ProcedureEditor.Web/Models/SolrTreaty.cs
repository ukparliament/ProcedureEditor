namespace Parliament.ProcedureEditor.Web.Models
{
    public class SolrTreaty
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Prefix { get; set; }
        public int? Number { get; set; }
        public string WebUrl { get; set; }
        public string Citation { get; set; }
        public string TripleStoreId { get; set; }
    }
}