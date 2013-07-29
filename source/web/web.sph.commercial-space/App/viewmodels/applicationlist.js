/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger', 'durandal/plugins/router'], function (context, logger, router) {
    var status = ko.observable(),
        isBusy = ko.observable(false),
        activate = function (routedata) {
            isBusy(true);
            status(routedata.status);
            var header = 'Senarai Permohonan ' + routedata.status;
            vm.title(header);
            var tcs = new $.Deferred();
            var query = String.format("Status eq '{0}'", status());
            context.loadAsync("RentalApplication", query).done(function (lo) {
                vm.applications.removeAll();
                vm.applications(lo.itemCollection);
                tcs.resolve(true);
                isBusy(false);

                vm.statusbar().text(lo.itemCollection.length + " items");
            });

            return tcs.promise();
        },
    viewAttached = function () {
        var $links = $('#app-list-table>tbody>tr');
        var filterInput = $('#appra-filter-text');

        var dofilter = function () {
            var filter = filterInput.val().toLowerCase();
            $links.each(function () {
                var $tr = $(this);
                
                if ($tr.text().toLowerCase().indexOf(filter) > -1) {
                    $tr.show();
                } else {
                    $tr.hide();
                }
            });

        };
        var throttled = _.throttle(dofilter, 800);
        filterInput.on('keyup', throttled).siblings('.icon-remove')
            .click(function () {
                filterInput.val('');
                dofilter();
            });

        if (filterInput.val()) {
            dofilter();
        }

    },
        printList = function () { },
        exportList = function () { }
    ;

    var vm = {
        status: status,
        isBusy: isBusy,
        activate: activate,
        viewAttached: viewAttached,
        title: ko.observable(),
        applications: ko.observableArray([]),
        toolbar: ko.observable({
            reloadCommand: function () {
                return activate({ status: status() });
            },
            printCommand: printList,
            exportCommand: exportList,
        }),
        statusbar: ko.observable({
            text: ko.observable()
        })
    };
    return vm;
});
