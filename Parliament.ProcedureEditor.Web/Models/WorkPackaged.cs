﻿using System;
using System.Linq;

namespace Parliament.ProcedureEditor.Web.Models
{
    public class WorkPackaged
    {
        public int Id { get; set; }
        public string TripleStoreId { get; set; }
        public string WebLink { get; set; }
        public string ProcedureWorkPackageTripleStoreId { get; set; }
        public int ProcedureId { get; set; }

        public int? StatutoryInstrumentNumber { get; set; }
        public string StatutoryInstrumentNumberPrefix { get; set; }
        public int? StatutoryInstrumentNumberYear { get; set; }
        public string ComingIntoForceNote { get; set; }
        public DateTimeOffset? ComingIntoForceDate { get; set; }
        public DateTimeOffset? MadeDate { get; set; }

        public string WorkPackagedThingName { get; set; }
        public WorkPackagedType WorkPackagedKind { get; set; }
        
        public string ProcedureName { get; set; }
        public DateTimeOffset? MostRecentBusinessItemDate { get; set; }

        public string LeadGovernmentOrganisationTripleStoreId { get; set; }
        public SeriesMembership[] SeriesMemberships { get; set; }

        public SeriesMembership NonTreatySeriesMembership
        {
            get
            {
                return SeriesMemberships
                    ?.Where(s => s.SeriesMembershipKind != SeriesMembershipType.Treaty)
                    ?.SingleOrDefault();
            }
        }

        public SeriesMembership TreatySeriesMembership
        {
            get
            {
                return SeriesMemberships
                    ?.Where(s => s.SeriesMembershipKind == SeriesMembershipType.Treaty)
                    ?.SingleOrDefault();
            }
        }

    }
}