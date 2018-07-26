(function () {
    var viewModel = function (route) {
        var self = this;
        if (route === null)
            self.route = {
                Id: null,
                TripleStoreId: null,
                ProcedureId: null,
                FromProcedureStepId: null,
                ToProcedureStepId: null,
                ProcedureRouteTypeId: null
            };
        else
            self.route = route;

        self.isNotValidResponse = ko.observable(false);
        self.procedureId = ko.observable(self.route.ProcedureId);
        self.fromProcedureStepId = ko.observable(self.route.FromProcedureStepId);
        self.toProcedureStepId = ko.observable(self.route.ToProcedureStepId);
        self.procedureRouteTypeId = ko.observable(self.route.ProcedureRouteTypeId);
        self.isDeletePopupVisible = ko.observable(false);
        self.warningText = "Are you sure you wish to delete " + self.route.TripleStoreId + " route?";
        self.procedures = ko.observableArray([]);
        self.routeTypes = ko.observableArray([]);
        self.steps = ko.observableArray([]);
        self.searchFromStepText = ko.observable("");
        self.searchToStepText = ko.observable("");
        self.fromProcedureStepName = ko.observable("");
        self.toProcedureStepName = ko.observable("");

        $.getJSON(window.urls.getRouteTypes, function (data) {
            self.routeTypes(data);
        });

        $.getJSON(window.urls.getSteps, function (data) {
            self.steps(data);
            if (self.fromProcedureStepId() !== null) {
                var id = self.fromProcedureStepId();
                var found = self.steps().filter(function (val) {
                    return val.Id === id;
                });
                self.fromProcedureStepName(found[0].ProcedureStepName);
            }
            if (self.toProcedureStepId() !== null) {
                var id = self.toProcedureStepId();
                var found = self.steps().filter(function (val) {
                    return val.Id === id;
                });
                self.toProcedureStepName(found[0].ProcedureStepName);
            }
        });

        $.getJSON(window.urls.getProcedures, function (data) {
            self.procedures(data);
        });

        self.removeFromStep = function () {
            self.fromProcedureStepId(null);
            self.fromProcedureStepName("");
            self.searchFromStepText("");
        };

        self.selectFromStep = function (data) {
            self.fromProcedureStepId(data.Id);
            self.fromProcedureStepName(data.ProcedureStepName);
            self.searchFromStepText("");
        };

        self.removeToStep = function () {
            self.toProcedureStepId(null);
            self.toProcedureStepName("");
            self.searchToStepText("");
        };

        self.selectToStep = function (data) {
            self.toProcedureStepId(data.Id);
            self.toProcedureStepName(data.ProcedureStepName);
            self.searchToStepText("");
        };

        self.filteredFromSteps = ko.pureComputed(function () {
            var text = self.searchFromStepText();
            if ((text !== null) && (text.length > 0))
                return self.steps().filter(function (val) {
                    return val.ProcedureStepName.toUpperCase().indexOf(text.toUpperCase()) >= 0;
                });
            else
                return [];
        });

        self.filteredToSteps = ko.pureComputed(function () {
            var text = self.searchToStepText();
            if ((text !== null) && (text.length > 0))
                return self.steps().filter(function (val) {
                    return val.ProcedureStepName.toUpperCase().indexOf(text.toUpperCase()) >= 0;
                });
            else
                return [];
        });

        self.canSave = ko.computed(function () {
            return (self.procedureId() !== null) && (self.procedureId() > 0) &&
                (self.fromProcedureStepId() !== null) && (self.fromProcedureStepId() > 0) &&
                (self.toProcedureStepId() !== null) && (self.toProcedureStepId() > 0) &&
                (self.procedureRouteTypeId() !== null) && (self.procedureRouteTypeId() > 0);
        });

        self.save = function () {
            if (Number.isNaN(Number.parseInt(self.route.Id)))
                $.ajax(window.urls.addRoute, {
                    method: "POST",
                    dataType: "json",
                    data: {
                        ProcedureId: self.procedureId(),
                        FromProcedureStepId: self.fromProcedureStepId(),
                        ToProcedureStepId: self.toProcedureStepId(),
                        ProcedureRouteTypeId: self.procedureRouteTypeId()
                    }
                }).done(function (data) {
                    if (data === true)
                        window.location = window.urls.showRoutes;
                    else
                        self.isNotValidResponse(true);
                }).fail(function () {
                    self.isNotValidResponse(true);
                });
            else
                $.ajax(window.urls.updateRoute.replace("{id}", self.route.Id), {
                    method: "PUT",
                    dataType: "json",
                    data: {
                        ProcedureId: self.procedureId(),
                        FromProcedureStepId: self.fromProcedureStepId(),
                        ToProcedureStepId: self.toProcedureStepId(),
                        ProcedureRouteTypeId: self.procedureRouteTypeId()
                    }
                }).done(function (data) {
                    if (data === true)
                        window.location = window.urls.showRoutes;
                    else
                        self.isNotValidResponse(true);
                }).fail(function () {
                    self.isNotValidResponse(true);
                });
        };

        self.showDeletePopup = function () {
            self.isDeletePopupVisible(true);
        };

        self.deleteProcedure = function () {
            $.ajax(window.urls.deleteRoute.replace("{id}", self.route.Id), {
                method: "DELETE",
                dataType: "json"
            }).done(function (data) {
                self.isDeletePopupVisible(false);
                if (data === true)
                    window.location = window.urls.showRoutes;
                else
                    self.isNotValidResponse(true);
            }).fail(function () {
                self.isNotValidResponse(true);
            });
        };
    };

    var routeId = $("#routeId").val();
    if (Number.isNaN(Number.parseInt(routeId)) === false)
        $.getJSON(window.urls.getRoute.replace("{id}", routeId), function (data) {
            var vm = new viewModel(data);
            ko.applyBindings(vm);
        });
    else {
        var vm = new viewModel(null);
        ko.applyBindings(vm);
    }
})();