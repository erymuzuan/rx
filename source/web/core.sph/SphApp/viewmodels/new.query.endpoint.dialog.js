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
            query = ko.observable(new bespoke.sph.domain.QueryEndpoint()),
            entity = ko.observable(),
            id = ko.observable(),
            activate = function () {
                query(new bespoke.sph.domain.QueryEndpoint({ "Entity": ko.unwrap(entity), "WebId": system.guid() }));
                query().Name.subscribe(function (v) {
                    query().Route(v.toLowerCase().replace(/\W+/g, "-"));
                });
                return context.getListAsync("EntityDefinition", "Id ne ''", "Name").done(entities);
            },
            attached = function (view) {
                setTimeout(function () { $(view).find("#name-input").focus(); }, 500);
            },
            okClick = function (data, ev) {
                if (!bespoke.utils.form.checkValidity(ev.target)) {
                    return Task.fromResult(0);
                }

                var json = ko.mapping.toJSON(query);
                return context.post(json, "/query-endpoints")
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
            attached : attached,
            query: query,
            activate: activate,
            okClick: okClick,
            entity: entity,
            id: id,
            entities: entities,
            cancelClick: cancelClick
        };


        return vm;

    });
