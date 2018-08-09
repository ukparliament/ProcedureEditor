requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function () {
            var self = this;
            self.businessItems = ko.observableArray([]);
            self.isDeletePopupVisible = ko.observable(false);
            self.warningText = ko.observable(null);
            self.isNotValidResponse = ko.observable(false);
            self.soonToBeDeleted = null;

            $.getJSON(window.urls.getBusinessItems, function (data) {
                enhanceBusinessItems(data);
            });

            var enhanceBusinessItems = function (bi) {
                $.getJSON(window.urls.getSteps, function (data) {
                    var steps = {};
                    data.forEach(function (val) {
                        steps["Id" + val.Id] = {
                            ProcedureStepName: val.ProcedureStepName,
                            HouseNames: val.HouseNames
                        };
                    });
                    var result = bi.map(function (val) {
                        return {
                            Id: val.Id,
                            TripleStoreId: val.TripleStoreId,
                            BusinessItemDate: val.BusinessItemDate,
                            WorkPackagedThingName: val.WorkPackagedThingName,
                            ProcedureName: val.ProcedureName,
                            Steps: val.Steps.map(function (stepId) {
                                if (steps["Id" + stepId])
                                    return steps["Id" + stepId];
                                else
                                    return {
                                        ProcedureStepName: "N/A",
                                        HouseNames: []
                                    };
                            })
                        };
                    });
                    self.businessItems(result);
                });
            };

            self.showDeletePopup = function (businessItem) {
                self.soonToBeDeleted = businessItem;
                self.warningText("Are you sure you wish to delete " + businessItem.TripleStoreId + " business item?");
                self.isDeletePopupVisible(true);
            };

            self.deleteBusinessItem = function () {
                $.ajax(window.urls.deleteBusinessItem.replace("{id}", self.soonToBeDeleted.Id), {
                    method: "DELETE",
                    dataType: "json"
                }).done(function (data) {
                    self.isDeletePopupVisible(false);
                    if (data === true)
                        self.businessItems.remove(self.soonToBeDeleted);
                    else
                        self.isNotValidResponse(true);
                }).fail(function () {
                    self.isDeletePopupVisible(false);
                    self.isNotValidResponse(true);
                });
            };
        };

        var vm = new viewModel();
        ko.applyBindings(vm);

    });
});