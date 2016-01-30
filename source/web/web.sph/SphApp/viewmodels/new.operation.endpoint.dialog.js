/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../../../core.sph/SphApp/schemas/__domain.js" />
/// <reference path="../../../core.sph/SphApp/objectbuilders.js" />
/// <reference path="../../../core.sph/Scripts/_task.js" />
/// <reference path="../../../core.sph/Scripts/require.js" />


define(["plugins/dialog", objectbuilders.datacontext, objectbuilders.system],
    function (dialog, context, system) {

        var entities = ko.observableArray(),
            endpoint = ko.observable(new bespoke.sph.domain.OperationEndpoint()),
            entity = ko.observable(),
            id = ko.observable(),
            activate = function () {
                endpoint(new bespoke.sph.domain.OperationEndpoint({ "Entity": ko.unwrap(entity), "WebId": system.guid() }));
                endpoint().Name.subscribe(function (v) {
                    endpoint().Route(v.toLowerCase().replace(/\W+/g, "-"));
                });
                return context.getListAsync("EntityDefinition", "Id ne ''", "Name").done(entities);
            },
            okClick = function (data, ev) {
                if (!bespoke.utils.form.checkValidity(ev.target)) {
                    return Task.fromResult(0);
                }

                var json = ko.mapping.toJSON(endpoint);
                return context.post(json, "/api/operation-endpoints")
                    .then(function (result) {
                        if (result) {
                            id(result.id);
                            dialog.close(data, "OK");
                        }
                    });

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        var vm = {
            endpoint: endpoint,
            activate: activate,
            okClick: okClick,
            entity: entity,
            id: id,
            entities: entities,
            cancelClick: cancelClick
        };


        return vm;

    });
