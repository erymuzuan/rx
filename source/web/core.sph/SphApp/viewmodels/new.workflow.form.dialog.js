/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../../../core.sph/SphApp/schemas/__domain.js" />
/// <reference path="../../../core.sph/SphApp/schemas/form.designer.g.js" />
/// <reference path="../../../core.sph/SphApp/objectbuilders.js" />
/// <reference path="../../../core.sph/Scripts/_task.js" />
/// <reference path="../../../core.sph/Scripts/require.js" />


define(["plugins/dialog", objectbuilders.datacontext, objectbuilders.system],
    function (dialog, context, system) {

        var wds = ko.observableArray(),
            activityOptions = ko.observableArray(),
            form = ko.observable(new bespoke.sph.domain.WorkflowForm(system.guid())),
            wd = ko.observable(),
            id = ko.observable(),
            activate = function () {
                form(new bespoke.sph.domain.WorkflowForm({ "WorkflowDefinitionId": ko.unwrap(wd), "WebId": system.guid() }));
                form().Name.subscribe(function (v) {
                    form().Route(v.toLowerCase().replace(/\W+/g, "-"));
                });
                form().WorkflowDefinitionId.subscribe(function (v) {
                    context.loadOneAsync("WorkflowDefinition", "Id eq '" + v + "'").done(function (b) {
                        if (null == b) {
                            return;
                        }
                        wd(b);
                        var list = _(ko.unwrap(b.ActivityCollection))
                            .chain()
                            .filter(function(x) {
                                return ko.unwrap(x.$type).indexOf("ReceiveActivity") > -1;
                            })
                            .value();
                        activityOptions(list);
                    });
                });
                return context.getTuplesAsync("WorkflowDefinition", null, "Id", "Name").done(wds);
            },
            okClick = function (data, ev) {
                if (!bespoke.utils.form.checkValidity(ev.target)) {
                    return Task.fromResult(0);
                }

                var json = ko.toJSON(form);
                return context.put(json, "/api/workflow-forms")
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
            activityOptions: activityOptions,
            form: form,
            activate: activate,
            okClick: okClick,
            wd: wd,
            id: id,
            wds: wds,
            cancelClick: cancelClick
        };


        return vm;

    });
