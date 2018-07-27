(function () {

    var viewModel = function (statutoryInstrument) {
        var self = this;
        self.statutoryInstrument = statutoryInstrument;
        self.procedureWorkPackageableThingName = ko.observable(statutoryInstrument.Title);
        self.statutoryInstrumentNumber = ko.observable(statutoryInstrument.SINumber);
        self.statutoryInstrumentNumberPrefix = ko.observable(statutoryInstrument.SIPrefix);
        self.statutoryInstrumentNumberYear = ko.observable(null);
        self.comingIntoForceNote = ko.observable(statutoryInstrument.ComingIntoForceNote);
        self.webLink = ko.observable(statutoryInstrument.WebUrl);
        self.procedureWorkPackageableThingTypeId = ko.observable(null);
        self.comingIntoForceDate = ko.observable(statutoryInstrument.ComingIntoForceDate);
        self.timeLimitForObjectionEndDate = ko.observable(null);
        self.procedureId = ko.observable(null)
        self.isNotValidResponse = ko.observable(false);

        self.procedures = ko.observableArray([]);
        $.getJSON(window.urls.getProcedures, function (data) {
            self.procedures(data);
        });

        self.types = ko.observableArray([]);
        $.getJSON(window.urls.getWorkPackageTypes, function (data) {
            self.types(data);
        });

        self.dateEntry = function (date) {
            var self = this;

            self.date = date;
            self.day = ko.observable(null);
            self.month = ko.observable(null);
            self.year = ko.observable(null);

            if (isNaN(Date.parse(self.date())) === false) {
                self.day(new Date(self.date()).getDate());
                self.month(new Date(self.date()).getMonth() + 1);
                self.year(new Date(self.date()).getFullYear());
            }

            self.checkDate = ko.computed(function () {
                var dateTxt = self.year() + "-" + (self.month() < 10 ? "0" + self.month() : self.month()) + "-" + (self.day() < 10 ? "0" + self.day() : self.day()) + "T00:00:00Z";
                if (isNaN(Date.parse(dateTxt)) === false)
                    self.date(dateTxt);
            });

        };

        self.objectionDate = new self.dateEntry(self.timeLimitForObjectionEndDate);
        self.forceDate = new self.dateEntry(self.comingIntoForceDate);

        self.canSave = ko.computed(function () {
            return (self.procedureWorkPackageableThingName().length > 0) &&
                (self.procedureId() !== null);
        });

        self.save = function () {
            $.ajax(window.urls.addSolrStatutoryInstrument.replace("{id}", self.statutoryInstrument.Id), {
                method: "POST",
                dataType: "json",
                data: {
                    ProcedureWorkPackageableThingName: self.procedureWorkPackageableThingName(),
                    StatutoryInstrumentNumber: self.statutoryInstrumentNumber(),
                    StatutoryInstrumentNumberPrefix: self.statutoryInstrumentNumberPrefix(),
                    StatutoryInstrumentNumberYear: self.statutoryInstrumentNumberYear(),
                    ComingIntoForceNote: self.comingIntoForceNote(),
                    WebLink: self.webLink(),
                    ProcedureWorkPackageableThingTypeId: self.procedureWorkPackageableThingTypeId(),
                    ComingIntoForceDate: self.comingIntoForceDate(),
                    TimeLimitForObjectionEndDate: self.timeLimitForObjectionEndDate(),
                    ProcedureId: self.procedureId()
                }
            }).done(function (data) {
                if (data === true)
                    window.location = window.urls.showSolrStatutoryInstruments;
                else
                    self.isNotValidResponse(true);
            }).fail(function () {
                self.isNotValidResponse(true);
            });
        };
        
    };

    var statutoryInstrumentId = $("#statutoryInstrumentId").val();
    if (Number.isNaN(Number.parseInt(statutoryInstrumentId)) === false)
        $.getJSON(window.urls.getSolrStatutoryInstrument.replace("{id}", statutoryInstrumentId), function (data) {
            var vm = new viewModel(data);
            ko.applyBindings(vm);
        });
    else {
        var vm = new viewModel(null);
        ko.applyBindings(vm);
    }
})();