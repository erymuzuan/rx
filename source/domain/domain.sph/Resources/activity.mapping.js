/// <reference path="../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../Scripts/require.js" />
/// <reference path="../Scripts/underscore.js" />
/// <reference path="../Scripts/trigger.workflow.g.js" />
/// <reference path="../services/datacontext.js" />



define(["services/datacontext", "services/logger", "plugins/dialog",objectbuilders.config],
    function (context, logger, dialog, config) {

        var activity = ko.observable(new bespoke.sph.domain.MappingActivity()),
            definitionOptions = ko.observableArray(),
            destinationOptions = ko.observableArray(),
            wd = ko.observable(),
            activate = function () {
                destinationOptions(wd().VariableDefinitionCollection());
                var query = "Id ne '0'",
                    tcs = new $.Deferred();

                if (activity().MappingSourceCollection().length === 0) {
                    activity().MappingSourceCollection([new bespoke.sph.domain.MappingSource()]);
                }

                context.getTuplesAsync("TransformDefinition", query, "Id", "Name")
                    .then(function (lo) {
                        var maps = _(lo).map(function (v) {
                            return {
                                Name: ko.unwrap(v.Name),
                                FullName: config.applicationName + ".Integrations.Transforms." + ko.unwrap(v.Name),
                                Id: v.Id
                            };
                        });
                        definitionOptions(maps);
                        tcs.resolve(true);
                    });
                return tcs.promise();

            },
            attached = function () {
                activity().MappingDefinition.subscribe(function (map) {
                    var md = _(definitionOptions()).find(function (v) {
                        return v.FullName === map;
                    });
                    if (!md) {
                        return;
                    }
                    context.loadOneAsync("TransformDefinition", "Id eq '" + ko.unwrap(md.Id) + "'")
                        .done(function (td) {
                            if (td.InputTypeName()) {
                                activity().MappingSourceCollection([new bespoke.sph.domain.MappingSource()]);
                            } else {
                                var args = _(td.InputCollection()).map(function (v) {
                                    return bespoke.sph.domain.MappingSource({ WebId: ko.unwrap(v.Name) });
                                });
                                activity().MappingSourceCollection(args);
                            }
                        });
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
            activity: activity,
            okClick: okClick,
            cancelClick: cancelClick,
            definitionOptions: definitionOptions,
            destinationOptions: destinationOptions,
            wd: wd
        };


        return vm;

    });
