/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="~/Scripts/_task.js" />

define(["services/datacontext", "services/logger", "plugins/dialog", objectbuilders.system],
    function (context, logger, dialog, system) {
        "use strict";
        var allowMultipleSource = ko.observable(),
            assemblyOptions = ko.observableArray(),
            inputTypeOptions = ko.observableArray(),
            selectedInputAssembly = ko.observable(),
            outputTypeOptions = ko.observableArray(),
            typeOptions = ko.observableArray(),
            selectedOutputAssembly = ko.observable(),
            td = ko.observable(new bespoke.sph.domain.TransformDefinition()),
            activate = function () {
                allowMultipleSource(!td().InputTypeName());
                return context.get("/api/assemblies")
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
                            return context.get("/api/assemblies/" + selectedOutputAssembly() + "/types");
                        }
                        return Task.fromResult([]);

                    }).then(function (list) {
                        outputTypeOptions(list);
                        if (selectedInputAssembly()) {
                            return context.get("/api/assemblies/" + selectedInputAssembly() + "/types");
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
            attached = function (view) {
                $(view).on("click", "a.fa-angle-double-down", function (e) {
                    e.preventDefault();
                    var arg = ko.dataFor(this);
                    td().InputCollection.push(new bespoke.sph.domain.MethodArg({
                        TypeName: arg.FullName,
                        Name: arg.Name,
                        WebId: system.guid()
                    }));
                });
                $(view).on("click", "a.fa-times", function (e) {
                    e.preventDefault();
                    var arg = ko.dataFor(this);
                    td().InputCollection.remove(arg);
                });

                //scrollable tbody
                var $table = $("#types-table"),
                    $bodyCells = $table.find("tbody tr:first").children(),
                    colWidth;

                $(window).resize(function () {
                    colWidth = $bodyCells.map(function () {
                        return $(this).width();
                    }).get();

                    $table.find("thead tr").children().each(function (i, v) {
                        $(v).width(colWidth[i]);
                    });
                }).resize();


                selectedInputAssembly.subscribe(function (dll) {
                    return context.get("/apiassemblies/" + dll + "/types")
                        .then(inputTypeOptions);
                });
                selectedOutputAssembly.subscribe(function (dll) {
                    return context.get("/api/assemblies/" + dll + "/types")
                        .then(outputTypeOptions);
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
            allowMultipleSource: allowMultipleSource,
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
