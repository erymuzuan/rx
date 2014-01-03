/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'config'],
    function (context, config) {

        var isBusy = ko.observable(false),
            activate = function () {
                var query = String.format("UserName eq '{0}'", config.userName);
                var tcs = new $.Deferred();

                context.loadAsync({
                    entity: "Message",
                    orderby: "CreatedDate desc"
                }, query)
                    .then(function (lo) {
                        isBusy(false);
                        var sorted = lo.itemCollection.sort(function(x, y) { return x.CreatedDate() > y.CreatedDate(); });
                        vm.messages(sorted);
                        tcs.resolve(true);
                    });
                return tcs.promise();


            },
            resetFilter = function (ev) {
                $('ul#filter-messages>li').removeClass('active');
                if (ev.target) {
                    $(ev.target.parentNode).addClass('active');
                }
            },
            filter = function (options) {
                options = options || {};
                options.read = includeRead();

                var query = String.format("UserName eq '{0}'", config.userName);
                if (options.start) {
                    query += " and CreatedDate ge DateTime'" + options.start + "'";
                }
                if (options.end) {
                    query += " and CreatedDate le DateTime'" + options.end + "'";
                }
                if (!options.read) {
                    query += " and IsRead eq 0";
                }
                var tcs = new $.Deferred();

                context.loadAsync({
                    entity: "Message",
                    orderby: "CreatedDate desc"
                }, query)
                    .then(function (lo) {
                        isBusy(false);

                        vm.messages(lo.itemCollection);
                        tcs.resolve(true);
                    });
                return tcs.promise();

            },
            attached = function (view) {
            },
            thisWeek = function (d, ev) {
                resetFilter(ev);
                filter({ start: moment().day("Sunday").format('YYYY-MM-DD') });
            },
            thisMonth = function (d, ev) {
                resetFilter(ev);
                filter({ start: moment().startOf('month').format('YYYY-MM-DD') });
            },
            older = function (d, ev) {
                resetFilter(ev);
                filter({ end: moment().startOf('month').format('YYYY-MM-DD') });
            },
            includeRead = ko.observable(false);

        var vm = {
            isBusy: isBusy,
            activate: activate,
            includeRead: includeRead,
            thisWeek: thisWeek,
            thisMonth: thisMonth,
            older: older,
            attached: attached,
            messages: ko.observableArray()
        };

        return vm;

    });
