requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function (businessItem) {
            var self = this;

            self.businessItem = businessItem;

            self.isNotValidResponse = ko.observable(false);
            self.webLink = ko.observable(self.businessItem.WebLink || "");
            self.procedureWorkPackageId = ko.observable(self.businessItem.ProcedureWorkPackageId);
            self.businessItemDate = ko.observable(self.businessItem.BusinessItemDate);
            self.isDeletePopupVisible = ko.observable(false);
            self.warningText = "Are you sure you wish to delete " + self.businessItem.TripleStoreId + " business item?";
            self.workPackageSteps = ko.observableArray([]);
            self.workPackagedList = ko.observableArray([]);

            var initialStepArray = self.businessItem.Steps.map(function (val) {
                return {
                    Id: ko.observable(val)
                }
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

            self.removeStep = function (stepId) {
                self.getSteps();
            };

            self.addStep = function (step) {
                self.workPackageSteps.remove(step);
                self.businessItemSteps.push({
                    Id: ko.observable(null)
                });
            };

            self.getSteps = function () {
                if (self.procedureWorkPackageId() === null) {
                    self.businessItemSteps([{
                        Id: ko.observable(null)
                    }]);
                    self.workPackageSteps([]);
                }
                else
                    $.getJSON(window.urls.getStepsSearchByWorkPackaged.replace("{workPackageId}", self.procedureWorkPackageId()), function (data) {
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

            ko.computed(function () {
                self.getSteps();
            });

            $.getJSON(window.urls.getWorkPackagedList, function (data) {
                self.workPackagedList(data);
            });

            self.canSave = ko.computed(function () {
                return (self.procedureWorkPackageId() !== 0) && (self.steps() !== null) && (self.steps().length > 0);
            });

            self.save = function () {
                if (Number.isNaN(Number.parseInt(self.businessItem.Id)))
                    $.ajax(window.urls.addBusinessItem, {
                        method: "POST",
                        dataType: "json",
                        data: {
                            WebLink: self.webLink(),
                            ProcedureWorkPackageId: self.procedureWorkPackageId(),
                            BusinessItemDate: self.businessItemDate(),
                            Steps: self.steps()
                        }
                    }).done(function (data) {
                        if (data === true)
                            window.location = window.urls.showWorkPackaged.replace("{id}", self.procedureWorkPackageId());
                        else
                            self.isNotValidResponse(true);
                    }).fail(function () {
                        self.isNotValidResponse(true);
                    });
                else
                    $.ajax(window.urls.updateBusinessItem.replace("{id}", self.businessItem.Id), {
                        method: "PUT",
                        dataType: "json",
                        data: {
                            WebLink: self.webLink(),
                            ProcedureWorkPackageId: self.procedureWorkPackageId(),
                            BusinessItemDate: self.businessItemDate(),
                            Steps: self.steps()
                        }
                    }).done(function (data) {
                        if (data === true)
                            window.location = window.urls.showWorkPackaged.replace("{id}", self.procedureWorkPackageId());
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
        };

        var businessItemId = $("#businessItemId").val();
        var workPackagedId = $("#workPackagedId").val();
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
