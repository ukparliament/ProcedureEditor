requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function (treaty) {
            var self = this;
            self.treaty = treaty;
            self.workPackagedThingName = ko.observable(treaty.Title);
            self.treatyNumber = ko.observable(treaty.Number);
            self.treatyPrefix = ko.observable(treaty.Prefix);
            self.comingIntoForceNote = ko.observable(null);
            self.comingIntoForceDate = ko.observable(null);
            self.leadGovernmentOrganisationTripleStoreId = ko.observable(null);
            self.seriesCitation = ko.observable(treaty.Citation);
            self.seriesTreatyCitation = ko.observable(null);
            self.webLink = ko.observable(treaty.WebUrl);
            self.procedureWorkPackageableThingTypeId = ko.observable(null);
            self.procedureName = ko.observable(null);
            self.procedureId = ko.observable(null);
            self.seriesMembershipId = ko.observable(null);
            self.isTreatySeriesMembership = ko.observable(false);
            self.isNotValidResponse = ko.observable(false);
            self.warningText = "Are you sure you wish to delete '" + treaty.Title + "' record?";
            self.isBeingSaved = ko.observable(false);
            self.isDeletePopupVisible = ko.observable(false);

            $.getJSON(window.urls.getProcedures, function (data) {
                for (i = 0; i < data.length; i++)
                    if (data[i].TripleStoreId === "D00dsjR2") {
                        self.procedureId(data[i].Id);
                        self.procedureName(data[i].ProcedureName);
                        break;
                    }
            });

            self.showDeletePopup = function () {
                self.isDeletePopupVisible(true);
            };

            self.canSave = ko.computed(function () {
                return (self.workPackagedThingName().length > 0) &&
                    (self.procedureId() !== null) &&
                    (self.seriesMembershipId() !== null) &&
                    (self.isBeingSaved() === false);
            });

            self.save = function () {
                self.isBeingSaved(true);
                var seriesMemberships = [];
                if (self.seriesMembershipId() !== null)
                    seriesMemberships.push({
                        SeriesMembershipId: self.seriesMembershipId(),
                        Citation: self.seriesCitation()
                    });
                if (self.isTreatySeriesMembership() === true)
                    seriesMemberships.push({
                        SeriesMembershipId: 4,
                        Citation: self.seriesTreatyCitation()
                    });
                $.ajax(window.urls.addSolrTreaty.replace("{id}", self.treaty.Id), {
                    method: "POST",
                    dataType: "json",
                    data: {
                        WorkPackagedThingName: self.workPackagedThingName(),
                        StatutoryInstrumentNumber: self.treatyNumber(),
                        StatutoryInstrumentNumberPrefix: self.treatyPrefix(),
                        ComingIntoForceNote: self.comingIntoForceNote(),
                        WebLink: self.webLink(),
                        ComingIntoForceDate: self.comingIntoForceDate(),
                        LeadGovernmentOrganisationTripleStoreId: self.leadGovernmentOrganisationTripleStoreId(),
                        SeriesMemberships: seriesMemberships,
                        ProcedureId: self.procedureId()
                    }
                }).done(function (data) {
                    if (data === true)
                        window.location = window.urls.showSolrTreaties;
                    else {
                        self.isNotValidResponse(true);
                        self.isBeingSaved(false);
                    }
                }).fail(function () {
                    self.isNotValidResponse(true);
                    self.isBeingSaved(false);
                });
            };

            self.deleteTreaty = function () {
                $.ajax(window.urls.deleteSolrTreaty.replace("{id}", self.treaty.Id), {
                    method: "DELETE",
                    dataType: "json"
                }).done(function (data) {
                    self.isDeletePopupVisible(false);
                    if (data === true)
                        window.location = window.urls.showSolrTreaties;
                    else
                        self.isNotValidResponse(true);
                }).fail(function () {
                    self.isDeletePopupVisible(false);
                    self.isNotValidResponse(true);
                });
            };

        };

        var treatyId = $("#treatyId").val();
        if (Number.isNaN(Number.parseInt(treatyId)) === false)
            $.getJSON(window.urls.getSolrTreaty.replace("{id}", treatyId), function (data) {
                var vm = new viewModel(data);
                ko.applyBindings(vm);
            });
        else {
            var vm = new viewModel(null);
            ko.applyBindings(vm);
        }
    });
});