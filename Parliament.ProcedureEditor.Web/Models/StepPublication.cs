using System.Collections.Generic;

namespace Parliament.ProcedureEditor.Web.Models
{
    public class StepPublication
    {
        public int Id { get; set; }
        public string TripleStoreId { get; set; }
        public string PublicationName { get; set; }
        public string PublicationUrl { get; set; }
    }
}