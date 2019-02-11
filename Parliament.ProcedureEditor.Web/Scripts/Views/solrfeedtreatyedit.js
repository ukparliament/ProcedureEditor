requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function (treaty) {
            var self = this;
            self.treaty = treaty;
            self.workPackagedThingName = ko.observable(treaty.Title);
            self.webLink = ko.observable(treaty.WebUrl);
            self.procedureWorkPackageableThingTypeId = ko.observable(null);
            self.procedureName = ko.observable(null);
            self.procedureId = ko.observable(null);
            self.isNotValidResponse = ko.observable(false);
            self.warningText = "Are you sure you wish to delete '" + treaty.Title + "' record?";
            self.isBeingSaved = ko.observable(false);
            self.isDeletePopupVisible = ko.observable(false);

            $.getJSON(window.urls.getProcedures, function (data) {
                for (i = 0; i < data.length; i++)
                    if (data[i].TripleStoreId === "D00dsjR2") {
                        self.procedureId(data[i].Id);
                        self.procedureName(data[i].ProcedureName);
                        break;
                    }                
            });

            self.showDeletePopup = function () {
                self.isDeletePopupVisible(true);
            };

            self.canSave = ko.computed(function () {
                return (self.workPackagedThingName().length > 0) &&
                    (self.procedureId() !== null) &&
                    (self.isBeingSaved() === false);
            });

            self.save = function () {
                self.isBeingSaved(true);
                $.ajax(window.urls.addSolrTreaty.replace("{id}", self.treaty.Id), {
                    method: "POST",
                    dataType: "json",
                    data: {
                        WorkPackagedThingName: self.workPackagedThingName(),
                        WebLink: self.webLink(),
                        ProcedureId: self.procedureId()
                    }
                }).done(function (data) {
                    if (data === true)
                        window.location = window.urls.showSolrTreaties;
                    else {
                        self.isNotValidResponse(true);
                        self.isBeingSaved(false);
                    }
                }).fail(function () {
                    self.isNotValidResponse(true);
                    self.isBeingSaved(false);
                });
            };

            self.deleteTreaty = function () {
                $.ajax(window.urls.deleteSolrTreaty.replace("{id}", self.treaty.Id), {
                    method: "DELETE",
                    dataType: "json"
                }).done(function (data) {
                    self.isDeletePopupVisible(false);
                    if (data === true)
                        window.location = window.urls.showSolrTreaties;
                    else
                        self.isNotValidResponse(true);
                }).fail(function () {
                    self.isDeletePopupVisible(false);
                    self.isNotValidResponse(true);
                });
            };

        };

        var treatyId = $("#treatyId").val();
        if (Number.isNaN(Number.parseInt(treatyId)) === false)
            $.getJSON(window.urls.getSolrTreaty.replace("{id}", treatyId), function (data) {
                var vm = new viewModel(data);
                ko.applyBindings(vm);
            });
        else {
            var vm = new viewModel(null);
            ko.applyBindings(vm);
        }
    });
});