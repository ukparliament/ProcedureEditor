requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function () {
            var self = this;
            self.workPackagedList = ko.observableArray([]);
            self.isDeletePopupVisible = ko.observable(false);
            self.warningText = ko.observable(null);
            self.isNotValidResponse = ko.observable(false);
            self.soonToBeDeleted = null;

            $.getJSON(window.urls.getWorkPackagedList, function (data) {
                self.workPackagedList(data);
            });

            self.showDeletePopup = function (workPackaged) {
                self.soonToBeDeleted = workPackaged;
                self.warningText("Are you sure you wish to delete " + workPackaged.TripleStoreId + " work package?");
                self.isDeletePopupVisible(true);
            };

            self.deleteWorkPackageable = function () {
                $.ajax(window.urls.deleteWorkPackaged.replace("{id}", self.soonToBeDeleted.Id), {
                    method: "DELETE",
                    dataType: "json"
                }).done(function (data) {
                    self.isDeletePopupVisible(false);
                    if (data === true)
                        self.workPackagedList.remove(self.soonToBeDeleted);
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