define(["knockout", "jquery", "Scripts/text!template/stepselector"], function (ko, $, htmlText) {
    return {
        viewModel: function (params) {
            var self = this;

            self.procedureStepId = params.procedureStepId;
            self.steps = params.steps;
            self.addCallback = params.addCallback;
            self.procedureStepName = ko.observable(null);
            self.searchStepText = ko.observable(null);

            if (self.procedureStepId() !== null) {
                $.getJSON(window.urls.getStep.replace("{id}", self.procedureStepId()), function (data) {
                    self.procedureStepName(data.ProcedureStepName);
                });
            }

            self.removeStep = function () {
                self.procedureStepId(null);
                self.procedureStepName("");
                self.searchStepText("");
            };

            self.selectStep = function (data) {
                self.procedureStepId(data.Id);
                self.procedureStepName(data.ProcedureStepName);
                self.searchStepText("");
                if (self.addCallback)
                    self.addCallback(data);
            };

            self.filteredSteps = ko.pureComputed(function () {
                var text = self.searchStepText();
                if ((text !== null) && (text.length > 0))
                    return self.steps().filter(function (val) {
                        return val.ProcedureStepName.toUpperCase().indexOf(text.toUpperCase()) >= 0;
                    });
                else
                    return [];
            });

        },
        template: htmlText
    }
});