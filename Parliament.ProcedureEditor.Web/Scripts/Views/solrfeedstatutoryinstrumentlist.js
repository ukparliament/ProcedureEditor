requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function (statutoryInstruments) {
            var self = this;

            self.statutoryInstruments = ko.observableArray(statutoryInstruments);
            self.isDeletePopupVisible = ko.observable(false);
            self.warningText = ko.observable(null);
            self.isNotValidResponse = ko.observable(false);
            self.soonToBeDeleted = null;

            self.showDeletePopup = function (statutoryInstruments) {
                self.soonToBeDeleted = statutoryInstruments;
                self.warningText("Are you sure you wish to delete '" + statutoryInstruments.Title + "' record?");
                self.isDeletePopupVisible(true);
            };

            self.deleteStatutoryInstrument = function () {
                $.ajax(window.urls.deleteSolrStatutoryInstrument.replace("{id}", self.soonToBeDeleted.Id), {
                    method: "DELETE",
                    dataType: "json"
                }).done(function (data) {
                    self.isDeletePopupVisible(false);
                    if (data === true)
                        self.statutoryInstruments.remove(self.soonToBeDeleted);
                    else
                        self.isNotValidResponse(true);
                }).fail(function () {
                    self.isDeletePopupVisible(false);
                    self.isNotValidResponse(true);
                });
            };

        };

        $.getJSON(window.urls.getSolrStatutoryInstruments, function (statutoryInstruments) {
            var vm = new viewModel(statutoryInstruments);
            ko.applyBindings(vm);
        });
    });
});
