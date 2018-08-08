define(["knockout", "Scripts/text!template/businessitemlistitem"], function (ko, htmlText) {
    return {
        viewModel: function (params) {
            var self = this;

            self.businessItem = params.businessItem;
            self.businessItemDate = isNaN(Date.parse(params.businessItem.BusinessItemDate)) ?
                'N/A' : new Date(params.businessItem.BusinessItemDate).toLocaleDateString('en-GB');


        },
        template: htmlText
    }
});