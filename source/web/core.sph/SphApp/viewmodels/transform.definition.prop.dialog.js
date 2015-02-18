/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/dialog', objectbuilders.system],
    function (context, logger, dialog, system) {
        "use strict";
        var assemblyOptions = ko.observableArray(),
            inputTypeOptions = ko.observableArray(),
            selectedInputAssembly = ko.observable(),
            outputTypeOptions = ko.observableArray(),
            typeOptions = ko.observableArray(),
            selectedOutputAssembly = ko.observable(),
            td = ko.observable(new bespoke.sph.domain.TransformDefinition()),
            activate = function () {
                var tcs = new $.Deferred();
                $.get("/transform-definition/assemblies", function (list) {
                    assemblyOptions(list);
                    var types = [];
                    _(list).each(function (v) {
                        var items = v.Types.map(function (x) {
                            x.Assembly = v.Name;
                            x.FullName = x.Namespace + "." + x.Name + ", " + v.Name;
                            return x;
                        });
                        types = types.concat(items);
                    });
                    typeOptions(types);
                    tcs.resolve(true);
                });

                return tcs.promise();
            },
            attached = function (view) {
                $(view).on('click', 'a.fa-angle-double-down', function (e) {
                    e.preventDefault();
                    var arg = ko.dataFor(this);
                    td().InputCollection.push(new bespoke.sph.domain.MethodArg({
                        TypeName: arg.FullName,
                        WebId : system.guid()
                    }));
                });
                $(view).on('click', 'a.fa-times', function (e) {
                    e.preventDefault();
                    var arg = ko.dataFor(this);
                    td().InputCollection.remove(arg);
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

        selectedInputAssembly.subscribe(function (dll) {
            $.get("/transform-definition/types/" + dll, function (list) {
                inputTypeOptions(list);
            });
        });
        selectedOutputAssembly.subscribe(function (dll) {
            $.get("/transform-definition/types/" + dll, function (list) {
                outputTypeOptions(list);
            });
        });

        var vm = {
            activate: activate,
            attached: attached,
            assemblyOptions: assemblyOptions,
            typeOptions: typeOptions,
            inputTypeOptions: inputTypeOptions,
            selectedInputAssembly: selectedInputAssembly,
            outputTypeOptions: outputTypeOptions,
            selectedOutputAssembly: selectedOutputAssembly,
            td: td,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
