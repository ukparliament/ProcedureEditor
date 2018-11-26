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

    getWorkPackagedList: "/workpackage",
    getWorkPackagedListSearchByProcedure: "/workpackage?procedureId={procedureId}",
    getWorkPackaged: "/workpackage/{id}",
    getWorkPackagedByTripleStoreId: "/workpackage/{tripleStoreId}",
    addWorkPackaged: "/workpackage",
    updateWorkPackaged: "/workpackage/{id}",
    deleteWorkPackaged: "/workpackage/{id}",
    showWorkPackagedList: "/WorkPackage",
    showWorkPackaged: "/WorkPackage/{id}",

    getRoutes: "/route",
    addRoute: "/route",
    updateRoute: "/route/{id}",
    deleteRoute: "/route/{id}",
    getRoute: "/route/{id}",
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
    getStepsSearchByWorkPackaged: "/step/search",
    showSteps: "/Step",
    showStep: "/Step/{id}",

    getLayingBodies: "/layingbody",
    getLayingBody: "/layingbody/{id}",

    getBusinessItems: "/businessitem",
    addBusinessItem: "/businessitem",
    updateBusinessItem: "/businessitem/{id}",
    deleteBusinessItem: "/businessitem/{id}",
    getBusinessItem: "/businessitem/{id}",
    getBusinessItemsSearchByWorkPackaged: "/businessitem?workPackageId={workPackageId}",
    getBusinessItemsSearchByStep: "/businessitem?stepId={stepId}",
    showBusinessItems: "/BusinessItem",
    showBusinessItem: "/BusinessItem/{id}",

    getLayingItems: "/layingitem",
    addLayingItem: "/layingitem",
    updateLayingItem: "/layingitem/{id}",
    deleteLayingItem: "/layingitem/{id}",
    getLayingItem: "/layingitem/{id}",
    showLayingItems: "/LayingItem",
    showLayingItem: "/LayingItem/{id}",

    getHouses: "/house",

    getWorkPackagedPrecedings: "/workpackagepreceding",
    getWorkPackagedPreceding: "/workpackagepreceding/{id}",
    addWorkPackagedPreceding: "/workpackagepreceding",
    updateWorkPackagedPreceding: "/workpackagepreceding/{id}",
    deleteWorkPackagedPreceding: "/workpackagepreceding/{id}",
    showWorkPackagedPrecedings: "/WorkPackagePreceding",
    showWorkPackagedPreceding: "/WorkPackagePreceding/{id}",

    getSolrStatutoryInstruments: "/solrfeed/statutoryinstrument",
    getSolrBusinessItems: "/solrfeed/businessitem",
    getSolrStatutoryInstrument: "/solrfeed/statutoryinstrument/{id}",
    getSolrBusinessItem: "/solrfeed/businessitem/{id}",
    addSolrStatutoryInstrument: "/solrfeed/statutoryinstrument/{id}",
    addSolrBusinessItem: "/solrfeed/businessitem/{id}",
    deleteSolrStatutoryInstrument: "/solrfeed/statutoryinstrument/{id}",
    editSolrStatutoryInstrument: "/SolrFeed/StatutoryInstrument/Edit/{id}",
    editSolrBusinessItem: "/SolrFeed/BusinessItem/Edit/{id}",
    showSolrStatutoryInstruments: "/SolrFeed/StatutoryInstrument",
    showSolrBusinessItems: "/SolrFeed/BusinessItem"

};
window.urls = urls;

define(["knockout", "jquery"], function (ko, $) {
    $.ajaxSetup({ cache: false });
    ko.components.register("popup", { require: '/Scripts/Modules/popup.js' });
    ko.components.register("date-entry", { require: '/Scripts/Modules/dateentry.js' });
    ko.components.register("work-packaged-selector", { require: '/Scripts/Modules/workpackagedselector.js' });    
    ko.components.register("step-selector", { require: '/Scripts/Modules/stepselector.js' });    
    ko.components.register("work-packaged-list-item", { require: '/Scripts/Modules/workpackagedlistitem.js' });    
    ko.components.register("route-list-item", { require: '/Scripts/Modules/routelistitem.js' });    
    ko.components.register("business-item-list-item", { require: '/Scripts/Modules/businessitemlistitem.js' });
    ko.components.register("business-item-selector", { require: '/Scripts/Modules/businessitemselector.js' });    
    ko.components.register("laying-body-selector", { require: '/Scripts/Modules/layingbodyselector.js' });    
    ko.components.register("procedure-selector", { require: '/Scripts/Modules/procedureselector.js' });    
});