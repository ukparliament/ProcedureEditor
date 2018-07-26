(function () {
    var viewModel = function (route) {
        var self = this;
        self.route = ko.observable(route);

    };

    var routeId = $("#routeId").val();
    $.getJSON(window.urls.getRoute.replace("{id}", routeId), function (data) {
        var vm = new viewModel(data);
        ko.applyBindings(vm);
    });
})();