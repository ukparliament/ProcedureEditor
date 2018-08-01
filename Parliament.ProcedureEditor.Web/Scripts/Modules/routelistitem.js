define(["knockout", "Scripts/text!template/routelistitem"], function (ko, htmlText) {
    return {
        viewModel: function (params) {
            var self = this;

            self.procedureName = params.route.ProcedureName;
            self.procedureRouteTypeName = params.route.ProcedureRouteTypeName;
            self.fromProcedureStepName = params.route.FromProcedureStepName;
            self.toProcedureStepName = params.route.ToProcedureStepName;

        },
        template: htmlText
    }
});