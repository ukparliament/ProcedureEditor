using System;
using System.Collections.Generic;

namespace Parliament.ProcedureEditor.Web.Models
{
    public class BusinessItem
    {
        public int Id { get; set; }
        public string TripleStoreId { get; set; }
        public string WebLink { get; set; }
        public int ProcedureWorkPackageId { get; set; }        
        public DateTimeOffset? BusinessItemDate { get; set; }
        public IEnumerable<int> Steps { get; set; }

        public string WorkPackagedThingName { get; set; }
        public int ProcedureId { get; set; }
        public string ProcedureName { get; set; }
        public int[] ProcedureWorkPackages { get; set; }
    }
}