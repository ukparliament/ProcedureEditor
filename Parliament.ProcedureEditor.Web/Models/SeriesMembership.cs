using System;

namespace Parliament.ProcedureEditor.Web.Models
{
    public class SeriesMembership
    {
        public int Id { get; set; }
        public string TripleStoreId { get; set; }
        public string Citation { get; set; }
        public int SeriesMembershipId { get; set; }
        public SeriesMembershipType SeriesMembershipKind
        {
            get
            {
                return (SeriesMembershipType)Enum.ToObject(typeof(SeriesMembershipType), SeriesMembershipId);
            }
        }
    }        
}