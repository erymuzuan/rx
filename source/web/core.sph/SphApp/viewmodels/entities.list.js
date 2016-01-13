/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/__domain.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(["services/datacontext", "services/logger", "services/new-item"],
    function (context, logger, addItemService) {

        var entities = ko.observableArray(),
            isBusy = ko.observable(false),
            activate = function () {

            },
            attached = function (view) {


            },
            uploadPackage = function () {

                var tcs = new $.Deferred();
                require(["viewmodels/entity.import.dialog", "durandal/app"], function (dialog, app2) {
                    app2.showDialog(dialog)
                        .done(function () {
                            var ent = dialog.entity();
                            if (ent) {
                                entities.push(ent);
                            }
                            tcs.resolve();
                        });
                });

                return tcs.promise();


            },
            exportPackage = function (entity) {
                var tcs = new $.Deferred();
                require(["viewmodels/entity.export.dialog", "durandal/app"], function (dialog, app2) {
                    dialog.entity(entity);

                    app2.showDialog(dialog)
                        .done(tcs.resolve);
                });

                return tcs.promise();

            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            entities: entities,
            exportPackage: exportPackage,
            toolbar: {
                importCommand: uploadPackage,
                addNewCommand: function () {
                    return addItemService.addEntityDefinitionAsync();
                }
            }
        };

        return vm;

    });
