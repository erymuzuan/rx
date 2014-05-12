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
                    rdlTask = context.loadAsync("ReportDefinition"),
                    wdTask = context.loadAsync("WorkflowDefinition");


                $.when(entitiesTask, formsTask, viewsTask, triggersTask, wdTask, rdlTask)
                    .then(function (entitiesLo, formsLo, viewsLo, triggersLo, wdsLo, rdlLo) {
                        isBusy(false);
                        var entities = _(entitiesLo.itemCollection).map(function (v) {
                            return {
                                name: v.Name,
                                id: v.EntityDefinitionId,
                                route: '#entity.details/' + v.EntityDefinitionId(),
                                operations : v.EntityOperationCollection(),
                                rdl: _(rdlLo.itemCollection).filter(function (f) { return f.DataSource().EntityName() === v.Name(); }),
                                triggers: _(triggersLo.itemCollection).filter(function (f) { return f.Entity() === v.Name(); }),
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
                $('#dev-quick-nav-dialog').on('click', 'a', function (e) {
                    e.preventDefault();
                });
                $('#dev-quick-nav-dialog').on('dblclick', 'a', function (e) {
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
            wds: wds,
            triggers: triggers,
            items: items,
            activate: activate,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
