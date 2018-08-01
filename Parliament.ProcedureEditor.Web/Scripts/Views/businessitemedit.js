requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function (businessItem) {
            var self = this;

            self.businessItem = businessItem;

            self.isNotValidResponse = ko.observable(false);
            self.webLink = ko.observable(self.businessItem.WebLink || "");
            self.layingBodyId = ko.observable(self.businessItem.LayingBodyId);
            self.procedureWorkPackageId = ko.observable(self.businessItem.ProcedureWorkPackageId);
            self.businessItemDate = ko.observable(self.businessItem.BusinessItemDate);
            self.isDeletePopupVisible = ko.observable(false);
            self.warningText = "Are you sure you wish to delete " + self.businessItem.TripleStoreId + " business item?";
            self.searchWorkPackageText = ko.observable("");
            self.searchStepText = ko.observable("");
            self.searchLayingBodyText = ko.observable("");
            self.workPackageSteps = ko.observableArray([]);
            self.workPackageables = ko.observableArray([]);
            self.layingBodies = [];
            self.procedureWorkPackageableThingName = ko.observable(self.businessItem.ProcedureWorkPackageableThingName || "");
            self.layingBodyName = ko.observable("");

            var initialStepArray = self.businessItem.Steps.map(function (val) {
                return {
                    Id: ko.observable(val)
                }
            });
            if (initialStepArray.length === 0)
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

            self.addStep = function (step) {
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
                    $.getJSON(window.urls.getStepsSearchByWorkPackage.replace("{workPackageId}", self.procedureWorkPackageId()), function (data) {
                        self.workPackageSteps(data);
                        self.businessItemSteps.remove(function (val) {
                            return self.workPackageSteps().filter(function (item) {
                                return item.Id === val.Id();
                            }).length === 0;
                        });
                        if (self.businessItemSteps().length === 0)
                            self.businessItemSteps([{
                                Id: ko.observable(null)
                            }]);
                    });
            };

            ko.computed(function () {
                self.getSteps();
            });

            $.getJSON(window.urls.getLayingBodies, function (data) {
                self.layingBodies = data;
                if ((self.layingBodyId() !== null) && (self.layingBodyId() !== 0)) {
                    var id = self.layingBodyId();
                    var found = self.layingBodies.filter(function (val) {
                        return val.Id === id;
                    });
                    self.layingBodyName(found[0].LayingBodyName);
                }
            });

            $.getJSON(window.urls.getWorkPackages, function (data) {
                self.workPackageables(data);
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
                            LayingBodyId: self.layingBodyId(),
                            ProcedureWorkPackageId: self.procedureWorkPackageId(),
                            BusinessItemDate: self.businessItemDate(),
                            Steps: self.steps()
                        }
                    }).done(function (data) {
                        if (data === true)
                            window.location = window.urls.showWorkPackage.replace("{id}", self.procedureWorkPackageId());
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
                            LayingBodyId: self.layingBodyId(),
                            ProcedureWorkPackageId: self.procedureWorkPackageId(),
                            BusinessItemDate: self.businessItemDate(),
                            Steps: self.steps()
                        }
                    }).done(function (data) {
                        if (data === true)
                            window.location = window.urls.showWorkPackage.replace("{id}", self.procedureWorkPackageId());
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
        var workPackageableId = $("#workPackageableId").val();
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
                LayingBodyId: null,
                ProcedureWorkPackageId: Number.isNaN(Number.parseInt(workPackageableId)) ? null : workPackageableId,
                ProcedureWorkPackageableThingName: null,
                BusinessItemDate: null,
                Steps: []
            };
            var vm = new viewModel(bi);
            ko.applyBindings(vm);
        }
    });
});
