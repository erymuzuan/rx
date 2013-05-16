define(['services/datacontext', 'services/logger', 'durandal/plugins/router'], function(context, logger, router) {

    var title = ko.observable(''),
        activate = function () {
            logger.log('Commercial Space View Activated', null, 'commercialspace', true);
            title('Commercial Space');

        var tcs = new $.Deferred();
        context.loadAsync("CommercialSpace", "CommercialSpaceId gt 0").done(function (lo) {
            vm.commercialspaces(lo.itemCollection);
            tcs.resolve(true);
        });
        tcs.promise();
    };

    var vm = {
        activate: activate,
        commercialspaces: ko.observableArray([]),
        title:title
    };

    return vm;
});