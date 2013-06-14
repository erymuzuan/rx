/// <reference path="../../Scripts/jquery-2.0.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../services/datacontext.js" />


define(['services/datacontext', 'services/logger'],
    function (context, logger) {

        var isBusy = ko.observable(false),
            activate = function () {
                var query = String.format("Key eq 'Rebate'");
                var tcs = new $.Deferred();
                context.loadAsync("Setting", query)
                    .then(function (lo) {
                        isBusy(false);
                        vm.settingCollection(lo.itemCollection);
                        tcs.resolve(true);
                    });
                return tcs.promise();
            },

            removeRebate = function (rebate) {
                vm.setting().RebateCollection.remove(rebate);
            },

	        addRebate = function () {
	            var rebate = new bespoke.sphcommercialspace.domain.Rebate();
	            vm.setting().RebateCollection.push(rebate);
	        },

            save = function () {
                var tcs = new $.Deferred(),
                    rebates = ko.mapping.toJS(vm.setting().RebateCollection()),
                    postdata = JSON.stringify({ id: vm.setting().SettingId(), rebates: rebates });
                context.post(postdata, "/Setting/SaveRebates")
                    .done(function () {
                        tcs.resolve(true);
                    })
                    .done(function (e) {
                        logger.log("New Rebate saved", e, "setting", true);

                    })
                    .fail(function () {
                        logger.logError("whoaaaaa");
                    });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            settingCollection: ko.observableArray(),
            setting: ko.observable(new bespoke.sphcommercialspace.domain.Setting()),
            addRebateCommand: addRebate,
            removeRebateCommand: removeRebate,
            saveCommand: save
        };


        return vm;

    });
