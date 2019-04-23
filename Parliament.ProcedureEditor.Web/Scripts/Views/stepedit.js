requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function (step) {
            var self = this;
            if (step === null)
                self.step = {
                    Id: null,
                    TripleStoreId: null,
                    ProcedureStepName: null,
                    ProcedureStepDescription: null,
                    ProcedureStepScopeNote: null,
                    ProcedureStepLinkNote: null,
                    ProcedureStepDateNote: null,
                    CommonlyActualisedAlongsideProcedureStepIds: [],
                    Houses: [],
                    Publication: {TripleStoreId: null, PublicationName:null, PublicationUrl:null}
                };
            else
                self.step = step;

            self.isNotValidResponse = ko.observable(false);
            self.procedureStepName = ko.observable(self.step.ProcedureStepName || "");
            self.procedureStepDescription = ko.observable(self.step.ProcedureStepDescription || "");
            self.procedureStepScopeNote = ko.observable(self.step.ProcedureStepScopeNote || "");
            self.procedureStepLinkNote = ko.observable(self.step.ProcedureStepLinkNote || "");
            self.procedureStepDateNote = ko.observable(self.step.ProcedureStepDateNote || "");
            self.selectedHouses = ko.observableArray(self.step.Houses);
            self.publicationName = ko.observable(self.step.Publication.PublicationName || "");
            self.publicationUrl = ko.observable(self.step.Publication.PublicationUrl || "");
            self.isDeletePopupVisible = ko.observable(false);
            self.warningText = "Are you sure you wish to delete " + self.step.TripleStoreId + " step?";
            self.isBeingSaved = ko.observable(false);
            self.houses = ko.observableArray();
            self.steps = ko.observableArray([]);
            self.commonlyActualisedAlongsideProcedureStepIds = ko.observableArray([]);
            if (self.step.CommonlyActualisedAlongsideProcedureStepIds != null)
                self.step.CommonlyActualisedAlongsideProcedureStepIds.forEach(function (val) {
                    self.commonlyActualisedAlongsideProcedureStepIds.push({
                        Id: ko.observable(val)
                    });
                });
            self.commonlyActualisedAlongsideProcedureStepIds.push({
                Id: ko.observable(null)
            });

            $.getJSON(window.urls.getHouses, function (data) {
                self.houses(data);
            });

            self.canSave = ko.computed(function () {
                return (self.procedureStepName().length > 0) &&
                    (self.isBeingSaved() === false);
            });

            $.getJSON(window.urls.getSteps, function (data) {
                self.steps(data);
            });

            self.removeStep = function (stepId) {
                self.commonlyActualisedAlongsideProcedureStepIds.remove(function (val) {
                    return val.Id() === null;
                });
                self.commonlyActualisedAlongsideProcedureStepIds.push({ Id: ko.observable(null) });
            };

            self.addStep = function (step) {
                self.commonlyActualisedAlongsideProcedureStepIds.push({ Id: ko.observable(null) });
            };

            self.alongsideSteps = ko.pureComputed(function () {
                return self.commonlyActualisedAlongsideProcedureStepIds().filter(function (itm) {
                    return itm.Id() !== null;
                })
                    .map(function (itm) {
                        return itm.Id();
                    });
            });

            self.save = function () {
                self.isBeingSaved(true);
                if (Number.isNaN(Number.parseInt(self.step.Id)))
                    $.ajax(window.urls.addStep, {
                        method: "POST",
                        dataType: "json",
                        data: {
                            ProcedureStepName: self.procedureStepName(),
                            ProcedureStepDescription: self.procedureStepDescription(),
                            ProcedureStepScopeNote: self.procedureStepScopeNote(),
                            ProcedureStepLinkNote: self.procedureStepLinkNote(),
                            ProcedureStepDateNote: self.procedureStepDateNote(),
                            CommonlyActualisedAlongsideProcedureStepIds: self.alongsideSteps(),
                            Houses: self.selectedHouses(),
                            Publication: {
                                TripleStoreId: self.step.Publication.TripleStoreId,
                                PublicationName: self.publicationName(),
                                PublicationUrl: self.publicationUrl(),
                            }
                        }
                    }).done(function (data) {
                        if (data === true)
                            window.location = window.urls.showSteps;
                        else {
                            self.isNotValidResponse(true);
                            self.isBeingSaved(false);
                        }
                    }).fail(function () {
                        self.isNotValidResponse(true);
                        self.isBeingSaved(false);
                    });
                else
                    $.ajax(window.urls.updateStep.replace("{id}", self.step.Id), {
                        method: "PUT",
                        dataType: "json",
                        data: {
                            ProcedureStepName: self.procedureStepName(),
                            ProcedureStepDescription: self.procedureStepDescription(),
                            ProcedureStepScopeNote: self.procedureStepScopeNote(),
                            ProcedureStepLinkNote: self.procedureStepLinkNote(),
                            ProcedureStepDateNote: self.procedureStepDateNote(),
                            CommonlyActualisedAlongsideProcedureStepIds: self.alongsideSteps(),
                            Houses: self.selectedHouses(),
                            Publication: {
                                TripleStoreId: self.step.Publication.TripleStoreId,
                                PublicationName: self.publicationName(),
                                PublicationUrl: self.publicationUrl(),
                            }
                        }
                    }).done(function (data) {
                        if (data === true)
                            window.location = window.urls.showSteps;
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

            self.deleteStep = function () {
                $.ajax(window.urls.deleteStep.replace("{id}", self.step.Id), {
                    method: "DELETE",
                    dataType: "json"
                }).done(function (data) {
                    self.isDeletePopupVisible(false);
                    if (data === true)
                        window.location = window.urls.showSteps;
                    else
                        self.isNotValidResponse(true);
                }).fail(function () {
                    self.isNotValidResponse(true);
                });
            };
        };

        var stepId = $("#stepId").val();
        if (Number.isNaN(Number.parseInt(stepId)) === false)
            $.getJSON(window.urls.getStep.replace("{id}", stepId), function (data) {
                var vm = new viewModel(data);
                ko.applyBindings(vm);
            });
        else {
            var vm = new viewModel(null);
            ko.applyBindings(vm);
        }
    });
});