(function () {
    var viewModel = function (procedure) {
        var self = this;
        self.procedure = ko.observable(procedure);
        self.workPackageables = ko.observableArray([]);
        self.routes = ko.observableArray([]);

        $.get(window.urls.getWorkPackagesSearchByProcedure.replace("{procedureId}", self.procedure().Id), function (data) {
            self.workPackageables(data);
        });

        $.get(window.urls.getRoutesSearchByProcedure.replace("{procedureId}", self.procedure().Id), function (data) {
            self.routes(data);
        });

    };

    var procedureId = $("#procedureId").val();
    $.get(window.urls.getProcedure.replace("{id}", procedureId), function (data) {
        var vm = new viewModel(data);
        ko.applyBindings(vm);
    });
})();