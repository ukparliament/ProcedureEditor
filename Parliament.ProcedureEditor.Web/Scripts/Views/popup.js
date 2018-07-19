(function () {
    var viewModel = function (params) {
        var self = this;
        self.popupText = params.popupText;
        self.isPopupVisible = params.isPopupVisible;
        self.action = params.action;
        self.actionText = params.actionText;

        self.close = function () {
            self.isPopupVisible(false);
            $("body").removeClass("modal-open");
        };

        self.show = ko.pureComputed(function () {
            if (self.isPopupVisible()) 
                $("body").addClass("modal-open");
        });
    };

    ko.components.register("popup", {
        template: {
            element: "popup"
        },
        viewModel: viewModel,
        synchronous: true
    });
})();