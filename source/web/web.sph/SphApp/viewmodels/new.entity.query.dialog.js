/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />


define(["plugins/dialog", objectbuilders.datacontext, objectbuilders.system],
    function (dialog, context, system) {

        var entities = ko.observableArray(),
            query = ko.observable(new bespoke.sph.domain.EntityQuery()),
            entity = ko.observable(),
            activate = function () {
                query(new bespoke.sph.domain.EntityQuery({ "Entity": ko.unwrap(entity), "WebId": system.guid() }));
                return context.getListAsync("EntityDefinition", "Id ne ''", "Name").done(entities);
            },
            okClick = function (data, ev) {
                if (!bespoke.utils.form.checkValidity(ev.target)) {
                    return Task.fromResult(0);
                }

                var json = ko.mapping.toJSON(query);
                return context.post(json, "/entity-query")
                    .then(function (result) {
                        if (result) {
                            id(result);
                            dialog.close(data, "OK");
                        }
                    });

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        var vm = {
            query: query,
            activate: activate,
            okClick: okClick,
            entity: entity,
            entities: entities,
            cancelClick: cancelClick
        };


        return vm;

    });
