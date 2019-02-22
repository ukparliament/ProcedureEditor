using System.Collections.Generic;

namespace Parliament.ProcedureEditor.Web.Models
{
    public class WorkPackagedTreaty
    {
        public int Id { get; set; }
        public string Citation { get; set; }
        public bool IsCountrySeriesMembership { get; set; }
        public bool IsEuropeanUnionSeriesMembership { get; set; }
        public bool IsMiscellaneousSeriesMembership { get; set; }
        public bool IsTreatySeriesMembership { get; set; }

        public int[] SeriesMembershipIds
        {
            get
            {
                List<int> ids=new List<int>();
                if (IsCountrySeriesMembership)
                    ids.Add((int)SeriesMembershipType.Country);
                if (IsEuropeanUnionSeriesMembership)
                    ids.Add((int)SeriesMembershipType.EuropeanUnion);
                if (IsMiscellaneousSeriesMembership)
                    ids.Add((int)SeriesMembershipType.Miscellaneous);
                if (IsTreatySeriesMembership)
                    ids.Add((int)SeriesMembershipType.Treaty);
                return ids.ToArray();
            }
        }
    }
}