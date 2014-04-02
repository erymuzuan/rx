/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(['services/datacontext', objectbuilders.system, 'plugins/router'],
    function (context, system, router) {

        var entities = ko.observableArray(),
            selected = ko.observable(new bespoke.sph.domain.EntityDefinition()),
            filters = ko.observableArray(),
            type = ko.observable(),
            isBusy = ko.observable(false),
            activate = function () {
                var query = String.format("IsPublished eq {0}", 1),
                    tcs = new $.Deferred();

                context.getListAsync("EntityDefinition", query, "Name")
                    .then(function (lo) {
                        isBusy(false);
                        entities(lo);

                        tcs.resolve(true);
                    });
                return tcs.promise();

            },
            attached = function (view) {

            },
            addFilter = function () {
                filters.push(new bespoke.sph.domain.Filter(system.guid()));
            },
            removeFilter = function (filter) {
                return function () {

                    filters.remove(filter);
                }
            };

        type.subscribe(function (v) {
            var query = String.format("Name eq '{0}'", v);
            var tcs = new $.Deferred();
            context.loadOneAsync("EntityDefinition", query)
                .done(function (b) {
                    selected(b);
                    filters.removeAll();
                    tcs.resolve(true);
                });

            return tcs.promise();
        });

        var vm = {
            removeFilter: removeFilter,
            addFilter: addFilter,
            filters: filters,
            selected: selected,
            type: type,
            entities: entities,
            isBusy: isBusy,
            activate: activate,
            attached: attached
        };

        return vm;

    });
