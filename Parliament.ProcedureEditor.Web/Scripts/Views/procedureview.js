(function () {
    var viewModel = function (procedure) {
        var self = this;
        self.procedure = ko.observable(procedure);
        self.workPackageables = ko.observableArray([]);
        self.routes = ko.observableArray([]);

        $.getJSON(window.urls.getWorkPackagesSearchByProcedure.replace("{procedureId}", self.procedure().Id), function (data) {
            self.workPackageables(data);
        });

        $.getJSON(window.urls.getRoutesSearchByProcedure.replace("{procedureId}", self.procedure().Id), function (data) {
            self.routes(data);
        });

    };

    var procedureId = $("#procedureId").val();
    $.getJSON(window.urls.getProcedure.replace("{id}", procedureId), function (data) {
        var vm = new viewModel(data);
        ko.applyBindings(vm);
    });
})();