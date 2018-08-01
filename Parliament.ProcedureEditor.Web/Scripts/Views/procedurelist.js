requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function (procedures) {
            var self = this;
            self.procedures = ko.observableArray(procedures);
            self.isDeletePopupVisible = ko.observable(false);
            self.warningText = ko.observable(null);
            self.isNotValidResponse = ko.observable(false);
            self.soonToBeDeleted = null;

            self.showDeletePopup = function (procedure) {
                self.soonToBeDeleted = procedure;
                self.warningText("Are you sure you wish to delete " + procedure.TripleStoreId + " procedure?");
                self.isDeletePopupVisible(true);
            };

            self.deleteProcedure = function () {
                $.ajax(window.urls.deleteProcedure.replace("{id}", self.soonToBeDeleted.Id), {
                    method: "DELETE",
                    dataType: "json"
                }).done(function (data) {
                    self.isDeletePopupVisible(false);
                    if (data === true)
                        self.procedures.remove(self.soonToBeDeleted);
                    else
                        self.isNotValidResponse(true);
                }).fail(function () {
                    self.isDeletePopupVisible(false);
                    self.isNotValidResponse(true);
                });
            };
        };

        $.getJSON(window.urls.getProcedures, function (data) {
            var vm = new viewModel(data);
            ko.applyBindings(vm);
        });
        
    });
});