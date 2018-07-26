(function () {
    var viewModel = function () {
        var self = this;
        self.workPackageables = ko.observableArray([]);

        $.getJSON(window.urls.getWorkPackages, function (data) {
            self.workPackageables(data);
        });
    };

    var vm = new viewModel();
    ko.applyBindings(vm);

})();