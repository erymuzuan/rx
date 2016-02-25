/// <reference path="../../../web/core.sph/scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../../web/core.sph/scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../../web/core.sph/SphApp/objectbuilders.js" />
/// <reference path="../../../web/core.sph/SphApp/schemas/trigger.workflow.g.js" />
/// <reference path="../../../web/core.sph/Scripts/__core.js" />
/// <reference path="../../../web/core.sph/Scripts/require.js" />


define(["plugins/dialog", objectbuilders.datacontext],
    function (dialog, context) {

        var entities = ko.observableArray(),
            activate = function () {
                var tcs = new $.Deferred();
                context.getListAsync("EntityDefinition", null, "Name")
                    .done(function (list) {
                        entities(list);
                        tcs.resolve(true);
                    });

                return tcs.promise();
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
            entities: entities,
            activate: activate,
            activity: ko.observable(new bespoke.sph.domain.CreateEntityActivity()),
            wd: ko.observable(new bespoke.sph.domain.WorkflowDefinition()),
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
