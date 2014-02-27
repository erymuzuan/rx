/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />

define(['plugins/dialog', objectbuilders.datacontext, objectbuilders.config],
    function (dialog, context, config) {

        var entityOptions = ko.observableArray(),
            activate = function () {
                var query = String.format("IsPublished eq 1"),
                    tcs = new $.Deferred();

                context.loadAsync("EntityDefinition", query)
                    .then(function (lo) {
                        var types = _(lo.itemCollection).map(function (v) {
                            return {
                                name: v.Name(),
                                fullName: String.format("Bespoke.{0}_{1}.Domain.{2}, {0}.{2}", config.applicationName, v.EntityDefinitionId(), v.Name())
                            };
                        });
                        entityOptions(types);
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
            entityOptions: entityOptions,
            activate: activate,
            variable: ko.observable(new bespoke.sph.domain.ClrTypeVariable()),
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
