/// <reference path="../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../Scripts/require.js" />
/// <reference path="../Scripts/objectbuilders.js" />
/// <reference path="../Scripts/underscore.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../Scripts/trigger.workflow.g.js" />



define(['services/datacontext', 'services/logger', 'plugins/dialog', objectbuilders.system],
    function (context, logger, dialog, system) {

        var action = ko.observable(new bespoke.sph.domain.AssemblyAction(system.guid())),
            assemblyOptions = ko.observableArray(),
            typeOptions = ko.observableArray(),
            methodOptions = ko.observableArray(),
            activate = function () {
                var tcs = new $.Deferred();
                $.get("/transform-definition/assemblies")
                    .done(function (assemblies) {
                        assemblyOptions(assemblies);

                        if (action().Assembly()) {

                            var loadTypesTask = $.get("/transform-definition/types/" + action().Assembly()),
                                loadMethodTask = $.get("/transform-definition/methods/" + action().Assembly() + "/" + action().TypeName());
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
                    $.get("/transform-definition/types/" + dll)
                   .done(function (classes) {
                       typeOptions(classes);
                   });
                });
                action().TypeName.subscribe(function (type) {
                    if (!type) {
                        methodOptions.removeAll();
                        return;
                    }
                    $.get("/transform-definition/methods/" + action().Assembly() + "/" + type)
                   .done(function (methods) {
                       methodOptions(methods);
                   });
                });
                action().Method.subscribe(function (method) {
                    if (!method) return;
                    var m = _(methodOptions()).find(function (v) { return v.Name === method; }),
                        args = _(m.Parameters).map(function (v) {
                            return new bespoke.sph.domain.MethodArg({
                                WebId: system.guid(),
                                Name: v.Name,
                                TypeName: v.Type
                            });
                        });
                    action().IsAsyncMethod(m.RetVal.indexOf("System.Threading.Tasks.Task") > -1);
                    action().MethodArgCollection(args);
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
