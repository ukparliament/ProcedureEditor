requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function (businessItem) {
            var self = this;

            self.businessItem = businessItem;

            self.isNotValidResponse = ko.observable(false);
            self.webLink = ko.observable(self.businessItem.WebLink || "");
            self.businessItemDate = ko.observable(self.businessItem.BusinessItemDate);
            self.isDeletePopupVisible = ko.observable(false);
            self.warningText = "Are you sure you wish to delete " + self.businessItem.TripleStoreId + " business item?";
            self.isBeingSaved = ko.observable(false);
            self.workPackageSteps = ko.observableArray([]);
            self.workPackagedList = ko.observableArray([]);
            self.workPackages = ko.observableArray([]);
            if (self.businessItem.ProcedureWorkPackageId !== null)
                self.workPackages.push({
                    Id: ko.observable(self.businessItem.ProcedureWorkPackageId)
                });
            else
                self.workPackages.push({
                    Id: ko.observable(null)
                });

            var initialStepArray = self.businessItem.Steps.map(function (val) {
                return {
                    Id: ko.observable(val)
                };
            });
            initialStepArray.push({
                Id: ko.observable(null)
            });
            self.businessItemSteps = ko.observableArray(initialStepArray);

            self.steps = ko.pureComputed(function () {
                return self.businessItemSteps().filter(function (itm) {
                    return itm.Id() !== null;
                })
                    .map(function (itm) {
                        return itm.Id();
                    });
            });

            self.removeWorkPackage = function (workPackagedId) {
                self.workPackages.remove(function (val) {
                    return val.Id() === null;
                });
                self.workPackages.push({ Id: ko.observable(null) });
                self.getSteps();
            };

            self.addWorkPackage = function (workPackaged) {
                if (Number.isNaN(Number.parseInt(self.businessItem.Id)))
                    self.workPackages.push({ Id: ko.observable(null) });
                self.getSteps();
            };

            self.removeStep = function () {
                self.getSteps();
            };

            self.addStep = function (step) {
                self.workPackageSteps.remove(step);
                self.businessItemSteps.push({
                    Id: ko.observable(null)
                });
            };

            self.workPackageIds = ko.computed(function () {
                return self.workPackages().filter(function (item) {
                    return item.Id() !== null;
                }).map(function (item) {
                    return item.Id();
                }).filter(function (item, index, arr) {
                    return arr.indexOf(item) === index;
                });
            });

            self.getSteps = function () {
                if (self.workPackageIds().length === 0) {
                    self.businessItemSteps([{
                        Id: ko.observable(null)
                    }]);
                    self.workPackageSteps([]);
                }
                else
                    $.ajax(window.urls.getStepsSearchByWorkPackaged, {
                        method: "POST",
                        dataType: "json",
                        data: {
                            WorkPackagedIds: self.workPackageIds()
                        }
                    }).done(function (data) {
                        self.workPackageSteps(data);
                        self.businessItemSteps.remove(function (val) {
                            return self.workPackageSteps().filter(function (item) {
                                return item.Id === val.Id();
                            }).length === 0;
                        });
                        var biNumber = self.businessItemSteps().length;
                        if (biNumber === 0)
                            self.businessItemSteps([{
                                Id: ko.observable(null)
                            }]);
                        if ((biNumber > 0) && (self.businessItemSteps()[biNumber - 1].Id() !== null))
                            self.businessItemSteps.push({
                                Id: ko.observable(null)
                            });
                    });
            };

            $.getJSON(window.urls.getWorkPackagedList, function (data) {
                self.workPackagedList(data);
            });

            self.canSave = ko.computed(function () {
                return (self.workPackageIds().length > 0) &&
                    (self.steps() !== null) &&
                    (self.steps().length > 0) &&
                    (self.isBeingSaved() === false);
            });

            self.saveAndReturnToWorkPackaged = function () {
                self.save(window.urls.showWorkPackaged.replace("{id}", self.workPackageIds()[0]));
            };

            self.save = function (redirectUrl) {
                self.isBeingSaved(true);
                if (Number.isNaN(Number.parseInt(self.businessItem.Id)))
                    $.ajax(window.urls.addBusinessItem, {
                        method: "POST",
                        dataType: "json",
                        data: {
                            WebLink: self.webLink(),
                            ProcedureWorkPackages: self.workPackageIds(),
                            BusinessItemDate: self.businessItemDate(),
                            Steps: self.steps()
                        }
                    }).done(function (data) {
                        if (data === true)
                            window.location = redirectUrl || window.urls.getBusinessItems;
                        else {
                            self.isNotValidResponse(true);
                            self.isBeingSaved(false);
                        }
                    }).fail(function () {
                        self.isNotValidResponse(true);
                        self.isBeingSaved(false);
                    });
                else
                    $.ajax(window.urls.updateBusinessItem.replace("{id}", self.businessItem.Id), {
                        method: "PUT",
                        dataType: "json",
                        data: {
                            WebLink: self.webLink(),
                            ProcedureWorkPackages: self.workPackageIds(),
                            BusinessItemDate: self.businessItemDate(),
                            Steps: self.steps()
                        }
                    }).done(function (data) {
                        if (data === true)
                            window.location = redirectUrl || window.urls.getBusinessItems;
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

            self.deleteBusinessItem = function () {
                $.ajax(window.urls.deleteBusinessItem.replace("{id}", self.businessItem.Id), {
                    method: "DELETE",
                    dataType: "json"
                }).done(function (data) {
                    self.isDeletePopupVisible(false);
                    if (data === true)
                        window.location = window.urls.showBusinessItems;
                    else
                        self.isNotValidResponse(true);
                }).fail(function () {
                    self.isDeletePopupVisible(false);
                    self.isNotValidResponse(true);
                });
            };

            self.getSteps();
        };

        var businessItemId = $("#businessItemId").val();
        var workPackageId = $("#workPackageId").val();
        if (Number.isNaN(Number.parseInt(businessItemId)) === false)
            $.getJSON(window.urls.getBusinessItem.replace("{id}", businessItemId), function (data) {
                var vm = new viewModel(data);
                ko.applyBindings(vm);
            });
        else {
            var bi = {
                Id: null,
                TripleStoreId: null,
                WebLink: null,
                ProcedureWorkPackageId: Number.isNaN(Number.parseInt(workPackageId)) ? null : workPackageId,
                WorkPackagedThingName: null,
                BusinessItemDate: null,
                Steps: []
            };
            var vm = new viewModel(bi);
            ko.applyBindings(vm);
        }
    });
});
