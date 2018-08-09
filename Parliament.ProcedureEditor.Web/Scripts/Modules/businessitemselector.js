define(["knockout", "jquery", "Scripts/text!template/businessitemselector"], function (ko, $, htmlText) {
    return {
        viewModel: function (params) {
            var self = this;

            self.businessItem = params.businessItem;
            self.businessItems = params.businessItems;
            self.selectedItem = params.selectedItem;
            self.businessItemName = ko.observable(null);
            self.searchBusinessItemText = ko.observable(null);

            if (self.businessItem().Id !== null) {
                $.getJSON(window.urls.getBusinessItem.replace("{id}", self.businessItem().Id), function (data) {
                    self.businessItemName(data.WorkPackagedThingName+"["+data.TripleStoreId+"]");
                });
            }

            self.canSearch = ko.pureComputed(function () {
                return (self.businessItem() === null) || (self.businessItem().Id === null);
            });

            self.removeBusinessItem = function () {
                self.businessItem(null);
                self.businessItemName("");
                self.searchBusinessItemText("");
            };

            self.selectBusinessItem = function (businessItem) {
                self.businessItem(businessItem);
                self.businessItemName(businessItem.WorkPackagedThingName + "[" + businessItem.TripleStoreId + "]");
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