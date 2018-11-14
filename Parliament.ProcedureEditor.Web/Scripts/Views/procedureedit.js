requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function (procedure) {
            var self = this;
            if (procedure === null)
                self.procedure = {
                    Id: null,
                    TripleStoreId: null,
                    ProcedureName: null
                };
            else
                self.procedure = procedure;

            self.isNotValidResponse = ko.observable(false);
            self.procedureName = ko.observable(self.procedure.ProcedureName || "");
            self.isDeletePopupVisible = ko.observable(false);
            self.warningText = "Are you sure you wish to delete " + self.procedure.TripleStoreId + " procedure?";
            self.isBeingSaved = ko.observable(false);

            self.canSave = ko.computed(function () {
                return (self.procedureName().length > 0) &&
                    (self.isBeingSaved() === false);
            });

            self.save = function () {
                self.isBeingSaved(true);
                if (Number.isNaN(Number.parseInt(self.procedure.Id)))
                    $.ajax(window.urls.addProcedure, {
                        method: "POST",
                        dataType: "json",
                        data: {
                            ProcedureName: self.procedureName()
                        }
                    }).done(function (data) {
                        if (data === true)
                            window.location = window.urls.showProcedures;
                        else {
                            self.isNotValidResponse(true);
                            self.isBeingSaved(false);
                        }
                    }).fail(function () {
                        self.isNotValidResponse(true);
                        self.isBeingSaved(false);
                    });
                else
                    $.ajax(window.urls.updateProcedure.replace("{id}", self.procedure.Id), {
                        method: "PUT",
                        dataType: "json",
                        data: {
                            ProcedureName: self.procedureName()
                        }
                    }).done(function (data) {
                        if (data === true)
                            window.location = window.urls.showProcedures;
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

            self.deleteProcedure = function () {
                $.ajax(window.urls.deleteProcedure.replace("{id}", self.procedure.Id), {
                    method: "DELETE",
                    dataType: "json"
                }).done(function (data) {
                    self.isDeletePopupVisible(false);
                    if (data === true)
                        window.location = window.urls.showProcedures;
                    else
                        self.isNotValidResponse(true);
                }).fail(function () {
                    self.isNotValidResponse(true);
                });
            };
        };

        var procedureId = $("#procedureId").val();
        if (Number.isNaN(Number.parseInt(procedureId)) === false)
            $.getJSON(window.urls.getProcedure.replace("{id}", procedureId), function (data) {
                var vm = new viewModel(data);
                ko.applyBindings(vm);
            });
        else {
            var vm = new viewModel(null);
            ko.applyBindings(vm);
        }
    });
});