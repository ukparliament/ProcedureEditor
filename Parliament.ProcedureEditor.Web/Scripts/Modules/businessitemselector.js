define(["knockout", "jquery", "Scripts/text!template/businessitemselector"], function (ko, $, htmlText) {
    return {
        viewModel: function (params) {
            var self = this;

            self.procedureBusinessItemId = params.procedureBusinessItemId;
            self.businessItems = params.businessItems;
            self.businessItemName = ko.observable(null);
            self.searchBusinessItemText = ko.observable(null);

            if (self.businessItemId() !== null) {
                $.getJSON(window.urls.getBusinessItem.replace("{id}", self.businessItemId()), function (data) {
                    self.businessItemName(data.WorkPackagedThingName+"["+data.TripleStoreId+"]");
                });
            }

            self.removeBusinessItem = function () {
                self.procedureBusinessItemId(null);
                self.workPackagedThingName("");
                self.searchBusinessItemText("");
            };

            self.selectBusinessItem = function (businessItem) {
                self.procedureBusinessItemId(businessItem.Id);
                self.workPackagedThingName(businessItem.WorkPackagedThingName + "[" + businessItem.TripleStoreId + "]");
                self.searchBusinessItemText("");
            };

            self.filteredBusinessItems = ko.pureComputed(function () {
                var text = self.searchBusinessItemText();
                if ((text !== null) && (text.length > 0))
                    return self.businessItems().filter(function (val) {
                        return (val.WorkPackagedThingName.toUpperCase().indexOf(text.toUpperCase()) >= 0) ||
                            (val.TripleStoreId.toUpperCase().indexOf(text.toUpperCase()) >= 0);
                    });
                else
                    return [];
            });

        },
        template: htmlText
    }
});