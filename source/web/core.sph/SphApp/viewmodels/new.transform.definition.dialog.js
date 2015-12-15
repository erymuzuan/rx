/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="~/Scripts/_task.js" />
define(["plugins/dialog", objectbuilders.datacontext, objectbuilders.system],
    function (dialog, context, system) {

        var td = ko.observable(),
            allowMultipleSource = ko.observable(false),
            assemblyOptions = ko.observableArray(),
            inputTypeOptions = ko.observableArray(),
            selectedInputAssembly = ko.observable(),
            outputTypeOptions = ko.observableArray(),
            typeOptions = ko.observableArray(),
            selectedOutputAssembly = ko.observable(),
            id = ko.observable(),
            activate = function() {
                td(new bespoke.sph.domain.TransformDefinition());

                return context.get("/transform-definition/assemblies")
                    .then(function (list) {
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

                        var input = ko.unwrap(td().InputTypeName),
                            output = ko.unwrap(td().OutputTypeName);
                        if (input) {
                            selectedInputAssembly(/, (.*)/.exec(input)[1]);
                        }
                        if (output) {
                            selectedOutputAssembly(/, (.*)/.exec(output)[1]);
                            return context.get("/transform-definition/types/" + selectedOutputAssembly());
                        }
                        return Task.fromResult([]);

                    }).then(function (list) {
                        outputTypeOptions(list);
                        if (selectedInputAssembly()) {
                            return context.get("/transform-definition/types/" + selectedInputAssembly());
                        }
                        return Task.fromResult([]);
                    })
                .then(function (list) {
                    inputTypeOptions(list);
                }).fail(function (e, x, z) {
                    console.log(e);
                    console.log(x);
                    console.log(z);
                });
            },
            attached = function() {
                setTimeout(function () {
                    $("#name-input").focus();
                }, 500);

                selectedInputAssembly.subscribe(function (dll) {
                    return context.get("/transform-definition/types/" + dll)
                        .then(inputTypeOptions);
                });
                selectedOutputAssembly.subscribe(function (dll) {
                    return context.get("/transform-definition/types/" + dll)
                        .then(outputTypeOptions);
                });
            },
            saveAsync = function(data,ev) {

                var json = ko.mapping.toJSON(td);
                return context.post(json, "/transform-definition")
                    .then(function (result) {
                        if (result.success) {
                            id(result.id);
                            dialog.close(data, "OK");
                        }
                    });
            },
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }

                return saveAsync(data,ev);

            },
            cancelClick = function() {
                dialog.close(this, "Cancel");
            };

        var vm = {
            td: td,
            allowMultipleSource: allowMultipleSource,
            assemblyOptions: assemblyOptions,
            typeOptions: typeOptions,
            inputTypeOptions: inputTypeOptions,
            selectedInputAssembly: selectedInputAssembly,
            outputTypeOptions: outputTypeOptions,
            selectedOutputAssembly: selectedOutputAssembly,
            id: id,
            attached: attached,
            activate : activate,
            okClick: okClick,
            cancelClick: cancelClick,
            saveAsync : saveAsync
        };


        return vm;

    });
