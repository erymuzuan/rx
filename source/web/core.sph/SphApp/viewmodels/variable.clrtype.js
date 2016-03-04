/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />

define(["plugins/dialog", objectbuilders.datacontext, objectbuilders.config],
    function (dialog, context, config) {

        var entityOptions = ko.observableArray(),
            assemblyOptions = ko.observableArray(),
            selectedAssembly = ko.observable(),
            variable = ko.observable(new bespoke.sph.domain.ClrTypeVariable()),
            activate = function () {
                var tcs = new $.Deferred();

                $.get("/api/assemblies")
                    .then(function (lo) {
                        assemblyOptions(lo);
                        tcs.resolve(true);
                    });
                return tcs.promise();

            },
            attached = function (view) {
                selectedAssembly.subscribe(function (dll) {

                    $.get("/api/assemblies/" + dll + "/types")
                        .then(function (lo) {
                            var types = _(lo).map(function (v) {
                                return {
                                    displayName: v.Name + " (" + v.FullName + ")",
                                    fullName: v.FullName
                                };
                            });
                            entityOptions(types);
                        });
                });
                setTimeout(function () { $(view).find("#clr-name").focus(); }, 700);
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
            selectedAssembly: selectedAssembly,
            assemblyOptions: assemblyOptions,
            entityOptions: entityOptions,
            activate: activate,
            attached: attached,
            variable: variable,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
