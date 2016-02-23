/// <reference path="../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../Scripts/require.js" />
/// <reference path="../Scripts/underscore.js" />
/// <reference path="../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="~/SphApp/schemas/sph.domain.g.js" />
/// <reference path="~/SphApp/objectbuilders.js" />

define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.app,"services/new-item"],
    function (context, logger, router, app,addItemService) {

        var adapterOptions = ko.observableArray(),
            getAdapterType = function (adapter) {

                return /^.*?,(.*?).adapter/.exec(ko.unwrap(adapter.$type))[1].trim();
            },
            isBusy = ko.observable(false),
            adapters = ko.observableArray(),
            activate = function () {

                var tcs = new $.Deferred();
                $.get("adapter/installed-adapters", function (d) {
                    adapterOptions(d);
                    tcs.resolve(true);
                });
                return tcs.promise();
            },
            attached = function (view) {

            },
            getDesigner = function ($type) {
                var item = _(adapterOptions()).find(function (v) {
                    return ko.unwrap(v.adapter.$type) === ko.unwrap($type);
                });
                if (item.designer) {
                    return item.designer;
                }
                return {};
            },
            remove = function (p) {
                var tcs = new $.Deferred();
                app.showMessage("Are you sure you want to remove " + p.Name() + ", this action cannot be undone", "Rx Developer", ["Yes", "No"])
                    .done(function (dialogResult) {
                        if (dialogResult === "Yes") {
                            context.send(null, "/adapter/" + p.Id(), "DELETE")
                                .done(function () {
                                    tcs.resolve();
                                    adapters.remove(p);
                                    logger.info(p.Name() + " has been successfully removed");
                                });
                        }
                    });

                return tcs.promise();
            };

        var vm = {
            remove: remove,
            adapterOptions: adapterOptions,
            getAdapterType: getAdapterType,
            getDesigner: getDesigner,
            adapters: adapters,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            toolbar : {
                 addNewCommand: function() {
                return addItemService.addAdapterAsync();
            }
            }
        };

        return vm;

    });
