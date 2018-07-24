(function () {
    var viewModel = function (businessItem) {
        var self = this;
        if (businessItem === null)
            self.businessItem = {
                Id: null,
                TripleStoreId: null,
                WebLink: null,
                LayingBodyId: null,
                ProcedureWorkPackageId: null,
                ProcedureWorkPackageableThingName: null,
                BusinessItemDate: null,
                Steps: []
            };
        else
            self.businessItem = businessItem;

        self.isNotValidResponse = ko.observable(false);
        self.webLink = ko.observable(self.businessItem.WebLink || "");
        self.layingBodyId = ko.observable(self.businessItem.LayingBodyId);
        self.procedureWorkPackageId = ko.observable(self.businessItem.ProcedureWorkPackageId);
        self.businessItemDate = ko.observable(self.businessItem.BusinessItemDate);
        self.steps = ko.observableArray(self.businessItem.Steps);
        self.isDeletePopupVisible = ko.observable(false);
        self.warningText = "Are you sure you wish to delete " + self.businessItem.TripleStoreId + " business item?";
        self.searchWorkPackageText = ko.observable("");
        self.searchStepText = ko.observable("");
        self.searchLayingBodyText = ko.observable("");
        self.workPackageSteps = ko.observableArray([]);
        self.workPackageables = [];
        self.layingBodies = [];
        self.procedureWorkPackageableThingName = ko.observable(self.businessItem.ProcedureWorkPackageableThingName || "");
        self.layingBodyName = ko.observable("");

        self.getSteps = function () {
            $.get(window.urls.getStepsSearchByWorkPackage.replace("{workPackageId}", self.procedureWorkPackageId()), function (data) {
                self.workPackageSteps(data);
                self.steps.remove(function (val) {
                    return self.workPackageSteps().filter(function (item) {
                        return item.Id === val;
                    }).length === 0;
                });

            });
        };

        if (self.procedureWorkPackageId() !== null)
            self.getSteps();

        $.get(window.urls.getLayingBodies, function (data) {
            self.layingBodies = data;
            if ((self.layingBodyId() !== null) && (self.layingBodyId() !== 0)) {
                var id = self.layingBodyId();
                var found = self.layingBodies.filter(function (val) {
                    return val.Id === id;
                });
                self.layingBodyName(found[0].LayingBodyName);
            }
        });

        $.get(window.urls.getWorkPackages, function (data) {
            self.workPackageables = data;
        });

        self.removeWorkPackage = function () {
            self.procedureWorkPackageId(null);
            self.procedureWorkPackageableThingName("");
            self.searchWorkPackageText("");
        };

        self.selectWorkPackage = function (data) {
            self.procedureWorkPackageId(data.Id);
            self.procedureWorkPackageableThingName(data.ProcedureWorkPackageableThingName);
            self.searchWorkPackageText("");
            self.getSteps();
        };

        self.filteredWorkPackages = ko.pureComputed(function () {
            var text = self.searchWorkPackageText();
            if ((text !== null) && (text.length > 0))
                return self.workPackageables.filter(function (val) {
                    return val.ProcedureWorkPackageableThingName.toUpperCase().indexOf(text.toUpperCase()) >= 0;
                });
            else
                return [];
        });

        self.removeLayingBody = function () {
            self.layingBodyId(null);
            self.layingBodyName("");
            self.searchLayingBodyText("");
        };

        self.selectLayingBody = function (data) {
            self.layingBodyId(data.Id);
            self.layingBodyName(data.LayingBodyName);
            self.searchLayingBodyText("");
        };

        self.filteredLayingBodies = ko.pureComputed(function () {
            var text = self.searchLayingBodyText();
            if ((text !== null) && (text.length > 0))
                return self.layingBodies.filter(function (val) {
                    return val.LayingBodyName.toUpperCase().indexOf(text.toUpperCase()) >= 0;
                });
            else
                return [];
        });

        self.removeStep = function (data) {
            self.steps.remove(function (val) {
                return val === data.Id;
            });
        };

        self.selectStep = function (data) {
            self.steps.push(data.Id);
            self.searchStepText("");
        };

        self.computedSteps = ko.pureComputed(function () {
            return self.steps().map(function (item) {
                var name = "";
                var found = self.workPackageSteps().filter(function (val) {
                    return val.Id === item;
                });
                if ((found !== null) && (found.length === 1))
                    name = found[0].ProcedureStepName;
                return {
                    Id: item,
                    ProcedureStepName: name
                }
            });
        });

        self.filteredSteps = ko.pureComputed(function () {
            var text = self.searchStepText();
            if ((text !== null) && (text.length > 0))
                return self.workPackageSteps().filter(function (val) {
                    return (val.ProcedureStepName.toUpperCase().indexOf(text.toUpperCase()) >= 0) && (self.steps().indexOf(val.Id) < 0);
                });
            else
                return [];
        });

        self.dateEntry = function (date) {
            var self = this;

            self.date = date;
            self.day = ko.observable(null);
            self.month = ko.observable(null);
            self.year = ko.observable(null);

            if (isNaN(Date.parse(self.date())) === false) {
                self.day(new Date(self.date()).getDate());
                self.month(new Date(self.date()).getMonth() + 1);
                self.year(new Date(self.date()).getFullYear());
            }

            self.checkDate = ko.computed(function () {
                var dateTxt = self.year() + "-" + (self.month() < 10 ? "0" + self.month() : self.month()) + "-" + (self.day() < 10 ? "0" + self.day() : self.day()) + "T00:00:00Z";
                if (isNaN(Date.parse(dateTxt)) === false)
                    self.date(dateTxt);
            });

        };

        self.biDate = new self.dateEntry(self.businessItemDate);

        self.canSave = ko.computed(function () {
            return (self.procedureWorkPackageId() !== 0) && (self.steps() !== null) && (self.steps().length > 0);
        });

        self.save = function () {
            if (Number.isNaN(Number.parseInt(self.businessItem.Id)))
                $.ajax(window.urls.addBusinessItem, {
                    method: "POST",
                    data: {
                        WebLink: self.webLink(),
                        LayingBodyId: self.layingBodyId(),
                        ProcedureWorkPackageId: self.procedureWorkPackageId(),
                        BusinessItemDate: self.businessItemDate(),
                        Steps: self.steps()
                    }
                }).done(function (data) {
                    if (data === true)
                        window.location = window.urls.showBusinessItems;
                    else
                        self.isNotValidResponse(true);
                }).fail(function () {
                    self.isNotValidResponse(true);
                });
            else
                $.ajax(window.urls.updateBusinessItem.replace("{id}", self.businessItem.Id), {
                    method: "PUT",
                    data: {
                        WebLink: self.webLink(),
                        LayingBodyId: self.layingBodyId(),
                        ProcedureWorkPackageId: self.procedureWorkPackageId(),
                        BusinessItemDate: self.businessItemDate(),
                        Steps: self.steps()
                    }
                }).done(function (data) {
                    if (data === true)
                        window.location = window.urls.showBusinessItems;
                    else
                        self.isNotValidResponse(true);
                }).fail(function () {
                    self.isNotValidResponse(true);
                });
        };

        self.showDeletePopup = function () {
            self.isDeletePopupVisible(true);
        };

        self.deleteBusinessItem = function () {
            $.ajax(window.urls.deleteBusinessItem.replace("{id}", self.businessItem.Id), {
                method: "DELETE"
            }).done(function (data) {
                self.isDeletePopupVisible(false);
                if (data === true)
                    window.location = window.urls.showBusinessItems;
                else
                    self.isNotValidResponse(true);
            }).fail(function () {
                self.isNotValidResponse(true);
            });
        };
    };

    var businessItemId = $("#businessItemId").val();
    if (Number.isNaN(Number.parseInt(businessItemId)) === false)
        $.get(window.urls.getBusinessItem.replace("{id}", businessItemId), function (data) {
            var vm = new viewModel(data);
            ko.applyBindings(vm);
        });
    else {
        var vm = new viewModel(null);
        ko.applyBindings(vm);
    }
})();