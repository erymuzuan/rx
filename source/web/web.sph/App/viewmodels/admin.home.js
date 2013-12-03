/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger', 'durandal/plugins/router'],
    function (context, logger, router) {

        var isBusy = ko.observable(false),
            activate = function () {
                var groups2 = _(router.allRoutes()).chain()
                    .map(function (v) {
                        return v.groupName;
                    })
                    .uniq()
                .map(function (g) {
                    return {
                        groupName: g,
                        routes: _(router.allRoutes()).filter(function (v) { return v.groupName === g && v.isAdminPage; })
                    };
                }).
                filter(function (v) { return v.groupName && v.routes.length; })
                    .value();
                groups(groups2);

            },
            viewAttached = function (view) {

            },
            groups = ko.observableArray();

        var vm = {
            isBusy: isBusy,
            activate: activate,
            viewAttached: viewAttached,
            groups: groups
        };

        return vm;

    });
