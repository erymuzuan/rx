/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(["services/datacontext", objectbuilders.system, objectbuilders.config],
    function (context, system, config) {
        var entities = ko.observableArray(),
            privateSearches = ko.observableArray(),
            sharedSearches = ko.observableArray(),
            selected = ko.observable(new bespoke.sph.domain.EntityDefinition()),
            filters = ko.observableArray(),
            type = ko.observable(),
            isBusy = ko.observable(false),
            activate = function () {
                var entitiesQuery = String.format("IsPublished eq {0}", 1),
                    tcs = new $.Deferred(),
                    privateSearchesQuery = String.format("OwnerType eq 'User' and Owner eq '{0}'", config.userName),
                    departmentSearchesQuery = String.format("OwnerType eq 'Department' and Owner eq '{0}'", config.profile.Department),
                    designationSearchesQuery = String.format("OwnerType eq 'Designation' and Owner eq '{0}'", config.profile.Designation);


                var privateTask = context.loadAsync("SearchDefinition", privateSearchesQuery),
                    departmentTask = context.loadAsync("SearchDefinition", departmentSearchesQuery),
                    designationTask = context.loadAsync("SearchDefinition", designationSearchesQuery),
                    entitiesTask = context.getListAsync("EntityDefinition", entitiesQuery, "Name");

                $.when(privateTask, departmentTask, designationTask, entitiesTask)
                    .done(function(plo,dplo,dslo, elo){
                        isBusy(false);

                        privateSearches(plo.itemCollection);
                        sharedSearches(dslo.itemCollection);
                        entities(elo);

                        tcs.resolve(true);
                    });
                return tcs.promise();

            },
            attached = function () {

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
            privateSearches: privateSearches,
            sharedSearches: sharedSearches,
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
