﻿/// <reference path="../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../Scripts/require.js" />
/// <reference path="../Scripts/objectbuilders.js" />
/// <reference path="../Scripts/underscore.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../Scripts/trigger.workflow.g.js" />



define(["services/datacontext", "services/logger", "plugins/dialog", objectbuilders.system],
    function (context, logger, dialog, system) {

        const action = ko.observable(new bespoke.sph.domain.AssemblyAction(system.guid())),
            assemblyOptions = ko.observableArray(),
            typeOptions = ko.observableArray(),
            methodOptions = ko.observableArray(),
            activate = function () {
                var tcs = new $.Deferred();
                $.get("/api/assemblies")
                    .done(function (assemblies) {
                        assemblyOptions(assemblies);

                        if (action().Assembly()) {

                            const loadTypesTask = $.get(`/api/assemblies/${action().Assembly()}/types`),
                                loadMethodTask = $.get(`/api/assemblies/${action().Assembly()}/types/${action().TypeName()}/methods`);
                            $.when(loadTypesTask, loadMethodTask)
                                .done(function (t1, m1) {
                                    typeOptions(t1[0]);
                                    methodOptions(m1[0]);
                                    tcs.resolve(true);
                                });

                        } else {
                            tcs.resolve(true);
                        }

                    });
                return tcs.promise();

            },
            attached = function (view) {
                action().Assembly.subscribe(function (dll) {
                    if (!dll) {
                        return;
                    }
                    $.get(`/api/assemblies/${dll}/types`)
                   .done(function (classes) {
                       typeOptions(classes);
                   });
                });
                action().TypeName.subscribe(function (type) {
                    if (!type) {
                        methodOptions.removeAll();
                        return;
                    }
                    $.get(`/api/assemblies/${action().Assembly()}/types/${type}/methods`)
                   .done(function (methods) {
                       methodOptions(methods);
                   });
                });

                action().Method.subscribe(function (method) {
                    if (!method) return;
                    const m = _(methodOptions()).find(function (v) { return v.Name === method; }),
                        args = _(m.Parameters).map(function (v) {
                            return new bespoke.sph.domain.MethodArg({
                                WebId: system.guid(),
                                Name: v.Name,
                                TypeName: v.TypeName
                            });
                        });
                    action().IsAsyncMethod(m.RetVal.indexOf("System.Threading.Tasks.Task") > -1);
                    action().IsVoid(m.RetVal === "System.Void");
                    action().IsStatic(m.IsStatic);
                    action().MethodArgCollection(args);
                    action().ReturnType(m.RetVal);

                    if (!action().Title()) {
                        action().Title(method);
                    }
                });

                const method = ko.unwrap(action().Method);
                if (!method) {
                    action().MethodArgCollection([]);
                }

                setTimeout(function () {
                    $(view).find("#assembly-action-title").focus();
                }, 500);
            },
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        const vm = {
            action: action,
            activate: activate,
            attached: attached,
            assemblyOptions: assemblyOptions,
            typeOptions: typeOptions,
            methodOptions: methodOptions,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
