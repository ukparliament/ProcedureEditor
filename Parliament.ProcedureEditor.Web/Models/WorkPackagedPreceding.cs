namespace Parliament.ProcedureEditor.Web.Models
{
    public class WorkPackagedPreceding
    {
        public int Id { get; set; }
        public int ProcedureProposedNegativeStatutoryInstrumentId { get; set; }
        public int ProcedureStatutoryInstrumentId { get; set; }

        public string ProcedureProposedNegativeStatutoryInstrumentName { get; set; }
        public string ProcedureStatutoryInstrumentName { get; set; }
    }
}