requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function (step) {
            var self = this;
            self.step = ko.observable(step);
            self.businessItems = ko.observableArray([]);
            self.routes = ko.observableArray([]);

            $.getJSON(window.urls.getBusinessItemsSearchByStep.replace("{stepId}", self.step().Id), function (data) {
                enhanceBusinessItems(data);
            });

            $.getJSON(window.urls.getRoutesSearchByStep.replace("{stepId}", self.step().Id), function (data) {
                self.routes(data);
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

        var stepId = $("#stepId").val();
        $.getJSON(window.urls.getStep.replace("{id}", stepId), function (data) {
            var vm = new viewModel(data);
            ko.applyBindings(vm);
        });
    });
});