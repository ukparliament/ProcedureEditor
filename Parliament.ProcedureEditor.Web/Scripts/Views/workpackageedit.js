requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function (workPackageable) {
            var self = this;
            if (workPackageable === null)
                self.workPackageable = {
                    Id: null,
                    TripleStoreId: null,
                    ProcedureWorkPackageableThingName: null,
                    StatutoryInstrumentNumber: null,
                    StatutoryInstrumentNumberPrefix: null,
                    StatutoryInstrumentNumberYear: null,
                    ComingIntoForceNote: null,
                    WebLink: null,
                    ProcedureWorkPackageableThingTypeId: null,
                    ComingIntoForceDate: null,
                    TimeLimitForObjectionEndDate: null,
                    MadeDate: null,
                    WithdrawalDate: null,
                    ProcedureWorkPackageTripleStoreId: null,
                    ProcedureId: null
                };
            else
                self.workPackageable = workPackageable;

            self.isNotValidResponse = ko.observable(false);
            self.procedureWorkPackageableThingName = ko.observable(self.workPackageable.ProcedureWorkPackageableThingName || "");
            self.statutoryInstrumentNumber = ko.observable(self.workPackageable.StatutoryInstrumentNumber);
            self.statutoryInstrumentNumberPrefix = ko.observable(self.workPackageable.StatutoryInstrumentNumberPrefix);
            self.statutoryInstrumentNumberYear = ko.observable(self.workPackageable.StatutoryInstrumentNumberYear);
            self.comingIntoForceNote = ko.observable(self.workPackageable.ComingIntoForceNote);
            self.webLink = ko.observable(self.workPackageable.WebLink);
            self.procedureWorkPackageableThingTypeId = ko.observable(self.workPackageable.ProcedureWorkPackageableThingTypeId);
            self.comingIntoForceDate = ko.observable(self.workPackageable.ComingIntoForceDate);
            self.timeLimitForObjectionEndDate = ko.observable(self.workPackageable.TimeLimitForObjectionEndDate);
            self.madeDate = ko.observable(self.workPackageable.MadeDate);
            self.withdrawalDate = ko.observable(self.workPackageable.WithdrawalDate);
            self.procedureId = ko.observable(self.workPackageable.ProcedureId);

            self.procedures = ko.observableArray([]);
            $.getJSON(window.urls.getProcedures, function (data) {
                self.procedures(data);
            });

            self.types = ko.observableArray([]);
            $.getJSON(window.urls.getWorkPackageTypes, function (data) {
                self.types(data);
            });

            self.isDeletePopupVisible = ko.observable(false);
            self.warningText = "Are you sure you wish to delete " + self.workPackageable.TripleStoreId + " work package?";

            self.canSave = ko.computed(function () {
                return (self.procedureWorkPackageableThingName().length > 0) &&
                    (self.procedureId() !== null);
            });

            self.save = function () {
                if (Number.isNaN(Number.parseInt(self.workPackageable.Id)))
                    $.ajax(window.urls.addWorkPackage, {
                        method: "POST",
                        dataType: "json",
                        data: {
                            ProcedureWorkPackageableThingName: self.procedureWorkPackageableThingName(),
                            StatutoryInstrumentNumber: self.statutoryInstrumentNumber(),
                            StatutoryInstrumentNumberPrefix: self.statutoryInstrumentNumberPrefix(),
                            StatutoryInstrumentNumberYear: self.statutoryInstrumentNumberYear(),
                            ComingIntoForceNote: self.comingIntoForceNote(),
                            WebLink: self.webLink(),
                            ProcedureWorkPackageableThingTypeId: self.procedureWorkPackageableThingTypeId(),
                            ComingIntoForceDate: self.comingIntoForceDate(),
                            TimeLimitForObjectionEndDate: self.timeLimitForObjectionEndDate(),
                            ProcedureId: self.procedureId(),
                            MadeDate: self.madeDate(),
                            WithdrawalDate: self.withdrawalDate()
                        }
                    }).done(function (data) {
                        if (data === true)
                            window.location = window.urls.showWorkPackages;
                        else
                            self.isNotValidResponse(true);
                    }).fail(function () {
                        self.isNotValidResponse(true);
                    });
                else
                    $.ajax(window.urls.updateWorkPackage.replace("{id}", self.workPackageable.Id), {
                        method: "PUT",
                        dataType: "json",
                        data: {
                            ProcedureWorkPackageableThingName: self.procedureWorkPackageableThingName(),
                            StatutoryInstrumentNumber: self.statutoryInstrumentNumber(),
                            StatutoryInstrumentNumberPrefix: self.statutoryInstrumentNumberPrefix(),
                            StatutoryInstrumentNumberYear: self.statutoryInstrumentNumberYear(),
                            ComingIntoForceNote: self.comingIntoForceNote(),
                            WebLink: self.webLink(),
                            ProcedureWorkPackageableThingTypeId: self.procedureWorkPackageableThingTypeId(),
                            ComingIntoForceDate: self.comingIntoForceDate(),
                            TimeLimitForObjectionEndDate: self.timeLimitForObjectionEndDate(),
                            ProcedureId: self.procedureId(),
                            MadeDate: self.madeDate(),
                            WithdrawalDate: self.withdrawalDate()
                        }
                    }).done(function (data) {
                        if (data === true)
                            window.location = window.urls.showWorkPackages;
                        else
                            self.isNotValidResponse(true);
                    }).fail(function () {
                        self.isNotValidResponse(true);
                    });
            };

            self.showDeletePopup = function () {
                self.isDeletePopupVisible(true);
            };

            self.deleteWorkPackageable = function () {
                $.ajax(window.urls.deleteWorkPackage.replace("{id}", self.workPackageable.Id), {
                    method: "DELETE",
                    dataType: "json"
                }).done(function (data) {
                    self.isDeletePopupVisible(false);
                    if (data === true)
                        window.location = window.urls.showWorkPackages;
                    else
                        self.isNotValidResponse(true);
                }).fail(function () {
                    self.isDeletePopupVisible(false);
                    self.isNotValidResponse(true);
                });
            };
        };

        var workPackageableId = $("#workPackageableId").val();
        if (Number.isNaN(Number.parseInt(workPackageableId)) === false)
            $.getJSON(window.urls.getWorkPackage.replace("{id}", workPackageableId), function (data) {
                var vm = new viewModel(data);
                ko.applyBindings(vm);
            });
        else {
            var vm = new viewModel(null);
            ko.applyBindings(vm);
        }
    });
});