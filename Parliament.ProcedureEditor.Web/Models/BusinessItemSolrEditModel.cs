using System;

namespace Parliament.ProcedureEditor.Web.Models
{
    public class BusinessItemSolrEditModel
    {
        public string WebLink { get; set; }
        public int ProcedureWorkPackageId { get; set; }
        public DateTimeOffset? BusinessItemDate { get; set; }
        public DateTimeOffset? LayingDate { get; set; }
        public int StepId { get; set; }
        public int? LayingBodyId { get; set; }
        public bool IsLaid { get; set; }
    }
}