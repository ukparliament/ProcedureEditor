(function () {
    var viewModel = function () {
        var self = this;
        self.steps = ko.observableArray([]);

        $.getJSON(window.urls.getSteps, function (data) {
            self.steps(data);
        });
    };

    var vm = new viewModel();
    ko.applyBindings(vm);

})();