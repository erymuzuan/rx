/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/dialog', objectbuilders.config],
    function (context, logger, dialog, config) {
        var items = ko.observableArray(),
            triggers= ko.observableArray(),
            wds= ko.observableArray(),
            isBusy = ko.observable(true),
            activate = function () {
                var tcs = new $.Deferred(),
                    entitiesTask = context.loadAsync("EntityDefinition"),
                    formsTask = context.loadAsync("EntityForm"),
                    viewsTask = context.loadAsync("EntityView"),
                    triggersTask = context.loadAsync("Trigger"),
                    wdTask = context.loadAsync("WorkflowDefinition");


                $.when(entitiesTask, formsTask, viewsTask, triggersTask, wdTask)
                    .then(function (entitiesLo, formsLo, viewsLo, triggersLo, wdsLo) {
                        isBusy(false);
                        var entities = _(entitiesLo.itemCollection).map(function (v) {
                            return {
                                name: v.Name,
                                id: v.EntityDefinitionId,
                                route: '#entity.details/' + v.EntityDefinitionId(),
                                forms: _(formsLo.itemCollection).filter(function (f) { return f.EntityDefinitionId() === v.EntityDefinitionId(); }),
                                views: _(viewsLo.itemCollection).filter(function (f) { return f.EntityDefinitionId() === v.EntityDefinitionId(); })
                            };
                        });
                        items(entities);
                        wds(wdsLo.itemCollection);
                        triggers(triggersLo.itemCollection);

                        tcs.resolve(true);
                    });
                return tcs.promise();

            },
            attached = function (view) {
                var self = this;
                $('#dev-quck-nav-dialog').on('click', 'a', function (e) {
                    dialog.close(self, "OK");
                });
            },
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        var vm = {
            attached: attached,
            isBusy: isBusy,
            wds: wds,
            triggers: triggers,
            items: items,
            activate: activate,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
