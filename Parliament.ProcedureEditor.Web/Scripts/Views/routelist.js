requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout","jquery"], function (ko, $) {
        var viewModel = function () {
            var self = this;
            self.routes = ko.observableArray([]);
            self.isDeletePopupVisible = ko.observable(false);
            self.warningText = ko.observable(null);
            self.isNotValidResponse = ko.observable(false);
            self.soonToBeDeleted = null;

            $.getJSON(window.urls.getRoutes, function (data) {
                self.routes(data);
                self.routes.sort(function (left, right) {
                    var leftTxt = left.FromProcedureStepName + left.ToProcedureStepName;
                    var rightTxt = right.FromProcedureStepName + right.ToProcedureStepName;
                    return leftTxt === rightTxt ? 0 : leftTxt < rightTxt ? -1 : 1;
                });
            });

            self.showDeletePopup = function (route) {
                self.soonToBeDeleted = route;
                self.warningText("Are you sure you wish to delete " + route.TripleStoreId + " route?");
                self.isDeletePopupVisible(true);
            };

            self.deleteRoute = function () {
                $.ajax(window.urls.deleteRoute.replace("{id}", self.soonToBeDeleted.Id), {
                    method: "DELETE",
                    dataType: "json"
                }).done(function (data) {
                    self.isDeletePopupVisible(false);
                    if (data === true)
                        self.routes.remove(self.soonToBeDeleted);
                    else
                        self.isNotValidResponse(true);
                }).fail(function () {
                    self.isDeletePopupVisible(false);
                    self.isNotValidResponse(true);
                });
            };
        }
        ko.applyBindings(new viewModel());
    });
});