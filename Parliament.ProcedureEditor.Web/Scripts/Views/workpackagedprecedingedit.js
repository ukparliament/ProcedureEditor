﻿requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function (workPackagedPreceding) {
            var self = this;
            if (workPackagedPreceding === null)
                self.workPackagedPreceding = {
                    Id: null,
                    WorkPackagedIsFollowedById: null,
                    WorkPackagedIsPrecededById: null
                };
            else
                self.workPackagedPreceding = workPackagedPreceding;

            self.isNotValidResponse = ko.observable(false);
            self.workPackagedIsFollowedById = ko.observable(self.workPackagedPreceding.WorkPackagedIsFollowedById);
            self.workPackagedIsPrecededById = ko.observable(self.workPackagedPreceding.WorkPackagedIsPrecededById);
            self.procedureProposedNegativeStatutoryInstruments = ko.observableArray([]);
            self.procedureStatutoryInstruments = ko.observableArray([]);

            $.getJSON(window.urls.getProcedures, function (data) {
                var negativeId = null;
                for (i = 0; i < data.length; i++) {
                    if (data[i].TripleStoreId === "iCdMN1MW")
                        negativeId = data[i].Id;
                }
                $.getJSON(window.urls.getWorkPackagedList, function (data) {
                    self.procedureStatutoryInstruments(
                        data.filter(function (wp) {
                            return wp.ProcedureId !== negativeId;
                        }));
                    self.procedureProposedNegativeStatutoryInstruments(
                        data.filter(function (wp) {
                            return wp.ProcedureId === negativeId;
                        }));
                });
            });

            self.isDeletePopupVisible = ko.observable(false);
            self.warningText = "Are you sure you wish to delete this work package preceding?";
            self.isBeingSaved = ko.observable(false);

            self.canSave = ko.computed(function () {
                return (self.workPackagedIsFollowedById() !== null) &&
                    (self.workPackagedIsPrecededById() !== null) &&
                    (self.isBeingSaved() === false);
            });

            self.save = function () {
                self.isBeingSaved(true);
                if (Number.isNaN(Number.parseInt(self.workPackagedPreceding.Id)))
                    $.ajax(window.urls.addWorkPackagedPreceding, {
                        method: "POST",
                        dataType: "json",
                        data: {
                            WorkPackagedIsFollowedById: self.workPackagedIsFollowedById(),
                            WorkPackagedIsPrecededById: self.workPackagedIsPrecededById()
                        }
                    }).done(function (data) {
                        if (data === true)
                            window.location = window.urls.showWorkPackagedPrecedings;
                        else {
                            self.isNotValidResponse(true);
                            self.isBeingSaved(false);
                        }
                    }).fail(function () {
                        self.isNotValidResponse(true);
                        self.isBeingSaved(false);
                    });
                else
                    $.ajax(window.urls.updateWorkPackagedPreceding.replace("{id}", self.workPackagedPreceding.Id), {
                        method: "PUT",
                        dataType: "json",
                        data: {
                            WorkPackagedIsFollowedById: self.workPackagedIsFollowedById(),
                            WorkPackagedIsPrecededById: self.workPackagedIsPrecededById()
                        }
                    }).done(function (data) {
                        if (data === true)
                            window.location = window.urls.showWorkPackagedPrecedings;
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

            self.deleteWorkPackagedPreceding = function () {
                $.ajax(window.urls.deleteWorkPackagedPreceding.replace("{id}", self.workPackagedPreceding.Id), {
                    method: "DELETE",
                    dataType: "json"
                }).done(function (data) {
                    self.isDeletePopupVisible(false);
                    if (data === true)
                        window.location = window.urls.showWorkPackagedPrecedings;
                    else
                        self.isNotValidResponse(true);
                }).fail(function () {
                    self.isDeletePopupVisible(false);
                    self.isNotValidResponse(true);
                });
            };
        };

        var workPackagedPrecedingId = $("#workPackagedPrecedingId").val();
        if (Number.isNaN(Number.parseInt(workPackagedPrecedingId)) === false)
            $.getJSON(window.urls.getWorkPackagedPreceding.replace("{id}", workPackagedPrecedingId), function (data) {
                var vm = new viewModel(data);
                ko.applyBindings(vm);
            });
        else {
            var vm = new viewModel(null);
            ko.applyBindings(vm);
        }
    });
});