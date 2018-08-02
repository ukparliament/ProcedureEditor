define(["knockout", "Scripts/text!template/workpackagedlistitem"], function (ko, htmlText) {
    return {
        viewModel: function (params) {
            var self = this;

            self.workPackagedThingName = params.workPackaged.WorkPackagedThingName;
            self.statutoryInstrumentType = params.workPackaged.IsStatutoryInstrument ? "Statutory instrument paper" : "Proposed negative statutory instrument paper";
            self.procedureName = params.workPackaged.ProcedureName;
            self.mostRecentBusinessItemDate = isNaN(Date.parse(params.workPackaged.MostRecentBusinessItemDate)) ?
                'N/A' : new Date(params.workPackaged.MostRecentBusinessItemDate).toLocaleDateString('en-GB');

        },
        template: htmlText
    }
});