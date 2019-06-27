requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function (businessItemCandidate) {
            var self = this;
            self.businessItemCandidate = businessItemCandidate;
            self.webLink = ko.observable(businessItemCandidate.WebUrl);
            self.createdStep = ko.observable({});
            self.laidSteps = ko.observableArray([]);
            self.startedStep = ko.observable({});
            self.createdDate = ko.observable();
            self.laidDate = ko.observable();
            self.startedDate = ko.observable();
            self.layingBodies = ko.observableArray([]);
            self.isStartedBusinessItemAllowed = ko.observable(true);
            self.isNotValidResponse = ko.observable(false);
            self.warningText = "Are you sure you wish to delete '" + businessItemCandidate.WorkPackagedThingName + "' record?";
            self.isBeingSaved = ko.observable(false);
            self.isDeletePopupVisible = ko.observable(false);

            self.assignSteps = function (steps, procedureTripleStoreId) {
                var filteredSteps = [];
                var minDate = null;
                if (((isNaN(Date.parse(businessItemCandidate.LaidDate)) === false) &&
                    (isNaN(Date.parse(businessItemCandidate.MadeDate)) === false) &&
                    (new Date(businessItemCandidate.LaidDate).getTime() < new Date(businessItemCandidate.MadeDate).getTime())) ||
                    ((isNaN(Date.parse(businessItemCandidate.LaidDate)) === false) &&
                    (isNaN(Date.parse(businessItemCandidate.MadeDate)) === true)))
                    minDate = businessItemCandidate.LaidDate;
                else
                    if (((isNaN(Date.parse(businessItemCandidate.LaidDate)) === false) &&
                        (isNaN(Date.parse(businessItemCandidate.MadeDate)) === false) &&
                        (new Date(businessItemCandidate.LaidDate).getTime() >= new Date(businessItemCandidate.MadeDate).getTime())) ||
                        ((isNaN(Date.parse(businessItemCandidate.LaidDate)) === true) &&
                        (isNaN(Date.parse(businessItemCandidate.MadeDate)) === false)))
                        minDate = businessItemCandidate.MadeDate;
                switch (procedureTripleStoreId) {
                    case "iWugpxMn"://Made affirmative
                        filteredSteps = steps.filter(function (item) {
                            return item.TripleStoreId === "wShvPQbP";
                        });
                        self.createdStep(filteredSteps[0]);
                        self.createdDate(minDate);
                        filteredSteps = steps.filter(function (item) {
                            return item.TripleStoreId === "No1qmqJ4";
                        });
                        self.startedStep(filteredSteps[0]);
                        break;
                    case "iCdMN1MW"://Proposed negative statutory instruments
                        filteredSteps = steps.filter(function (item) {
                            return item.TripleStoreId === "u7VOBBH0";
                        });
                        self.createdStep(filteredSteps[0]);
                        self.createdDate(businessItemCandidate.LaidDate);
                        filteredSteps = steps.filter(function (item) {
                            return item.TripleStoreId === "uQY6bCqe";
                        });
                        self.startedStep(filteredSteps[0]);
                        break;
                    case "H5YJQsK2"://Draft affirmative
                        filteredSteps = steps.filter(function (item) {
                            return item.TripleStoreId === "wShvPQbP";
                        });
                        self.createdStep(filteredSteps[0]);
                        self.createdDate(businessItemCandidate.LaidDate);
                        self.isStartedBusinessItemAllowed(false);
                        break;
                    case "5S6p4YsP"://Made negative
                        filteredSteps = steps.filter(function (item) {
                            return item.TripleStoreId === "wShvPQbP";
                        });
                        self.createdStep(filteredSteps[0]);
                        self.createdDate(minDate);
                        filteredSteps = steps.filter(function (item) {
                            return item.TripleStoreId === "oUjmJcf5";
                        });
                        self.startedStep(filteredSteps[0]);
                        break;
                    case "gTgidljI"://Draft negative
                        filteredSteps = steps.filter(function (item) {
                            return item.TripleStoreId === "wShvPQbP";
                        });
                        self.createdStep(filteredSteps[0]);
                        self.createdDate(businessItemCandidate.LaidDate);
                        filteredSteps = steps.filter(function (item) {
                            return item.TripleStoreId === "oUjmJcf5";
                        });
                        self.startedStep(filteredSteps[0]);
                        break;
                    case "D00dsjR2"://Treaties subject to the Constitutional Reform and Governance Act 2010
                        filteredSteps = steps.filter(function (item) {
                            return item.TripleStoreId === "H6GOB6yX";
                        });
                        self.createdStep(filteredSteps[0]);
                        self.createdDate(businessItemCandidate.LaidDate);
                        filteredSteps = steps.filter(function (item) {
                            return item.TripleStoreId === "NnizWAGU";
                        });
                        self.startedStep(filteredSteps[0]);
                        break;
                }
                filteredSteps = steps.filter(function (item) {
                    return item.TripleStoreId === "cspzmb6w" || item.TripleStoreId === "puVMaN7t";
                }).map(function (item) {
                    return {
                        step: item,
                        webLink: ko.observable(null),
                        layingBodyId: ko.observable(null)
                    };
                });
                self.laidSteps(filteredSteps);
                self.laidDate(businessItemCandidate.LaidDate);
                if (procedureTripleStoreId == "iWugpxMn") //Made affirmative
                {
                    self.startedDate(businessItemCandidate.MadeDate);
                }
                else
                {
                    self.startedDate(businessItemCandidate.LaidDate);
                }
            };

            $.getJSON(window.urls.getProcedure.replace("{id}", businessItemCandidate.ProcedureId), function (procedure) {
                $.getJSON(window.urls.getSteps, function (steps) {
                    self.assignSteps(steps, procedure.TripleStoreId);
                });
            });

            $.getJSON(window.urls.getLayingBodies, function (data) {
                self.layingBodies(data);
            });

            self.showDeletePopup = function () {
                self.isDeletePopupVisible(true);
            };

            self.canSave = ko.computed(function () {
                return self.isBeingSaved() === false;
            });

            self.save = function () {
                self.isBeingSaved(true);
                var bis = [{
                    WebLink: self.webLink(),
                    ProcedureWorkPackageId: self.businessItemCandidate.WorkPackagedId,
                    BusinessItemDate: self.createdDate(),
                    StepId: self.createdStep().Id,
                    IsLaid: false
                },
                {
                    WebLink: self.laidSteps()[0].webLink(),
                    ProcedureWorkPackageId: self.businessItemCandidate.WorkPackagedId,
                    LayingDate: self.laidDate(),
                    StepId: self.laidSteps()[0].step.Id,
                    LayingBodyId: self.laidSteps()[0].layingBodyId(),
                    IsLaid: true
                },
                {
                    WebLink: self.laidSteps()[1].webLink(),
                    ProcedureWorkPackageId: self.businessItemCandidate.WorkPackagedId,
                    LayingDate: self.laidDate(),
                    StepId: self.laidSteps()[1].step.Id,
                    LayingBodyId: self.laidSteps()[1].layingBodyId(),
                    IsLaid: true
                }];
                if (self.isStartedBusinessItemAllowed() === true)
                    bis.push({
                        ProcedureWorkPackageId: self.businessItemCandidate.WorkPackagedId,
                        BusinessItemDate: self.startedDate(),
                        StepId: self.startedStep().Id,
                        IsLaid: false
                    });
                $.ajax(window.urls.addSolrBusinessItem.replace("{tripleStoreId}", self.businessItemCandidate.WorkPackagedTripleStoreId), {
                    method: "POST",
                    dataType: "json",
                    data: { "": bis }
                }).done(function (data) {
                    if (data === true)
                        window.location = window.urls.showSolrBusinessItems;
                    else {
                        self.isNotValidResponse(true);
                        self.isBeingSaved(false);
                    }
                }).fail(function () {
                    self.isNotValidResponse(true);
                    self.isBeingSaved(false);
                });
            };

            self.deleteSolrBusinessItem = function () {
                $.ajax(window.urls.deleteSolrBusinessItem.replace("{tripleStoreId}", self.businessItemCandidate.WorkPackagedTripleStoreId), {
                    method: "DELETE",
                    dataType: "json"
                }).done(function (data) {
                    self.isDeletePopupVisible(false);
                    if (data === true)
                        window.location = window.urls.showSolrBusinessItems;
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
            $.getJSON(window.urls.getSolrBusinessItem.replace("{id}", workPackagedId), function (data) {
                var vm = new viewModel(data);
                ko.applyBindings(vm);
            });
        
    });
});