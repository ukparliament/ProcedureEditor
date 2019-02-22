define(["knockout", "jquery", "Scripts/text!template/governmentorganisationselector"], function (ko, $, htmlText) {
    return {
        viewModel: function (params) {
            var self = this;

            self.governmentOrganisationId = params.governmentOrganisationId;
            self.governmentOrganisations = ko.observableArray([]);
            self.groupName = ko.observable(null);
            self.searchGovernmentOrganisationText = ko.observable(null);

            $.ajax("https://api.parliament.uk/sparql", {
                method: "POST",
                dataType: "json",
                contentType: "application/sparql-query",
                accepts: {
                    json: "application/ld+json"
                },
                data: `PREFIX : <https://id.parliament.uk/schema/>
                    construct {
	                    ?s :groupName ?groupName.
                    }where {
	                    ?s a :GovernmentOrganisation;
		                    :groupName ?groupName.
                    }`
            }).done(function (data) {
                var arr = data.map(function (val) {
                    return {
                        id: val["@id"].substring(val["@id"].lastIndexOf("/") + 1),
                        groupName: val["https://id.parliament.uk/schema/groupName"][0]["@value"]
                    };
                });
                self.governmentOrganisations(arr);
                if (self.governmentOrganisationId() !== null) {
                    var id = self.governmentOrganisationId();
                    var org = self.governmentOrganisations().filter(function (val) {
                        return val.id === id;
                    });
                    if ((org !== null) && (org.length===1))
                        self.groupName(org[0].groupName);
                }
            });

            self.removeGovernmentOrganisation = function () {
                self.governmentOrganisationId(null);
                self.groupName("");
                self.searchGovernmentOrganisationText("");
            };

            self.selectGovernmentOrganisation = function (governmentOrganisation) {
                self.governmentOrganisationId(governmentOrganisation.id);
                self.groupName(governmentOrganisation.groupName);
                self.searchGovernmentOrganisationText("");
            };

            self.filteredGovernmentOrganisations = ko.pureComputed(function () {
                var text = self.searchGovernmentOrganisationText();
                if ((text !== null) && (text.length > 0))
                    return self.governmentOrganisations().filter(function (val) {
                        return val.groupName.toUpperCase().indexOf(text.toUpperCase()) >= 0;
                    });
                else
                    return [];
            });

        },
        template: htmlText
    }
});