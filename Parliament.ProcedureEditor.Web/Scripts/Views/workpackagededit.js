requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function (workPackaged) {
            var self = this;
            if (workPackaged === null)
                self.workPackaged = {
                    Id: null,
                    TripleStoreId: null,
                    WebLink: null,
                    ProcedureWorkPackageTripleStoreId: null,
                    ProcedureId: null,

                    StatutoryInstrumentNumber: null,
                    StatutoryInstrumentNumberPrefix: null,
                    StatutoryInstrumentNumberYear: null,
                    ComingIntoForceNote: null,
                    ComingIntoForceDate: null,
                    MadeDate: null,

                    WorkPackagedThingName: null,
                    IsStatutoryInstrument: true
                };
            else
                self.workPackaged = workPackaged;

            self.isNotValidResponse = ko.observable(false);
            self.workPackagedThingName = ko.observable(self.workPackaged.WorkPackagedThingName || "");
            self.statutoryInstrumentNumber = ko.observable(self.workPackaged.StatutoryInstrumentNumber);
            self.statutoryInstrumentNumberPrefix = ko.observable(self.workPackaged.StatutoryInstrumentNumberPrefix);
            self.statutoryInstrumentNumberYear = ko.observable(self.workPackaged.StatutoryInstrumentNumberYear);
            self.comingIntoForceNote = ko.observable(self.workPackaged.ComingIntoForceNote);
            self.webLink = ko.observable(self.workPackaged.WebLink);
            self.comingIntoForceDate = ko.observable(self.workPackaged.ComingIntoForceDate);
            self.madeDate = ko.observable(self.workPackaged.MadeDate);
            self.procedureId = ko.observable(self.workPackaged.ProcedureId);
            self.isStatutoryInstrument = ko.observable(self.workPackaged.IsStatutoryInstrument);

            self.procedures = ko.observableArray([]);
            $.getJSON(window.urls.getProcedures, function (data) {
                self.procedures(data);
            });

            self.isDeletePopupVisible = ko.observable(false);
            self.warningText = "Are you sure you wish to delete " + self.workPackaged.TripleStoreId + " work package?";

            self.canSave = ko.computed(function () {
                return (self.workPackagedThingName().length > 0) &&
                    (self.procedureId() !== null) && (self.isStatutoryInstrument() !== null);
            });

            self.save = function () {
                if (Number.isNaN(Number.parseInt(self.workPackaged.Id)))
                    $.ajax(window.urls.addWorkPackaged, {
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
                            window.location = window.urls.showWorkPackagedList;
                        else
                            self.isNotValidResponse(true);
                    }).fail(function () {
                        self.isNotValidResponse(true);
                    });
                else
                    $.ajax(window.urls.updateWorkPackaged.replace("{id}", self.workPackaged.Id), {
                        method: "PUT",
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
                            window.location = window.urls.showWorkPackagedList;
                        else
                            self.isNotValidResponse(true);
                    }).fail(function () {
                        self.isNotValidResponse(true);
                    });
            };

            self.showDeletePopup = function () {
                self.isDeletePopupVisible(true);
            };

            self.deleteWorkPackaged = function () {
                $.ajax(window.urls.deleteWorkPackaged.replace("{id}", self.workPackaged.Id), {
                    method: "DELETE",
                    dataType: "json"
                }).done(function (data) {
                    self.isDeletePopupVisible(false);
                    if (data === true)
                        window.location = window.urls.showWorkPackagedList;
                    else
                        self.isNotValidResponse(true);
                }).fail(function () {
                    self.isDeletePopupVisible(false);
                    self.isNotValidResponse(true);
                });
            };
        };

        var workPackagedId = $("#workPackagedId").val();
        if (Number.isNaN(Number.parseInt(workPackagedId)) === false)
            $.getJSON(window.urls.getWorkPackaged.replace("{id}", workPackagedId), function (data) {
                var vm = new viewModel(data);
                ko.applyBindings(vm);
            });
        else {
            var vm = new viewModel(null);
            ko.applyBindings(vm);
        }
    });
});