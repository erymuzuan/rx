define(['services/datacontext'], function(context) {

    var title = ko.observable('Senarai Ruang Komersil'),
        isBusy = ko.observable(false),
        activate = function() {
            var tcs = new $.Deferred();
            context.loadAsync("CommercialSpace", "CommercialSpaceId gt 0").done(function(lo) {
                vm.commercialspaces(lo.itemCollection);
                tcs.resolve(true);
            });
           return tcs.promise();
        };

    var vm = {
        title: title,
        activate: activate,
        isBusy : isBusy,
        commercialspaces: ko.observableArray([])
    };

    return vm;
});