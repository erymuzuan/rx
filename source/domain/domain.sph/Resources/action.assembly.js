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
                        tcs.resolve(true);
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
                    $.get("/transform-definition/methods/" + action().Assembly() + "/" + type)
                   .done(function (methods) {
                       methodOptions(methods);
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
