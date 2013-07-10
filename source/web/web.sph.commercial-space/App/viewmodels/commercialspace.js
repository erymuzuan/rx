define(['services/datacontext'], function(context) {

    var title = ko.observable(''),
        isBusy = ko.observable(false),
        activate = function() {
            var tcs = new $.Deferred();
            context.loadAsync("CommercialSpace", "CommercialSpaceId gt 0").done(function(lo) {
                vm.commercialspaces(lo.itemCollection);
                tcs.resolve(true);
            });
            tcs.promise();
        };

    var vm = {
        activate: activate,
        isBusy : isBusy,
        commercialspaces: ko.observableArray([])
    };

    return vm;
});