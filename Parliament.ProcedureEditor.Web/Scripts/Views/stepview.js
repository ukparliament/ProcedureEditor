(function () {
    var viewModel = function (step) {
        var self = this;
        self.step = ko.observable(step);
        self.businessItems = ko.observableArray([]);
        self.routes = ko.observableArray([]);

        $.get(window.urls.getBusinessItemsSearchByStep.replace("{stepId}", self.step().Id), function (data) {
            self.businessItems(data);
        });

        $.get(window.urls.getRoutesSearchByStep.replace("{stepId}", self.step().Id), function (data) {
            self.routes(data);
        });
        
    };

    var stepId = $("#stepId").val();
    $.get(window.urls.getStep.replace("{id}", stepId), function (data) {
        var vm = new viewModel(data);
        ko.applyBindings(vm);
    });
})();