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

        var entityOptions = ko.observableArray(),
            form = ko.observable(new bespoke.sph.domain.PartialView(system.guid())),
            entity = ko.observable(),
            id = ko.observable(),
            activate = function () {
                var view = new bespoke.sph.domain.PartialView({ "Entity": ko.unwrap(entity), "WebId": system.guid() });

                view.Name.subscribe(function (v) {
                    form().Route(v.toLowerCase().replace(/\W+/g, "-"));
                });
                view.Entity.subscribe(function (v) {
                    context.getScalarAsync("EntityDefinition", "Name eq '" + v + "'", "Id")
                        .done(form().EntityDefinitionId);
                });

                form(view);
                return context.getTuplesAsync("EntityDefinition", null, "Id", "Name").done(entityOptions);
            },
            okClick = function (data, ev) {
                if (!bespoke.utils.form.checkValidity(ev.target)) {
                    return Task.fromResult(0);
                }

                var json = ko.toJSON(form);
                return context.post(json, "/api/partial-views")
                    .then(function (result) {
                        if (result) {
                            id(result.id);
                            dialog.close(data, "OK");
                        }
                    });

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            attached = function (view) {
                setTimeout(function () {
                    $(view).find("#partial-view-name").focus();
                }, 500);
            };

        var vm = {
            form: form,
            activate: activate,
            attached: attached,
            okClick: okClick,
            entity: entity,
            id: id,
            entityOptions: entityOptions,
            cancelClick: cancelClick
        };


        return vm;

    });
