(function () {
    var viewModel = function (procedures) {
        var self = this;

        self.procedures = ko.observableArray(procedures);
        
    };

    $.getJSON(window.urls.getProcedures, function (data) {
        var vm = new viewModel(data);
        ko.applyBindings(vm);
    });
})();