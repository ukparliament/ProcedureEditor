﻿requirejs(["/Scripts/main.js"], function (main) {
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
            self.isStatutoryInstrument = ko.observable(null);
            self.procedureWorkPackageableThingTypeId = ko.observable(null);
            self.comingIntoForceDate = ko.observable(statutoryInstrument.ComingIntoForceDate);
            self.timeLimitForObjectionEndDate = ko.observable(null);
            self.procedureId = ko.observable(null)
            self.isNotValidResponse = ko.observable(false);
            self.warningText = "Are you sure you wish to delete '" + statutoryInstrument.Title + "' record?";
            self.isDeletePopupVisible = ko.observable(false);

            self.procedures = ko.observableArray([]);
            $.getJSON(window.urls.getProcedures, function (data) {
                self.procedures(data);
            });

            self.showDeletePopup = function () {
                self.isDeletePopupVisible(true);
            };
            
            self.canSave = ko.computed(function () {
                return (self.workPackagedThingName().length > 0) &&
                    (self.procedureId() !== null);
            });

            self.save = function () {
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
                    else
                        self.isNotValidResponse(true);
                }).fail(function () {
                    self.isNotValidResponse(true);
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