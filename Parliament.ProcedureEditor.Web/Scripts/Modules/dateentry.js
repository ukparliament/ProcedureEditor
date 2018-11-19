define(["knockout", "Scripts/text!template/dateentry"], function (ko, htmlText) {
    return {
        viewModel: function (params) {
            var self = this;

            self.date = params.date;
            self.day = ko.observable(null);
            self.month = ko.observable(null);
            self.year = ko.observable(null);

            if (isNaN(Date.parse(self.date())) === false) {
                self.day(new Date(self.date()).getDate());
                self.month(new Date(self.date()).getMonth() + 1);
                self.year(new Date(self.date()).getFullYear());
            }

            self.checkDate = ko.computed(function () {
                var timeZoneOffset = new Date(
                    Date.UTC(self.year(), self.month() - 1, self.day(), 0, 0, 0))
                    .toLocaleString("en-GB", {
                        hour: "2-digit",
                        minute: "2-digit",
                        hour12: false,
                        timeZone: "Europe/London"
                    });
                var dateTxt = self.year() + "-" + (self.month() * 1 < 10 ? "0" + (self.month() * 1) : self.month()) + "-" + (self.day() * 1 < 10 ? "0" + (self.day() * 1) : self.day()) + "T00:00:00+" + timeZoneOffset;
                if (isNaN(Date.parse(dateTxt)) === false)
                    self.date(dateTxt);
                else
                    self.date(null);
            });

        },
        template: htmlText
    }
});