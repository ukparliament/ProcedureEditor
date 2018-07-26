(function () {
    var viewModel = function (businessItem) {
        var self = this;
        self.businessItem = ko.observable(businessItem);

    };

    var businessItemId = $("#businessItemId").val();
    $.getJSON(window.urls.getBusinessItem.replace("{id}", businessItemId), function (data) {
        var vm = new viewModel(data);
        ko.applyBindings(vm);
    });
})();