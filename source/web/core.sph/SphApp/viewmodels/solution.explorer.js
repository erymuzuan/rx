﻿/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/dialog'],
    function (context, logger, dialog) {
        var items = ko.observableArray(),
            triggers= ko.observableArray(),
            wds= ko.observableArray(),
            transforms = ko.observableArray(),
            isBusy = ko.observable(true),
            activate = function () {
                var tcs = new $.Deferred(),
                    entitiesTask = context.loadAsync("EntityDefinition"),
                    formsTask = context.loadAsync({ entity:"EntityForm", size:200 }),
                    viewsTask = context.loadAsync({ entity: "EntityView", size: 200 }),
                    triggersTask = context.loadAsync({ entity:"Trigger", size:200 }),
                    rdlTask = context.loadAsync({ entity:"ReportDefinition", size:200 }),
                    transformTask = context.loadAsync({ entity:"TransformDefinition", size:200 }),
                    wdTask = context.loadAsync({ entity:"WorkflowDefinition", size:200 });


                $.when(entitiesTask, formsTask, viewsTask, triggersTask, wdTask, rdlTask, transformTask)
                    .then(function (entitiesLo, formsLo, viewsLo, triggersLo, wdsLo, rdlLo, transformLo) {
                        isBusy(false);
                        var entities = _(entitiesLo.itemCollection).map(function (v) {
                            return {
                                name: v.Name,
                                id: v.Id,
                                route: '#entity.details/' + v.Id(),
                                operations : v.EntityOperationCollection(),
                                rdl: _(rdlLo.itemCollection).filter(function (f) { return f.DataSource().EntityName() === v.Name(); }),
                                triggers: _(triggersLo.itemCollection).filter(function (f) { return f.Entity() === v.Name(); }),
                                forms: _(formsLo.itemCollection).filter(function (f) { return f.EntityDefinitionId() === v.Id(); }),
                                views: _(viewsLo.itemCollection).filter(function (f) { return f.EntityDefinitionId() === v.Id(); })
                            };
                        });
                        items(entities);
                        wds(wdsLo.itemCollection);
                        transforms(transformLo.itemCollection);
                        triggers(triggersLo.itemCollection);

                        tcs.resolve(true);
                    });
                return tcs.promise();

            },
            attached = function (view) {
                var self = this;
                $('#solution-explorer-dialog').on('click', 'a', function (e) {
                    e.preventDefault();
                });
                $('#solution-explorer-dialog').on('dblclick', 'a', function (e) {
                    e.preventDefault();
                    window.location = this.href;
                    dialog.close(self, "OK");
                });

                var $filterInput = $('#quick-nav-filter'),
                    dofilter = function () {
                        var $rows = $(view).find('li'),
                            filter = $filterInput.val().toLowerCase();
                        $rows.each(function () {
                            var $tr = $(this);
                            if ($tr.text().toLowerCase().indexOf(filter) > -1) {
                                $tr.show();
                            } else {
                                $tr.hide();
                            }
                        });
                    },
                throttled = _.throttle(dofilter, 800);

                $filterInput.on('keyup', throttled).siblings('span.input-group-addon')
                    .click(function () {
                        $filterInput.val('');
                        dofilter();
                    })
                .end()
                .focus();
                setTimeout(function() { $filterInput.focus(); }, 200);

                if ($filterInput.val()) {
                    dofilter();
                }
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
            transforms: transforms,
            wds: wds,
            triggers: triggers,
            items: items,
            activate: activate,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });