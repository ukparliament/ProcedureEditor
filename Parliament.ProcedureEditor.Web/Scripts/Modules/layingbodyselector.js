define(["knockout", "jquery", "Scripts/text!template/layingbodyselector"], function (ko, $, htmlText) {
    return {
        viewModel: function (params) {
            var self = this;

            self.layingBodyId = params.layingBodyId;
            self.layingBodies = params.layingBodies;
            self.layingBodyName = ko.observable(null);
            self.searchLayingBodyText = ko.observable(null);

            if (self.layingBodyId() !== null) {
                $.getJSON(window.urls.getLayingBody.replace("{id}", self.layingBodyId()), function (data) {
                    self.layingBodyName(data.LayingBodyName);
                });
            }

            self.removeLayingBody = function () {
                self.layingBodyId(null);
                self.layingBodyName("");
                self.searchLayingBodyText("");
            };

            self.selectLayingBody = function (layingBody) {
                self.layingBodyId(layingBody.Id);
                self.layingBodyName(layingBody.LayingBodyName);
                self.searchLayingBodyText("");
            };

            self.filteredLayingBodies = ko.pureComputed(function () {
                var text = self.searchLayingBodyText();
                if ((text !== null) && (text.length > 0))
                    return self.layingBodies().filter(function (val) {
                        return val.LayingBodyName.toUpperCase().indexOf(text.toUpperCase()) >= 0;
                    });
                else
                    return [];
            });

        },
        template: htmlText
    }
});