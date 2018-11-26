define(["knockout", "jquery", "Scripts/text!template/procedureselector"], function (ko, $, htmlText) {
    return {
        viewModel: function (params) {
            var self = this;

            self.procedureId = params.procedureId;
            self.procedureList = params.procedureList;
            self.removeCallback = params.removeCallback;
            self.addCallback = params.addCallback;
            self.procedureName = ko.observable(null);
            self.searchProcedureText = ko.observable(null);

            if (self.procedureId() !== null) {
                var id = self.procedureId();
                var foundProcedure = self.procedureList().filter(function (val) {
                    return val.Id === id;
                });
                if ((foundProcedure !== null) && (foundProcedure.length === 1))
                    self.procedureName(foundProcedure[0].ProcedureName);
            }

            self.removeProcedure = function () {
                var procedureId = self.procedureId();
                self.procedureId(null);
                self.procedureName("");
                self.searchProcedureText("");
                if (self.removeCallback)
                    self.removeCallback(procedureId);
            };

            self.selectProcedure = function (procedure) {
                self.procedureId(procedure.Id);
                self.procedureName(procedure.ProcedureName);
                self.searchProcedureText("");
                if (self.addCallback)
                    self.addCallback(procedure);
            };

            self.filteredProcedures = ko.pureComputed(function () {
                var text = self.searchProcedureText();
                if ((text !== null) && (text.length > 0))
                    return self.procedureList().filter(function (val) {
                        return val.ProcedureName.toUpperCase().indexOf(text.toUpperCase()) >= 0;
                    });
                else
                    return [];
            });

        },
        template: htmlText
    }
});