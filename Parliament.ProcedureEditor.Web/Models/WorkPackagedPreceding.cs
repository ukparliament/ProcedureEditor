namespace Parliament.ProcedureEditor.Web.Models
{
    public class WorkPackagedPreceding
    {
        public int Id { get; set; }
        public int WorkPackagedIsFollowedById { get; set; }
        public int WorkPackagedIsPrecededById { get; set; }

        public string WorkPackagedIsFollowedByName { get; set; }
        public string WorkPackagedIsPrecededByName { get; set; }
    }
}