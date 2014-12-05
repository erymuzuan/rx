/// <reference path="../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../Scripts/require.js" />
/// <reference path="../Scripts/underscore.js" />
/// <reference path="../Scripts/trigger.workflow.g.js" />
/// <reference path="../services/datacontext.js" />



define(['services/datacontext', 'services/logger', 'plugins/dialog'],
    function (context, logger, dialog) {

        var activity = ko.observable(new bespoke.sph.domain.MappingActivity()),
            definitionOptions = ko.observableArray(),
            destinationOptions = ko.observableArray(),
            wd = ko.observable(),
            activate = function () {
                destinationOptions(wd().VariableDefinitionCollection());
                var query = "Id ne '0'";
                var tcs = new $.Deferred();

                if (activity().MappingSourceCollection().length === 0) {
                    activity().MappingSourceCollection().push(new new bespoke.sph.domain.MappingSource());
                }

                context.loadAsync("TransformDefinition", query)
                    .then(function (lo) {
                        var maps = _(lo.itemCollection).map(function (v) {
                            return {
                                Name: ko.unwrap(v.Name),
                                FullName: ko.unwrap(v.CodeNamespace) + '.' + ko.unwrap(v.Name),
                                Id: v.Id
                            };
                        });
                        definitionOptions(maps);
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

        var vm = {
            activate: activate,
            activity:activity,
            okClick: okClick,
            cancelClick: cancelClick,
            definitionOptions: definitionOptions,
            destinationOptions: destinationOptions,
            wd: wd
        };


        return vm;

    });
