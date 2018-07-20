namespace Parliament.ProcedureEditor.Web.Models
{
    public class WorkPackageablePreceding
    {
        public int Id { get; set; }
        public int PrecedingProcedureWorkPackageableThingId { get; set; }
        public int FollowingProcedureWorkPackageableThingId { get; set; }

        public string PrecedingProcedureWorkPackageableThingName { get; set; }
        public string FollowingProcedureWorkPackageableThingName { get; set; }
    }
}