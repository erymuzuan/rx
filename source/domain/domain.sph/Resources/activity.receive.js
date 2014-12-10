/// <reference path="../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../Scripts/require.js" />
/// <reference path="../Scripts/underscore.js" />
/// <reference path="../Scripts/trigger.workflow.g.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/dialog'],
    function (context, logger, dialog) {

        var activity = ko.observable(),
            wd = ko.observable(),
            isBusy = ko.observable(false),
            variableOptions = ko.observableArray(),
            activate = function () {
                var variables = _(wd().VariableDefinitionCollection()).map(function(v) {
                    return {
                        Name: v.Name,
                        DisplayName: ko.unwrap(v.Name) + "("+ ko.unwrap(v.TypeName) + ")"

                    };
                });
                variableOptions(variables);
            },
            attached = function(view) {

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
            variableOptions: variableOptions,
            activity: activity,
            wd : wd,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            okClick: okClick,
            cancelClick: cancelClick
        };

        return vm;

    });
