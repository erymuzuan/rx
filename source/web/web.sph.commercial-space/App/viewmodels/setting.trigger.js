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
            
            editedRule = ko.observable(),
            showDetails = function (rule) {
                isBusy(true);
                var c1 = ko.mapping.fromJSON(ko.mapping.toJSON(rule));
                var clone = _(c1).extend(new bespoke.sphcommercialspace.domain.DepositPartial(c1));
                editedRule(rule);
                vm.trigger(clone);

                $('#deposit-modal').modal({});
            },
            save = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON({ comp: vm.trigger() });
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
            functionField : ko.observable(new bespoke.sphcommercialspace.domain.FunctionField()),
            constantField : ko.observable(new bespoke.sphcommercialspace.domain.ConstantField()),
            trigger: ko.observable(new bespoke.sphcommercialspace.domain.Trigger()),
            rule : ko.observable(new bespoke.sphcommercialspace.domain.Rule()),
            saveCommand: save
        };

        return vm;

    });
