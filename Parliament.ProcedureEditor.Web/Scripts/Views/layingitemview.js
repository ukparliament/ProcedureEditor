requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function (layingItem) {
            var self = this;
            self.layingItem = ko.observable(layingItem);

        };

        var layingItemId = $("#layingItemId").val();
        $.getJSON(window.urls.getLayingItem.replace("{id}", layingItemId), function (data) {
            var vm = new viewModel(data);
            ko.applyBindings(vm);
        });
    });
});