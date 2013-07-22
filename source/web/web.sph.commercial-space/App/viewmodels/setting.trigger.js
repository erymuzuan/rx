/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext'],
    function (context) {

        var isBusy = ko.observable(false),
            activate = function () {
                return true;
            },
            save = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON({ comp: vm.complaint() });
                isBusy(true);

                context.post(data, "/Trigger/Save")
                    .then(function (result) {
                        isBusy(false);

                        tcs.resolve(result);
                    });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            trigger: ko.observable(new bespoke.sphcommercialspace.domain.Complaint()),
            saveCommand: save
        };

        return vm;

    });
