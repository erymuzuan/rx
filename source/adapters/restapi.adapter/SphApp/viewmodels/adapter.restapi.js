/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />

/*globals console, define, objectbuilders, bespoke, String*/
/**
 * @param{{app:string, system:string}}objectbuilders
 * @param{{KeyColumn:function, ValueColumn:function}}LookupColumn
 * @param{{navigate:function}} router
 * @param{{showMessage:function, showDialog:function}} app
 * @param{{OldValue:function,NewValue:function}}Change
 * @param{{databases:functions}} options
 * @param{{responseJSON:object}} response
 * @param{{FileName:string, Line:number}}error
 */

define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.app, "ko/adapter.restapi.ko.binding", "schemas/adapter.restapi.operation"],
    function (context, logger, router, app) {
        let originalEntity = "";
        const isBusy = ko.observable(false),
            adapter = ko.observable(),
            selected = ko.observable(),
            errors = ko.observableArray(),
            validations = ko.observableArray(),
            changes = ko.observableArray(),
            activate = function (id) {
                errors.removeAll();
                changes.removeAll();

                const query = String.format("Id eq '{0}'", id);
                return context.loadOneAsync("Adapter", query)
                    .then(function (result) {
                        adapter(result);
                    });
            },
            attached = function (view) {

            },
            canDeactivate = function () {
                var tcs = new $.Deferred();
                if (!originalEntity) {
                    return true;
                }


                if (originalEntity !== ko.toJSON(adapter)) {
                    app.showMessage("Save change to the item", "Rx Developer", ["Yes", "No", "Cancel"])
                        .done(function (dialogResult) {
                            if (dialogResult === "Yes") {
                                save().done(function () {
                                    tcs.resolve(true);
                                });
                            }
                            if (dialogResult === "No") {
                                tcs.resolve(true);
                            }
                            if (dialogResult === "Cancel") {
                                tcs.resolve(false);
                            }

                        });
                } else {
                    return true;
                }
                return tcs.promise();
            },
            viewFile = function (e) {
                const file = e.FileName || e,
                    line = e.Line || 1;
                const params = [
                        `height=${screen.height}`,
                        `width=${screen.width}`,
                        "toolbar=0",
                        "location=0",
                        "fullscreen=yes"
                ].join(","),
                    editor = window.open(`/sph/editor/file?id=${file.replace(/\\/g, "/")}&line=${line}`, "_blank", params);
                editor.moveTo(0, 0);
            },
            removeAdapter = function () {
                var tcs = new $.Deferred();
                app.showMessage("Are you sure you want to permanently remove this adapter", "Reactive Developer", ["Yes", "No"])
                    .done(function (dialogResult) {
                        if (dialogResult === "Yes") {
                            context.send(ko.mapping.toJSON(adapter), "adapter", "DELETE")
                                .done(function () {
                                    router.navigate("#dev.home");
                                    tcs.resolve(dialogResult);
                                });
                        } else {
                            tcs.resolve(dialogResult);
                        }

                    });

                return tcs.promise();
            },
            publishAsync = function () {

                const data = ko.mapping.toJSON(adapter);
                isBusy(true);

                return context.post(data, "/restapi-adapter/publish")
                    .then(function (result) {
                        isBusy(false);
                        if (result.success) {
                            logger.info(result.message);
                            errors.removeAll();
                        } else {
                            logger.error(result.message);
                            errors(result.errors);
                        }
                    });


            },
            addOperation = function () {
                require(["viewmodels/adapter.restapi.add.operation.dialog", "durandal/app"],
                    function (dialog, app2) {
                        dialog.adapter(adapter());
                        app2.showDialog(dialog)
                            .done(function (result) {
                                if (!result) return;
                                if (result === "OK") {
                                    const endpoints = adapter().OperationDefinitionCollection;
                                    for (let v of dialog.selectedOptions()) {
                                        const exist = endpoints().find(x => ko.unwrap(x.WebId) === ko.unwrap(v.WebId));
                                        if (!exist) {
                                            endpoints.push(v);
                                        }
                                    }
                                }
                            });
                    });
            },
            removeOperation = function (endpoint) {
                adapter().OperationDefinitionCollection.remove(endpoint);
            },
            saveAsync = function () {
                const data = ko.mapping.toJSON(adapter);
                isBusy(true);

                return context.put(data, "/adapter/" + ko.unwrap(adapter().Id))
                    .then(function (result) {
                        isBusy(false);
                        if (result.success) {
                            validations.removeAll();
                            logger.info("Your REST Api adapter has been saved");

                        } else {
                            validations(result.errors);
                            logger.error("Please check for any errors in your adapter");
                        }

                        originalEntity = ko.toJSON(adapter);

                    });
            };

        const vm = {
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            errors: errors,
            validations: validations,
            changes: changes,
            addOperation: addOperation,
            removeOperation: removeOperation,
            adapter: adapter,
            canDeactivate: canDeactivate,
            viewFile: viewFile,
            selected: selected,
            toolbar: {
                saveCommand: saveAsync,
                removeCommand: removeAdapter,
                commands: ko.observableArray([
                    {
                        caption: "Publish",
                        icon: "fa fa-sign-in",
                        command: publishAsync,
                        tooltip: "Compile the adapter",
                        enable: ko.computed(function () {
                            if (!ko.unwrap(adapter)) return false;
                            return adapter().OperationDefinitionCollection().length > 0;
                        })
                    }
                ])

            }
        };

        return vm;

    });
