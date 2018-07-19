namespace Parliament.ProcedureEditor.Web.Models
{
    public class StepHouse
    {
        public int Id { get; set; }
        public int ProcedureStepId { get; set; }
        public int HouseId { get; set; }

        public string HouseName { get; set; }
    }
}