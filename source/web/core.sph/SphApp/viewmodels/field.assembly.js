/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />

define(['plugins/dialog'],
    function (dialog) {

        var assemblyOptions = ko.observableArray(),
            typeOptions = ko.observableArray(),
            methodOptions = ko.observableArray(),
            parameterOptions = ko.observableArray(),
            field = ko.observable(new bespoke.sph.domain.AssemblyField()),
            activate = function () {
                if (!field().Method()) {
                    return $.getJSON("/transform-definition/assemblies")
                        .done(function (list) {
                            assemblyOptions(list);
                        });
                }

                return $.getJSON("/transform-definition/assemblies")
                        .then(function (list) {
                            assemblyOptions(list);
                            return $.getJSON("/transform-definition/types/" + field().Location());
                        }).then(function (types) {
                            typeOptions(types);
                            return $.getJSON("/transform-definition/methods/" + ko.unwrap(field().Location) + "/" + ko.unwrap(field().TypeName));
                        }).then(function (methods) {
                            methodOptions(methods);
                        });
            },
            attached = function () {
                field().Location.subscribe(function (dll) {
                    $.getJSON("/transform-definition/types/" + dll)
                        .done(function (types) {
                            typeOptions(types);
                        });
                });
                field().TypeName.subscribe(function (type) {
                    $.getJSON("/transform-definition/methods/" + ko.unwrap(field().Location) + "/" + type)
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
