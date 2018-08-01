define(["knockout", "jquery", "Scripts/text!template/popup"], function (ko, $, htmlText) {
    return {
        viewModel: function (params) {            
            var self = this;
            self.popupText = params.popupText;
            self.isPopupVisible = params.isPopupVisible;
            self.action = params.action;
            self.actionText = params.actionText;

            self.close = function () {
                self.isPopupVisible(false);
                $("body").removeClass("modal-open");
            };

            self.show = ko.computed(function () {
                if (self.isPopupVisible())
                    $("body").addClass("modal-open");
            });

        },
        template: htmlText
    }
});