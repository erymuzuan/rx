define(['services/datacontext', 'services/logger', 'durandal/plugins/router'], function(context, logger, router) {

    var title = ko.observable(''),
        isBusy = ko.observable(false),
        activate = function() {
            logger.log('Commercial Space View Activated', null, 'commercialspace', true);
            title('Commercial Space');

            var tcs = new $.Deferred();
            context.loadAsync("CommercialSpace", "CommercialSpaceId gt 0").done(function(lo) {
                vm.commercialspaces(lo.itemCollection);
                tcs.resolve(true);
            });
            tcs.promise();
        },
        viewAttached = function(view) {
            bindEventToList(view, '#div-cs', gotoDetails);
        },
        bindEventToList = function(rootSelector, selector, callback, eventName) {
            var eName = eventName || 'click';
            $(rootSelector).on(eName, selector, function() {
                var building = ko.dataFor(this);
                callback(building);
                return false;
            });
        },
        gotoDetails = function(selectedCs) {
            if (selectedCs && selectedCs.CommercialSpaceId()) {
                var url = '/#/commercialspacedetail/' + selectedCs.BuildingId() + '/' + selectedCs.FloorName() + '/' + selectedCs.CommercialSpaceId();
                router.navigateTo(url);
            }
        };

    var vm = {
        activate: activate,
        isBusy : isBusy,
        commercialspaces: ko.observableArray([]),
        title: title,
        viewAttached : viewAttached
    };

    return vm;
});