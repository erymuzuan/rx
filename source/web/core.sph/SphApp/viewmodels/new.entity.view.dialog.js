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
            view = ko.observable(new bespoke.sph.domain.EntityView(system.guid())),
            entity = ko.observable(),
            id = ko.observable(),
            activate = function () {
                view(new bespoke.sph.domain.EntityView({ "Entity": ko.unwrap(entity), "WebId": system.guid() }));
                view().Name.subscribe(function (v) {
                    view().Route(v.toLowerCase().replace(/\W+/g, "-"));
                });
                view().Entity.subscribe(function (v) {
                    context.getScalarAsync("EntityDefinition", "Name eq '" + v + "'", "Id")
                        .done(view().EntityDefinitionId);
                });
                return context.getListAsync("EntityDefinition", "Id ne ''", "Name").done(entities);
            },
            attached = function (vw) {
                setTimeout(function () {
                    $(vw).find("#name-input").focus();
                }, 500);
            },
            okClick = function (data, ev) {
                if (!bespoke.utils.form.checkValidity(ev.target)) {
                    return Task.fromResult(0);
                }

                var json = ko.toJSON(view);
                return context.put(json, "/api/entity-views")
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
            view: view,
            activate: activate,
            attached: attached,
            okClick: okClick,
            entity: entity,
            id: id,
            entities: entities,
            cancelClick: cancelClick
        };


        return vm;

    });
