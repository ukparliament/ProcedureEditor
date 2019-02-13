namespace Parliament.ProcedureEditor.Web.Models
{
    public class SolrTreaty
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TreatyPrefix { get; set; }
        public int? TreatyNumber { get; set; }
        public string WebUrl { get; set; }
        public string TripleStoreId { get; set; }
    }
}