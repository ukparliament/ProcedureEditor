define(["knockout", "Scripts/text!template/routelistitem"], function (ko, htmlText) {
    return {
        viewModel: function (params) {
            var self = this;

            self.route = params.route;
        },
        template: htmlText
    }
});