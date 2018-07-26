(function () {
    var viewModel = function (workPackageablePreceding) {
        var self = this;
        self.workPackageablePreceding = ko.observable(workPackageablePreceding);

    };

    var workPackageablePrecedingId = $("#workPackageablePrecedingId").val();
    $.getJSON(window.urls.getWorkPackageablePreceding.replace("{id}", workPackageablePrecedingId), function (data) {
        var vm = new viewModel(data);
        ko.applyBindings(vm);
    });
})();