requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function (route) {
            var self = this;
            if (route === null)
                self.route = {
                    Id: null,
                    TripleStoreId: null,
                    Procedures: [],
                    FromProcedureStepId: null,
                    ToProcedureStepId: null,
                    ProcedureRouteTypeId: null
                };
            else
                self.route = route;

            self.isNotValidResponse = ko.observable(false);
            self.fromProcedureStepId = ko.observable(self.route.FromProcedureStepId);
            self.toProcedureStepId = ko.observable(self.route.ToProcedureStepId);
            self.procedureRouteTypeId = ko.observable(self.route.ProcedureRouteTypeId);
            self.isDeletePopupVisible = ko.observable(false);
            self.warningText = "Are you sure you wish to delete " + self.route.TripleStoreId + " route?";
            self.isBeingSaved = ko.observable(false);
            self.procedures = ko.observableArray([]);
            self.routeTypes = ko.observableArray([]);
            self.steps = ko.observableArray([]);
            self.searchFromStepText = ko.observable("");
            self.searchToStepText = ko.observable("");
            self.fromProcedureStepName = ko.observable("");
            self.toProcedureStepName = ko.observable("");
            self.procedureList = ko.observableArray([]);
            if (self.route.Procedures !== null)
                self.route.Procedures.forEach(function (val) {
                    self.procedures.push({
                        Id: ko.observable(val)
                    });
                });
            self.procedures.push({
                Id: ko.observable(null)
            });

            $.getJSON(window.urls.getProcedures, function (data) {
                self.procedureList(data);
            });

            $.getJSON(window.urls.getRouteTypes, function (data) {
                self.routeTypes(data);
            });

            $.getJSON(window.urls.getSteps, function (data) {
                self.steps(data);
            });

            self.removeProcedure = function (procedureId) {
                self.procedures.remove(function (val) {
                    return val.Id() === null;
                });
                self.procedures.push({ Id: ko.observable(null) });
            };

            self.addProcedure = function (procedure) {
                self.procedures.push({ Id: ko.observable(null) });
            };

            self.routeProcedures = ko.pureComputed(function () {
                return self.procedures().filter(function (itm) {
                    return itm.Id() !== null;
                })
                    .map(function (itm) {
                        return itm.Id();
                    });
            });

            self.canSave = ko.computed(function () {
                return (self.routeProcedures().length > 0) &&
                    (self.fromProcedureStepId() !== null) && (self.fromProcedureStepId() > 0) &&
                    (self.toProcedureStepId() !== null) && (self.toProcedureStepId() > 0) &&
                    (self.procedureRouteTypeId() !== null) && (self.procedureRouteTypeId() > 0) &&
                    (self.isBeingSaved() === false);
            });

            self.saveAndReturnToProcedure = function () {
                self.save(window.urls.showProcedure.replace("{id}", self.routeProcedures()[0]));
            };

            self.save = function (redirectUrl) {
                self.isBeingSaved(true);
                if (Number.isNaN(Number.parseInt(self.route.Id)))
                    $.ajax(window.urls.addRoute, {
                        method: "POST",
                        dataType: "json",
                        data: {
                            FromProcedureStepId: self.fromProcedureStepId(),
                            ToProcedureStepId: self.toProcedureStepId(),
                            ProcedureRouteTypeId: self.procedureRouteTypeId(),
                            Procedures: self.routeProcedures()
                        }
                    }).done(function (data) {
                        if (data === true)
                            window.location = redirectUrl || window.urls.showRoutes;
                        else {
                            self.isNotValidResponse(true);
                            self.isBeingSaved(false);
                        }
                    }).fail(function () {
                        self.isNotValidResponse(true);
                        self.isBeingSaved(false);
                    });
                else
                    $.ajax(window.urls.updateRoute.replace("{id}", self.route.Id), {
                        method: "PUT",
                        dataType: "json",
                        data: {
                            FromProcedureStepId: self.fromProcedureStepId(),
                            ToProcedureStepId: self.toProcedureStepId(),
                            ProcedureRouteTypeId: self.procedureRouteTypeId(),
                            Procedures: self.routeProcedures()
                        }
                    }).done(function (data) {
                        if (data === true)
                            window.location = redirectUrl || window.urls.showRoutes;
                        else {
                            self.isNotValidResponse(true);
                            self.isBeingSaved(false);
                        }
                    }).fail(function () {
                        self.isNotValidResponse(true);
                        self.isBeingSaved(false);
                    });
            };

            self.showDeletePopup = function () {
                self.isDeletePopupVisible(true);
            };

            self.deleteRoute = function () {
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
                    self.isDeletePopupVisible(false);
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
    });
});