requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function (step) {
            var self = this;
            if (step === null)
                self.step = {
                    Id: null,
                    TripleStoreId: null,
                    ProcedureStepName: null,
                    ProcedureStepDescription: null,
                    Houses: []
                };
            else
                self.step = step;

            self.isNotValidResponse = ko.observable(false);
            self.procedureStepName = ko.observable(self.step.ProcedureStepName || "");
            self.procedureStepDescription = ko.observable(self.step.ProcedureStepDescription || "");
            self.selectedHouses = ko.observableArray(self.step.Houses);
            self.isDeletePopupVisible = ko.observable(false);
            self.warningText = "Are you sure you wish to delete " + self.step.TripleStoreId + " step?";
            self.houses = ko.observableArray();

            $.getJSON(window.urls.getHouses, function (data) {
                self.houses(data);
            });

            self.canSave = ko.computed(function () {
                return self.procedureStepName().length > 0;
            });

            self.save = function () {
                if (Number.isNaN(Number.parseInt(self.step.Id)))
                    $.ajax(window.urls.addStep, {
                        method: "POST",
                        dataType: "json",
                        data: {
                            ProcedureStepName: self.procedureStepName(),
                            ProcedureStepDescription: self.procedureStepDescription(),
                            Houses: self.selectedHouses()
                        }
                    }).done(function (data) {
                        if (data === true)
                            window.location = window.urls.showSteps;
                        else
                            self.isNotValidResponse(true);
                    }).fail(function () {
                        self.isNotValidResponse(true);
                    });
                else
                    $.ajax(window.urls.updateStep.replace("{id}", self.step.Id), {
                        method: "PUT",
                        dataType: "json",
                        data: {
                            ProcedureStepName: self.procedureStepName(),
                            ProcedureStepDescription: self.procedureStepDescription(),
                            Houses: self.selectedHouses()
                        }
                    }).done(function (data) {
                        if (data === true)
                            window.location = window.urls.showSteps;
                        else
                            self.isNotValidResponse(true);
                    }).fail(function () {
                        self.isNotValidResponse(true);
                    });
            };

            self.showDeletePopup = function () {
                self.isDeletePopupVisible(true);
            };

            self.deleteStep = function () {
                $.ajax(window.urls.deleteStep.replace("{id}", self.step.Id), {
                    method: "DELETE",
                    dataType: "json"
                }).done(function (data) {
                    self.isDeletePopupVisible(false);
                    if (data === true)
                        window.location = window.urls.showSteps;
                    else
                        self.isNotValidResponse(true);
                }).fail(function () {
                    self.isNotValidResponse(true);
                });
            };
        };

        var stepId = $("#stepId").val();
        if (Number.isNaN(Number.parseInt(stepId)) === false)
            $.getJSON(window.urls.getStep.replace("{id}", stepId), function (data) {
                var vm = new viewModel(data);
                ko.applyBindings(vm);
            });
        else {
            var vm = new viewModel(null);
            ko.applyBindings(vm);
        }
    });
});