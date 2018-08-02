requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function (workPackaged) {
            var self = this;
            self.workPackaged = ko.observable(workPackaged);
            self.businessItems = ko.observableArray([]);
            self.statutoryInstrumentType = self.workPackaged().IsStatutoryInstrument ? "Statutory instrument paper" : "Proposed negative statutory instrument paper";

            $.getJSON(window.urls.getBusinessItemsSearchByWorkPackaged.replace("{workPackageId}", workPackaged.Id), function (data) {
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

        };

        var workPackagedId = $("#workPackagedId").val();
        $.getJSON(window.urls.getWorkPackaged.replace("{id}", workPackagedId), function (data) {
            var vm = new viewModel(data);
            ko.applyBindings(vm);
        });
    });
});