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
                    LeadGovernmentOrganisationTripleStoreId: null,
                    Citation: null,
                    SeriesMembershipIds:[]
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
            self.leadGovernmentOrganisationTripleStoreId = ko.observable(self.workPackaged.LeadGovernmentOrganisationTripleStoreId);
            self.citation = ko.observable(self.workPackaged.Citation);
            self.seriesMembershipIds = ko.observableArray(self.workPackaged.SeriesMembershipIds);

            self.procedures = ko.observableArray([]);
            self.procedureDictionary = ko.observable(null);
            $.getJSON(window.urls.getProcedures, function (data) {
                self.procedures(data);
                var procedureDictionary = {};
                for (i = 0; i < data.length; i++)
                    procedureDictionary[data[i].Id.toString()] = data[i].TripleStoreId;
                self.procedureDictionary(procedureDictionary);
            });

            self.workPackagedKind = ko.computed(function () {
                if ((self.procedureId() === null) || (self.procedureDictionary()===null))
                    return 1;
                var id = self.procedureDictionary()[self.procedureId().toString()];
                switch (id) {
                    case "iCdMN1MW":
                        return 2;
                    case "D00dsjR2":
                        return 3;
                    default:
                        return 1;
                }
            });

            self.isDeletePopupVisible = ko.observable(false);
            self.warningText = "Are you sure you wish to delete " + self.workPackaged.TripleStoreId + " work package?";
            self.isBeingSaved = ko.observable(false);

            self.canSave = ko.computed(function () {
                return (self.workPackagedThingName().length > 0) &&
                    (self.procedureId() !== null) &&
                    (((self.workPackagedKind() === 3) && (self.seriesMembershipIds().length > 0)) || (self.workPackagedKind()!==3)) &&
                    (self.isBeingSaved() === false);
            });

            self.save = function () {
                self.isBeingSaved(true);
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
                            LeadGovernmentOrganisationTripleStoreId: self.leadGovernmentOrganisationTripleStoreId(),
                            Citation: self.citation(),
                            SeriesMembershipIds: self.seriesMembershipIds(),
                            ProcedureId: self.procedureId(),
                            WorkPackagedKind: self.workPackagedKind()
                        }
                    }).done(function (data) {
                        if (data === true)
                            window.location = window.urls.showWorkPackagedList;
                        else {
                            self.isNotValidResponse(true);
                            self.isBeingSaved(false);
                        }
                    }).fail(function () {
                        self.isNotValidResponse(true);
                        self.isBeingSaved(false);
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
                            LeadGovernmentOrganisationTripleStoreId: self.leadGovernmentOrganisationTripleStoreId(),
                            Citation: self.citation(),
                            SeriesMembershipIds: self.seriesMembershipIds(),
                            ProcedureId: self.procedureId(),
                            WorkPackagedKind: self.workPackagedKind()
                        }
                    }).done(function (data) {
                        if (data === true)
                            window.location = window.urls.showWorkPackagedList;
                        else {
                            self.isNotValidResponse(true);
                            self.isBeingSaved(false);
                        }
                    }).fail(function () {
                        self.isNotValidResponse(true);
                        self.isBeingSaved(false);
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