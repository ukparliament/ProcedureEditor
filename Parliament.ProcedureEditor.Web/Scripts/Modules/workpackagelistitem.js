define(["knockout", "Scripts/text!template/workpackagelistitem"], function (ko, htmlText) {
    return {
        viewModel: function (params) {
            var self = this;

            self.procedureWorkPackageableThingName = params.workPackageable.ProcedureWorkPackageableThingName;
            self.procedureName = params.workPackageable.ProcedureName;
            self.mostRecentBusinessItemDate = isNaN(Date.parse(params.workPackageable.MostRecentBusinessItemDate)) ?
                'N/A' : new Date(params.workPackageable.MostRecentBusinessItemDate).toLocaleDateString('en-GB');

        },
        template: htmlText
    }
});