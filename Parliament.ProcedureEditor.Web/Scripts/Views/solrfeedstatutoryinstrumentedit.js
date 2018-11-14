requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function (statutoryInstrument) {
            var self = this;
            self.statutoryInstrument = statutoryInstrument;
            self.workPackagedThingName = ko.observable(statutoryInstrument.Title);
            self.statutoryInstrumentNumber = ko.observable(statutoryInstrument.SINumber);
            self.statutoryInstrumentNumberPrefix = ko.observable(statutoryInstrument.SIPrefix);
            self.statutoryInstrumentNumberYear = ko.observable(null);
            self.comingIntoForceNote = ko.observable(statutoryInstrument.ComingIntoForceNote);
            self.comingIntoForceDate = ko.observable(statutoryInstrument.ComingIntoForceDate);
            self.madeDate = ko.observable(statutoryInstrument.MadeDate);
            self.webLink = ko.observable(statutoryInstrument.WebUrl);
            self.procedureWorkPackageableThingTypeId = ko.observable(null);
            self.comingIntoForceDate = ko.observable(statutoryInstrument.ComingIntoForceDate);
            self.timeLimitForObjectionEndDate = ko.observable(null);
            self.procedureId = ko.observable(null);
            self.isNotValidResponse = ko.observable(false);
            self.warningText = "Are you sure you wish to delete '" + statutoryInstrument.Title + "' record?";
            self.isBeingSaved = ko.observable(false);
            self.isDeletePopupVisible = ko.observable(false);
            self.proposedNegativeStatutoryInstrumentId = ko.observable(null);

            self.procedures = ko.observableArray([]);
            $.getJSON(window.urls.getProcedures, function (data) {
                self.procedures(data);
                var procedureDictionary = {};
                for (i = 0; i < data.length; i++)
                    procedureDictionary[data[i].TripleStoreId] = data[i].Id;
                self.proposedNegativeStatutoryInstrumentId(procedureDictionary["iCdMN1MW"]);
                if (self.statutoryInstrument.IsStatutoryInstrument === false)
                    self.procedureId(self.proposedNegativeStatutoryInstrumentId());
                else
                    if (self.madeDate() === null) {
                        if (self.statutoryInstrument.SIProcedure.toLowerCase().indexOf("negative") >= 0)
                            self.procedureId(procedureDictionary["gTgidljI"]);
                        else
                            if (self.statutoryInstrument.SIProcedure.toLowerCase().indexOf("affirmative") >= 0)
                                self.procedureId(procedureDictionary["H5YJQsK2"]);
                    }
                    else {
                        if (self.statutoryInstrument.SIProcedure.toLowerCase().indexOf("negative") >= 0)
                            self.procedureId(procedureDictionary["5S6p4YsP"]);
                        else
                            if (self.statutoryInstrument.SIProcedure.toLowerCase().indexOf("affirmative") >= 0)
                                self.procedureId(procedureDictionary["iWugpxMn"]);
                    }
            });

            self.isStatutoryInstrument = ko.computed(function () {
                return self.procedureId() !== self.proposedNegativeStatutoryInstrumentId();
            });

            self.showDeletePopup = function () {
                self.isDeletePopupVisible(true);
            };

            self.canSave = ko.computed(function () {
                return (self.workPackagedThingName().length > 0) &&
                    (self.procedureId() !== null) &&
                    (self.isBeingSaved() === false);
            });

            self.save = function () {
                self.isBeingSaved(true);
                $.ajax(window.urls.addSolrStatutoryInstrument.replace("{id}", self.statutoryInstrument.Id), {
                    method: "POST",
                    dataType: "json",
                    data: {
                        WorkPackagedThingName: self.workPackagedThingName(),
                        StatutoryInstrumentNumber: self.statutoryInstrumentNumber(),
                        StatutoryInstrumentNumberPrefix: self.statutoryInstrumentNumberPrefix(),
                        StatutoryInstrumentNumberYear: self.statutoryInstrumentNumberYear(),
                        ComingIntoForceNote: self.comingIntoForceNote(),
                        WebLink: self.webLink(),
                        ComingIntoForceDate: self.comingIntoForceDate(),
                        MadeDate: self.madeDate(),
                        ProcedureId: self.procedureId(),
                        IsStatutoryInstrument: self.isStatutoryInstrument()
                    }
                }).done(function (data) {
                    if (data === true)
                        window.location = window.urls.showSolrStatutoryInstruments;
                    else {
                        self.isNotValidResponse(true);
                        self.isBeingSaved(false);
                    }
                }).fail(function () {
                    self.isNotValidResponse(true);
                    self.isBeingSaved(false);
                });
            };

            self.deleteStatutoryInstrument = function () {
                $.ajax(window.urls.deleteSolrStatutoryInstrument.replace("{id}", self.statutoryInstrument.Id), {
                    method: "DELETE",
                    dataType: "json"
                }).done(function (data) {
                    self.isDeletePopupVisible(false);
                    if (data === true)
                        window.location = window.urls.showSolrStatutoryInstruments;
                    else
                        self.isNotValidResponse(true);
                }).fail(function () {
                    self.isDeletePopupVisible(false);
                    self.isNotValidResponse(true);
                });
            };

        };

        var statutoryInstrumentId = $("#statutoryInstrumentId").val();
        if (Number.isNaN(Number.parseInt(statutoryInstrumentId)) === false)
            $.getJSON(window.urls.getSolrStatutoryInstrument.replace("{id}", statutoryInstrumentId), function (data) {
                var vm = new viewModel(data);
                ko.applyBindings(vm);
            });
        else {
            var vm = new viewModel(null);
            ko.applyBindings(vm);
        }
    });
});