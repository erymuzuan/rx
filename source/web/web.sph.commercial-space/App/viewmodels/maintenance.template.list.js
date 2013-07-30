define(['services/datacontext'], function (context) {
    var isBusy = ko.observable(false),
        activate = function () {
            var query = String.format("MaintenanceTemplateId gt 0");
            var tcs = new $.Deferred();
            context.loadAsync("MaintenanceTemplate", query)
                .then(function (lo) {
                    isBusy(false);
                    vm.maintenanceTemplates(lo.itemCollection);

                    tcs.resolve(true);
                });
            return tcs.promise();
        };


    var vm = {
        activate: activate,
        maintenanceTemplates: ko.observableArray()
    };

    return vm;
});
