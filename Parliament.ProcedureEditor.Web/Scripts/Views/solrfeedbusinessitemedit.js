requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function (statutoryInstrument, procedureId, workPackageId) {
            var self = this;
            self.statutoryInstrument = statutoryInstrument;
            self.webLink = ko.observable(statutoryInstrument.WebUrl);
            self.createdStep = ko.observable({});
            self.laidSteps = ko.observableArray([]);
            self.startedStep = ko.observable({});
            self.createdDate = ko.observable();
            self.laidDate = ko.observable();
            self.startedDate = ko.observable();
            self.isStartedBusinessItemAllowed = ko.observable(true);
            self.isNotValidResponse = ko.observable(false);
            self.warningText = "Are you sure you wish to delete '" + statutoryInstrument.Title + "' record?";
            self.isDeletePopupVisible = ko.observable(false);
            
            self.assignSteps = function (steps, procedureTripleStoreId) {
                var filteredSteps = [];
                var minDate = null;
                if (((isNaN(Date.parse(statutoryInstrument.LaidDate)) === false) &&
                    (isNaN(Date.parse(statutoryInstrument.MadeDate)) === false) &&
                    (new Date(statutoryInstrument.LaidDate).getTime() < new Date(statutoryInstrument.MadeDate).getTime())) ||
                    ((isNaN(Date.parse(statutoryInstrument.LaidDate)) === false) &&
                        (isNaN(Date.parse(statutoryInstrument.MadeDate)) === true)))
                    minDate = statutoryInstrument.LaidDate;
                else
                    if (((isNaN(Date.parse(statutoryInstrument.LaidDate)) === false) &&
                        (isNaN(Date.parse(statutoryInstrument.MadeDate)) === false) &&
                        (new Date(statutoryInstrument.LaidDate).getTime() >= new Date(statutoryInstrument.MadeDate).getTime())) ||
                        ((isNaN(Date.parse(statutoryInstrument.LaidDate)) === true) &&
                            (isNaN(Date.parse(statutoryInstrument.MadeDate)) === false)))
                        minDate = statutoryInstrument.MadeDate;
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
                        self.createdDate(statutoryInstrument.LaidDate);
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
                        self.createdDate(statutoryInstrument.LaidDate);
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
                        self.createdDate(statutoryInstrument.LaidDate);
                        filteredSteps = steps.filter(function (item) {
                            return item.TripleStoreId === "oUjmJcf5";
                        });
                        self.startedStep(filteredSteps[0]);
                        break;
                }
                filteredSteps = steps.filter(function (item) {
                    return item.TripleStoreId === "cspzmb6w" || item.TripleStoreId === "puVMaN7t";
                }).map(function (item) {
                    return {
                        step: item,
                        webLink: ko.observable(null)
                    };
                });
                self.laidSteps(filteredSteps);
                self.laidDate(statutoryInstrument.LaidDate);
                self.startedDate(statutoryInstrument.LaidDate);
            };

            $.getJSON(window.urls.getProcedure.replace("{id}", procedureId), function (procedure) {
                $.getJSON(window.urls.getSteps, function (steps) {
                    self.assignSteps(steps, procedure.TripleStoreId);
                });
            });

            self.showDeletePopup = function () {
                self.isDeletePopupVisible(true);
            };

            self.save = function () {
                var bis = [{
                    WebLink: self.webLink(),
                    ProcedureWorkPackages: [workPackageId],
                    BusinessItemDate: self.createdDate(),
                    Steps: [self.createdStep().Id]
                },
                {
                    WebLink: self.laidSteps()[0].webLink(),
                    ProcedureWorkPackages: workPackageId,
                    BusinessItemDate: self.laidDate(),
                    Steps: [self.laidSteps()[0].step.Id]
                },
                {
                    WebLink: self.laidSteps()[1].webLink(),
                    ProcedureWorkPackages: [workPackageId],
                    BusinessItemDate: self.laidDate(),
                    Steps: [self.laidSteps()[1].step.Id]
                }];
                if (self.isStartedBusinessItemAllowed()=== true)
                    bis.push({
                        ProcedureWorkPackages: [workPackageId],
                        BusinessItemDate: self.startedDate(),
                        Steps: [self.startedStep().Id]
                    });
                $.ajax(window.urls.addSolrBusinessItem.replace("{id}", self.statutoryInstrument.Id), {
                    method: "POST",
                    dataType: "json",
                    data: { "": bis }
                }).done(function (data) {
                    if (data === true)
                        window.location = window.urls.showSolrBusinessItems;
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
                        window.location = window.urls.showSolrBusinessItems;
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
            $.getJSON(window.urls.getSolrBusinessItem.replace("{id}", statutoryInstrumentId), function (statutoryInstrument) {
                $.getJSON(window.urls.getWorkPackagedByTripleStoreId.replace("{tripleStoreId}", statutoryInstrument.TripleStoreId), function (workPackaged) {
                    var vm = new viewModel(statutoryInstrument, workPackaged.ProcedureId, workPackaged.Id);
                    ko.applyBindings(vm);
                });
            });
        else {
            var vm = new viewModel(null);
            ko.applyBindings(vm);
        }
    });
});