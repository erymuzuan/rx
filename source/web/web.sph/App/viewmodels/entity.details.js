/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../objectbuilders.js" />


define(['services/datacontext', 'services/logger', 'durandal/plugins/router', objectbuilders.system],
    function (context, logger, router, system) {

        var entity = ko.observable(),
            isBusy = ko.observable(false),
            activate = function (routeData) {
                var id = parseInt(routeData.id);
                if (id) {
                    var query = String.format("EntityDefinitionId eq {0}", id);
                    var tcs = new $.Deferred();
                    context.loadOneAsync("EntityDefinition", query)
                        .done(function (b) {
                            entity(b);
                            tcs.resolve(true);
                        });

                    return tcs.promise();
                }

                entity(new bespoke.sph.domain.EntityDefinition(system.guid()));
                return Task.fromResult(true);

            },
            viewAttached = function (view) {

            },
            save = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(entity);
                isBusy(true);

                context.post(data, "/EntityDefinition/Save")
                    .then(function (result) {
                        isBusy(false);


                        tcs.resolve(result);
                    });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            viewAttached: viewAttached,
            entity: entity,
            toolbar: {
                saveCommand: save
            }
        };

        return vm;

    });
