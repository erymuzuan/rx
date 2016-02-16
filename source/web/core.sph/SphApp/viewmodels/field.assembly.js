/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />

define(['plugins/dialog'],
    function (dialog) {

        var assemblyOptions = ko.observableArray(),
            typeOptions = ko.observableArray(),
            methodOptions = ko.observableArray(),
            parameterOptions = ko.observableArray(),
            field = ko.observable(new bespoke.sph.domain.AssemblyField()),
            activate = function () {
                if (!field().Method()) {
                    return $.getJSON("/api/assemblies")
                        .done(function (list) {
                            assemblyOptions(list);
                        });
                }

                return $.getJSON("/api/assemblies")
                        .then(function (list) {
                            assemblyOptions(list);
                            return $.getJSON("/api/assemblies/" + field().Location() + "/types");
                        }).then(function (types) {
                            typeOptions(types);
                            return $.getJSON("/api/assemblies/" + ko.unwrap(field().Location) + "/types/" + ko.unwrap(field().TypeName) + "/methods");
                        }).then(function (methods) {
                            methodOptions(methods);
                        });
            },
            attached = function () {
                field().Location.subscribe(function (dll) {
                    $.getJSON("/api/assemblies/" + dll + "/types")
                        .done(function (types) {
                            typeOptions(types);
                        });
                });
                field().TypeName.subscribe(function (type) {
                    $.getJSON("/assemblies/" + ko.unwrap(field().Location) + "/types/" + type + "/methods")
                        .done(function (methods) {
                            methodOptions(methods);
                        });
                });
                field().Method.subscribe(function (method) {
                    var m = _(methodOptions()).find(function (v) {
                        return v.Name === method;
                    });
                    field().IsAsync(m.IsAsync);
                    parameterOptions(m.Parameters);

                    var args = _(m.Parameters).map(function (v) {
                        return new bespoke.sph.domain.MethodArg({
                            Name: v.Name,
                            TypeName: v.Type
                        });
                    });
                    field().MethodArgCollection(args);
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
            activate: activate,
            attached: attached,
            assemblyOptions: assemblyOptions,
            typeOptions: typeOptions,
            methodOptions: methodOptions,
            parameterOptions: parameterOptions,
            field: field,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
