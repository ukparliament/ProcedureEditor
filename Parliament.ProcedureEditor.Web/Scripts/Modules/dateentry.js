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
                var dateTxt = self.year() + "-" + (self.month() < 10 ? "0" + self.month() : self.month()) + "-" + (self.day() < 10 ? "0" + self.day() : self.day()) + "T00:00:00Z";
                if (isNaN(Date.parse(dateTxt)) === false)
                    self.date(dateTxt);
                else
                    self.date(null);
            });

        },
        template: htmlText
    }
});