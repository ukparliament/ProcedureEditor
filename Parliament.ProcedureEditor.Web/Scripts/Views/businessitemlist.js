(function () {
    var viewModel = function () {
        var self = this;
        self.businessItems = ko.observableArray([]);

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
                        BusinessItemDate: val.BusinessItemDate,
                        ProcedureWorkPackageableThingName: val.ProcedureWorkPackageableThingName,
                        ProcedureName: val.ProcedureName,
                        Steps: val.Steps.map(function (stepId) {
                            return steps["Id" + stepId];
                        })
                    };
                });
                self.businessItems(result);
            });
        };
    };

    var vm = new viewModel();
    ko.applyBindings(vm);

})();