(function () {
    var viewModel = function () {
        var self = this;
        self.workPackageablePrecedings = ko.observableArray([]);

        $.getJSON(window.urls.getWorkPackageablePrecedings, function (data) {
            self.workPackageablePrecedings(data);
        });
    };

    var vm = new viewModel();
    ko.applyBindings(vm);

})();