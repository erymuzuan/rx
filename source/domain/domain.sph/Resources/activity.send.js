/// <reference path="../../../web/core.sph/scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../../web/core.sph/scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../../web/core.sph/SphApp/objectbuilders.js" />
/// <reference path="../../../web/core.sph/SphApp/schemas/trigger.workflow.g.js" />
/// <reference path="../../../web/core.sph/Scripts/__core.js" />
/// <reference path="../../../web/core.sph/Scripts/require.js" />


define(["services/datacontext", "services/logger", "plugins/dialog", objectbuilders.config, objectbuilders.system],
    function (context, logger, dialog, config, system) {

        var methodOptions = ko.observableArray(),
            adapterAssemblyOptions = ko.observableArray(),
            adapterOptions = ko.observableArray(),
            variableOptions = ko.observableArray(),
            activity = ko.observable(),
            selectedMethod = ko.observable(),
            wd = ko.observable(),
            isBusy = ko.observable(false),
            activate = function () {

                variableOptions(_(wd().VariableDefinitionCollection()).map(function (v) {
                    return ko.unwrap(v.Name);
                }));
                var tcs = new $.Deferred();
                $.get("/api/assemblies")
                    .done(function (b) {
                        var assemblies = _(b).chain()
                            .filter(function (v) {
                                return v.Name.indexOf(config.applicationName) > -1;
                            })
                            .map(function (v) {
                                return v.Name;
                            })
                            .value();

                        adapterAssemblyOptions(assemblies);

                        // for existing activity
                        var act = activity();
                        if (act.AdapterAssembly()) {
                            var getTypeTask = $.get("/api/assemblies/" + act.AdapterAssembly() + "/types"),
                                getAdapterTask = $.get("/api/assemblies/" + act.AdapterAssembly() + "/types/" + act.Adapter() + "/methods");

                            $.when(getTypeTask, getAdapterTask)
                               .done(function (b1, b2) {
                                   var adpterTypes = _(b1[0] || b1).map(function (v) {
                                       return v.TypeName;
                                   });
                                   adapterOptions(adpterTypes);
                                   methodOptions(b2[0]);
                                   tcs.resolve(true);
                               });

                        } else {
                            tcs.resolve(true);
                        }
                    });

                return tcs.promise();
            },
            attached = function () {

                activity().AdapterAssembly.subscribe(function (dll) {
                    $.get("/api/assemblies/" + dll + "/types")
                   .done(function (b) {
                       var adpterTypes = _(b).map(function (v) {
                           return v.TypeName;
                       });
                       adapterOptions(adpterTypes);
                   });
                });
                activity().Adapter.subscribe(function (type) {
                    $.get("/api/assemblies/" + activity().AdapterAssembly() + "/types/" + type + "/methods")
                   .done(function (b) {
                       methodOptions(b);
                   });
                });
                selectedMethod.subscribe(function (m) {
                    var method = (m || { Name: "" }).Name;
                    activity().Method(method);
                });

                var sm = methodOptions().find(function(v) {
                    return v.Name === activity().Method();
                });
                selectedMethod(sm);
            },
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }

            },
           cancelClick = function () {
               dialog.close(this, "Cancel");
           },
            addExceptionFilter = function () {
                activity().ExceptionFilterCollection.push(new bespoke.sph.domain.ExceptionFilter(system.guid()));
            },
            removeExceptionFilter = function (filter) {

                activity().ExceptionFilterCollection.remove(filter);
            };

        var vm = {
            selectedMethod: selectedMethod,
            variableOptions: variableOptions,
            adapterAssemblyOptions: adapterAssemblyOptions,
            adapterOptions: adapterOptions,
            methodOptions: methodOptions,
            removeExceptionFilter: removeExceptionFilter,
            addExceptionFilter: addExceptionFilter,
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
