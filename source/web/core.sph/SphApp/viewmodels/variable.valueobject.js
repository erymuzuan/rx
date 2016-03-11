/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../../../core.sph/Scripts/require.js" />
/// <reference path="../../../core.sph/Scripts/__core.js" />
/// <reference path="../../../core.sph/SphApp/schemas/trigger.workflow.g.js" />
/// <reference path="../../../core.sph/SphApp/objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />



define(["plugins/dialog", objectbuilders.datacontext],
    function(dialog, context) {

        var valueObjectDefinitionOptions = ko.observableArray(),
            activate = function() {
                return context.getListAsync("ValueObjectDefinition", null, "Name")
                    .done(valueObjectDefinitionOptions);
            },
            attached = function() {

                setTimeout(function () {
                    $("#variable-name").focus();
                }, 500);
            },
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }
            },
            cancelClick = function() {
                dialog.close(this, "Cancel");
            };

        var vm = {
            valueObjectDefinitionOptions: valueObjectDefinitionOptions,
            activate : activate,
            attached : attached,
            variable: ko.observable(new bespoke.sph.domain.ValueObjectVariable()),
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
