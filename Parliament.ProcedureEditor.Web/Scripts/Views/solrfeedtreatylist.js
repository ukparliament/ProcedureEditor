requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function (treaties) {
            var self = this;

            self.treaties = ko.observableArray(treaties);
            self.isDeletePopupVisible = ko.observable(false);
            self.warningText = ko.observable(null);
            self.isNotValidResponse = ko.observable(false);
            self.soonToBeDeleted = null;

            self.showDeletePopup = function (treaty) {
                self.soonToBeDeleted = treaty;
                self.warningText("Are you sure you wish to delete '" + treaty.Title + "' record?");
                self.isDeletePopupVisible(true);
            };

            self.deleteTreaty = function () {
                $.ajax(window.urls.deleteSolrTreaty.replace("{id}", self.soonToBeDeleted.Id), {
                    method: "DELETE",
                    dataType: "json"
                }).done(function (data) {
                    self.isDeletePopupVisible(false);
                    if (data === true)
                        self.treaties.remove(self.soonToBeDeleted);
                    else
                        self.isNotValidResponse(true);
                }).fail(function () {
                    self.isDeletePopupVisible(false);
                    self.isNotValidResponse(true);
                });
            };

        };

        $.getJSON(window.urls.getSolrTreaties, function (treaties) {
            var vm = new viewModel(treaties);
            ko.applyBindings(vm);
        });
    });
});
