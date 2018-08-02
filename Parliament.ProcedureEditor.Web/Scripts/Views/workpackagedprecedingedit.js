requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function (workPackagedPreceding) {
            var self = this;
            if (workPackagedPreceding === null)
                self.workPackagedPreceding = {
                    Id: null,
                    ProcedureProposedNegativeStatutoryInstrumentId: null,
                    ProcedureStatutoryInstrumentId: null
                };
            else
                self.workPackagedPreceding = workPackagedPreceding;

            self.isNotValidResponse = ko.observable(false);
            self.procedureProposedNegativeStatutoryInstrumentId = ko.observable(self.workPackagedPreceding.ProcedureProposedNegativeStatutoryInstrumentId);
            self.procedureStatutoryInstrumentId = ko.observable(self.workPackagedPreceding.ProcedureStatutoryInstrumentId);
            self.procedureProposedNegativeStatutoryInstruments = ko.observableArray([]);
            self.procedureStatutoryInstruments = ko.observableArray([]);

            $.getJSON(window.urls.getWorkPackagedList, function (data) {
                self.procedureStatutoryInstruments(
                    data.filter(function (wp) {
                        return wp.IsStatutoryInstrument === true;
                    }));
                self.procedureProposedNegativeStatutoryInstruments(
                    data.filter(function (wp) {
                        return wp.IsStatutoryInstrument === false;
                    }));
            });

            self.isDeletePopupVisible = ko.observable(false);
            self.warningText = "Are you sure you wish to delete this work package preceding?";

            self.canSave = ko.computed(function () {
                return (self.procedureProposedNegativeStatutoryInstrumentId() !== null) &&
                    (self.procedureStatutoryInstrumentId() !== null);
            });

            self.save = function () {
                if (Number.isNaN(Number.parseInt(self.workPackagedPreceding.Id)))
                    $.ajax(window.urls.addWorkPackagedPreceding, {
                        method: "POST",
                        dataType: "json",
                        data: {
                            ProcedureProposedNegativeStatutoryInstrumentId: self.procedureProposedNegativeStatutoryInstrumentId(),
                            ProcedureStatutoryInstrumentId: self.procedureStatutoryInstrumentId()
                        }
                    }).done(function (data) {
                        if (data === true)
                            window.location = window.urls.showWorkPackagedPrecedings;
                        else
                            self.isNotValidResponse(true);
                    }).fail(function () {
                        self.isNotValidResponse(true);
                    });
                else
                    $.ajax(window.urls.updateWorkPackagedPreceding.replace("{id}", self.workPackagedPreceding.Id), {
                        method: "PUT",
                        dataType: "json",
                        data: {
                            ProcedureProposedNegativeStatutoryInstrumentId: self.procedureProposedNegativeStatutoryInstrumentId(),
                            ProcedureStatutoryInstrumentId: self.procedureStatutoryInstrumentId()
                        }
                    }).done(function (data) {
                        if (data === true)
                            window.location = window.urls.showWorkPackagedPrecedings;
                        else
                            self.isNotValidResponse(true);
                    }).fail(function () {
                        self.isNotValidResponse(true);
                    });
            };

            self.showDeletePopup = function () {
                self.isDeletePopupVisible(true);
            };

            self.deleteWorkPackagedPreceding = function () {
                $.ajax(window.urls.deleteWorkPackagedPreceding.replace("{id}", self.workPackagedPreceding.Id), {
                    method: "DELETE",
                    dataType: "json"
                }).done(function (data) {
                    self.isDeletePopupVisible(false);
                    if (data === true)
                        window.location = window.urls.showWorkPackagedPrecedings;
                    else
                        self.isNotValidResponse(true);
                }).fail(function () {
                    self.isDeletePopupVisible(false);
                    self.isNotValidResponse(true);
                });
            };
        };

        var workPackagedPrecedingId = $("#workPackagedPrecedingId").val();
        if (Number.isNaN(Number.parseInt(workPackagedPrecedingId)) === false)
            $.getJSON(window.urls.getWorkPackagedPreceding.replace("{id}", workPackagedPrecedingId), function (data) {
                var vm = new viewModel(data);
                ko.applyBindings(vm);
            });
        else {
            var vm = new viewModel(null);
            ko.applyBindings(vm);
        }
    });
});