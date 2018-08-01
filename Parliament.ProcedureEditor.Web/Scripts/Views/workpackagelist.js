requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function () {
            var self = this;
            self.workPackageables = ko.observableArray([]);
            self.isDeletePopupVisible = ko.observable(false);
            self.warningText = ko.observable(null);
            self.isNotValidResponse = ko.observable(false);
            self.soonToBeDeleted = null;

            $.getJSON(window.urls.getWorkPackages, function (data) {
                self.workPackageables(data);
            });

            self.showDeletePopup = function (workPackageable) {
                self.soonToBeDeleted = workPackageable;
                self.warningText("Are you sure you wish to delete " + workPackageable.TripleStoreId + " work package?");
                self.isDeletePopupVisible(true);
            };

            self.deleteWorkPackageable = function () {
                $.ajax(window.urls.deleteWorkPackage.replace("{id}", self.soonToBeDeleted.Id), {
                    method: "DELETE",
                    dataType: "json"
                }).done(function (data) {
                    self.isDeletePopupVisible(false);
                    if (data === true)
                        self.workPackageables.remove(self.soonToBeDeleted);
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