/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext'],
    function (context) {

        var isBusy = ko.observable(false),
            activate = function () {
                var query = String.format("Key eq 'Organization'");
                var tcs = new $.Deferred();
                var orgTask = context.loadOneAsync("Setting", query);
                var stateTask = context.loadOneAsync("Setting","Key eq 'State'")
                $.when(orgTask,stateTask).done(function (o,s) {
                        if (o) {
                            var organization = JSON.parse(ko.mapping.toJS(o.Value));
                            vm.organization(organization);
                        }
                        if (s) {
                            var states = JSON.parse(s.Value());
                            vm.stateOptions(states);
                    }
                        tcs.resolve(true);
                    });

                return tcs.promise();
            },
            save = function () {
                var tcs = new $.Deferred();
                var data = JSON.stringify({
                    settings: [{
                        Key: "Organization",
                        Value: ko.mapping.toJSON(vm.organization())
                    }]
                });;
                isBusy(true);
                context.post(data, "/Setting/Save")
                    .then(function(result) {
                        isBusy(false);

                        tcs.resolve(result);
                    });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            organization: ko.observable(new bespoke.sphcommercialspace.domain.Organization()),
            stateOptions: ko.observableArray(),
            toolbar: { saveCommand: save }
        };

        return vm;

    });
