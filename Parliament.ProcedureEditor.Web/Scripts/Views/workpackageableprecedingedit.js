requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function (workPackageablePreceding) {
            var self = this;
            if (workPackageablePreceding === null)
                self.workPackageablePreceding = {
                    Id: null,
                    PrecedingProcedureWorkPackageableThingId: null,
                    FollowingProcedureWorkPackageableThingId: null
                };
            else
                self.workPackageablePreceding = workPackageablePreceding;

            self.isNotValidResponse = ko.observable(false);
            self.precedingProcedureWorkPackageableThingId = ko.observable(self.workPackageablePreceding.PrecedingProcedureWorkPackageableThingId);
            self.followingProcedureWorkPackageableThingId = ko.observable(self.workPackageablePreceding.FollowingProcedureWorkPackageableThingId);
            self.workPackageables = ko.observableArray([]);

            $.getJSON(window.urls.getWorkPackages, function (data) {
                self.workPackageables(data);
            });

            self.isDeletePopupVisible = ko.observable(false);
            self.warningText = "Are you sure you wish to delete this work package preceding?";

            self.canSave = ko.computed(function () {
                return (self.precedingProcedureWorkPackageableThingId() !== null) &&
                    (self.followingProcedureWorkPackageableThingId() !== null);
            });

            self.save = function () {
                if (Number.isNaN(Number.parseInt(self.workPackageablePreceding.Id)))
                    $.ajax(window.urls.addWorkPackageablePreceding, {
                        method: "POST",
                        dataType: "json",
                        data: {
                            PrecedingProcedureWorkPackageableThingId: self.precedingProcedureWorkPackageableThingId(),
                            FollowingProcedureWorkPackageableThingId: self.followingProcedureWorkPackageableThingId()
                        }
                    }).done(function (data) {
                        if (data === true)
                            window.location = window.urls.showWorkPackageablePrecedings;
                        else
                            self.isNotValidResponse(true);
                    }).fail(function () {
                        self.isNotValidResponse(true);
                    });
                else
                    $.ajax(window.urls.updateWorkPackageablePreceding.replace("{id}", self.workPackageablePreceding.Id), {
                        method: "PUT",
                        dataType: "json",
                        data: {
                            PrecedingProcedureWorkPackageableThingId: self.precedingProcedureWorkPackageableThingId(),
                            FollowingProcedureWorkPackageableThingId: self.followingProcedureWorkPackageableThingId()
                        }
                    }).done(function (data) {
                        if (data === true)
                            window.location = window.urls.showWorkPackageablePrecedings;
                        else
                            self.isNotValidResponse(true);
                    }).fail(function () {
                        self.isNotValidResponse(true);
                    });
            };

            self.showDeletePopup = function () {
                self.isDeletePopupVisible(true);
            };

            self.deleteWorkPackageablePreceding = function () {
                $.ajax(window.urls.deleteWorkPackageablePreceding.replace("{id}", self.workPackageablePreceding.Id), {
                    method: "DELETE",
                    dataType: "json"
                }).done(function (data) {
                    self.isDeletePopupVisible(false);
                    if (data === true)
                        window.location = window.urls.showWorkPackageablePrecedings;
                    else
                        self.isNotValidResponse(true);
                }).fail(function () {
                    self.isDeletePopupVisible(false);
                    self.isNotValidResponse(true);
                });
            };
        };

        var workPackageablePrecedingId = $("#workPackageablePrecedingId").val();
        if (Number.isNaN(Number.parseInt(workPackageablePrecedingId)) === false)
            $.getJSON(window.urls.getWorkPackageablePreceding.replace("{id}", workPackageablePrecedingId), function (data) {
                var vm = new viewModel(data);
                ko.applyBindings(vm);
            });
        else {
            var vm = new viewModel(null);
            ko.applyBindings(vm);
        }
    });
});