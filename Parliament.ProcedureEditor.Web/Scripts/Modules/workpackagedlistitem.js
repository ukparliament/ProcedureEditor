define(["knockout", "Scripts/text!template/workpackagedlistitem"], function (ko, htmlText) {
    return {
        viewModel: function (params) {
            var self = this;

            self.workPackaged = params.workPackaged;
            self.mostRecentBusinessItemDate = isNaN(Date.parse(params.workPackaged.MostRecentBusinessItemDate)) ?
                'N/A' : new Date(params.workPackaged.MostRecentBusinessItemDate).toLocaleDateString('en-GB');

        },
        template: htmlText
    }
});