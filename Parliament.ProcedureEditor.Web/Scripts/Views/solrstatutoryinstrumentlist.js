(function () {
    var viewModel = function (statutoryInstruments) {
        var self = this;

        self.statutoryInstruments = ko.observableArray(statutoryInstruments);
        self.isNotValidResponse = ko.observable(false);

        self.deleteStatutoryInstrument = function (statutoryInstrument) {
            $.ajax(window.urls.deleteSolrStatutoryInstrument.replace("{id}", statutoryInstrument.Id), {
                method: "DELETE",
                dataType: "json"
            }).done(function (data) {
                if (data === true)
                    self.statutoryInstruments.remove(statutoryInstrument);
                else
                    self.isNotValidResponse(true);
            }).fail(function () {
                self.isNotValidResponse(true);
            });
        };
        
    };

    $.getJSON(window.urls.getSolrStatutoryInstruments, function (data) {
        var vm = new viewModel(data);
        ko.applyBindings(vm);
    });
})();