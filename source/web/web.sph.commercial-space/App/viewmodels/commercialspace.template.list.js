define(['services/datacontext'], function (context) {
    var isBusy = ko.observable(false),
        activate = function () {
            var query = String.format("CommercialSpaceTemplateId gt 0");
            var tcs = new $.Deferred();
            context.loadAsync("CommercialSpaceTemplate", query)
                .then(function (lo) {
                    isBusy(false);
                    vm.csTemplates(lo.itemCollection);
                    tcs.resolve(true);
                });
            return tcs.promise();
        };


    var vm = {
        activate: activate,
        csTemplates: ko.observableArray()
    };

    return vm;
});
