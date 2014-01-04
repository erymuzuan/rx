define(['services/datacontext', 'services/logger', 'plugins/router'],
    function (context, logger, router) {
        var activate = function () {
            var tcs = new $.Deferred();
            //context.loadAsync("ReportDefinition", "ReportDefinitionId gt 0").done(function (lo) {
            //    vm.reports(lo.itemCollection);
            //    tcs.resolve(true);
            //});
            tcs.resolve(true);
            return tcs.promise();
        },
        addNew = function () {
            var url = '/#/reportdefinition.edit-id.0/0';
            router.navigateTo(url);
            return {
                then: function () { }
            };
        };

        var vm = {
            activate: activate,
            reports: ko.observableArray([]),
            toolbar: {
                addNewCommand: addNew,
            }
        };

        return vm;

    });