/// <reference path="../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../Scripts/require.js" />
/// <reference path="../Scripts/underscore.js" />
/// <reference path="../Scripts/trigger.workflow.g.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/dialog', objectbuilders.system],
    function (context, logger, dialog, system) {

        var activity = ko.observable(),
            wd = ko.observable(),
            isBusy = ko.observable(false),
            variableOptions = ko.observableArray(),
            activate = function () {
                var variables = _(wd().VariableDefinitionCollection()).map(function (v) {
                    return {
                        Name: v.Name,
                        DisplayName: ko.unwrap(v.Name) + "(" + ko.unwrap(v.TypeName) + ")"

                    };
                });
                variableOptions(variables);
            },
            attached = function (view) {
                $(view).on('change', '#following-sets input[type=checkbox]', function () {
                    var cb = $(this),
                        cs = cb.val(),
                        checked = cb.is(':checked');
                    console.log(cs + " is " + checked);
                    if (checked) {
                        // get the correlation property for the set
                        var set = _(wd().CorrelationSetCollection()).find(function (v) { return v.Name() === cs; }),
                            type = _(wd().CorrelationTypeCollection()).find(function (v) { return v.Name() === set.Type(); });
                        console.log(type);

                        _(type.CorrelationPropertyCollection()).each(function (v) {
                            var p = new bespoke.sph.domain.CorrelationProperty({
                                WebId: system.guid(),
                                Name: cs,
                                Origin: v.Path()

                            });
                            activity().CorrelationPropertyCollection.push(p);
                        });

                        return;
                    } else {
                        activity().CorrelationPropertyCollection.remove(function (v) {
                            return v.Name() === cs;
                        });
                    }
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
            variableOptions: variableOptions,
            activity: activity,
            wd: wd,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            okClick: okClick,
            cancelClick: cancelClick
        };

        return vm;

    });
