(function () {

    var viewModel = function (params) {
        var self = this;
        self.search = params.search;
        self.placeholderText = params.placeholderText;
        self.searchText = params.searchText;
        self.searchLabel = params.searchLabel;

        if (params.canSearch)
            self.canSearch = params.canSearch;
        else
            self.canSearch = ko.pureComputed(function () {
                return self.searchText().length >= 3;
            });
    };

    ko.components.register("search", {
        template: {
            element: "search"
        },
        viewModel: viewModel,
        synchronous: true
    });
})();