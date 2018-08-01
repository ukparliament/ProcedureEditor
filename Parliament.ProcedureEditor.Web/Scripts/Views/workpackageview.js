requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function (workPackageable) {
            var self = this;
            self.workPackageable = ko.observable(workPackageable);
            self.businessItems = ko.observableArray([]);

            $.getJSON(window.urls.getBusinessItemsSearchByWorkPackage.replace("{workPackageId}", workPackageable.Id), function (data) {
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
                            ProcedureWorkPackageableThingName: val.ProcedureWorkPackageableThingName,
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

        };

        var workPackageableId = $("#workPackageableId").val();
        $.getJSON(window.urls.getWorkPackage.replace("{id}", workPackageableId), function (data) {
            var vm = new viewModel(data);
            ko.applyBindings(vm);
        });
    });
});