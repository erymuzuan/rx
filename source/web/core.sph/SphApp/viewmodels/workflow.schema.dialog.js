/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />


define(["plugins/dialog", objectbuilders.datacontext],
    function(dialog, context) {

        var member = ko.observable(),
            vodOptions= ko.observableArray(),
            vod = ko.observable(new bespoke.sph.domain.ValueObjectDefinition()),
            id = ko.observable(),
            activate = function() {
                return context.getTuplesAsync("ValueObjectDefinition", null, "Id", "Name")
                    .done(vodOptions);
            },
            attached = function() {
                id.subscribe(function(id2) {
                    context.loadOneAsync("ValueObjectDefinition", String.format("Id eq '{0}'", id2))
                        .done(vod);
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
            vodOptions: vodOptions,
            entityId: id,
            vod: vod,
            member: member,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
