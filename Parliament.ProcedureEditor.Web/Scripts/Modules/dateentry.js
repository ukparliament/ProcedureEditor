define(["knockout", "Scripts/text!template/dateentry"], function (ko, htmlText) {
    return {
        viewModel: function (params) {
            var self = this;

            self.date = params.date;
            self.day = ko.observable(null);
            self.month = ko.observable(null);
            self.year = ko.observable(null);
            self.dateInput = ko.observable(null);

            self.dateToString = ko.computed(function () {
                return self.year() + "-" + (self.month() * 1 < 10 ? "0" + (self.month() * 1) : self.month()) + "-" + (self.day() * 1 < 10 ? "0" + (self.day() * 1) : self.day());
            });

            var timeZoneOffset = new Date(
                Date.UTC(self.year(), self.month() - 1, self.day(), 0, 0, 0))
                .toLocaleString("en-GB", {
                    hour: "2-digit",
                    minute: "2-digit",
                    hour12: false,
                    timeZone: "Europe/London"
                });

            if (isNaN(Date.parse(self.date())) === false) {
                var dt = new Date(self.date());
                self.day(dt.getDate());
                self.month(dt.getMonth() + 1);
                self.year(dt.getFullYear());
                self.dateInput(self.dateToString());
            }            

            self.checkDate = ko.computed(function () {
                var dateTxt = self.dateToString() + "T00:00:00+" + timeZoneOffset;
                if (isNaN(Date.parse(dateTxt)) === false) {
                    self.date(dateTxt);
                    self.dateInput(self.dateToString());
                }
                else {
                    self.date(null);
                    self.dateInput(null);
                }
            });

            self.checkDateInput = ko.computed(function () {
                var dateTxt = self.dateInput() + "T00:00:00+" + timeZoneOffset;
                if (isNaN(Date.parse(dateTxt)) === false) {
                    self.date(dateTxt);
                    var dt = new Date(dateTxt);
                    self.day(dt.getDate());
                    self.month(dt.getMonth() + 1);
                    self.year(dt.getFullYear());
                }
                else {
                    self.day(null);
                    self.month(null);
                    self.year(null);
                    self.dateInput(null);
                }
            });

        },
        template: htmlText
    }
});