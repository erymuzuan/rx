/// <reference path="/Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="/Scripts/knockout-3.4.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="/Scripts/require.js" />
/// <reference path="/Scripts/underscore.js" />
/// <reference path="/Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.app, "services/new-item"],
    function (context, logger, router, app, addItemService) {

        var isBusy = ko.observable(false),
            mappings = ko.observableArray(),
            activate = function () {
                return true;
            },
            attached = function () {

            },
            remove = function (p) {
                var tcs = new $.Deferred();
                app.showMessage("Are you sure you want to remove " + p.Name() + ", this action cannot be undone", "Rx Developer", ["Yes", "No"])
                    .done(function (dialogResult) {
                        if (dialogResult === "Yes") {
                            context.send(null, "/adapter/" + p.Id(), "DELETE")
                                .done(function () {
                                    tcs.resolve();
                                    mappings.remove(p);
                                    logger.info(p.Name() + " has been successfully removed");
                                });
                        }
                    });

                return tcs.promise();
            };

        var vm = {
            remove: remove,
            mappings: mappings,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            toolbar: {
                addNewCommand: function () {
                    return addItemService.addTransformDefinitionAsync();
                }
            }
        };

        return vm;

    });
