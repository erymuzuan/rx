/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(["plugins/dialog", objectbuilders.datacontext],
    function(dialog, context) {

        var member = ko.observable(),
            entityOptions= ko.observableArray(),
            entity = ko.observable(new bespoke.sph.domain.EntityDefinition()),
            entityId = ko.observable(),
            activate = function() {
                return context.getTuplesAsync("EntityDefinition", "", "Id", "Name")
                    .done(entityOptions);
            },
            attached = function() {
                entityId.subscribe(function(id) {
                    context.loadOneAsync("EntityDefinition", String.format("Id eq '{0}'", id))
                        .done(entity);
                });
            },
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }

            },
            cancelClick = function() {
                dialog.close(this, "Cancel");
            };

        var vm = {
            activate: activate,
            attached: attached,
            entityOptions: entityOptions,
            entityId: entityId,
            entity: entity,
            member: member,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
