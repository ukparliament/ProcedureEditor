requirejs(["/Scripts/main.js"], function (main) {
    requirejs(["knockout", "jquery"], function (ko, $) {
        var viewModel = function (layingItem) {
            var self = this;

            self.layingItem = layingItem;

            self.isNotValidResponse = ko.observable(false);
            self.businessItem = ko.observable({
                Id: self.layingItem.ProcedureBusinessItemId,
                ProcedureWorkPackagedId: self.layingItem.ProcedureWorkPackagedId
            });
            self.layingBodyId = ko.observable(self.layingItem.LayingBodyId);
            self.layingDate = ko.observable(self.layingItem.LayingDate);
            self.isDeletePopupVisible = ko.observable(false);
            self.warningText = "Are you sure you wish to delete " + self.layingItem.TripleStoreId + " business item?";
            self.searchLayingBodyText = ko.observable("");
            self.businessItems = ko.observableArray([]);
            self.workPackagedList = ko.observableArray([]);
            self.layingBodies = ko.observableArray([]);

            $.getJSON(window.urls.getBusinessItems, function (data) {
                self.businessItems(data);
            });

            $.getJSON(window.urls.getLayingBodies, function (data) {
                self.layingBodies(data);
            });

            $.getJSON(window.urls.getWorkPackagedList, function (data) {
                self.workPackagedList(data);
            });

            self.canSave = ko.computed(function () {
                return (self.businessItem() !== null) && (self.businessItem().Id !== 0) && (self.businessItem().ProcedureWorkPackagedId !== null);
            });

            self.save = function () {
                if (Number.isNaN(Number.parseInt(self.layingItem.Id)))
                    $.ajax(window.urls.addLayingItem, {
                        method: "POST",
                        dataType: "json",
                        data: {
                            ProcedureBusinessItemId: self.businessItem().Id,
                            ProcedureWorkPackagedId: self.businessItem().ProcedureWorkPackagedId,
                            LayingBodyId: self.layingBodyId(),
                            LayingDate: self.layingDate()
                        }
                    }).done(function (data) {
                        if (data === true)
                            window.location = window.urls.showLayingItems;
                        else
                            self.isNotValidResponse(true);
                    }).fail(function () {
                        self.isNotValidResponse(true);
                    });
                else
                    $.ajax(window.urls.updateLayingItem.replace("{id}", self.layingItem.Id), {
                        method: "PUT",
                        dataType: "json",
                        data: {
                            ProcedureBusinessItemId: self.businessItem().Id,
                            ProcedureWorkPackagedId: self.businessItem().ProcedureWorkPackagedId,
                            LayingBodyId: self.layingBodyId(),
                            LayingDate: self.layingDate()
                        }
                    }).done(function (data) {
                        if (data === true)
                            window.location = window.urls.showLayingItems;
                        else
                            self.isNotValidResponse(true);
                    }).fail(function () {
                        self.isNotValidResponse(true);
                    });
            };

            self.showDeletePopup = function () {
                self.isDeletePopupVisible(true);
            };

            self.deleteLayingItem = function () {
                $.ajax(window.urls.deleteLayingItem.replace("{id}", self.layingItem.Id), {
                    method: "DELETE",
                    dataType: "json"
                }).done(function (data) {
                    self.isDeletePopupVisible(false);
                    if (data === true)
                        window.location = window.urls.showLayingItems;
                    else
                        self.isNotValidResponse(true);
                }).fail(function () {
                    self.isDeletePopupVisible(false);
                    self.isNotValidResponse(true);
                });
            };
        };

        var layingItemId = $("#layingItemId").val();
        if (Number.isNaN(Number.parseInt(layingItemId)) === false)
            $.getJSON(window.urls.getLayingItem.replace("{id}", layingItemId), function (data) {
                var vm = new viewModel(data);
                ko.applyBindings(vm);
            });
        else {
            var bi = {
                Id: null,
                TripleStoreId: null,
                ProcedureBusinessItemId: null,
                ProcedureWorkPackagedId: null,
                LayingBodyId: null,
                LayingDate: null
            };
            var vm = new viewModel(bi);
            ko.applyBindings(vm);
        }
    });
});
