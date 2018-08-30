requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function () {
            var self = this;
            self.layingItems = ko.observableArray([]);
            self.isDeletePopupVisible = ko.observable(false);
            self.warningText = ko.observable(null);
            self.isNotValidResponse = ko.observable(false);
            self.soonToBeDeleted = null;

            $.getJSON(window.urls.getLayingItems, function (data) {
                self.layingItems(data);
            });

            self.showDeletePopup = function (layingItem) {
                self.soonToBeDeleted = layingItem;
                self.warningText("Are you sure you wish to delete " + layingItem.TripleStoreId + " laying?");
                self.isDeletePopupVisible(true);
            };

            self.deleteLayingItem = function () {
                $.ajax(window.urls.deleteLayingItem.replace("{id}", self.soonToBeDeleted.Id), {
                    method: "DELETE",
                    dataType: "json"
                }).done(function (data) {
                    self.isDeletePopupVisible(false);
                    if (data === true)
                        self.layingItems.remove(self.soonToBeDeleted);
                    else
                        self.isNotValidResponse(true);
                }).fail(function () {
                    self.isDeletePopupVisible(false);
                    self.isNotValidResponse(true);
                });
            };
        };

        var vm = new viewModel();
        ko.applyBindings(vm);

    });
});