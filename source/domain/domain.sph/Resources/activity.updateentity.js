/// <reference path="../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../Scripts/require.js" />
/// <reference path="../Scripts/underscore.js" />
/// <reference path="../Scripts/trigger.workflow.g.js" />
/// <reference path="../Scripts/objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(["plugins/dialog", objectbuilders.datacontext, objectbuilders.config, objectbuilders.system],
    function (dialog, context, config, system) {

        var wd =ko.observable(new bespoke.sph.domain.WorkflowDefinition()),
            activity = ko.observable(new bespoke.sph.domain.CreateEntityActivity(system.guid())),
            entityOptions = ko.observableArray(),
            activate = function () {
              return  context.getTuplesAsync("EntityDefinition", null, "Id", "Name")
                    .then(entityOptions);
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
            entityOptions: entityOptions,
            activate: activate,
            activity:activity ,
            wd: wd,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
