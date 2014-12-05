/// <reference path="../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../Scripts/require.js" />
/// <reference path="../Scripts/underscore.js" />
/// <reference path="../Scripts/moment.js" />
/// <reference path="../Scripts/t.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />

define(['services/datacontext', 'services/logger', 'plugins/dialog', objectbuilders.config],
    function (context, logger, dialog, config) {

        var methodOptions = ko.observableArray(),
            adapterAssemblyOptions = ko.observableArray(),
            adapterOptions = ko.observableArray(),
            variableOptions = ko.observableArray(),
            activity = ko.observable(),
            wd = ko.observable(),
            isBusy = ko.observable(false),
            activate = function () {

                variableOptions(_(wd().VariableDefinitionCollection()).map(function (v) {
                    return ko.unwrap(v.Name);
                }));
                var tcs = new $.Deferred();
                $.get("/wf-designer/assemblies")
                    .done(function (b) {
                        adapterAssemblyOptions(_(b).filter(function (v) { return v.indexOf(config.applicationName) > -1; }));

                        // for existing activity
                        var act = activity();
                        if (act.AdapterAssembly()) {
                            var getTypeTask = $.get("/wf-designer/types/" + act.AdapterAssembly()),
                                getAdapterTask = $.get("/wf-designer/methods/" + act.AdapterAssembly() + "/" + act.Adapter());

                            $.when(getTypeTask, getAdapterTask)
                               .done(function (b1, b2) {
                                   adapterOptions(b1[0]);
                                   methodOptions(b2[0]);
                                   tcs.resolve(true);
                               });

                        } else {
                            tcs.resolve(true);
                        }
                    });

                return tcs.promise();
            },
            attached = function (view) {

                activity().AdapterAssembly.subscribe(function (dll) {
                    $.get("/wf-designer/types/" + dll)
                   .done(function (b) {
                       adapterOptions(b);
                   });
                });
                activity().Adapter.subscribe(function (type) {
                    $.get("/wf-designer/methods/" + activity().AdapterAssembly() + "/" + type)
                   .done(function (b) {
                       methodOptions(b);
                   });
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
            variableOptions: variableOptions,
            adapterAssemblyOptions: adapterAssemblyOptions,
            adapterOptions: adapterOptions,
            methodOptions: methodOptions,
            activity: activity,
            wd: wd,
            isBusy: isBusy,
            okClick: okClick,
            cancelClick: cancelClick,
            activate: activate,
            attached: attached
        };

        return vm;

    });
