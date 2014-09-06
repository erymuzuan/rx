/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/dialog'],
    function (context, logger, dialog) {

        var assemblyOptions = ko.observableArray(),
            inputTypeOptions = ko.observableArray(),
            selectedInputAssembly = ko.observable(),
            outputTypeOptions = ko.observableArray(),
            selectedOutputAssembly = ko.observable(),
            activate = function() {
                var tcs = new $.Deferred();
                $.get("/sph/transformdefinition/assemblies", function(list) {
                    assemblyOptions(list);
                    tcs.resolve(true);
                });

                return tcs.promise();
            },
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        selectedInputAssembly.subscribe(function(dll) {
            $.get("/sph/transformdefinition/gettypes?dll=" + dll, function(list) {
                inputTypeOptions(list);
            });
        });
        selectedOutputAssembly.subscribe(function (dll) {
            $.get("/sph/transformdefinition/gettypes?dll=" + dll, function(list) {
                outputTypeOptions(list);
            });
        });

        var vm = {
            activate: activate,
            assemblyOptions: assemblyOptions,
            inputTypeOptions: inputTypeOptions,
            selectedInputAssembly: selectedInputAssembly,
            outputTypeOptions: outputTypeOptions,
            selectedOutputAssembly: selectedOutputAssembly,
            td: ko.observable(new bespoke.sph.domain.TransformDefinition()),
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
