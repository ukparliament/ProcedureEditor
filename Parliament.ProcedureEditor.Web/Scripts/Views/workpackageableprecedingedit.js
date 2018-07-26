(function () {

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
        self.precedingProcedureWorkPackageableThingName = ko.observable(self.workPackageablePreceding.PrecedingProcedureWorkPackageableThingName);
        self.followingProcedureWorkPackageableThingName = ko.observable(self.workPackageablePreceding.FollowingProcedureWorkPackageableThingName);
        self.searchPrecedingText = ko.observable("");
        self.searchFollowingText = ko.observable("");

        self.workPackageables = [];
        $.getJSON(window.urls.getWorkPackages, function (data) {
            self.workPackageables=data;
        });

        self.removePreceding = function () {
            self.precedingProcedureWorkPackageableThingId(null);
            self.precedingProcedureWorkPackageableThingName("");
            self.searchPrecedingText("");
        };

        self.selectPreceding = function (data) {
            self.precedingProcedureWorkPackageableThingId(data.Id);
            self.precedingProcedureWorkPackageableThingName(data.ProcedureWorkPackageableThingName);
            self.searchPrecedingText("");
        };

        self.filteredPrecedings = ko.pureComputed(function () {
            var text = self.searchPrecedingText();
            if ((text !== null) && (text.length > 0))
                return self.workPackageables.filter(function (val) {
                    return val.ProcedureWorkPackageableThingName.toUpperCase().indexOf(text.toUpperCase()) >= 0;
                });
            else
                return [];
        });

        self.removeFollowing = function () {
            self.followingProcedureWorkPackageableThingId(null);
            self.followingProcedureWorkPackageableThingName("");
            self.searchFollowingText("");
        };

        self.selectFollowing = function (data) {
            self.followingProcedureWorkPackageableThingId(data.Id);
            self.followingProcedureWorkPackageableThingName(data.ProcedureWorkPackageableThingName);
            self.searchFollowingText("");
        };

        self.filteredFollowing = ko.pureComputed(function () {
            var text = self.searchFollowingText();
            if ((text !== null) && (text.length > 0))
                return self.workPackageables.filter(function (val) {
                    return val.ProcedureWorkPackageableThingName.toUpperCase().indexOf(text.toUpperCase()) >= 0;
                });
            else
                return [];
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
})();