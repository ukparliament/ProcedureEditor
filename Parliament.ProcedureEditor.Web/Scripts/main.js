requirejs.config({
    baseUrl: "/",
    paths: {
        "jquery": "Scripts/jquery-3.3.1.min",
        "knockout": "Scripts/knockout-3.4.2"
    }
});

var urls = {
    getProcedures: "/procedure",
    addProcedure: "/procedure",
    updateProcedure: "/procedure/{id}",
    deleteProcedure: "/procedure/{id}",
    getProcedure: "/procedure/{id}",
    showProcedures: "/Procedure",
    showProcedure: "/Procedure/{id}",

    getWorkPackages: "/workpackage",
    getWorkPackagesSearchByProcedure: "/workpackage?procedureId={procedureId}",
    getWorkPackagesSearch: "/workpackage?searchText={searchText}",
    getWorkPackage: "/workpackage/{id}",
    addWorkPackage: "/workpackage",
    updateWorkPackage: "/workpackage/{id}",
    deleteWorkPackage: "/workpackage/{id}",
    showWorkPackages: "/WorkPackage",
    showWorkPackage: "/WorkPackage/{id}",

    getWorkPackageTypes: "/workpackageabletype",

    getRoutes: "/route",
    addRoute: "/route",
    updateRoute: "/route/{id}",
    deleteRoute: "/route/{id}",
    getRoute: "/route/{id}",
    getRoutesSearch: "/route?searchText={searchText}",
    getRoutesSearchByProcedure: "/route?procedureId={procedureId}",
    getRoutesSearchByStep: "/route?stepId={stepId}",
    showRoutes: "/Route",
    showRoute: "/Route/{id}",

    getRouteTypes: "/routetype",

    getSteps: "/step",
    addStep: "/step",
    updateStep: "/step/{id}",
    deleteStep: "/step/{id}",
    getStep: "/step/{id}",
    getStepsSearch: "/step?searchText={searchText}",
    getStepsSearchByWorkPackage: "/step?workPackageId={workPackageId}",
    showSteps: "/Step",
    showStep: "/Step/{id}",

    getLayingBodies: "/layingbody",

    getBusinessItems: "/businessitem",
    addBusinessItem: "/businessitem",
    updateBusinessItem: "/businessitem/{id}",
    deleteBusinessItem: "/businessitem/{id}",
    getBusinessItem: "/businessitem/{id}",
    getBusinessItemsSearch: "/businessitem?searchText={searchText}",
    getBusinessItemsSearchByWorkPackage: "/businessitem?workPackageId={workPackageId}",
    getBusinessItemsSearchByStep: "/businessitem?stepId={stepId}",
    showBusinessItems: "/BusinessItem",
    showBusinessItem: "/BusinessItem/{id}",

    getHouses: "/house",

    getWorkPackageablePrecedings: "/workpackagepreceding",
    getWorkPackageablePreceding: "/workpackagepreceding/{id}",
    addWorkPackageablePreceding: "/workpackagepreceding",
    updateWorkPackageablePreceding: "/workpackagepreceding/{id}",
    deleteWorkPackageablePreceding: "/workpackagepreceding/{id}",
    showWorkPackageablePrecedings: "/WorkPackagePreceding",
    showWorkPackageablePreceding: "/WorkPackagePreceding/{id}",

    getSolrStatutoryInstruments: "/solrfeed",
    getSolrStatutoryInstrument: "/solrfeed/{id}",
    addSolrStatutoryInstrument: "/solrfeed/{id}",
    deleteSolrStatutoryInstrument: "/solrfeed/{id}",
    editSolrStatutoryInstruments: "/SolrFeed/Edit/{id}"

};
window.urls = urls;

define(["knockout"], function (ko) {
    ko.components.register("popup", { require: '/Scripts/Modules/popup.js' });
    ko.components.register("date-entry", { require: '/Scripts/Modules/dateentry.js' });
    ko.components.register("work-package-selector", { require: '/Scripts/Modules/workpackageselector.js' });    
    ko.components.register("step-selector", { require: '/Scripts/Modules/stepselector.js' });    
    ko.components.register("work-package-list-item", { require: '/Scripts/Modules/workpackagelistitem.js' });    
    ko.components.register("route-list-item", { require: '/Scripts/Modules/routelistitem.js' });    
    ko.components.register("business-item-list-item", { require: '/Scripts/Modules/businessitemlistitem.js' });    
});