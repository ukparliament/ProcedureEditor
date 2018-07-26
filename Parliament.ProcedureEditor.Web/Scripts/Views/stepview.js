(function () {
    var viewModel = function (step) {
        var self = this;
        self.step = ko.observable(step);
        self.businessItems = ko.observableArray([]);
        self.routes = ko.observableArray([]);

        $.getJSON(window.urls.getBusinessItemsSearchByStep.replace("{stepId}", self.step().Id), function (data) {
            self.businessItems(data);
        });

        $.getJSON(window.urls.getRoutesSearchByStep.replace("{stepId}", self.step().Id), function (data) {
            self.routes(data);
        });
        
    };

    var stepId = $("#stepId").val();
    $.getJSON(window.urls.getStep.replace("{id}", stepId), function (data) {
        var vm = new viewModel(data);
        ko.applyBindings(vm);
    });
})();