﻿(function () {
    var viewModel = function () {
        var self = this;
        self.routes = ko.observableArray([]);

        $.getJSON(window.urls.getRoutes, function (data) {
            self.routes(data);
        });

    };

    var vm = new viewModel();
    ko.applyBindings(vm);
})();