/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/dialog'],
    function (context, logger, dialog) {

        var self = this,
            selectedAssemblies = ko.observableArray(),
            assemblies = ko.observableArray(),
            activate = function () {
                var tcs = new $.Deferred();
                $.get('/Sph/WorkflowDefinition/GetLoadedAssemblies').done(assemblies).done(tcs.resolve);

                return tcs.promise();
            },
            attached = function (view) {
                $(view).on('click', 'input[type=checkbox]', function () {
                    var dll = ko.dataFor(this);
                    if ($(this).is(':checked')) {
                        selectedAssemblies.push(dll);
                    } else {
                        selectedAssemblies.remove(dll);
                    }
                });
            },
            okClick = function (data, ev) {
                dialog.close(this, "OK");

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            selectAssembly = function (dll) {
                vm.assembly(dll);
                dialog.close(self, "OK");
            };

        var vm = {
            selectedAssemblies: selectedAssemblies,
            attached: attached,
            assemblies: assemblies,
            selectAssembly: selectAssembly,
            activate: activate,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
