define(["knockout", "jquery", "Scripts/text!template/seriesmembershipeditor"], function (ko, $, htmlText) {
    return {
        viewModel: function (params) {
            var self = this;

            self.seriesCitation = params.seriesCitation;
            self.seriesMembershipId = params.seriesMembershipId;
            self.seriesTreatyCitation = params.seriesTreatyCitation;
            self.isTreatySeriesMembership = params.isTreatySeriesMembership;

        },
        template: htmlText
    }
});