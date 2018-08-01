define(["knockout", "jquery", "Scripts/text!template/workpackageselector"], function (ko, $, htmlText) {
    return {
        viewModel: function (params) {
            var self = this;

            self.procedureWorkPackageId = params.procedureWorkPackageId;
            self.workPackageables = params.workPackageables;
            self.procedureWorkPackageableThingName = ko.observable(null);
            self.searchWorkPackageText = ko.observable(null);

            if (self.procedureWorkPackageId() !== null) {
                $.getJSON(window.urls.getWorkPackage.replace("{id}", self.procedureWorkPackageId()), function (data) {
                    self.procedureWorkPackageableThingName(data.ProcedureWorkPackageableThingName);
                });
            }

            self.removeWorkPackage = function () {
                self.procedureWorkPackageId(null);
                self.procedureWorkPackageableThingName("");
                self.searchWorkPackageText("");
            };

            self.selectWorkPackage = function (workPackage) {
                self.procedureWorkPackageId(workPackage.Id);
                self.procedureWorkPackageableThingName(workPackage.ProcedureWorkPackageableThingName);
                self.searchWorkPackageText("");
            };

            self.filteredWorkPackages = ko.pureComputed(function () {
                var text = self.searchWorkPackageText();
                if ((text !== null) && (text.length > 0))
                    return self.workPackageables().filter(function (val) {
                        return val.ProcedureWorkPackageableThingName.toUpperCase().indexOf(text.toUpperCase()) >= 0;
                    });
                else
                    return [];
            });

        },
        template: htmlText
    }
});