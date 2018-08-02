requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function (workPackagedPreceding) {
            var self = this;
            self.workPackagedPreceding = ko.observable(workPackagedPreceding);

        };

        var workPackagedPrecedingId = $("#workPackagedPrecedingId").val();
        $.getJSON(window.urls.getWorkPackagedPreceding.replace("{id}", workPackagedPrecedingId), function (data) {
            var vm = new viewModel(data);
            ko.applyBindings(vm);
        });
    });
});