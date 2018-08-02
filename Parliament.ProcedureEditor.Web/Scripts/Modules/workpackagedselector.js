define(["knockout", "jquery", "Scripts/text!template/workpackagedselector"], function (ko, $, htmlText) {
    return {
        viewModel: function (params) {
            var self = this;

            self.procedureWorkPackagedId = params.procedureWorkPackagedId;
            self.workPackagedList = params.workPackagedList;
            self.workPackagedThingName = ko.observable(null);
            self.searchWorkPackagedText = ko.observable(null);

            if (self.procedureWorkPackagedId() !== null) {
                $.getJSON(window.urls.getWorkPackaged.replace("{id}", self.procedureWorkPackagedId()), function (data) {
                    self.workPackagedThingName(data.WorkPackagedThingName);
                });
            }

            self.removeWorkPackaged = function () {
                self.procedureWorkPackagedId(null);
                self.workPackagedThingName("");
                self.searchWorkPackagedText("");
            };

            self.selectWorkPackaged = function (workPackaged) {
                self.procedureWorkPackagedId(workPackaged.Id);
                self.workPackagedThingName(workPackaged.WorkPackagedThingName);
                self.searchWorkPackagedText("");
            };

            self.filteredWorkPackagedList = ko.pureComputed(function () {
                var text = self.searchWorkPackagedText();
                if ((text !== null) && (text.length > 0))
                    return self.workPackagedList().filter(function (val) {
                        return val.WorkPackagedThingName.toUpperCase().indexOf(text.toUpperCase()) >= 0;
                    });
                else
                    return [];
            });

        },
        template: htmlText
    }
});