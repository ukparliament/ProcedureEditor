requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function () {
            var self = this;
            self.workPackageablePrecedings = ko.observableArray([]);
            self.isDeletePopupVisible = ko.observable(false);
            self.warningText = ko.observable(null);
            self.isNotValidResponse = ko.observable(false);
            self.soonToBeDeleted = null;

            $.getJSON(window.urls.getWorkPackageablePrecedings, function (data) {
                self.workPackageablePrecedings(data);
            });

            self.showDeletePopup = function (workPackageablePreceding) {
                self.soonToBeDeleted = workPackageablePreceding;
                self.warningText("Are you sure you wish to delete this work package preceding?");
                self.isDeletePopupVisible(true);
            };

            self.deleteWorkPackageablePreceding = function () {
                $.ajax(window.urls.deleteWorkPackageablePreceding.replace("{id}", self.soonToBeDeleted.Id), {
                    method: "DELETE",
                    dataType: "json"
                }).done(function (data) {
                    self.isDeletePopupVisible(false);
                    if (data === true)
                        self.workPackageablePrecedings.remove(self.soonToBeDeleted);
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