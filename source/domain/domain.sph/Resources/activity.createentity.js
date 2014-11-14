/// <reference path="../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../Scripts/require.js" />
/// <reference path="../Scripts/underscore.js" />
/// <reference path="../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['plugins/dialog', objectbuilders.datacontext],
    function (dialog, context) {

        var entities = ko.observableArray(),
            activate = function () {
                var tcs = new $.Deferred();
                context.getListAsync("EntityDefinition", "Id ne '0'", "Name")
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
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            attached = function() {
            };

        var vm = {
            entities: entities,
            attached: attached,
            activate: activate,
            activity: ko.observable(new bespoke.sph.domain.CreateEntityActivity()),
            wd: ko.observable(new bespoke.sph.domain.WorkflowDefinition()),
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
